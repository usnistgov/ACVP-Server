using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private readonly IKdfFactory _kdfFactory;

        public TestGroupGenerator(IKdfFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
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
                            var testOutputLengths = new List<int>();
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => w % macOutputLength == 0, 2, true));
                            testOutputLengths.AddRange(capability.SupportedLengths.GetValues(w => w % macOutputLength != 0, 2, true));

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

                                if (capability.SupportsEmptyIv)
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
