using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnagramSolver.EF.DatabaseFirst.Entities
{
    public class WordEntity
    {
        public int ID { get; set; }
        public string Word { get; set; }
        public string Category { get; set; }
        [Column("Sorted_word")]
        public string SortedWord { get; set; }
    }
}
