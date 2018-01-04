using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SNMP
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
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
