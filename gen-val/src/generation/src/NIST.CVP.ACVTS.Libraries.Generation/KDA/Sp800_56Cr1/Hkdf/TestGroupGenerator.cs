using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared.Hkdf;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr1.Hkdf
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _testTypes = { "AFT", "VAL" };

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();
            bool usesHybridSS = false;
            List<int> zLengths;
            int zLength;

            foreach (var testType in _testTypes)
            {
                foreach (var fixedInfoEncoding in parameters.Encoding)
                {
                    foreach (var hmacAlg in parameters.HmacAlg)
                    {
                        foreach (var saltMethod in parameters.MacSaltMethods)
                        {
                            zLengths = GetSSLens(parameters.Z.GetDeepCopy());
                            int numSSLens = zLengths.Count;

                            for (int i = 0; i < numSSLens; i++)
                            {
                                // tLength should default to 0
                                // We're dealing w/ 56Cr1, we don't need to factor in/worry about t
                                zLength = zLengths[i];
                                
                                groups.Add(new TestGroup()
                                {
                                    KdfConfiguration = new HkdfConfiguration()
                                    {
                                        L = parameters.L,
                                        HmacAlg = hmacAlg,
                                        SaltMethod = saltMethod,
                                        SaltLen = GetSaltLen(hmacAlg),
                                        FixedInfoEncoding = fixedInfoEncoding,
                                        FixedInfoPattern = parameters.FixedInfoPattern
                                    },
                                    TestType = testType,
                                    IsSample = parameters.IsSample,
                                    ZLength = zLength
                                });
                            }
                        }
                    }
                }
            }

            return Task.FromResult(groups);
        }

        private int GetSaltLen(HashFunctions hmacAlg)
        {
            switch (hmacAlg)
            {
                case HashFunctions.Sha1:
                    return 512;
                case HashFunctions.Sha2_d224:
                    return 512;
                case HashFunctions.Sha2_d512t224:
                    return 1024;
                case HashFunctions.Sha3_d224:
                    return 1152;
                case HashFunctions.Sha2_d256:
                    return 512;
                case HashFunctions.Sha2_d512t256:
                    return 1024;
                case HashFunctions.Sha3_d256:
                    return 1088;
                case HashFunctions.Sha2_d384:
                    return 1024;
                case HashFunctions.Sha3_d384:
                    return 832;
                case HashFunctions.Sha2_d512:
                    return 1024;
                case HashFunctions.Sha3_d512:
                    return 576;
            }

            return 0;
        }

        private List<int> GetSSLens(MathDomain sS)
        {
            var values = new List<int>();

            // Only one shared secret length is supported. Only need one test group
            if (sS.GetDomainMinMax().Minimum == sS.GetDomainMinMax().Maximum)
            {
                values.Add(sS.GetDomainMinMax().Minimum);
            }
            else
            {
                values.AddRange(sS.GetRandomValues(i => i < 1024, 10));
                values.AddRange(sS.GetRandomValues(i => i < 4098, 5));
                values.AddRange(sS.GetRandomValues(i => i < 8196, 2));
                values.AddRange(sS.GetRandomValues(1));

                values = values.Shuffle().Take(3).ToList();
            
                values.Add(sS.GetDomainMinMax().Minimum);
                values.Add(sS.GetDomainMinMax().Maximum);                
            }
            
            return values.Shuffle();
        }
    }
}
