using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestGroup _group;
        private RandomProbablePrimeGenerator _primeGen;
        public int TestCaseId { get; }

        public TestCaseValidatorGDT(TestCase expectedResult, TestGroup group)
        {
            _group = group;
            TestCaseId = expectedResult.TestCaseId;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if(suppliedResult.Key != null)
            {
                _primeGen = new RandomProbablePrimeGenerator(EntropyProviderTypes.Testable);
                _primeGen.AddEntropy(new BitString(suppliedResult.Key.PrivKey.P, _group.Modulo / 2));
                _primeGen.AddEntropy(new BitString(suppliedResult.Key.PrivKey.Q, _group.Modulo / 2));

                var genResult = _primeGen.GeneratePrimes(_group.Modulo, suppliedResult.Key.PubKey.E, null);

                // Check p, q for compsiteness (if the generator failed then at least one is composite)
                if (!genResult.Success)
                {
                    errors.Add(genResult.ErrorMessage);
                }

                // Check n, d for equality with expected values
                var expectedKeyPair = new KeyPair(genResult.P, genResult.Q, suppliedResult.Key.PubKey.E);

                if (suppliedResult.Key.PubKey.N != expectedKeyPair.PubKey.N)
                {
                    errors.Add("N value is incorrect");
                }

                // Be aware of CRT
                if (suppliedResult.Key.PrivKey.D == 0)
                {
                    if (suppliedResult.Key.PrivKey.DMP1 != expectedKeyPair.PrivKey.DMP1)
                    {
                        errors.Add("DMP1 value is incorrect");
                    }

                    if (suppliedResult.Key.PrivKey.DMQ1 != expectedKeyPair.PrivKey.DMQ1)
                    {
                        errors.Add("DMQ1 value is incorrect");
                    }

                    if (suppliedResult.Key.PrivKey.IQMP != expectedKeyPair.PrivKey.IQMP)
                    {
                        errors.Add("IQMP value is incorrect");
                    }
                }
                else
                {
                    if (suppliedResult.Key.PrivKey.D != expectedKeyPair.PrivKey.D)
                    {
                        errors.Add("D value is incorrect");
                    }
                }
            }
            else
            {
                errors.Add("No key found");
            }

           
            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join(";", errors)};
            }
            
            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = "passed"};
        }
    }
}
