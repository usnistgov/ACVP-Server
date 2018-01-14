using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Dynamic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_CBCI
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

        //public TestCase(BitString keys, BitString iv1, BitString iv2 = null, BitString iv3 = null,
        //    BitString plainText = null, BitString plainText1 = null, BitString plainText2 = null, BitString plainText3 = null,
        //    BitString cipherText = null, BitString cipherText1 = null, BitString cipherText2 = null, BitString cipherText3 = null)
        public TestCase(BitString keys, BitString iv1, BitString iv2, BitString iv3, BitString plainText, BitString cipherText)
        {
            Keys = keys;

            IV1 = iv1;
            IV2 = iv2;
            IV3 = iv3;

            PlainText = plainText;
            //PlainText1 = plainText1;
            //PlainText2 = plainText2;
            //PlainText3 = plainText3;

            CipherText = cipherText;
            //CipherText1 = cipherText1;
            //CipherText2 = cipherText2;
            //CipherText3 = cipherText3;
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
        //public BitString PlainText1 { get; set; }
        //public BitString PlainText2 { get; set; }
        //public BitString PlainText3 { get; set; }
        //public BitString CipherText1 { get; set; }
        //public BitString CipherText2 { get; set; }
        //public BitString CipherText3 { get; set; }



        public List<AlgoArrayResponseWithIvs> ResultsArray { get; set; } = new List<AlgoArrayResponseWithIvs>();

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
            if (((ExpandoObject)source).ContainsProperty("key1"))
            {
                Key1 = BitStringFromObject("key1", (ExpandoObject)source);
                Key2 = BitStringFromObject("key2", (ExpandoObject)source);
                Key3 = BitStringFromObject("key3", (ExpandoObject)source);
            }
            else
            {
                Keys = BitStringFromObject("key", (ExpandoObject)source);
            }

            CipherText = BitStringFromObject("ct", (ExpandoObject)source);
            //CipherText1 = BitStringFromObject("ct1", (ExpandoObject)source);
            //CipherText2 = BitStringFromObject("ct2", (ExpandoObject)source);
            //CipherText3 = BitStringFromObject("ct3", (ExpandoObject)source);

            PlainText = BitStringFromObject("pt", (ExpandoObject)source);
            //PlainText1 = BitStringFromObject("pt1", (ExpandoObject)source);
            //PlainText2 = BitStringFromObject("pt2", (ExpandoObject)source);
            //PlainText3 = BitStringFromObject("pt3", (ExpandoObject)source);

            IV1 = BitStringFromObject("iv1", (ExpandoObject)source);
            IV2 = BitStringFromObject("iv2", (ExpandoObject)source);
            IV3 = BitStringFromObject("iv3", (ExpandoObject)source);

        }

        private List<AlgoArrayResponseWithIvs> ResultsArrayToObject(dynamic resultsArray)
        {
            var list = new List<AlgoArrayResponseWithIvs>();

            foreach (dynamic item in resultsArray)
            {
                var response = new AlgoArrayResponseWithIvs();

                var key1 = BitStringFromObject("key1", (ExpandoObject)item);
                var key2 = BitStringFromObject("key2", (ExpandoObject)item);
                var key3 = BitStringFromObject("key3", (ExpandoObject)item);

                response.Keys = key1.ConcatenateBits(key2.ConcatenateBits(key3));
                response.PlainText = BitStringFromObject("pt", (ExpandoObject)item);

                response.CipherText = BitStringFromObject("ct", (ExpandoObject)item);

                response.IV1 = BitStringFromObject("iv1", (ExpandoObject)item);
                response.IV2 = BitStringFromObject("iv2", (ExpandoObject)item);
                response.IV3 = BitStringFromObject("iv3", (ExpandoObject)item);
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

            return valueAsBitString ?? new BitString(sourcePropertyValue.ToString());
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
                return true;
            }

            //if (PlainText1 == null && otherTypedTest.PlainText1 != null &&
            //    PlainText2 == null && otherTypedTest.PlainText2 != null &&
            //    PlainText3 == null && otherTypedTest.PlainText3 != null)
            //{
            //    PlainText1 = otherTypedTest.PlainText1;
            //    PlainText2 = otherTypedTest.PlainText2;
            //    PlainText3 = otherTypedTest.PlainText3;
            //    return true;
            //}

            if (CipherText == null && otherTypedTest.CipherText != null)
            {
                CipherText = otherTypedTest.CipherText;
                return true;
            }

            //if (CipherText1 == null && otherTypedTest.CipherText1 != null &&
            //    CipherText2 == null && otherTypedTest.CipherText2 != null &&
            //    CipherText3 == null && otherTypedTest.CipherText3 != null)
            //{
            //    CipherText1 = otherTypedTest.CipherText1;
            //    CipherText2 = otherTypedTest.CipherText2;
            //    CipherText3 = otherTypedTest.CipherText3;
            //    return true;
            //}

            return ResultsArray.Count != 0 && otherTypedTest.ResultsArray.Count != 0;
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
                //case "pt1":
                //    PlainText1 = new BitString(value);
                //    return true;
                //case "pt2":
                //    PlainText2 = new BitString(value);
                //    return true;
                //case "pt3":
                //    PlainText3 = new BitString(value);
                //    return true;
                case "ciphertext":
                case "ct":
                    CipherText = new BitString(value);
                    return true;
                //case "ct1":
                //    CipherText1 = new BitString(value);
                //    return true;
                //case "ct2":
                //    CipherText2 = new BitString(value);
                //    return true;
                //case "ct3":
                //    CipherText3 = new BitString(value);
                //    return true;
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