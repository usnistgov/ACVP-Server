using System;
using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseGeneratorG : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IGGeneratorValidator _gGen;
        private readonly IPQGeneratorValidator _pqGen;
        private readonly IShaFactory _shaFactory = new ShaFactory();

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorG(IRandom800_90 rand, IGGeneratorValidator gGen, IPQGeneratorValidator pqGen = null)
        {
            _rand = rand;
            _gGen = gGen;

            if (pqGen == null)
            {
                var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
                _pqGen = new ProbablePQGeneratorValidator(_shaFactory.GetShaInstance(hashFunction));
            }
            else
            {
                _pqGen = pqGen;
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
            }

            // We need a PQ pair for the test case
            var pqResult = _pqGen.Generate(group.L, group.N, group.N);
            if (!pqResult.Success)
            {
                return new TestCaseGenerateResponse(pqResult.ErrorMessage);
            }

            // Make sure index is not "0000 0000"
            var index = BitString.Zeroes(8);
            do
            {
                index = _rand.GetRandomBitString(8);
            } while (index.ToHex() == "00");

            // Determine failure reason
            var reason = group.GTestCaseExpectationProvider.GetRandomReason();

            // Assign values of the TestCase
            var testCase = new TestCase
            {
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed,
                Counter = pqResult.Count,
                Index = index,
                Reason = reason.GetName(),
                FailureTest = (reason.GetReason() != GFailureReasons.None)
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            // Generate g
            GGenerateResult gResult = null;
            try
            {
                gResult = _gGen.Generate(testCase.P, testCase.Q, testCase.Seed, testCase.Index);

                if (!gResult.Success)
                {
                    ThisLogger.Warn($"Error generating g: {gResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating g: {gResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating g: {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating g: {ex.StackTrace}");
            }

            testCase.G = gResult.G;
            if (group.GGenMode == GeneratorGenMode.Unverifiable)
            {
                testCase.H = gResult.H;
            }

            // Modify g
            if (testCase.FailureTest)
            {
                do
                {
                    testCase.G = _rand.GetRandomBitString(group.L).ToPositiveBigInteger();

                } while (BigInteger.ModPow(testCase.G, testCase.Q, testCase.P) == 1);
            }

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
