using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        
        public BitString NInit { get; set; }
        public BitString NResp { get; set; }
        public BitString Gir { get; set; }
        public BitString GirNew { get; set; }
        public BitString SpiInit { get; set; }
        public BitString SpiResp { get; set; }
        public BitString SKeySeed { get; set; }
        public BitString DerivedKeyingMaterial { get; set; }
        public BitString DerivedKeyingMaterialChild { get; set; }
        public BitString DerivedKeyingMaterialDh { get; set; }
        public BitString SKeySeedReKey { get; set; }
        
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

                case "g^ir":
                    Gir = new BitString(value);
                    return true;

                case "g^ir (new)":
                    GirNew = new BitString(value);
                    return true;

                case "spii":
                    SpiInit = new BitString(value);
                    return true;

                case "spir":
                    SpiResp = new BitString(value);
                    return true;

                case "skeyseed":
                    SKeySeed = new BitString(value);
                    return true;

                case "dkm":
                    DerivedKeyingMaterial = new BitString(value);
                    return true;

                case "dkm(child sa)":
                    DerivedKeyingMaterialChild = new BitString(value);
                    return true;

                case "dkm(child sa d-h)":
                    DerivedKeyingMaterialDh = new BitString(value);
                    return true;

                case "skeyseed(rekey)":
                    SKeySeedReKey = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
