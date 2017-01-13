using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        private readonly IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>> _iKATTestGroupFactory;
        private readonly IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>> _IMCTTestGroupFactory;

        public TestVectorFactory(
            IKATTestGroupFactory<Parameters, IEnumerable<TestGroup>> iKATTestGroupFactory, 
            IMCTTestGroupFactory<Parameters, IEnumerable<TestGroup>> iMCTTestGroupFactory)
        {
            _iKATTestGroupFactory = iKATTestGroupFactory;
            _IMCTTestGroupFactory = iMCTTestGroupFactory;
        }

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters); // Random tests for test groups
            groups.AddRange(_iKATTestGroupFactory.BuildKATTestGroups(parameters));
            // @@@ TODO complete MCT validation implementation prior to adding to groups
            // groups.AddRange(_IMCTTestGroupFactory.BuildMCTTestGroups(parameters));
            var testVector = new TestVectorSet {TestGroups = groups, IsSample = parameters.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                    foreach (var ptLength in parameters.PtLen)
                    {
                        var testGroup = new TestGroup
                        {
                            Function = function,
                            PTLength = ptLength,
                            KeyLength = keyLength,
                            TestType = "MMT"
                        };
                        testGroups.Add(testGroup);
                    }
                }
            }
            return testGroups;
        }
    }
}
