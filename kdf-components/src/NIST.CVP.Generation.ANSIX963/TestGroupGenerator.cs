using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper.Helpers;

namespace NIST.CVP.Generation.ANSIX963
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new HashSet<TestGroup>();
            foreach (var hashAlg in parameters.HashAlg)
            {
                var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);

                foreach (var sharedInfo in parameters.SharedInfoLength.GetDomainMinMaxAsEnumerable())
                {
                    foreach (var keyDataLength in parameters.KeyDataLength.GetDomainMinMaxAsEnumerable())
                    {
                        foreach (var fieldSize in parameters.FieldSize)
                        {
                            // Only allow combinations such that the hash function matches (or exceeds) the security of the field size
                            // So for field size 283, you can use SHA2-256, SHA2-384 and SHA2-512 but not SHA2-224
                            if (fieldSize > 512)
                            {
                                if (hashFunction.OutputLen != 512)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (hashFunction.OutputLen < fieldSize)
                                {
                                    continue;
                                }
                            }

                            testGroups.Add(new TestGroup
                            {
                                HashAlg = hashFunction,
                                SharedInfoLength = sharedInfo,
                                KeyDataLength = keyDataLength,
                                FieldSize = fieldSize
                            });
                        }
                    }
                }
            }
            
            return testGroups;
        }
    }
}
