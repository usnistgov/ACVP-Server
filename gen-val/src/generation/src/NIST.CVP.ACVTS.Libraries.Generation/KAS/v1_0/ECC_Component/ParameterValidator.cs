using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC_Component
{
    public class ParameterValidator : Core.ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public string Algorithm => "KAS-ECC";
        public string Mode => "CDH-Component";

        public static string[] ValidFunctions => new string[]
        {
            "dpGen",
            "dpVal",
            "keyPairGen",
            "fullVal",
            "partialVal",
            "keyRegen"
        };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateAlgorithm(parameters, errorResults);
            ValidateFunctions(parameters, errorResults);
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

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Function == null || parameters.Function.Length == 0)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(
                ValidateArray(parameters.Function, ValidFunctions, "Functions")
            );
        }

        private void ValidateCurves(Parameters parameters, List<string> errorResults)
        {
            var validValues = EnumHelpers.GetEnumDescriptions<Curve>();
            errorResults.AddIfNotNullOrEmpty(
                ValidateArray(
                    parameters.Curve,
                    validValues.ToArray(),
                    "Curves"
                )
            );
        }
    }
}
