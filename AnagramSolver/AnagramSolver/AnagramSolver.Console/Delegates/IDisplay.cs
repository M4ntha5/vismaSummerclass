using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Console.Delegates
{
    public interface IDisplay
    {
        void FormattedPrint(Print del, string input);
    }
}
