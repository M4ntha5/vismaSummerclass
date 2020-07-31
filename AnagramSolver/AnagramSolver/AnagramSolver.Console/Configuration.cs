using AnagramSolver.Contracts.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AnagramSolver.Console
{
    public class Configuration
    {
        public static void ReadAppSettingsFile()
        {
            var  path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
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
