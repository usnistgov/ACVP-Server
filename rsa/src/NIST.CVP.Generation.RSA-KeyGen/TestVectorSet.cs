using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
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

        public TestVectorSet(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
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
                    updateDict.Add("modulo", group.Modulo);
                    updateDict.Add("testType", group.TestType.ToLower());
                    updateDict.Add("randPQ", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeGenMode));
                    updateDict.Add("pubExpMode", EnumHelpers.GetEnumDescriptionFromEnum(group.PubExp));
                    updateDict.Add("keyFormat", EnumHelpers.GetEnumDescriptionFromEnum(group.KeyFormat));

                    if (group.PubExp == PublicExponentModes.Fixed)
                    {
                        updateDict.Add("fixedPubExp", group.FixedPubExp);
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        updateDict.Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                    {
                        updateDict.Add("hashAlg", group.HashAlg.Name);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B33 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                    {
                        updateDict.Add("primeTest", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeTest));
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer)
                            {
                                AddKeyToDynamic(testObject, test.Key, group.PubExp == PublicExponentModes.Fixed);

                                if (group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    testDict.Add("bitlens", test.Bitlens);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    testDict.Add("seed", test.Seed);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    testDict.Add("xp", test.XP);
                                    testDict.Add("xq", test.XQ);
                                }
                                else if (group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    testDict.Add("xp", test.XP);
                                    testDict.Add("xp1", test.XP1);
                                    testDict.Add("xp2", test.XP2);
                                    testDict.Add("xq", test.XQ);
                                    testDict.Add("xq1", test.XQ1);
                                    testDict.Add("xq2", test.XQ2);
                                }
                            }
                        }
                        else if (group.TestType.ToLower() == "kat")
                        {
                            // "e" can stay here because this is only defined when PubExpMode == Random
                            testDict.Add("e", test.Key.PubKey.E);
                            testDict.Add("pRand", test.Key.PrivKey.P);
                            testDict.Add("qRand", test.Key.PrivKey.Q);
                            testDict.Add("result", !test.FailureTest ? EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed) : EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed));
                        }

                        if (test.Deferred)
                        {
                            testDict.Add("deferred", test.Deferred);
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
                    var updateDict = ((IDictionary<string, object>)updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("modulo", group.Modulo);
                    updateDict.Add("testType", group.TestType.ToLower());
                    updateDict.Add("randPQ", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeGenMode));
                    updateDict.Add("pubExpMode", EnumHelpers.GetEnumDescriptionFromEnum(group.PubExp));
                    updateDict.Add("keyFormat", EnumHelpers.GetEnumDescriptionFromEnum(group.KeyFormat));

                    if (group.PubExp == PublicExponentModes.Fixed)
                    {
                        updateDict.Add("fixedPubExp", group.FixedPubExp);
                    }

                    if (group.TestType.ToLower() == "aft")
                    {
                        updateDict.Add("infoGeneratedByServer", group.InfoGeneratedByServer);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                    {
                        updateDict.Add("hashAlg", group.HashAlg.Name);
                    }

                    if (group.PrimeGenMode == PrimeGenModes.B33 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                    {
                        updateDict.Add("primeTest", EnumHelpers.GetEnumDescriptionFromEnum(group.PrimeTest));
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);

                        // Always include tcId
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer)
                            {
                                if (group.PubExp == PublicExponentModes.Random)
                                {
                                    testDict.Add("e", test.Key.PubKey.E);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    testDict.Add("bitlens", test.Bitlens);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    testDict.Add("seed", test.Seed);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    testDict.Add("xp", test.XP);
                                    testDict.Add("xq", test.XQ);
                                }
                                else if (group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    testDict.Add("xp", test.XP);
                                    testDict.Add("xp1", test.XP1);
                                    testDict.Add("xp2", test.XP2);
                                    testDict.Add("xq", test.XQ);
                                    testDict.Add("xq1", test.XQ1);
                                    testDict.Add("xq2", test.XQ2);
                                }
                            }
                        }
                        else if (group.TestType.ToLower() == "kat")
                        {
                            // "e" can stay here because this is only defined when PubExpMode == Random
                            testDict.Add("e", test.Key.PubKey.E);
                            testDict.Add("pRand", test.Key.PrivKey.P);
                            testDict.Add("qRand", test.Key.PrivKey.Q);
                        }

                        if (test.Deferred)
                        {
                            testDict.Add("deferred", test.Deferred);
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
                var groups = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic groupObject = new ExpandoObject();
                    var groupDict = (IDictionary<string, object>) groupObject;
                    groupDict.Add("tgId", group.TestGroupId);

                    var tests = new List<dynamic>();
                    groupDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);

                        // Always include tcId
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.ToLower() == "aft")
                        {
                            if (group.InfoGeneratedByServer || IsSample)
                            {
                                if (group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35 || group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    testDict.Add("bitlens", test.Bitlens);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B32 || group.PrimeGenMode == PrimeGenModes.B34 || group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    testDict.Add("seed", test.Seed);
                                }

                                if (group.PrimeGenMode == PrimeGenModes.B35)
                                {
                                    testDict.Add("xp", test.XP);
                                    testDict.Add("xq", test.XQ);
                                }
                                else if (group.PrimeGenMode == PrimeGenModes.B36)
                                {
                                    testDict.Add("xp", test.XP);
                                    testDict.Add("xp1", test.XP1);
                                    testDict.Add("xp2", test.XP2);
                                    testDict.Add("xq", test.XQ);
                                    testDict.Add("xq1", test.XQ1);
                                    testDict.Add("xq2", test.XQ2);
                                }

                                AddKeyToDynamic(testObject, test.Key, group.PubExp == PublicExponentModes.Fixed);
                            }
                        }
                        else if (group.TestType.ToLower() == "kat")
                        {
                            testDict.Add("result", !test.FailureTest ? EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed) : EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed));
                        }
                        else if (group.TestType.ToLower() == "gdt")
                        {
                            if (IsSample)
                            {
                                AddKeyToDynamic(testObject, test.Key, group.PubExp == PublicExponentModes.Fixed);
                            }
                        }

                        tests.Add(testObject);
                    }

                    groups.Add(groupObject);
                }

                return groups;
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

        private void AddKeyToDynamic(ExpandoObject jsonObject, KeyPair key, bool fixedE)
        {
            var jsonDict = ((IDictionary<string, object>) jsonObject);

            if (!fixedE)
            {
                // Only add the public exponent to the test case if it isn't fixed (otherwise it's already in the group)
                jsonDict.Add("e", key.PubKey.E);
            }

            jsonDict.Add("n", key.PubKey.N);
            jsonDict.Add("p", key.PrivKey.P);
            jsonDict.Add("q", key.PrivKey.Q);

            if (key.PrivKey is PrivateKey standardKey)
            {
                jsonDict.Add("d", standardKey.D);
            }
            else if (key.PrivKey is CrtPrivateKey crtKey)
            {
                jsonDict.Add("dmp1", crtKey.DMP1);
                jsonDict.Add("dmq1", crtKey.DMQ1);
                jsonDict.Add("iqmp", crtKey.IQMP);
            }
            else
            {
                throw new Exception("Invalid private key is not in a supported format");
            }
        }
    }
}
