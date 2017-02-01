using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB1
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
            var groups = BuildTestGroups(parameters);
            var katGroups = _iKATTestGroupFactory.BuildKATTestGroups(parameters);
            if (katGroups != null && katGroups.Count() != 0)
            {
                groups.AddRange(katGroups);
            }
            var mctGroups = _IMCTTestGroupFactory.BuildMCTTestGroups(parameters);
            if (mctGroups != null && mctGroups.Count() != 0)
            {
                groups.AddRange(mctGroups);
            }

            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "AES-OFB", IsSample = parameters.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                foreach (var keyLength in parameters.KeyLen)
                {
                        var testGroup = new TestGroup
                        {
                            Function = function,
                            KeyLength = keyLength,
                            TestType = "MMT"
                        };
                        testGroups.Add(testGroup);
                }
            }
            return testGroups;
        }
    }
}
