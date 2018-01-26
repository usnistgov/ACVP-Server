using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
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

        public TestVectorSet(dynamic answers, dynamic prompts)
        {
            foreach(var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                TestGroups.Add(group);
            }

            foreach(var prompt in prompts.testGroups)
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

        /// <summary>
        /// Expected answers (not sent to client)
        /// </summary>
        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach(var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("function", SHAEnumHelpers.ModeToString(group.Function));
                    ((IDictionary<string, object>)updateObject).Add("digestSize", SHAEnumHelpers.DigestToString(group.DigestSize));
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType.ToUpper());

                    if (group.TestType.ToLower() != "mct")
                    {
                        //((IDictionary<string, object>)updateObject).Add("bitOriented", group.BitOriented);
                        //((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach(var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                ((IDictionary<string, object>) resultObject).Add("msg", result.Message);
                                ((IDictionary<string, object>) resultObject).Add("md", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            ((IDictionary<string, object>) testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("md", test.Digest);
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
                    ((IDictionary<string, object>)updateObject).Add("function", SHAEnumHelpers.ModeToString(group.Function));
                    ((IDictionary<string, object>)updateObject).Add("digestSize", SHAEnumHelpers.DigestToString(group.DigestSize));
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType.ToUpper());

                    if (group.TestType.ToLower() != "mct")
                    {
                        //((IDictionary<string, object>)updateObject).Add("bitOriented", group.BitOriented);
                        //((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("msg", test.Message);

                        if (group.TestType.ToLower() != "mct")
                        {
                            ((IDictionary<string, object>)testObject).Add("len", test.Message.BitLength);
                        }

                        //if (group.TestType.ToLower() == "mct")
                        //{
                        //    var resultsArray = new List<dynamic>();

                        //    // Add two messages to MCT file for simple checking
                        //    var resultOne = test.ResultsArray[0];
                        //    dynamic resultObjectOne = new ExpandoObject();
                        //    ((IDictionary<string, object>)resultObjectOne).Add("message", resultOne.Message);

                        //    var resultTwo = test.ResultsArray[1];
                        //    dynamic resultObjectTwo = new ExpandoObject();
                        //    ((IDictionary<string, object>)resultObjectTwo).Add("message", resultTwo.Message);

                        //    resultsArray.Add(resultObjectOne);
                        //    resultsArray.Add(resultObjectTwo);
                        //    ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        //}

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
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                ((IDictionary<string, object>)resultObject).Add("msg", result.Message);
                                ((IDictionary<string, object>)resultObject).Add("md", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("md", test.Digest);
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
