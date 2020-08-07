using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IWordRepository
    {
        Task<List<WordEntity>> GetAllWords();
        Task<List<WordEntity>> GetSelectedWordAnagrams(string word);
        Task AddNewWord(Anagram anagram);

    }
}
