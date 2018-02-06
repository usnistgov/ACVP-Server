using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

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
                    ((IDictionary<string, object>)updateObject).Add("randPQ", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeGenMode));
                    ((IDictionary<string, object>)updateObject).Add("pubExp", EnumHelpers.GetEnumDescriptionFromEnum(group.PubExp));
                    ((IDictionary<string, object>)updateObject).Add("keyFormat", EnumHelpers.GetEnumDescriptionFromEnum(group.KeyFormat));

                    if (group.PubExp == PublicExponentModes.Fixed)
                    {
                        ((IDictionary<string, object>)updateObject).Add("fixedPubExp", group.FixedPubExp);
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        ((IDictionary<string, object>)updateObject).Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                    {
                        ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B33 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                    {
                        ((IDictionary<string, object>)updateObject).Add("primeTest", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeTest));
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>) updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>) testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "kat")
                        {
                            ((IDictionary<string, object>) testObject).Add("result", !test.FailureTest ? EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed) : EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed));
                        }
                        else if(group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer)
                            {
                                AddKeyToDynamic(testObject, test.Key);
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
                    ((IDictionary<string, object>)updateObject).Add("randPQ", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeGenMode));
                    ((IDictionary<string, object>)updateObject).Add("pubExp", EnumHelpers.GetEnumDescriptionFromEnum(group.PubExp));
                    ((IDictionary<string, object>)updateObject).Add("keyFormat", EnumHelpers.GetEnumDescriptionFromEnum(group.KeyFormat));

                    if (group.PubExp == PublicExponentModes.Fixed)
                    {
                        ((IDictionary<string, object>) updateObject).Add("fixedPubExp", group.FixedPubExp);
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        ((IDictionary<string, object>)updateObject).Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                    {
                        ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B33 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                    {
                        ((IDictionary<string, object>)updateObject).Add("primeTest", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeTest));
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
                                if (group.PubExp == PublicExponentModes.Random)
                                {
                                    ((IDictionary<string, object>)testObject).Add("e", test.Key.PubKey.E);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("bitlens", test.Bitlens);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("seed", test.Seed);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp", test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xq", test.XQ);
                                }
                                else if (group.PrimeGenMode == PrimeGenModes.B36)
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
                                if (group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("bitlens", test.Bitlens);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("seed", test.Seed);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp", test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xq", test.XQ);
                                }
                                else if (group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    ((IDictionary<string, object>)testObject).Add("xp",  test.XP);
                                    ((IDictionary<string, object>)testObject).Add("xp1", test.XP1);
                                    ((IDictionary<string, object>)testObject).Add("xp2", test.XP2);
                                    ((IDictionary<string, object>)testObject).Add("xq",  test.XQ);
                                    ((IDictionary<string, object>)testObject).Add("xq1", test.XQ1);
                                    ((IDictionary<string, object>)testObject).Add("xq2", test.XQ2);
                                }

                                AddKeyToDynamic(testObject, test.Key);
                            }
                        }
                        else if (group.TestType.ToLower() == "kat")
                        {
                            ((IDictionary<string, object>)testObject).Add("result", !test.FailureTest ? EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed) : EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed));
                        }
                        else if (group.TestType.ToLower() == "gdt")
                        {
                            if (IsSample)
                            {
                                AddKeyToDynamic(testObject, test.Key);
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

        private void AddKeyToDynamic(ExpandoObject jsonObject, KeyPair key)
        {
            ((IDictionary<string, object>)jsonObject).Add("e", key.PubKey.E);
            ((IDictionary<string, object>)jsonObject).Add("n", key.PubKey.N);
            ((IDictionary<string, object>)jsonObject).Add("p", key.PrivKey.P);
            ((IDictionary<string, object>)jsonObject).Add("q", key.PrivKey.Q);

            if (key.PrivKey is PrivateKey standardKey)
            {
                ((IDictionary<string, object>)jsonObject).Add("d", standardKey.D);
            }
            else if (key.PrivKey is CrtPrivateKey crtKey)
            {
                ((IDictionary<string, object>)jsonObject).Add("dmp1", crtKey.DMP1);
                ((IDictionary<string, object>)jsonObject).Add("dmq1", crtKey.DMQ1);
                ((IDictionary<string, object>)jsonObject).Add("iqmp", crtKey.IQMP);
            }
            else
            {
                throw new Exception("Invalid private key is not in a supported format");
            }
        }
    }
}
