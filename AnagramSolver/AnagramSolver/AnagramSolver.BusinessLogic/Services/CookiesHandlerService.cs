using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AnagramSolver.BusinessLogic.Services
{
    public class CookiesHandlerService : ICookiesHandlerServvice
    {
        private readonly IHttpContextAccessor _httpAccessor;

        public CookiesHandlerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpAccessor = httpContextAccessor;
        }

        public void AddCookie(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
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
        public string GetCookieByKey(string key)
        {
            return _httpAccessor.HttpContext.Request.Cookies[key];
        }
    }
}
