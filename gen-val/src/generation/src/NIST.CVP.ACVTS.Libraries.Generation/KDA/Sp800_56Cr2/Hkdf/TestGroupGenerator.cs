using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
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
            var algoMode =
                AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
            bool usesHybridSS = false;
            List<int> zLengths;
            List<int> tLengths = new List<int>(){};
            int zLength;
            int tLength;

            if (algoMode == AlgoMode.KDA_HKDF_Sp800_56Cr2)
            {
                usesHybridSS = parameters.UsesHybridSharedSecret.Value;
            }
            
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
                            
                            if (usesHybridSS)
                            {
                                tLengths = GetSSLens(parameters.AuxSharedSecretLen.GetDeepCopy());
                                if (tLengths.Count > numSSLens)
                                {
                                    numSSLens = tLengths.Count;
                                }
                            }
                            
                            for (int i = 0; i < numSSLens; i++)
                            {
                                // tLength should default to 0
                                tLength = 0; 
                                // If we're dealing w/ 56Cr1 or if it's 56Cr2 w/ !usesHybridSS, then we don't need to factor in/worry about t
                                if (algoMode == AlgoMode.KDA_HKDF_Sp800_56Cr1 || (algoMode == AlgoMode.KDA_HKDF_Sp800_56Cr2 && !usesHybridSS))
                                {
                                    zLength = zLengths[i];
                                } 
                                else // if 56Cr2 and usesHybridSS is true, then we could be in a scenario where the  
                                { // number of zLengths is less than the number of tLengths and we'll need to reuse a
                                  // zLength 1 or more times or vice versus
                                    zLength = i < zLengths.Count ? zLengths[i] : zLengths[zLengths.Count-1]; 
                                    tLength = i < tLengths.Count ? tLengths[i] : tLengths[tLengths.Count-1];
                                }

                                if (algoMode == AlgoMode.KDA_HKDF_Sp800_56Cr1)
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
                                        ZLength = zLength
                                    });                                    
                                }
                                else
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
                                        UsesHybridSharedSecret = parameters.UsesHybridSharedSecret,
                                        AuxSharedSecretLen = tLength,
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
                                            UsesHybridSharedSecret = parameters.UsesHybridSharedSecret,
                                            AuxSharedSecretLen = tLength,
                                            MultiExpansion = true
                                        });
                                    } 
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
