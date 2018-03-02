using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDsaEccFactory _dsaEccFactory;
        private IDsaEcc _eccDsa;
        private readonly IEccCurveFactory _curveFactory;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult, TestGroup group, IDsaEccFactory dsaEccFactory, IEccCurveFactory curveFactory)
        {
            _expectedResult = expectedResult;
            _group = group;
            _dsaEccFactory = dsaEccFactory;
            _curveFactory = curveFactory;
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
                // TODO move to deferred
                // No hash function
                _eccDsa = _dsaEccFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256), EntropyProviderTypes.Testable);
                _eccDsa.AddEntropy(suppliedResult.KeyPair.PrivateD);
                var generateResult = _eccDsa.GenerateKeyPair(new EccDomainParameters(_curveFactory.GetCurve(_group.Curve)));
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
