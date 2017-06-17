using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestVectorSet : ITestVectorSet
    {
        public TestVectorSet()
        {
        }

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
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);

                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    //((IDictionary<string, object>)updateObject).Add("numberOfKeys", group.NumberOfKeys);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.Equals("MCT", StringComparison.OrdinalIgnoreCase))
                        {
                            var resultsArray = new List<dynamic>();

                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();

                                TDESKeys keys = new TDESKeys(result.Keys);
                                for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    ((IDictionary<string, object>)resultObject).Add($"key{iKeyIndex + 1}",
                                        keys.KeysAsBitStrings[iKeyIndex]);
                                }
                                ((IDictionary<string, object>)resultObject).Add("iv", result.IV);
                                if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((IDictionary<string, object>)resultObject).Add("plainText", result.PlainText);
                                    ((IDictionary<string, object>)resultObject).Add("cipherText", result.CipherText);
                                }
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((IDictionary<string, object>)resultObject).Add("cipherText", result.CipherText);
                                    ((IDictionary<string, object>)resultObject).Add("plainText", result.PlainText);
                                }

                                resultsArray.Add(resultObject);
                            }
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            TDESKeys keys = test.Keys;
                            for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                            {
                                ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex + 1}",
                                    keys.KeysAsBitStrings[iKeyIndex]);
                            }

                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                            }
                            else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                            }
                            ((IDictionary<string, object>)testObject).Add("iv", test.Iv);
                        }
                        //((IDictionary<string, object>)testObject).Add("key", test.Key);
                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
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
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    //((IDictionary<string, object>)updateObject).Add("numberOfKeys", group.NumberOfKeys);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        TDESKeys keys = test.Keys;
                        for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex + 1}",
                                keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                        }
                        else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                        }
                        ((IDictionary<string, object>)testObject).Add("iv", test.Iv);

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
                var tests = new List<dynamic>();
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

                                TDESKeys keys = new TDESKeys(result.Keys);
                                for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    ((IDictionary<string, object>)resultObject).Add($"key{iKeyIndex + 1}",
                                        keys.KeysAsBitStrings[iKeyIndex]);
                                }

                                if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((IDictionary<string, object>)resultObject).Add("plainText", result.PlainText);
                                    ((IDictionary<string, object>)resultObject).Add("cipherText", result.CipherText);
                                    ((IDictionary<string, object>)resultObject).Add("iv", result.IV);
                                }
                                else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((IDictionary<string, object>)resultObject).Add("cipherText", result.CipherText);
                                    ((IDictionary<string, object>)resultObject).Add("plainText", result.PlainText);
                                    ((IDictionary<string, object>)resultObject).Add("iv", result.IV);
                                }
                                resultsArray.Add(resultObject);
                            }
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {


                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                            }

                            if (test.FailureTest)
                            {
                                ((IDictionary<string, object>)testObject).Add("decryptFail", true);
                            }
                            else
                            {
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                                }
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
