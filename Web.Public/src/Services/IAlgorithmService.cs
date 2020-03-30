using System.Collections.Generic;
using NIST.CVP.Algorithms.External;

namespace Web.Public.Services
{
    public interface IAlgorithmService
    {
        IEnumerable<AlgorithmBase> GetAlgorithmList();
        AlgorithmBase GetAlgorithm(int id);
    }
}