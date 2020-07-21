using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class FileRepository : IWordRepository
    {
        public Dictionary<string, List<Anagram>> ReadDataFromFile()
        {
            if (!File.Exists(@Settings.DataFileName))
                throw new Exception($"File '{Settings.DataFileName}' does not exist!");

            string[] lines = File.ReadAllLines(@Settings.DataFileName);

            var data = new Dictionary<string, List<Anagram>>();
            string previousWord = string.Empty;
            foreach (string line in lines)
            {
                var lineParts = line.Split('\t');
                //if same line found skip to the next one
                if (lineParts[0] == previousWord)
                    continue;

                string word = lineParts[0].ToLower();
                string wordCase = lineParts[1].ToLower();

                //sortingh string chars alphabetical order
                var sortedWord = String.Concat(word.OrderBy(x => x));
                sortedWord = sortedWord.ToLower();

                if (data.ContainsKey(sortedWord))
                {
                    data[sortedWord].Add(new Anagram
                    {
                        Word = word,
                        Case = wordCase
                    });
                }
                else
                {
                    data.Add(
                        sortedWord,
                        new List<Anagram>
                        {
                            new Anagram
                            {
                                Case = wordCase,
                                Word = word
                            }
                        });
                }
                previousWord = word;
            }
            return data;
        }

        public void ReadSettingsFile(string filePath = @"../../../appsettings.json")
        {
            if (!File.Exists(filePath))
                throw new Exception($"Settings file '{filePath}' does not exist!");

            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(
                Path.GetDirectoryName(Environment.CurrentDirectory), @"../../../AnagramSolver.Console"))
            .AddJsonFile("appsettings.json");


            int.TryParse(builder.Build().GetSection("AnagramsToGenerate").Value, out int anagramsCount);
            int.TryParse(builder.Build().GetSection("MinInputLength").Value, out int minLength);
            var dataFile = builder.Build().GetSection("DataFileName").Value;

            if (anagramsCount > 10 || anagramsCount < 1)
                throw new Exception("Generated anagrams count must be between 1 and 10");

            Settings.AnagramsToGenerate = anagramsCount;
            Settings.MinInputLength = minLength;
            Settings.DataFileName = dataFile;

        }
    }
}
