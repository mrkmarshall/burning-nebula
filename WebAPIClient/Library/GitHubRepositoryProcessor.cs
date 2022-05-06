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

        public GitHubRepositoryProcessor(string url, HttpClient client){}
        public string url;
        public HttpClient client;
        public string GetString() { return url; }
        public HttpClient GetClient() { return client; }

        public static async Task<List<Repository>> ProcessRepositories(string url, HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync(url);
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }

        public ReadOnlyCollection<Repository> ProcessRepositories()
        {
            throw new NotImplementedException();
        }
    }


}