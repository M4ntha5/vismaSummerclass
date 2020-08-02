using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class WordRepository : IWordRepository
    {
        public void AddWordToFile(Anagram anagram)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<Anagram>> GetData()
        {
            throw new NotImplementedException();
        }

        public string GetDataFilePath()
        {
            throw new NotImplementedException();
        }

        public List<Anagram> GetSelectedWordAnagrams(string key)
        {
            throw new NotImplementedException();
        }

        public List<Anagram> GetWords()
        {
            throw new NotImplementedException();
        }

        public void InsertFileContentToDB()
        {
            throw new NotImplementedException();
        }
    }
}
