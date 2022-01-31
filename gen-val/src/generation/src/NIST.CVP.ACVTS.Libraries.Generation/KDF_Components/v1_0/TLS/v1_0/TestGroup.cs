using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0
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
