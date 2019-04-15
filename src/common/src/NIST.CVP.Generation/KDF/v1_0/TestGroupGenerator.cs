using System;
using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IKdfFactory _kdfFactory;

        public TestGroupGenerator(IKdfFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                capability.SupportedLengths.SetRangeOptions(RangeDomainSegmentOptions.Random);
                var kdfMode = EnumHelpers.GetEnumFromEnumDescription<KdfModes>(capability.KdfMode);

                foreach (var mac in capability.MacMode)
                {
                    var macMode = EnumHelpers.GetEnumFromEnumDescription<MacModes>(mac);
                    var macOutputLength = _kdfFactory.GetMacInstance(macMode).OutputLength;

                    foreach (var counterLength in capability.CounterLength)
                    {
                        foreach (var counterOrder in capability.FixedDataOrder)
                        {
                            // If counter length is 0, only do the 'none', otherwise, skip the 'none'
                            if (counterLength == 0)
                            {
                                if (!counterOrder.Equals("none", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                if (counterOrder.Equals("none", StringComparison.OrdinalIgnoreCase))
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
                                    KdfMode = kdfMode,
                                    MacMode = macMode,
                                    CounterLength = counterLength,
                                    CounterLocation = EnumHelpers.GetEnumFromEnumDescription<CounterLocations>(counterOrder),
                                    KeyOutLength = outputLen,
                                    ZeroLengthIv = false,
                                    TestType = "AFT"
                                };

                                testGroups.Add(testGroup);

                                // Only Feedback has an IV
                                if (capability.SupportsEmptyIv && kdfMode == KdfModes.Feedback)
                                {
                                    var testGroupNoIv = new TestGroup
                                    {
                                        KdfMode = kdfMode,
                                        MacMode = macMode,
                                        CounterLength = counterLength,
                                        CounterLocation = EnumHelpers.GetEnumFromEnumDescription<CounterLocations>(counterOrder),
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
