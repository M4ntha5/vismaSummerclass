using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console.UI;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace AnagramSolver.Console
{
    class Program
    {
        static readonly IAnagramSolver AnagramSolver = new BusinessLogic.Services.AnagramSolver()
        {
            FileRepository = new FileRepository()
        };
        static readonly UserInterface UserInterface = new UserInterface();

        static void Main(string[] args)
        {
            //loading data from settings file
            ReadSettingsFile();
            //getting initial user input
            var userInput = UserInterface.GetInput();

            var result = (List<string>)AnagramSolver.GetAnagrams(userInput);

            UserInterface.DisplayResults(result);

        }

        public static void ReadSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../../AnagramSolver.Console"))
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            int.TryParse(configuration.GetSection("AnagramsToGenerate").Value, out int anagramsCount);
            int.TryParse(configuration.GetSection("MinInputLength").Value, out int minLength);
            var dataFile = configuration.GetSection("DataFileName").Value;

            if (anagramsCount > 10 || anagramsCount < 1)
                throw new Exception("Generated anagrams count must be between 1 and 10");

            Settings.AnagramsToGenerate = anagramsCount;
            Settings.MinInputLength = minLength;
            Settings.DataFileName = dataFile;

        }
    }
}
