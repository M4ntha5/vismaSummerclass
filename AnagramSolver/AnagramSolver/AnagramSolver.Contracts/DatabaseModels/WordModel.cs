using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.DatabaseModels
{
    public class WordModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Word { get; set; }
        public string SortedWord { get; set; }
    }
}
