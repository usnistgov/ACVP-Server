using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestVectorSet : ITestVectorSet
    {
        protected readonly DynamicBitStringPrintWithOptions DynamicBitStringPrintWithOptions = new DynamicBitStringPrintWithOptions(PrintOptionBitStringNull.DoNotPrintProperty, PrintOptionBitStringEmpty.PrintAsEmptyString);

        public TestVectorSet()
        {
        }

        public TestVectorSet(dynamic answers)
        {
            SetAnswers(answers);
        }

        public string Algorithm { get; set; } = "KAS-ECC";
        public string Mode { get; set; } = "CDH Component";
        public bool IsSample { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "testGroupsNotSerialized")]
        public List<ITestGroup> TestGroups { get; set; } = new List<ITestGroup>();

        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();

                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    ExpandoObject updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    updateObject.TryAdd("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);
                        ((IDictionary<string, object>)testObject).Add("deferred", test.Deferred);
                        ((IDictionary<string, object>)testObject).Add("failureTest", test.FailureTest);
                        testObject.AddBigIntegerWhenNotZero("privateServer", test.PrivateKeyServer);
                        testObject.AddBigIntegerWhenNotZero("publicServerX", test.PublicKeyServerX);
                        testObject.AddBigIntegerWhenNotZero("publicServerY", test.PublicKeyServerY);

                        testObject.AddBigIntegerWhenNotZero("privateIut", test.PrivateKeyIut);
                        testObject.AddBigIntegerWhenNotZero("publicIutX", test.PublicKeyIutX);
                        testObject.AddBigIntegerWhenNotZero("publicIutY", test.PublicKeyIutY);

                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "z", test.Z);

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
                    ExpandoObject updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    updateObject.TryAdd("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);
                        testObject.AddBigIntegerWhenNotZero("publicServerX", test.PublicKeyServerX);
                        testObject.AddBigIntegerWhenNotZero("publicServerY", test.PublicKeyServerY);

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
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);

                        testObject.AddBigIntegerWhenNotZero("publicIutX", test.PublicKeyIutX);
                        testObject.AddBigIntegerWhenNotZero("publicIutY", test.PublicKeyIutY);
                        
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "z", test.Z);
                            
                        list.Add(testObject);
                    }
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

        protected void SetAnswers(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);

                TestGroups.Add(group);
            }
        }

        private void SharedProjectionTestGroupInfo(TestGroup @group, dynamic updateObject)
        {
            var updateDict = ((IDictionary<string, object>) updateObject);
            updateDict.Add("tgId", group.TestGroupId);
            updateDict.Add("testType", group.TestType);
            updateDict.Add("curveName", EnumHelpers.GetEnumDescriptionFromEnum(group.CurveName));
        }
    }
}