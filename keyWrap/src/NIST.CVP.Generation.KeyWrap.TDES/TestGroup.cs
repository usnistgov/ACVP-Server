using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestGroup : TestGroupBase
    {
        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength
        {
            get => 192;
            set { }
        }

        public override KeyWrapType KeyWrapType
        {
            get => KeyWrapType.TDES_KW;
            set { }
        }

        [JsonProperty(PropertyName = "numberOfKeys")]
        public int NumberOfKeys { get; set; }

        public int KeyingOption { get; set; }

        public TestGroup() { }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            LoadSource(source);
        }

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            name = name.ToLower();

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name)
            {
                case "numberofkeys":
                    NumberOfKeys = intVal;
                    return true;
            }
            return false;
        }

        protected override void LoadSource(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            Direction = expandoSource.GetTypeFromProperty<string>("direction");
            KwCipher = expandoSource.GetTypeFromProperty<string>("kwCipher");
            KeyingOption = expandoSource.GetTypeFromProperty<int>("keyingOption");
            PtLen = expandoSource.GetTypeFromProperty<int>("ptLen");
            
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}