using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console.UI;
using AnagramSolver.Contracts.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

            var AnagramSolver = new BusinessLogic.Services.AnagramSolver(new FileRepository());
            var howToSolve = UserInterface.DisplayOptions();


            while (true)
            {
                //getting initial user input
                var userInput = UserInterface.GetInput();

                if (userInput == null)
                    break;
                
                List<string> result;
                if (howToSolve == 2)
                    result = (List<string>)AnagramSolver.GetAnagrams(userInput);
                else
                    result = await apiActions.CallAnagramSolverApi(userInput);


                UserInterface.DisplayResults(result);
            }
        }


    }
}
