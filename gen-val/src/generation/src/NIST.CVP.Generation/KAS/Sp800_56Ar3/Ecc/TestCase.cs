using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestCase : TestCaseBase<TestGroup, TestCase>
    {
        private int OrderN => ParentGroup == null ? 0 : CurveAttributesHelper.GetCurveAttribute(ParentGroup.Curve).LengthN;
    
        [JsonIgnore] public EccKeyPair StaticKeyServer { get; set; } = new EccKeyPair();
        
        [JsonProperty(PropertyName = "staticPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyServer
        {
            get => StaticKeyServer.PrivateD != 0 ? new BitString(StaticKeyServer.PrivateD, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyServer.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyServerX
        {
            get => StaticKeyServer.PublicQ.X != 0 ? new BitString(StaticKeyServer.PublicQ.X, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyServer.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyServerY
        {
            get => StaticKeyServer.PublicQ.Y != 0 ? new BitString(StaticKeyServer.PublicQ.Y, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyServer.PublicQ.Y = value.ToPositiveBigInteger();
        }

        
        [JsonIgnore] public EccKeyPair EphemeralKeyServer { get; set; } = new EccKeyPair();
        
        [JsonProperty(PropertyName = "ephemeralPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyServer
        {
            get => EphemeralKeyServer.PrivateD != 0 ? new BitString(EphemeralKeyServer.PrivateD, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyServer.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyServerX
        {
            get => EphemeralKeyServer.PublicQ.X != 0 ? new BitString(EphemeralKeyServer.PublicQ.X, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyServer.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyServerY
        {
            get => EphemeralKeyServer.PublicQ.Y != 0 ? new BitString(EphemeralKeyServer.PublicQ.Y, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyServer.PublicQ.Y = value.ToPositiveBigInteger();
        }

        
        [JsonIgnore] public EccKeyPair StaticKeyIut { get; set; } = new EccKeyPair();
        
        [JsonProperty(PropertyName = "staticPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyIut
        {
            get => StaticKeyIut.PrivateD != 0 ? new BitString(StaticKeyIut.PrivateD, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyIut.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyIutX
        {
            get => StaticKeyIut.PublicQ.X != 0 ? new BitString(StaticKeyIut.PublicQ.X, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyIut.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyIutY
        {
            get => StaticKeyIut.PublicQ.Y != 0 ? new BitString(StaticKeyIut.PublicQ.Y, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => StaticKeyIut.PublicQ.Y = value.ToPositiveBigInteger();
        }

        
        [JsonIgnore] public EccKeyPair EphemeralKeyIut { get; set; } = new EccKeyPair();
        
        [JsonProperty(PropertyName = "ephemeralPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyIut
        {
            get => EphemeralKeyIut.PrivateD != 0 ? new BitString(EphemeralKeyIut.PrivateD, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyIut.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyIutX
        {
            get => EphemeralKeyIut.PublicQ.X != 0 ? new BitString(EphemeralKeyIut.PublicQ.X, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyIut.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyIutY
        {
            get => EphemeralKeyIut.PublicQ.Y != 0 ? new BitString(EphemeralKeyIut.PublicQ.Y, OrderN).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => EphemeralKeyIut.PublicQ.Y = value.ToPositiveBigInteger();
        }
    }
}