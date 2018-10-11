using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestCase : TestCaseBase<TestGroup, TestCase>
    {
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

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "k":
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
                case "p":
                    PlainText = new BitString(value);
                    return true;
                case "ciphertext":
                case "ct":
                case "c":
                    CipherText = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}