using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.TDES
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const int _MAX_BIT_SIZE = 4096;

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            CreateGroups(groups, parameters);

            return Task.FromResult(groups);
        }

        private void CreateGroups(List<TestGroup> groups, Parameters parameters)
        {
            KeyWrapType keyWrapType;

            if (SpecificationToDomainMapping.Map
                .TryFirst(w =>
                    w.algorithm == parameters.Algorithm, out var result))
            {
                keyWrapType = result.keyWrapType;
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(parameters.Algorithm)} passed into {nameof(GetType)}");
            }

            // We don't want to generate test groups that have a potential infinite upper bound,
            // set to a maximum value to generate.
            parameters.PayloadLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
            parameters.PayloadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var minMaxPtLen = parameters.PayloadLen.GetDomainMinMax();
            var ptLensAvailableToTest = parameters.PayloadLen.GetValues(10).ToList();

            List<int> testPtLens = new List<int>();
            //Get 64 mod values
            testPtLens.AddRangeIfNotNullOrEmpty(ptLensAvailableToTest
                .Where(
                    w => w % 64 == 0
                //&& w % 128 != 0
                //&& w != minMaxPtLen.Maximum
                )
                .Take(2)
            );
            ////Get 128 mod values
            //testPtLens.AddRangeIfNotNullOrEmpty(ptLensAvailableToTest
            //    .Where(w => w % 128 == 0 && w != minMaxPtLen.Maximum)
            //    .Take(2));
            ////testPtLens.Add(minMaxPtLen.Maximum);

            foreach (var direction in parameters.Direction)
            {
                foreach (var kwCipher in parameters.KwCipher)
                {
                    foreach (var ptLen in testPtLens)
                    {
                        TestGroup testGroup = new TestGroup()
                        {
                            Direction = direction,
                            PayloadLen = ptLen,
                            KwCipher = kwCipher
                        };

                        groups.Add(testGroup);
                    }
                }
            }

        }
    }
}
