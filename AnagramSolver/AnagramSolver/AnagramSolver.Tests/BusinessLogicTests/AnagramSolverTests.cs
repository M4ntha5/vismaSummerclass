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
        List<Anagram> list1;
        List<Anagram> list2;
        [SetUp]
        public void Setup()
        {
            Configuration.ReadAppSettingsFile();
            solver = new BusinessLogic.Services.AnagramSolver(new FileRepository());
            Configuration.ReadAppSettingsFile();

            wordMock = Substitute.For<IWordRepository>();

            list1 = new List<Anagram>()
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
            list2 = new List<Anagram>()
            {
                new Anagram
                {
                    Case = "case1",
                    Word = "word1"
                }
            };
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
            //adding one more to list to check if it will return less than total found 
            list1.Add(new Anagram { Case = "case3", Word = "word3" });
            Settings.AnagramsToGenerate = list1.Count-1;

            var sortedInput = String.Concat("labasrytas".OrderBy(x => x));          

            var data = new Dictionary<string, List<Anagram>>()
            {
                { sortedInput, list1 },
                { "key2", list2 }
            };

            wordMock.GetData().Returns(data);

            solver = new BusinessLogic.Services.AnagramSolver(wordMock);

            var result = solver.GetAnagrams("labasrytas");
            Assert.AreEqual(Settings.AnagramsToGenerate, result.Count);
        }

        [Test]
        public void TestGetAnagramsWithMock()
        {
            Settings.AnagramsToGenerate = 3;
            
            var inputWord = "naujas";
            var sortedInput = String.Concat(inputWord.OrderBy(x => x));

            var data = new Dictionary<string, List<Anagram>>()
            {
                { sortedInput, list1 },
                { "key2", list2 }
            };

            wordMock.GetData().Returns(data);

            solver = new BusinessLogic.Services.AnagramSolver(wordMock);

            var result = solver.GetAnagrams(inputWord);

            wordMock.Received().GetData();
            Assert.LessOrEqual(result.Count, Settings.AnagramsToGenerate);
            Assert.AreEqual(list1.Count, result.Count);
            Assert.AreEqual(list1[0].Word, result[0]);

        }

    }
}