using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.Fips186_5.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_CURVES = { "P-224", "P-256", "P-384", "P-521", "B-233", "B-283", "B-409", "B-571", "K-233", "K-283", "K-409", "K-571" };
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
