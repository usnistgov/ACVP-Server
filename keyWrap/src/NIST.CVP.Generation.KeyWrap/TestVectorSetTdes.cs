using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestVectorSetTdes : ITestVectorSet
    {

        private readonly DynamicBitStringPrintWithOptions _bitStringPrinter =
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsEmptyString
            );

        public TestVectorSetTdes()
        {
        }

        public TestVectorSetTdes(dynamic answers, dynamic prompts)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroupTdes(answer);

                TestGroups.Add(group);
            }

            foreach (var prompt in prompts.testGroups)
            {
                var promptGroup = new TestGroupTdes(prompt);
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

        public string Algorithm { get; set; }
        [JsonIgnore]
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        /// <summary>
        /// Expected answers (not sent to client)
        /// </summary>
        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroupTdes)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCaseTdes)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        _bitStringPrinter.AddToDynamic(testObject, "key", test.Key);

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }
                        if (group.Direction.ToLower() == "decrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
                        }

                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        /// <summary>
        /// What the client receives (should not include expected answers)
        /// </summary>
        [JsonProperty(PropertyName = "testGroups")]
        public List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroupTdes)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCaseTdes)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        _bitStringPrinter.AddToDynamic(testObject, "key", test.Key);

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
                        }
                        if (group.Direction.ToLower() == "decrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        /// <summary>
        /// Debug projection (internal), as well as potentially sample projection (sent to client)
        /// </summary>
        [JsonProperty(PropertyName = "testResults")]
        public List<dynamic> ResultProjection
        {
            get
            {
                var tests = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroupTdes)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCaseTdes)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }

                        if (test.FailureTest)
                        {
                            ((IDictionary<string, object>)testObject).Add("decryptFail", true);
                        }
                        else
                        {
                            if (group.Direction.ToLower() == "decrypt")
                            {
                                _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
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

        private dynamic BuildGroupInformation(TestGroupTdes group)
        {
            dynamic updateObject = new ExpandoObject();
            ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
            ((IDictionary<string, object>)updateObject).Add("direction", group.Direction);
            ((IDictionary<string, object>)updateObject).Add("kwCipher", group.KwCipher);
            ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
            ((IDictionary<string, object>)updateObject).Add("ptLen", group.PtLen);
            return updateObject;
        }
    }
}
