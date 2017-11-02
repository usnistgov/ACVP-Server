using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        KeyPair = new EccKeyPair(new EccPoint(1, 2), 3),
                        Message = new BitString("BEEFFACE"),
                        Signature = new EccSignature(1, 2),
                        FailureTest = false,
                        Reason = SigFailureReasons.None,
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    DomainParameters = new EccDomainParameters(new PrimeCurve(Curve.P192, 0, 0, new EccPoint(0, 0), 0)),
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
