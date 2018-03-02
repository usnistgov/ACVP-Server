using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.Common.DRBG.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            DrbgAttributes attributes = null;
            var errorResults = new List<string>();

            if (parameters.PredResistanceEnabled.Length != 1 && parameters.PredResistanceEnabled.Length != 2)
            {
                errorResults.Add("predResistance must be a minimal array of booleans");
            }

            if (parameters.PredResistanceEnabled.Distinct().Count() != parameters.PredResistanceEnabled.Length)
            {
                errorResults.Add("predResistance must have distinct elements");
            }

            foreach (var capability in parameters.Capabilities)
            {
                ValidateAndGetOptions(parameters, capability, errorResults, ref attributes);

                // Cannot validate the rest of the parameters as they are dependant on the successful validation of the mechanism and mode.
                if (errorResults.Count > 0)
                {
                    return new ParameterValidateResponse(string.Join(";", errorResults));
                }

                ValidateEntropy(capability, attributes, errorResults);
                ValidateNonce(capability, attributes, errorResults);
                ValidatePersonalizationString(capability, attributes, errorResults);
                ValidateAdditionalInput(capability, attributes, errorResults);

                // TODO this can be removed with an interface
                var baseOutputLength = 0;
                if (attributes.Mechanism == DrbgMechanism.Counter)
                {
                    baseOutputLength = DrbgAttributesHelper.GetCounterDrbgAttributes(attributes.Mode).OutputLength;
                }
                else if (attributes.Mechanism == DrbgMechanism.Hash || attributes.Mechanism == DrbgMechanism.HMAC)
                {
                    baseOutputLength = DrbgAttributesHelper.GetHashDrbgAttriutes(attributes.Mode).OutputLength;
                }

                ValidateOutputBitLength(capability, errorResults, baseOutputLength);
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }
        
        private void ValidateAndGetOptions(Parameters parameters, Capability capability, List<string> errorResults, ref DrbgAttributes attributes)
        {
            try
            {
                attributes = DrbgAttributesHelper.GetDrbgAttributes(parameters.Algorithm, capability.Mode, capability.DerFuncEnabled);
            }
            catch (ArgumentException)
            {
                errorResults.Add("Invalid Algorithm/Mode provided.");
                return;
            }

            if (attributes.Mechanism != DrbgMechanism.Counter && capability.DerFuncEnabled)
            {
                errorResults.Add("Derivation Function not supported for hash/hmac based drbgs");
            }
        }

        private void ValidateEntropy(Capability capability, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(capability.EntropyInputLen, "Entropy Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var entropyFullDomain = capability.EntropyInputLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(
                new long[] { entropyFullDomain.Minimum , entropyFullDomain.Maximum },
                attributes.MinEntropyInputLength,
                attributes.MaxEntropyInputLength,
                "Entropy Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(capability.EntropyInputLen, 8, "Entropy Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateNonce(Capability capability, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(capability.NonceLen, "Nonce Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var nonceFullDomain = capability.NonceLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { nonceFullDomain.Minimum, nonceFullDomain.Maximum },
                attributes.MinNonceLength,
                attributes.MaxNonceLength,
                "Nonce Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(capability.NonceLen, 8, "Nonce Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidatePersonalizationString(Capability capability, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(capability.PersoStringLen, "Personalization String Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var personalizationStringFullDomain = capability.PersoStringLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(
                new long[] {personalizationStringFullDomain.Minimum, personalizationStringFullDomain.Maximum, },
                0,
                attributes.MaxPersonalizationStringLength,
                "Personalization String Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(capability.PersoStringLen, 8, "Personalization String Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateAdditionalInput(Capability capability, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(capability.AdditionalInputLen, "Additional Input Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var additionalInputFullDomain = capability.AdditionalInputLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(new long[] { additionalInputFullDomain.Minimum, additionalInputFullDomain.Maximum, },
                0, 
                attributes.MaxAdditionalInputStringLength,
                "Additional Input Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(capability.AdditionalInputLen, 8, "Additional Input Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
        
        private void ValidateOutputBitLength(Capability capability, List<string> errorResults, int blockSize)
        {
            var modCheck = ValidateMultipleOf(new [] {capability.ReturnedBitsLen}, blockSize, "Returned Bits Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
