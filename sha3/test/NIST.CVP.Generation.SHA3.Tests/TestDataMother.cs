using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.SHA3.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(HashFunction hashFunction, int groups = 1, bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Deferred = false,
                        FailureTest = failureTest,
                        Digest = new BitString("FACEDAD1"),
                        TestCaseId = testId
                    });
                }

                var domain = new MathDomain();
                domain.AddSegment(new RangeDomainSegment(null, 16 + groupIdx, 4000 + groupIdx));

                testGroups.Add(new TestGroup
                {
                    Function = hashFunction.XOF ? "SHAKE" : "SHA3",
                    DigestSize = hashFunction.DigestSize + groupIdx,
                    TestType = "AFT",
                    BitOrientedInput = false,
                    BitOrientedOutput = false,
                    IncludeNull = false,
                    OutputLength = domain,
                    Tests = tests
                });
            }

            return testGroups;
        }

        public List<TestGroup> GetMCTTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Deferred = false,
                        TestCaseId = testId,
                        Message = new BitString("5EED")
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        Function = "SHA3",
                        DigestSize = 224,
                        Tests = tests,
                        TestType = "mct"
                    }
                );
            }

            return testGroups;
        }
    }
}
