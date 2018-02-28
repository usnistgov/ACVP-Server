using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestGroup : TestGroupBase
    {
        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength { get; set; }

        private bool _withPadding = false;

        public override  KeyWrapType KeyWrapType
        {
            get => _withPadding ? KeyWrapType.AES_KWP : KeyWrapType.AES_KW;
            set => _withPadding = (value == KeyWrapType.AES_KWP);
        }

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

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            switch (name)
            {
                case "keylen":
                case "keylength":
                    KeyLength = intVal;
                    return true;

                case "ptlen":
                case "plaintext length":
                    PtLen = intVal;
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
            KeyLength = expandoSource.GetTypeFromProperty<int>("keyLen");
            PtLen = expandoSource.GetTypeFromProperty<int>("ptLen");

            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
    }
}
