using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestVectorFactory<TParameters> : ITestVectorFactory<TParameters>
        where TParameters : IParameters
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

            var testVector = new TestVectorSet { TestGroups = groups, IsSample = parameters.IsSample, Algorithm = parameters.Algorithm };

            return testVector;
        }
    }
}
