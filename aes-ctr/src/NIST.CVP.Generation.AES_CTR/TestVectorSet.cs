using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
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

        public TestVectorSet(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);

                TestGroups.Add(group);
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
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("direction", group.Direction);
                    updateDict.Add("keyLen", group.KeyLength);
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("overflow", group.OverflowCounter);
                    updateDict.Add("incremental", group.IncrementalCounter);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("key", test.Key);
                        testDict.Add("dataLen", test.Length);
                        testDict.Add("deferred", test.Deferred);
                        testDict.Add("iv", test.IV);
                        testDict.Add("plainText", test.PlainText);
                        testDict.Add("cipherText", test.CipherText);

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
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("direction", group.Direction);
                    updateDict.Add("keyLen", group.KeyLength);
                    updateDict.Add("testType", group.TestType);

                    if (group.TestType.ToLower() == "counter")
                    {
                        updateDict.Add("overflow", group.OverflowCounter);
                        updateDict.Add("incremental", group.IncrementalCounter);
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("key", test.Key);

                        if (!(group.TestType.ToLower().Equals("gfsbox") ||
                              group.TestType.ToLower().Equals("keysbox") ||
                              group.TestType.ToLower().Equals("vartxt") ||
                              group.TestType.ToLower().Equals("varkey")))
                        {
                            testDict.Add("dataLen", test.Length);
                        }

                        testDict.Add("iv", test.IV);

                        if (group.Direction.ToLower().Equals("encrypt"))
                        {
                            testDict.Add("plainText", test.PlainText);
                        }
                        else
                        {
                            testDict.Add("cipherText", test.CipherText);
                        }

                        if (group.TestType.ToLower().Equals("counter"))
                        {
                            testDict.Add("deferred", test.Deferred);
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
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "counter")
                        {
                            testDict.Add("ivs", test.IVs);
                        }

                        if (group.Direction.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            testDict.Add("cipherText", test.CipherText);
                        }
                        else
                        {
                            testDict.Add("plainText", test.PlainText);
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
