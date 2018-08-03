using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorKat : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponseKey> _kats;
        private readonly IOracle _oracle;
        private int _katsIndex;

        public int NumberOfTestCasesToGenerate => _kats.Count;

        public TestCaseGeneratorKat(TestGroup testGroup, IOracle oracle)
        {
            _kats = KatData.GetKatsForProperties(testGroup.Modulo, testGroup.PrimeTest);
            _oracle = oracle;

            if (_kats == null)
            {
                throw new ArgumentException($"Invalid {nameof(testGroup.Modulo)} of {testGroup.Modulo} or {nameof(testGroup.PrimeTest)} of {testGroup.PrimeTest})");
            }
        }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist.");
            }

            var currentKat = _kats[_katsIndex++];

            var keyResult = new RsaKeyResult
            {
                Key = new KeyPair
                {
                    PrivKey = new PrivateKey
                    {
                        P = currentKat.P.ToPositiveBigInteger(),
                        Q = currentKat.Q.ToPositiveBigInteger()
                    },
                    PubKey = new PublicKey
                    {
                        E = currentKat.E.ToPositiveBigInteger()
                    }
                }
            };

            var result = await _oracle.CompleteKeyAsync(keyResult, group.KeyFormat);

            TestCase testCase = new TestCase
            {
                Key = result.Key,
                TestPassed = !currentKat.FailureTest
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }
    }
}
