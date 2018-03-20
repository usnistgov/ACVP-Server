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
    public class TestCase : ITestCase
    {
        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public int TestCaseId { get; set; }

        public bool FailureTest => false;
        public bool Deferred { get; set; } = true;

        public BigInteger PrivateKeyServer { get; set; }
        public BigInteger PublicKeyServerX { get; set; }
        public BigInteger PublicKeyServerY { get; set; }

        [JsonIgnore]
        public EccKeyPair KeyPairPartyServer => new EccKeyPair(
            new EccPoint(
                PublicKeyServerX, 
                PublicKeyServerY
            ), 
            PrivateKeyServer
        );

        public BigInteger PrivateKeyIut { get; set; }
        public BigInteger PublicKeyIutX { get; set; }
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

        protected void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;

            PrivateKeyServer = expandoSource.GetBigIntegerFromProperty("privateServer");
            PublicKeyServerX = expandoSource.GetBigIntegerFromProperty("publicServerX");
            PublicKeyServerY = expandoSource.GetBigIntegerFromProperty("publicServerY");
            
            PrivateKeyIut = expandoSource.GetBigIntegerFromProperty("privateIut");
            PublicKeyIutX = expandoSource.GetBigIntegerFromProperty("publicIutX");
            PublicKeyIutY = expandoSource.GetBigIntegerFromProperty("publicIutY");
            
            Z = expandoSource.GetBitStringFromProperty("z");
        }
    }
}