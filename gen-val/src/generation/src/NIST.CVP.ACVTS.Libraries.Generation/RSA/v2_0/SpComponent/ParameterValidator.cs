using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        static readonly int[] VALID_MODULI = { 2048, 3072, 4096 };
        static readonly PrivateKeyModes[] VALID_KEY_FORMATS = { PrivateKeyModes.Standard, PrivateKeyModes.Crt };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.KeyFormat, VALID_KEY_FORMATS, "keyFormat"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Modulus, VALID_MODULI, "modulo"));

            if (parameters.PublicExponentMode == PublicExponentModes.Invalid)
            {
                errorResults.Add("Invalid or no public exponent mode provided");
            }
            else if (parameters.PublicExponentMode == PublicExponentModes.Fixed)
            {
                if (parameters.PublicExponentValue == null || parameters.PublicExponentValue.BitLength == 0)
                {
                    errorResults.Add("No public exponent provided.");
                }
                else if (!RsaKeyHelper.IsValidExponent(parameters.PublicExponentValue.ToPositiveBigInteger()))
                {
                    errorResults.Add("Invalid public exponent provided.");
                }
            }
            
            return new ParameterValidateResponse(errorResults);
        }
    }
}
