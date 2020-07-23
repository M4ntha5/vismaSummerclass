using System;
using System.Collections.Generic;
using NUnit.Framework;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Utils;
using Shouldly;
using AnagramSolver.Console;
using NSubstitute;
using AnagramSolver.Contracts.Models;
using System.Linq;

namespace AnagramSolver.Tests
{
    class AnagramSolverTests
    {
        IAnagramSolver solver;
        IWordRepository wordMock;
        [SetUp]
        public void Setup()
        {
            Settings.DataFileName = "zodynas.txt";
            solver = new BusinessLogic.Services.AnagramSolver()
            {
                FileRepository = new FileRepository()
            };
            Configuration.ReadAppSettingsFile();

            wordMock = Substitute.For<IWordRepository>();
        }

        [Test]
        public void TestGetAnagramsFromSingleWord()
        {
            var result = solver.GetAnagrams("naujas");
            result.ShouldContain("jaunas");

           //var isContains = result.Exists(x => x == "jaunas");      
           //Assert.IsTrue(isContains);
        }

        [Test]
        public void TestGetAnagramsFromPhrase()
        {
            var result = solver.GetAnagrams("labasrytas");
            result.ShouldContain("balas tyras");

            /*var isContains = result.Exists(x => x == "balas tyras");
            Assert.IsTrue(isContains);*/
        }

        [Test]
        public void TestCanGetFixedNumberOfAnagrams()
        {
            Settings.AnagramsToGenerate = 2;
            var result = solver.GetAnagrams("labasrytas");
            Assert.AreEqual(Settings.AnagramsToGenerate, result.Count);
        }

        [Test]
        public void TestGetAnagramsWithMock()
        {
            Settings.AnagramsToGenerate = 3;
            var list1 = new List<Anagram>()
            {
                new Anagram
                {
                    Case = "case1",
                    Word = "word1"
                },
                new Anagram
                {
                    Case = "case2",
                    Word = "word2"
                }
            };
            var list2 = new List<Anagram>()
            {
                new Anagram
                {
                    Case = "case1",
                    Word = "word1"
                }
            };

            var inputWord = "naujas";
            var sortedInput = String.Concat(inputWord.OrderBy(x => x));

            var data = new Dictionary<string, List<Anagram>>()
            {
                { sortedInput, list1 },
                { "key2", list2 }
            };

            wordMock.ReadDataFromFile().Returns(data);

            solver = new BusinessLogic.Services.AnagramSolver()
            {
                FileRepository = wordMock
            };

            var result = solver.GetAnagrams("naujas");

            wordMock.Received().ReadDataFromFile();
            Assert.LessOrEqual(result.Count, Settings.AnagramsToGenerate);
            Assert.AreEqual(list1.Count, result.Count);
            Assert.AreEqual(list1[0].Word, result[0]);

        }

    }
}