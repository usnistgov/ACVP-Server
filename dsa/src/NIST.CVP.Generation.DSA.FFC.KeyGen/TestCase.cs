using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.FFC;
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

        public FfcKeyPair Key { get; set; }

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

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            BigInteger x, y;

            if (((ExpandoObject)source).ContainsProperty("x"))
            {
                x = ((ExpandoObject)source).GetBigIntegerFromProperty("x");
            }

            if (((ExpandoObject)source).ContainsProperty("y"))
            {
                y = ((ExpandoObject)source).GetBigIntegerFromProperty("y");
            }

            Key = new FfcKeyPair(x, y);
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
    }
}
