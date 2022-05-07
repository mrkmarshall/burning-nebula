using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace UtilityLibraries
{
    public class GitHubRepositoryProcessor : IRepositoryProcessor
    {

        public string URL { get; private set; }
        private HttpClient Client;

        public GitHubRepositoryProcessor(string url, HttpClient client)
        {
            URL = url;
            Client = client;
        }

        public async Task<List<Repository>> ProcessRepositories()
        {
            var streamTask = Client.GetStreamAsync(URL);
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }
    }


}