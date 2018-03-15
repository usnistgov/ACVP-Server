using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
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
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Direction);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("keyingOption", TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys));

                    if (group.TestType.ToLower() == "counter")
                    {
                        ((IDictionary<string, object>)updateObject).Add("overflow", group.OverflowCounter);
                        ((IDictionary<string, object>)updateObject).Add("incremental", group.IncrementalCounter);
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        var keys = test.Keys;
                        for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        if (group.TestType.ToLower() == "counter")
                        {
                            ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("iv", test.Iv);

                            if (group.Direction.ToLower().Equals("encrypt"))
                            {
                                ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                            }
                            else
                            {
                                ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                            }
                        }

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
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Direction);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("keyingOption", TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys));

                    if (group.TestType.ToLower() == "counter")
                    {
                        ((IDictionary<string, object>)updateObject).Add("overflow", group.OverflowCounter);
                        ((IDictionary<string, object>)updateObject).Add("incremental", group.IncrementalCounter);
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        var keys = test.Keys;
                        for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        ((IDictionary<string, object>)testObject).Add("dataLen", test.Length);
                        ((IDictionary<string, object>)testObject).Add("iv", test.Iv);

                        if (group.Direction.ToLower().Equals("encrypt"))
                        {
                            ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                        }

                        if (group.TestType.ToLower().Equals("counter"))
                        {
                            ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "counter")
                        {
                            if (IsSample)
                            {
                                if (group.Direction.ToLower().Equals("encrypt"))
                                {
                                    ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                                }
                                else
                                {
                                    ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                                }

                                ((IDictionary<string, object>)testObject).Add("ivs", test.Ivs);
                            }
                        }
                        else
                        {
                            if (group.Direction.ToLower().Equals("encrypt"))
                            {
                                ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                            }
                            else
                            {
                                ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                            }
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
