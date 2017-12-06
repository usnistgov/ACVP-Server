using System;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Dynamic;

namespace NIST.CVP.Generation.TDES_CFB
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
        public TestCase(BitString keys, BitString plainText, BitString cipherText, BitString iv)
        {
            Iv = iv;
            Keys = keys;
            PlainText = plainText;
            CipherText = cipherText;
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
            if (((ExpandoObject)source).ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }
            if (((ExpandoObject)source).ContainsProperty("resultsArray"))
            {
                ResultsArray = ResultsArrayToObject(source.resultsArray);
            }
            if (((ExpandoObject) source).ContainsProperty("key1"))
            {
                Key1 = BitStringFromObject("key1", (ExpandoObject) source);
                Key2 = BitStringFromObject("key2", (ExpandoObject) source);
                Key3 = BitStringFromObject("key3", (ExpandoObject) source);
            }
            else
            {
                Keys = BitStringFromObject("key", (ExpandoObject)source);
            }

            CipherText = BitStringFromObject("ct", (ExpandoObject)source);
            if (((ExpandoObject)source).ContainsProperty("ctLen"))
            {
                CipherText = CipherText?.MSBSubstring(0, (int)source.ctLen);
            }

            PlainText = BitStringFromObject("pt", (ExpandoObject)source);
            if (((ExpandoObject)source).ContainsProperty("ptLen"))
            {
                var ptLen = (int)source.ptLen;
                PlainText = PlainText?.MSBSubstring(0, ptLen);
            }

            Iv = BitStringFromObject("iv", (ExpandoObject)source);

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
                response.PlainText = BitStringFromObject("pt", (ExpandoObject)item);
                response.CipherText = BitStringFromObject("ct", (ExpandoObject)item);
                response.IV = BitStringFromObject("iv", (ExpandoObject)item);
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

        private BitString _keys;
        public BitString Keys //TODO this belongs in TDES keys, not here
        {
            get => _keys;
            set
            {
                if (value == null)
                {
                    _keys = null;
                    return;
                }
                if (value.BitLength == 64)
                {
                    _keys = value.ConcatenateBits(value).ConcatenateBits(value);
                }
                else if (value.BitLength == 128)
                {
                    _keys = value.Substring(0, 64)
                        .ConcatenateBits(value.Substring(64, 64))
                        .ConcatenateBits(value.Substring(0, 64));
                }
                else if (value.BitLength == 192)
                {
                    _keys = value;
                }
                else
                {
                    throw new ArgumentException("Invalid key size");
                }
            }
        }
        //This allows for either the main key to be set, or any of the three keys to be set individually
        public BitString Key1
        {
            get => Keys?.MSBSubstring(0, 64);
            set => Keys = Keys == null ?
                value.ConcatenateBits(new BitString(128)) :
                value.ConcatenateBits(Keys.MSBSubstring(64, 128));
        }

        public BitString Key2
        {
            get => Keys?.MSBSubstring(64, 64);
            set => Keys = Keys == null ?
                new BitString(64).ConcatenateBits(value).ConcatenateBits(new BitString(64)) :
                Keys.MSBSubstring(0, 64).ConcatenateBits(value).ConcatenateBits(Keys.MSBSubstring(128, 64));
        }
        public BitString Key3
        {
            get => Keys?.MSBSubstring(128, 64);
            set => Keys = Keys == null ?
                new BitString(128).ConcatenateBits(value) :
                Keys.MSBSubstring(0, 128).ConcatenateBits(value);
        }

        public BitString PlainText { get; set; }
        public int? PlainTextLength => PlainText?.BitLength;

        public BitString CipherText { get; set; }
        public int? CipherTextLength => CipherText?.BitLength;

        public BitString Iv { get; set; }
        public List<AlgoArrayResponse> ResultsArray { get; set; } = new List<AlgoArrayResponse>();

        //public TDESKeys Keys
        //{
        //    get
        //    {
        //        if (Key2 == null)
        //        {
        //            return new TDESKeys(Key);
        //        }
        //        return new TDESKeys(Key1.ConcatenateBits(Key2.ConcatenateBits(Key3)));
        //    }
        //}



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
                case "initialization vector":
                case "iv":
                    ResultsArray[index].IV = new BitString(value);
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
                    Keys = value == null ? null : new BitString(value);
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
                case "initialization vector":
                case "iv":
                    Iv = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}