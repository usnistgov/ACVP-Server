using NIST.CVP.Common.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.Core
{
    public class TestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase> : ITestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase>
        where TParameters : IParameters
        where TTestVectorSet : class, ITestVectorSet<TTestGroup, TTestCase>, new()
        where TTestGroup : class, ITestGroup<TTestGroup, TTestCase>
        where TTestCase : class, ITestCase<TTestGroup, TTestCase>
    {
        private readonly ITestGroupGeneratorFactory<TParameters, TTestGroup, TTestCase> _iTestGroupGeneratorFactory;

        public TestVectorFactory(ITestGroupGeneratorFactory<TParameters, TTestGroup, TTestCase> iTestGroupGeneratorFactory)
        {
            _iTestGroupGeneratorFactory = iTestGroupGeneratorFactory;
        }

        public TTestVectorSet BuildTestVectorSet(TParameters parameters)
        {
            List<TTestGroup> groups = new List<TTestGroup>();

            var groupGenerators = _iTestGroupGeneratorFactory.GetTestGroupGenerators(parameters).ToList();
            foreach (var groupGenerator in groupGenerators)
            {
                groups.AddRangeIfNotNullOrEmpty(groupGenerator.BuildTestGroups(parameters));
            }

            int testGroupId = 1;
            foreach (var group in groups)
            {
                group.TestGroupId = testGroupId++;
                if (string.IsNullOrEmpty(group.TestType))
                {
                    group.TestType = "AFT";
                }
            }

            var testVector = new TTestVectorSet
            {
                VectorSetId = parameters.VectorSetId,
                TestGroups = groups,
                IsSample = parameters.IsSample,
                Algorithm = parameters.Algorithm,
                Mode = parameters.Mode
            };

            return testVector;
        }
    }
}
