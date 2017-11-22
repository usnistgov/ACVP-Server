using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using NIST.CVP.Crypto.AES_XTS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_XTS
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public XtsKey Key { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        public BitString I { get; set; }
        public int SequenceNumber { get; set; }

        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject)source;

            if (expandoSource.ContainsProperty("decryptFail"))
            {
                FailureTest = source.decryptFail;
            }
            if (expandoSource.ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }

            var bitStringKey = expandoSource.GetBitStringFromProperty("key");
            if (bitStringKey != null)
            {
                Key = new XtsKey(bitStringKey);
            }

            I = expandoSource.GetBitStringFromProperty("i");
            SequenceNumber = expandoSource.GetTypeFromProperty<int>("sequenceNumber");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)otherTest;

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
                    Key = new XtsKey(new BitString(value));
                    return true;

                case "i":
                    I = new BitString(value);
                    return true;

                case "dataunitseqnumber":
                    SequenceNumber = Int32.Parse(value);
                    return true;
            }
            return false;
        }
    }
}
