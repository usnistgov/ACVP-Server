using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests),
                nameof(TestGroup.TestType),
                nameof(TestGroup.Scheme),
                nameof(TestGroup.KasRole),
                nameof(TestGroup.KasMode),
                nameof(TestGroup.ParmSet),
                nameof(TestGroup.HashAlgName),
                nameof(TestGroup.Curve),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties

            var macIncludeProperties = new[]
            {
                nameof(TestGroup.MacType),
                nameof(TestGroup.KeyLen),
                nameof(TestGroup.MacLen),
                nameof(TestGroup.KdfType),
                nameof(TestGroup.IdServerLen),
                nameof(TestGroup.IdServer)
            };
            if (macIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.MacType != KeyAgreementMacType.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var macValIncludeProperties = new[]
            {
                nameof(TestGroup.IdIutLen),
                nameof(TestGroup.IdIut),
            };
            if (macValIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.MacType != KeyAgreementMacType.None
                            && testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var macAesCcmIncludeProperties = new[]
            {
                nameof(TestGroup.AesCcmNonceLen)
            };
            if (macAesCcmIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.MacType == KeyAgreementMacType.AesCcm)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var kdfKcIncludeProperties = new[]
            {
                nameof(TestGroup.KcRole),
                nameof(TestGroup.KcType),
            };
            if (kdfKcIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.KasMode == KasMode.KdfKc)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.StaticPublicKeyServerX),
                nameof(TestCase.StaticPublicKeyServerY),
                nameof(TestCase.EphemeralPublicKeyServerX),
                nameof(TestCase.EphemeralPublicKeyServerY),
                nameof(TestCase.DkmNonceServer),
                nameof(TestCase.EphemeralNonceServer),
                nameof(TestCase.NonceNoKc)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var macAesCcmIncludeProperties = new[]
            {
                nameof(TestCase.NonceAesCcm)
            };
            if (macAesCcmIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.MacType == KeyAgreementMacType.AesCcm)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var valIncludeProperties = new[]
            {
                nameof(TestCase.StaticPrivateKeyIut),
                nameof(TestCase.StaticPublicKeyIutX),
                nameof(TestCase.StaticPublicKeyIutY),
                nameof(TestCase.EphemeralPrivateKeyIut),
                nameof(TestCase.EphemeralPublicKeyIutX),
                nameof(TestCase.EphemeralPublicKeyIutY),
                nameof(TestCase.DkmNonceIut),
                nameof(TestCase.EphemeralNonceIut)
            };
            if (valIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var valMacIncludeProperties = new[]
            {
                nameof(TestCase.OiLen),
                nameof(TestCase.OtherInfo),
                nameof(TestCase.Tag),

            };
            if (valMacIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase)
                            && testGroup.MacType != KeyAgreementMacType.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var valNoMacIncludeProperties = new[]
            {
                nameof(TestCase.HashZ)
            };
            if (valNoMacIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("val", StringComparison.OrdinalIgnoreCase)
                            && testGroup.MacType == KeyAgreementMacType.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
