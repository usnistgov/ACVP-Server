﻿using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>();

            if (parameters.Conformances.Length == 0)
            {
                list.Add(new TestGroupGenerator());
            }

            // Having any extra conformances invalidates the default TestGroupGenerator
            if (parameters.Conformances.Contains("802.11"))
            {
                list.Add(new TestGroupGenerator80211());
            }

            if (parameters.Conformances.Contains("ecma", StringComparer.OrdinalIgnoreCase))
            {
                list.Add(new TestGroupGeneratorEcma());
            }
            
            return list;
        }
    }
}