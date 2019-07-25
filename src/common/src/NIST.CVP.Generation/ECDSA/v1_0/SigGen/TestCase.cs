﻿using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "message")]
        public BitString Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString RandomValue { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RandomValueLen { get; set; }

        private int OrderN => ParentGroup == null ? 0 : CurveAttributesHelper.GetCurveAttribute(ParentGroup.Curve).LengthN;
        
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

        // Needed for FireHoseTests
        public EccKeyPair KeyPair;
        public BigInteger K;

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Sometimes these values aren't even length...
            if (value.Length % 2 != 0)
            {
                value = value.Insert(0, "0");
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    Signature.R = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    Signature.S = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "d":
                    KeyPair = new EccKeyPair(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
