using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console.UI;
using AnagramSolver.Contracts.Interfaces;
using System.Collections.Generic;

namespace AnagramSolver.Console
{
    class Program
    {
        static readonly UserInterface UserInterface = new UserInterface();

        static void Main(string[] args)
        {
            var AnagramSolver = new BusinessLogic.Services.AnagramSolver(new FileRepository());
            //loading data from settings file
            Configuration.ReadAppSettingsFile();

            while (true)
            {
                //getting initial user input
                var userInput = UserInterface.GetInput();

                if (userInput == null)
                    break;

                var result = (List<string>)AnagramSolver.GetAnagrams(userInput);

                UserInterface.DisplayResults(result);
            }
        }      
    }
}
