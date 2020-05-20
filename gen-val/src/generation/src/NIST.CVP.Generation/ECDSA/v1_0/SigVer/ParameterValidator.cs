using System;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256", "sha3-224", "sha3-256", "sha3-384", "sha3-512" };
        public static string[] VALID_CURVES = EnumHelpers.GetEnumDescriptions<Curve>().ToArray();
        public static string[] VALID_CONFORMANCES = { "SP800-106" };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            if (errors.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.Capabilities, "Capabilities")))
            {
                return new ParameterValidateResponse(errors);
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
                
                // Not a valid combination
                if (parameters.Conformances.Contains("SP800-106", StringComparer.OrdinalIgnoreCase) &&
                    parameters.Component)
                {
                    errors.Add("Cannot combine SP800-106 conformance with pre-hash component testing");
                }
            }
        }
    }
}
