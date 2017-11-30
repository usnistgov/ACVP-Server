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
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString PlainText { get; set; }
        public int Length { get; set; }
        public BitString CipherText { get; set; }
        public BitString IV { get; set; }
        public List<BitString> IVs { get; set; }
        public BitString Key { get; set; }

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

            Length = expandoSource.GetTypeFromProperty<int>("length");

            IV = expandoSource.GetBitStringFromProperty("iv");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            Key = expandoSource.GetBitStringFromProperty("key");

            if (expandoSource.ContainsProperty("ctrArray"))
            {
                //CtrArray = CtrArrayToObject(expandoSource);
            }
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
                IV = otherTypedTest.IV;
                return true;
            }

            if (CipherText == null && otherTypedTest.CipherText != null)
            {
                CipherText = otherTypedTest.CipherText;
                IV = otherTypedTest.IV;
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

                case "iv":
                    IV = new BitString(value);
                    return true;

                case "key":
                    Key = new BitString(value);
                    return true;
            }
            return false;
        }

        //private List<AlgoArrayResponse> CtrArrayToObject(ExpandoObject ctrArray)
        //{
        //    return (from dynamic item in ctrArray
        //        select new AlgoArrayResponse
        //        {
        //            CipherText = ctrArray.GetBitStringFromProperty("cipherText"),
        //            IV = ctrArray.GetBitStringFromProperty("iv"),
        //            PlainText = ctrArray.GetBitStringFromProperty("plainText")
        //        }).ToList();
        //}
    }
}
