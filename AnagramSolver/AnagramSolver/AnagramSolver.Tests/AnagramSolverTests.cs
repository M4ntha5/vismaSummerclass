using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Text;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Utils;
using Shouldly;

namespace AnagramSolver.Tests
{
    class AnagramSolverTests
    {
        IAnagramSolver solver;
        [SetUp]
        public void Setup()
        {
            Settings.DataFileName = "zodynas.txt";
            solver = new BusinessLogic.Services.AnagramSolver()
            {
                FileRepository = new FileRepository()
            };
        }

        [Test]
        public void TestGetAnagramsFromSingleWord()
        {
            var result = (List<string>)solver.GetAnagrams("naujas");
            result.ShouldContain("jaunas");

           //var isContains = result.Exists(x => x == "jaunas");      
           //Assert.IsTrue(isContains);
        }

        [Test]
        public void TestGetAnagramsFromPhrase()
        {
            var result = (List<string>)solver.GetAnagrams("labasrytas");
            result.ShouldContain("balas tyras");

            /*var isContains = result.Exists(x => x == "balas tyras");
            Assert.IsTrue(isContains);*/
        }

    }
}