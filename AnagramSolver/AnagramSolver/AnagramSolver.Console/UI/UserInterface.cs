using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.Console.UI
{
    public class UserInterface
    {
        public string GetInput()
        {
            System.Console.WriteLine("Enter word or phrase (enter x to exit):");
            var userInput = System.Console.ReadLine().ToLower();
            if (userInput == "x")
                return null;
            //validating user input
            userInput = ValidateInputData(userInput);
            return userInput;
        }

        public void DisplayResults(List<string> angarams)
        {
            if (angarams != null)
            {
                System.Console.WriteLine("Anagrams found:");
                //if must show less than found
               /* if (angarams.Count > Settings.AnagramsToGenerate)
                    for (int i = 0; i < Settings.AnagramsToGenerate; i++)
                        System.Console.WriteLine(angarams[i]);

                //if must show all or more than found
                else*/
                    angarams.ForEach(x => System.Console.WriteLine(x));
            }
            else
                System.Console.WriteLine("No anagrams found for yuor input!");
        }

        private string ValidateInputData(string userInput)
        {
            var userWords = userInput.Split(' ').ToList();
            foreach (var word in userWords)
            {
                if (word.Length < Settings.MinInputLength)
                    throw new Exception($"Minimum one word length must be at least " +
                        $"{Settings.MinInputLength} characters!");
            }         
            return string.Join("", userWords);
        }
    }
}
