using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SRTP
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        [JsonIgnore]
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }

        public BitString MasterKey { get; set; }
        public BitString MasterSalt { get; set; }
        public BitString Index { get; set; }
        public BitString SrtcpIndex { get; set; }
        public BitString SrtpKe { get; set; }
        public BitString SrtpKa { get; set; }
        public BitString SrtpKs { get; set; }
        public BitString SrtcpKe { get; set; }
        public BitString SrtcpKa { get; set; }
        public BitString SrtcpKs { get; set; }

        // For FireHoseTests
        public BitString Kdr { get; private set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "k_master":
                    MasterKey = new BitString(value);
                    return true;

                case "master_salt":
                    MasterSalt = new BitString(value);
                    return true;

                case "kdr":
                    Kdr = new BitString(value);
                    return true;

                case "index":
                    Index = new BitString(value);
                    return true;

                case "index (srtcp)":
                    SrtcpIndex = new BitString(value);
                    return true;

                case "srtp k_e":
                    SrtpKe = new BitString(value);
                    return true;

                case "srtp k_a":
                    SrtpKa = new BitString(value);
                    return true;

                case "srtp k_s":
                    SrtpKs = new BitString(value);
                    return true;

                case "srtcp k_e":
                    SrtcpKe = new BitString(value);
                    return true;

                case "srtcp k_a":
                    SrtcpKa = new BitString(value);
                    return true;

                case "srtcp k_s":
                    SrtcpKs = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
