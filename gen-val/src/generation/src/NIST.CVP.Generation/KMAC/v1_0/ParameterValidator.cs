using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public const int _MIN_KEY_LENGTH = 128;
        public const int _MAX_KEY_LENGTH = 524288;
        public static string[] VALID_ALGORITHMS = { "KMAC-128", "KMAC-256" };
        public static int[] VALID_DIGEST_SIZES = { 128, 256 };

        private int _minMacLength = 32;
        private int _maxMacLength = 65536;

        private int _minMsgLength = 0;
        private int _maxMsgLength = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorResults = new List<string>();

            // Implementing "default values" to match registration expectations
            if (parameters.DigestSizes == null)
            {
                parameters.DigestSizes = new List<int>();
            }
            if (parameters.DigestSizes.Count == 0)
            {
                var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
                switch (algoMode)
                {
                    case AlgoMode.KMAC_128_v1_0:
                        parameters.DigestSizes.Add(128);
                        break;
                    case AlgoMode.KMAC_256_v1_0:
                        parameters.DigestSizes.Add(256);
                        break;
                    
                    default:
                        errorResults.Add("Invalid AlgoMode");
                        break;
                }
            }
            
            ValidateFunctions(parameters, errorResults);

            ValidateMsgLen(parameters, errorResults);
            ValidateKeyLen(parameters, errorResults);
            ValidateMacLen(parameters, errorResults);

            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm.ToLower(), VALID_ALGORITHMS, "KMAC Function");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            result = ValidateArray(parameters.DigestSizes, VALID_DIGEST_SIZES, "Digest Size");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            if ((parameters.XOF.Length != 1 && parameters.XOF.Length != 2) || parameters.XOF.ToHashSet().Count != parameters.XOF.Length)
            {
                errorResults.Add("XOF must contain only a single true, a single false, or both");
            }
        }

        private void ValidateKeyLen(Parameters parameters, List<string> errorResults)
        {
            if (parameters.KeyLen == null)
            {
                errorResults.AddIfNotNullOrEmpty($"{nameof(parameters.KeyLen)} must be provided");
                return;
            }

            string segmentCheck = "";
            if (parameters.KeyLen.DomainSegments.Count() != 1)
            {
                // TODO why?
                segmentCheck = "Must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.KeyLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _MIN_KEY_LENGTH,
                _MAX_KEY_LENGTH,
                "KeyLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.KeyLen.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.KeyLen, bitOriented, "KeyLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMacLen(Parameters parameters, List<string> errorResults)
        {
            if (parameters.MacLen == null)
            {
                errorResults.AddIfNotNullOrEmpty($"{nameof(parameters.MacLen)} must be provided");
                return;
            }

            string segmentCheck = "";
            if (parameters.MacLen.DomainSegments.Count() != 1)
            {
                // TODO why?
                segmentCheck = "Must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MacLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _minMacLength,
                _maxMacLength,
                "MacLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.MacLen.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.MacLen, bitOriented, "MsgLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }

        private void ValidateMsgLen(Parameters parameters, List<string> errorResults)
        {
            if (parameters.MsgLen == null)
            {
                errorResults.AddIfNotNullOrEmpty($"{nameof(parameters.MsgLen)} must be provided");
                return;
            }

            string segmentCheck = "";
            if (parameters.MsgLen.DomainSegments.Count() != 1)
            {
                // TODO why?
                segmentCheck = "Must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MsgLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _minMsgLength,
                _maxMsgLength,
                "MsgLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.MsgLen.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.MsgLen, bitOriented, "MsgLen Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
