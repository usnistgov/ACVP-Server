using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.External;

namespace Web.Public.Providers
{
    public interface IAlgorithmProvider
    {
        IEnumerable<AlgorithmBase> GetAlgorithmList();
        AlgorithmBase GetAlgorithm(int id);
    }
}