using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase>
    {
        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength
        {
            get => 192;
            set { }
        }

        [JsonIgnore]
        public override KeyWrapType KeyWrapType
        {
            get => KeyWrapType.TDES_KW;
            set { }
        }

        [JsonProperty(PropertyName = "numberOfKeys")]
        public int NumberOfKeys { get; set; }

        [JsonIgnore]
        public int KeyingOption { get; set; }

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

            if (!int.TryParse(value, out var intVal))
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
    }
}