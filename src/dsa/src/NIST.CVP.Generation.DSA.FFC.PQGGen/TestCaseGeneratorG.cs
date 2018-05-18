using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorG : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IGGeneratorValidatorFactory _gGenFactory;
        private readonly IPQGeneratorValidatorFactory _pqGenFactory;
        private readonly IShaFactory _shaFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 2;

        public TestCaseGeneratorG(IRandom800_90 rand, IShaFactory shaFactory, IPQGeneratorValidatorFactory pqGenFactory, IGGeneratorValidatorFactory gGenFactory)
        {
            _rand = rand;
            _shaFactory = shaFactory;
            _pqGenFactory = pqGenFactory;
            _gGenFactory = gGenFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            // Make sure index is not "0000 0000"
            BitString index;
            do
            {
                index = _rand.GetRandomBitString(8);
            } while (index.Equals(BitString.Zeroes(8)));

            // We need a PQ pair for the test case
            var sha = _shaFactory.GetShaInstance(group.HashAlg);
            var pqGen = _pqGenFactory.GetGeneratorValidator(group.PQGenMode, sha, EntropyProviderTypes.Random);
            var pqResult = pqGen.Generate(group.L, group.N, group.N);
            if (!pqResult.Success)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(pqResult.ErrorMessage);
            }

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
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            GGenerateResult gResult = null;
            try
            {
                var sha = _shaFactory.GetShaInstance(group.HashAlg);
                var gGen = _gGenFactory.GetGeneratorValidator(group.GGenMode, sha);
                gResult = gGen.Generate(testCase.P, testCase.Q, testCase.Seed, testCase.Index);

                if (!gResult.Success)
                {
                    ThisLogger.Warn($"Error generating g: {gResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating g: {gResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating g: {ex.StackTrace}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating g: {ex.Message}");
            }

            testCase.G = gResult.G;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
