using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.Console.UI
{
    public class UserInterface : IUserInterface
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
                angarams.ForEach(x => System.Console.WriteLine(x));
            }
            else
                System.Console.WriteLine("No anagrams found for yuor input!");
        }

        public string ValidateInputData(string userInput)
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
