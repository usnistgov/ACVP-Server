using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.CMAC.TDES
{
    public class TestGroup : TestGroupBase
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            LoadSource(source);
        }

        public int KeyingOption { get; set; }

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    if (intVal == 3) KeyingOption = 1;
                    else if (intVal == 2) KeyingOption = 2;
                    else throw new ArgumentException($"Cannon parse klen {intVal} to KeyingOption");
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal;
                    return true;
            }
            return false;
        }

        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength
        {
            get => 192;
            set {} //there must be a better way to do this
        }


        protected override void LoadSource(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            Function = expandoSource.GetTypeFromProperty<string>("direction");
            KeyLength = expandoSource.GetTypeFromProperty<int>("keyLen");
            MessageLength = expandoSource.GetTypeFromProperty<int>("msgLen");
            MacLength = expandoSource.GetTypeFromProperty<int>("macLen");

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}
