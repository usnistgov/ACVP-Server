using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestVectorSetBase<TTestGroup> : ITestVectorSet
        where TTestGroup : TestGroupBase, new()
    {
        protected readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions =
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsEmptyString);

        public string Algorithm { get; set; }
        [JsonIgnore]
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        /// <summary>
        /// Expected answers (not sent to client)
        /// </summary>
        public abstract List<dynamic> AnswerProjection { get; }

        /// <summary>
        /// What the client receives (should not include expected answers)
        /// </summary>
        [JsonProperty(PropertyName = "testGroups")]
        public abstract List<dynamic> PromptProjection { get; }

        /// <summary>
        /// Debug projection (internal), as well as potentially sample projection (sent to client)
        /// </summary>
        [JsonProperty(PropertyName = "testResults")]
        public abstract List<dynamic> ResultProjection { get; }

        protected TestVectorSetBase() { }

        protected TestVectorSetBase(dynamic answers)
        {
            SetAnswers(answers);
        }

        public void SetAnswers(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = (TTestGroup)Activator.CreateInstance(typeof(TTestGroup), answer);

                TestGroups.Add(group);
            }
        }
    }
}