using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using System.Collections;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestVectorSet: ITestVectorSet
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
                    ((IDictionary<string, object>)updateObject).Add("numberOfKeys", group.NumberOfKeys);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                        }
                        if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                        }
                        ((IDictionary<string, object>)testObject).Add("key", test.Key);
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
                    ((IDictionary<string, object>)updateObject).Add("numberOfKeys", group.NumberOfKeys);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("plainText", test.PlainText);
                        }
                        if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("cipherText", test.CipherText);
                        }
                        if (test.Keys.KeyOption == KeyOptionValues.OneKey)
                        {
                            ((IDictionary<string, object>)testObject).Add("key", test.Key);
                        }
                        else
                        {
                            for (int iKeyIndex = 0; iKeyIndex < test.Keys.Keys.Count; iKeyIndex++)
                            {
                                ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex+1}", test.Keys.KeysAsBitStrings[iKeyIndex]);
                            }
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
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
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
