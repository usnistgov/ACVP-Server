using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        // Only used in FireHoseTests
        public int L;
        
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        public BitString KeyIn { get; set; }
        public BitString FixedData { get; set; }
        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }
        public int BreakLocation { get; set; }
        public BitString KeyOut { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keyin":
                case "ki":
                    KeyIn = new BitString(value);
                    return true;

                case "keyout":
                case "ko":
                    KeyOut = new BitString(value);
                    return true;

                case "iv":
                    IV = new BitString(value);
                    return true;

                case "fixedinputdata":
                case "databeforectrdata":
                    FixedData = new BitString(value);
                    return true;

                case "dataafterctrdata":
                    FixedData = FixedData.ConcatenateBits(new BitString(value));
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "fixedinputdatabytelen":
                case "databeforectrlen":
                    BreakLocation = int.Parse(value) * 8;
                    return true;
            }

            return false;
        }
    }
}
