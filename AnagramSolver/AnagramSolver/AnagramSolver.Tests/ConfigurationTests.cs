using AnagramSolver.Console;
using NUnit.Framework;
using System;

namespace AnagramSolver.Tests
{
    class ConfigurationTests
    {
        [SetUp]
        public static void Setup()
        {
            
        }

        [Test]
        public static void TestReadAppSettingsFile()
        {
            Configuration.ReadAppSettingsFile();
        }
    }
}
