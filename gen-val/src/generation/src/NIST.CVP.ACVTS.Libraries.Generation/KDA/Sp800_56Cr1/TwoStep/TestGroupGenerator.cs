using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared.TwoStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr1.TwoStep
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

            foreach (var testType in _testTypes)
            {
                foreach (var kdfConfig in GetKdfConfiguration(parameters))
                {
                    zLengths = GetSSLens(parameters.Z.GetDeepCopy());
                    int numSSLens = zLengths.Count;

                    for (int i = 0; i < numSSLens; i++)
                    {
                        // tLength should default to 0
                        tLength = 0; 
                        zLength = zLengths[i];

                        groups.Add(new TestGroup()
                        {
                            KdfConfiguration = new TwoStepConfiguration()
                            {
                                L = parameters.L,
                                CounterLen = kdfConfig.CounterLen,
                                CounterLocation = kdfConfig.CounterLocation,
                                IvLen = kdfConfig.IvLen,
                                KdfMode = kdfConfig.KdfMode,
                                MacMode = kdfConfig.MacMode,
                                SaltLen = kdfConfig.SaltLen,
                                SaltMethod = kdfConfig.SaltMethod,
                                FixedInfoEncoding = kdfConfig.FixedInfoEncoding,
                                FixedInfoPattern = kdfConfig.FixedInfoPattern
                            },
                            TestType = testType,
                            IsSample = parameters.IsSample,
                            ZLength = zLength
                        });
                    }
                }
            }

            return Task.FromResult(groups);
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

        private List<TwoStepConfiguration> GetKdfConfiguration(Parameters parameters)
        {
            var list = new List<TwoStepConfiguration>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var encoding in capability.Encoding)
                {
                    foreach (var saltMethod in capability.MacSaltMethods)
                    {
                        foreach (var counterLen in capability.CounterLength)
                        {
                            foreach (var fixedDataOrder in capability.FixedDataOrder)
                            {
                                foreach (var mac in GetCapabilityMacMode(capability))
                                {
                                    // If counter length is 0, only do the 'none', otherwise, skip the 'none'
                                    if (counterLen == 0)
                                    {
                                        if (fixedDataOrder != CounterLocations.None)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (fixedDataOrder == CounterLocations.None)
                                        {
                                            continue;
                                        }
                                    }

                                    // Cannot generate groups when the counter is in the "middle".
                                    if (fixedDataOrder == CounterLocations.MiddleFixedData)
                                    {
                                        continue;
                                    }

                                    var saltLen = 0;
                                    var ivLen = 0;
                                    switch (mac)
                                    {
                                        case MacModes.CMAC_AES128:
                                            saltLen = 128;
                                            ivLen = 128;
                                            break;
                                        case MacModes.CMAC_AES192:
                                            saltLen = 192;
                                            ivLen = 128;
                                            break;
                                        case MacModes.CMAC_AES256:
                                            saltLen = 256;
                                            ivLen = 128;
                                            break;
                                        case MacModes.CMAC_TDES:
                                            continue;
                                        case MacModes.HMAC_SHA1:
                                            saltLen = 512;
                                            ivLen = 160;
                                            break;
                                        case MacModes.HMAC_SHA224:
                                            saltLen = 512;
                                            ivLen = 224;
                                            break;
                                        case MacModes.HMAC_SHA_d512t224:
                                            saltLen = 1024;
                                            ivLen = 224;
                                            break;
                                        case MacModes.HMAC_SHA3_224:
                                            saltLen = 1152;
                                            ivLen = 224;
                                            break;
                                        case MacModes.HMAC_SHA256:
                                            saltLen = 512;
                                            ivLen = 256;
                                            break;
                                        case MacModes.HMAC_SHA_d512t256:
                                            saltLen = 1024;
                                            ivLen = 256;
                                            break;
                                        case MacModes.HMAC_SHA3_256:
                                            saltLen = 1088;
                                            ivLen = 256;
                                            break;
                                        case MacModes.HMAC_SHA384:
                                            saltLen = 1024;
                                            ivLen = 384;
                                            break;
                                        case MacModes.HMAC_SHA3_384:
                                            saltLen = 832;
                                            ivLen = 384;
                                            break;
                                        case MacModes.HMAC_SHA512:
                                            saltLen = 1024;
                                            ivLen = 512;
                                            break;
                                        case MacModes.HMAC_SHA3_512:
                                            saltLen = 576;
                                            ivLen = 512;
                                            break;
                                    }

                                    if (capability.KdfMode != KdfModes.Feedback ||
                                        (capability.KdfMode == KdfModes.Feedback && (capability.RequiresEmptyIv)))
                                    {
                                        ivLen = 0;
                                    }

                                    list.Add(new TwoStepConfiguration()
                                    {
                                        L = parameters.L,
                                        SaltLen = saltLen,
                                        FixedInfoEncoding = encoding,
                                        FixedInfoPattern = capability.FixedInfoPattern,
                                        SaltMethod = saltMethod,
                                        KdfMode = capability.KdfMode,
                                        MacMode = mac,
                                        CounterLocation = fixedDataOrder,
                                        CounterLen = counterLen,
                                        IvLen = ivLen
                                    });

                                    // Only Feedback has an IV, so add additional group of 0 len iv when feedback group supports it,
                                    // and it's not required (as that would have been covered in the above add.
                                    if (capability.SupportsEmptyIv && !capability.RequiresEmptyIv && capability.KdfMode == KdfModes.Feedback)
                                    {
                                        list.Add(new TwoStepConfiguration()
                                        {
                                            L = parameters.L,
                                            SaltLen = saltLen,
                                            FixedInfoEncoding = encoding,
                                            FixedInfoPattern = capability.FixedInfoPattern,
                                            SaltMethod = saltMethod,
                                            KdfMode = capability.KdfMode,
                                            MacMode = mac,
                                            CounterLocation = fixedDataOrder,
                                            CounterLen = counterLen,
                                            IvLen = 0
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        private MacModes[] GetCapabilityMacMode(TwoStepCapabilities capability)
        {
            var hmacSha2 = new[]
            {
                MacModes.HMAC_SHA224,
                MacModes.HMAC_SHA256,
                MacModes.HMAC_SHA384,
                MacModes.HMAC_SHA512,
                MacModes.HMAC_SHA_d512t224,
                MacModes.HMAC_SHA_d512t256,
            };
            var hmacSha3 = new[]
            {
                MacModes.HMAC_SHA3_224,
                MacModes.HMAC_SHA3_256,
                MacModes.HMAC_SHA3_384,
                MacModes.HMAC_SHA3_512,
            };
            var cmac = new[]
            {
                MacModes.CMAC_AES128,
                MacModes.CMAC_AES192,
                MacModes.CMAC_AES256,
            };

            var registeredMacMethods = capability.MacMode.ToList().Shuffle();

            var chosenMacModes = new List<MacModes>();

            var chosenHmacSha2 = registeredMacMethods.FirstOrDefault(f => hmacSha2.Contains(f));
            if (chosenHmacSha2 != MacModes.None)
            {
                chosenMacModes.Add(chosenHmacSha2);
            }

            var chosenHmacSha3 = registeredMacMethods.FirstOrDefault(f => hmacSha3.Contains(f));
            if (chosenHmacSha3 != MacModes.None)
            {
                chosenMacModes.Add(chosenHmacSha3);
            }

            var chosenCmac = registeredMacMethods.FirstOrDefault(f => cmac.Contains(f));
            if (chosenCmac != MacModes.None)
            {
                chosenMacModes.Add(chosenCmac);
            }

            return chosenMacModes.ToArray();
        }
    }
}
