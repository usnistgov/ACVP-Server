using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SpComponent
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (parameters.KeyFormat == PrivateKeyModes.Invalid)
            {
                errors.Add("Invalid private key format.");
            }

            if (parameters.PublicExponentMode == PublicExponentModes.Invalid)
            {
                errors.Add("Invalid public exponent mode.");
            }
            else if (parameters.PublicExponentMode == PublicExponentModes.Fixed)
            {
                if (parameters.PublicExponent == null || parameters.PublicExponent.BitLength == 0)
                {
                    errors.Add("No public exponent provided.");
                }
                else if (!RsaKeyHelper.IsValidExponent(parameters.PublicExponent.ToPositiveBigInteger()))
                {
                    errors.Add("Invalid public exponent provided.");
                }
            }

            return new ParameterValidateResponse(errors);
        }
    }
}
