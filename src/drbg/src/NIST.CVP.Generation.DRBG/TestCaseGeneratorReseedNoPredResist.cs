﻿using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DRBG
{
    /// <summary>
    /// Reseed flow w/o Prediction Resistance:
    ///    Instantiate -> Reseed -> Generate -> Generate 
    /// </summary>
    public class TestCaseGeneratorReseedNoPredResist : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IEntropyProviderFactory _iEntropyProviderFactory;
        private readonly IDrbgFactory _iDrbgFactory;

        public TestCaseGeneratorReseedNoPredResist(IEntropyProviderFactory iEntropyProviderFactory, IDrbgFactory iDrbgFactory)
        {
            _iEntropyProviderFactory = iEntropyProviderFactory;
            _iDrbgFactory = iDrbgFactory;
        }

        public int NumberOfTestCasesToGenerate => 15;

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            var randomEntropyProvider = _iEntropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);

            TestCase testCase = new TestCase
            {
                EntropyInput = randomEntropyProvider.GetEntropy(@group.EntropyInputLen),
                Nonce = randomEntropyProvider.GetEntropy(@group.NonceLen),
                PersoString = randomEntropyProvider.GetEntropy(@group.PersoStringLen)
            };
            
            // Reseed - needs entropy and additional input
            AddOtherInput(group, randomEntropyProvider, testCase, true);

            // Gen 1 - needs additional input
            AddOtherInput(group, randomEntropyProvider, testCase, false);

            // Gen 2 - needs additional input
            AddOtherInput(group, randomEntropyProvider, testCase, false);

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            var testableEntropyProvider = _iEntropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable) as TestableEntropyProvider;
            SetupTestableEntropy(testCase, testableEntropyProvider);

            var drbg = _iDrbgFactory.GetDrbgInstance(group.DrbgParameters, testableEntropyProvider);

            drbg.Instantiate(group.DrbgParameters.SecurityStrength, testCase.PersoString);

            bool needsReseed = true;
            foreach (var item in testCase.OtherInput)
            {
                if (needsReseed)
                {
                    needsReseed = false;
                    var reseed = drbg.Reseed(item.AdditionalInput);

                    if (reseed != DrbgStatus.Success)
                    {
                        return new TestCaseGenerateResponse<TestGroup, TestCase>(reseed.ToString());
                    }
                    continue;
                }

                var result = drbg.Generate(group.ReturnedBitsLen, item.AdditionalInput);
                if (!result.Success)
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(result.DrbgStatus.ToString());
                }

                testCase.ReturnedBits = result.Bits.GetDeepCopy();
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private void SetupTestableEntropy(TestCase testCase, TestableEntropyProvider testableEntropyProvider)
        {
            testableEntropyProvider.AddEntropy(testCase.EntropyInput);
            testableEntropyProvider.AddEntropy(testCase.Nonce);
            foreach (var entropy in testCase.OtherInput)
            {
                testableEntropyProvider.AddEntropy(entropy.EntropyInput);
            }
        }

        private void AddOtherInput(TestGroup group, IEntropyProvider randomEntropyProvider, TestCase tc, bool needsEntropyInput)
        {
            tc.OtherInput.Add(
                new OtherInput()
                {
                    // We only need additional Entropy input in some situations (Reseed, but not generate)
                    EntropyInput = needsEntropyInput ? 
                        randomEntropyProvider.GetEntropy(group.EntropyInputLen) : 
                        new BitString(0),
                    AdditionalInput = randomEntropyProvider.GetEntropy(group.AdditionalInputLen)
                }
            );
        }
    }
}