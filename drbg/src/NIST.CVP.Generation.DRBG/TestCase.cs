using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class TestCase : ITestCase
    {

        public TestCase()
        {
            
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public int TestCaseId { get; set; }
        public bool FailureTest => false;
        public bool Deferred => false;
        public BitString EntropyInput { get; set; }
        public BitString Nonce { get; set; }
        public BitString PersoString { get; set; }
        public List<OtherInput> OtherInput { get; set; } = new List<OtherInput>();
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

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;
            EntropyInput = expandoSource.GetBitStringFromProperty("entropyInput");
            Nonce = expandoSource.GetBitStringFromProperty("nonce");
            PersoString = expandoSource.GetBitStringFromProperty("persoString");

            if (expandoSource.ContainsProperty("otherInput"))
            {
                OtherInput = OtherInputToObject(source.otherInput);
            }

            ReturnedBits = expandoSource.GetBitStringFromProperty("returnedBits");
        }

        private List<OtherInput> OtherInputToObject(dynamic otherInput)
        {
            List<OtherInput> list = new List<OtherInput>();

            foreach (dynamic item in otherInput)
            {
                ExpandoObject expandoItem = (ExpandoObject) item;

                OtherInput response = new OtherInput
                {
                    AdditionalInput = expandoItem.GetBitStringFromProperty("additionalInput"),
                    EntropyInput = expandoItem.GetBitStringFromProperty("entropyInput")
                };

                list.Add(response);
            }

            return list;
        }
    }
}
