using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>) updateObject).Add("modulo", group.Modulo);
                    ((IDictionary<string, object>) updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>) updateObject).Add("hashAlg", SHAEnumHelpers.HashFunctionToString(group.HashAlg));
                    ((IDictionary<string, object>) updateObject).Add("pubExp", RSAEnumHelpers.PubExpModeToString(group.PubExp));
                    ((IDictionary<string, object>) updateObject).Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    ((IDictionary<string, object>) updateObject).Add("randPQ", RSAEnumHelpers.KeyGenModeToString(group.Mode));

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>) updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>) testObject).Add("tcId", test.TestCaseId);

                        ((IDictionary<string, object>) testObject).Add("p", new BitString(test.Key.PrivKey.P));
                        ((IDictionary<string, object>) testObject).Add("q", new BitString(test.Key.PrivKey.Q));
                        ((IDictionary<string, object>) testObject).Add("d", new BitString(test.Key.PrivKey.D));
                        ((IDictionary<string, object>)testObject).Add("n", new BitString(test.Key.PubKey.N));
                        ((IDictionary<string, object>)testObject).Add("e", new BitString(test.Key.PubKey.E));

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        public List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("modulo", group.Modulo);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", SHAEnumHelpers.HashFunctionToString(group.HashAlg));
                    ((IDictionary<string, object>)updateObject).Add("pubExp", RSAEnumHelpers.PubExpModeToString(group.PubExp));
                    ((IDictionary<string, object>)updateObject).Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    ((IDictionary<string, object>)updateObject).Add("randPQ", RSAEnumHelpers.KeyGenModeToString(group.Mode));

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        ((IDictionary<string, object>)testObject).Add("e", new BitString(test.Key.PubKey.E));
                        ((IDictionary<string, object>)testObject).Add("seed", test.Seed);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        public List<dynamic> ResultProjection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }
}
