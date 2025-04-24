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

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.CXOF128
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int MIN_MESSAGE_LENGTH = 0;
        public static int MAX_MESSAGE_LENGTH = 65536;
        public static int MIN_DIGEST_LENGTH = 1;
        public static int MAX_DIGEST_LENGTH = 65536;
        public static int MIN_CS_LENGTH = 0;
        public static int MAX_CS_LENGTH = 2048;
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            ValidateMessageLength(parameters, errors);
            ValidateDigestLength(parameters, errors);
            ValidateCSLength(parameters, errors);

            return new ParameterValidateResponse(errors);
        }
        private void ValidateMessageLength(Parameters parameters, List<string> errors)
        {
            if (parameters.MessageLength == null)
            {
                errors.Add("messageLength was null and is required.");
                return;
            }

            var fullDomain = parameters.MessageLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_MESSAGE_LENGTH, MAX_MESSAGE_LENGTH,
                "MessageLength Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateDigestLength(Parameters parameters, List<string> errors)
        {
            if (parameters.DigestLength == null)
            {
                errors.Add("digestLength was null and is required.");
                return;
            }

            var fullDomain = parameters.DigestLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_DIGEST_LENGTH, MAX_DIGEST_LENGTH,
                "DigestLength Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateCSLength(Parameters parameters, List<string> errors)
        {
            if (parameters.CustomizationStringLength == null)
            {
                errors.Add("customizationStringLength was null and is required.");
                return;
            }

            var fullDomain = parameters.CustomizationStringLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                MIN_CS_LENGTH, MAX_CS_LENGTH,
                "CustomizationStringLength Range"
            );
            errors.AddIfNotNullOrEmpty(rangeCheck);
        }
    }


}
