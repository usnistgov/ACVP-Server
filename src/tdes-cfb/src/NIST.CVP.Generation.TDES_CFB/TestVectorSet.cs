using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using Newtonsoft.Json;
using System.Dynamic;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using System;
using System.Linq;
using NIST.CVP.Crypto.TDES;

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

        public TestVectorSet(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);

                TestGroups.Add(group);
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
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("direction", group.Function);
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("keyingOption", group.KeyingOption);
                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        if (group.TestType.Equals("MCT", StringComparison.OrdinalIgnoreCase))
                        {
                            var resultsArray = new List<dynamic>();

                            foreach (var result in test.ResultsArray)
                            {
                                dynamic resultObject = new ExpandoObject();
                                var resultDict = ((IDictionary<string, object>) resultObject);
                                
                                var keys = new TDESKeys(result.Keys);
                                for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                                {
                                    resultDict.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                                }
                                resultDict.Add("iv", result.IV);
                                resultDict.Add("plainText", result.PlainText);
                                resultDict.Add("cipherText", result.CipherText);
                                if (result.CipherTextLength != null)
                                {
                                    resultDict.Add("ctLen", result.CipherTextLength.Value);
                                }

                                if (result.PlainTextLength != null)
                                {
                                    resultDict.Add("ptLen", result.PlainTextLength.Value);
                                }

                                resultsArray.Add(resultObject);
                            }
                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {
                            var keys = new TDESKeys(test.Keys);
                            for (int iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                            {
                                testDict.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                            }
                            testDict.Add("plainText", test.PlainText);
                            if (test.PlainTextLength.HasValue)
                            {
                                testDict.Add("ptLen", test.PlainTextLength.Value);
                            }

                            testDict.Add("cipherText", test.CipherText);
                            if (test.CipherTextLength.HasValue)
                            {
                                testDict.Add("ctLen", test.CipherTextLength.Value);
                            }
                            
                            testDict.Add("iv", test.Iv);
                        }
                        testDict.Add("deferred", test.Deferred);
                        testDict.Add("failureTest", test.FailureTest);
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
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("direction", group.Function);
                    updateDict.Add("testType", group.TestType);
                    updateDict.Add("keyingOption", group.KeyingOption);
                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);

                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);

                        var keys = new TDESKeys(test.Keys);
                        for (var iKeyIndex = 0; iKeyIndex < keys.KeysAsBitStrings.Count; iKeyIndex++)
                        {
                            testDict.Add($"key{iKeyIndex + 1}", keys.KeysAsBitStrings[iKeyIndex]);
                        }

                        if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            testDict.Add("plainText", test.PlainText);
                            if (test.PlainTextLength.HasValue)
                            {
                                testDict.Add("ptLen", test.PlainTextLength.Value);
                            }
                        }
                        else if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                        {
                            testDict.Add("cipherText", test.CipherText);
                            if (test.CipherTextLength.HasValue)
                            {
                                testDict.Add("ctLen", test.CipherTextLength.Value);
                            }
                        }

                        testDict.Add("iv", test.Iv);

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
                        var testDict = ((IDictionary<string, object>) testObject);

                        testDict.Add("tcId", test.TestCaseId);
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

                                resultObject.Add("plainText", result.PlainText);
                                resultObject.Add("cipherText", result.CipherText);

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
                            testDict.Add("resultsArray", resultsArray);
                        }
                        else
                        {


                            if (group.Function.Equals("encrypt", StringComparison.OrdinalIgnoreCase))
                            {
                                testDict.Add("cipherText", test.CipherText);
                                if (test.CipherTextLength.HasValue)
                                {
                                    testDict.Add("ctLen", test.CipherTextLength.Value);
                                }
                            }

                            if (test.FailureTest)
                            {
                                testDict.Add("decryptFail", true);
                            }
                            else
                            {
                                if (group.Function.Equals("decrypt", StringComparison.OrdinalIgnoreCase))
                                {
                                    testDict.Add("plainText", test.PlainText);
                                    if (test.PlainTextLength.HasValue)
                                    {
                                        testDict.Add("ptLen", test.PlainTextLength.Value);
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