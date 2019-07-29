using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public EcdsaSignatureDisposition Reason { get; set; }
        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString RandomValue { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RandomValueLen { get; set; }

        private int OrderN => ParentGroup == null ? 0 : CurveAttributesHelper.GetCurveAttribute(ParentGroup.Curve).LengthN;
        
        [JsonIgnore] public EccKeyPair KeyPair { get; set; } = new EccKeyPair();
        [JsonProperty(PropertyName = "d", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString D
        {
            get => KeyPair?.PrivateD != 0 ? new BitString(KeyPair.PrivateD, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPair.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "qx", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Qx
        {
            get => KeyPair?.PublicQ?.X != 0 ? new BitString(KeyPair.PublicQ.X, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPair.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "qy", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Qy
        {
            get => KeyPair?.PublicQ?.Y != 0 ? new BitString(KeyPair.PublicQ.Y, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPair.PublicQ.Y = value.ToPositiveBigInteger();
        }

        [JsonIgnore] public EccSignature Signature { get; set; } = new EccSignature();
        [JsonProperty(PropertyName = "r", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString R
        {
            get => Signature.R != 0 ? new BitString(Signature.R, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => Signature.R = value.ToPositiveBigInteger();
        }
        [JsonProperty(PropertyName = "s", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString S
        {
            get => Signature.S != 0 ? new BitString(Signature.S, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => Signature.S = value.ToPositiveBigInteger();
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (value.Length % 2 != 0)
            {
                value = "0" + value;
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "qx":
                    KeyPair.PublicQ.X = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qy":
                    KeyPair.PublicQ.Y = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "r":
                    Signature.R = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    Signature.S = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "result":
                    TestPassed = value.ToLower()[0] == 'p';
                    return true;
            }

            return false;
        }
    }
}
