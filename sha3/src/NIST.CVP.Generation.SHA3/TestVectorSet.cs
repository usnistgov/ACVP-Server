using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestVectorSet : ITestVectorSet
    {
        public string Algorithm { get; set; }
        [JsonIgnore]
        public string Mode { get; set; } = string.Empty;
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

        /// <summary>
        /// Expected answers (not sent to client)
        /// </summary>
        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("function", group.Function);
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("digestSize", group.DigestSize);

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
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
                                resultDict.Add("msg", result.Message);
                                resultDict.Add("md", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            testDict.Add("md", test.Digest.ToLittleEndianHex());
                            testDict.Add("msg", test.Message.ToLittleEndianHex());

                            if (group.TestType.ToLower() == "aft")
                            {
                                testDict.Add("len", test.Message.BitLength);
                            }
                            else if (group.TestType.ToLower() == "vot")
                            {
                                testDict.Add("outLen", test.Digest.BitLength);
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
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("function", group.Function);
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("digestSize", group.DigestSize);

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("msg", test.Message.ToLittleEndianHex());

                        if (group.TestType.ToLower() == "aft")
                        {
                            testDict.Add("len", test.Message.BitLength);
                        }
                        else if (group.TestType.ToLower() == "vot")
                        {
                            testDict.Add("outLen", test.Digest.BitLength);
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
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
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
                                resultDict.Add("msg", result.Message);
                                resultDict.Add("md", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            testDict.Add("md", test.Digest.ToLittleEndianHex());

                            if (group.TestType.ToLower() == "vot")
                            {
                                testDict.Add("outLen", test.Digest.BitLength);
                            }
                        }

                        list.Add(testObject);
                    }
                }

                return list;
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
