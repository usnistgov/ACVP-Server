﻿using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "ECDSA";
        public string Mode { get; set; } = "SigVer";
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();

        //public TestVectorSet() { }

        //public TestVectorSet(dynamic answers)
        //{
        //    foreach (var answer in answers.answerProjection)
        //    {
        //        var group = new TestGroup(answer);
        //        TestGroups.Add(group);
        //    }
        //}

        //public List<dynamic> AnswerProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("curve", EnumHelpers.GetEnumDescriptionFromEnum(group.Curve));
        //            updateDict.Add("hashAlg", group.HashAlg.Name);
        //            updateDict.Add("qx", group.KeyPair.PublicQ.X);
        //            updateDict.Add("qy", group.KeyPair.PublicQ.Y);

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("result", test.FailureTest ? EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed) : EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed));
        //                testDict.Add("reason", EnumHelpers.GetEnumDescriptionFromEnum(test.Reason));
        //                testDict.Add("message", test.Message);
        //                testDict.Add("r", test.Signature.R);
        //                testDict.Add("s", test.Signature.S);

        //                tests.Add(testObject);
        //            }

        //            list.Add(updateObject);
        //        }

        //        return list;
        //    }
        //}

        //[JsonProperty(PropertyName = "testGroups")]
        //public List<dynamic> PromptProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("curve", EnumHelpers.GetEnumDescriptionFromEnum(group.Curve));
        //            updateDict.Add("hashAlg", group.HashAlg.Name);
        //            updateDict.Add("qx", group.KeyPair.PublicQ.X);
        //            updateDict.Add("qy", group.KeyPair.PublicQ.Y);

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("message", test.Message);
        //                testDict.Add("r", test.Signature.R);
        //                testDict.Add("s", test.Signature.S);

        //                tests.Add(testObject);
        //            }

        //            list.Add(updateObject);
        //        }

        //        return list;
        //    }
        //}

        //[JsonProperty(PropertyName = "testResults")]
        //public List<dynamic> ResultProjection
        //{
        //    get
        //    {
        //        var groups = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup)g))
        //        {
        //            dynamic groupObject = new ExpandoObject();
        //            var groupDict = (IDictionary<string, object>) groupObject;
        //            groupDict.Add("tgId", group.TestGroupId);

        //            var tests = new List<dynamic>();
        //            groupDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase)t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("result", test.FailureTest ? EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Failed) : EnumHelpers.GetEnumDescriptionFromEnum(Disposition.Passed));
        //                testDict.Add("reason", EnumHelpers.GetEnumDescriptionFromEnum(test.Reason));

        //                tests.Add(testObject);
        //            }

        //            groups.Add(groupObject);
        //        }

        //        return groups;
        //    }
        //}

        //public dynamic ToDynamic()
        //{
        //    dynamic vectorSetObject = new ExpandoObject();
        //    ((IDictionary<string, object>)vectorSetObject).Add("answerProjection", AnswerProjection);
        //    ((IDictionary<string, object>)vectorSetObject).Add("testGroups", PromptProjection);
        //    ((IDictionary<string, object>)vectorSetObject).Add("resultProjection", ResultProjection);
        //    return vectorSetObject;
        //}
    }
}