using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
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
                    updateDict.Add("l", group.L);
                    updateDict.Add("n", group.N);
                    updateDict.Add("hashAlg", group.HashAlg.Name);
                    updateDict.Add("testType", group.TestType);

                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        updateDict.Add("pqMode", EnumHelpers.GetEnumDescriptionFromEnum(group.PQGenMode));
                    }
                    else if(group.GGenMode != GeneratorGenMode.None)
                    {
                        updateDict.Add("gMode", EnumHelpers.GetEnumDescriptionFromEnum(group.GGenMode));
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.PQGenMode != PrimeGenMode.None)
                        {
                            testDict.Add("p", test.P);
                            testDict.Add("q", test.Q);
                            testDict.Add("domainSeed", test.Seed.Seed);

                            if (group.PQGenMode == PrimeGenMode.Probable)
                            {
                                testDict.Add("counter", test.Counter.Count);
                            }
                            else if (group.PQGenMode == PrimeGenMode.Provable)
                            {
                                testDict.Add("pSeed", test.Seed.PSeed);
                                testDict.Add("qSeed", test.Seed.QSeed);
                                testDict.Add("pCounter", test.Counter.PCount);
                                testDict.Add("qCounter", test.Counter.QCount);
                            }
                        }
                        else if (group.GGenMode != GeneratorGenMode.None)
                        {
                            testDict.Add("g", test.G);
                            testDict.Add("p", test.P);
                            testDict.Add("q", test.Q);

                            if (group.GGenMode == GeneratorGenMode.Canonical)
                            {
                                testDict.Add("domainSeed", test.Seed.GetFullSeed());
                                testDict.Add("index", test.Index);
                            }
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
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("l", group.L);
                    updateDict.Add("n", group.N);
                    updateDict.Add("hashAlg", group.HashAlg.Name);
                    updateDict.Add("testType", group.TestType);

                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        updateDict.Add("pqMode", EnumHelpers.GetEnumDescriptionFromEnum(group.PQGenMode));
                    }
                    else if (group.GGenMode != GeneratorGenMode.None)
                    {
                        updateDict.Add("gMode", EnumHelpers.GetEnumDescriptionFromEnum(group.GGenMode));
                    }

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.PQGenMode != PrimeGenMode.None)
                        {
                            // Nothing
                        }
                        else if (group.GGenMode != GeneratorGenMode.None)
                        {
                            testDict.Add("p", test.P);
                            testDict.Add("q", test.Q);

                            if (group.GGenMode == GeneratorGenMode.Canonical)
                            {
                                testDict.Add("domainSeed", test.Seed.GetFullSeed());
                                testDict.Add("index", test.Index);
                            }
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
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.PQGenMode != PrimeGenMode.None)
                        {
                            testDict.Add("p", test.P);
                            testDict.Add("q", test.Q);

                            if (group.PQGenMode == PrimeGenMode.Provable)
                            {
                                testDict.Add("pSeed", test.Seed.PSeed);
                                testDict.Add("qSeed", test.Seed.QSeed);
                                testDict.Add("pCounter", test.Counter.PCount);
                                testDict.Add("qCounter", test.Counter.QCount);
                            }
                            else if (group.PQGenMode == PrimeGenMode.Probable)
                            {
                                testDict.Add("counter", test.Counter.Count);
                            }
                        }
                        else if (group.GGenMode != GeneratorGenMode.None)
                        {
                            testDict.Add("g", test.G);
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
    }
}
