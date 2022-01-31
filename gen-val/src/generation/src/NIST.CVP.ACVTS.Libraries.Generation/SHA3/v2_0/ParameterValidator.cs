using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_MODES = { "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512" };
        public static int VALID_MIN_MESSAGE_LENGTH = 0;
        public static int VALID_MAX_MESSAGE_LENGTH = 65536;
        public static int[] VALID_LARGE_DATA_SIZES = { 1, 2, 4, 8 };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();

            var result = ValidateValue(parameters.Algorithm, VALID_MODES, "Algorithm name");
            errorResults.AddIfNotNullOrEmpty(result);

            ValidateDomain(parameters.MessageLength, errorResults, "Message Length", VALID_MIN_MESSAGE_LENGTH, VALID_MAX_MESSAGE_LENGTH);

            if (parameters.PerformLargeDataTest.Any())
            {
                result = ValidateArray(parameters.PerformLargeDataTest, VALID_LARGE_DATA_SIZES, "Large data size");
                errorResults.AddIfNotNullOrEmpty(result);
            }

            // MCT assumption
            var digestSize = ShaAttributes.GetHashFunctionFromName(parameters.Algorithm).OutputLen;
            var mctLength = digestSize * 3;
            if (!parameters.MessageLength.IsWithinDomain(digestSize) || !parameters.MessageLength.IsWithinDomain(mctLength))
            {
                errorResults.Add("Message length must contain the digest size and 3x the digest size for MCT");
            }

            return new ParameterValidateResponse(errorResults);
        }
    }
}
