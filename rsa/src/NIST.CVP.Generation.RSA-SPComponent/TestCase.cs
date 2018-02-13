using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public KeyPair Key { get; set; }
        public BitString Message { get; set; }
        public BitString Signature { get; set; }

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

            var expandoSource = (ExpandoObject) source;

            if (expandoSource.ContainsProperty("isSuccess"))
            {
                // Negate it for 'FailureTest'
                FailureTest = !expandoSource.GetTypeFromProperty<bool>("isSuccess");
            }
            else
            {
                FailureTest = false;
            }
            
            Signature = expandoSource.GetBitStringFromProperty("signature");
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

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
                case "em":
                    Message = new BitString(value);
                    return true;

                case "s":
                    if (value.Contains("fail"))
                    {
                        FailureTest = true;
                        return true;
                    }
                    Signature = new BitString(value);
                    return true;

                case "n":
                    Key = new KeyPair {PrivKey = new PrivateKey(), PubKey = new PublicKey()};
                    Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "p":
                    Key.PrivKey.P = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    Key.PrivKey.Q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "d":
                    ((PrivateKey) Key.PrivKey).D = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "e":
                    Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
