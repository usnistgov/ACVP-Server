using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorFactory : ITestCaseValidatorFactory<TestVectorSet, TestCase>
    {
        public IEnumerable<ITestCaseValidator<TestCase>> GetValidators(TestVectorSet testVectorSet, IEnumerable<TestCase> suppliedResults)
        {
            var list = new List<ITestCaseValidator<TestCase>>();

            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup) g))
            {
                foreach (var test in group.Tests.Select(t => (TestCase) t))
                {
                    SignerBase signer;
                    if(group.Mode == SigGenModes.ANS_931)
                    {
                        signer = new ANS_X931_Signer(group.HashAlg);
                    }
                    else if(group.Mode == SigGenModes.PKCS_v15)
                    {
                        signer = new RSASSA_PKCSv15_Signer(group.HashAlg);
                    }
                    else if(group.Mode == SigGenModes.PSS)
                    {
                        signer = new RSASSA_PSS_Signer(group.HashAlg, EntropyProviderTypes.Testable, group.SaltLen);
                    }
                    else
                    {
                        throw new Exception("Cannot find Signer");
                    }

                    if (group.TestType.ToLower() == "gdt")
                    {
                        list.Add(new TestCaseValidatorGDT(test, group, signer));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorNull($"Could not determine TestType for TestCase", test.TestCaseId));
                    }
                }
            }

            return list;
        }
    }
}
