using System;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseGeneratorPQ : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IPQGeneratorValidator _pqGen;
        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorPQ(IRandom800_90 rand, IPQGeneratorValidator pqGen)
        {
            _rand = rand;
            _pqGen = pqGen;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                // This is a bit awkward when we need to cover the sets
                NumberOfTestCasesToGenerate = 2;
            }

            var reason = group.PQTestCaseExpectationProvider.GetRandomReason();

            var testCase = new TestCase
            {
                Reason = reason.GetName(),
                FailureTest = (reason.GetReason() != PQFailureReasons.None)
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            // Generate normal case
            PQGenerateResult sampleResult = null;
            try
            {
                sampleResult = _pqGen.Generate(group.L, group.N, group.N);
                if (!sampleResult.Success)
                {
                    ThisLogger.Warn($"Error generating sample: {sampleResult.ErrorMessage}");
                    return new TestCaseGenerateResponse(sampleResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating sample: {sampleResult.ErrorMessage}, {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating sample: {sampleResult.ErrorMessage}, {ex.StackTrace}");
            }

            testCase.P = sampleResult.P;
            testCase.Q = sampleResult.Q;
            testCase.Seed = sampleResult.Seed;
            testCase.Counter = sampleResult.Count;

            // Modify values
            var friendlyReason = EnumHelpers.GetEnumFromEnumDescription<PQFailureReasons>(testCase.Reason);
            if (friendlyReason == PQFailureReasons.ModifyP)
            {
                // Make P not prime
                do
                {
                    testCase.P = _rand.GetRandomBitString(group.L).ToPositiveBigInteger();

                } while (NumberTheory.MillerRabin(testCase.P, DSAHelper.GetMillerRabinIterations(group.L, group.N)));
            }
            else if (friendlyReason == PQFailureReasons.ModifyQ)
            {
                // Modify Q so that 0 != (P-1) mod Q
                testCase.Q = _rand.GetRandomBitString(group.N).ToPositiveBigInteger();
            }
            else if (friendlyReason == PQFailureReasons.ModifySeed)
            {
                // Modify FirstSeed
                var oldSeed = new BitString(testCase.Seed.Seed);
                var newSeed = _rand.GetRandomBitString(oldSeed.BitLength).ToPositiveBigInteger();

                testCase.Seed.ModifySeed(newSeed);
            }

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
