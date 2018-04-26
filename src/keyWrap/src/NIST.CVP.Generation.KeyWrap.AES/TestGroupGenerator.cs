using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const int _MAX_BIT_SIZE = 4096;

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            CreateGroups(groups, parameters);

            return groups;
        }

        private void CreateGroups(List<TestGroup> groups, Parameters parameters)
        {
            KeyWrapType keyWrapType;

            if (SpecificationToDomainMapping.Map
                .TryFirst(w => w.algorithm == parameters.Algorithm, out var result))
            {
                keyWrapType = result.keyWrapType;
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(parameters.Algorithm)} passed into {nameof(GetType)}");
            }

            // We don't want to generate test groups that have a potential infinite upper bound,
            // set to a maximum value to generate.
            parameters.PtLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
            parameters.PtLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var minMaxPtLen = parameters.PtLen.GetDomainMinMax();
            var ptLensAvailableToTest = parameters.PtLen.GetValues(10).ToList();

            List<int> testPtLens = new List<int>();
            // Get 64 mod values
            testPtLens.AddRangeIfNotNullOrEmpty(ptLensAvailableToTest
                .Where(
                    w => w % 64 == 0
                    && w % 128 != 0
                    && w != minMaxPtLen.Maximum
                 )
                .Take(2)
            );
            // Get 128 mod values
            testPtLens.AddRangeIfNotNullOrEmpty(ptLensAvailableToTest
                .Where(w => w % 128 == 0 && w != minMaxPtLen.Maximum)
                .Take(2));
            testPtLens.Add(minMaxPtLen.Maximum);

            foreach (var direction in parameters.Direction)
            {
                foreach (var kwCipher in parameters.KwCipher)
                {
                    foreach (var keyLen in parameters.KeyLen)
                    {
                        foreach (var ptLen in testPtLens)
                        {
                            TestGroup testGroup = new TestGroup()
                            {
                                KeyWrapType = keyWrapType,
                                Direction = direction,
                                PtLen = ptLen,
                                KwCipher = kwCipher,
                                KeyLength = keyLen
                            };

                            groups.Add(testGroup);
                        }
                    }
                }
            }
        }
    }
}