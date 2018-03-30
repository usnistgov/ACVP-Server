using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestGroup : TestGroupBase<TestCase>
    {
        public TestGroup()
        {
            
        }

        public TestGroup(dynamic source) 
        {
            LoadSource(source);
        }

        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength { get; set; }

        private bool _withPadding = false;

        public override  KeyWrapType KeyWrapType
        {
            get => _withPadding ? KeyWrapType.AES_KWP : KeyWrapType.AES_KW;
            set => _withPadding = (value == KeyWrapType.AES_KWP);
        }

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            name = name.ToLower();

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name)
            {
                case "keylen":
                case "keylength":
                    KeyLength = intVal;
                    return true;
                case "ptlen":
                case "plaintext length":
                    PtLen = intVal;
                    return true;
            }
            return false;
        }

        protected override void LoadSource(dynamic source)
        {
            TestGroupId = (int) source.tgId;
            TestType = source.testType;
            Direction = source.direction;
            KwCipher = source.kwCipher;
            KeyLength = source.keyLen;
            PtLen = source.ptLen;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

    }
}
