using AnagramSolver.Contracts.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Console
{
    public  class ApiActions
    {
        private readonly HttpClient Client;

        public ApiActions()
        {
            Client = new HttpClient();
        }

        public async Task<List<string>> CallAnagramSolverApi(string word)
        {
            var defaultUrl = "https://localhost:44389/api/";
            var response = await Client.GetAsync(defaultUrl + "anagrams/" + word);

            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<string>>(resultString);
            return list;
        }

      /*  public async Task<List<string>> DownloadDataFile()
        {
            var defaultUrl = "https://localhost:44389/api/";
            var dataFilePath = _wordRepository.GetDataFilePath();
            var fileName = dataFilePath.Split('/');
            var file = fileName[fileName.Length - 1];

        }*/
    }
}
