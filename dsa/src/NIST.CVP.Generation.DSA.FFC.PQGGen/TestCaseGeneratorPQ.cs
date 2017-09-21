using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
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
                sampleResult = _pqGen.Generate(group.L, group.N, group.N);
                if (!sampleResult.Success)
                {
                    ThisLogger.Warn($"Error generating sample: {sampleResult.ErrorMessage}");
                    return new TestCaseGenerateResponse(sampleResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating sample: {sampleResult.ErrorMessage}");
                return new TestCaseGenerateResponse($"Exception generating sample: {sampleResult.ErrorMessage}");
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

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
