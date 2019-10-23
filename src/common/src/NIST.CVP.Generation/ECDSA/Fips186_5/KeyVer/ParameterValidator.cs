using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.ECDSA.v1_0.KeyVer;

namespace NIST.CVP.Generation.ECDSA.Fips186_5.KeyVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = EnumHelpers.GetEnumDescriptions<Curve>().Where(c => c.Contains("p-", StringComparison.OrdinalIgnoreCase)).ToArray();    // Only p curves

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            result = ValidateArray(parameters.Curve, VALID_CURVES, "Curves");
            errors.AddIfNotNullOrEmpty(result);

            return new ParameterValidateResponse(errors);
        }
    }
}