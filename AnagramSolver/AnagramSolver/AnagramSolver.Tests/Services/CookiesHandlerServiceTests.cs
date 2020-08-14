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
        public void AddCookieSuccessfully()
        {
            _httpContextMock.HttpContext.Response.Cookies.Append(Arg.Any<string>(), Arg.Any<string>());

            _cookiesHandlerService.AddCookie("key", "value");

            _httpContextMock.HttpContext.Response.Cookies.Received().Append(Arg.Any<string>(), Arg.Any<string>());
        }

    }
}
