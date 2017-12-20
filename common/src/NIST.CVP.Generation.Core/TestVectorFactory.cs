using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.Core
{
    public class TestVectorFactory<TParameters, TTestVectorSet> : ITestVectorFactory<TParameters>
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet, new()
    {
        private readonly ITestGroupGeneratorFactory<TParameters> _iTestGroupGeneratorFactory;

        public TestVectorFactory(ITestGroupGeneratorFactory<TParameters> iTestGroupGeneratorFactory)
        {
            _iTestGroupGeneratorFactory = iTestGroupGeneratorFactory;
        }

        public ITestVectorSet BuildTestVectorSet(TParameters parameters)
        {
            List<ITestGroup> groups = new List<ITestGroup>();

            var groupGenerators = _iTestGroupGeneratorFactory.GetTestGroupGenerators().ToList();
            foreach (var groupGenerator in groupGenerators)
            {
                groups.AddRangeIfNotNullOrEmpty(groupGenerator.BuildTestGroups(parameters));
            }

            var testVector = new TTestVectorSet
            {
                TestGroups = groups,
                IsSample = parameters.IsSample,
                Algorithm = parameters.Algorithm,
                Mode = parameters.Mode
            };

            return testVector;
        }
    }
}
