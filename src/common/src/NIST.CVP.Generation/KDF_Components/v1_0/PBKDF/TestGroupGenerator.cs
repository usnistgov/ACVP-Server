using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            return parameters.HashAlg.Select(hashAlg => new TestGroup
                {
                    IterationCount = parameters.IterationCount,
                    KeyLength = parameters.KeyLength,
                    PasswordLength = parameters.PasswordLength,
                    SaltLength = parameters.SaltLength,
                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg)
                })
                .ToList();
        }
    }
}