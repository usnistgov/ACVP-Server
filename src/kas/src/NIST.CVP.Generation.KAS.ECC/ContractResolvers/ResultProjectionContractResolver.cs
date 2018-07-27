using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Linq;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Generation.KAS.ECC.ContractResolvers
{
    public class ResultProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.Tests)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId)
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var valIncludeProperties = new[]
            {
                nameof(TestCase.TestPassed)
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

            var aftIncludeProperties = new[]
            {
                nameof(TestCase.StaticPublicKeyIutX),
                nameof(TestCase.StaticPublicKeyIutY),
                nameof(TestCase.EphemeralPublicKeyIutX),
                nameof(TestCase.EphemeralPublicKeyIutY),
                nameof(TestCase.DkmNonceIut),
                nameof(TestCase.EphemeralNonceIut),
            };
            if (aftIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var aftMacCcmIncludeProperties = new[]
            {
                nameof(TestCase.NonceAesCcm)
            };
            if (aftMacCcmIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase)
                            && testGroup.MacType == KeyAgreementMacType.AesCcm)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var aftMacIncludeProperties = new[]
            {
                nameof(TestCase.IdIutLen),
                nameof(TestCase.IdIut),
                nameof(TestCase.OiLen),
                nameof(TestCase.OtherInfo),
                nameof(TestCase.Tag),
            };
            if (aftMacIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase)
                            && testGroup.MacType != KeyAgreementMacType.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }

            var aftNoMacIncludeProperties = new[]
            {
                nameof(TestCase.HashZ)
            };
            if (aftNoMacIncludeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase)
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
