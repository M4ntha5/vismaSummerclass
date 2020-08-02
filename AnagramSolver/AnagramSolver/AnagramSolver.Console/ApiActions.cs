using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Console
{
    public class ApiActions
    {
        private readonly HttpClient Client;
        private const string BaseApiURL = "https://localhost:44389/api/";

        public ApiActions()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(BaseApiURL)
            };
        }

        public async Task<List<string>> CallAnagramSolverApi(string word)
        {
            var response = await Client.GetAsync("anagrams/" + word);

            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<string>>(resultString);
            return list;
        }

    }
}
