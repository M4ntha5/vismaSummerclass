using System.Collections.Generic;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IUserInterface
    {
        string GetInput();
        void DisplayResults(List<string> angarams);
        int DisplayOptions();
    }
}
