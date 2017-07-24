using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class ParameterValidatorTdes : ParameterValidatorBase, IParameterValidator<ParametersTdes>
    {
        // @@@ better way to do this without having to redefine valid values in tests?
        public static string[] VALID_DIRECTIONS = { "encrypt", "decrypt" };
        public static int[] VALID_KEYING_OPTIONS = { 1, 2 };

        public ParameterValidateResponse Validate(ParametersTdes parameters)
        {
            var errorResults = new List<string>();

            ValidateDirection(parameters, errorResults);
            ValidateKeyingOption(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateDirection(ParametersTdes parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.Direction, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateKeyingOption(ParametersTdes parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.KeyingOption, VALID_KEYING_OPTIONS, "Keying Options");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }
    }
}
