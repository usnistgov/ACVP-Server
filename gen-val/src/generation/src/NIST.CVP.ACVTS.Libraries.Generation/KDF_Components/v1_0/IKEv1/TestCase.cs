using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        public BitString CkyInit { get; set; }
        public BitString CkyResp { get; set; }
        public BitString NInit { get; set; }
        public BitString NResp { get; set; }
        public BitString Gxy { get; set; }
        public BitString PreSharedKey { get; set; }
        public BitString SKeyId { get; set; }
        public BitString SKeyIdD { get; set; }
        public BitString SKeyIdA { get; set; }
        public BitString SKeyIdE { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "ninit":
                case "ni":
                    NInit = new BitString(value);
                    return true;

                case "nresp":
                case "nr":
                    NResp = new BitString(value);
                    return true;

                case "cky_i":
                    CkyInit = new BitString(value);
                    return true;

                case "cky_r":
                    CkyResp = new BitString(value);
                    return true;

                case "g^xy":
                    Gxy = new BitString(value);
                    return true;

                case "pre-shared-key":
                    PreSharedKey = new BitString(value);
                    return true;

                case "skeyid":
                    SKeyId = new BitString(value);
                    return true;

                case "skeyid_a":
                    SKeyIdA = new BitString(value);
                    return true;

                case "skeyid_d":
                    SKeyIdD = new BitString(value);
                    return true;

                case "skeyid_e":
                    SKeyIdE = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
