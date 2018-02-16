using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.RSA_SigVer
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
                foreach(var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("sigType", EnumHelpers.GetEnumDescriptionFromEnum(group.Mode));
                    ((IDictionary<string, object>)updateObject).Add("modulo", group.Modulo);
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);

                    if (group.Mode == SignatureSchemes.Pss)
                    {
                        ((IDictionary<string, object>)updateObject).Add("saltLen", group.SaltLen);
                    }

                    ((IDictionary<string, object>)updateObject).Add("n", group.Key.PubKey.N);
                    ((IDictionary<string, object>)updateObject).Add("e", group.Key.PubKey.E);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach(var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("sigResult", EnumHelpers.GetEnumDescriptionFromEnum(test.FailureTest ? Disposition.Failed : Disposition.Passed));
                        ((IDictionary<string, object>)testObject).Add("reason", test.Reason.GetName());

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
                    ((IDictionary<string, object>)updateObject).Add("sigType", EnumHelpers.GetEnumDescriptionFromEnum(group.Mode));
                    ((IDictionary<string, object>)updateObject).Add("modulo", group.Modulo);
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);

                    if (group.Mode == SignatureSchemes.Pss)
                    {
                        ((IDictionary<string, object>)updateObject).Add("saltLen", group.SaltLen);
                    }

                    ((IDictionary<string, object>)updateObject).Add("n", group.Key.PubKey.N);
                    ((IDictionary<string, object>)updateObject).Add("e", group.Key.PubKey.E);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("message", test.Message);
                        ((IDictionary<string, object>)testObject).Add("signature", test.Signature);

                        if (group.Mode == SignatureSchemes.Pss)
                        {
                            ((IDictionary<string, object>)testObject).Add("salt", test.Salt);
                        }

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
                foreach(var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach(var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("sigResult", EnumHelpers.GetEnumDescriptionFromEnum(test.FailureTest ? Disposition.Failed : Disposition.Passed));
                        ((IDictionary<string, object>)testObject).Add("reason", test.Reason.GetName());

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
