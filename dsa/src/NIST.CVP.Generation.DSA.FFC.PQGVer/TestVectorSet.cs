using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
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
                    ((IDictionary<string, object>)updateObject).Add("l", group.L);
                    ((IDictionary<string, object>)updateObject).Add("n", group.N);
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);

                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        ((IDictionary<string, object>)updateObject).Add("pqMode", EnumHelpers.GetEnumDescriptionFromEnum(group.PQGenMode));
                    }
                    else if(group.GGenMode != GeneratorGenMode.None)
                    {
                        ((IDictionary<string, object>)updateObject).Add("gMode", EnumHelpers.GetEnumDescriptionFromEnum(group.GGenMode));
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("result", (test.FailureTest ? "failed" : "passed"));
                        ((IDictionary<string, object>)testObject).Add("reason", test.Reason);

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
                    ((IDictionary<string, object>)updateObject).Add("l", group.L);
                    ((IDictionary<string, object>)updateObject).Add("n", group.N);
                    ((IDictionary<string, object>)updateObject).Add("hashAlg", group.HashAlg.Name);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);

                    if (group.PQGenMode != PrimeGenMode.None)
                    {
                        ((IDictionary<string, object>)updateObject).Add("pqMode", EnumHelpers.GetEnumDescriptionFromEnum(group.PQGenMode));
                    }
                    else if (group.GGenMode != GeneratorGenMode.None)
                    {
                        ((IDictionary<string, object>)updateObject).Add("gMode", EnumHelpers.GetEnumDescriptionFromEnum(group.GGenMode));
                    }

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("p", test.P);
                        ((IDictionary<string, object>)testObject).Add("q", test.Q);

                        if (group.PQGenMode != PrimeGenMode.None)
                        {
                            // Counter
                            if (group.PQGenMode == PrimeGenMode.Probable)
                            {
                                ((IDictionary<string, object>)testObject).Add("domainSeed", test.Seed.Seed);
                                ((IDictionary<string, object>)testObject).Add("counter", test.Counter.Count);
                            }
                            else if (group.PQGenMode == PrimeGenMode.Provable)
                            {
                                ((IDictionary<string, object>)testObject).Add("pCounter", test.Counter.PCount);
                                ((IDictionary<string, object>)testObject).Add("qCounter", test.Counter.QCount);
                                ((IDictionary<string, object>)testObject).Add("pSeed", test.Seed.PSeed);
                                ((IDictionary<string, object>)testObject).Add("qSeed", test.Seed.QSeed);
                            }
                        }
                        else if (group.GGenMode != GeneratorGenMode.None)
                        {
                            ((IDictionary<string, object>)testObject).Add("g", test.G);

                            if (group.GGenMode == GeneratorGenMode.Canonical)
                            {
                                // Index
                                ((IDictionary<string, object>)testObject).Add("index", test.Index);
                            }
                            else if (group.GGenMode == GeneratorGenMode.Unverifiable)
                            {
                                // H and Seed
                                ((IDictionary<string, object>)testObject).Add("h", test.H);
                                ((IDictionary<string, object>)testObject).Add("domainSeed", test.Seed.GetFullSeed());
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
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("result", (test.FailureTest ? "failed" : "passed"));
                        ((IDictionary<string, object>)testObject).Add("reason", test.Reason);

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
