using NIST.CVP.Common.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class TestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase> : ITestVectorFactoryAsync<TParameters, TTestVectorSet, TTestGroup, TTestCase>
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

        public async Task<TTestVectorSet> BuildTestVectorSetAsync(TParameters parameters)
        {
            List<TTestGroup> groups = new List<TTestGroup>();

            var groupGenerators = _iTestGroupGeneratorFactory.GetTestGroupGenerators(parameters).ToList();
            foreach (var groupGenerator in groupGenerators)
            {
                groups.AddRangeIfNotNullOrEmpty(await groupGenerator.BuildTestGroupsAsync(parameters));
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
                Mode = parameters.Mode,
                Revision = parameters.Revision
            };

            return testVector;
        }
    }
}
