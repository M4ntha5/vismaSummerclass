using System;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Utils;
using NUnit.Framework;
using Shouldly;

namespace AnagramSolver.Tests
{
    class FileRepositoryTests
    {
        IWordRepository wordRepository;
        [SetUp]
        public void Setup()
        {
            Configuration.ReadAppSettingsFile();
            wordRepository = new FileRepository();
        }


        [Test]
        public void TestReadDataFromFile()
        {
            var result = wordRepository.GetData();
            result.ShouldNotBeEmpty();
            //Assert.Greater(result.Count, 0);
        }
    }
}
