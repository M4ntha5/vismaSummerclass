using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnagramSolver.Contracts.Entities
{
    public class WordEntity
    {
        public int ID { get; set; }
        public string Word { get; set; }
        public string Category { get; set; }
        public string SortedWord { get; set; }
    }
}
