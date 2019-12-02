using System;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XTS.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        [JsonIgnore]
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore] public XtsKey XtsKey { get; set; }

        [JsonProperty(PropertyName = "key")]
        public BitString Key
        {
            get => XtsKey?.Key;
            set => XtsKey = new XtsKey(value);
        }
        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }
        [JsonProperty(PropertyName = "tweakValue")]
        public BitString I { get; set; }
        [JsonProperty(PropertyName = "sequenceNumber")]
        public int SequenceNumber { get; set; }

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
                    XtsKey = new XtsKey(new BitString(value));
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
