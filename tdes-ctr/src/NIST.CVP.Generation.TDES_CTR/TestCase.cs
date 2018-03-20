using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCase : ITestCase
    {
        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(string key, string pt, string ct, string iv)
        {
            Key = new BitString(key);
            PlainText = new BitString(pt);
            CipherText = new BitString(ct);
            Iv = new BitString(iv);
        }

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public int Length { get; set; }
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString Key1 { get; set; }
        public BitString Key2 { get; set; }
        public BitString Key3 { get; set; }
        public BitString CipherText { get; set; }
        public BitString Iv { get; set; }
        public List<BitString> Ivs { get; set; }

        public TDESKeys Keys
        {
            get
            {
                if (Key2 == null)
                {
                    return new TDESKeys(Key);
                }
                return new TDESKeys(Key1.ConcatenateBits(Key2.ConcatenateBits(Key3)));
            }
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            var expandoSource = (ExpandoObject) source;

            Deferred = expandoSource.GetTypeFromProperty<bool>("deferred");
            Length = expandoSource.GetTypeFromProperty<int>("dataLength");
            CipherText = expandoSource.GetBitStringFromProperty("cipherText");
            PlainText = expandoSource.GetBitStringFromProperty("plainText");
            Iv = expandoSource.GetBitStringFromProperty("iv");

            if (expandoSource.ContainsProperty("key1"))
            {
                Key1 = expandoSource.GetBitStringFromProperty("key1");
                Key2 = expandoSource.GetBitStringFromProperty("key2");
                Key3 = expandoSource.GetBitStringFromProperty("key3");
                Key = Key1.ConcatenateBits(Key2.ConcatenateBits(Key3));
            }
            else if (expandoSource.ContainsProperty("key"))
            {
                Key = expandoSource.GetBitStringFromProperty("key");
            }

            if (expandoSource.ContainsProperty("ivs"))
            {
                Ivs = new List<BitString>();
                foreach (var iv in source.ivs)
                {
                    Ivs.Add(new BitString(iv));
                }
            }
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
