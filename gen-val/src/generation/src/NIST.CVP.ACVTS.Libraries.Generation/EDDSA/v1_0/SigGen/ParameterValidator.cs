using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = { "ED-25519", "ED-448" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            result = ValidateArray(parameters.Curve, VALID_CURVES, "Curves");
            errors.AddIfNotNullOrEmpty(result);

            // Max ContextLength is 255 octets based on FIPS 186-5, sections 7.6 and 7.8
            if (parameters.ContextLength.GetDomainMinMax().Maximum > 255)
            {
                errors.Add("Invalid ContextLength maximum. It must be below 256 octets.");
            }
            
            if (!parameters.PreHash && !parameters.Pure)
            {
                errors.Add("No valid signature generation mode chosen; PreHash or Pure must be true");
            }

            return new ParameterValidateResponse(errors);
        }
    }
}
