using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.DRBG
{
    public class TestVectorSet : ITestVectorSet
    {

        private readonly DynamicBitStringPrintWithOptions _bitStringPrinter =
            new DynamicBitStringPrintWithOptions(PrintOptionBitStringNull.DoNotPrintProperty,
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
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = BuildSharedTestCaseInformation(updateObject, group, test);
                        _bitStringPrinter.AddToDynamic(testObject, "returnedBits", test.ReturnedBits);
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
        public List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);

                    var tests = new List<dynamic>();
                    ((IDictionary<string, object>)updateObject).Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = BuildSharedTestCaseInformation(updateObject, group, test);
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
        public dynamic ResultProjection
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
                        _bitStringPrinter.AddToDynamic(testObject, "returnedBits", test.ReturnedBits);

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

        private dynamic BuildGroupInformation(TestGroup group)
        {
            dynamic updateObject = new ExpandoObject();
            ((IDictionary<string, object>)updateObject).Add("testType", group.TestType);
            ((IDictionary<string, object>)updateObject).Add("derFunc", group.DerFunc);
            ((IDictionary<string, object>)updateObject).Add("predResistance", group.PredResistance);
            ((IDictionary<string, object>)updateObject).Add("entropyInputLen", group.EntropyInputLen);
            ((IDictionary<string, object>)updateObject).Add("reSeed", group.ReSeed);
            ((IDictionary<string, object>)updateObject).Add("nonceLen", group.NonceLen);
            ((IDictionary<string, object>)updateObject).Add("persoStringLen", group.PersoStringLen);
            ((IDictionary<string, object>)updateObject).Add("additionalInputLen", group.AdditionalInputLen);
            ((IDictionary<string, object>)updateObject).Add("returnedBitsLen", group.ReturnedBitsLen);
            return updateObject;
        }

        private dynamic BuildSharedTestCaseInformation(dynamic updateObject, TestGroup group, TestCase test)
        {
            dynamic testObject = new ExpandoObject();
            ((IDictionary<string, object>)testObject).Add("tcId", test.TestCaseId);

            _bitStringPrinter.AddToDynamic(testObject, "entropyInput", test.EntropyInput);
            _bitStringPrinter.AddToDynamic(testObject, "nonce", test.Nonce);
            _bitStringPrinter.AddToDynamic(testObject, "persoString", test.PersoString);

            BuildOtherInputs(updateObject, group, test);
            return testObject;
        }

        private void BuildOtherInputs(dynamic updateObject, TestGroup group, TestCase test)
        {
            // Don't write the array at all if both additional input and entropy input will never have a value
            if (group.AdditionalInputLen == 0 && group.EntropyInputLen == 0)
            {
                return;
            }

            var otherInputs = new List<dynamic>();
            ((IDictionary<string, object>)updateObject).Add("otherInput", otherInputs);
            foreach (var otherInput in test.OtherInput)
            {
                dynamic otherInputObject = new ExpandoObject();
                _bitStringPrinter.AddToDynamic(otherInputObject, "additionalInput", otherInput.AdditionalInput);
                _bitStringPrinter.AddToDynamic(otherInputObject, "entropyInput", otherInput.EntropyInput);

                otherInputs.Add(otherInputObject);
            }
        }
    }
}
