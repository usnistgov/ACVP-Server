using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CBC.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }
        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }

        [JsonProperty(PropertyName = "key1")]
        public BitString Key1
        {
            get => Key?.MSBSubstring(0, 64);
            set => Key = Key == null ?
                value.ConcatenateBits(new BitString(128)) :
                value.ConcatenateBits(Key.MSBSubstring(64, 128));
        }

        [JsonProperty(PropertyName = "key2")]
        public BitString Key2
        {
            get
            {
                if (Key == null) return null;
                if (Key.BitLength == 64) return Key1;
                return Key.MSBSubstring(64, 64);
            }
            set => Key = Key == null ?
                new BitString(64).ConcatenateBits(value).ConcatenateBits(new BitString(64)) :
                Key.MSBSubstring(0, 64).ConcatenateBits(value).ConcatenateBits(Key.MSBSubstring(128, 64));
        }

        [JsonProperty(PropertyName = "key3")]
        public BitString Key3
        {
            get
            {
                if (Key == null) return null;
                if (Key.BitLength == 64) return Key1;
                if (Key.BitLength == 128) return Key1;
                return Key.MSBSubstring(128, 64);
            }
            set => Key = Key == null ?
                new BitString(128).ConcatenateBits(value) :
                Key.MSBSubstring(0, 128).ConcatenateBits(value);
        }

        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }
        [JsonProperty(PropertyName = "iv")]
        public BitString Iv { get; set; }
        [JsonProperty(PropertyName = "resultsArray")]
        public List<AlgoArrayResponse> ResultsArray { get; set; }

        private BitString _keys;

        [JsonIgnore]
        public BitString Key
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
                case "initialization vector":
                case "iv":
                    Iv = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}
