using System;
using WebApi.Library;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApi.Program
{
    public class Program
    {

        private static readonly HttpClient client = new HttpClient();
        private static string gitHubUrl = new string("https://api.github.com/orgs/dotnet/repos");


        public async static Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json")
                );
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            IRepositoryProcessor processor = new GitHubRepositoryProcessor(gitHubUrl, client);
            await Program.ProcessRepository(processor);
        }

        public async static Task<int> ProcessRepository(IRepositoryProcessor processor)
        {
            var repositories = await processor.ProcessRepositories();

            int repositoryCount = 0;

            foreach (var repo in repositories)
            {
                Console.WriteLine(repo.Name);
                Console.WriteLine(repo.Description);
                Console.WriteLine(repo.GitHubHomeUrl);
                Console.WriteLine(repo.Homepage);
                Console.WriteLine(repo.Watchers);
                Console.WriteLine();
                Console.WriteLine(repo.LastPush);

                repositoryCount++;
            }

            return repositoryCount;
        }
    }
}