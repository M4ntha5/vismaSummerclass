using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface ITableHandler
    {
        Task ClearSelectedTable(List<string> tables);
    }
}
