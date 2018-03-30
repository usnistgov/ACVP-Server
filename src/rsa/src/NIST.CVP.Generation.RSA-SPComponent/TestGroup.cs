using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public int TestGroupId { get; set; }
        public int Modulo { get; set; } = 2048;
        public PrivateKeyModes KeyFormat { get; set; }
        public string TestType { get; set; }
        public List<ITestCase> Tests { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "mod":
                    Modulo = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
