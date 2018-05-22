using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core
{
    public class TestVectorValidation
    {
        public Disposition Disposition
        {
            get
            {
                if (Validations.All(v => v.Result == Disposition.Passed))
                {
                    return Disposition.Passed;
                }
                return Validations.First(v => v.Result != Disposition.Passed).Result;
            }
        }

        [JsonProperty(PropertyName = "tests")]
        public List<TestCaseValidation> Validations { get; set; }
    }
}
