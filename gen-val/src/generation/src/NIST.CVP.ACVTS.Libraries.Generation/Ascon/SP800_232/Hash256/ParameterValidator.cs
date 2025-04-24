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

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.Hash256
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int MIN_MESSAGE_LENGTH = 0;
        public static int MAX_MESSAGE_LENGTH = 65536;
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            ValidateMessageLength(parameters, errors);

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
    }


}
