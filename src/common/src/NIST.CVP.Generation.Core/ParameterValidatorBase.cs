using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Math.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.Core
{
    public abstract class ParameterValidatorBase
    {
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

            var intersection = supplied.Select(v => v.ToLower()).Intersect(valid, StringComparer.OrdinalIgnoreCase);
            if (intersection.Count() != supplied.Length)
            {
                var invalid = supplied.Except(valid);
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}, intersect : {intersection.Count()}, supplied: {supplied.Length}";
            }
            return null;
        }

        protected string ValidateArray<T>(IEnumerable<T> supplied, IEnumerable<T> valid, string friendlyName)
        {
            if (supplied == null || !supplied.Any())
            {
                return $"No {friendlyName} supplied.";
            }

            var intersection = supplied.Intersect(valid);
            if (intersection.Count() != supplied.Count())
            {
                var invalid = supplied.Except(valid);
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}, intersect : {intersection.Count()}, supplied: {supplied.Count()}";
            }
            return null;
        }


        protected string ValidateValue(string supplied, string[] validValues, string friendlyName)
        {
            if (string.IsNullOrEmpty(supplied))
            {
                return $"No {friendlyName} supplied.";
            }

            if (!validValues.Contains(supplied, StringComparer.OrdinalIgnoreCase))
            {
                return $"Invalid {friendlyName} supplied: {supplied}";
            }

            return null;
        }

        protected string ValidateValue(int supplied, int[] validValues, string friendlyName)
        {
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

        protected string ValidateRange(long[] supplied, long minInclusive, long maxInclusive, string friendlyName)
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

        protected string ValidateSegmentCountGreaterThanZero(MathDomain supplied, string friendlyName)
        {
            if (supplied == null)
            {
                return $"{friendlyName} was not provided.";
            }

            if (!supplied.DomainSegments.Any())
            {
                return $"Invalid {friendlyName} supplied.  Domain must contain literals, or ranges of values.";
            }

            return null;
        }

        protected string ValidateMultipleOf(MathDomain supplied, int multiple, string friendlyName)
        {
            if (supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            List<string> invalid = new List<string>();

            foreach (var segment in supplied.GetMinMaxRanges())
            {
                var segmentCheck = ValidateMultipleOf(segment, multiple, "Domain Segment");
                invalid.AddIfNotNullOrEmpty(segmentCheck);
            }

            if (invalid.Count() != 0)
            {
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}.  {friendlyName} contained values that were not a multiple of {multiple}";
            }

            return null;
        }

        protected string ValidateMultipleOf(RangeMinMax supplied, int multiple, string friendlyName)
        {
            if (supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            List<string> invalid = new List<string>();

            if (supplied.Minimum == supplied.Maximum)
            {
                if (supplied.Minimum % multiple != 0)
                {
                    invalid.Add($"{supplied.Minimum}");
                }
            }
            else
            {
                if (supplied.Minimum % multiple != 0)
                {
                    invalid.Add($"{supplied.Minimum}");
                }
                if (supplied.Maximum % multiple != 0)
                {
                    invalid.Add($"{supplied.Maximum}");
                }
                if (supplied.Increment % multiple != 0)
                {
                    invalid.Add($"{supplied.Increment}");
                }
            }

            if (invalid.Count != 0)
            {
                return $"Invalid {friendlyName} supplied.  Domain Segment {supplied} contained values ({string.Join(",", invalid)}) that were not a multiple of {multiple}";
            }

            return null;
        }

        protected string ValidateMultipleOf(long[] supplied, long multiple, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }

            long[] invalid = supplied.Where(w => w % multiple != 0).ToArray();

            if (invalid.Count() != 0)
            {
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}.  Values were not a multiple of {multiple}";
            }

            return null;
        }

        protected string ValidateBoolean(bool? supplied, string friendlyName)
        {
            if (supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            return null;
        }

        protected string ValidateCollectionCountNoMoreThan<T>(IEnumerable<T> supplied, int maximumCount, string friendlyName)
        {
            if (supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            if (supplied.Count() > maximumCount)
            {
                return $"Invalid {friendlyName} supplied. Supplied collection must contain no more than {maximumCount} elements.";
            }

            return null;
        }

        protected string ValidateCollectionCountExactly<T>(IEnumerable<T> supplied, int expectedCount, string friendlyName)
        {
            if (supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            if (supplied.Count() != expectedCount)
            {
                return $"Invalid {friendlyName} supplied. Supplied collection must contain exactly {expectedCount} elements.";
            }

            return null;
        }

        protected string ValidateHex(string supplied, string friendlyName)
        {
            if (supplied == null)
            {
                return $"No {friendlyName} supplied.";
            }

            if (supplied.Length % 2 != 0)
            {
                return $"Invalid length for hex";
            }

            // Count all the values 0-9, a-f, and compare to original length. If string is hex, they will be equal
            if (supplied.ToLower().Count(c => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f')) != supplied.Length)
            {
                return $"Invalid {friendlyName} supplied. Supplied string is not hex.";
            }

            return null;
        }
    }
}
