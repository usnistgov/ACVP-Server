using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public string Algorithm => "XECDH";
        public string Mode => "keyVer";
        public static Curve[] VALID_CURVES = EnumHelpers.GetEnums<Curve>().Except(new [] { Curve.None }).ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateCurves(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateCurves(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(
                ValidateArray(
                    parameters.Curve,
                    VALID_CURVES,
                    "Curves"
                )
            );
        }
    }
}
