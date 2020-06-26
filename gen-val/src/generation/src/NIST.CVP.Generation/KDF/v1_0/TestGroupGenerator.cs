using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.KDF.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.KDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
    }
}
