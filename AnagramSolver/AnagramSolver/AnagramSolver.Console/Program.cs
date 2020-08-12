using AnagramSolver.BusinessLogic.Properties;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console.Delegates;
using AnagramSolver.Console.UI;
using AnagramSolver.Contracts.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Resources;
using System.Threading.Tasks;

namespace AnagramSolver.Console
{
    class Program
    {
        static readonly UserInterface UserInterface = new UserInterface();
        static readonly ApiActions apiActions = new ApiActions();


        static async Task Main(string[] args)
        {
            var del = new Print(WriteToConsole);
            var display = new Display(del);

            del("test");



            //loading data from settings file
            Configuration.ReadAppSettingsFile();

            var AnagramSolver = new BusinessLogic.Services.AnagramSolver(
                new FileRepository(), new UserInterface(), new CachedWordRepositoryDB(), new WordRepositoryDB());
            var howToSolve = UserInterface.DisplayOptions();

            while (true)
            {
                if (howToSolve == 0)
                    await SeedDBWithFileData();

                //getting initial user input
                var userInput = UserInterface.GetInput();

                if (userInput == null)
                    break;

                List<string> result;
                if (howToSolve == 2)
                    result = (List<string>)await AnagramSolver.GetAnagrams(userInput);
                else
                    result = await apiActions.CallAnagramSolverApi(userInput);

                //displays results
                del("Anagrams found:");
                foreach (var anagram in result)
                    display.FormattedPrint(del, anagram);

               // UserInterface.DisplayResults(result);
            }
        }

        private static async Task SeedDBWithFileData()
        {
            System.Console.WriteLine("Seeding database, please wait");
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
            IResourceWriter writer = new ResourceWriter(Resources.zodynas);

            writer.AddResource("", message);
            writer.Close();
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
