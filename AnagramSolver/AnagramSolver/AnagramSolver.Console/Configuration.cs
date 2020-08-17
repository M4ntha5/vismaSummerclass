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
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            int.TryParse(configuration.GetSection("AnagramsToGenerate").Value, out int anagramsCount);
            int.TryParse(configuration.GetSection("MinInputLength").Value, out int minLength);
            var dataFile = configuration.GetSection("DataFileName").Value;
            var connString = configuration.GetConnectionString("Development");
            var connStringCodeFirst = configuration.GetConnectionString("CodeFirst");
            var connStringTesting = configuration.GetConnectionString("Testing");
            int.TryParse(configuration.GetSection("MaxAnagramsForIp").Value, out int maxForIp);
            var anagramica = configuration.GetSection("AnagramicaApiUrl").Value;

            if (anagramsCount > 10 || anagramsCount < 1)
                throw new Exception("Generated anagrams count must be between 1 and 10");

            Settings.AnagramsToGenerate = anagramsCount;
            Settings.MinInputLength = minLength;
            Settings.DataFileName = dataFile;
            Settings.ConnectionString = connString;
            Settings.ConnectionStringCodeFirst = connStringCodeFirst;
            Settings.ConnectionStringTesting = connStringTesting;
            Settings.MaxAnagramsForIp = maxForIp;
            Settings.AnagramicaApiUrl = anagramica;

        }
    }
}
