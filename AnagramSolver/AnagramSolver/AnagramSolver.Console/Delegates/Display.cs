using AnagramSolver.BusinessLogic.Properties;
using AnagramSolver.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Text;

namespace AnagramSolver.Console.Delegates
{
    public delegate void Print(string message);
    public class Display : IDisplay
    {
        
        public Display(Print print)
        {
            Print del = print;
        }

        public void FormattedPrint(Print del, string input)
        {
            del(input);
        }


    }
}
