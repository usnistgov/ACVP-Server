﻿using System.Collections.Generic;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.EDDSA.v1_0.SigGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator(_oracle),
                new TestGroupGeneratorBitFlip(_oracle)
            };

            return list;
        }
    }
}