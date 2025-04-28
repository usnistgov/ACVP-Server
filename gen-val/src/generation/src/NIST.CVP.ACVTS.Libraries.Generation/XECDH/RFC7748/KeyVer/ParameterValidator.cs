using System.Collections.Generic;
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
        public static string[] VALID_CURVES = EnumHelpers.GetEnumDescriptions<Curve>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateAlgorithm(parameters, errorResults);
            ValidateCurves(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(
                ValidateValue(
                    parameters.Algorithm,
                    new string[] { Algorithm },
                    nameof(Algorithm)
                )
            );
            errorResults.AddIfNotNullOrEmpty(
                ValidateValue(
                    parameters.Mode,
                    new string[] { Mode },
                    nameof(Mode)
                )
            );
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
