using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using System.Numerics;

namespace NIST.CVP.Generation.AES_GCM
{
    public class ParameterValidator : IParameterValidator
    {
        // @@@ better way to do this without having to redefine valid values in tests?
   
        public static int[] VALID_KEY_SIZES = new int[] { 128, 192, 256 };
        public static int[] VALID_TAG_LENGTHS = new int[] { 32, 64, 96, 104, 112, 120, 128 };
        public static string[] VALID_DIRECTIONS = new string[] { "encrypt", "decrypt" };
        public static string[] VALID_IV_GEN = new string[] { "internal", "external" };
        public static string[] VALID_IV_GEN_MODE = new string[] { "8.2.1", "8.2.2" };
        public static int VALID_MIN_PT = 0;
        public static int VALID_MAX_PT = 65536;
        public static int VALID_MIN_AAD = 0;
        public static int VALID_MAX_AAD = 65536;
        public static int VALID_MIN_IV = 8;
        public static int VALID_MAX_IV = 1024;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateTagSizes(parameters, errorResults);
            ValidatePlainText(parameters, errorResults);
            ValidateAAD(parameters, errorResults);
            ValidateIV(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            return new ParameterValidateResponse();
        }

        private void ValidatePlainText(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.PtLen, VALID_MIN_PT, VALID_MAX_PT, "Plaintext length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateMultipleOf(parameters.PtLen, true, 8, "Plaintext length (multiples check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
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
            string result = ValidateArray(parameters.Mode, VALID_DIRECTIONS, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateTagSizes(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateArray(parameters.TagLen, VALID_TAG_LENGTHS, "Tag Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateAAD(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.aadLen, VALID_MIN_AAD, VALID_MAX_AAD, "AAD length (range check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateMultipleOf(parameters.aadLen, true, 8, "AAD length (multiples check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateIV(Parameters parameters,  List<string> errorResults)
        {
            var result = ValidateRange(parameters.ivLen, VALID_MIN_IV, VALID_MAX_IV, "IV Length (range)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
            result = ValidateMultipleOf(parameters.ivLen, true, 8, "IV length (multiples check)");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateValue(parameters.ivGen, VALID_IV_GEN, "IV Generation");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            if (parameters.ivGen.ToLower() == "internal")
            {
                result = ValidateValue(parameters.ivGenMode, VALID_IV_GEN_MODE, "IV Generation Mode (Internal)");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
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
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}";
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
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}";
            }
            return null;
        }

        private string ValidateValue(string supplied, string[] validValues, string friendlyName)
        {
            if (string.IsNullOrEmpty(supplied))
            {
                return $"No {friendlyName} supplied.";
            }

            if (!validValues.Contains(supplied))
            {
                return $"Invalid {friendlyName} supplied: {supplied}";
            }

            return null;
        }

        private string ValidateRange(int[] supplied, int minInclusive, int maxInclusive, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }

            var invalid = supplied.Where(w => w < minInclusive || w > maxInclusive);
            if (invalid.Count() != 0)
            {
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}.  Values were not between {minInclusive} and {maxInclusive}";
            }

            return null;
        }

        private string ValidateMultipleOf(int[] supplied, bool checkIsMultipleOf, int multiple, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }

            int[] invalid;
            if (checkIsMultipleOf)
            {
                invalid = supplied.Where(w => w % multiple != 0).ToArray();
            }
            else
            {
                invalid = supplied.Where(w => w % multiple == 0).ToArray();
            }

            if (invalid.Count() != 0)
            {
                string invalidMessagePart = checkIsMultipleOf ? "were" : "were not";
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}.  Values {invalidMessagePart} a multiple of {multiple}";
            }

            return null;
        }
    }
}
