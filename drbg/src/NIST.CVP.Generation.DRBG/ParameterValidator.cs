using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Crypto.DRBG.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DRBG
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            DrbgAttributes attributes = null;
            var errorResults = new List<string>();

            ValidateAndGetOptions(parameters, errorResults, ref attributes);

            // Cannot validate the rest of the parameters as they are dependant on the successful validation of the mechanism and mode.
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            ValidateEntropy(parameters, attributes, errorResults);
            ValidateNonce(parameters, attributes, errorResults);
            ValidatePersonalizationString(parameters, attributes, errorResults);
            ValidateAdditionalInput(parameters, attributes, errorResults);

            if (attributes.Mechanism == DrbgMechanism.Counter)
            {
                var counterAttributes = DrbgAttributesHelper.GetCounterDrbgAttributes(attributes.Mode);
                ValidateOutputBitLength(parameters, errorResults, counterAttributes.OutputLength);
            }
            else if (attributes.Mechanism == DrbgMechanism.Hash || attributes.Mechanism == DrbgMechanism.HMAC)
            {
                var hashAttributes = DrbgAttributesHelper.GetHashDrbgAttriutes(attributes.Mode);
                ValidateOutputBitLength(parameters, errorResults, hashAttributes.OutputLength);
            }

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }
        
        private void ValidateAndGetOptions(Parameters parameters, List<string> errorResults, ref DrbgAttributes attributes)
        {
            try
            {
                attributes = DrbgAttributesHelper.GetDrbgAttributes(parameters.Algorithm, parameters.Mode, parameters.DerFuncEnabled);
            }
            catch (ArgumentException)
            {
                errorResults.Add("Invalid Algorithm/Mode provided.");
            }
        }

        private void ValidateEntropy(Parameters parameters, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.EntropyInputLen, "Entropy Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var entropyFullDomain = parameters.EntropyInputLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(
                new long[] { entropyFullDomain.Minimum , entropyFullDomain.Maximum },
                attributes.MinEntropyInputLength,
                attributes.MaxEntropyInputLength,
                "Entropy Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.EntropyInputLen, 8, "Entropy Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateNonce(Parameters parameters, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.NonceLen, "Nonce Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var nonceFullDomain = parameters.NonceLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { nonceFullDomain.Minimum, nonceFullDomain.Maximum },
                attributes.MinNonceLength,
                attributes.MaxNonceLength,
                "Nonce Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.NonceLen, 8, "Nonce Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidatePersonalizationString(Parameters parameters, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PersoStringLen, "Personalization String Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var personalizationStringFullDomain = parameters.PersoStringLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(
                new long[] {personalizationStringFullDomain.Minimum, personalizationStringFullDomain.Maximum, },
                0,
                attributes.MaxPersonalizationStringLength,
                "Personalization String Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.PersoStringLen, 8, "Personalization String Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateAdditionalInput(Parameters parameters, DrbgAttributes attributes, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.AdditionalInputLen, "Additional Input Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var additionalInputFullDomain = parameters.AdditionalInputLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(new long[] { additionalInputFullDomain.Minimum, additionalInputFullDomain.Maximum, },
                0, 
                attributes.MaxAdditionalInputStringLength,
                "Additional Input Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.AdditionalInputLen, 8, "Additional Input Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
        
        private void ValidateOutputBitLength(Parameters parameters, List<string> errorResults, int blockSize)
        {
            var modCheck = ValidateMultipleOf(new int[] {parameters.ReturnedBitsLen}, blockSize, "Returned Bits Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
