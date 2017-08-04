using Newtonsoft.Json;
using NIST.CVP.Crypto.KeyWrap.Enums;

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


        public override  KeyWrapType KeyWrapType
        {
            get { return KeyWrapType.AES_KW; }
            set { }
        }

        public override int GetHashCode()
        {
            return
                $"{KeyWrapType}|{TestType}|{Direction}|{KwCipher}|{KeyLength}|{PtLen}"
                    .GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherGroup = obj as TestGroup;
            if (otherGroup == null)
            {
                return false;
            }
            return this.GetHashCode() == otherGroup.GetHashCode();
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

    }
}
