using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;

namespace NIST.CVP.Generation.ECDSA.Fips186_5.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256", "sha3-224", "sha3-256", "sha3-384", "sha3-512", "shake-128", "shake-256" };
        public static string[] VALID_CURVES = { "p-224", "p-256", "p-384", "p-521" };
        public static string[] VALID_CONFORMANCES = { "SP800-106" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            if (parameters.Capabilities.Length == 0)
            {
                errors.Add("No capabilities found");
            }

            foreach (var capability in parameters.Capabilities)
            {
                result = ValidateArray(capability.Curve, VALID_CURVES, "Curves");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateArray(capability.HashAlg, VALID_HASH_ALGS, "Hash Algs");
                errors.AddIfNotNullOrEmpty(result);
            }

            ValidateConformances(parameters, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateConformances(Parameters parameters, List<string> errors)
        {
            if (parameters.Conformances != null && parameters.Conformances.Length != 0)
            {
                var result = ValidateArray(parameters.Conformances, VALID_CONFORMANCES, "Conformances");
                if (!string.IsNullOrEmpty(result))
                {
                    errors.Add(result);
                }
            }
        }
    }
}