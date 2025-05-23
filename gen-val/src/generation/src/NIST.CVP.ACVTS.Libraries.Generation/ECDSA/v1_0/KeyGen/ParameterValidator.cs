﻿using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = EnumHelpers.GetEnumDescriptions<Curve>().Except(new[] { "p-192", "b-163", "k-163" }, StringComparer.OrdinalIgnoreCase).ToArray();
        public static string[] VALID_SECRET_GENERATION_MODES = EnumHelpers.GetEnumDescriptions<SecretGenerationMode>().ToArray();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();
            var result = "";

            result = ValidateArray(parameters.Curve, VALID_CURVES, "Curves");
            errors.AddIfNotNullOrEmpty(result);

            result = ValidateArray(parameters.SecretGenerationMode, VALID_SECRET_GENERATION_MODES, "Secret Generation Modes");
            errors.AddIfNotNullOrEmpty(result);

            return new ParameterValidateResponse(errors);
        }
    }
}
