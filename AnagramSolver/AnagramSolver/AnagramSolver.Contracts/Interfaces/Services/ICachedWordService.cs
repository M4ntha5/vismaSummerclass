using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Services
{
    public interface ICachedWordService
    {
        Task AddCachedWord(string phrase, List<string> anagramsIds);
        Task<CachedWord> GetSelectedCachedWord(string phrase);
    }
}
