using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }

        public bool? TestPassed => true;

        public bool Deferred { get; set; } = true;

        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "privateServer", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PrivateKeyServer { get; set; }

        [JsonProperty(PropertyName = "publicServerX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PublicKeyServerX { get; set; }

        [JsonProperty(PropertyName = "publicServerY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PublicKeyServerY { get; set; }

        [JsonIgnore]
        public EccKeyPair KeyPairPartyServer => new EccKeyPair(
            new EccPoint(
                PublicKeyServerX, 
                PublicKeyServerY
            ), 
            PrivateKeyServer
        );

        [JsonProperty(PropertyName = "privateIut", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PrivateKeyIut { get; set; }

        [JsonProperty(PropertyName = "publicIutX", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PublicKeyIutX { get; set; }

        [JsonProperty(PropertyName = "publicIutY", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PublicKeyIutY { get; set; }

        [JsonIgnore]
        public EccKeyPair KeyPairPartyIut => new EccKeyPair(
            new EccPoint(
                PublicKeyIutX,
                PublicKeyIutY
            ),
            PrivateKeyIut
        );

        public BitString Z { get; set; }
    }
}