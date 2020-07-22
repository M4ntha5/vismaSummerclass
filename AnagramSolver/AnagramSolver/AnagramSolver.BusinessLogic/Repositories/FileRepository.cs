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
            if (!File.Exists(@Settings.DataFileName))
                throw new Exception($"File '{Settings.DataFileName}' does not exist!");

            string[] lines = File.ReadAllLines(@Settings.DataFileName);

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
    }
}
