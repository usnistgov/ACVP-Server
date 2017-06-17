using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestVectorSet : ITestVectorSet
    {
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions = 
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty, 
                PrintOptionBitStringEmpty.PrintAsEmptyString
            );

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
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("ivLen", group.IVLength);
                    ((IDictionary<string, object>)updateObject).Add("ptLen", group.PTLength);
                    ((IDictionary<string, object>)updateObject).Add("aadLen", group.AADLength);
                    ((IDictionary<string, object>)updateObject).Add("tagLen", group.TagLength);
                    ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }
                        if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "plainText", test.PlainText);
                        }
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "aad", test.AAD);
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
                    ((IDictionary<string, object>)updateObject).Add("direction", group.Function);
                    ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
                    ((IDictionary<string, object>)updateObject).Add("ivLen", group.IVLength);
                    ((IDictionary<string, object>)updateObject).Add("ptLen", group.PTLength);
                    ((IDictionary<string, object>)updateObject).Add("aadLen", group.AADLength);
                    ((IDictionary<string, object>)updateObject).Add("tagLen", group.TagLength);
                    ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);
                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "plainText", test.PlainText);
                        }
                        if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
                        _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "aad", test.AAD);
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
                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "cipherText", test.CipherText);
                            _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "iv", test.IV);
                        }

                        if (test.FailureTest)
                        {
                            ((IDictionary<string, object>)testObject).Add("decryptFail", true);
                        }
                        else
                        {
                            if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                _dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "plainText", test.PlainText);
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
