using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SpaceTradersAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseURL = "https://api.spacetraders.io/";
        private static string statusURL = "game/status";
        private static string username;
        private static string accessKeyURL;

        static async Task Main()
        {
            await GetAPIStatus(statusURL);
            Console.WriteLine("Enter username: ");
            username = Console.ReadLine();
            accessKeyURL = $"{baseURL}users/{username}/claim";
            await GetAccessToken(accessKeyURL);
        }

        private static async Task GetAPIStatus(string pathToCall)
        {
            var stringTask = client.GetStringAsync($"{baseURL}{pathToCall}");
            var currentStatus = await stringTask;
            Console.Write(currentStatus);
            Console.WriteLine();
        }

        static async Task GetAccessToken(string pathToCall)
        {
            HttpContent content = new StringContent(pathToCall);
            HttpResponseMessage myKeyResponseMessage = new HttpResponseMessage();
            myKeyResponseMessage = await client.PostAsync(pathToCall, content);
            Console.Write(myKeyResponseMessage);
            Console.WriteLine();
            Console.Write("Your access key is " + myKeyResponseMessage.Content.ReadAsStringAsync().Result);
        }

    }
}