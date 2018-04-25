using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TLS
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = {"sha2-256", "sha2-384", "sha2-512"};
        public static string[] VALID_TLS_VERSIONS = EnumHelpers.GetEnumDescriptions<TlsModes>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("tls", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }
            
            string result;
            result = ValidateArray(parameters.TlsVersion, VALID_TLS_VERSIONS, "TLS Version");
            errors.AddIfNotNullOrEmpty(result);

            if (parameters.TlsVersion.Contains(EnumHelpers.GetEnumDescriptionFromEnum(TlsModes.v12)))
            {
                result = ValidateArray(parameters.HashAlg, VALID_HASH_ALGS, "Hash Algs");
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
