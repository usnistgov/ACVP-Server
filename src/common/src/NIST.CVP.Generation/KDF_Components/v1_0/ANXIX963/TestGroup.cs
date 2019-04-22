using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963
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

        public int SharedInfoLength { get; set; }

        public int KeyDataLength { get; set; }

        public int FieldSize { get; set; }

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

                case "shared secret length":
                    FieldSize = int.Parse(value);
                    return true;

                case "sharedinfo length":
                    SharedInfoLength = int.Parse(value);
                    return true;

                case "key data length":
                    KeyDataLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
