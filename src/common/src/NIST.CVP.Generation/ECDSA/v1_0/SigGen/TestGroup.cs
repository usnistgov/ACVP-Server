using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "curve")]
        public Curve Curve { get; set; }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        [JsonIgnore] public EccKeyPair KeyPair { get; set; } = new EccKeyPair();
        [JsonProperty(PropertyName = "d", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger D
        {
            get => KeyPair?.PrivateD ?? 0;
            set => KeyPair.PrivateD = value;
        }

        [JsonProperty(PropertyName = "qx", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Qx
        {
            get => KeyPair?.PublicQ?.X ?? 0;
            set => KeyPair.PublicQ.X = value;
        }

        [JsonProperty(PropertyName = "qy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Qy
        {
            get => KeyPair?.PublicQ?.Y ?? 0;
            set => KeyPair.PublicQ.Y = value;
        }

        [JsonProperty(PropertyName = "componentTest")]
        public bool ComponentTest { get; set; }

        
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "curve":
                    Curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(value);
                    return true;

                case "hashalg":
                    HashAlgName = value;
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return $"{EnumHelpers.GetEnumDescriptionFromEnum(Curve)}{HashAlg.Name}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TestGroup otherGroup)
            {
                return GetHashCode() == otherGroup.GetHashCode();
            }
            
            return false;
        }
    }
}
