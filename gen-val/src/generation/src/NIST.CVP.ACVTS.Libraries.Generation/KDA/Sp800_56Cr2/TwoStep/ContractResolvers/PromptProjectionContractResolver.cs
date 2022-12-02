using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.TwoStep.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            var includePropertiesAllScenarios = new[]
            {
                nameof(TestGroup.TestGroupId),
                nameof(TestGroup.TestType),
                nameof(TestGroup.KdfConfiguration),
                nameof(TestGroup.KdfMultiExpansionConfiguration),
                nameof(TestGroup.UsesHybridSharedSecret),
                nameof(TestGroup.ZLength),
                nameof(TestGroup.AuxSharedSecretLen),
                nameof(TestGroup.MultiExpansion),
                nameof(TestGroup.Tests),
            };

            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // VAL tests - include private keys, public keys, Cs, nonce, salt from both parties
            // AFT tests - include, public keys, Cs, nonce, salt from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.KdfParameter),
                nameof(TestCase.FixedInfoPartyU),
                nameof(TestCase.FixedInfoPartyV),
                nameof(TestCase.KdfMultiExpansionParameter)
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.Dkm),
                nameof(TestCase.Dkms),
            };

            if (includePropertiesValScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
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
            #endregion Conditional Test Case properties

            return jsonProperty.ShouldSerialize = instance => false;
        }

        protected override Predicate<object> ShouldSerialize(JsonProperty jsonProperty)
        {
            var type = jsonProperty.DeclaringType;

            if (typeof(TestGroup).IsAssignableFrom(type))
            {
                return TestGroupSerialization(jsonProperty);
            }

            if (typeof(TestCase).IsAssignableFrom(type))
            {
                return TestCaseSerialization(jsonProperty);
            }

            if (typeof(TwoStepConfiguration).IsAssignableFrom(type))
            {
                return TwoStepConfigurationSerialization(jsonProperty);
            }

            if (typeof(KdfParameterTwoStep).IsAssignableFrom(type))
            {
                return KdfParameterTwoStepSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }

        private Predicate<object> TwoStepConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TwoStepConfiguration.KdfType),
                nameof(TwoStepConfiguration.KdfMode),
                nameof(TwoStepConfiguration.FixedInfoEncoding),
                nameof(TwoStepConfiguration.FixedInfoPattern),
                nameof(TwoStepConfiguration.SaltMethod),
                nameof(TwoStepConfiguration.SaltLen),
                nameof(TwoStepConfiguration.CounterLen),
                nameof(TwoStepConfiguration.CounterLocation),
                nameof(TwoStepConfiguration.IvLen),
                nameof(TwoStepConfiguration.MacMode),
                nameof(TwoStepConfiguration.L),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterTwoStepSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterTwoStep.KdfType),
                nameof(KdfParameterTwoStep.Context),
                nameof(KdfParameterTwoStep.AlgorithmId),
                nameof(KdfParameterTwoStep.Iv),
                nameof(KdfParameterTwoStep.Label),
                nameof(KdfParameterTwoStep.Salt),
                nameof(KdfParameterTwoStep.Z),
                nameof(KdfParameterTwoStep.T),
                nameof(KdfParameterTwoStep.L),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
    }
}
