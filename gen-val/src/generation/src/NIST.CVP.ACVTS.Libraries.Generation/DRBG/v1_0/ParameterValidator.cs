using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        private static int MaxOutputSize = 4096;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            DrbgAttributes attributes = null;
            var errorResults = new List<string>();

            var algoModeRevision =
                AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);

            if (parameters.PredResistanceEnabled == null)
            {
                errorResults.Add("predResistance array must be provided.");
                return new ParameterValidateResponse(errorResults);
            }

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
                if (new[] { AlgoMode.DRBG_Hash_v1_0, AlgoMode.DRBG_HMAC_v1_0 }.Contains(algoModeRevision) &&
                    capability.DerFuncEnabled)
                {
                    errorResults.Add("The derFuncEnabled property is not valid for HASH and HMAC DRBG testing.");
                    continue;
                }

                ValidateAndGetOptions(parameters, capability, errorResults, ref attributes);

                // Cannot validate the rest of the parameters as they are dependant on the successful validation of the mechanism and mode.
                if (errorResults.Count > 0)
                {
                    return new ParameterValidateResponse(errorResults);
                }

                ValidateEntropyAndNonce(algoModeRevision, capability, attributes, errorResults);
                ValidatePersonalizationString(capability, attributes, errorResults);
                ValidateAdditionalInput(capability, attributes, errorResults);

                var baseOutputLength = 0;
                if (attributes.Mechanism == DrbgMechanism.Counter)
                {
                    baseOutputLength = DrbgAttributesHelper.GetCounterDrbgAttributes(attributes.Mode).OutputLength;
                }
                else if (attributes.Mechanism == DrbgMechanism.Hash || attributes.Mechanism == DrbgMechanism.HMAC)
                {
                    baseOutputLength = DrbgAttributesHelper.GetHashDrbgAttributes(attributes.Mode).OutputLength;
                }

                ValidateOutputBitLength(capability, errorResults, baseOutputLength);
            }

            return new ParameterValidateResponse(errorResults);
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

        private void ValidateEntropyAndNonce(AlgoMode algoModeRevision, Capability capability, DrbgAttributes attributes, List<string> errorResults)
        {
            var entropySegmentCheck = ValidateSegmentCountGreaterThanZero(capability.EntropyInputLen, "Entropy Domain");
            errorResults.AddIfNotNullOrEmpty(entropySegmentCheck);
            var nonceSegmentCheck = ValidateSegmentCountGreaterThanZero(capability.NonceLen, "Nonce Domain");
            errorResults.AddIfNotNullOrEmpty(nonceSegmentCheck);
            if (!string.IsNullOrEmpty(entropySegmentCheck) || !string.IsNullOrEmpty(nonceSegmentCheck))
            {
                return;
            }

            var entropyFullDomain = capability.EntropyInputLen.GetDomainMinMax();
            var nonceFullDomain = capability.NonceLen.GetDomainMinMax();

            // 1) Verify the supplied entropy input lengths, i.e., is the supplied entropyInputLen within the valid range of entropy input len values?
            var entropyRangeCheck = ValidateRange(
                new long[] { entropyFullDomain.Minimum, entropyFullDomain.Maximum },
                attributes.MinEntropyInputLength,
                attributes.MaxEntropyInputLength,
                "Entropy Range"
            );
            errorResults.AddIfNotNullOrEmpty(entropyRangeCheck);

            // 2) Perform checks to verify that the supplied nonce lengths are valid
            // 2)a.) Drbgs/scenarios where nonces are prohibited/not used (a nonceLen range of 0 should be supplied to indicate this)
            if (algoModeRevision == AlgoMode.DRBG_CTR_v1_0 && !capability.DerFuncEnabled)
            {
                var nonceCheckCtrDrbgDerFuncFalse = ValidateRange(
                    new long[] { nonceFullDomain.Minimum, nonceFullDomain.Maximum },
                    attributes.MinNonceLength,
                    attributes.MaxNonceLength,
                    $"Nonce Range for {capability.Mode}"
                );
                
                errorResults.AddIfNotNullOrEmpty(nonceCheckCtrDrbgDerFuncFalse);
            }
            else
            {
                // 2)b.) Drbgs/scenarios where nonces are required.
                // Ordinarily, SP 800-90Ar1 requires that nonce's contain at least 1/2 * security strength bits of entropy and
                // that the entropy input contain at least security strength bits of entropy. We would ordinarily check that
                // the smallest value supplied for nonceLen is >= 1/2 * security strength bits, but SP 800-90Ar1 actually isn't that strict. 
                // What it really cares about is that the entropy input + nonce contains 3/2 * security strength bits of entropy.
                // So, instead of checking that nonceLen is >= 1/2 * security strength bits, what we really want to check is that 
                // the smallest value supplied for entropyInputLen + the smallest value supplied for nonceLen is >= 3/2 * security strength bits.
                if (entropyFullDomain.Minimum + nonceFullDomain.Minimum < attributes.MinEntropyInputLength + attributes.MinNonceLength)
                    errorResults.Add($"The supplied min entropyInputLen ({entropyFullDomain.Minimum}) + the supplied min nonceLen ({nonceFullDomain.Minimum}) must be >= 3/2 security strength bits or {attributes.MinEntropyInputLength + attributes.MinNonceLength} bits, but is {entropyFullDomain.Minimum + nonceFullDomain.Minimum} bits.");

                // also verify that the supplied nonceLens do not exceed the maximum allow nonceLens
                var nonceMaxCheck = ValidateRange(
                    new long[] { nonceFullDomain.Maximum },
                    0,
                    attributes.MaxNonceLength,
                    $"Nonce Max for {capability.Mode}"
                );

                errorResults.AddIfNotNullOrEmpty(nonceMaxCheck);
            }
            
            var entropyModCheck = ValidateMultipleOf(capability.EntropyInputLen, 8, "Entropy Modulus");
            errorResults.AddIfNotNullOrEmpty(entropyModCheck);
            
            var nonceModCheck = ValidateMultipleOf(capability.NonceLen, 8, "Nonce Modulus");
            errorResults.AddIfNotNullOrEmpty(nonceModCheck);
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
                new long[] { personalizationStringFullDomain.Minimum, personalizationStringFullDomain.Maximum, },
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

        private void ValidateOutputBitLength(Capability capability, List<string> errorResults, int minimumOutputLength)
        {
            var modCheck = ValidateMultipleOf(new[] { capability.ReturnedBitsLen }, BitString.BITSINBYTE, "Returned Bits Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);

            if (capability.ReturnedBitsLen < minimumOutputLength)
            {
                errorResults.Add($"Requested {nameof(capability.ReturnedBitsLen)} of {capability.ReturnedBitsLen} is below the minimum allowed length of {minimumOutputLength}");
            }

            if (capability.ReturnedBitsLen > MaxOutputSize)
            {
                errorResults.Add($"Requested {nameof(capability.ReturnedBitsLen)} of {capability.ReturnedBitsLen} exceeds maximum allowed length of {MaxOutputSize}");
            }
        }
    }
}
