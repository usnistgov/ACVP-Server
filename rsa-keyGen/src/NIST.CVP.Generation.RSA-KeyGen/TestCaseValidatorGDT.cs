using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestGroup _group;
        public int TestCaseId { get; }

        public TestCaseValidatorGDT(TestCase expectedResult, TestGroup group)
        {
            _group = group;
            TestCaseId = expectedResult.TestCaseId;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            // Check p, q for PROBABLY PRIME
            if (!MillerRabin(suppliedResult.Key.PrivKey.P))
            {
                errors.Add("P is COMPOSITE");
            }

            if (!MillerRabin(suppliedResult.Key.PrivKey.Q))
            {
                errors.Add("Q is COMPOSITE");
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join(";", errors)};
            }
            
            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = "passed"};
        }

        private bool MillerRabin(BigInteger val)
        {
            if (_group.Modulo == 2048)
            {
                if (_group.PrimeTest == PrimeTestModes.C2)
                {
                    return NumberTheory.MillerRabin(val, 5);
                }
                else // if (_primeTestMode == PrimeTestModes.C3)
                {
                    return NumberTheory.MillerRabin(val, 4);
                }
            }
            else // if(nlen == 3072)
            {
                if (_group.PrimeTest == PrimeTestModes.C2)
                {
                    return NumberTheory.MillerRabin(val, 4);
                }
                else // if (_primeTestMode == PrimeTestModes.C3)
                {
                    return NumberTheory.MillerRabin(val, 3);
                }
            }
        }
    }
}
