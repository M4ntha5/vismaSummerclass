using System.ComponentModel.DataAnnotations;

namespace AnagramSolver.Contracts.Models
{
    public class Anagram
    {
        public int ID { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Word { get; set; }

    }
}
