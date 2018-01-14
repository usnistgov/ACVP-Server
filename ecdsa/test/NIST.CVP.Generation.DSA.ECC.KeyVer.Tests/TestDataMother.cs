using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.KeyVer.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
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
                        TestCaseId = testId,
                        KeyPair = new EccKeyPair(new EccPoint(1, 2), 3),
                        FailureTest = true,
                        Result = true,
                        Reason = TestCaseExpectationEnum.NotOnCurve
                    });
                }

                testGroups.Add(new TestGroup
                {
                    DomainParameters = new EccDomainParameters(new PrimeCurve(Curve.P224, groupIdx, 0, new EccPoint(0, 0), 0)),
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
