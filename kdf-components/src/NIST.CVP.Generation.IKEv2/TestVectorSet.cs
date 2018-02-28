using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.IKEv2
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

        public TestVectorSet(dynamic answers)
        {
            foreach (var answer in answers.answerProjection)
            {
                var group = new TestGroup(answer);

                TestGroups.Add(group);
            }
        }

        public List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("hashAlg", group.HashAlg.Name);
                    updateDict.Add("nInitLength", group.NInitLength);
                    updateDict.Add("nRespLength", group.NRespLength);
                    updateDict.Add("dhLength", group.GirLength);
                    updateDict.Add("derivedKeyingMaterialLength", group.DerivedKeyingMaterialLength);

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("nInit", test.NInit);
                        testDict.Add("nResp", test.NResp);
                        testDict.Add("spiInit", test.SpiInit);
                        testDict.Add("spiResp", test.SpiResp);
                        testDict.Add("gir", test.Gir);
                        testDict.Add("girNew", test.GirNew);

                        testDict.Add("sKeySeed", test.SKeySeed);
                        testDict.Add("derivedKeyingMaterial", test.DerivedKeyingMaterial);
                        testDict.Add("derivedKeyingMaterialChild", test.DerivedKeyingMaterialChild);
                        testDict.Add("derivedKeyingMaterialDh", test.DerivedKeyingMaterialDh);
                        testDict.Add("sKeySeedReKey", test.SKeySeedReKey);

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
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic updateObject = new ExpandoObject();
                    var updateDict = ((IDictionary<string, object>) updateObject);
                    updateDict.Add("tgId", group.TestGroupId);
                    updateDict.Add("hashAlg", group.HashAlg.Name);
                    updateDict.Add("nInitLength", group.NInitLength);
                    updateDict.Add("nRespLength", group.NRespLength);
                    updateDict.Add("dhLength", group.GirLength);
                    updateDict.Add("derivedKeyingMaterialLength", group.DerivedKeyingMaterialLength);

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("nInit", test.NInit);
                        testDict.Add("nResp", test.NResp);
                        testDict.Add("spiInit", test.SpiInit);
                        testDict.Add("spiResp", test.SpiResp);
                        testDict.Add("gir", test.Gir);
                        testDict.Add("girNew", test.GirNew);

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
                var groups = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic groupObject = new ExpandoObject();
                    var groupDict = (IDictionary<string, object>) groupObject;
                    groupDict.Add("tgId", group.TestGroupId);

                    var tests = new List<dynamic>();
                    groupDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        testDict.Add("sKeySeed", test.SKeySeed);
                        testDict.Add("derivedKeyingMaterial", test.DerivedKeyingMaterial);
                        testDict.Add("derivedKeyingMaterialChild", test.DerivedKeyingMaterialChild);
                        testDict.Add("derivedKeyingMaterialDh", test.DerivedKeyingMaterialDh);
                        testDict.Add("sKeySeedReKey", test.SKeySeedReKey);

                        tests.Add(testObject);
                    }

                    groups.Add(groupObject);
                }

                return groups;
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
