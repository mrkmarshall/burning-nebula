using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text;
using System.Linq;

namespace SpaceTradersAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string tokenDefaultFilename = ".spaceTradersToken";
        private const string baseURL = "https://api.spacetraders.io/";
        
        private const string gameStatusPath = "game/status";
        private const string usernameClaimPath = "users/{0}/claim";
        private const string accountInfoPath = "my/account";

        public static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand // shows API status; loads default (or specified) user token; shows user account profile and Leaderboard status
            {
                new Option<string>(
                    new string[] {"--user", "-u"},
                    getDefaultValue: () => string.Empty,
                    description: "username / token-file to load")
            };
            rootCommand.Handler = CommandHandler.Create<string>(RootHandler);

            // TODO add subcommand for "claim" to call ClaimUsername
            
            // await GetAPIStatus();
            // Console.WriteLine("Enter username: ");
            // username = Console.ReadLine();
            // await GetAccessToken(username);

            await rootCommand.InvokeAsync(args);
        }

        
        
        private static async Task RootHandler(string user)
        {
            // Show API status
            await ShowApiStatus();

            // Get user's token from file
            var token = await LoadTokenForUser(user);

            // Show account status summary
            await ShowAccountStatus(token);

            // TODO Show position on Leaderboard, if any
            // TODO Show "score to beat" to move up (or get on) Leaderboard
        }

        private static async Task ShowAccountStatus(string token)
        {
            var uri = System.IO.Path.Join(baseURL, accountInfoPath);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            
            SpaceTradersUserProfileResponse result = await client.GetFromJsonAsync<SpaceTradersUserProfileResponse>(uri);
            SpaceTradersUserProfile user = result.user;
          
            if (result == null)
            {
                await Console.Error.WriteLineAsync("Request returned a null response! This is surprising and unexpected. We were expecting at *least* an exception or something here, not just... nothing... no, we're not mad, just disappointed.");
            }
            else
            {
                await Console.Out.WriteLineAsync($"User info:");
                await Console.Out.WriteLineAsync($"{user}");
            }
        }

        private static async Task<string> LoadTokenForUser(string username)
        {
            // TODO extract method under "LoadTokenForUser" or smth
            // Handle token file for specified user (or default)
            FileInfo tokenFile = new FileInfo(string.Join("_", $"{tokenDefaultFilename}", username).TrimEnd('_'));
            Console.WriteLine($"Loading token file {tokenFile.FullName}");

            if (!tokenFile.Exists)
            {
                await Console.Error.WriteLineAsync($"Token file {tokenFile.FullName} does not exist, please create it e.g. 'dotnet run -- --user new [username]'");
            }
            else
            {
                username = string.IsNullOrWhiteSpace(username) ? "DEFAULT" : username; // TODO improve this so it... looks up username based on token from API? Stores a more-complex username / token value in file?
                var token = await File.ReadAllTextAsync(tokenFile.FullName);

                bool tokenValid = true; //TODO
                if (tokenValid)
                {
                    return token;
                }
                else
                {
                    await Console.Error.WriteLineAsync("Token file for user {username} is invalid; please specify a username with a valid token file, or repair token file {tokenFile.FullName}");
                }
            }
            
            // default result is null for any erroneous or invalid username / token-file situation
            return null;
        }

        private static async Task ShowApiStatus()
        {
            var uri = System.IO.Path.Join(baseURL, gameStatusPath);
            SpaceTradersApiStatusResponse response = await client.GetFromJsonAsync<SpaceTradersApiStatusResponse>(uri);
            
            // Unnecessary fancy-ness
            var status = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(response.status);
            status = status.Replace("online", "ONLINE", StringComparison.InvariantCultureIgnoreCase);
            
            Console.Write(status);
            Console.WriteLine();
        }

        private static async Task ClaimUsername(string username)
        {
            var uri = System.IO.Path.Join(baseURL, string.Format(usernameClaimPath, username));
            var response = (await client.PostAsync(uri, null));
            response.EnsureSuccessStatusCode();

            var claim = await response.Content.ReadFromJsonAsync<SpaceTradersClaimUsernameResponse>();
            
            Console.WriteLine();
            Console.Write("Your username is " + claim.user.username);
            Console.Write("Your access key is " + claim.token);
        }

        public class SpaceTradersApiStatusResponse
        {
            // "{"status":"spacetraders is currently online and available to play"}"
            public string status { get; set; }
        }

        public class SpaceTradersClaimUsernameResponse
        {
            public string token { get; set; }
            public SpaceTradersUserProfile user { get; set; }
        }

        public class SpaceTradersUserProfileResponse
        {
           public SpaceTradersUserProfile user { get; set; } = new();
        }

        public class SpaceTradersUserProfile
        {
            public string username { get; set; }
            public DateTime joinedAt { get; set; }
            public int credits { get; set; }
            public int shipCount { get; set; }
            public int structureCount { get; set; }
            
            public IEnumerable<string> ships { get; set; } = Enumerable.Empty<string>();
            public IEnumerable<string> loans { get; set; } = Enumerable.Empty<string>();
            public IEnumerable<string> structures { get; set; } = Enumerable.Empty<string>();

            public override string ToString()
            {
                var loanList = string.Join(", ", loans.Select( x => x.ToString()));
                var shipList = string.Join(", ", ships.Select( x => x.ToString()));
                var structureList = string.Join(", ", structures.Select( x => x.ToString()));
                
                StringBuilder sb = new StringBuilder();
                sb.AppendJoin(
                    System.Environment.NewLine
                    , $" - Username       : {username}"
                    , $" - Join Date      : {joinedAt}"
                    , $" - Credits        : {credits} "
                    , $" - Ships          : {shipCount} [{shipList}]"
                    , $" - Structures     : {structureCount} [{structureList}]"
                    , $" - Loans          : {loans.Count()} [{loanList}]"
                );
                return sb.ToString();
            }
        }
    }
}
