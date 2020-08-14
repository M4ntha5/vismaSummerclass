using AnagramSolver.BusinessLogic.Properties;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Console.Delegate;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Profiles;
using AutoMapper;
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
        //delegate
        //static readonly Display _userInterface = new Display(print => WriteToConsole(print));
        
        static readonly ApiActions _apiActions = new ApiActions();
        static readonly IMapper Mapper = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        })
        .CreateMapper();

        static readonly ICachedWordService _cachedWordService =
            new CachedWordService(new CachedWordRepositoryDB(), Mapper);

        static readonly IAnagramSolver _anagramSolver = 
            new BusinessLogic.Services.AnagramSolver(new FileRepository(), _cachedWordService);

        static async Task Main(string[] args)
        {
            if (File.Exists(@"ConsoleLog.txt"))
                File.Delete(@"ConsoleLog.txt");

            //events
            DisplayWithEvents _userInterface = new DisplayWithEvents();
            _userInterface.PrintEvent += WriteToConsole;
            _userInterface.PrintEvent += WriteToFile;

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
            var wordsList = await fileRepo.GetAllWords();

            foreach (var word in wordsList)
            {
                var model = new Anagram()
                {
                    Category = word.Category,
                    Word = word.Word,
                };

                await connection.AddNewWord(model);
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
    }
       
}
