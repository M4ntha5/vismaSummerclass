using System.ComponentModel.DataAnnotations;

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
