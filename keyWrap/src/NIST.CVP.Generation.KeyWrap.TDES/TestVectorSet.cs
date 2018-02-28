using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestVectorSet : TestVectorSetBase<TestGroup>
    {
        public TestVectorSet() { }

        public TestVectorSet(dynamic answers)
        {
            SetAnswers(answers);
        }

        public override List<dynamic> AnswerProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);
                    var updateDict = ((IDictionary<string, object>) updateObject);

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        _bitStringPrinter.AddToDynamic(testObject, "key1", test.Key1);
                        _bitStringPrinter.AddToDynamic(testObject, "key2", test.Key2);
                        _bitStringPrinter.AddToDynamic(testObject, "key3", test.Key3);
                        _bitStringPrinter.AddToDynamic(testObject, "plainText", test.PlainText);
                        _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        testDict.Add("failureTest", test.FailureTest);

                        tests.Add(testObject);
                    }

                    list.Add(updateObject);
                }

                return list;
            }
        }

        public override List<dynamic> PromptProjection
        {
            get
            {
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup)g))
                {
                    dynamic updateObject = BuildGroupInformation(group);
                    var updateDict = ((IDictionary<string, object>) updateObject);

                    var tests = new List<dynamic>();
                    updateDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        dynamic testObject = new ExpandoObject();
                        var testDict = ((IDictionary<string, object>) testObject);
                        testDict.Add("tcId", test.TestCaseId);
                        _bitStringPrinter.AddToDynamic(testObject, "key1", test.Key1);
                        _bitStringPrinter.AddToDynamic(testObject, "key2", test.Key2);
                        _bitStringPrinter.AddToDynamic(testObject, "key3", test.Key3);

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

        public override List<dynamic> ResultProjection
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

                        if (group.Direction.ToLower() == "encrypt")
                        {
                            _bitStringPrinter.AddToDynamic(testObject, "cipherText", test.CipherText);
                        }

                        if (test.FailureTest)
                        {
                            testDict.Add("decryptFail", true);
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

                    groups.Add(groupObject);
                }

                return groups;
            }
        }
        protected override dynamic BuildGroupInformation(TestGroup group)
        {
            dynamic updateObject = new ExpandoObject();
            var updateDict = ((IDictionary<string, object>) updateObject);
            updateDict.Add("tgId", group.TestGroupId);
            updateDict.Add("testType", group.TestType);
            updateDict.Add("direction", group.Direction);
            updateDict.Add("kwCipher", group.KwCipher);
            updateDict.Add("keyingOption", group.KeyingOption);
            updateDict.Add("ptLen", group.PtLen);
            return updateObject;
        }
    }
}
