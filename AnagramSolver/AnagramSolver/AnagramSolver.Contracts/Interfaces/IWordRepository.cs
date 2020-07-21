using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IWordRepository
    {
        Dictionary<string, List<Anagram>> ReadDataFromFile();
        void ReadSettingsFile(string filePath = "../../../appsettings.json");
    }
}
