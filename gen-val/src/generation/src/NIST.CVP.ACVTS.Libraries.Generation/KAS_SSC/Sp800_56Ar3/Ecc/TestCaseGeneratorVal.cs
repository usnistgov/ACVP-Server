﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3.Ecc
{
    public class TestCaseGeneratorVal : TestCaseGeneratorValBase<TestGroup, TestCase, EccKeyPair>
    {
        public TestCaseGeneratorVal(IOracle oracle, ITestCaseExpectationProvider<KasSscTestCaseExpectation> testCaseExpectationProvider, int numberOfTestCasesToGenerate)
            : base(oracle, testCaseExpectationProvider, numberOfTestCasesToGenerate)
        {
        }

        protected override EccKeyPair GetKey(IDsaKeyPair keyPair)
        {
            if (keyPair == null)
                return new EccKeyPair();

            return (EccKeyPair)keyPair;
        }
    }
}
