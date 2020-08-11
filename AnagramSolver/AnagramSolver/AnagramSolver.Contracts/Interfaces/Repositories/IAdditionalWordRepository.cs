using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAdditionalWordRepository
    {
        Task<WordEntity> SelectWordById(int id);
        Task<List<WordEntity>> SelectWordsBySearch(string phrase);


        Task UpdateSelectedWord(int id, Anagram updatedWord);
        Task DeleteSelectedWord(int id);
    }
}
