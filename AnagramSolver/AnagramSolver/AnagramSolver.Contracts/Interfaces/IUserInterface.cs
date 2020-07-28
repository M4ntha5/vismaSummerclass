using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IUserInterface
    {
        string GetInput();
        void DisplayResults(List<string> angarams);
        string ValidateInputData(string userInput);
    }
}
