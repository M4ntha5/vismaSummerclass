using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IUserInterface
    {
        string GetInput();
        void DisplayResults(List<string> angarams);
        string ValidateInputData(string userInput);
        int DisplayOptions();
    }
}
