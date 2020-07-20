using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.Console
{
    class Program
    {
        static IAnagramSolver AnagramRepository = new AnagramRepository()
        {
            FileRepository = new FileRepository()
        };

        static void Main(string[] args)
        {
          /*  var dic = new Dictionary<string, List<Anagram>>();

            dic.Add("antras", new List<Anagram> { new Anagram { Word = "zodis1", Case = "linksnis1" } });

            if(dic.ContainsKey("pirmas"))
            {
                dic["pirmas"].Add(new Anagram { Word = "zodis2", Case = "linksnis2" });
            }
            else
            {
                dic.Add("pirmas", new List<Anagram> { new Anagram { Word = "zodis2", Case = "linksnis2" } });
            }


            // dic["pirmas"].ForEach(x => System.Console.WriteLine(x.Word));

            */

            System.Console.WriteLine("Enter your word:");
            var userInput = System.Console.ReadLine();
    

            var result = (List<string>)AnagramRepository.GetAnagrams(userInput);

            if (result != null)
            {
                System.Console.WriteLine("Yuor anagrams:");
                result.ForEach(x => System.Console.WriteLine(x));
            }
            else
                System.Console.WriteLine("No anagrams for yuor input!");




        }
    }
}
