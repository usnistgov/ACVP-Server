using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.v1_0.KeyGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        private int l => ParentGroup?.L ?? 0;
        private int n => ParentGroup?.N ?? 0;
        
        /// <summary>
        /// Ignoring for (De)Serialization as KeyPairs are flattened
        /// </summary>
        [JsonIgnore]
        public FfcKeyPair Key { get; set; } = new FfcKeyPair();

        [JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString X
        {
            get => Key.PrivateKeyX != 0 ? new BitString(Key.PrivateKeyX, n) : null;
            set => Key.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Y
        {
            get => Key.PublicKeyY != 0 ? new BitString(Key.PublicKeyY, l) : null;
            set => Key.PublicKeyY = value.ToPositiveBigInteger();
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "x":
                    X = new BitString(value).PadToModulusMsb(32);
                    return true;

                case "y":
                    Y = new BitString(value).PadToModulusMsb(32);
                    return true;
            }

            return false;
        }
    }
}
