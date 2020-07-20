using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IFileRepository
    {
        Dictionary<string, List<Anagram>> ReadDataFromFile(string filePath = @"zodynas.txt");
    }
}
