using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc.ContractResolvers
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

            var aftIncludeZ = new[]
            {
                nameof(TestCase.Z),
            };
            if (aftIncludeZ.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase) && testGroup.HashFunctionZ == HashFunctions.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            
            var aftIncludeHashZ = new[]
            {
                nameof(TestCase.HashZ),
            };
            if (aftIncludeHashZ.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase) && testGroup.HashFunctionZ != HashFunctions.None)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
        
        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(TestGroup).IsAssignableFrom(type) ||
                typeof(TestGroupBase<TestGroup, TestCase, EccKeyPair>).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type) ||
                typeof(TestCaseBase<TestGroup, TestCase, EccKeyPair>).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }
    }
}