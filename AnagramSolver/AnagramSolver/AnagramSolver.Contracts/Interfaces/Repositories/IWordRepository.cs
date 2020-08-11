using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IWordRepository
    {
        List<WordEntity> GetAllWords();
        List<WordEntity> GetSelectedWordAnagrams(string word);
        void AddNewWord(Anagram anagram);

    }
}
