using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CFBP
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

        public TestCase(BitString keys, BitString iv1, BitString iv2 = null, BitString iv3 = null,
            BitString plainText = null, BitString plainText1 = null, BitString plainText2 = null, BitString plainText3 = null,
            BitString cipherText = null, BitString cipherText1 = null, BitString cipherText2 = null, BitString cipherText3 = null)
        {
            Keys = keys;

            IV1 = iv1;
            IV2 = iv2;
            IV3 = iv3;
            
            PlainText = plainText;
            PlainText1 = plainText1;
            PlainText2 = plainText2;
            PlainText3 = plainText3;

            CipherText = cipherText;
            CipherText1 = cipherText1;
            CipherText2 = cipherText2;
            CipherText3 = cipherText3;
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }
        public int? PlainTextLength => (PlainText ?? PlainText1).BitLength;

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
        public int? CipherTextLength => (CipherText ?? CipherText1).BitLength;
        public BitString IV1 { get; set; }
        public BitString IV2 { get; set; }
        public BitString IV3 { get; set; }
        public BitString PlainText1 { get; set; }
        public BitString PlainText2 { get; set; }
        public BitString PlainText3 { get; set; }
        public BitString CipherText1 { get; set; }
        public BitString CipherText2 { get; set; }
        public BitString CipherText3 { get; set; }

        public List<AlgoArrayResponseWithIvs> ResultsArray { get; set; } = new List<AlgoArrayResponseWithIvs>();

        private void MapToProperties(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestCaseId = (int)source.tcId;
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
            CipherText1 = expandoSource.GetBitStringFromProperty("cipherText1");
            CipherText2 = expandoSource.GetBitStringFromProperty("cipherText2");
            CipherText3 = expandoSource.GetBitStringFromProperty("cipherText3");

            if (expandoSource.ContainsProperty("ctLen"))
            {
                var ctLen = (int)source.ctLen;
                CipherText = CipherText?.MSBSubstring(0, ctLen);
                CipherText1 = CipherText1?.MSBSubstring(0, ctLen);
                CipherText2 = CipherText2?.MSBSubstring(0, ctLen);
                CipherText3 = CipherText3?.MSBSubstring(0, ctLen);
            }

            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            PlainText1 = expandoSource.GetBitStringFromProperty("plainText1");
            PlainText2 = expandoSource.GetBitStringFromProperty("plainText2");
            PlainText3 = expandoSource.GetBitStringFromProperty("plainText3");

            if (expandoSource.ContainsProperty("ptLen"))
            {
                var ptLen = (int)source.ptLen;
                PlainText = PlainText?.MSBSubstring(0, ptLen);
                PlainText1 = PlainText1?.MSBSubstring(0, ptLen);
                PlainText2 = PlainText2?.MSBSubstring(0, ptLen);
                PlainText3 = PlainText3?.MSBSubstring(0, ptLen);
            }

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
                case "pt1":
                    PlainText1 = new BitString(value);
                    return true;
                case "pt2":
                    PlainText2 = new BitString(value);
                    return true;
                case "pt3":
                    PlainText3 = new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                    CipherText = new BitString(value);
                    return true;
                case "ct1":
                    CipherText1 = new BitString(value);
                    return true;
                case "ct2":
                    CipherText2 = new BitString(value);
                    return true;
                case "ct3":
                    CipherText3 = new BitString(value);
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