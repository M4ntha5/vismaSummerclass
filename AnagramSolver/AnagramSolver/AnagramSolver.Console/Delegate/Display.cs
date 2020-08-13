using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.Console.Delegate
{
    public delegate void Print(string message);
    public delegate string FormattedPrint(string message);

    public class Display : IDisplay
    {
        private readonly Print _print;
        private readonly FormattedPrint _formattedPrint;

        //private readonly Action<string> _print;
        //private readonly Func<string, string> _formattedPrint;

        public Display(Print print)
                        //Action<string> print)
        {
            _print = print;  

            //action, func
            //_formattedPrint = CapitalizeFirstLetter;

            //delegate
            _formattedPrint = new FormattedPrint(CapitalizeFirstLetter);
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
                angarams.ForEach(x => FormattedPrint(_formattedPrint, x));
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

        //delegate
        public void FormattedPrint(FormattedPrint del, string input)
        {
            _print(del(input));
        }
        //action, func
        //public void FormattedPrint(Func<string, string> del, string input)
        //{
        //    _print(del(input));          
        //}

        public string CapitalizeFirstLetter(string input)
        {
            if (input == null)
                return null;

            if (input.Length > 1)
                return char.ToUpper(input[0]) + input.Substring(1);

            return input.ToUpper();
        }

    }
}
