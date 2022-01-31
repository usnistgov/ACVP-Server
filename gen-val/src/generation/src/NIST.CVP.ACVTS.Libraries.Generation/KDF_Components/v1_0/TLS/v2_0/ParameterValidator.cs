using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v2_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_HASH_ALGS = { "SHA2-256", "SHA2-384", "SHA2-512" };

        public static TlsModes[] VALID_TLS_VERSIONS = EnumHelpers.GetEnums<TlsModes>()
            .Except(new[] { TlsModes.v12_extendedMasterSecret }).ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            ValidateAlgoMode(parameters, new[] { AlgoMode.Tls_v1_2_RFC7627 }, errors);
            errors.AddIfNotNullOrEmpty(ValidateArray(parameters.HashAlg, VALID_HASH_ALGS, "Hash Algs"));

            parameters.TlsVersion = new[] { TlsModes.v12_extendedMasterSecret };

            return new ParameterValidateResponse(errors);
        }
    }
}
