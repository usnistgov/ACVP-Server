using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestVectorSet : TestVectorSetBase<TestGroup, TestCase, KasDsaAlgoAttributesFfc>
    {

        public TestVectorSet()
        {
        }

        public TestVectorSet(dynamic answers, dynamic prompts)
        {
            SetAnswerAndPrompts(answers, prompts);
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
                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralServer", test.EphemeralNonceServer);

                        DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                        testObject.AddBigIntegerWhenNotZero("staticPrivateIut", test.StaticPrivateKeyIut);
                        testObject.AddBigIntegerWhenNotZero("staticPublicIut", test.StaticPublicKeyIut);
                        testObject.AddBigIntegerWhenNotZero("ephemeralPrivateIut", test.EphemeralPrivateKeyIut);
                        testObject.AddBigIntegerWhenNotZero("ephemeralPublicIut", test.EphemeralPublicKeyIut);
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
                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceEphemeralServer", test.EphemeralNonceServer);

                            DynamicBitStringPrintWithOptions.AddToDynamic(testObject, "nonceNoKc", test.NonceNoKc);

                            testObject.AddBigIntegerWhenNotZero("staticPrivateIut", test.StaticPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("staticPublicIut", test.StaticPublicKeyIut);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPrivateIut", test.EphemeralPrivateKeyIut);
                            testObject.AddBigIntegerWhenNotZero("ephemeralPublicIut", test.EphemeralPublicKeyIut);
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
                var list = new List<dynamic>();
                foreach (var group in TestGroups.Select(g => (TestGroup) g))
                {
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

                        list.Add(testObject);
                    }
                }
                return list;
            }
        }

        private void SharedProjectionTestGroupInfo(TestGroup @group, dynamic updateObject)
        {
            ((IDictionary<string, object>) updateObject).Add("scheme", EnumHelpers.GetEnumDescriptionFromEnum(group.Scheme));
            ((IDictionary<string, object>) updateObject).Add("testType", group.TestType);
            ((IDictionary<string, object>) updateObject).Add("kasRole", EnumHelpers.GetEnumDescriptionFromEnum(group.KasRole));
            ((IDictionary<string, object>) updateObject).Add("kasMode", EnumHelpers.GetEnumDescriptionFromEnum(group.KasMode));
            ((IDictionary<string, object>) updateObject).Add("parmSet", EnumHelpers.GetEnumDescriptionFromEnum(group.ParmSet));
            ((IDictionary<string, object>) updateObject).Add("hashAlg", ShaAttributes.GetShaAttributes(group.HashAlg.Mode, group.HashAlg.DigestSize).name);

            if (group.MacType != KeyAgreementMacType.None)
            {
                ((IDictionary<string, object>)updateObject).Add("macType", EnumHelpers.GetEnumDescriptionFromEnum(group.MacType));
                ((IDictionary<string, object>)updateObject).Add("keyLen", group.KeyLen);

                if (group.MacType == KeyAgreementMacType.AesCcm)
                {
                    ((IDictionary<string, object>)updateObject).Add("aesCcmNonceLen", group.AesCcmNonceLen);
                }

                ((IDictionary<string, object>)updateObject).Add("macLen", group.MacLen);
                ((IDictionary<string, object>)updateObject).Add("kdfType", group.KdfType);
                ((IDictionary<string, object>)updateObject).Add("idServerLen", group.IdServerLen);
                DynamicBitStringPrintWithOptions.AddToDynamic(updateObject, "idServer", group.IdServer);

                if (group.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                {
                    ((IDictionary<string, object>)updateObject).Add("idIutLen", group.IdIutLen);
                    DynamicBitStringPrintWithOptions.AddToDynamic(updateObject, "idIut", group.IdIut);
                }
            }

            if (group.KasMode == KasMode.KdfKc)
            {
                ((IDictionary<string, object>)updateObject).Add("kcRole", EnumHelpers.GetEnumDescriptionFromEnum(group.KcRole));
                ((IDictionary<string, object>)updateObject).Add("kcType", EnumHelpers.GetEnumDescriptionFromEnum(group.KcType));
            }

            ((IDictionary<string, object>)updateObject).Add("p", group.P);
            ((IDictionary<string, object>)updateObject).Add("q", group.Q);
            ((IDictionary<string, object>)updateObject).Add("g", group.G);

        }
    }
}