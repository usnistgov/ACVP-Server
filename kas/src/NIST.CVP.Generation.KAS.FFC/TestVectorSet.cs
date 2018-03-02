using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase, KasDsaAlgoAttributesFfc>
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
                        testObject.AddBigIntegerWhenNotZero("staticPrivateServer", test.StaticPrivateKeyServer);
                        testObject.AddBigIntegerWhenNotZero("staticPublicServer", test.StaticPublicKeyServer);
                        testObject.AddBigIntegerWhenNotZero("ephemeralPrivateServer", test.EphemeralPrivateKeyServer);
                        testObject.AddBigIntegerWhenNotZero("ephemeralPublicServer", test.EphemeralPublicKeyServer);
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceDkmServer", test.DkmNonceServer);
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralServer", test.EphemeralNonceServer);
                        
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                        testObject.AddBigIntegerWhenNotZero("staticPrivateIut", test.StaticPrivateKeyIut);
                        testObject.AddBigIntegerWhenNotZero("staticPublicIut", test.StaticPublicKeyIut);
                        testObject.AddBigIntegerWhenNotZero("ephemeralPrivateIut", test.EphemeralPrivateKeyIut);
                        testObject.AddBigIntegerWhenNotZero("ephemeralPublicIut", test.EphemeralPublicKeyIut);
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceDkmIut", test.DkmNonceIut);
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralIut", test.EphemeralNonceIut);

                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "z", test.Z);
                        if (group.MacType != KeyAgreementMacType.None)
                        {
                            testObject.AddIntegerWhenNotZero("oiLen", test.OiLen);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "otherInfo", test.OtherInfo);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "dkm", test.Dkm);

                            if (group.MacType == KeyAgreementMacType.AesCcm)
                            {
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceAesCcm",
                                    test.NonceAesCcm);
                            }

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "macData", test.MacData);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "tagIut", test.Tag);
                        }
                        else
                        {
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "hashZServer", test.HashZ);
                        }

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
                    ExpandoObject updateObject = new ExpandoObject();

                    SharedProjectionTestGroupInfo(group, updateObject);

                    var tests = new List<dynamic>();
                    updateObject.TryAdd("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase)t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);

                        if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                        {
                            testObject.AddBigIntegerWhenNotZero("staticPublicServer", test.StaticPublicKeyServer);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPublicServer", test.EphemeralPublicKeyServer);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceDkmServer", test.DkmNonceServer);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralServer", test.EphemeralNonceServer);
                            
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                            if (group.MacType == KeyAgreementMacType.AesCcm)
                            {
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceAesCcm", test.NonceAesCcm);
                            }
                        }

                        if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            testObject.AddBigIntegerWhenNotZero("staticPublicServer", test.StaticPublicKeyServer);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPublicServer", test.EphemeralPublicKeyServer);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceDkmServer", test.DkmNonceServer);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralServer", test.EphemeralNonceServer);
                            
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                            testObject.AddBigIntegerWhenNotZero("staticPrivateIut", test.StaticPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("staticPublicIut", test.StaticPublicKeyIut);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPrivateIut", test.EphemeralPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPublicIut", test.EphemeralPublicKeyIut);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceDkmIut", test.DkmNonceIut);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralIut", test.EphemeralNonceIut);
                            
                            if (group.MacType != KeyAgreementMacType.None)
                            {
                                testObject.AddIntegerWhenNotZero("oiLen", test.OiLen);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "otherInfo", test.OtherInfo);

                                if (group.MacType == KeyAgreementMacType.AesCcm)
                                {
                                    DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceAesCcm",
                                        test.NonceAesCcm);
                                }

                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "tagIut", test.Tag);
                            }
                            else
                            {
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "hashZIut", test.HashZ);
                            }
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
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
                    dynamic groupObject = new ExpandoObject();
                    var groupDict = (IDictionary<string, object>) groupObject;
                    groupDict.Add("tgId", group.TestGroupId);

                    var tests = new List<dynamic>();
                    groupDict.Add("tests", tests);
                    foreach (var test in group.Tests.Select(t => (TestCase) t))
                    {
                        ExpandoObject testObject = new ExpandoObject();

                        testObject.AddIntegerWhenNotZero("tcId", test.TestCaseId);

                        if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase) && !test.Deferred)
                        {
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                            testObject.AddBigIntegerWhenNotZero("staticPrivateIut", test.StaticPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("staticPublicIut", test.StaticPublicKeyIut);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPrivateIut", test.EphemeralPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPublicIut", test.EphemeralPublicKeyIut);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceDkmIut", test.DkmNonceIut);
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralIut", test.EphemeralNonceIut);
                            
                            if (group.MacType != KeyAgreementMacType.None)
                            {
                                testObject.AddIntegerWhenNotZero("idIutLen", test.IdIutLen);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "idIut", test.IdIut);

                                testObject.AddIntegerWhenNotZero("oiLen", test.OiLen);
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "oi", test.OtherInfo);

                                if (group.MacType == KeyAgreementMacType.AesCcm)
                                {
                                    DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceAesCcm",
                                        test.NonceAesCcm);
                                }

                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "tagIut", test.Tag);
                            }
                            else
                            {
                                DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "hashZIut", test.HashZ);
                            }
                        }

                        if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            ((IDictionary<string, object>) testObject).Add("result",
                                test.FailureTest ? "fail" : "pass");
                        }

                        tests.Add(testObject);
                    }

                    groups.Add(groupObject);
                }

                return groups;
            }
        }

        private void SharedProjectionTestGroupInfo(TestGroup @group, dynamic updateObject)
        {
            var updateDict = ((IDictionary<string, object>) updateObject);

            updateDict.Add("tgId", group.TestGroupId);
            updateDict.Add("scheme", EnumHelpers.GetEnumDescriptionFromEnum(group.Scheme));
            updateDict.Add("testType", group.TestType);
            updateDict.Add("kasRole", EnumHelpers.GetEnumDescriptionFromEnum(group.KasRole));
            updateDict.Add("kasMode", EnumHelpers.GetEnumDescriptionFromEnum(group.KasMode));
            updateDict.Add("parmSet", EnumHelpers.GetEnumDescriptionFromEnum(group.ParmSet));
            updateDict.Add("hashAlg", ShaAttributes.GetShaAttributes(group.HashAlg.Mode, group.HashAlg.DigestSize).name);

            if (group.MacType != KeyAgreementMacType.None)
            {
                updateDict.Add("macType", EnumHelpers.GetEnumDescriptionFromEnum(group.MacType));
                updateDict.Add("keyLen", group.KeyLen);

                if (group.MacType == KeyAgreementMacType.AesCcm)
                {
                    updateDict.Add("aesCcmNonceLen", group.AesCcmNonceLen);
                }

                updateDict.Add("macLen", group.MacLen);
                updateDict.Add("kdfType", group.KdfType);
                updateDict.Add("idServerLen", group.IdServerLen);
                DynamicBitStringPrintWithOptions.AddToDynamic(updateObject, "idServer", group.IdServer);

                if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                {
                    updateDict.Add("idIutLen", group.IdIutLen);
                    DynamicBitStringPrintWithOptions.AddToDynamic(updateObject, "idIut", group.IdIut);
                }
            }

            if (group.KasMode == KasMode.KdfKc)
            {
                updateDict.Add("kcRole", EnumHelpers.GetEnumDescriptionFromEnum(group.KcRole));
                updateDict.Add("kcType", EnumHelpers.GetEnumDescriptionFromEnum(group.KcType));
            }

            updateDict.Add("p", group.P);
            updateDict.Add("q", group.Q);
            updateDict.Add("g", group.G);

        }
    }
}