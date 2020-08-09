﻿using AnagramSolver.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Services
{
    public interface IWordService
    {
        Task<List<Anagram>> GetAllWords();
        Task<List<Anagram>> GetWordsBySearch(string phrase);
        Task InsertWord(Anagram anagram);
        Task<List<Anagram>> GetWordAnagrams(string word);
        Task<string> GetWordById(string id);
    }

}
