using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int MIN_PLAINTEXT_LENGTH = 0;
        public static int MAX_PLAINTEXT_LENGTH = 65536;
        public static int MIN_AD_LENGTH = 0;
        public static int MAX_AD_LENGTH = 65536;
        public static int MIN_TRUNC_LENGTH = 32;
        public static int MAX_TRUNC_LENGTH = 128;
        
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            ValidateDirections(parameters, errors);
            ValidatePlaintextLength(parameters, errors);
            ValidateADLength(parameters, errors);
            ValidateTagLength(parameters, errors);
            ValidateNonceMasking(parameters, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateDirections(Parameters parameters, List<string> errors)
        {
            if (!parameters.Direction.Any())
            {
                errors.Add("Expected at least one cipher direction");
            }

            if (parameters.Direction.Contains(BlockCipherDirections.None))
            {
                errors.Add("Unexpected value in cipher direction");
            }

            string err = ValidateArray(parameters.Direction.Distinct(), [BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt], "Direction");
            errors.AddIfNotNullOrEmpty(err);
        }

        private void ValidatePlaintextLength(Parameters parameters, List<string> errors)
        {
            if (parameters.PayloadLen == null)
            {
                errors.Add("payloadLen was null and is required.");
                return;
            }

            var fullDomain = parameters.PayloadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_PLAINTEXT_LENGTH, MAX_PLAINTEXT_LENGTH,
                "payloadLen Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateADLength(Parameters parameters, List<string> errors)
        {
            if (parameters.AadLen == null)
            {
                errors.Add("adLen was null and is required.");
                return;
            }

            var fullDomain = parameters.AadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_AD_LENGTH, MAX_AD_LENGTH,
                "AadLen Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateTagLength(Parameters parameters, List<string> errors)
        {
            if (parameters.TagLen == null)
            {
                errors.Add("tagLen was null and is required.");
                return;
            }
            
            var fullDomain = parameters.TagLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_TRUNC_LENGTH, MAX_TRUNC_LENGTH,
                "TruncationLength Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateNonceMasking(Parameters parameters, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateBoolArray(parameters.SupportsNonceMasking, "SupportsNonceMasking"));
        }
    }
}
