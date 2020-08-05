using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnagramSolver.EF.DatabaseFirst.Entities
{
    public class UserLogEntity
    {
        public int ID { get; set; }
        public string Ip { get; set; }
        public string Phrase { get; set; }
        [Column("Search_time")]
        public TimeSpan SearchTime { get; set; }
    }
}
