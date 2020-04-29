using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.External;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class AlgorithmService : IAlgorithmService
    {
        private readonly IAlgorithmProvider _algoProvider;

        public AlgorithmService(IAlgorithmProvider algoProvider)
        {
            _algoProvider = algoProvider;
        }

        public IEnumerable<AlgorithmBase> GetAlgorithmList() => _algoProvider.GetAlgorithmList();

        public AlgorithmBase GetAlgorithm(int id) => _algoProvider.GetAlgorithm(id);

        public AlgorithmBase GetAlgorithm(string algorithmName, string mode, string revision) => _algoProvider.GetAlgorithm(algorithmName, mode, revision);
    }
}