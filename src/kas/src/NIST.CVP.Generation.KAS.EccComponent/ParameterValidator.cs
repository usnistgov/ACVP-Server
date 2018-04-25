using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class ParameterValidator : Core.ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public string Algorithm => "KAS";
        public string Mode => "EccComponent";

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateAlgorithm(parameters, errorResults);
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

        private void ValidateCurves(Parameters parameters, List<string> errorResults)
        {
            var validValues = EnumHelpers.GetEnumDescriptions<Curve>();
            errorResults.AddIfNotNullOrEmpty(
                ValidateArray(
                    parameters.Curves, 
                    validValues.ToArray(), 
                    "Curves"
                )
            );
        }
    }
}