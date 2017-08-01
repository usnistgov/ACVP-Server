using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Crypto.SHAWrapper;
using System.Linq;

namespace NIST.CVP.Generation.HMAC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {

        public const int _MIN_KEY_LENGTH = 8;
        public const int _MAX_KEY_LENGTH = 524288;

        private int[] _validMacLens = null;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            ValidateAlgorithm(parameters, errorResults);

            ModeValues shaMode = ModeValues.SHA1;
            DigestSizes shaDigestSize = DigestSizes.d160;
            SetAlgorithmShaOptions(parameters, errorResults, ref shaMode, ref shaDigestSize, ref _validMacLens);

            // Cannot validate the rest of the parameters as they are dependant on the successful validation of the mechanism and mode.
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            ValidateKeyLen(parameters, errorResults);
            ValidateMacLen(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            var validAlgorithmValues = AlgorithmSpecificationToDomainMapping.Mapping
                .Select(s => s.specificationAlgorithm)
                .ToArray();

            var algoCheck = ValidateValue(parameters.Algorithm, validAlgorithmValues, "Algorithm");
            errorResults.AddIfNotNullOrEmpty(algoCheck);
        }

        private void SetAlgorithmShaOptions(Parameters parameters, List<string> errorResults, ref ModeValues shaMode, ref DigestSizes shaDigestSize, ref int[] validMacLens)
        {
            try
            {
                var result = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(parameters.Algorithm);

                shaMode = result.shaMode;
                shaDigestSize = result.shaDigestSize;
                validMacLens = result.validMacLengths;
            }
            catch (ArgumentException ex)
            {
                errorResults.AddIfNotNullOrEmpty(ex.Message);
            }
        }

        private void ValidateKeyLen(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.KeyLen, "KeyLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.KeyLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _MIN_KEY_LENGTH,
                _MAX_KEY_LENGTH,
                "KeyLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.KeyLen, 8, "KeyLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMacLen(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.MacLen, "MacLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MacLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _validMacLens.Min(),
                _validMacLens.Max(),
                "MacLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(parameters.KeyLen, 8, "MacLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);

            // TODO question to Larry regarding valid HMAC lens, 
            // perhaps take this out depending on answer
            // Validate at least one valid HMAC length (as per the validation specification) exists.
            // http://csrc.nist.gov/groups/STM/cavp/documents/mac/HMACVS.pdf
            bool found = false;
            foreach (var macLen in _validMacLens)
            {
                if (parameters.MacLen.IsWithinDomain(macLen))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                errorResults.AddIfNotNullOrEmpty("Valid MacLen value not found.");
            }
        }
    }
}
