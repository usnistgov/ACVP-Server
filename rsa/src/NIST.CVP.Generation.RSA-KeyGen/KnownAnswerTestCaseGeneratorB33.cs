using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class KnownAnswerTestCaseGeneratorB33 : IKnownAnswerTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            var data = KATData.GetKATsForProperties(testGroup.Modulo, testGroup.PrimeTest);
            if (data == null)
            {
                return
                    new MultipleTestCaseGenerateResponse<TestCase>(
                        $"Invalid {nameof(testGroup.Modulo)} of {testGroup.Modulo} or {nameof(testGroup.PrimeTest)} of {testGroup.PrimeTest})");
            }

            var testCases = data.Select(s => new TestCase
            {
                Key = new KeyPair(s.P.ToPositiveBigInteger(), s.Q.ToPositiveBigInteger(), s.E.ToPositiveBigInteger()),
                FailureTest = s.FailureTest
            }).ToList();

            return new MultipleTestCaseGenerateResponse<TestCase>(testCases);
        }
    }
}
