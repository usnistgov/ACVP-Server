using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KAS
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase, TKasDsaAlgoAttributes> : ITestVectorSet
        where TTestGroup : TestGroupBase<TKasDsaAlgoAttributes>, new()
        where TTestCase : TestCaseBase, new()
        where TKasDsaAlgoAttributes : IKasDsaAlgoAttributes
    {

        protected readonly DynamicBitStringPrintWithOptions DynamicBitStringPrintWithOptions = new DynamicBitStringPrintWithOptions(PrintOptionBitStringNull.DoNotPrintProperty, PrintOptionBitStringEmpty.PrintAsEmptyString);
        
        public string Algorithm { get; set; }
        [JsonIgnore]
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        public dynamic ToDynamic()
        {
            dynamic vectorSetObject = new ExpandoObject();
            ((IDictionary<string, object>)vectorSetObject).Add("answerProjection", AnswerProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("testGroups", PromptProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("resultProjection", ResultProjection);
            return vectorSetObject;
        }

        public abstract List<dynamic> AnswerProjection { get; }
        [JsonProperty(PropertyName = "testGroups")]
        public abstract List<dynamic> PromptProjection { get; }
        [JsonProperty(PropertyName = "testResults")]
        public abstract List<dynamic> ResultProjection { get; }

        protected void SetAnswers(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = (TTestGroup)Activator.CreateInstance(typeof(TTestGroup), answer);
                
                TestGroups.Add(group);
            }
        }
    }
}