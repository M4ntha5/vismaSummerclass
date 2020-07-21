using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolver : IAnagramSolver
    {
        public IWordRepository FileRepository { get; set; }

        public IList<string> GetAnagrams(string inputWords)
        {        
            var data = FileRepository.ReadDataFromFile();
            var sortedInput = String.Concat(inputWords.OrderBy(x => x));
            
            if (data.ContainsKey(sortedInput))
            {
                var results = new List<string>();

                data[sortedInput].ForEach(x => results.Add(x.Word));
                //removes user input from results
                results.Remove(inputWords);
                return results;
            }
            else
                return null;
        }
    }
}
