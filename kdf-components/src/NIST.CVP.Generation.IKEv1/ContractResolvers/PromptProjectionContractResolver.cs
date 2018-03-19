using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;

namespace NIST.CVP.Generation.IKEv1.ContractResolvers
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
                nameof(TestGroup.HashAlgName),
                nameof(TestGroup.AuthenticationMethod),
                nameof(TestGroup.NInitLength),
                nameof(TestGroup.NRespLength),
                nameof(TestGroup.GxyLength),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test properties
            if (jsonProperty.UnderlyingName == nameof(TestGroup.PreSharedKeyLength))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestGroupFromTestGroupObject(instance, out var testGroup);

                        if (testGroup.AuthenticationMethod == AuthenticationMethods.Psk)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test properties

            return jsonProperty.ShouldSerialize = instance => false;
        }

        
        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.NInit),
                nameof(TestCase.NResp),
                nameof(TestCase.CkyInit),
                nameof(TestCase.CkyResp),
                nameof(TestCase.Gxy),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test properties
            if (jsonProperty.UnderlyingName == nameof(TestCase.PreSharedKey))
            {
                return jsonProperty.ShouldSerialize =
                    instance =>
                    {
                        GetTestCaseFromTestCaseObject(instance, out var testGroup, out var testCase);

                        if (testGroup.AuthenticationMethod == AuthenticationMethods.Psk)
                        {
                            return true;
                        }

                        return false;
                    };
            }
            #endregion Conditional Test properties

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}