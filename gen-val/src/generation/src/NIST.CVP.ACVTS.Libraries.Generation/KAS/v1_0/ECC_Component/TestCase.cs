using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }

        public bool? TestPassed => true;

        public bool Deferred { get; set; } = true;

        public TestGroup ParentGroup { get; set; }

        private int DegreeOfPolynomial => ParentGroup == null ? 0 : CurveAttributesHelper.GetCurveAttribute(ParentGroup.Curve).DegreeOfPolynomial;

        [JsonIgnore] public EccKeyPair KeyPairPartyServer { get; set; } = new EccKeyPair();

        [JsonProperty(PropertyName = "privateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PrivateKeyServer
        {
            get => KeyPairPartyServer.PrivateD != 0 ? new BitString(KeyPairPartyServer.PrivateD, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPairPartyServer.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "publicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PublicKeyServerX
        {
            get => KeyPairPartyServer.PublicQ.X != 0 ? new BitString(KeyPairPartyServer.PublicQ.X, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPairPartyServer.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "publicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PublicKeyServerY
        {
            get => KeyPairPartyServer.PublicQ.Y != 0 ? new BitString(KeyPairPartyServer.PublicQ.Y, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPairPartyServer.PublicQ.Y = value.ToPositiveBigInteger();
        }


        [JsonIgnore]
        public EccKeyPair KeyPairPartyIut { get; set; } = new EccKeyPair();

        [JsonProperty(PropertyName = "privateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PrivateKeyIut
        {
            get => KeyPairPartyIut.PrivateD != 0 ? new BitString(KeyPairPartyIut.PrivateD, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPairPartyIut.PrivateD = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "publicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PublicKeyIutX
        {
            get => KeyPairPartyIut.PublicQ.X != 0 ? new BitString(KeyPairPartyIut.PublicQ.X, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPairPartyIut.PublicQ.X = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "publicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString PublicKeyIutY
        {
            get => KeyPairPartyIut.PublicQ.Y != 0 ? new BitString(KeyPairPartyIut.PublicQ.Y, DegreeOfPolynomial).PadToModulusMsb(BitString.BITSINBYTE) : null;
            set => KeyPairPartyIut.PublicQ.Y = value.ToPositiveBigInteger();
        }

        public BitString Z { get; set; }
    }
}
