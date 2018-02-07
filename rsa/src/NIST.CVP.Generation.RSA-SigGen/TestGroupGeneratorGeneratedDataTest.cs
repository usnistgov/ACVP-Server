using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.RSA2.Enums;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroupGeneratorGeneratedDataTest : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var sigType = capability.SigType;

                foreach (var moduloCap in capability.ModuloCapabilities)
                {
                    var modulo = moduloCap.Modulo;

                    foreach (var hashPair in moduloCap.HashPairs)
                    {
                        var testGroup = new TestGroup
                        {
                            Mode = EnumHelpers.GetEnumFromEnumDescription<SignatureSchemes>(sigType),
                            Modulo = modulo,
                            HashAlg = ShaAttributes.GetHashFunctionFromName(hashPair.HashAlg),
                            SaltLen = hashPair.SaltLen,

                            TestType = TEST_TYPE
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
