using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.External;

namespace Web.Public.Services
{
    public interface IAlgorithmService
    {
        IEnumerable<AlgorithmBase> GetAlgorithmList();
        AlgorithmBase GetAlgorithm(int id);
        AlgorithmBase GetAlgorithm(string algorithmName, string mode, string revision);
    }
}