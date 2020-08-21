using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface ISearchHistoryService
    {
        Task<List<string>> GetSearchedAnagrams(string searchPhrase);
    }
}
