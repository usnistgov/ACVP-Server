using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public const int _MAX_BIT_SIZE = 4096;

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = new List<ITestGroup>();

            CreateGroups(groups, parameters);

            var testVector = new TestVectorSet
            {
                TestGroups = groups,
                Algorithm = parameters.Algorithm,
                IsSample = parameters.IsSample
            };

            return testVector;
        }

        private void CreateGroups(List<ITestGroup> groups, Parameters parameters)
        {
            var keyWrapType = SpecificationToDomainMapping.Map
                .FirstOrDefault(w => w.Item1 == parameters.Algorithm);

            if (keyWrapType == null)
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
                                KeyWrapType = keyWrapType.Item2,
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
