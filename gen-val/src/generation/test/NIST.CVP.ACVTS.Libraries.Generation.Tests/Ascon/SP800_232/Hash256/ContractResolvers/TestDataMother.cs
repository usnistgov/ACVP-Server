using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.Hash256;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.Hash256.ContractResolvers;
public static class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet
        {
            Algorithm = "Ascon",
            Mode = "Hash256",
            Revision = "SP800-232",
            IsSample = true
        };

        var testGroups = new List<TestGroup>();
        
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(16)),
                TestType = "AFT"
            };
            testGroups.Add(tg);

            var tests = new List<TestCase>();
            tg.Tests = tests;
            for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
            {
                var tc = new TestCase
                {
                    ParentGroup = tg,
                    Message = new BitString("ABCD"),
                    MessageBitLength = 16,
                    Digest = new BitString("CDEF"),
                    TestCaseId = testId
                };
                tests.Add(tc);
            }
        }
        tvs.TestGroups = testGroups;
        return tvs;
    }
}
