using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStep.ContractResolvers
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
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.Dkm)
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

            if (typeof(OneStepConfiguration).IsAssignableFrom(type))
            {
                return OneStepConfigurationSerialization(jsonProperty);
            }

            if (typeof(KdfParameterOneStep).IsAssignableFrom(type))
            {
                return KdfParameterOneStepSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }

        private Predicate<object> OneStepConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(OneStepConfiguration.KdfType),
                nameof(OneStepConfiguration.AuxFunction),
                nameof(OneStepConfiguration.FixedInfoEncoding),
                nameof(OneStepConfiguration.FixedInfoPattern),
                nameof(OneStepConfiguration.SaltMethod),
                nameof(OneStepConfiguration.SaltLen),
                nameof(OneStepConfiguration.L),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterOneStepSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterOneStep.KdfType),
                nameof(KdfParameterOneStep.Context),
                nameof(KdfParameterOneStep.AlgorithmId),
                nameof(KdfParameterOneStep.Iv),
                nameof(KdfParameterOneStep.Label),
                nameof(KdfParameterOneStep.Salt),
                nameof(KdfParameterOneStep.Z),
                nameof(KdfParameterOneStep.EntropyBits),
                nameof(KdfParameterOneStep.L),
                nameof(KdfParameterOneStep.T),
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
