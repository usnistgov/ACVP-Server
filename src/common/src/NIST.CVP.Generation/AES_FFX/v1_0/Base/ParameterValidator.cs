using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    

    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static int VALID_MIN_PT = 2;
        public static int VALID_MIN_RADIX = 2;
        public static int VALID_MAX_RADIX = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateCapabilities(parameters, errorResults);
            
            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateKeySizes(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateDirection(Parameters parameters,  List<string> errorResults)
        {
            string result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
        
        private void ValidateCapabilities(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Capabilities == null || parameters.Capabilities?.Length == 0)
            {
                errorResults.Add("Must provide at least one set of Capabilities.");
                return;
            }

            foreach (var capability in parameters.Capabilities)
            {
                if (!FfxRadixHelper.IsRadixValidWithPayload(capability.Radix, capability.MinLength,
                    capability.MaxLength))
                {
                    errorResults.Add($"Provided {nameof(capability)} of {nameof(capability.Radix)} ({capability.Radix}), {nameof(capability.MinLength)} ({capability.MinLength}), and {nameof(capability.MaxLength)} ({capability.MaxLength}) is invalid.");
                }
            }
        }
    }
}
