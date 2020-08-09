using AnagramSolver.Contracts.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAdditionalWordRepository
    {
        Task<string> SelectWordById(string id);
        Task<List<WordEntity>> SelectWordsBySearch(string phrase);
        Task ClearSelectedTable(List<string> tables);
    }
}
