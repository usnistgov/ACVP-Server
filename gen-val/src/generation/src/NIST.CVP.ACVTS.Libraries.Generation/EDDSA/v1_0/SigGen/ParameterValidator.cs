using System.Collections.Generic;
using System.Linq;
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
            
            if (!parameters.PreHash && !parameters.Pure)
            {
                errors.Add("No valid signature generation mode chosen; PreHash or Pure must be true");
            }
            
            //  1) we want to make the contextLength registration property mandatory when a) "curve" contains "ED-448" and b) mandatory when "preHash" : true 
            if (parameters.ContextLength == null)
            {
                if (parameters.Curve.Contains("ED-448"))
                {
                    errors.Add("A ContextLength was not provided and is required when testing with the ED-449 curve.");
                }

                if (parameters.PreHash)
                {
                    errors.Add("A ContextLength was not provided and is required when testing with PreHash.");
                }
            }
            
            // 2) we want to reject this specific registration:
            //      "preHash": false,
            //      "pure": true,
            //      "curve" : ["ED-25519"]
            //      "contextLength" : [9] <-- the contextLength property can take on any value
            if (parameters.ContextLength != null)
            {
                if (parameters.Curve.Length == 1
                    && parameters.Curve.GetValue(0).Equals("ED-25519")
                    && parameters.Pure
                    && !parameters.PreHash)
                {
                    errors.Add("ContextLength is not a valid registration property for the ED-25519 curve when Pure is true and PreHash false.");
                }
            }
            
            // Max ContextLength is 255 octets based on FIPS 186-5, sections 7.6 and 7.8
            if (parameters.ContextLength != null)
            {
                if (parameters.ContextLength.GetDomainMinMax().Maximum > 255)
                {
                    errors.Add("Invalid ContextLength maximum. It must be below 256 octets.");
                }
            }
            
            // Minimum is 0
            if (parameters.ContextLength != null)
            {
                if (parameters.ContextLength.GetDomainMinMax().Minimum < 0)
                {
                    errors.Add("Invalid ContextLength maximum, it must be 0 or above.");
                }
            }
            
            return new ParameterValidateResponse(errors);
        }
    }
}
