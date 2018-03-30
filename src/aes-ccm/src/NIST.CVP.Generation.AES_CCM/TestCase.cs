using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCase : ITestCase
    {

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
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString AAD { get; set; }
        public BitString IV { get; set; }
        public BitString CipherText { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
      
            switch (name.ToLower())
            {
                case "key":
                    Key = new BitString(value);
                    return true;
                case "aad":
                case "adata":
                    AAD = new BitString(value);
                    return true;
                case "iv":
                case "nonce":
                    IV = new BitString(value);
                    return true;
                case "payload":
                case "plaintext":
                case "pt":
                    PlainText= new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                    CipherText = new BitString(value);
                    return true;
            }
            return false;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            ExpandoObject expandoSource = source;
            if (expandoSource.ContainsProperty("decryptFail"))
            {
                FailureTest = source.decryptFail;
            }
            if (expandoSource.ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (expandoSource.ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }

            Key = expandoSource.GetBitStringFromProperty("key");
            IV = expandoSource.GetBitStringFromProperty("iv");
            AAD = expandoSource.GetBitStringFromProperty("aad");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
        }
    }
}
