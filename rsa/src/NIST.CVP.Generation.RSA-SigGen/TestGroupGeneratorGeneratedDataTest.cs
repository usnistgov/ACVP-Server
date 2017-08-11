using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroupGeneratorGeneratedDataTest : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach(var mode in parameters.SigGenModes)
            {
                foreach(var modulo in parameters.Moduli)
                {
                    foreach(var capability in parameters.Capabilities)
                    {
                        var testGroup = new TestGroup
                        {
                            Mode = RSAEnumHelpers.StringToSigGenMode(mode),
                            Modulo = modulo,
                            HashAlg = SHAEnumHelpers.StringToHashFunction(capability.HashAlg),
                            SaltLen = capability.SaltLen,
                            
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
