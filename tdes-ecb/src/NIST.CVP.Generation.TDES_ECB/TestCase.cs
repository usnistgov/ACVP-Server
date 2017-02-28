using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
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

        public TestCase(string key, string plainText, string cipherText)
        {
            Key = new BitString(key);
            PlainText = new BitString(plainText);
            CipherText = new BitString(cipherText);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            if (((ExpandoObject)source).ContainsProperty("decryptFail"))
            {
                FailureTest = source.decryptFail;
            }
            if (((ExpandoObject)source).ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (((ExpandoObject) source).ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }
            if (((ExpandoObject)source).ContainsProperty("resultsArray"))
            {
                ResultsArray = ResultsArrayToObject(source.resultsArray);
            }

            Key = BitStringFromObject("key", (ExpandoObject) source);
            CipherText = BitStringFromObject("cipherText", (ExpandoObject)source);
            PlainText = BitStringFromObject("plainText", (ExpandoObject)source);
        }

        private List<AlgoArrayResponse> ResultsArrayToObject(dynamic resultsArray)
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();

            foreach (dynamic item in resultsArray)
            {
                AlgoArrayResponse response = new AlgoArrayResponse();

                var key1 = BitStringFromObject("key1", (ExpandoObject)item);
                var key2 = BitStringFromObject("key2", (ExpandoObject)item);
                var key3 = BitStringFromObject("key3", (ExpandoObject)item);

                response.Keys = key1.ConcatenateBits(key2.ConcatenateBits(key3));
                response.PlainText = BitStringFromObject("plainText", (ExpandoObject)item);
                response.CipherText = BitStringFromObject("cipherText", (ExpandoObject)item);

                list.Add(response);
            }

            return list;
        }

        private BitString BitStringFromObject(string sourcePropertyName, ExpandoObject source)
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
            return new BitString(sourcePropertyValue.ToString());
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
        public BitString Key1 { get; set; }
        public BitString Key2 { get; set; }
        public BitString Key3 { get; set; }
        public BitString CipherText { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

        public TDESKeys Keys
        {
            get
            {
                if (Key2 == null)
                {
                    return new TDESKeys(Key);
                }
                return new TDESKeys(Key1.ConcatenateBits(Key2.ConcatenateBits(Key3)));
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }
            var otherTypedTest = (TestCase) otherTest;

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

            if (ResultsArray.Count != 0 && otherTypedTest.ResultsArray.Count != 0)
            {
                return true;
            }
            return false;
        }

        public bool SetResultsArrayString(int index, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "key":
                case "keys":
                case "key1":
                    ResultsArray[index].Keys = new BitString(value);
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
                case "keys":
                    Key = new BitString(value);
                    return true;

                case "key1":
                    Key1 = new BitString(value);
                    return true;

                case "key2":
                    Key2 = new BitString(value);
                    return true;

                case "key3":
                    Key3 = new BitString(value);
                    return true;

                case "plaintext":
                case "pt":
                    PlainText = new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                    CipherText = new BitString(value);
                    return true;
            }
            return false;
        }

    }
}
