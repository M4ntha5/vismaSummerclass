using System;
using System.Collections.Generic;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAnagramSolver
    {
        IList<string> GetAnagrams(string myWords);
    }
}
