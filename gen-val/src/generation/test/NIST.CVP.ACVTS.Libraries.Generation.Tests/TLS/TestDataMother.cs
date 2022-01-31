using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg =
                    new TestGroup
                    {
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                        TlsMode = TlsModes.v12,
                        PreMasterSecretLength = groupIdx,
                        KeyBlockLength = groupIdx,
                        TestType = "Sample"
                    };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        ClientHelloRandom = new BitString("1AAADFF1"),
                        ServerHelloRandom = new BitString("1AAADFF0"),
                        ClientRandom = new BitString("1AAADFFA"),
                        ServerRandom = new BitString("1AAADFFB"),
                        PreMasterSecret = new BitString("1AAADFFC"),
                        MasterSecret = new BitString("1AAADFFC02"),
                        KeyBlock = new BitString("1AAADFFD"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }


            }

            return vectorSet;
        }
    }
}
