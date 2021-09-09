using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BurningNebulaPlayground
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseURL = "https://api.spacetraders.io/game/";
        private static string URLpath = "status";

        static async Task Main(string[] args)
        {
            await CallAPI();
        }
        
        private static async Task CallAPI()
        {

            var stringTask = client.GetStringAsync($"{baseURL}{URLpath}");
            var msg = await stringTask;
            Console.Write(msg);
        }
    }
}