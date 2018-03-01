using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB128
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
        public BitString IV { get; set; } 
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString CipherText { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

        public bool SetResultsArrayString(int index, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "key":
                    ResultsArray[index].Key = new BitString(value);
                    return true;

                case "iv":
                    ResultsArray[index].IV = new BitString(value);
                    return true;

                case "plaintext":
                case "pt":
                    ResultsArray[index].PlainText = new BitString(value);
                    return true;

                case "ciphertext":
                case "ct":
                    ResultsArray[index].CipherText = new BitString(value);
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
                case "key":
                    Key = new BitString(value);
                    return true;

                case "iv":
                    IV = new BitString(value);
                    return true;

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
            var expandoSource = (ExpandoObject) source;
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
            if (expandoSource.ContainsProperty("resultsArray"))
            {
                ResultsArray = ResultsArrayToObject(source.resultsArray);
            }

            IV = expandoSource.GetBitStringFromProperty("iv");
            Key = expandoSource.GetBitStringFromProperty("key");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
        }

        private List<AlgoArrayResponse> ResultsArrayToObject(dynamic resultsArray)
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();

            foreach (dynamic item in resultsArray)
            {
                var expandoItem = (ExpandoObject) item;
                AlgoArrayResponse response = new AlgoArrayResponse();
                response.IV = expandoItem.GetBitStringFromProperty("iv");
                response.Key = expandoItem.GetBitStringFromProperty("key");
                response.PlainText = expandoItem.GetBitStringFromProperty("plainText");
                response.CipherText = expandoItem.GetBitStringFromProperty("cipherText");

                list.Add(response);
            }

            return list;
        }
    }
}
