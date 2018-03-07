using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorPQ : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IPQGeneratorValidatorFactory _pqGenFactory;
        private readonly IShaFactory _shaFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 5;

        public TestCaseGeneratorPQ(IRandom800_90 rand, IShaFactory shaFactory, IPQGeneratorValidatorFactory pqGenFactory)
        {
            _rand = rand;
            _shaFactory = shaFactory;
            _pqGenFactory = pqGenFactory;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase();

            if (isSample)
            {
                NumberOfTestCasesToGenerate = 2;
                return Generate(group, testCase);
            }

            return new TestCaseGenerateResponse(testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            PQGenerateResult sampleResult = null;
            try
            {
                var sha = _shaFactory.GetShaInstance(group.HashAlg);
                var pqGen = _pqGenFactory.GetGeneratorValidator(group.PQGenMode, sha, EntropyProviderTypes.Random);
                sampleResult = pqGen.Generate(group.L, group.N, group.N);
                if (!sampleResult.Success)
                {
                    ThisLogger.Warn($"Error generating sample: {sampleResult.ErrorMessage}");
                    return new TestCaseGenerateResponse(sampleResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating sample: {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating sample: {ex.Message}");
            }

            // We don't actually need anything from the old test case
            testCase = new TestCase
            {
                P = sampleResult.P,
                Q = sampleResult.Q,
                Seed = sampleResult.Seed,
                Counter = sampleResult.Count
            };

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
