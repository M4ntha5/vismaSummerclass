using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Console.Delegate
{
    public interface IDisplay
    {
        void DisplayResults(List<string> angarams);

        //delegate 
        void FormattedPrint(FormattedPrint del, string input);

        //action func
        //void FormattedPrint(Func<string, string> del, string input);
   
    }
}
