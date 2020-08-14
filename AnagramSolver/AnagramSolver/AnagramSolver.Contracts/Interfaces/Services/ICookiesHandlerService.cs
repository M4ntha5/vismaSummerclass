using System.Collections.Generic;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface ICookiesHandlerService
    {
        void AddCookie(string key, string value);
        void ClearAllCookies();
        Dictionary<string, string> GetCurrentCookies();
        string GetCookieByKey(string key);

    }
}
