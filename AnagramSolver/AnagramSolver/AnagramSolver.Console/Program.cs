using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console.UI;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AnagramSolver.Console
{
    class Program
    {
        static readonly UserInterface UserInterface = new UserInterface();
        static readonly ApiActions apiActions = new ApiActions();

        static async Task Main(string[] args)
        {
            //loading data from settings file
            Configuration.ReadAppSettingsFile();
          
            var AnagramSolver = new BusinessLogic.Services.AnagramSolver(
                new FileRepository(), new UserInterface(), new CachedWordRepositoryDB());
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
                    result = (List<string>) await AnagramSolver.GetAnagrams(userInput);             
                else
                    result = await apiActions.CallAnagramSolverApi(userInput);


                UserInterface.DisplayResults(result);
            }
        }

        private static async Task SeedDBWithFileData()
        {
            System.Console.WriteLine("Seeding database, please wait");
            var fileRepo = new FileRepository();
            var connection = new WordRepositoryDB();
            var wordsList = await fileRepo.GetAllWords();

            foreach(var word in wordsList)
            {
                var model = new Anagram()
                {
                    Case = word.Category,
                    Word = word.Word,                  
                };

                await connection.AddNewWord(model);
            }
        }


    }
}
