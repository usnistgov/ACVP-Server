﻿using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_OFB.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
                {
                    new TestGroupGeneratorKnownAnswer(),
                    new TestGroupGeneratorMultiblockMessage(),
                    new TestGroupGeneratorMonteCarlo()
                };

            return list;
        }
    }
}
