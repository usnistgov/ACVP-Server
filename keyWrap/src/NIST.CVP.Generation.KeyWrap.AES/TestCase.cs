using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestCase : TestCaseBase
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

        public override bool SetString(string name, string value)
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

        protected override void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            
            ExpandoObject expandoSource = (ExpandoObject)source;

            if (expandoSource.ContainsProperty("decryptFail"))
            {
                FailureTest = source.decryptFail;
            }
            if (expandoSource.ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }

            Key = expandoSource.GetBitStringFromProperty("key");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
        }
    }
}
