using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SNMP
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
