using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAnagramSolver
    {
        IList<string> GetAnagrams(string myWords);
    }
}
