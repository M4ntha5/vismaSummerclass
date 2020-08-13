using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Console.Delegate
{
    public class DisplayWithEvents : IDisplay
    {
        public event Print PrintEvent;
        private readonly FormattedPrint _formattedPrint;

        public DisplayWithEvents()
        {
            _formattedPrint = CapitalizeFirstLetter;
        }

        public string GetInput()
        {
            PrintEvent("Enter word or phrase (enter x to exit):");
            var userInput = System.Console.ReadLine().ToLower();
            if (userInput == "x")
                return null;

            return userInput;
        }

        public void DisplayResults(List<string> angarams)
        {
            if (angarams != null)
            {
                PrintEvent("Anagrams found:");
                angarams.ForEach(x => FormattedPrint(_formattedPrint, x));
            }
            else
                PrintEvent("No anagrams found for yuor input!");
        }

        public int DisplayOptions()
        {
            PrintEvent("If you want to seed database with file content press: 0");
            PrintEvent("If you want to solve your anagrams using API call press: 1");
            PrintEvent("If you want to solve your anagrams without using API press: 2");

            int.TryParse(System.Console.ReadLine(), out int userInput);

            if (userInput > 2 || userInput < 0)
                throw new Exception("Wrong input. You must enter 1 or 2");

            return userInput;
        }

        //delegate
        public void FormattedPrint(FormattedPrint del, string input)
        {
            PrintEvent(del(input));
        }

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
