using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public EccKeyPair KeyPair { get; set; }

        public ITestGroup Parent { get; set; }

        private BigInteger _setStringD;
        private BigInteger _setStringQx;
        private BigInteger _setStringQy;

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

            BigInteger d, qx, qy;
            d = expandoSource.GetBigIntegerFromProperty("d");
            qx = expandoSource.GetBigIntegerFromProperty("qx");
            qy = expandoSource.GetBigIntegerFromProperty("qy");
            KeyPair = new EccKeyPair(new EccPoint(qx, qy), d);
        }

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
                case "d":
                    _setStringD = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qx":
                    _setStringQx = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qy":
                    _setStringQy = new BitString(value).ToPositiveBigInteger();
                    KeyPair = new EccKeyPair(new EccPoint(_setStringQx, _setStringQy), _setStringD);
                    return true;
            }

            return false;
        }
    }
}
