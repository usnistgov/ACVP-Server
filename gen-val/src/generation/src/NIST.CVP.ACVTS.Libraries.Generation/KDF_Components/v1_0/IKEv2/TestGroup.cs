using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2
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

        [JsonProperty(PropertyName = "dhLength")]
        public int GirLength { get; set; }

        public int NInitLength { get; set; }

        public int NRespLength { get; set; }

        public int DerivedKeyingMaterialLength { get; set; }

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
                case "nilength":
                case "ni length":
                    NInitLength = int.Parse(value);
                    return true;

                case "g^ir length":
                    GirLength = int.Parse(value);
                    return true;

                case "nr length":
                    NRespLength = int.Parse(value);
                    return true;

                case "hashalg":
                    HashAlgName = value;
                    return true;

                case "dkm length":
                    DerivedKeyingMaterialLength = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
