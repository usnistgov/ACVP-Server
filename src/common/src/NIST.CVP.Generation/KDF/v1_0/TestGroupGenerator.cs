using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.KDF.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Generation.KDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                capability.SupportedLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);

                foreach (var mac in capability.MacMode)
                {
                    var macOutputLength = MacHelper.GetMacOutputLength(mac);

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
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => w % macOutputLength == 0, 2, true));
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => w % macOutputLength != 0 && w > macOutputLength, 2, true));

                            foreach (var outputLen in testOutputLengths)
                            {
                                var testGroup = new TestGroup
                                {
                                    KdfMode = capability.KdfMode,
                                    MacMode = mac,
                                    CounterLength = counterLength,
                                    CounterLocation = counterOrder,
                                    KeyOutLength = outputLen,
                                    ZeroLengthIv = false,
                                    TestType = "AFT"
                                };

                                testGroups.Add(testGroup);

                                // Only Feedback has an IV
                                if (capability.SupportsEmptyIv && capability.KdfMode == KdfModes.Feedback)
                                {
                                    var testGroupNoIv = new TestGroup
                                    {
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
                            }
                        }
                    }
                }
            }

            return testGroups;
        }
    }
}
