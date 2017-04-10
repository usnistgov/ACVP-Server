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
                    ((IDictionary<string, object>)updateObject).Add("function", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("digestSize", group.DigestSize);

                    ((IDictionary<string, object>)updateObject).Add("bitOrientedInput", group.BitOrientedInput);
                    ((IDictionary<string, object>)updateObject).Add("bitOrientedOutput", group.BitOrientedOutput);
                    ((IDictionary<string, object>)updateObject).Add("minOutputLength", group.MinOutputLength);
                    ((IDictionary<string, object>)updateObject).Add("maxOutputLength", group.MaxOutputLength);
                    ((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);

                    //if (group.TestType.ToLower() != "mct")
                    //{
                    //    ((IDictionary<string, object>)updateObject).Add("bitOrientedInput", group.BitOrientedInput);
                    //}

                    //if (group.Function.ToLower() != "shake")
                    //{
                    //    ((IDictionary<string, object>)updateObject).Add("bitOrientedOutput", group.BitOrientedOutput);
                    //    ((IDictionary<string, object>)updateObject).Add("minOutputLength", group.MinOutputLength);
                    //    ((IDictionary<string, object>)updateObject).Add("maxOutputLength", group.MaxOutputLength);
                    //}

                    //if (group.TestType.ToLower() == "aft")
                    //{
                    //    ((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);
                    //}

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>) updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                ((IDictionary<string, object>) resultObject).Add("message", result.Message);
                                ((IDictionary<string, object>) resultObject).Add("digest", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            ((IDictionary<string, object>) testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("digest", test.Digest.ToLittleEndianHex());
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
                    ((IDictionary<string, object>)updateObject).Add("function", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("digestSize", group.DigestSize);

                    ((IDictionary<string, object>)updateObject).Add("bitOrientedInput", group.BitOrientedInput);
                    ((IDictionary<string, object>)updateObject).Add("bitOrientedOutput", group.BitOrientedOutput);
                    ((IDictionary<string, object>)updateObject).Add("minOutputLength", group.MinOutputLength);
                    ((IDictionary<string, object>)updateObject).Add("maxOutputLength", group.MaxOutputLength);
                    ((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);

                    //if (group.TestType.ToLower() != "mct")
                    //{
                    //    ((IDictionary<string, object>)updateObject).Add("bitOrientedInput", group.BitOrientedInput);
                    //}

                    //if (group.Function.ToLower() != "shake")
                    //{
                    //    ((IDictionary<string, object>)updateObject).Add("bitOrientedOutput", group.BitOrientedOutput);
                    //    ((IDictionary<string, object>)updateObject).Add("minOutputLength", group.MinOutputLength);
                    //    ((IDictionary<string, object>)updateObject).Add("maxOutputLength", group.MaxOutputLength);
                    //}

                    //if (group.TestType.ToLower() == "aft")
                    //{
                    //    ((IDictionary<string, object>)updateObject).Add("includeNull", group.IncludeNull);
                    //}

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>) updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("message", test.Message.ToLittleEndianHex());

                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            var resultOne = test.ResultsArray[1];
                            dynamic resultObject = new ExpandoObject();
                            ((IDictionary<string, object>)resultObject).Add("message", resultOne.Message);

                            resultsArray.Add(resultObject);
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else if (group.TestType.ToLower() == "aft")
                        {
                            ((IDictionary<string, object>)testObject).Add("inputLength", test.Message.BitLength);
                        }
                        else if (group.TestType.ToLower() == "vot")
                        {
                            ((IDictionary<string, object>) testObject).Add("outputLength", test.Digest.BitLength);
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
        public dynamic ResultProjection
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
                                ((IDictionary<string, object>) resultObject).Add("message", result.Message);
                                ((IDictionary<string, object>) resultObject).Add("digest", result.Digest);

                                resultsArray.Add(resultObject);
                            }

                            ((IDictionary<string, object>) testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("digest", test.Digest.ToLittleEndianHex());
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
