﻿using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>> list =
                new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGenerator()
                };

            return list;
        }
    }
}