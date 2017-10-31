using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.ECC;
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
            TestCaseId = (int)source.tcId;
            ParseKey((ExpandoObject)source);
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            // Nothing to merge from prompt into answers
            return true;
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

        private void ParseKey(ExpandoObject source)
        {
            BigInteger d, Qx, Qy;

            if (source.ContainsProperty("d"))
            {
                d = source.GetBigIntegerFromProperty("d");
            }

            if (source.ContainsProperty("qx"))
            {
                Qx = source.GetBigIntegerFromProperty("qx");
            }

            if (source.ContainsProperty("qy"))
            {
                Qy = source.GetBigIntegerFromProperty("qy");
            }

            KeyPair = new EccKeyPair(new EccPoint(Qx, Qy), d);
        }
    }
}
