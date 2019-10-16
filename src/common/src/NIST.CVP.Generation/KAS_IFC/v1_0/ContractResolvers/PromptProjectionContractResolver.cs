using Newtonsoft.Json.Serialization;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Generation.Core.ContractResolvers;
using System;
using System.Linq;

namespace NIST.CVP.Generation.KAS_IFC.v1_0.ContractResolvers
{
    public class PromptProjectionContractResolver : ProjectionContractResolverBase<TestGroup, TestCase>
    {
        protected override Predicate<object> TestGroupSerialization(JsonProperty jsonProperty)
        {
            return jsonProperty.ShouldSerialize = instance => true;
        }

        protected override Predicate<object> TestCaseSerialization(JsonProperty jsonProperty)
        {
            // VAL tests - include private keys, public keys, Cs, nonce, salt from both parties
            // AFT tests - include, public keys, Cs, nonce, salt from only the server party perspective

            var includePropertiesAllScenarios = new[]
            {
                nameof(TestCase.TestCaseId),
                nameof(TestCase.IutE),
                nameof(TestCase.IutN),
                nameof(TestCase.ServerE),
                nameof(TestCase.ServerN),
                nameof(TestCase.ServerC),
                nameof(TestCase.ServerNonce),
                nameof(TestCase.Salt),
                nameof(TestCase.KdfParameter)
            };
            if (includePropertiesAllScenarios.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            #region Conditional Test Case properties
            var includePropertiesValScenarios = new[]
            {
                nameof(TestCase.ServerD),
                nameof(TestCase.ServerP),
                nameof(TestCase.ServerQ),
                nameof(TestCase.ServerDmp1),
                nameof(TestCase.ServerDmq1),
                nameof(TestCase.ServerIqmp),
                nameof(TestCase.IutD),
                nameof(TestCase.IutP),
                nameof(TestCase.IutQ),
                nameof(TestCase.IutDmp1),
                nameof(TestCase.IutDmq1),
                nameof(TestCase.IutIqmp),
                nameof(TestCase.Tag)
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

            return jsonProperty.ShouldSerialize = instance => false;
            #endregion Conditional Test Case properties
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

            if (typeof(TwoStepConfiguration).IsAssignableFrom(type))
            {
                return TwoStepConfigurationSerialization(jsonProperty);
            }

            if (typeof(IkeV1Configuration).IsAssignableFrom(type))
            {
                return IkeV1ConfigurationSerialization(jsonProperty);
            }

            if (typeof(IkeV2Configuration).IsAssignableFrom(type))
            {
                return IkeV2ConfigurationSerialization(jsonProperty);
            }


            if (typeof(KdfParameterOneStep).IsAssignableFrom(type))
            {
                return KdfParameterOneStepSerialization(jsonProperty);
            }

            if (typeof(KdfParameterTwoStep).IsAssignableFrom(type))
            {
                return KdfParameterTwoStepSerialization(jsonProperty);
            }

            if (typeof(KdfParameterIkeV1).IsAssignableFrom(type))
            {
                return KdfParameterIkeV1Serialization(jsonProperty);
            }

            if (typeof(KdfParameterIkeV2).IsAssignableFrom(type))
            {
                return KdfParameterIkeV2Serialization(jsonProperty);
            }

            return jsonProperty.ShouldSerialize;
        }

        #region KdfConfiguration
        private Predicate<object> OneStepConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(OneStepConfiguration.L),
                nameof(OneStepConfiguration.KdfType),
                nameof(OneStepConfiguration.AuxFunction),
                nameof(OneStepConfiguration.FixedInputEncoding),
                nameof(OneStepConfiguration.FixedInputPattern),
                nameof(OneStepConfiguration.SaltLen),
                nameof(OneStepConfiguration.SaltMethod),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> TwoStepConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(TwoStepConfiguration.L),
                nameof(TwoStepConfiguration.KdfType),
                nameof(TwoStepConfiguration.FixedInputEncoding),
                nameof(TwoStepConfiguration.FixedInputPattern),
                nameof(TwoStepConfiguration.SaltLen),
                nameof(TwoStepConfiguration.SaltMethod),
                nameof(TwoStepConfiguration.CounterLen),
                nameof(TwoStepConfiguration.CounterLocation),
                nameof(TwoStepConfiguration.IvLen),
                nameof(TwoStepConfiguration.MacMode),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> IkeV1ConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(IkeV1Configuration.L),
                nameof(IkeV1Configuration.KdfType),
                nameof(IkeV1Configuration.HashFunction),
                nameof(IkeV1Configuration.RequiresAdditionalNoncePair),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> IkeV2ConfigurationSerialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(IkeV2Configuration.L),
                nameof(IkeV2Configuration.KdfType),
                nameof(IkeV2Configuration.HashFunction),
                nameof(IkeV2Configuration.RequiresAdditionalNoncePair),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
        #endregion KdfConfiguration

        #region KdfParameter
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
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterIkeV1Serialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterIkeV1.KdfType),
                nameof(KdfParameterIkeV1.AdditionalInitiatorNonce),
                nameof(KdfParameterIkeV1.AdditionalResponderNonce),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }

        private Predicate<object> KdfParameterIkeV2Serialization(JsonProperty jsonProperty)
        {
            var includeProperties = new[]
            {
                nameof(KdfParameterIkeV2.KdfType),
                nameof(KdfParameterIkeV2.AdditionalInitiatorNonce),
                nameof(KdfParameterIkeV2.AdditionalResponderNonce),
            };

            if (includeProperties.Contains(jsonProperty.UnderlyingName, StringComparer.OrdinalIgnoreCase))
            {
                return jsonProperty.ShouldSerialize =
                    instance => true;
            }

            return jsonProperty.ShouldSerialize = instance => false;
        }
        #endregion KdfParameter
    }
}
