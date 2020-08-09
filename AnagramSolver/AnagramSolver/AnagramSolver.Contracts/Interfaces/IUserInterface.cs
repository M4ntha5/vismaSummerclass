using System.Collections.Generic;

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
