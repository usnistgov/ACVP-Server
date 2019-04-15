using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
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

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
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
            if (ValidFunctions == null || ValidFunctions.Length == 0)
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