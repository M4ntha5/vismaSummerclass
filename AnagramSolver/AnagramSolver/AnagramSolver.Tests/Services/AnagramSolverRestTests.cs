using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Utils;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class AnagramSolverRestTests
    {
        IAnagramSolver _anagramSolverRest;

        [SetUp]
        public void Setup()
        {
            Settings.AnagramicaApiUrl = "http://www.anagramica.com/";
            _anagramSolverRest = new AnagramSolverRest();
        }

        [Test]
        public async Task GetAnagramsSuccess()
        {
            var result = await _anagramSolverRest.GetAnagrams("evil");

            Assert.Greater(result.Count, 1);
        }

        [Test]
        public void GetAnagramsFailedWhenWordNotDefined()
        {
            Assert.ThrowsAsync<Exception>(async () => await _anagramSolverRest.GetAnagrams(""));
        }
    }
}
