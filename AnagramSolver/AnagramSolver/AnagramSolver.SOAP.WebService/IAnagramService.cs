using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace AnagramSolver.SOAP.WebService
{
    [ServiceContract]
    public interface IAnagramService
    {
        [OperationContract]
        Task<List<string>> GetAnagrams(string word);
    }
}
