using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DRBG
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {

        private int _seedLength;

        private long _minimumEntropy;
        private long _maximumEntropy;

        private long _minimumNonce;
        private long _maximumNonce;

        private long _minimumPersonalizationString;
        private long _maximumPersonalizationString;

        private long _minimumAdditionalInput;
        private long _maximumAdditionalInput;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            DrbgMechanism drbgMechanism = 0;
            DrbgMode drbgMode = 0;
            int securityStrength = 0;
            int blockSize = 0;

            List<string> errorResults = new List<string>();

            ValidateAndGetOptions(parameters, errorResults, ref drbgMechanism, ref drbgMode, ref securityStrength, ref blockSize);

            // Cannot validate the rest of the parameters as they are dependant on the successful validation of the mechanism and mode.
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            SetDrbgValidationAttributes(parameters, drbgMechanism, drbgMode, securityStrength);

            ValidateEntropy(parameters, errorResults);
            ValidateNonce(parameters, errorResults);
            ValidatePersonalizationString(parameters, errorResults);
            ValidateAdditionalInput(parameters, errorResults);
            ValidateOutputBitLength(parameters, errorResults, blockSize);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }
        
        private void ValidateAndGetOptions(Parameters parameters, List<string> errorResults,
            ref DrbgMechanism drbgMechanism, ref DrbgMode drbgMode, ref int securityStrength, ref int blockSize)
        {
            var found = DrbgSpecToDomainMapping.Map
                .FirstOrDefault(w => w.Item1.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) &&
                                     w.Item3.Equals(parameters.Mode, StringComparison.OrdinalIgnoreCase));

            if (found != null)
            {
                drbgMechanism = found.Item2;
                drbgMode = found.Item4;
                securityStrength = found.Item5;
                blockSize = found.Item6;
            }
            else
            {
                errorResults.Add("Invalid Algorithm/Mode provided.");
            }
        }

        /// <summary>
        /// Set the min/max values that are to be validated against - based on Parameters
        /// </summary>
        private void SetDrbgValidationAttributes(Parameters parameters, DrbgMechanism drbgMechanism, DrbgMode drbgMode,
            int securityStrength)
        {
            int outLength = 128;
            int keyLength = 0;
            switch (drbgMode)
            {
                case DrbgMode.AES128:
                    keyLength = 128;
                    break;
                case DrbgMode.AES192:
                    keyLength = 192;
                    break;
                case DrbgMode.AES256:
                    keyLength = 256;
                    break;
            }

            _seedLength = outLength + keyLength;
            _minimumAdditionalInput = 0;
            _minimumPersonalizationString = 0;

            if (parameters.DerFuncEnabled)
            {
                _minimumEntropy = securityStrength;
                _maximumEntropy = (long) 1 << 35;
                _minimumNonce = securityStrength / 2;
                _maximumNonce = (long) 1 << 35;
                _maximumPersonalizationString = (long) 1 << 35;
                _maximumAdditionalInput = (long) 1 << 35;
            }
            else
            {
                _minimumEntropy = _seedLength;
                _maximumEntropy = _seedLength;
                _minimumNonce = 0;
                _maximumNonce = (long)1 << 35;
                _maximumPersonalizationString = _seedLength;
                _maximumAdditionalInput = _seedLength;
            }
        }

        private void ValidateEntropy(Parameters parameters, List<string> errorResults)
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
                _minimumEntropy, 
                _maximumEntropy,
                "Entropy Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.EntropyInputLen, 8, "Entropy Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateNonce(Parameters parameters, List<string> errorResults)
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
                _minimumNonce, 
                _maximumNonce,
                "Nonce Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.NonceLen, 8, "Nonce Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidatePersonalizationString(Parameters parameters, List<string> errorResults)
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
                _minimumPersonalizationString, 
                _maximumPersonalizationString,
                "Personalization String Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.PersoStringLen, 8, "Personalization String Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateAdditionalInput(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.AdditionalInputLen, "Additional Input Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var additionalInputFullDomain = parameters.AdditionalInputLen.GetDomainMinMax();

            var rangeCheck = ValidateRange(new long[] { additionalInputFullDomain.Minimum, additionalInputFullDomain.Maximum, },
                _minimumAdditionalInput, 
                _maximumAdditionalInput,
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
