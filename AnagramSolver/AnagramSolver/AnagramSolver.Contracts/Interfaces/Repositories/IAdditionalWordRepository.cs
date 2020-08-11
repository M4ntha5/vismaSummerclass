using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAdditionalWordRepository
    {
        WordEntity SelectWordById(int id);
        List<WordEntity> SelectWordsBySearch(string phrase);


        void UpdateSelectedWord(int id, Anagram updatedWord);
        void DeleteSelectedWord(int id);
    }
}
