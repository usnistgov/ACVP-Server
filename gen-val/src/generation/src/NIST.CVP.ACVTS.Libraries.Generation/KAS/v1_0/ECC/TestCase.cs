using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC
{
    public class TestCase : TestCaseBase<TestGroup, TestCase, KasDsaAlgoAttributesEcc>
    {
        private int DegreeOfPolynomial => ParentGroup == null ? 0 : CurveAttributesHelper.GetCurveAttribute(ParentGroup.Curve).DegreeOfPolynomial;

        [JsonIgnore] public EccKeyPair StaticKeyServer { get; set; } = new EccKeyPair();

        [JsonProperty(PropertyName = "staticPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyServer
        {
            get => StaticKeyServer.PrivateD != 0 ? new BitString(StaticKeyServer.PrivateD, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyServer.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyServerX
        {
            get => StaticKeyServer.PublicQ.X != 0 ? new BitString(StaticKeyServer.PublicQ.X, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyServer.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyServerY
        {
            get => StaticKeyServer.PublicQ.Y != 0 ? new BitString(StaticKeyServer.PublicQ.Y, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyServer.PublicQ.Y = value.ToPositiveBigInteger();
        }


        [JsonIgnore] public EccKeyPair EphemeralKeyServer { get; set; } = new EccKeyPair();

        [JsonProperty(PropertyName = "ephemeralPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyServer
        {
            get => EphemeralKeyServer.PrivateD != 0 ? new BitString(EphemeralKeyServer.PrivateD, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyServer.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyServerX
        {
            get => EphemeralKeyServer.PublicQ.X != 0 ? new BitString(EphemeralKeyServer.PublicQ.X, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyServer.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyServerY
        {
            get => EphemeralKeyServer.PublicQ.Y != 0 ? new BitString(EphemeralKeyServer.PublicQ.Y, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyServer.PublicQ.Y = value.ToPositiveBigInteger();
        }


        [JsonIgnore] public EccKeyPair StaticKeyIut { get; set; } = new EccKeyPair();

        [JsonProperty(PropertyName = "staticPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyIut
        {
            get => StaticKeyIut.PrivateD != 0 ? new BitString(StaticKeyIut.PrivateD, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyIut.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyIutX
        {
            get => StaticKeyIut.PublicQ.X != 0 ? new BitString(StaticKeyIut.PublicQ.X, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyIut.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyIutY
        {
            get => StaticKeyIut.PublicQ.Y != 0 ? new BitString(StaticKeyIut.PublicQ.Y, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyIut.PublicQ.Y = value.ToPositiveBigInteger();
        }


        [JsonIgnore] public EccKeyPair EphemeralKeyIut { get; set; } = new EccKeyPair();

        [JsonProperty(PropertyName = "ephemeralPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyIut
        {
            get => EphemeralKeyIut.PrivateD != 0 ? new BitString(EphemeralKeyIut.PrivateD, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyIut.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyIutX
        {
            get => EphemeralKeyIut.PublicQ.X != 0 ? new BitString(EphemeralKeyIut.PublicQ.X, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyIut.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyIutY
        {
            get => EphemeralKeyIut.PublicQ.Y != 0 ? new BitString(EphemeralKeyIut.PublicQ.Y, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyIut.PublicQ.Y = value.ToPositiveBigInteger();
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "count":
                    int.TryParse(value, out var result);
                    TestCaseId = result;
                    return true;
                case "dscavs":
                    StaticPrivateKeyServer = new BitString(value);
                    return true;
                case "qscavsx":
                    StaticPublicKeyServerX = new BitString(value);
                    return true;
                case "qscavsy":
                    StaticPublicKeyServerY = new BitString(value);
                    return true;
                case "decavs":
                    EphemeralPrivateKeyServer = new BitString(value);
                    return true;
                case "qecavsx":
                    EphemeralPublicKeyServerX = new BitString(value);
                    return true;
                case "qecavsy":
                    EphemeralPublicKeyServerY = new BitString(value);
                    return true;
                case "noncedkmcavs":
                    DkmNonceServer = new BitString(value);
                    break;
                case "nonceephemcavs":
                    EphemeralNonceServer = new BitString(value);
                    break;
                case "dsiut":
                    StaticPrivateKeyIut = new BitString(value);
                    return true;
                case "qsiutx":
                    StaticPublicKeyIutX = new BitString(value);
                    return true;
                case "qsiuty":
                    StaticPublicKeyIutY = new BitString(value);
                    return true;
                case "deiut":
                    EphemeralPrivateKeyIut = new BitString(value);
                    return true;
                case "qeiutx":
                    EphemeralPublicKeyIutX = new BitString(value);
                    return true;
                case "qeiuty":
                    EphemeralPublicKeyIutY = new BitString(value);
                    return true;
                case "noncedkmiut":
                    DkmNonceIut = new BitString(value);
                    break;
                case "nonceephemiut":
                    EphemeralNonceIut = new BitString(value);
                    break;
                case "nonce":
                    NonceNoKc = new BitString(value);
                    return true;
                case "ccmnonce":
                    NonceAesCcm = new BitString(value);
                    return true;
                case "oi":
                    OtherInfo = new BitString(value);
                    OiLen = OtherInfo.BitLength;
                    return true;
                case "cavstag":
                    Tag = new BitString(value);
                    return true;
                case "cavshashzz":
                    HashZ = new BitString(value);
                    return true;
                case "z":
                    Z = new BitString(value);
                    return true;
                case "macdata":
                    MacData = new BitString(value);
                    return true;
                case "dkm":
                    Dkm = new BitString(value);
                    return true;
                case "hashzz":
                    HashZ = new BitString(value);
                    return true;
                case "result":
                    TestPassed = value.StartsWith("p");
                    return true;
            }

            return false;
        }
    }
}
