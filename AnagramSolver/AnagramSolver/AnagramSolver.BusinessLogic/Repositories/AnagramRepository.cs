using AnagramSolver.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class AnagramRepository : IAnagramSolver
    {
        public IFileRepository FileRepository { get; set; }

        public IList<string> GetAnagrams(string inputWords)
        {        
            var data = FileRepository.ReadDataFromFile();
            var sortedInput = String.Concat(inputWords.OrderBy(x => x));

            if (data.ContainsKey(sortedInput))
            {
                var results = new List<string>();

                data[sortedInput].ForEach(x => results.Add(x.Word));
                //removes user input as result
                results.Remove(inputWords);
                return results;
            }
            else
                return null;
        }
    }
}
