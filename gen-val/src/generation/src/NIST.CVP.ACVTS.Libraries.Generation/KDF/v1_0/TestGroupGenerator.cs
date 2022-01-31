using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                capability.SupportedLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.SupportedLengths = capability.SupportedLengths.GetDeepCopy();

                foreach (var mac in capability.MacMode)
                {
                    var macOutputLength = MacHelper.GetMacOutputLength(mac);

                    var (keyLen, ivLen) = GetKeyInIvLen(mac, capability);

                    foreach (var counterLength in capability.CounterLength)
                    {
                        foreach (var counterOrder in capability.FixedDataOrder)
                        {
                            // If counter length is 0, only do the 'none', otherwise, skip the 'none'
                            if (counterLength == 0)
                            {
                                if (counterOrder != CounterLocations.None)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (counterOrder == CounterLocations.None)
                                {
                                    continue;
                                }
                            }

                            var testOutputLengths = new List<int>();
                            testOutputLengths.AddRange(capability.SupportedLengths.GetDomainMinMaxAsEnumerable());
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => true, 2, true));
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => w % macOutputLength == 0, 2, true));
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => w % macOutputLength != 0 && w > macOutputLength, 2, true));
                            testOutputLengths = testOutputLengths.Distinct().ToList();

                            foreach (var outputLen in testOutputLengths)
                            {
                                // If an empty IV is allowed while the KDF is Feedback
                                if ((capability.SupportsEmptyIv || capability.RequiresEmptyIv) && capability.KdfMode == KdfModes.Feedback)
                                {
                                    var testGroupNoIv = new TestGroup
                                    {
                                        IvLength = ivLen,
                                        KeyInLength = keyLen,
                                        KdfMode = capability.KdfMode,
                                        MacMode = mac,
                                        CounterLength = counterLength,
                                        CounterLocation = counterOrder,
                                        KeyOutLength = outputLen,
                                        ZeroLengthIv = true,
                                        TestType = "AFT"
                                    };

                                    testGroups.Add(testGroupNoIv);
                                }

                                // If the KDF is not Feedback, OR if the IUT does not require an empty IV
                                if (!capability.RequiresEmptyIv || capability.KdfMode != KdfModes.Feedback)
                                {
                                    var testGroup = new TestGroup
                                    {
                                        IvLength = ivLen,
                                        KeyInLength = keyLen,
                                        KdfMode = capability.KdfMode,
                                        MacMode = mac,
                                        CounterLength = counterLength,
                                        CounterLocation = counterOrder,
                                        KeyOutLength = outputLen,
                                        ZeroLengthIv = false,
                                        TestType = "AFT"
                                    };

                                    testGroups.Add(testGroup);
                                }
                            }
                        }
                    }
                }
            }

            return Task.FromResult(testGroups);
        }

        private static (int KeyLen, int IvLen) GetKeyInIvLen(MacModes mac, Capability capability)
        {
            int keyLen = 0;
            int ivLen = 0;

            switch (mac)
            {
                case MacModes.HMAC_SHA1:
                    keyLen = 160;
                    ivLen = 160;
                    break;

                case MacModes.HMAC_SHA224:
                case MacModes.HMAC_SHA_d512t224:
                case MacModes.HMAC_SHA3_224:
                    keyLen = 224;
                    ivLen = 224;
                    break;

                case MacModes.HMAC_SHA256:
                case MacModes.HMAC_SHA_d512t256:
                case MacModes.HMAC_SHA3_256:
                    keyLen = 256;
                    ivLen = 256;
                    break;

                case MacModes.HMAC_SHA384:
                case MacModes.HMAC_SHA3_384:
                    keyLen = 384;
                    ivLen = 384;
                    break;

                case MacModes.HMAC_SHA512:
                case MacModes.HMAC_SHA3_512:
                    keyLen = 512;
                    ivLen = 512;
                    break;

                case MacModes.CMAC_AES128:
                    keyLen = 128;
                    ivLen = 128;
                    break;

                case MacModes.CMAC_AES192:
                    keyLen = 192;
                    ivLen = 128;
                    break;

                case MacModes.CMAC_AES256:
                    keyLen = 256;
                    ivLen = 128;
                    break;

                case MacModes.CMAC_TDES:
                    keyLen = 192;
                    ivLen = 64;
                    break;
            }

            if (capability.CustomKeyInLength != 0)
            {
                keyLen = capability.CustomKeyInLength;
            }

            if (capability.RequiresEmptyIv)
            {
                ivLen = 0;
            }

            return (keyLen, ivLen);
        }
    }
}
