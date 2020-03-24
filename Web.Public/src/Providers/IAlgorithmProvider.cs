using System.Collections.Generic;
using NIST.CVP.Algorithms.External;

namespace Web.Public.Providers
{
    public interface IAlgorithmProvider
    {
        IEnumerable<AlgorithmBase> GetAlgorithmList();
        AlgorithmBase GetAlgorithm(int id);
    }
}