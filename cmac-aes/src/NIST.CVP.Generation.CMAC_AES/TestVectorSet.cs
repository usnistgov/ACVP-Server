using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestVectorSet: ITestVectorSet
    {
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions = new DynamicBitStringPrintWithOptions(PrintOptionBitStringNull.DoNotPrintProperty, PrintOptionBitStringEmpty.PrintAsEmptyString);

        public TestVectorSet()
        {
        }

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

        public string Algorithm { get; set; }
        [JsonIgnore]
        public string Mode { get; set; } = string.Empty;
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        SharedProjectionTestCaseInfo(test, testObject);

                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "msg", test.Message);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "mac", test.Mac);
                        ((IDictionary<string, object>)testObject).Add("result", test.Result);

                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        SharedProjectionTestCaseInfo(test, testObject);

                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "msg", test.Message);

                        if (group.Function.Equals("ver", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "mac", test.Mac);
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.Function.Equals("gen", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "mac", test.Mac);
                        }
                        
                        if (group.Function.Equals("ver", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("result", test.Result);
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

        private void SharedProjectionTestGroupInfo(TestGroup group, dynamic updateObject)
        {
            ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
            ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
            ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
            ((IDictionary<string, object>)updateObject).Add("msgLen", group.MessageLength);
            ((IDictionary<string, object>)updateObject).Add("macLen", group.MacLength);
        }

        private void SharedProjectionTestCaseInfo(TestCase test, dynamic testObject)
        {
            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
        }
    }
}
