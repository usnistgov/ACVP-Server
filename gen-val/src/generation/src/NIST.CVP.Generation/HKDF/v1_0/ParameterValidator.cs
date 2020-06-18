using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.HKDF.v1_0
{
public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public const int MIN_KEY_LENGTH = 8;
        public const int MAX_KEY_LENGTH = 4096;

        public const int MIN_SALT_LENGTH = 0;
        public const int MAX_SALT_LENGTH = 512;

        public const int MIN_INFO_LENGTH = 0;
        public const int MAX_INFO_LENGTH = 512;

        public const int MIN_INPUT_LENGTH = 0;
        public const int MAX_INPUT_LENGTH = 4096;
        
        public string[] VALID_HASH_ALGS = ShaAttributes.GetShaNames().Except(new [] {"SHAKE-128", "SHAKE-256"}).ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            if (!parameters.Capabilities.Any())
            {
                errorResults.Add("No capabilities found");
            }

            foreach (var capability in parameters.Capabilities)
            {
                ValidateDomain(capability.KeyLength, nameof(capability.KeyLength), MIN_KEY_LENGTH, MAX_KEY_LENGTH, errorResults);
                ValidateDomain(capability.SaltLength, nameof(capability.SaltLength), MIN_SALT_LENGTH, MAX_SALT_LENGTH, errorResults);
                ValidateDomain(capability.InfoLength, nameof(capability.InfoLength), MIN_INFO_LENGTH, MAX_INFO_LENGTH, errorResults);
                ValidateDomain(capability.InputKeyingMaterialLength, nameof(capability.InputKeyingMaterialLength), MIN_INPUT_LENGTH, MAX_INPUT_LENGTH, errorResults);
            
                var result = ValidateArray(capability.HmacAlg, VALID_HASH_ALGS, "Hmac Algs");
                errorResults.AddIfNotNullOrEmpty(result);
            }
            
            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateDomain(MathDomain domain, string friendlyName, int min, int max, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(domain, friendlyName);
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = domain.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                min,
                max,
                friendlyName
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            var modCheck = ValidateMultipleOf(domain, 8, friendlyName);
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}