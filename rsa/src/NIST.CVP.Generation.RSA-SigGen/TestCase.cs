using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public bool IsSample { get; set; }      // Internal only

        public KeyPair Key { get; set; }        // Only needed for Validation
        public BitString Message { get; set; }
        public BitString Signature { get; set; }
        public BitString Salt { get; set; }

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

            if(((ExpandoObject)source).ContainsProperty("message"))
            {
                Message = BitStringFromObject("message", (ExpandoObject)source);
            }

            if (((ExpandoObject)source).ContainsProperty("signature"))
            {
                Signature = BitStringFromObject("signature", (ExpandoObject)source);
            }

            if (((ExpandoObject)source).ContainsProperty("salt"))
            {
                Salt = BitStringFromObject("salt", (ExpandoObject)source);
            }

            Key = new KeyPair { PubKey = new PublicKey() };
            if (((ExpandoObject)source).ContainsProperty("e"))
            {
                Key.PubKey.E = ((ExpandoObject)source).GetBigIntegerFromProperty("e");
            }

            if (((ExpandoObject)source).ContainsProperty("n"))
            {
                Key.PubKey.N = ((ExpandoObject)source).GetBigIntegerFromProperty("n");
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)otherTest;
            if (Salt == null && otherTypedTest.Salt != null)
            {
                Salt = otherTypedTest.Salt.GetDeepCopy();
            }

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

            switch (name.ToLower()){
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "s":
                    Signature = new BitString(value);
                    return true;

                case "saltval":
                    Salt = new BitString(value);
                    return true;
            }

            return false;
        }

        private BitString BitStringFromObject(string propName, ExpandoObject source)
        {
            if (!source.ContainsProperty(propName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[propName];
            if (sourcePropertyValue == null)
            {
                return null;
            }

            var valueAsBitString = sourcePropertyValue as BitString;
            if (valueAsBitString != null)
            {
                return valueAsBitString;
            }

            return new BitString(sourcePropertyValue.ToString());
        }
    }
}
