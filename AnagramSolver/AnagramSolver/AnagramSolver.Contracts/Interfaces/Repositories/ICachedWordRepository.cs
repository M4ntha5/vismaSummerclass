using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface ICachedWordRepository
    {
        Task InsertCachedWord(CachedWord cachedWord);
        Task<CachedWordEntity> GetCachedWord(string phrase);
    }
}
