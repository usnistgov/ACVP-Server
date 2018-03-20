using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.TLS
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        [JsonProperty(PropertyName = "tlsVersion")]
        public TlsModes TlsMode { get; set; }

        public int KeyBlockLength { get; set; }

        public int PreMasterSecretLength { get; set; }

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
                case "hashalg":
                    HashAlgName = value;
                    return true;

                case "pre-master secret length":
                    PreMasterSecretLength = int.Parse(value);
                    return true;

                case "key block length":
                    KeyBlockLength = int.Parse(value);
                    return true;

                case "tlsversion":
                    TlsMode = EnumHelpers.GetEnumFromEnumDescription<TlsModes>(value);
                    return true;
            }

            return false;
        }
    }
}
