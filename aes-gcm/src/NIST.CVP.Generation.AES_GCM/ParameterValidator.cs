using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class ParameterValidator : IParameterValidator
    {
        private int[] VALID_KEY_SIZES = new[] { 128, 192, 256 };
        private string[] VALID_DIRECTIONS = new[] { "encrypt", "decrypt" };
        public ParameterValidateResponse Validate(Parameters parameters)
        {

            var errorResults = new List<string>();
            var result = ValidateArray(parameters.KeyLen, VALID_KEY_SIZES, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateArray(parameters.Mode, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            //tag lengths -- array check

            //pt lengths -- must be in range, multiples of 128

            //non mult pt lengths -- must be in range, non multiples of 128

            //aad lengths -- must be in range, multiples of 128

            //non mult aad lengths -- must be in range, non multiples of 128

            //must iv length selected -- either 96 or other

            //iv lengths must be multiples of 8

            //internal iv and affirmation check

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        //@@@make generic, move to base class?
        private string ValidateArray(int[] supplied, int[] valid, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }
            var intersection = supplied.Intersect(valid);
            if (intersection.Count() != supplied.Length)
            {
                var invalid = supplied.Except(valid);
                return $"Invalid Key Sizes supplied: {string.Join(",", invalid)}";
            }
            return null;
        }

        private string ValidateArray(string[] supplied, string[] valid, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }
            if (supplied.Contains(null))
            {
                return $"{friendlyName} Contains null value.";
            }

            var intersection = supplied.Select(v => v.ToLower()).Intersect(valid);
            if (intersection.Count() != supplied.Length)
            {
                var invalid = supplied.Except(valid);
                return $"Invalid Key Sizes supplied: {string.Join(",", invalid)}";
            }
            return null;
        }
    }
}
