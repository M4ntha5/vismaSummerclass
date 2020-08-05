using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnagramSolver.EF.DatabaseFirst.Entities
{
    public class CachedWordEntity
    {
        public int ID { get; set; }
        public string Phrase { get; set; }
        [Column("Anagrams_ids")]
        public string AnagramsIds { get; set; }
    }
}
