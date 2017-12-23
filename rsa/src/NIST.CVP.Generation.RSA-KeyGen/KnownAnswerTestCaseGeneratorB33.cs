using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class KnownAnswerTestCaseGeneratorB33 : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly List<AlgoArrayResponse> _kats = new List<AlgoArrayResponse>();
        private int _katsIndex = 0;

        public KnownAnswerTestCaseGeneratorB33(TestGroup testGroup)
        {
            _kats = KATData.GetKATsForProperties(testGroup.Modulo, testGroup.PrimeTest);

            if (_kats == null)
            {
                throw new ArgumentException($"Invalid {nameof(testGroup.Modulo)} of {testGroup.Modulo} or {nameof(testGroup.PrimeTest)} of {testGroup.PrimeTest})");
            }
        }

        public int NumberOfTestCasesToGenerate => _kats.Count;

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            TestCase testCase = new TestCase();
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (_katsIndex + 1 > _kats.Count)
            {
                return new TestCaseGenerateResponse("No additional KATs exist.");
            }

            var currentKat = _kats[_katsIndex++];

            testCase.Key = new KeyPair(
                currentKat.P.ToPositiveBigInteger(), 
                currentKat.Q.ToPositiveBigInteger(),
                currentKat.E.ToPositiveBigInteger()
            );
            testCase.FailureTest = currentKat.FailureTest;
            
            return new TestCaseGenerateResponse(testCase);
        }
    }
}
