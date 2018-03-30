using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Key = new FfcKeyPair(1, 2),
                        DomainParams = new FfcDomainParameters(3, 4, 5),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    L = 2048 + groupIdx,
                    N = 224,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
