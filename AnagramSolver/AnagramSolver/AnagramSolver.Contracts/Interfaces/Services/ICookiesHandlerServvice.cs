using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface ICookiesHandlerServvice
    {
        void AddCookie(string key, string value);
        void ClearAllCookies();
        Dictionary<string, string> GetCurrentCookies();
        string GetCookieByKey(string key);

    }
}
