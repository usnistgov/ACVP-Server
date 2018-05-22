using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE_LABEL = "aft";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            parameters.PtLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
            var minMaxPtLen = parameters.PtLen.GetDomainMinMax();

            foreach (var function in parameters.Direction)
            {
                foreach (var keyLen in parameters.KeyLen)
                {
                    foreach (var tweakMode in parameters.TweakModes)
                    {
                        var testPtLens = new List<int>();

                        testPtLens.AddRange(parameters.PtLen.GetValues(w => w % 128 == 0 && w != minMaxPtLen.Maximum, 2, true));
                        testPtLens.AddRange(parameters.PtLen.GetValues(w => w % 128 != 0 && w != minMaxPtLen.Maximum, 2, true));
                        testPtLens.Add(minMaxPtLen.Maximum);

                        foreach (var ptLen in testPtLens)
                        {
                            var testGroup = new TestGroup
                            {
                                Direction = function,
                                KeyLen = keyLen,
                                TweakMode = tweakMode,
                                PtLen = ptLen,
                                TestType = TEST_TYPE_LABEL
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return testGroups;
        }
    }
}
