using System;
using AnagramSolver.BusinessLogic.Repositories;
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
            wordRepository = new FileRepository();         
        }

        [Test]
        public void TestFileDoesNotExistException()
        {
            Settings.DataFileName = "not found.txt";
            Should.Throw<Exception>(() => wordRepository.ReadDataFromFile());
            //Assert.Throws<Exception>(() => wordRepository.ReadDataFromFile());
        }

        [Test]
        public void TestReadDataFromFile()
        {
            Settings.DataFileName = "zodynas.txt";
            var result = wordRepository.ReadDataFromFile();
            result.ShouldNotBeEmpty();
            //Assert.Greater(result.Count, 0);
        }
    }
}
