using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.TwoStep
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _testTypes = { "AFT", "VAL" };

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var testType in _testTypes)
            {
                foreach (var kdfConfig in GetKdfConfiguration(parameters))
                {
                    foreach (var zLength in GetZs(parameters.Z.GetDeepCopy()))
                    {
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
                            ZLength = zLength,
                            MultiExpansion = false,
                        });

                        // Create groups for multi expansion using more or less the same options
                        if (parameters.PerformMultiExpansionTests)
                        {
                            groups.Add(new TestGroup()
                            {
                                KdfMultiExpansionConfiguration = new TwoStepMultiExpansionConfiguration()
                                {
                                    L = parameters.L,
                                    CounterLen = kdfConfig.CounterLen,
                                    CounterLocation = kdfConfig.CounterLocation,
                                    IvLen = kdfConfig.IvLen,
                                    KdfMode = kdfConfig.KdfMode,
                                    MacMode = kdfConfig.MacMode,
                                    SaltLen = kdfConfig.SaltLen,
                                    SaltMethod = kdfConfig.SaltMethod,
                                },
                                TestType = testType,
                                IsSample = parameters.IsSample,
                                ZLength = zLength,
                                MultiExpansion = true,
                            });
                        }
                    }
                }
            }

            return Task.FromResult(groups);
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
                                            saltLen = 160;
                                            ivLen = 160;
                                            break;
                                        case MacModes.HMAC_SHA224:
                                        case MacModes.HMAC_SHA3_224:
                                            saltLen = 224;
                                            ivLen = 224;
                                            break;
                                        case MacModes.HMAC_SHA256:
                                        case MacModes.HMAC_SHA3_256:
                                            saltLen = 256;
                                            ivLen = 256;
                                            break;
                                        case MacModes.HMAC_SHA384:
                                        case MacModes.HMAC_SHA3_384:
                                            saltLen = 384;
                                            ivLen = 384;
                                            break;
                                        case MacModes.HMAC_SHA512:
                                        case MacModes.HMAC_SHA3_512:
                                            saltLen = 512;
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
