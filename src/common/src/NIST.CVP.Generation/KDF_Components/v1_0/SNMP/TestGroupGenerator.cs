﻿using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SNMP
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var engineId in parameters.EngineId)
            {
                foreach (var passwordLen in parameters.PasswordLength.GetDomainMinMaxAsEnumerable())
                {
                    list.Add(new TestGroup
                    {
                        EngineId = new BitString(engineId),
                        PasswordLength = passwordLen
                    });
                }
            }

            return list;
        }
    }
}