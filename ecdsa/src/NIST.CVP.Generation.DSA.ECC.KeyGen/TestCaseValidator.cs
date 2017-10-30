using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDsaEcc _eccDsa;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidator(TestCase expectedResult, TestGroup group, IDsaEcc dsaEcc)
        {
            _expectedResult = expectedResult;
            _group = group;
            _eccDsa = dsaEcc;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (suppliedResult.KeyPair.PrivateD == 0 || suppliedResult.KeyPair.PublicQ.X == 0 || suppliedResult.KeyPair.PublicQ.Y == 0)
            {
                errors.Add("Could not find value in key pair");
            }
            else
            {
                _eccDsa.AddEntropy(suppliedResult.KeyPair.PrivateD);
                var generateResult = _eccDsa.GenerateKeyPair(_group.DomainParameters);
                if (!generateResult.Success)
                {
                    errors.Add($"Unable to generate public key from private key d value");
                }
                else
                {
                    if (generateResult.KeyPair.PublicQ.X != suppliedResult.KeyPair.PublicQ.X)
                    {
                        errors.Add($"Incorrect Qx generated from private key");
                    }

                    if (generateResult.KeyPair.PublicQ.Y != suppliedResult.KeyPair.PublicQ.Y)
                    {
                        errors.Add($"Incorrect Qy generated from private key");
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Failed, Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }
    }
}
