using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class CookiesHandlerServiceTests
    {
        IHttpContextAccessor _httpContextMock;
        ICookiesHandlerService _cookiesHandlerService;

        [SetUp]
        public void Setup()
        {
            _httpContextMock = Substitute.For<IHttpContextAccessor>();
            _cookiesHandlerService = new CookiesHandlerService(_httpContextMock);
        }

        [Test]
        public void AddCookieSuccess()
        {
            _httpContextMock.HttpContext.Response.Cookies.Append(Arg.Any<string>(), Arg.Any<string>());

            _cookiesHandlerService.AddCookie("key", "value");

            _httpContextMock.HttpContext.Response.Cookies.Received().Append(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void ClearAllCookiesSuccess()
        {
            var cookiesList = new List<string>() { "cookie1" };
            _httpContextMock.HttpContext.Request.Cookies.Keys.Returns(cookiesList);
            _httpContextMock.HttpContext.Response.Cookies.Delete(Arg.Any<string>());

            _cookiesHandlerService.ClearAllCookies();

            _httpContextMock.Received().HttpContext.Response.Cookies.Delete(Arg.Any<string>());
        }

        [Test]
        public void GetCookieByKeySuccess()
        {
            _httpContextMock.HttpContext.Request.Cookies[Arg.Any<string>()].Returns("cookie");

            var result = _cookiesHandlerService.GetCookieByKey("key");

            Assert.AreEqual("cookie", result);
        }

    }
}
