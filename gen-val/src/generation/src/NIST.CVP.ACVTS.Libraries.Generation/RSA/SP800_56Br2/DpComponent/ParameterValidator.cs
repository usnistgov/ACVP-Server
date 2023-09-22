using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        static readonly int[] VALID_MODULI = { 2048, 3072, 4096 };
        static readonly PrivateKeyModes[] VALID_KEY_FORMATS = { PrivateKeyModes.Standard, PrivateKeyModes.Crt };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.KeyFormat, VALID_KEY_FORMATS, "keyFormat"));
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Modulo, VALID_MODULI, "modulo"));

            return new ParameterValidateResponse(errorResults);
        }
    }
}
