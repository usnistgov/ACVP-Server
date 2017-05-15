using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCase : ITestCase
    {

        public int TestCaseId { get; set; }

        public BitString Key { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }

        public bool FailureTest { get; set; }
        public bool Deferred => false;

        public TestCase()
        {
            
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public bool Merge(ITestCase promptTestCase)
        {
            if (TestCaseId != promptTestCase.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)promptTestCase;

            if (PlainText == null && otherTypedTest.PlainText != null)
            {
                PlainText = otherTypedTest.PlainText;
                return true;
            }

            if (CipherText == null && otherTypedTest.CipherText != null)
            {
                CipherText = otherTypedTest.CipherText;
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
                case "pt":
                case "plaintext":
                case "p":
                    PlainText = new BitString(value);
                    return true;
                case "ct":
                case "ciphertext":
                case "c":
                    CipherText = new BitString(value);
                    return true;
                case "key":
                case "k":
                    Key = new BitString(value);
                    return true;
            }
            return false;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;
            Key = expandoSource.GetBitStringFromProperty("key");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
        }
    }
}
