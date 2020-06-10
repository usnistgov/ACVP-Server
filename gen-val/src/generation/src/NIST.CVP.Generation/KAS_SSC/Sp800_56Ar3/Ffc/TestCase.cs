using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ffc
{
	public class TestCase : TestCaseBase<TestGroup, TestCase, FfcKeyPair>
    {
        private int l => ParentGroup?.DomainParameterL ?? 0;
        private int n => ParentGroup?.DomainParameterN ?? 0;
        
        public override FfcKeyPair StaticKeyServer { get; set; } = new FfcKeyPair();
        public override FfcKeyPair EphemeralKeyServer { get; set; } = new FfcKeyPair();
        public override FfcKeyPair StaticKeyIut { get; set; } = new FfcKeyPair();
        public override FfcKeyPair EphemeralKeyIut { get; set; } = new FfcKeyPair();
        
        [JsonProperty(PropertyName = "staticPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyServer
        {
            get => StaticKeyServer.PrivateKeyX == 0 ? null : new BitString(StaticKeyServer.PrivateKeyX, n);
            set => StaticKeyServer.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyServer
        {
            get => StaticKeyServer.PublicKeyY == 0 ? null : new BitString(StaticKeyServer.PublicKeyY, l);
            set => StaticKeyServer.PublicKeyY = value.ToPositiveBigInteger();
        }

        
        [JsonProperty(PropertyName = "ephemeralPrivateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyServer
        {
            get => EphemeralKeyServer.PrivateKeyX == 0 ? null : new BitString(EphemeralKeyServer.PrivateKeyX, n);
            set => EphemeralKeyServer.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyServer
        {
            get => EphemeralKeyServer.PublicKeyY == 0 ? null : new BitString(EphemeralKeyServer.PublicKeyY, l);
            set => EphemeralKeyServer.PublicKeyY = value.ToPositiveBigInteger();
        }

        
        [JsonProperty(PropertyName = "staticPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPrivateKeyIut
        {
            get => StaticKeyIut.PrivateKeyX == 0 ? null : new BitString(StaticKeyIut.PrivateKeyX, n);
            set => StaticKeyIut.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "staticPublicIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString StaticPublicKeyIut
        {
            get => StaticKeyIut.PublicKeyY == 0 ? null : new BitString(StaticKeyIut.PublicKeyY, l);
            set => StaticKeyIut.PublicKeyY = value.ToPositiveBigInteger();
        }

        
        [JsonProperty(PropertyName = "ephemeralPrivateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPrivateKeyIut
        {
            get => EphemeralKeyIut.PrivateKeyX == 0 ? null : new BitString(EphemeralKeyIut.PrivateKeyX, n);
            set => EphemeralKeyIut.PrivateKeyX = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "ephemeralPublicIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString EphemeralPublicKeyIut
        {
            get => EphemeralKeyIut.PublicKeyY == 0 ? null : new BitString(EphemeralKeyIut.PublicKeyY, l);
            set => EphemeralKeyIut.PublicKeyY = value.ToPositiveBigInteger();
        }
    }
}