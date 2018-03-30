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
        // Needed for SetString, FireHoseTests
        private BigInteger x;
        private BigInteger y;

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

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public FfcDomainParameters DomainParams { get; set; }
        public FfcKeyPair Key { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "x":
                    x = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "y":
                    y = new BitString(value).ToPositiveBigInteger();
                    Key = new FfcKeyPair(x, y);
                    return true;
            }

            return false;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ParseKey((ExpandoObject)source);
            ParseDomainParams((ExpandoObject)source);
        }

        

        private void ParseDomainParams(ExpandoObject source)
        {
            BigInteger p, q, g;

            if (source.ContainsProperty("p"))
            {
                p = source.GetBigIntegerFromProperty("p");
            }

            if (source.ContainsProperty("q"))
            {
                q = source.GetBigIntegerFromProperty("q");
            }

            if (source.ContainsProperty("g"))
            {
                g = source.GetBigIntegerFromProperty("g");
            }

            DomainParams = new FfcDomainParameters(p, q, g);
        }

        private void ParseKey(ExpandoObject source)
        {
            BigInteger x, y;

            if (source.ContainsProperty("x"))
            {
                x = source.GetBigIntegerFromProperty("x");
            }

            if (source.ContainsProperty("y"))
            {
                y = source.GetBigIntegerFromProperty("y");
            }

            Key = new FfcKeyPair(x, y);
        }
    }
}
