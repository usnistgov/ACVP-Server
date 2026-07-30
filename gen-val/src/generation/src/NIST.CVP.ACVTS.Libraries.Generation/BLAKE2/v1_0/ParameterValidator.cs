using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using ParameterValidatorBase = NIST.CVP.ACVTS.Libraries.Generation.Core.ParameterValidatorBase;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static readonly string[] ValidAlgorithms = { "BLAKE2b" };
        public const int MinDigestLength = 8;
        public const int MaxDigestLength = 512;
        public const int MinMessageLength = 0;
        public const int MaxMessageLength = 65536;
        public const int MinKeyLength = 0;
        public const int MaxKeyLength = 512;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            errors.AddIfNotNullOrEmpty(ValidateValue(parameters.Algorithm, ValidAlgorithms, "BLAKE2 function"));
            ValidateDigestLength(parameters, errors);
            ValidateMessageLength(parameters.MessageLength, errors);
            ValidateKeyLength(parameters.KeyLength, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateDigestLength(Parameters parameters, List<string> errors)
        {
            if (parameters.DigestLength == null)
            {
                parameters.DigestLength = new MathDomain().AddSegment(new ValueDomainSegment(MaxDigestLength));
            }

            if (ValidateDomain(parameters.DigestLength, errors, "Digest Length", MinDigestLength, MaxDigestLength))
            {
                ValidateMultipleOf(parameters.DigestLength, errors, 8, "byte-aligned digest lengths");
            }
        }

        private void ValidateMessageLength(MathDomain messageLength, List<string> errors)
        {
            if (ValidateDomain(messageLength, errors, "Message Length", MinMessageLength, MaxMessageLength))
            {
                ValidateMultipleOf(messageLength, errors, 8, "byte-aligned message lengths");
            }
        }

        private void ValidateKeyLength(MathDomain keyLength, List<string> errors)
        {
            if (keyLength == null)
            {
                return;
            }

            if (ValidateDomain(keyLength, errors, "Key Length", MinKeyLength, MaxKeyLength))
            {
                ValidateMultipleOf(keyLength, errors, 8, "byte-aligned key lengths");
            }
        }
    }
}
