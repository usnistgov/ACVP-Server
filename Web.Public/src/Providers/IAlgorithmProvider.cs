using System.Collections.Generic;
using ACVPCore.Algorithms.External;

namespace Web.Public.Providers
{
    public interface IAlgorithmProvider
    {
        IEnumerable<AlgorithmBase> GetAlgorithmList();
    }
}