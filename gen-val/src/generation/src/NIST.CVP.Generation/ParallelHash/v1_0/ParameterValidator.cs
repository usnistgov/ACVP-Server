using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.ParallelHash.v1_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_ALGORITHMS = { "PARALLELHASH-128", "PARALLELHASH-256" };
        public static int[] VALID_DIGEST_SIZES = { 128, 256 };

        public static int VALID_MIN_OUTPUT_SIZE = 16;
        public static int VALID_MAX_OUTPUT_SIZE = 65536;

        private int _minMsgLength = 0;
        private int _maxMsgLength = 65536;

        public static int VALID_MIN_BLOCK_SIZE = 1;
        public static int VALID_MAX_BLOCK_SIZE = 128;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

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
                    case AlgoMode.ParallelHash_128_v1_0:
                        parameters.DigestSizes.Add(128);
                        break;
                    case AlgoMode.ParallelHash_256_v1_0:
                        parameters.DigestSizes.Add(256);
                        break;
                    
                    default:
                        errorResults.Add("Invalid AlgoMode");
                        break;
                }
            }
            
            ValidateFunctions(parameters, errorResults);
            ValidateOutputLength(parameters, errorResults);
            ValidateMessageLength(parameters, errorResults);
            ValidateBlockSize(parameters, errorResults);
            
            return new ParameterValidateResponse(errorResults);
        }

        private void ValidateFunctions(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateValue(parameters.Algorithm.ToLower(), VALID_ALGORITHMS, "ParallelHash Function");
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

        private void ValidateOutputLength(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.OutputLength, "OutputLength Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }
            
            if (parameters.OutputLength.DomainSegments.Count() != 1)
            {
                segmentCheck = "OutputLength must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.OutputLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_OUTPUT_SIZE,
                VALID_MAX_OUTPUT_SIZE,
                "OutputLength Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.OutputLength.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.OutputLength, bitOriented, "OutputLength Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
        
        private void ValidateBlockSize(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.BlockSize, "BlockSize Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }
            
            if (parameters.BlockSize.DomainSegments.Count() != 1)
            {
                segmentCheck = "BlockSize must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.BlockSize.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                VALID_MIN_BLOCK_SIZE,
                VALID_MAX_BLOCK_SIZE,
                "BlockSize Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);
        }

        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.MessageLength, "MessageLength Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }
            
            if (parameters.MessageLength.DomainSegments.Count() != 1)
            {
                segmentCheck = "MessageLength must have exactly one segment in the domain";
            }
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.MessageLength.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                _minMsgLength,
                _maxMsgLength,
                "MessageLength Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            // Links BitOriented and Domain
            var bitOriented = parameters.MessageLength.DomainSegments.ElementAt(0).RangeMinMax.Increment;
            if (bitOriented == 0)
            {
                bitOriented = 1;
            }
            var modCheck = ValidateMultipleOf(parameters.MessageLength, bitOriented, "MessageLength Modulus");
            errorResults.AddIfNotNullOrEmpty(modCheck);
        }
    }
}
