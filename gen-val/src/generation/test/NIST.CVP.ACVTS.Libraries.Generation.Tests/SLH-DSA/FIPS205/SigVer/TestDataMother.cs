using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.SigVer;

public class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet { Algorithm = "SLH-DSA", Mode = "sigVer", Revision = "FIPS205", IsSample = true };
        
        var testGroups = new List<TestGroup>();
        tvs.TestGroups = testGroups;
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                TestType = "AFT",
                ParameterSet = SlhdsaParameterSet.SLH_DSA_SHA2_128f,
                SignatureInterface = SignatureInterface.Internal,
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1024, 4096, 8))
            };
            testGroups.Add(tg);

            var tests = new List<TestCase>();
            tg.Tests = tests;
            for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
            {
                var tc = new TestCase
                {
                    ParentGroup = tg,
                    TestCaseId = testId,
                    PublicKey = new BitString("123456"),
                    PrivateKey = new BitString("654321"),
                    AdditionalRandomness = new BitString("615243"),
                    Message = new BitString("789ABC9ABC"),
                    Signature = new BitString("DEF012"),
                    Reason = SLHDSASignatureDisposition.ModifyMessage
                };
                tests.Add(tc);
            }
        }
        
        return tvs;
    }
}
