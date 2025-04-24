using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Common;
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
        public static int MIN_TRUNC_LENGTH = 64;
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
            if (!parameters.Directions.Any())
            {
                errors.Add("Expected at least one cipher direction");
            }

            if (parameters.Directions.Contains(BlockCipherDirections.None))
            {
                errors.Add("Unexpectd value in ciper direction");
            }

            string err = ValidateArray<BlockCipherDirections>(parameters.Directions.Distinct(), new[] { BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt }, "Direction");
            errors.AddIfNotNullOrEmpty(err);
        }

        private void ValidatePlaintextLength(Parameters parameters, List<string> errors)
        {
            if (parameters.PayloadLength == null)
            {
                errors.Add("plaintextLength was null and is required.");
                return;
            }

            var fullDomain = parameters.PayloadLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_PLAINTEXT_LENGTH, MAX_PLAINTEXT_LENGTH,
                "PlaintextLength Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateADLength(Parameters parameters, List<string> errors)
        {
            if (parameters.ADLength == null)
            {
                errors.Add("associatedDataLength was null and is required.");
                return;
            }

            var fullDomain = parameters.ADLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_AD_LENGTH, MAX_AD_LENGTH,
                "ADLength Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateTagLength(Parameters parameters, List<string> errors)
        {
            if (parameters.TagLength.DomainSegments.Count() > 1)
            {
                errors.Add("tagLength must have exactly one segment in the domain.");
                return;
            }

            var fullDomain = parameters.TagLength.GetDomainMinMax();
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
