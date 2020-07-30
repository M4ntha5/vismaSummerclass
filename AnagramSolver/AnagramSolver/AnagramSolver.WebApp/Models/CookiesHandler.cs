using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Models
{
    public class CookiesHandler : ICookiesHandler
    {
        private readonly IHttpContextAccessor _httpAccessor;

        public CookiesHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }

        public void AddCookie(string key, string value)
        {
            _httpAccessor.HttpContext.Response.Cookies.Append(key, value);
        }
        public void ClearAllCookies()
        {
            foreach (var cookieKey in _httpAccessor.HttpContext.Request.Cookies.Keys)
                _httpAccessor.HttpContext.Response.Cookies.Delete(cookieKey);
        }
        public Dictionary<string, string> GetCurrentCookies()
        {
            var cookiesDictionary = new Dictionary<string, string>();
            foreach (var cookie in _httpAccessor.HttpContext.Request.Cookies)
                cookiesDictionary.Add(cookie.Key, cookie.Value);

            return cookiesDictionary;
        }


    }
}
