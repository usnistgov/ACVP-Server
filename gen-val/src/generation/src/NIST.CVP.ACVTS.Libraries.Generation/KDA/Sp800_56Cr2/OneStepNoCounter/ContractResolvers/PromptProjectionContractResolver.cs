using System;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Generation.Core.ContractResolvers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.OneStepNoCounter.ContractResolvers
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

            if (typeof(OneStepNoCounterConfiguration).IsAssignableFrom(type))
            {
                return OneStepNoCounterConfigurationSerialization(jsonProperty);
            }

            if (typeof(KdfParameterOneStepNoCounter).IsAssignableFrom(type))
            {
                return KdfParameterOneStepSerialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }

        private Predicate<object> OneStepNoCounterConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(OneStepNoCounterConfiguration.KdfType),
                nameof(OneStepNoCounterConfiguration.AuxFunction),
                nameof(OneStepNoCounterConfiguration.FixedInfoEncoding),
                nameof(OneStepNoCounterConfiguration.FixedInfoPattern),
                nameof(OneStepNoCounterConfiguration.SaltMethod),
                nameof(OneStepNoCounterConfiguration.SaltLen),
                nameof(OneStepNoCounterConfiguration.L),
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
                nameof(KdfParameterOneStepNoCounter.KdfType),
                nameof(KdfParameterOneStepNoCounter.Context),
                nameof(KdfParameterOneStepNoCounter.AlgorithmId),
                nameof(KdfParameterOneStepNoCounter.Iv),
                nameof(KdfParameterOneStepNoCounter.Label),
                nameof(KdfParameterOneStepNoCounter.Salt),
                nameof(KdfParameterOneStepNoCounter.Z),
                nameof(KdfParameterOneStepNoCounter.EntropyBits),
                nameof(KdfParameterOneStepNoCounter.L),
                nameof(KdfParameterOneStepNoCounter.T),
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
