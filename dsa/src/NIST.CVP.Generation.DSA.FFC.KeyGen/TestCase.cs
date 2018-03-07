using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public FfcDomainParameters DomainParams { get; set; }
        public FfcKeyPair Key { get; set; }

        public ITestGroup Parent { get; set; }

        // Needed for SetString, FireHoseTests
        private BigInteger _x;
        private BigInteger _y;

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int) source.tcId;

            var expandoSource = (ExpandoObject) source;

            var x = expandoSource.GetBigIntegerFromProperty("x");
            var y = expandoSource.GetBigIntegerFromProperty("y");
            Key = new FfcKeyPair(x, y);
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
                    _x = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "y":
                    _y = new BitString(value).ToPositiveBigInteger();
                    Key = new FfcKeyPair(_x, _y);
                    return true;
            }

            return false;
        }
    }
}
