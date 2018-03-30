using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorKat : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private int _katsIndex;

        public int NumberOfTestCasesToGenerate => _kats.Count;

        public TestCaseGeneratorKat(TestGroup testGroup, IKeyComposerFactory keyComposerFactory)
        {
            _kats = KatData.GetKatsForProperties(testGroup.Modulo, testGroup.PrimeTest);
            _keyComposerFactory = keyComposerFactory;

            if (_kats == null)
            {
                throw new ArgumentException($"Invalid {nameof(testGroup.Modulo)} of {testGroup.Modulo} or {nameof(testGroup.PrimeTest)} of {testGroup.PrimeTest})");
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase();
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return new TestCaseGenerateResponse("No additional KATs exist.");
            }

            var currentKat = _kats[_katsIndex++];

            var keyComposer = _keyComposerFactory.GetKeyComposer(group.KeyFormat);
            var primePair = new PrimePair
            {
                P = currentKat.P.ToPositiveBigInteger(),
                Q = currentKat.Q.ToPositiveBigInteger()
            };

            testCase.Key = keyComposer.ComposeKey(currentKat.E.ToPositiveBigInteger(), primePair);
            testCase.FailureTest = currentKat.FailureTest;
            
            return new TestCaseGenerateResponse(testCase);
        }
    }
}
