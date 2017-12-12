using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using Newtonsoft.Json;
using System.Dynamic;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using System;
using System.Linq;
using NIST.CVP.Crypto.TDES;
using System.Collections;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestVectorSet : ITestVectorSet
    {
        private readonly DynamicBitStringPrintWithOptions _dynamicBitStringPrintWithOptions =
            new DynamicBitStringPrintWithOptions(
                PrintOptionBitStringNull.DoNotPrintProperty,
                PrintOptionBitStringEmpty.PrintAsEmptyString);


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
                var matchingAnswerGroup = TestGroups.Single(g => g.Equals(promptGroup));
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
                    ((IDictionary<string, object>)updateObject).Add("keyingOption", group.KeyingOption);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

                        if (group.TestType.Equals("MCT", StringComparison.OrdinalIgnoreCase))
                        {
                            var resultsArray = new List<dynamic>();

                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();


                                var keys = new TDESKeys(result.Keys);
                                for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    ((IDictionary<string, object>)resultObject).Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                                }
                                ((IDictionary<string, object>)resultObject).Add("iv", result.IV);
                                ((IDictionary<string, object>)resultObject).Add("pt", result.PlainText);
                                ((IDictionary<string, object>)resultObject).Add("ct", result.CipherText);
                                if (result.CipherTextLength != null)
                                {
                                    ((IDictionary<string, object>)resultObject).Add("ctLen", result.CipherTextLength.Value);
                                }

                                if (result.PlainTextLength != null)
                                {
                                    ((IDictionary<string, object>)resultObject).Add("ptLen", result.PlainTextLength.Value);
                                }

                                resultsArray.Add(resultObject);
                            }
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            var keys = new TDESKeys(test.Keys);
                            for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                            {
                                ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                            }

                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                ((IDictionary<string, object>)testObject).Add("ct", test.CipherText);
                                if (test.CipherTextLength.HasValue)
                                {
                                    ((IDictionary<string, object>)testObject).Add("ctLen", test.CipherTextLength.Value);
                                }
                            }
                            else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                ((IDictionary<string, object>)testObject).Add("pt", test.PlainText);
                                if (test.PlainTextLength.HasValue)
                                {
                                    ((IDictionary<string, object>)testObject).Add("ptLen", test.PlainTextLength.Value);
                                }
                            }
                            ((IDictionary<string, object>)testObject).Add("iv", test.Iv);
                        }
                        //testObject.Add("key", test.Key);
                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
                        tests.Add(testObject);
                    }
                    list.Add(updateObject);

                }

                return list;
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
                    ((IDictionary<string, object>)updateObject).Add("keyingOption", group.KeyingOption);
                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);



                        var keys = new TDESKeys(test.Keys);
                        for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            ((IDictionary<string, object>)testObject).Add($"key{iKeyIndex + 1}",
                                keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("pt", test.PlainText);
                            if (test.PlainTextLength.HasValue)
                            {
                                ((IDictionary<string, object>)testObject).Add("ptLen", test.PlainTextLength.Value);
                            }
                        }
                        else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>)testObject).Add("ct", test.CipherText);
                            if (test.CipherTextLength.HasValue)
                            {
                                ((IDictionary<string, object>)testObject).Add("ctLen",
                                    test.CipherTextLength.Value);
                            }
                        }

                        ((IDictionary<string, object>)testObject).Add("iv", test.Iv);


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
                        if (group.TestType.ToLower() == "mct")
                        {
                            var resultsArray = new List<dynamic>();
                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new Dictionary<string, object>();

                                var keys = new TDESKeys(result.Keys);
                                for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    resultObject.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                                }

                                resultObject.Add("pt", result.PlainText);
                                resultObject.Add("ct", result.CipherText);

                                if (result.CipherTextLength != null)
                                {
                                    resultObject.Add("ctLen", result.CipherTextLength.Value);
                                }

                                if (result.PlainTextLength != null)
                                {
                                    resultObject.Add("ptLen", result.PlainTextLength.Value);
                                }

                                resultObject.Add("iv", result.IV);
                                resultsArray.Add(resultObject);
                            }
                            ((IDictionary<string, object>)testObject).Add("resultsArray", resultsArray);
                        }
                        else
                        {


                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                ((IDictionary<string, object>)testObject).Add("ct", test.CipherText);
                                if (test.CipherTextLength.HasValue)
                                {
                                    ((IDictionary<string, object>)testObject).Add("ctLen", test.CipherTextLength.Value);
                                }
                            }

                            if (test.FailureTest)
                            {
                                ((IDictionary<string, object>)testObject).Add("decryptFail", true);
                            }
                            else
                            {
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    ((IDictionary<string, object>)testObject).Add("pt", test.PlainText);
                                    if (test.PlainTextLength.HasValue)
                                    {
                                        ((IDictionary<string, object>)testObject).Add("ptLen", test.PlainTextLength.Value);
                                    }
                                }
                            }
                        }

                        tests.Add(testObject);
                    }
                }
                return tests;
            }
        }

        private void SharedProjectionTestCaseInfo(TestCase test, dynamic testObject)
        {
            throw new NotImplementedException();
            //_dynamicBitStringPrintWithOptions.AddToDynamic(testObject, "key", test.Key);
        }

        private void SharedProjectionTestGroupInfo(TestGroup group, dynamic updateObject)
        {
            throw new NotImplementedException();
            //((IDictionary<string, object>)updateObject).Add("direction", group.Function);
            //((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
            //((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLength);
            //((IDictionary<string, object>)updateObject).Add("msgLen", group.MessageLength);
            //((IDictionary<string, object>)updateObject).Add("macLen", group.MacLength);
        }
    }
}