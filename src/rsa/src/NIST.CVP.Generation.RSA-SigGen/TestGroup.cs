using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public SignatureSchemes Mode { get; set; }
        public int Modulo { get; set; }

        [JsonIgnore]
        public HashFunction HashAlg { get; set; }

        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public int SaltLen { get; set; }

        [JsonIgnore]
        public KeyPair Key { get; set; } = new KeyPair {PubKey = new PublicKey()};

        public BigInteger N
        {
            get => Key.PubKey.N;
            set => Key.PubKey.N = value;
        }

        public BigInteger E
        {
            get => Key.PubKey.E;
            set => Key.PubKey.E = value;
        }

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
                case "mod":
                    Modulo = int.Parse(value);
                    return true;

                case "n":
                    if (Key == null) { Key = new KeyPair {PubKey = new PublicKey()}; }
                    Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "e":
                    Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "d":
                    var d = new BitString(value).ToPositiveBigInteger();
                    Key.PrivKey = new PrivateKey {D = d};
                    return true;

                case "hash":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;

                case "saltlen":
                    SaltLen = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
