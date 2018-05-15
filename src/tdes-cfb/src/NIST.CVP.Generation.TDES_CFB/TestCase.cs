using System;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Dynamic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestCase : ITestCase
    {
        private BitString _keys;

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

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
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
            if (expandoSource.ContainsProperty("ctLen"))
            {
                CipherText = CipherText?.MSBSubstring(0, (int)source.ctLen);
            }

            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            if (expandoSource.ContainsProperty("ptLen"))
            {
                var ptLen = (int)source.ptLen;
                PlainText = PlainText?.MSBSubstring(0, ptLen);
            }

            Iv = expandoSource.GetBitStringFromProperty("iv");

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
                response.IV = expandoItem.GetBitStringFromProperty("iv");
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