using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace NIST.CVP.Generation.Core
{
    public class JsonOutputDetail
    {
        public IContractResolver Resolver { get; set; }
        public string OutputFileName { get; set; }
    }
}
