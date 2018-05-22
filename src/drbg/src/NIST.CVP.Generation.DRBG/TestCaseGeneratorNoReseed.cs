using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DRBG
{
    /// <summary>
    /// No Reseed flow 
    ///    Instantiate -> Generate -> Generate
    /// </summary>
    public class TestCaseGeneratorNoReseed : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IEntropyProviderFactory _iEntropyProviderFactory;
        private readonly IDrbgFactory _iDrbgFactory;

        public TestCaseGeneratorNoReseed(IEntropyProviderFactory iEntropyProviderFactory, IDrbgFactory iDrbgFactory)
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

            // Gen
            AddOtherInput(group, randomEntropyProvider, testCase);

            // Gen
            AddOtherInput(group, randomEntropyProvider, testCase);

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            var testableEntropyProvider = _iEntropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable) as TestableEntropyProvider;
            SetupTestableEntropy(testCase, testableEntropyProvider);

            var drbg = _iDrbgFactory.GetDrbgInstance(group.DrbgParameters, testableEntropyProvider);

            drbg.Instantiate(group.DrbgParameters.SecurityStrength, testCase.PersoString);

            foreach (var item in testCase.OtherInput)
            {
                var result = drbg.Generate(group.ReturnedBitsLen, item.AdditionalInput);
                if (!result.Success)
                {
                    return new TestCaseGenerateResponse<TestGroup, TestCase>(result.DrbgStatus.ToString());
                }

                testCase.ReturnedBits = result.Bits.GetDeepCopy();
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private void AddOtherInput(TestGroup group, IEntropyProvider randomEntropyProvider, TestCase tc)
        {
            tc.OtherInput.Add(
                new OtherInput()
                {
                    // we only want entropy if a PR group
                    EntropyInput = group.PredResistance ? randomEntropyProvider.GetEntropy(group.EntropyInputLen) : new BitString(0),
                    AdditionalInput = randomEntropyProvider.GetEntropy(group.AdditionalInputLen)
                }
            );
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
    }
}
