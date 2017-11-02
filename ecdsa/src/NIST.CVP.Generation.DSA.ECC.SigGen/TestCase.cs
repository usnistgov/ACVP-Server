using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString Message { get; set; }
        public EccKeyPair KeyPair { get; set; }
        public EccSignature Signature { get; set; }

        // Needed for FireHoseTests
        public BigInteger K;
        public BigInteger _rSetString;
        public BigInteger _sSetString;

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

            // Sometimes these values aren't even length...
            if (value.Length % 2 != 0)
            {
                value = value.Insert(0, "0");
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    _rSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    _sSetString = new BitString(value).ToPositiveBigInteger();
                    Signature = new EccSignature(_rSetString, _sSetString);
                    return true;

                case "d":
                    KeyPair = new EccKeyPair(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "k":
                    K = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }

        private void ParseKey(ExpandoObject source)
        {
            BigInteger qx, qy;

            if (source.ContainsProperty("qx"))
            {
                qx = source.GetBigIntegerFromProperty("qx");
            }

            if (source.ContainsProperty("qy"))
            {
                qy = source.GetBigIntegerFromProperty("qy");
            }

            KeyPair = new EccKeyPair(new EccPoint(qx, qy));
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

            Signature = new EccSignature(r, s);
        }
    }
}
