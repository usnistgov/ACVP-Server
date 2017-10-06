using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.RSA_KeyGen
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();
                    ((IDictionary<string, object>)updateObject).Add("modulo", group.Modulo);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType.ToLower());
                    ((IDictionary<string, object>)updateObject).Add("randPQ", RSAEnumHelpers.KeyGenModeToString(group.Mode));
                    ((IDictionary<string, object>)updateObject).Add("pubExp", RSAEnumHelpers.PubExpModeToString(group.PubExp));

                    if (group.PubExp == PubExpModes.FIXED)
                    {
                        ((IDictionary<string, object>)updateObject).Add("fixedPubExp", group.FixedPubExp);
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        ((IDictionary<string, object>)updateObject).Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    }

                    if (group.Mode == KeyGenModes.B32 || group.Mode == KeyGenModes.B34 || group.Mode == KeyGenModes.B35)
                    {
                        ((IDictionary<string, object>)updateObject).Add("hashAlg", SHAEnumHelpers.HashFunctionToString(group.HashAlg));
                    }

                    if (group.Mode == KeyGenModes.B33 || group.Mode == KeyGenModes.B35 || group.Mode == KeyGenModes.B36)
                    {
                        ((IDictionary<string, object>)updateObject).Add("primeTest", RSAEnumHelpers.PrimeTestModeToString(group.PrimeTest));
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>) updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>) testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "kat")
                        {
                            ((IDictionary<string, object>) testObject).Add("result", !test.FailureTest ? "passed" : "failed");
                        }
                        else if(group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer)
                            {
                                AddKeyToDynamic(testObject, group.PubExp, test.Key);
                            }
                        }

                        if (test.Deferred)
                        {
                            ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
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
                    ((IDictionary<string, object>)updateObject).Add("modulo", group.Modulo);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType.ToLower());
                    ((IDictionary<string, object>)updateObject).Add("randPQ", RSAEnumHelpers.KeyGenModeToString(group.Mode));
                    ((IDictionary<string, object>)updateObject).Add("pubExp", RSAEnumHelpers.PubExpModeToString(group.PubExp));

                    if (group.PubExp == PubExpModes.FIXED)
                    {
                        ((IDictionary<string, object>) updateObject).Add("fixedPubExp", group.FixedPubExp);
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        ((IDictionary<string, object>)updateObject).Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    }

                    if (group.Mode == KeyGenModes.B32 || group.Mode == KeyGenModes.B34 || group.Mode == KeyGenModes.B35)
                    {
                        ((IDictionary<string, object>)updateObject).Add("hashAlg", SHAEnumHelpers.HashFunctionToString(group.HashAlg));
                    }

                    if (group.Mode == KeyGenModes.B33 || group.Mode == KeyGenModes.B35 || group.Mode == KeyGenModes.B36)
                    {
                        ((IDictionary<string, object>)updateObject).Add("primeTest", RSAEnumHelpers.PrimeTestModeToString(group.PrimeTest));
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();

                        // Always include tcId
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer)
                            {
                                if (group.PubExp == PubExpModes.RANDOM)
                                {
                                    ((IDictionary<string, object>)testObject).Add("e", test.Key.PubKey.E);
                                }

                                if (group.Mode == KeyGenModes.B34 || group.Mode == KeyGenModes.B35 || group.Mode == KeyGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("bitlens", test.Bitlens);
                                }

                                if (group.Mode == KeyGenModes.B32 || group.Mode == KeyGenModes.B34 || group.Mode == KeyGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("seed", test.Seed);
                                }

                                if (group.Mode == KeyGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp", test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xq", test.XQ);
                                }
                                else if (group.Mode == KeyGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp", test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xp1", test.XP1);
                                    ((IDictionary<string, object>)testObject).Add("xp2", test.XP2);
                                    ((IDictionary<string, object>)testObject).Add("xq", test.XQ);
                                    ((IDictionary<string, object>)testObject).Add("xq1", test.XQ1);
                                    ((IDictionary<string, object>)testObject).Add("xq2", test.XQ2);
                                }
                            }
                        }
                        else if (group.TestType.ToLower() == "kat")
                        {
                            ((IDictionary<string, object>)testObject).Add("e", test.Key.PubKey.E);
                            ((IDictionary<string, object>)testObject).Add("pRand", test.Key.PrivKey.P);
                            ((IDictionary<string, object>)testObject).Add("qRand", test.Key.PrivKey.Q);
                        }

                        if (test.Deferred)
                        {
                            ((IDictionary<string, object>) testObject).Add("deferred", test.Deferred);
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

                        // Always include tcId
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer || IsSample)
                            {
                                if (group.Mode == KeyGenModes.B34 || group.Mode == KeyGenModes.B35 || group.Mode == KeyGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("bitlens", test.Bitlens);
                                }

                                if (group.Mode == KeyGenModes.B32 || group.Mode == KeyGenModes.B34 || group.Mode == KeyGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("seed", test.Seed);
                                }

                                if (group.Mode == KeyGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp", test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xq", test.XQ);
                                }
                                else if (group.Mode == KeyGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp",  test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xp1", test.XP1);
                                    ((IDictionary<string, object>)testObject).Add("xp2", test.XP2);
                                    ((IDictionary<string, object>)testObject).Add("xq",  test.XQ);
                                    ((IDictionary<string, object>)testObject).Add("xq1", test.XQ1);
                                    ((IDictionary<string, object>)testObject).Add("xq2", test.XQ2);
                                }

                                AddKeyToDynamic(testObject, group.PubExp, test.Key);
                            }
                        }
                        else if (group.TestType.ToLower() == "kat")
                        {
                            ((IDictionary<string, object>)testObject).Add("result", !test.FailureTest ? "passed" : "failed");
                        }
                        else if (group.TestType.ToLower() == "gdt")
                        {
                            if (IsSample)
                            {
                                AddKeyToDynamic(testObject, group.PubExp, test.Key);
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

        private void AddKeyToDynamic(ExpandoObject jsonObject, PubExpModes expMode, KeyPair key)
        {
            if (expMode == PubExpModes.RANDOM)
            {
                ((IDictionary<string, object>)jsonObject).Add("e", key.PubKey.E);
            }

            ((IDictionary<string, object>)jsonObject).Add("n", key.PubKey.N);
            ((IDictionary<string, object>)jsonObject).Add("p", key.PrivKey.P);
            ((IDictionary<string, object>)jsonObject).Add("q", key.PrivKey.Q);

            if (key.PrivKey.D != 0)
            {
                ((IDictionary<string, object>)jsonObject).Add("d", key.PrivKey.D);
            }
            else
            {
                ((IDictionary<string, object>)jsonObject).Add("dmp1", key.PrivKey.DMP1);
                ((IDictionary<string, object>)jsonObject).Add("dmq1", key.PrivKey.DMQ1);
                ((IDictionary<string, object>)jsonObject).Add("iqmp", key.PrivKey.IQMP);
            }
        }
    }
}
