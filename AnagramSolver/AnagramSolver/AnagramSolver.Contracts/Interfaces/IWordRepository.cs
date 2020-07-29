using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IWordRepository
    {
        Dictionary<string, List<Anagram>> GetData();
        List<Anagram> GetWords();
        List<Anagram> GetSelectedWordAnagrams(string key);
        void AddWordToFile(Anagram anagram);
        string GetDataFilePath();
    }
}
