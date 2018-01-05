using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SRTP
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
                    ((IDictionary<string, object>)updateObject).Add("kdr", group.Kdr);
                    ((IDictionary<string, object>)updateObject).Add("aesKeyLength", group.AesKeyLength);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("masterKey", test.MasterKey);
                        ((IDictionary<string, object>)testObject).Add("masterSalt", test.MasterSalt);
                        ((IDictionary<string, object>)testObject).Add("index", test.Index);
                        ((IDictionary<string, object>)testObject).Add("srtcpIndex", test.SrtcpIndex);
                        
                        ((IDictionary<string, object>)testObject).Add("srtpKe", test.SrtpKe);
                        ((IDictionary<string, object>)testObject).Add("srtpKa", test.SrtpKa);
                        ((IDictionary<string, object>)testObject).Add("srtpKs", test.SrtpKs);
                        
                        ((IDictionary<string, object>)testObject).Add("srtcpKe", test.SrtcpKe);
                        ((IDictionary<string, object>)testObject).Add("srtcpKa", test.SrtcpKa);
                        ((IDictionary<string, object>)testObject).Add("srtcpKs", test.SrtcpKs);

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
                    ((IDictionary<string, object>)updateObject).Add("kdr", group.Kdr);
                    ((IDictionary<string, object>)updateObject).Add("aesKeyLength", group.AesKeyLength);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("masterKey", test.MasterKey);
                        ((IDictionary<string, object>)testObject).Add("masterSalt", test.MasterSalt);
                        ((IDictionary<string, object>)testObject).Add("index", test.Index);
                        ((IDictionary<string, object>)testObject).Add("srtcpIndex", test.SrtcpIndex);

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
                        ((IDictionary<string, object>)testObject).Add("srtpKe", test.SrtpKe);
                        ((IDictionary<string, object>)testObject).Add("srtpKa", test.SrtpKa);
                        ((IDictionary<string, object>)testObject).Add("srtpKs", test.SrtpKs);
                        
                        ((IDictionary<string, object>)testObject).Add("srtcpKe", test.SrtcpKe);
                        ((IDictionary<string, object>)testObject).Add("srtcpKa", test.SrtcpKa);
                        ((IDictionary<string, object>)testObject).Add("srtcpKs", test.SrtcpKs);

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
