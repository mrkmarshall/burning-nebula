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
        private static string URLpath;
        private static string username = "skulltar_123456789";
        private static string accessKeyURL = $"{baseURL}users/{username}/claim";

        static async Task Main()
        {
            string statusURL = "game/status";
            URLpath = statusURL;
            await GetAPIStatus(URLpath);
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