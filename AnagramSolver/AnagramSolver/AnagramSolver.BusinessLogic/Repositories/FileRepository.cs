using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class FileRepository : IFileRepository
    {
        public Dictionary<string, List<Anagram>> ReadDataFromFile(string filePath = @"zodynas.txt")
        {
            string[] lines = File.ReadAllLines(filePath);

            var data = new Dictionary<string, List<Anagram>>();
            string previousWord = string.Empty;
            foreach (string line in lines)
            {
                var lineParts = line.Split('\t');
                //if same line found skip to the next one
                if (lineParts[0] == previousWord)
                    continue;

                string word = lineParts[0];
                string wordCase = lineParts[1];

                //sortingh string chars alphabetical order
                var sortedWord = String.Concat(word.OrderBy(x => x));

                if(data.ContainsKey(sortedWord))
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
