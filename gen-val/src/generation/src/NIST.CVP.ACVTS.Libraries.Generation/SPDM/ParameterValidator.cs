using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int MIN_KEY_LENGTH = 112;
        public static int MAX_KEY_LENGTH = 4096;
        public static int[] VALID_TH_LENGTHS = [256, 384, 512];
        public static HashFunctions[] VALID_HASH_FUNCTIONS = [HashFunctions.Sha2_d256, HashFunctions.Sha2_d384, HashFunctions.Sha2_d512, HashFunctions.Sha3_d256, HashFunctions.Sha3_d384, HashFunctions.Sha3_d512];
        public static SPDMVersions[] VALID_VERSIONS = [SPDMVersions.SPDM11, SPDMVersions.SPDM12, SPDMVersions.SPDM13];
        
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            ValidateHashFunctions(parameters, errors);
            ValidateSPDMVersion(parameters, errors);
            ValidateKeyLength(parameters, errors);
            ValidateUsesPSK(parameters, errors);
            ValidateTHLength(parameters, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateHashFunctions(Parameters parameters, List<string> errors)
        {
            if (parameters.HashAlgs.Length == 0)
            {
                errors.Add($"Expected at least one {nameof(parameters.HashAlgs)}");
            }

            if (parameters.HashAlgs.Length != parameters.HashAlgs.Distinct().Count())
            {
                errors.Add($"Unexpected duplicate in {nameof(parameters.HashAlgs)}");
            }
            
            string err = ValidateArray(parameters.HashAlgs, VALID_HASH_FUNCTIONS, nameof(parameters.HashAlgs));
            errors.AddIfNotNullOrEmpty(err);
        }

        private void ValidateSPDMVersion(Parameters parameters, List<string> errors)
        {
            if (parameters.SPDMVersion.Length == 0)
            {
                errors.Add("Expected at least one spdm version");
            }

            if (parameters.SPDMVersion.Contains(SPDMVersions.None))
            {
                errors.Add("Unexpected value in spdm version");
            }

            string err = ValidateArray(parameters.SPDMVersion.Distinct(), VALID_VERSIONS, nameof(parameters.SPDMVersion));
            errors.AddIfNotNullOrEmpty(err);
        }

        private void ValidateKeyLength(Parameters parameters, List<string> errors)
        {
            if (parameters.KeyLen == null)
            {
                errors.Add("keyLen was null and is required.");
                return;
            }

            var fullDomain = parameters.KeyLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_KEY_LENGTH, MAX_KEY_LENGTH,
                "keyLen Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
            
            var multipleCheck = ValidateMultipleOf(parameters.KeyLen, 8, nameof(parameters.KeyLen));
            errors.AddIfNotNullOrEmpty(multipleCheck);
        }

        private void ValidateUsesPSK(Parameters parameters, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateBoolArray(parameters.UsesPSK, nameof(parameters.UsesPSK)));
        }
        
        private void ValidateTHLength(Parameters parameters, List<string> errors)
        {
            if (parameters.THLen == null)
            {
                errors.Add("THLen was null and is required.");
                return;
            }

            var fullDomain = parameters.THLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_TH_LENGTHS.First(), VALID_TH_LENGTHS.Last(),
                "THLen Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
            
            var multipleCheck = ValidateMultipleOf(parameters.THLen, 128, nameof(parameters.THLen));
            errors.AddIfNotNullOrEmpty(multipleCheck);
        }
    }
}
