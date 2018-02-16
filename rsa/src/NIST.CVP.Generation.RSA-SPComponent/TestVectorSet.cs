using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SPComponent
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
            foreach(var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                TestGroups.Add(group);
            }

            foreach(var prompt in prompts.testGroups)
            {
                var promptGroup = new TestGroup(prompt);
                var matchingAnswerGroup = TestGroups.FirstOrDefault(g => g.Equals(promptGroup));
                if(matchingAnswerGroup != null)
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
                    ((IDictionary<string, object>)updateObject).Add("keyFormat", EnumHelpers.GetEnumDescriptionFromEnum(group.KeyFormat));

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>) testObject).Add("message", test.Message);
                        AddKeyToDynamic(testObject, test.Key);

                        if (test.FailureTest)
                        {
                            ((IDictionary<string, object>)testObject).Add("sigResult", !test.FailureTest);
                        }
                        else
                        {
                            ((IDictionary<string, object>) testObject).Add("signature", test.Signature);
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
                    ((IDictionary<string, object>)updateObject).Add("keyFormat", EnumHelpers.GetEnumDescriptionFromEnum(group.KeyFormat));

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>) testObject).Add("message", test.Message);
                        AddKeyToDynamic(testObject, test.Key);

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
                    foreach(var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (test.FailureTest)
                        {
                            ((IDictionary<string, object>)testObject).Add("sigResult", !test.FailureTest);
                        }
                        else
                        {
                            ((IDictionary<string, object>)testObject).Add("signature", test.Signature);
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

        private void AddKeyToDynamic(ExpandoObject jsonObject, KeyPair key)
        {
            ((IDictionary<string, object>)jsonObject).Add("n", key.PubKey.N);

            switch (key.PrivKey)
            {
                case PrivateKey standardKey:
                    ((IDictionary<string, object>)jsonObject).Add("d", standardKey.D);
                    break;

                case CrtPrivateKey crtKey:
                    ((IDictionary<string, object>)jsonObject).Add("dmp1", crtKey.DMP1);
                    ((IDictionary<string, object>)jsonObject).Add("dmq1", crtKey.DMQ1);
                    ((IDictionary<string, object>)jsonObject).Add("iqmp", crtKey.IQMP);
                    break;

                default:
                    throw new Exception("Invalid private key is not in a supported format");
            }
        }
    }
}
