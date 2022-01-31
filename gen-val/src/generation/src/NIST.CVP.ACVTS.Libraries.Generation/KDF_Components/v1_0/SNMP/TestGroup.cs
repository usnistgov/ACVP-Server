using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public BitString EngineId { get; set; }
        public int PasswordLength { get; set; }

        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "engineid":
                    EngineId = new BitString(value);
                    return true;

                case "passwordlen":
                    PasswordLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
