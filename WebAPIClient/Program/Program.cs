using System;
using UtilityLibraries;
using System.Threading.Tasks;
using System.Net.Http;

class Program
{

    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {

        var repositories = await GitHubRepositoryProcessor.ProcessRepositories(client);
        foreach (var repo in repositories)
        {
            Console.WriteLine(repo.Name);
            Console.WriteLine(repo.Description);
            Console.WriteLine(repo.GitHubHomeUrl);
            Console.WriteLine(repo.Homepage);
            Console.WriteLine(repo.Watchers);
            Console.WriteLine();
            Console.WriteLine(repo.LastPush);
        }
    }
}