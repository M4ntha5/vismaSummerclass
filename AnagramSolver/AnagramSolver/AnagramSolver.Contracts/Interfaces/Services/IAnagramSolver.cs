using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAnagramSolver
    {
        Task<IList<string>> GetAnagrams(string myWords);
    }
}
