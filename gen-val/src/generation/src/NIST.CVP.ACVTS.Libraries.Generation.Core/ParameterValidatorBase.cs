using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public abstract class ParameterValidatorBase
    {
        /// <summary>
        /// Get the <see cref="AlgoMode"/> from <see cref="IParameters"/>.
        /// </summary>
        /// <param name="parameters">The <see cref="IParameters"/> from which to determine the <see cref="AlgoMode"/></param>
        /// <returns>The <see cref="AlgoMode"/></returns>
        protected AlgoMode GetAlgoModeRevisionFromParameters(IParameters parameters)
        {
            return AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
        }

        /// <summary>
        /// Validates the <see cref="AlgoMode"/> derived from <see cref="allowedAlgoModes"/> exists within <see cref="allowedAlgoModes"/>.
        ///
        /// If the <see cref="AlgoMode"/> is not found, add to <see cref="errors"/>.
        /// </summary>
        /// <param name="parameters">The <see cref="IParameters"/> to parse for an <see cref="AlgoMode"/>.</param>
        /// <param name="allowedAlgoModes">The allowed <see cref="AlgoMode"/>s.</param>
        /// <param name="errors">The errors list that is added to in cases where the <see cref="AlgoMode"/> is not valid compared to the <see cref="allowedAlgoModes"/>.</param>
        /// <returns>If the provided <see cref="AlgoMode"/> is valid when compared to <see cref="allowedAlgoModes"/>.</returns>
        protected bool ValidateAlgoMode(IParameters parameters, IEnumerable<AlgoMode> allowedAlgoModes, List<string> errors)
        {
            AlgoMode algoMode;
            try
            {
                algoMode = GetAlgoModeRevisionFromParameters(parameters);
            }
            catch (Exception e)
            {
                errors.Add(e.Message);
                return false;
            }

            return ValidateAlgoMode(algoMode, allowedAlgoModes, errors);
        }

        /// <summary>
        /// Validates the <see cref="algoMode"/> exists within <see cref="allowedAlgoModes"/>.
        ///
        /// If the <see cref="AlgoMode"/> is not found, add to <see cref="errors"/>.
        /// </summary>
        /// <param name="algoMode">The <see cref="AlgoMode"/> to check.</param>
        /// <param name="allowedAlgoModes">The allowed <see cref="AlgoMode"/>s.</param>
        /// <param name="errors">The errors list that is added to in cases where the <see cref="AlgoMode"/> is not valid compared to the <see cref="allowedAlgoModes"/>.</param>
        /// <returns>If the provided <see cref="AlgoMode"/> is valid when compared to <see cref="allowedAlgoModes"/>.</returns>
        protected bool ValidateAlgoMode(AlgoMode algoMode, IEnumerable<AlgoMode> allowedAlgoModes, List<string> errors)
        {
            if (!allowedAlgoModes.Contains(algoMode))
            {
                errors.Add($"Provided {nameof(algoMode)} {algoMode} in invalid.  Valid {nameof(algoMode)}s include: {string.Join(", ", allowedAlgoModes)}");
                return false;
            }

            return true;
        }

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

            var intersection = supplied.Intersect(valid);
            if (intersection.Count() != supplied.Length)
            {
                var invalid = supplied.Except(valid);
                return $"Invalid {friendlyName} supplied: {string.Join(",", invalid)}, intersect : {intersection.Count()}, supplied: {supplied.Length}";
            }
            return null;
        }
        protected string ValidateBoolArray(bool[] supplied, string friendlyName)
        {
            string err = ValidateArray<bool>(supplied, new bool[] { true, false }, friendlyName);
            if (err != null)
            {
                return err;
            }
            if (supplied.Count() != supplied.Distinct().Count())
            {
                return $"Duplicate {friendlyName} supplied.";
            }
            return null;
        }

        protected string ValidateArrayAtLeastOneItem<T>(IEnumerable<T> supplied, string friendlyName)
        {
            if (supplied == null || !supplied.Any())
            {
                return $"No {friendlyName} supplied.";
            }

            return null;
        }

        protected string ValidateArrayHasNoDuplicates<T>(T[] supplied, string friendlyName)
        {
            if (supplied == null || supplied.Length == 0)
            {
                return $"No {friendlyName} supplied.";
            }

            if (supplied.Count() != supplied.Distinct().Count())
            {
                return $"Duplicate {friendlyName} supplied.";
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

            if (!validValues.Contains(supplied))
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

        /// <summary>
        /// Validate a domain - not null, contains at least one item, and is within the min/max.
        ///
        /// When an issue occurs, add the error message to <see cref="errors"/> and return false; otherwise true.
        /// </summary>
        /// <param name="domain">The domain to validate.</param>
        /// <param name="errors">The List of errors to add to in the event of a validation issue.</param>
        /// <param name="errorTag">The friend name of the domain being validated.</param>
        /// <param name="min">The minimum allowed value for the domain.</param>
        /// <param name="max">The maximum allowed value for the domain.</param>
        /// <returns></returns>
        protected bool ValidateDomain(MathDomain domain, List<string> errors, string errorTag, int min, int max)
        {
            var result = ValidateSegmentCountGreaterThanZero(domain, errorTag);
            if (!string.IsNullOrEmpty(result))
            {
                errors.Add(result);
                return false;
            }

            if (domain.GetDomainMinMax().Minimum < min)
            {
                errors.Add($"Minimum {errorTag} must be greater than or equal to {min}");
                return false;
            }

            if (domain.GetDomainMinMax().Maximum > max)
            {
                errors.Add($"Maximum {errorTag} must be less than or equal to {max}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates a domain is a multiple of the provided multiple.
        /// </summary>
        /// <param name="supplied">The math domain to check.</param>
        /// <param name="errors">The list of errors to add to in case of failing the check.</param>
        /// <param name="multiple">The multiple to check against.</param>
        /// <param name="friendlyName">The friendly label for the object being checked.</param>
        /// <returns>true if valid, false if not (adds to errors if not valid).</returns>
        protected bool ValidateMultipleOf(MathDomain supplied, List<string> errors, int multiple, string friendlyName)
        {
            var result = ValidateMultipleOf(supplied, multiple, friendlyName);

            if (string.IsNullOrEmpty(result))
            {
                return true;
            }

            errors.Add(result);
            return false;
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
