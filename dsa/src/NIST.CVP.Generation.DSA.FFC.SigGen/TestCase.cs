using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public FfcDomainParameters DomainParams { get; set; }
        public FfcKeyPair Key { get; set; }
        public BitString Message { get; set; }
        public FfcSignature Signature { get; set; }

        // Needed for FireHoseTests
        public BigInteger K;

        // Needed for SetString, FireHoseTests
        private BigInteger _xSetString;
        private BigInteger _ySetString;
        private BigInteger _rSetString;
        private BigInteger _sSetString;

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
            ParseDomainParams((ExpandoObject)source);
            ParseSignature((ExpandoObject)source);

            if (((ExpandoObject)source).ContainsProperty("message"))
            {
                Message = ((ExpandoObject)source).GetBitStringFromProperty("message");
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)otherTest;

            if (Message == null && otherTypedTest.Message != null)
            {
                Message = otherTypedTest.Message.GetDeepCopy();
                return true;
            }

            return false;
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
                    _xSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "y":
                    _ySetString = new BitString(value).ToPositiveBigInteger();
                    Key = new FfcKeyPair(_xSetString, _ySetString);
                    return true;

                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    _rSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    _sSetString = new BitString(value).ToPositiveBigInteger();
                    Signature = new FfcSignature(_rSetString, _sSetString);
                    return true;

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
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

        private void ParseSignature(ExpandoObject source)
        {
            BigInteger r, s;

            if (source.ContainsProperty("r"))
            {
                r = source.GetBigIntegerFromProperty("r");
            }

            if (source.ContainsProperty("s"))
            {
                s = source.GetBigIntegerFromProperty("s");
            }

            Signature = new FfcSignature(r, s);
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
    }
}
