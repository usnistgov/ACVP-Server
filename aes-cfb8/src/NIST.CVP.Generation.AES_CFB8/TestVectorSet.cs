using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB8
{
    public class TestVectorSet : ITestVectorSet
    {
        public TestVectorSet()
        {
        }

        public TestVectorSet(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                
                TestGroups.Add(group);
            }
        }

        public string Algorithm { get; set; } = "AES-CFB8";
        [JsonIgnore]
        public string Mode { get; set; } = string.Empty;
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        /// <summary>
        /// Expected answers (not sent to client)
        /// </summary>
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
                    updateDict.Add("direction", group.Function);
                    updateDict.Add("testType", group.TestType);

                    updateDict.Add("keyLen", group.KeyLength);
                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>)testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                var resultDict = ((IDictionary<string, object>) resultObject);
                                resultDict.Add("iv", result.IV);
                                resultDict.Add("key", result.Key);
                                resultDict.Add("plainText", result.PlainText);
                                resultDict.Add("cipherText", result.CipherText);

                                resultsArray.Add(resultObject);
                            }
                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            testDict.Add("iv", test.IV);
                            testDict.Add("key", test.Key);
                            ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                            ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                            ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                            ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
                        }
                        
                        tests.Add(testObject);
                    }
                    list.Add(updateObject);
                    
                }

                return list;
            }
        }

        /// <summary>
        /// What the client receives (should not include expected answers)
        /// </summary>
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
                    updateDict.Add("direction", group.Function);
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("keyLen", group.KeyLength);
                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        testDict.Add("iv", test.IV);
                        testDict.Add("key", test.Key);
                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            testDict.Add("plainText", test.PlainText);
                        }
                        if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            testDict.Add("cipherText", test.CipherText);
                        }
                        
                        tests.Add(testObject);
                    }
                    list.Add(updateObject);
                }

                return list;
            }
        }

        /// <summary>
        /// Debug projection (internal), as well as potentially sample projection (sent to client)
        /// </summary>
        [JsonProperty(PropertyName = "testResults")]
        public List<dynamic> ResultProjection
        {
            get
            {
                var groups = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic groupObject = new ExpandoObject();
                    var groupDict = (IDictionary<string, object>)groupObject;
                    groupDict.Add("tgId", group.TestGroupId);

                    var tests = new List<dynamic>();
                    groupDict.Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                var resultDict = ((IDictionary<string, object>) resultObject);
                                resultDict.Add("iv", result.IV);
                                resultDict.Add("key", result.Key);
                                if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    resultDict.Add("plainText", result.PlainText);
                                    resultDict.Add("cipherText", result.CipherText);
                                }
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    resultDict.Add("cipherText", result.CipherText);
                                    resultDict.Add("plainText", result.PlainText);
                                }
                                resultsArray.Add(resultObject);
                            }
                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                testDict.Add("cipherText", test.CipherText);
                            }
                            if (test.FailureTest)
                            {
                                testDict.Add("decryptFail", true);
                            }
                            else
                            {
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    testDict.Add("plainText", test.PlainText);
                                }
                            }
                        }
                        
                        tests.Add(testObject);
                    }

                    groups.Add(groupObject);
                }
                return groups;
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
