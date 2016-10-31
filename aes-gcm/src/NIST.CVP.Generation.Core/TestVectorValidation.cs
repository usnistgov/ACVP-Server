using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    public class TestVectorValidation
    {
        public string Disposition
        {
            get
            {
                if (Validations.All(v => v.Result == "passed"))
                {
                    return "passed";
                }
                return Validations.First(v => v.Result != "passed").Result;
            }
        }

        [JsonProperty(PropertyName = "tests")]
        public List<TestCaseValidation> Validations { get; set; }
    }
}
