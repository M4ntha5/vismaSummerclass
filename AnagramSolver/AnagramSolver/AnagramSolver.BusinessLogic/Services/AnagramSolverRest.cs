using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolverRest : IAnagramSolver
    {
        private readonly HttpClient _client;

        public AnagramSolverRest()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Settings.AnagramicaApiUrl)
            };
        }

        public async Task<IList<string>> GetAnagrams(string myWords)
        {
            if (string.IsNullOrEmpty(myWords))
                throw new Exception("Cannot get anagrams, because phrase not deifned");
                            
            var response = await _client.GetAsync("best/" + myWords);

            if (!response.IsSuccessStatusCode)
                throw new Exception("No anagrams found from your input");

            var resultString = await response.Content.ReadAsStringAsync();
            var resultJson = JObject.Parse(resultString);
            var bestDetails = resultJson["best"].ToString();
            var anagramsList = JsonConvert.DeserializeObject<List<string>>(bestDetails);

            return anagramsList;

        }
    }
}
