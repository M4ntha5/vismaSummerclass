using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnagramSolver.Console
{
    class Program
    {
        static readonly IWordRepository FileRepository = new FileRepository();
        static readonly IAnagramSolver AnagramSolver = new BusinessLogic.Services.AnagramSolver()
        {
            FileRepository = FileRepository
        };

        static void Main(string[] args)
        {
            //loading data from settings file
            FileRepository.ReadSettingsFile();

            System.Console.WriteLine("Enter your word:");
            var userInput = System.Console.ReadLine().ToLower();

            var userWords = userInput.Split(' ').ToList();
            foreach(var word in userWords)
            {
                if (word.Length < Settings.MinInputLength)
                    throw new Exception($"Minimum one word length must be at least " +
                        $"{Settings.MinInputLength} characters!");
            }
            userInput = string.Join("", userWords);


            var result = (List<string>)AnagramSolver.GetAnagrams(userInput);

            if (result != null)
            {
                System.Console.WriteLine("Yuor anagrams:");
                //if must show less than found
                if (result.Count > Settings.AnagramsToGenerate)
                    for (int i = 0; i < Settings.AnagramsToGenerate; i++)
                        System.Console.WriteLine(result[i]);

                //if must show all or more than found
                else
                    result.ForEach(x => System.Console.WriteLine(x));
            }
            else
                System.Console.WriteLine("No anagrams found for yuor input!");


        }
    }
}
