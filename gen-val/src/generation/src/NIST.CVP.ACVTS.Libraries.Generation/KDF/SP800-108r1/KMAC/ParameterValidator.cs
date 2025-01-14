using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static MacModes[] VALID_MAC_MODES = { MacModes.KMAC_128, MacModes.KMAC_256 };
        public static readonly int MIN_KEY_DERIVATION_KEY_LENGTH = 112;
        public static readonly int MAX_KEY_DERIVATION_KEY_LENGTH = 4096;
        public static readonly int MIN_OTHER_INFO_LENGTH = 8;
        public static readonly int MAX_OTHER_INFO_LENGTH = 4096;
        
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (errors.AddIfNotNullOrEmpty(ValidateArrayAtLeastOneItem(parameters.MacMode, "MAC Mode")))
            {
                return new ParameterValidateResponse(errors);
            }
            
            string result = ValidateArray(parameters.MacMode, VALID_MAC_MODES, "MAC Modes");
            errors.AddIfNotNullOrEmpty(result);

            ValidateDomain(parameters.KeyDerivationKeyLength, errors, "Key Derivation Key Length", MIN_KEY_DERIVATION_KEY_LENGTH, MAX_KEY_DERIVATION_KEY_LENGTH);
            ValidateMultipleOf(parameters.KeyDerivationKeyLength, errors, 8, "Key Derivation Key Length");
            
            ValidateDomain(parameters.ContextLength, errors, "Context Length", MIN_OTHER_INFO_LENGTH, MAX_OTHER_INFO_LENGTH);
            ValidateMultipleOf(parameters.ContextLength, errors, 8, "Context Length");
            
            ValidateDomain(parameters.DerivedKeyLength, errors, "Derived Key Length", MIN_KEY_DERIVATION_KEY_LENGTH, MAX_KEY_DERIVATION_KEY_LENGTH);
            ValidateMultipleOf(parameters.DerivedKeyLength, errors, 8, "Derived Key Length");
            
            if (parameters.LabelLength != null)
            {
                ValidateDomain(parameters.LabelLength, errors, "Label Length", 0, MAX_OTHER_INFO_LENGTH);
                ValidateMultipleOf(parameters.LabelLength, errors, 8, "Label Length");
            }

            return new ParameterValidateResponse(errors);
        }
    }
}
