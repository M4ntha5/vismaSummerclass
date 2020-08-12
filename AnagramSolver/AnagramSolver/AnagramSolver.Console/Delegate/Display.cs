using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.Console.Delegate
{
    public delegate void Print(string message);

    public class Display : IDisplay
    {
        private readonly Print _print;

        public Display(Print print)
        {
            _print = print;
        }

        public string GetInput()
        {
            _print("Enter word or phrase (enter x to exit):");
            var userInput = System.Console.ReadLine().ToLower();
            if (userInput == "x")
                return null;

            return userInput;
        }

        public void DisplayResults(List<string> angarams)
        {
            if (angarams != null)
            {
                _print("Anagrams found:");
                angarams.ForEach(x => _print(x));
            }
            else
                _print("No anagrams found for yuor input!");
        }

        public int DisplayOptions()
        {
            _print("If you want to seed database with file content press: 0");
            _print("If you want to solve your anagrams using API call press: 1");
            _print("If you want to solve your anagrams without using API press: 2");

            int.TryParse(System.Console.ReadLine(), out int userInput);

            if (userInput > 2 || userInput < 0)
                throw new Exception("Wrong input. You must enter 1 or 2");

            return userInput;
        }
    }
}
