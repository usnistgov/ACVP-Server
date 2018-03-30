using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SNMP
{
    public class TestCase : ITestCase
    {
        public TestCase() { }
        
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
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public string Password { get; set; }
        public BitString SharedKey { get; set; }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject) source;
            
            Password = expandoSource.GetTypeFromProperty<string>("password");
            SharedKey = expandoSource.GetBitStringFromProperty("sharedKey");
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "password":
                    Password = value;
                    return true;

                case "shared_key":
                    SharedKey = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
