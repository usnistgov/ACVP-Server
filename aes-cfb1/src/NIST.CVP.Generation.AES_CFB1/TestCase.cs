using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1
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
        public BitOrientedBitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitOrientedBitString CipherText { get; set; }
        public List<BitOrientedAlgoArrayResponse> ResultsArray { get; set; } = new List<BitOrientedAlgoArrayResponse>();

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
                    ResultsArray[index].PlainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(value);
                    return true;

                case "ciphertext":
                case "ct":
                    ResultsArray[index].CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(value);
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
                    PlainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(value);
                    return true;

                case "ciphertext":
                case "ct":
                    CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(value);
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
            if (expandoSource.ContainsProperty("resultsArray"))
            {
                ResultsArray = ResultsArrayToObject(source.resultsArray);
            }

            IV = expandoSource.GetBitStringFromProperty("iv");
            Key = expandoSource.GetBitStringFromProperty("key");
            CipherText = BitOrientedBitString.GetDerivedFromBase(BitStringFromObject("cipherText", (ExpandoObject)source, true));
            PlainText = BitOrientedBitString.GetDerivedFromBase(BitStringFromObject("plainText", (ExpandoObject)source, true));
        }

        private List<BitOrientedAlgoArrayResponse> ResultsArrayToObject(dynamic resultsArray)
        {
            List<BitOrientedAlgoArrayResponse> list = new List<BitOrientedAlgoArrayResponse>();

            foreach (dynamic item in resultsArray)
            {
                BitOrientedAlgoArrayResponse response = new BitOrientedAlgoArrayResponse();
                response.IV = BitStringFromObject("iv", (ExpandoObject)item, false);
                response.Key = BitStringFromObject("key", (ExpandoObject)item, false);
                response.PlainText = BitOrientedBitString.GetDerivedFromBase(BitStringFromObject("plainText", (ExpandoObject)item, true));
                response.CipherText = BitOrientedBitString.GetDerivedFromBase(BitStringFromObject("cipherText", (ExpandoObject)item, true));

                list.Add(response);
            }

            return list;
        }

        private BitString BitStringFromObject(string sourcePropertyName, ExpandoObject source, bool isBitsModeBitString)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }
            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return null;
            }
            var valueAsBitString = sourcePropertyValue as BitString;
            if (valueAsBitString != null)
            {
                return valueAsBitString;
            }

            if (isBitsModeBitString)
            {
                return BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(sourcePropertyValue.ToString());
            }

            return new BitString(sourcePropertyValue.ToString());
        }
    }
}
