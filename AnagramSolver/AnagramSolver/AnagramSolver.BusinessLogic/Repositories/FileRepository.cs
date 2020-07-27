using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class FileRepository : IWordRepository
    {
        public Dictionary<string, List<Anagram>> ReadDataFromFile()
        {
            var path = @"E:\Github\Visma\vismaSummerclass\AnagramSolver\AnagramSolver\AnagramSolver.WebApp\bin\Debug\netcoreapp3.1\" + Settings.DataFileName;
            if (!File.Exists(path))
                throw new Exception($"File '{path}' does not exist!");

            string[] lines = File.ReadAllLines(path);

            var data = new Dictionary<string, List<Anagram>>();
            string previousWord = string.Empty;
            foreach (string line in lines)
            {
                var lineParts = line.Split('\t');

                string word = lineParts[0].ToLower();
                string wordCase = lineParts[1].ToLower();

                //if same line found skip to the next one
                if (word == previousWord)
                    continue;

                //sorting string chars alphabetical order
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

        public List<Anagram> GetSelectedWordAnagrams(string key)
        {
            var allData = ReadDataFromFile();

            if (allData.ContainsKey(key))
                return allData[key];
            else
                return null;
        }
    }
}
