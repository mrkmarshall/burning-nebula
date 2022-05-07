using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Collections.ObjectModel;

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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync(url);
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }
    }


}