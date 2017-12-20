using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestVectorSet : ITestVectorSet
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        public TestVectorSet() { }

        public TestVectorSet(dynamic answers, dynamic prompts)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                TestGroups.Add(group);
            }

            foreach (var prompt in prompts.testGroups)
            {
                var promptGroup = new TestGroup(prompt);
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

        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("curve", EnumHelpers.GetEnumDescriptionFromEnum(group.DomainParameters.CurveE.CurveName));
                    ((IDictionary<string, object>)updateObject).Add("secretGenerationMode", EnumHelpers.GetEnumDescriptionFromEnum(group.DomainParameters.SecretGeneration));

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        [JsonProperty(PropertyName = "testGroups")]
        public List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("curve", EnumHelpers.GetEnumDescriptionFromEnum(group.DomainParameters.CurveE.CurveName));
                    ((IDictionary<string, object>)updateObject).Add("secretGenerationMode", EnumHelpers.GetEnumDescriptionFromEnum(group.DomainParameters.SecretGeneration));

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        [JsonProperty(PropertyName = "testResults")]
        public List<dynamic> ResultProjection
        {
            get
            {
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (IsSample)
                        {
                            ((IDictionary<string, object>)testObject).Add("Qx", test.KeyPair.PublicQ.X);
                            ((IDictionary<string, object>)testObject).Add("Qy", test.KeyPair.PublicQ.Y);
                            ((IDictionary<string, object>)testObject).Add("d", test.KeyPair.PrivateD);
                        }

                        tests.Add(testObject);
                    }
                }

                return tests;
            }
        }

        public dynamic ToDynamic()
        {
            dynamic vectorSetObject = new ExpandoObject();
            ((IDictionary<string, object>)vectorSetObject).Add("answerProjection", AnswerProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("testGroups", PromptProjection);
            ((IDictionary<string, object>)vectorSetObject).Add("resultProjection", ResultProjection);
            return vectorSetObject;
        }
    }
}
