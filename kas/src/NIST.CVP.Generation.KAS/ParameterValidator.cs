using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public const string ALGORITHM = "KAS-FFC";

        public readonly string[] VALID_FUNCTIONS = new string[]
        {
            "dpGen",
            "dpVal",
            "keyPairGen",
            "fullVal",
            "keyRegen"
        };

        public readonly string[] VALID_SCHEMES = new string[] { "dhEphem" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            // Validate Algorithm
            ValidateAlgorithm(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            // Validate at least one "function"
            ValidateFunction(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            // Validate Schemes
            ValidateSchemes(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateAlgorithm(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateValue(parameters.Algorithm, new string[] {ALGORITHM}, "Algorithm"));
        }

        private void ValidateFunction(Parameters parameters, List<string> errorResults)
        {
            errorResults.AddIfNotNullOrEmpty(ValidateArray(parameters.Function, VALID_FUNCTIONS, "Functions"));
        }

        private void ValidateSchemes(Parameters parameters, List<string> errorResults)
        {
            ValidateDhEphemScheme(parameters.Scheme.DhEphem, errorResults);
        }

        #region scheme validation
        private void ValidateDhEphemScheme(DhEphem scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            ValidateKeyAgreementRoles(scheme.Role, errorResults);

            if (scheme.KdfKc != null)
            {
                errorResults.Add("Key Confirmation not possible with dhEphem.");
                return;
            }

            
        }
        #endregion scheme validation

        private void ValidateKeyAgreementRoles(string[] schemeRoles, List<string> errorResults)
        {
            throw new NotImplementedException();
        }

    }
}
