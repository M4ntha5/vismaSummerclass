using AnagramSolver.Contracts.DatabaseModels;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class FileRepository : IWordRepository
    {
        private readonly Dictionary<string, List<Anagram>> AllData;
        private readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + Settings.DataFileName));


        public FileRepository()
        {         
            AllData = new Dictionary<string, List<Anagram>>();
            ReadDataFromFile();
        }

        public Dictionary<string, List<Anagram>> GetData()
        {
            return AllData;
        }
        public List<Anagram> GetWords()
        {
            var words = AllData.Values.ToList().SelectMany(x => x).ToList();
            return words;
        }

        private void ReadDataFromFile()
        {
            if (!File.Exists(FilePath))
                throw new Exception($"File '{FilePath}' does not exist!");

            string[] lines = File.ReadAllLines(FilePath);

            //file reading using resources
            //string[] lines = File.ReadAllLines(Resources.zodynas);

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

                if (AllData.ContainsKey(sortedWord))
                {
                    AllData[sortedWord].Add(new Anagram
                    {
                        Word = word,
                        Case = wordCase
                    });
                }
                else
                {
                    AllData.Add(
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
        }

        public List<Anagram> GetSelectedWordAnagrams(string key)
        {
            var sortedWord = String.Concat(key.OrderBy(x => x));
            if (AllData.ContainsKey(sortedWord))
                return AllData[sortedWord];
            else
                return null;
        }

        public void AddWordToFile(Anagram anagram)
        {
            if(!File.Exists(FilePath))
                throw new Exception($"File '{FilePath}' does not exist!");

            var sortedInputWord = String.Concat(anagram.Word.OrderBy(x => x));
            if (AllData.ContainsKey(sortedInputWord))
            {
                var anagramWords = AllData[sortedInputWord];

                foreach(var item in anagramWords)
                    if (item.Word == anagram.Word)
                        throw new Exception($"Word {anagram.Word} already exists");                 
            }

            string appendText = anagram.Word + '\t' + anagram.Case + '\t' + "" + '\t' + "" + '\n'; 
            File.AppendAllText(FilePath, appendText);
        }

        public string GetDataFilePath()
        {
            return FilePath;
        }
    }
}
