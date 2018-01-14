using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TLS
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

        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("tlsVersion", EnumHelpers.GetEnumDescriptionFromEnum(group.TlsMode));
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("preMasterSecretLength", group.PreMasterSecretLength);
                    ((IDictionary<string, object>)updateObject).Add("keyBlockLength", group.KeyBlockLength);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("clientHelloRandom", test.ClientHelloRandom);
                        ((IDictionary<string, object>)testObject).Add("serverHelloRandom", test.ServerHelloRandom);
                        ((IDictionary<string, object>)testObject).Add("clientRandom", test.ClientRandom);
                        ((IDictionary<string, object>)testObject).Add("serverRandom", test.ServerRandom);
                        ((IDictionary<string, object>)testObject).Add("preMasterSecret", test.PreMasterSecret);
                        
                        ((IDictionary<string, object>)testObject).Add("masterSecret", test.MasterSecret);
                        ((IDictionary<string, object>)testObject).Add("keyBlock", test.KeyBlock);

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
                    ((IDictionary<string, object>)updateObject).Add("tlsVersion", EnumHelpers.GetEnumDescriptionFromEnum(group.TlsMode));
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("preMasterSecretLength", group.PreMasterSecretLength);
                    ((IDictionary<string, object>)updateObject).Add("keyBlockLength", group.KeyBlockLength);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("clientHelloRandom", test.ClientHelloRandom);
                        ((IDictionary<string, object>)testObject).Add("serverHelloRandom", test.ServerHelloRandom);
                        ((IDictionary<string, object>)testObject).Add("clientRandom", test.ClientRandom);
                        ((IDictionary<string, object>)testObject).Add("serverRandom", test.ServerRandom);
                        ((IDictionary<string, object>)testObject).Add("preMasterSecret", test.PreMasterSecret);

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
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("masterSecret", test.MasterSecret);
                        ((IDictionary<string, object>)testObject).Add("keyBlock", test.KeyBlock);

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
