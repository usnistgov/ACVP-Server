using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public KeyPair Key { get; set; }
        public BitString CipherText { get; set; }
        public BitString PlainText { get; set; }

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
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");

            var n = expandoSource.GetBigIntegerFromProperty("n");
            var e = expandoSource.GetBigIntegerFromProperty("e");
            Key = new KeyPair{PubKey = new PublicKey {E = e, N = n}};

            FailureTest = !expandoSource.GetTypeFromProperty<bool>("result");
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            return true;
        }
    }
}
