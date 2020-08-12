using AnagramSolver.BusinessLogic.Properties;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Console.Delegate;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Threading.Tasks;

namespace AnagramSolver.Console
{
    class Program
    {
        static readonly Display _userInterface = new Display(print => WriteToConsole(print));

        static readonly ApiActions _apiActions = new ApiActions();
        static readonly IAnagramSolver _anagramSolver = new BusinessLogic.Services.AnagramSolver(
            new FileRepository(), new CachedWordRepositoryDB());

        static async Task Main(string[] args)
        {
            //loading data from settings file
            Configuration.ReadAppSettingsFile();

            var howToSolve = _userInterface.DisplayOptions();

            while (true)
            {
                if (howToSolve == 0)
                    await SeedDBWithFileData();

                //getting initial user input
                var userInput = _userInterface.GetInput();

                if (userInput == null)
                    break;

                List<string> result;
                if (howToSolve == 2)
                    result = (List<string>)await _anagramSolver.GetAnagrams(userInput);
                else
                    result = await _apiActions.CallAnagramSolverApi(userInput);

                _userInterface.DisplayResults(result);
            }
        }

        private static async Task SeedDBWithFileData()
        {
            var fileRepo = new FileRepository();
            var connection = new WordRepositoryDB();
            var wordsList = fileRepo.GetAllWords();

            foreach (var word in wordsList)
            {
                var model = new Anagram()
                {
                    Case = word.Category,
                    Word = word.Word,
                };

                connection.AddNewWord(model);
            }
        }

        public static void WriteToConsole(string message)
        {
            System.Console.WriteLine(message);
        }

        public static void WriteToDebug(string message)
        {
            Debug.Print(message);
        }

        public static void WriteToFile(string message)
        {
            File.AppendAllText(@"ConsoleLog.txt", message + '\n');
        }

        public static string CapitalizeFirstLetter(string input)
        {
            if (input == null)
                return null;

            if (input.Length > 1)
                return char.ToUpper(input[0]) + input.Substring(1);

            return input.ToUpper();
        }


    }
}
