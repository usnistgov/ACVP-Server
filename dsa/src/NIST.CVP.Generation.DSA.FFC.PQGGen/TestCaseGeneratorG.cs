using System;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
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
            } while (index == BitString.Zeroes(8));

            // Assign values of the TestCase
            var testCase = new TestCase
            {
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed,
                Counter = pqResult.Count,
                Index = index
            };
            
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
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
                ThisLogger.Error($"Exception generating g: {gResult.ErrorMessage}");
                return new TestCaseGenerateResponse($"Exception generating g: {gResult.ErrorMessage}");
            }

            testCase.G = gResult.G;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
