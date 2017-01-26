using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public abstract class ParameterValidatorBase
    {
        protected string ValidateArray(int[] supplied, int[] valid, string friendlyName)
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

        protected string ValidateArray(string[] supplied, string[] valid, string friendlyName)
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

        protected string ValidateValue(string supplied, string[] validValues, string friendlyName)
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

        protected string ValidateRange(int[] supplied, int minInclusive, int maxInclusive, string friendlyName)
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

        protected string ValidateMultipleOf(int[] supplied, int multiple, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }

            int[] invalid = supplied.Where(w => w % multiple != 0).ToArray();

            if (invalid.Count() != 0)
            {
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}.  Values were not a multiple of {multiple}";
            }

            return null;
        }

        protected string ValidateBoolean(bool? supplied, string friendlyName)
        {
            if(supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            return null;
        }
    }
}
