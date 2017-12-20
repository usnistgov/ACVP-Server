using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.IKEv1.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.IKEv1
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
                    ((IDictionary<string, object>)updateObject).Add("authenticationMethod", EnumHelpers.GetEnumDescriptionFromEnum(group.AuthenticationMethod));
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("nInitLength", group.NInitLength);
                    ((IDictionary<string, object>)updateObject).Add("nRespLength", group.NRespLength);
                    ((IDictionary<string, object>)updateObject).Add("dhLength", group.GxyLength);

                    if (group.AuthenticationMethod == AuthenticationMethods.Psk)
                    {
                        ((IDictionary<string, object>)updateObject).Add("preSharedKeyLength", group.PreSharedKeyLength);
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("nInit", test.NInit);
                        ((IDictionary<string, object>)testObject).Add("nResp", test.NResp);
                        ((IDictionary<string, object>)testObject).Add("ckyInit", test.CkyInit);
                        ((IDictionary<string, object>)testObject).Add("ckyResp", test.CkyResp);
                        ((IDictionary<string, object>)testObject).Add("gxy", test.Gxy);

                        if (group.AuthenticationMethod == AuthenticationMethods.Psk)
                        {
                            ((IDictionary<string, object>)testObject).Add("preSharedKey", test.PreSharedKey);
                        }

                        ((IDictionary<string, object>)testObject).Add("sKeyId", test.SKeyId);
                        ((IDictionary<string, object>)testObject).Add("sKeyIdD", test.SKeyIdD);
                        ((IDictionary<string, object>)testObject).Add("sKeyIdA", test.SKeyIdA);
                        ((IDictionary<string, object>)testObject).Add("sKeyIdE", test.SKeyIdE);

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
                    ((IDictionary<string, object>)updateObject).Add("authenticationMethod", EnumHelpers.GetEnumDescriptionFromEnum(group.AuthenticationMethod));
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("nInitLength", group.NInitLength);
                    ((IDictionary<string, object>)updateObject).Add("nRespLength", group.NRespLength);
                    ((IDictionary<string, object>)updateObject).Add("dhLength", group.GxyLength);

                    if (group.AuthenticationMethod == AuthenticationMethods.Psk)
                    {
                        ((IDictionary<string, object>)updateObject).Add("preSharedKeyLength", group.PreSharedKeyLength);
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("nInit", test.NInit);
                        ((IDictionary<string, object>)testObject).Add("nResp", test.NResp);
                        ((IDictionary<string, object>)testObject).Add("ckyInit", test.CkyInit);
                        ((IDictionary<string, object>)testObject).Add("ckyResp", test.CkyResp);
                        ((IDictionary<string, object>)testObject).Add("gxy", test.Gxy);

                        if (group.AuthenticationMethod == AuthenticationMethods.Psk)
                        {
                            ((IDictionary<string, object>)testObject).Add("preSharedKey", test.PreSharedKey);
                        }

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
                        ((IDictionary<string, object>) testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("sKeyId", test.SKeyId);
                        ((IDictionary<string, object>)testObject).Add("sKeyIdD", test.SKeyIdD);
                        ((IDictionary<string, object>)testObject).Add("sKeyIdA", test.SKeyIdA);
                        ((IDictionary<string, object>)testObject).Add("sKeyIdE", test.SKeyIdE);

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
