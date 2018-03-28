using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "dataLen")]
        public int Length { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        public BitString Iv { get; set; }
        public List<BitString> Ivs { get; set; }

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

        public TestCase() { }

        public TestCase(string key, string pt, string ct, string iv)
        {
            Key = new BitString(key);
            PlainText = new BitString(pt);
            CipherText = new BitString(ct);
            Iv = new BitString(iv);
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "pt":
                case "plaintext":
                case "p":
                    PlainText = new BitString(value);
                    return true;

                case "ct":
                case "ciphertext":
                case "c":
                    CipherText = new BitString(value);
                    return true;

                case "iv":
                    Iv = new BitString(value);
                    return true;

                case "key":
                    Key = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}
