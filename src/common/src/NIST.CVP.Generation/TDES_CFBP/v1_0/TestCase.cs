using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Generation.TDES_CFBP.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        private BitString _keys;

        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore]
        public bool? TestPassed { get; set; }

        [JsonIgnore]
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }

        [JsonIgnore]
        public BitString Keys
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

        public BitString Key1
        {
            get => Keys?.MSBSubstring(0, 64);
            set => Keys = Keys == null ?
                value.ConcatenateBits(new BitString(128)) :
                value.ConcatenateBits(Keys.MSBSubstring(64, 128));
        }

        public BitString Key2
        {
            get
            {
                if (Keys == null) return null;
                if (Keys.BitLength == 64) return Key1;
                return Keys.MSBSubstring(64, 64);
            }
            set => Keys = Keys == null ?
                new BitString(64).ConcatenateBits(value).ConcatenateBits(new BitString(64)) :
                Keys.MSBSubstring(0, 64).ConcatenateBits(value).ConcatenateBits(Keys.MSBSubstring(128, 64));
        }

        public BitString Key3
        {
            get
            {
                if (Keys == null) return null;
                if (Keys.BitLength == 64) return Key1;
                if (Keys.BitLength == 128) return Key1;
                return Keys.MSBSubstring(128, 64);
            }
            set => Keys = Keys == null ?
                new BitString(128).ConcatenateBits(value) :
                Keys.MSBSubstring(0, 128).ConcatenateBits(value);
        }

        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }

        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }
        
        [JsonProperty(PropertyName = "payloadLen", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int PayloadLen
        {
            get
            {
                // We only want to populate a dataLen if it matters (like in cfb1)
                if (ParentGroup?.AlgoMode != AlgoMode.TDES_CFBP1_v1_0)
                {
                    return 0;
                }

                if (PlainText != null && PlainText.BitLength != 0)
                {
                    return PlainText.BitLength;
                }
                if (CipherText != null && CipherText.BitLength != 0)
                {
                    return CipherText.BitLength;
                }

                return 0;
            }
        }

        // TODO get rid of properties
        #region only used on kats TODO remove
        public BitString PlainText1 { get; set; }
        public BitString PlainText2 { get; set; }
        public BitString PlainText3 { get; set; }
        public BitString CipherText1 { get; set; }
        public BitString CipherText2 { get; set; }
        public BitString CipherText3 { get; set; }
        #endregion only used on kats TODO remove

        public List<AlgoArrayResponse> ResultsArray { get; set; }
        
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
                    IV = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}