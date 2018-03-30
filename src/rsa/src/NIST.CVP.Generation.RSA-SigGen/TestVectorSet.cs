using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;

namespace NIST.CVP.Generation.RSA_SigGen
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

        public TestVectorSet(dynamic answers)
        {
            foreach(var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);
                TestGroups.Add(group);
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
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("sigType", EnumHelpers.GetEnumDescriptionFromEnum(group.Mode));
                    updateDict.Add("modulo", group.Modulo);
                    updateDict.Add("hashAlg", group.HashAlg.Name);
                    updateDict.Add("testType", group.TestType);

                    if (group.Mode == SignatureSchemes.Pss)
                    {
                        updateDict.Add("saltLen", group.SaltLen);
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("message", test.Message);

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
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("sigType", EnumHelpers.GetEnumDescriptionFromEnum(group.Mode));
                    updateDict.Add("modulo", group.Modulo);
                    updateDict.Add("hashAlg", group.HashAlg.Name);
                    updateDict.Add("testType", group.TestType);

                    if (group.Mode == SignatureSchemes.Pss)
                    {
                        updateDict.Add("saltLen", group.SaltLen);
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("message", test.Message);

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
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (IsSample)
                        {
                            testDict.Add("n", group.Key.PubKey.N);
                            testDict.Add("e", group.Key.PubKey.E);
                            testDict.Add("signature", test.Signature);

                            if (group.Mode == SignatureSchemes.Pss)
                            {
                                testDict.Add("salt", test.Salt);
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
