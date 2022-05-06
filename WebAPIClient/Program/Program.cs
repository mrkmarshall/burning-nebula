using System;
using UtilityLibraries;
using System.Threading.Tasks;

class Program
{
        static async Task Main(string[] args)
    {
        
        var repositories = await StringLibrary.ProcessRepositories();
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