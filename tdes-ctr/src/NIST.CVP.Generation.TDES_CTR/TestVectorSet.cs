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
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("keyingOption", TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys));

                    if (group.TestType.ToLower() == "counter")
                    {
                        updateDict.Add("overflow", group.OverflowCounter);
                        updateDict.Add("incremental", group.IncrementalCounter);
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        var keys = test.Keys;
                        for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            testDict.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        testDict.Add("dataLen", test.Length);
                        testDict.Add("iv", test.Iv);

                        if (group.TestType.ToLower() == "counter")
                        {
                            testDict.Add("deferred", test.Deferred);
                        }

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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    var updateDict = ((IDictionary<string, object>) updateObject);
                     updateDict.Add("tgId", group.TestGroupId);
                     updateDict.Add("direction", group.Direction);
                     updateDict.Add("testType", group.TestType);
                     updateDict.Add("keyingOption", TdesHelpers.GetKeyingOptionFromNumberOfKeys(group.NumberOfKeys));

                    if (group.TestType.ToLower() == "counter")
                    {
                        updateDict.Add("overflow", group.OverflowCounter);
                        updateDict.Add("incremental", group.IncrementalCounter);
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        var keys = test.Keys;
                        for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            testDict.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        testDict.Add("dataLen", test.Length);
                        testDict.Add("iv", test.Iv);

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

                        if (group.Direction.ToLower().Equals("encrypt"))
                        {
                            testDict.Add("cipherText", test.CipherText);
                        }
                        else
                        {
                            testDict.Add("plainText", test.PlainText);
                        }

                        if (group.TestType.ToLower() == "counter")
                        {
                            testDict.Add("ivs", test.Ivs);
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
