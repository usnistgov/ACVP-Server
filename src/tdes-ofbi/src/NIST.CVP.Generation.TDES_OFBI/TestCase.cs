using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Dynamic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_OFBI
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

        public TestCase(BitString keys, BitString iv1, BitString iv2, BitString iv3, BitString plainText, BitString cipherText)
        {
            Keys = keys;

            IV1 = iv1;
            IV2 = iv2;
            IV3 = iv3;

            PlainText = plainText;
            
            CipherText = cipherText;
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }

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


        public BitString CipherText { get; set; }

        public BitString IV1 { get; set; }
        public BitString IV2 { get; set; }
        public BitString IV3 { get; set; }
        
        public List<AlgoArrayResponseWithIvs> ResultsArray { get; set; } = new List<AlgoArrayResponseWithIvs>();

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
            if (expandoSource.ContainsProperty("key1"))
            {
                Key1 = expandoSource.GetBitStringFromProperty("key1");
                Key2 = expandoSource.GetBitStringFromProperty("key2");
                Key3 = expandoSource.GetBitStringFromProperty("key3");
            }
            else
            {
                Keys = expandoSource.GetBitStringFromProperty("key");
            }

            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            
            IV1 = expandoSource.GetBitStringFromProperty("iv1");
            IV2 = expandoSource.GetBitStringFromProperty("iv2");
            IV3 = expandoSource.GetBitStringFromProperty("iv3");

        }

        private List<AlgoArrayResponseWithIvs> ResultsArrayToObject(dynamic resultsArray)
        {
            var list = new List<AlgoArrayResponseWithIvs>();

            foreach (dynamic item in resultsArray)
            {
                var expandoItem = (ExpandoObject) item;
                var response = new AlgoArrayResponseWithIvs();

                var key1 = expandoItem.GetBitStringFromProperty("key1");
                var key2 = expandoItem.GetBitStringFromProperty("key2");
                var key3 = expandoItem.GetBitStringFromProperty("key3");

                response.Keys = key1.ConcatenateBits(key2.ConcatenateBits(key3));
                response.PlainText = expandoItem.GetBitStringFromProperty("plainText");

                response.CipherText = expandoItem.GetBitStringFromProperty("cipherText");

                response.IV1 = expandoItem.GetBitStringFromProperty("iv1");
                response.IV2 = expandoItem.GetBitStringFromProperty("iv2");
                response.IV3 = expandoItem.GetBitStringFromProperty("iv3");
                list.Add(response);
            }

            return list;
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
                    Keys = (value == null ? null : new BitString(value));
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
                case "iv1":
                    IV1 = new BitString(value);
                    return true;
                case "iv2":
                    IV3 = new BitString(value);
                    return true;
                case "iv3":
                    IV3 = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}