using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.Hkdf
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _testTypes = { "AFT", "VAL" };

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var testType in _testTypes)
            {
                foreach (var fixedInfoEncoding in parameters.Encoding)
                {
                    foreach (var hmacAlg in parameters.HmacAlg)
                    {
                        foreach (var saltMethod in parameters.MacSaltMethods)
                        {
                            foreach (var zLength in GetZs(parameters.Z.GetDeepCopy()))
                            {
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
                                    ZLength = zLength,
                                    MultiExpansion = false
                                });

                                // Create groups for multi expansion using more or less the same options
                                if (parameters.PerformMultiExpansionTests)
                                {
                                    groups.Add(new TestGroup()
                                    {
                                        KdfMultiExpansionConfiguration = new HkdfMultiExpansionConfiguration()
                                        {
                                            HmacAlg = hmacAlg,
                                            SaltMethod = saltMethod,
                                            SaltLen = GetSaltLen(hmacAlg),
                                            L = parameters.L
                                        },
                                        TestType = testType,
                                        IsSample = parameters.IsSample,
                                        ZLength = zLength,
                                        MultiExpansion = true
                                    });
                                }
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

        private List<int> GetZs(MathDomain z)
        {
            var values = new List<int>();

            values.AddRange(z.GetValues(i => i < 1024, 10, false));
            values.AddRange(z.GetValues(i => i < 4098, 5, false));
            values.AddRange(z.GetValues(i => i < 8196, 2, false));
            values.AddRange(z.GetValues(1));

            return values.Shuffle().Take(5).ToList();
        }
    }
}
