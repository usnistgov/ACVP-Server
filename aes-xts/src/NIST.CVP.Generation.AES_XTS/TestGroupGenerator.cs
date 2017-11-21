using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
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
                        //// Note, number of valid values (mod 128) is small compared to otherwise, need a consistent way to hit all groups
                        //var ptLensAvailableToTest = parameters.PtLen.GetValues(5000).ToList();  // TODO Fix this so it isn't just a shotgun approach
                        var testPtLens = new List<int>();

                        //// Add two random ptLens that are modulo 128
                        //testPtLens.AddRangeIfNotNullOrEmpty(ptLensAvailableToTest
                        //    .Where(w => w % 128 == 0 &&
                        //                w != minMaxPtLen.Maximum)
                        //    .Take(2));

                        //// Add two random ptLens that are not modulo 128
                        //testPtLens.AddRangeIfNotNullOrEmpty(ptLensAvailableToTest
                        //    .Where(w => w % 128 != 0 &&
                        //                w != minMaxPtLen.Maximum)
                        //    .Take(2));

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
