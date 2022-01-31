using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore]
        public BigInteger SharedSecret { get; set; }

        [JsonProperty("Z")]
        public BitString Z
        {
            get => new BitString(SharedSecret, ParentGroup?.FieldSize ?? 0).PadToModulusMsb(8);
            set => SharedSecret = value.ToPositiveBigInteger();
        }

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
                    SharedSecret = new BitString(value).ToPositiveBigInteger();
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
