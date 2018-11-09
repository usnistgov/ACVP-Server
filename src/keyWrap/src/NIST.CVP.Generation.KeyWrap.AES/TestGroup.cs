using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;

namespace NIST.CVP.Generation.KeyWrap.AES
{
    public class TestGroup : TestGroupBase<TestGroup, TestCase>
    {
        [JsonProperty(PropertyName = "keyLen")]
        public override int KeyLength { get; set; }

        [JsonIgnore]
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

            if (!int.TryParse(value, out var intVal))
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
                    PayloadLen = intVal;
                    return true;
            }

            return false;
        }
    }
}
