using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface ICookiesHandler
    {
        void AddCookie(string key, string value);
        void ClearAllCookies();
        Dictionary<string, string> GetCurrentCookies();

    }
}
