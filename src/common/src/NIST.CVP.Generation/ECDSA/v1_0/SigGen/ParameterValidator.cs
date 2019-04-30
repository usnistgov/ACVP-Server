using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256", "sha3-224", "sha3-256", "sha3-384", "sha3-512" };
        public static string[] VALID_CURVES = { "p-224", "p-256", "p-384", "p-521", "b-233", "b-283", "b-409", "b-571", "k-233", "k-283", "k-409", "k-571" };

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

            return new ParameterValidateResponse(errors);
        }
    }
}
