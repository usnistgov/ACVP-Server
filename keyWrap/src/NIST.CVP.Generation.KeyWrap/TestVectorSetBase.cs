using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KeyWrap
{
    public abstract class TestVectorSetBase<TTestGroup, TTestCase> : ITestVectorSet
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {

        protected readonly DynamicBitStringPrintWithOptions _bitStringPrinter =
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsEmptyString
            );

        public TestVectorSetBase()
        {
        }

        public TestVectorSetBase(dynamic answers, dynamic prompts)
        {
            SetAnswerAndPrompts(answers, prompts);
        }

        public void SetAnswerAndPrompts(dynamic answers, dynamic prompts)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = (TTestGroup) Activator.CreateInstance(typeof(TTestGroup), answer);
                //var group = new TTestGroup(answer);

                TestGroups.Add(@group);
            }

            foreach (var prompt in prompts.testGroups)
            {
                var promptGroup = (TTestGroup) Activator.CreateInstance(typeof(TTestGroup), prompt);
                //var promptGroup = new TTestGroup(prompt);
                var matchingAnswerGroup = TestGroups.FirstOrDefault(g => g.Equals(promptGroup));
                if (matchingAnswerGroup != null)
                {
                    if (!matchingAnswerGroup.MergeTests(promptGroup.Tests))
                    {
                        throw new Exception("Could not reconstitute TestVectorSet from supplied answers and prompts");
                    }
                }
            }
        }

        public string Algorithm { get; set; }
        [JsonIgnore]
        public string Mode { get; set; }
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

        public dynamic ToDynamic()
        {
            dynamic vectorSetObject = new ExpandoObject();
            ((IDictionary<string, object>)vectorSetObject).Add("answerProjection", AnswerProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("testGroups", PromptProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("resultProjection", ResultProjection);
            return vectorSetObject;
        }

        protected dynamic BuildGroupInformation(TTestGroup group)
        {
            dynamic updateObject = new ExpandoObject();
            ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
            ((IDictionary<string, object>)updateObject).Add("direction", group.Direction);
            ((IDictionary<string, object>)updateObject).Add("kwCipher", group.KwCipher);
            ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
            ((IDictionary<string, object>)updateObject).Add("ptLen", group.PtLen);
            return updateObject;
        }
    }
}
