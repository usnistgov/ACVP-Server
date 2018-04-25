using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "sha-1", "sha2-224", "sha2-256", "sha2-384", "sha2-512", "sha2-512/224", "sha2-512/256" };
        public static string[] VALID_CURVES = EnumHelpers.GetEnumDescriptions<Curve>().ToArray();

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

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}
