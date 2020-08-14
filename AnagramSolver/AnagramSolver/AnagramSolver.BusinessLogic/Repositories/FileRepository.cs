using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class FileRepository : IWordRepository
    {
        private List<WordEntity> AllData;
        private readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + Settings.DataFileName));

        public FileRepository()
        {
            AllData = new List<WordEntity>();
            GetDataFromDB();
        }

        private void GetDataFromDB()
        {
            AllData = GetAllWords().Result;
        }

        public async Task<List<WordEntity>> GetAllWords()
        {
            //file reading using path

            if (!File.Exists(FilePath))
                throw new Exception($"File '{FilePath}' does not exist!");
            string[] lines = await File.ReadAllLinesAsync(FilePath);

            //file reading using resources
            //string[] lines = File.ReadAllLines(Resources.zodynas);
            string previousWord = string.Empty;
            var result = new List<WordEntity>();
            foreach (string line in lines)
            {
                var lineParts = line.Split('\t');

                string word = lineParts[0].ToLower().Trim();
                string wordCase = lineParts[1].ToLower().Trim();

                //if same line found skip to the next one
                if (word == previousWord)
                    continue;

                //sorting string chars alphabetical order
                var sortedWord = String.Concat(word.OrderBy(x => x));
                sortedWord = sortedWord.ToLower();

                var entity = new WordEntity
                {
                    Category = wordCase,
                    SortedWord = sortedWord,
                    Word = word
                };
                result.Add(entity);

                previousWord = word;
            }
            return result;
        }

        public async Task<List<WordEntity>> GetSelectedWordAnagrams(string word)
        {
            var sortedWord = String.Concat(word.OrderBy(x => x));
            var anagrams = AllData.Where(x => x.SortedWord == sortedWord).ToList();

            if (anagrams.Count == 0)
                return null;
            else
                return anagrams;
        }

        public Task AddNewWord(Anagram anagram)
        {
            if (!File.Exists(FilePath))
                throw new Exception($"File '{FilePath}' does not exist!");
            if (string.IsNullOrEmpty(anagram.Category) || string.IsNullOrEmpty(anagram.Word))
                throw new Exception("Cannot add Word, because Word is empty");

            var isPresent = AllData.Where(x => x.Word == anagram.Word).ToList();
            if (isPresent.Count > 0)
                throw new Exception($"Word {anagram.Word} already exists");

            string appendText = anagram.Word + '\t' + anagram.Category + '\t' + "" + '\t' + "" + '\n';
            return File.AppendAllTextAsync(FilePath, appendText);
        }
    }
}
