using Newtonsoft.Json;
using NIST.CVP.Crypto.KeyWrap.Enums;

namespace NIST.CVP.Generation.KeyWrap.TDES
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
        public override int KeyLength
        {
            get { return 192; }
            set { }
        }

        public override KeyWrapType KeyWrapType
        {
            get { return KeyWrapType.TDES_KW; }
            set { }
        }

        [JsonProperty(PropertyName = "numberOfKeys")]
        public int NumberOfKeys { get; set; }

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

        //public TestGroupTdes(JObject source) : this(source.ToObject<ExpandoObject>())
        //{

        //}

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            name = name.ToLower();

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name)
            {
                case "numberofkeys":
                    NumberOfKeys = intVal;
                    return true;
            }
            return false;
        }


    }
}