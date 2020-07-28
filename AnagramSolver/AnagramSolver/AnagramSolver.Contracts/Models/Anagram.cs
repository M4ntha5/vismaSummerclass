using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AnagramSolver.Contracts.Models
{
    public class Anagram
    {
        [Required]
        public string Case { get; set; }
        [Required]
        public string Word { get; set; }
    }
}
