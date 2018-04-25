using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        [JsonProperty(PropertyName = "entropyInput")]
        public BitString EntropyInput { get; set; }
        [JsonProperty(PropertyName = "nonce")]
        public BitString Nonce { get; set; }
        [JsonProperty(PropertyName = "persoString")]
        public BitString PersoString { get; set; }
        [JsonProperty(PropertyName = "otherInput")]
        public List<OtherInput> OtherInput { get; set; } = new List<OtherInput>();
        [JsonProperty(PropertyName = "returnedBits")]
        public BitString ReturnedBits { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
      
            switch (name.ToLower())
            {
                case "entropyinput":
                    EntropyInput = new BitString(value);
                    return true;
                case "nonce":
                    Nonce = new BitString(value);
                    return true;
                case "personalizationstring":
                case "persostring":
                    PersoString = new BitString(value);
                    return true;
                case "returnedbits":
                    ReturnedBits = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}
