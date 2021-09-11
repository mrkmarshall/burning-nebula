using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BurningNebulaPlayground
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseURL = "https://api.spacetraders.io/";
        private static string URLpath;
        private static string username = "skulltar";
        private static string accessKeyURL = $"{baseURL}users/{username}/claim";

        static async Task Main()
        {
            string statusURL = "game/status";
            URLpath = statusURL;
            await CallAPI(URLpath);
            await GetAccessToken(accessKeyURL);
        }

        private static async Task CallAPI(string pathToCall)
        {
            var stringTask = client.GetStringAsync($"{baseURL}{pathToCall}");
            var currentStatus = await stringTask;
            Console.Write(currentStatus);
        }

        static async Task GetAccessToken(string pathToCall)
        {
            var myAccessKey = await client.PostAsJsonAsync(accessKeyURL, username); 
            // The above parameters are totally the wrong things to be passing in. But it's what got the 500 error. 
            //I think I need to pass in the client and url, but that gives me a different error.
            Console.Write(myAccessKey.ToString());
        }

    }
}