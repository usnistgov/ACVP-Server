using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public class TestVectorValidation
    {
        [JsonProperty(PropertyName = "vsId")]
        public int VectorSetId { get; set; }

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
