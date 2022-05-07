using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace UtilityLibraries
{
    public class GitHubRepositoryProcessor : IRepositoryProcessor
    {

        private string url;
        private HttpClient client;

        public GitHubRepositoryProcessor(string urlParameter, HttpClient clientParameter)
        {
            url = urlParameter;
            client = clientParameter;
        }

        public async Task<List<Repository>> ProcessRepositories()
        {
            var streamTask = client.GetStreamAsync(url);
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }
    }


}