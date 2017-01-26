using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class TestVectorSet : ITestVectorSet
    {
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

        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("msgLen", group.MessageLength);
                    ((IDictionary<string, object>)updateObject).Add("digLen", group.DigestLength);
                    ((IDictionary<string, object>)updateObject).Add("bitOriented", group.BitOriented);
                    ((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if(group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach(var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                ((IDictionary<string, object>)resultObject).Add("message", result.Message);
                                ((IDictionary<string, object>)resultObject).Add("digest", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("digest", test.Digest);
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

        [JsonProperty(PropertyName = "testGroups")]
        public List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("msgLen", group.MessageLength);
                    ((IDictionary<string, object>)updateObject).Add("digLen", group.DigestLength);
                    ((IDictionary<string, object>)updateObject).Add("bitOriented", group.BitOriented);
                    ((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>) updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>) testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>) testObject).Add("message", test.Message);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        [JsonProperty(PropertyName = "testResults")]
        public dynamic ResultProjection
        {
            get
            {
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("digest", test.Digest);

                        if (test.FailureTest)
                        {
                            ((IDictionary<string, object>) testObject).Add("hashFail", true);
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
