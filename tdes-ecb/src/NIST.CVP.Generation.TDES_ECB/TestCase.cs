using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }

        [JsonIgnore]
        public BitString Key { get; set; }

        public BitString Key1
        {
            get => Key?.MSBSubstring(0, 64);
            set => Key = Key == null ?
                value.ConcatenateBits(new BitString(128)) :
                value.ConcatenateBits(Key.MSBSubstring(64, 128));
        }

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
        
        public List<AlgoArrayResponse> ResultsArray { get; set; }

        public TestCase() { }

        public TestCase(string key, string plainText, string cipherText)
        {
            Key = new BitString(key);
            PlainText = new BitString(plainText);
            CipherText = new BitString(cipherText);
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
