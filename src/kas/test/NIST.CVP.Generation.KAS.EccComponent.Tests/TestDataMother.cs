using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.KAS.v1_0.ECC_Component;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.EccComponent.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetVectorSet()
        {
            var vectorSet = new TestVectorSet();
            vectorSet.TestGroups = new List<TestGroup>();

            var tg = new TestGroup()
            {
                Curve = Curve.B409,
                TestGroupId = 2,
                Tests = new List<TestCase>(),
                TestType = "AFT"
            };
            vectorSet.TestGroups.Add(tg);

            var tc = new TestCase()
            {
                TestCaseId = 5,
                ParentGroup = tg,
                KeyPairPartyServer = new EccKeyPair(new EccPoint(1, 2), 3),
                KeyPairPartyIut = new EccKeyPair(new EccPoint(4, 5), 6),
                Z = new BitString("07")
            };
            tg.Tests.Add(tc);

            return vectorSet;
        }
    }
}
