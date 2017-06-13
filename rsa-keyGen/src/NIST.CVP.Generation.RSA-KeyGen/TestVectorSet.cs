using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestVectorSet : ITestVectorSet
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        public TestVectorSet() { }

        public TestVectorSet(dynamic answers, dynamic prompts)
        {
            
        }

        public List<dynamic> AnswerProjection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<dynamic> PromptProjection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<dynamic> ResultProjection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
