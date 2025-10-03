using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.AEAD128.ContractResolvers;
public static class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 4)
    {
        var tvs = new TestVectorSet
        {
            Algorithm = "Ascon",
            Mode = "AEAD128",
            Revision = "SP800-232",
            IsSample = true
        };

        var testGroups = new List<TestGroup>();
        
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                PlaintextLength = new MathDomain().AddSegment(new ValueDomainSegment(16)),
                ADLength = new MathDomain().AddSegment(new ValueDomainSegment(16)),
                TruncationLength = new MathDomain().AddSegment(new ValueDomainSegment(16)),
                TestType = "AFT"
            };
            if(groupIdx % 2 == 0)
            {
                tg.NonceMasking = true;
            }
            else
            {
                tg.NonceMasking = false;
            }
            if(groupIdx < 2)
            {
                tg.Direction = BlockCipherDirections.Encrypt;
            }
            else
            {
                tg.Direction = BlockCipherDirections.Decrypt;
            }
            testGroups.Add(tg);

            var tests = new List<TestCase>();
            tg.Tests = tests;
            for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
            {
                var tc = new TestCase
                {
                    ParentGroup = tg,
                    Key = new BitString("ABCD"),
                    Nonce = new BitString("BCDE"),
                    AD = new BitString("CDEF"),
                    Tag = new BitString("DEF1"),
                    Plaintext = new BitString("EF12"),
                    PayloadBitLength = 16,
                    ADBitLength = 16,
                    TagLength = 16,
                    SecondKey = new BitString("F123"),
                    Ciphertext = new BitString("1234"),
                    TestCaseId = testId
                };
                tests.Add(tc);
            }
        }
        tvs.TestGroups = testGroups;
        return tvs;
    }
}
