using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.IKEv1
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public string Algorithm { get; set; } = "kdf-components";
        public string Mode { get; set; } = "ikev1";
        public bool IsSample { get; set; }

        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();

        
        //public List<dynamic> AnswerProjection
        //{
        //    get
        //    {
        //        var list = new List<dynamic>();
        //        foreach (var group in TestGroups.Select(g => (TestGroup) g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("authenticationMethod", EnumHelpers.GetEnumDescriptionFromEnum(group.AuthenticationMethod));
        //            updateDict.Add("hashAlg", group.HashAlg.Name);
        //            updateDict.Add("nInitLength", group.NInitLength);
        //            updateDict.Add("nRespLength", group.NRespLength);
        //            updateDict.Add("dhLength", group.GxyLength);

        //            if (group.AuthenticationMethod == AuthenticationMethods.Psk)
        //            {
        //                updateDict.Add("preSharedKeyLength", group.PreSharedKeyLength);
        //            }

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase) t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("nInit", test.NInit);
        //                testDict.Add("nResp", test.NResp);
        //                testDict.Add("ckyInit", test.CkyInit);
        //                testDict.Add("ckyResp", test.CkyResp);
        //                testDict.Add("gxy", test.Gxy);

        //                if (group.AuthenticationMethod == AuthenticationMethods.Psk)
        //                {
        //                    testDict.Add("preSharedKey", test.PreSharedKey);
        //                }

        //                testDict.Add("sKeyId", test.SKeyId);
        //                testDict.Add("sKeyIdD", test.SKeyIdD);
        //                testDict.Add("sKeyIdA", test.SKeyIdA);
        //                testDict.Add("sKeyIdE", test.SKeyIdE);

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
        //        foreach (var group in TestGroups.Select(g => (TestGroup) g))
        //        {
        //            dynamic updateObject = new ExpandoObject();
        //            var updateDict = ((IDictionary<string, object>) updateObject);
        //            updateDict.Add("tgId", group.TestGroupId);
        //            updateDict.Add("authenticationMethod", EnumHelpers.GetEnumDescriptionFromEnum(group.AuthenticationMethod));
        //            updateDict.Add("hashAlg", group.HashAlg.Name);
        //            updateDict.Add("nInitLength", group.NInitLength);
        //            updateDict.Add("nRespLength", group.NRespLength);
        //            updateDict.Add("dhLength", group.GxyLength);

        //            if (group.AuthenticationMethod == AuthenticationMethods.Psk)
        //            {
        //                updateDict.Add("preSharedKeyLength", group.PreSharedKeyLength);
        //            }

        //            var tests = new List<dynamic>();
        //            updateDict.Add("tests", tests);
        //            foreach (var test in group.Tests.Select(t => (TestCase) t))
        //            {
        //                dynamic testObject = new ExpandoObject();
        //                var testDict = ((IDictionary<string, object>) testObject);
        //                testDict.Add("tcId", test.TestCaseId);
        //                testDict.Add("nInit", test.NInit);
        //                testDict.Add("nResp", test.NResp);
        //                testDict.Add("ckyInit", test.CkyInit);
        //                testDict.Add("ckyResp", test.CkyResp);
        //                testDict.Add("gxy", test.Gxy);

        //                if (group.AuthenticationMethod == AuthenticationMethods.Psk)
        //                {
        //                    testDict.Add("preSharedKey", test.PreSharedKey);
        //                }

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
        //                testDict.Add("sKeyId", test.SKeyId);
        //                testDict.Add("sKeyIdD", test.SKeyIdD);
        //                testDict.Add("sKeyIdA", test.SKeyIdA);
        //                testDict.Add("sKeyIdE", test.SKeyIdE);

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
