using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCase : ITestCase
    {
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

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString PlainText { get; set; }
        public int Length { get; set; }
        public BitString CipherText { get; set; }
        public BitString IV { get; set; }
        public List<BitString> IVs { get; set; }
        public BitString Key { get; set; }

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

                case "iv":
                    IV = new BitString(value);
                    return true;

                case "key":
                    Key = new BitString(value);
                    return true;
            }
            return false;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject)source;

            Length = expandoSource.GetTypeFromProperty<int>("dataLength");

            IV = expandoSource.GetBitStringFromProperty("iv");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            Key = expandoSource.GetBitStringFromProperty("key");

            if (expandoSource.ContainsProperty("ivs"))
            {
                IVs = new List<BitString>();
                foreach (var iv in source.ivs)
                {
                    IVs.Add(new BitString(iv));
                }
            }
        }
    }
}
