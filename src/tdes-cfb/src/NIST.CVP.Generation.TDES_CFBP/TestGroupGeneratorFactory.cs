﻿using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGeneratorKnownAnswer(),
                    new TestGroupGeneratorMultiblockMessage(),
                    new TestGroupGeneratorMonteCarlo()
                };

            return list;
        }
    }
}