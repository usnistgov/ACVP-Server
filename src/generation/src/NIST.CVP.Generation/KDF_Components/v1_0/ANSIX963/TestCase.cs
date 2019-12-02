using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        public BitString Z { get; set; }
        public BitString SharedInfo { get; set; }
        public BitString KeyData { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "z":
                    Z = new BitString(value);
                    return true;

                case "sharedinfo":
                    SharedInfo = new BitString(value);
                    return true;

                case "key_data":
                    KeyData = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
