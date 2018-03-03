using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap.TDES
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

        public BitString Key1
        {
            get => Key?.MSBSubstring(0, 64);
            set
            {
                if (value == null)
                {
                    value = new BitString(64);
                }
                Key = Key != null
                    ? value.ConcatenateBits(Key.Substring(64, 128))
                    : value.ConcatenateBits(new BitString(128));
            }
        }

        public BitString Key2
        {
            get => Key?.MSBSubstring(64, 64);
            set
            {
                if (value == null)
                {
                    value = new BitString(64);
                }
                Key = Key != null
                    ? Key.Substring(0, 64).ConcatenateBits(value).ConcatenateBits(Key.Substring(128, 64))
                    : new BitString(64).ConcatenateBits(value).ConcatenateBits(new BitString(64));
            }
        }

        public BitString Key3
        {
            get => Key?.MSBSubstring(128, 64);
            set
            {
                if (value == null)
                {
                    value = new BitString(64);
                }
                Key = Key != null
                    ? Key.Substring(0, 128).ConcatenateBits(value)
                    : new BitString(64).ConcatenateBits(value);
            }
        }

        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

        public override bool SetString(string name, string value)
        {

            //fix me here!!!!!

            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "k":
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
                case "p":
                    PlainText = new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                case "c":
                    CipherText = new BitString(value);
                    return true;
            }
            return false;
        }
        
        protected override void MapToProperties(dynamic source)
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

            Key = expandoSource.GetBitStringFromProperty("key");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            Key1 = expandoSource.GetBitStringFromProperty("key1");
            Key2 = expandoSource.GetBitStringFromProperty("key2");
            Key3 = expandoSource.GetBitStringFromProperty("key3");
        }

        private List<AlgoArrayResponse> ResultsArrayToObject(dynamic resultsArray)
        {
            List<AlgoArrayResponse> list = new List<AlgoArrayResponse>();

            foreach (dynamic item in resultsArray)
            {
                var expandoItem = (ExpandoObject) item;
                AlgoArrayResponse response = new AlgoArrayResponse();

                var key1 = expandoItem.GetBitStringFromProperty("key1");
                var key2 = expandoItem.GetBitStringFromProperty("key2");
                var key3 = expandoItem.GetBitStringFromProperty("key3");

                response.Keys = key1.ConcatenateBits(key2.ConcatenateBits(key3));
                response.PlainText = expandoItem.GetBitStringFromProperty("plainText");
                response.CipherText = expandoItem.GetBitStringFromProperty("cipherText");
                response.Key1 = expandoItem.GetBitStringFromProperty("key1");
                response.Key2 = expandoItem.GetBitStringFromProperty("key2");
                response.Key3 = expandoItem.GetBitStringFromProperty("key3");

                list.Add(response);
            }

            return list;
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
    }
}