using AnagramSolver.Contracts.Enums;
using NUnit.Framework;

namespace AnagramSolver.Tests
{
    public class GenericsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MapIntToGender()
        {
            var result = Generics.Generics.MapIntToGender(1);

            Assert.AreEqual(Gender.Male, result);
        }

        [Test]
        public void MapStringToGender()
        {
            var result = Generics.Generics.MapStringToGender("Female");

            Assert.AreEqual(Gender.Female, result);
        }

        [Test]
        public void MapStringToWeekday()
        {
            var result = Generics.Generics.MapStringToWeekday("Friday");

            Assert.AreEqual(Weekday.Friday, result);
        }

        [Test]
        public void MapValueToEnumGenderInt()
        {
            var result = Generics.Generics.MapValueToEnum<Gender, int>(3);

            Assert.AreEqual(Gender.Other, result);
        }

        [Test]
        public void MapValueToEnumGenderString()
        {
            var result = Generics.Generics.MapValueToEnum<Gender, string>("Male");

            Assert.AreEqual(Gender.Male, result);
        }

        [Test]
        public void MapValueToEnumWeekday()
        {
            var result = Generics.Generics.MapValueToEnum<Weekday, string>("Tuesday");

            Assert.AreEqual(Weekday.Tuesday, result);
        }

        [Test]
        public void CompareStandartAndGenericWithStringToWeekday()
        {
            var result = Generics.Generics.MapStringToWeekday("Friday");

            var genericResult = Generics.Generics.MapValueToEnum<Weekday, string>("Friday");

            Assert.AreEqual(result, genericResult);
        }

        [Test]
        public void CompareStandartAndGenericWithIntToGender()
        {
            var result = Generics.Generics.MapIntToGender(2);

            var genericResult = Generics.Generics.MapValueToEnum<Gender, int>(2);

            Assert.AreEqual(result, genericResult);
        }

        [Test]
        public void CompareStandartAndGenericWithStringToGender()
        {
            var result = Generics.Generics.MapStringToGender("Male");

            var genericResult = Generics.Generics.MapValueToEnum<Gender, string>("Male");

            Assert.AreEqual(result, genericResult);
        }

    }
}
