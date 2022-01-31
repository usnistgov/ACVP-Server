using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_CTS.v1_0
{


    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int[] ValidKeySizes = { 128, 192, 256 };
        public static string[] ValidDirections = { "encrypt", "decrypt" };
        public static int ValidMinPayload = 128;
        public static int ValidMaxPayload = 65536;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errorResults = new List<string>();
            ValidateKeySizes(parameters, errorResults);
            ValidateDirection(parameters, errorResults);
            ValidateMessageLength(parameters, errorResults);

            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(errorResults);
            }

            return new ParameterValidateResponse();
        }

        private void ValidateKeySizes(Parameters parameters, List<string> errorResults)
        {
            var result = ValidateArray(parameters.KeyLen, ValidKeySizes, "Key Sizes");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateDirection(Parameters parameters, List<string> errorResults)
        {
            string result = ValidateArray(parameters.Direction, ValidDirections, "Direction");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateMessageLength(Parameters parameters, List<string> errorResults)
        {
            var segmentCheck = ValidateSegmentCountGreaterThanZero(parameters.PayloadLen, "PayloadLen Domain");
            errorResults.AddIfNotNullOrEmpty(segmentCheck);
            if (!string.IsNullOrEmpty(segmentCheck))
            {
                return;
            }

            var fullDomain = parameters.PayloadLen.GetDomainMinMax();
            var rangeCheck = ValidateRange(
                new long[] { fullDomain.Minimum, fullDomain.Maximum },
                ValidMinPayload,
                ValidMaxPayload,
                "PayloadLen Range"
            );
            errorResults.AddIfNotNullOrEmpty(rangeCheck);

            //var modCheck = ValidateMultipleOf(parameters.PayloadLen, 8, "PayloadLen Modulus");
            //errorResults.AddIfNotNullOrEmpty(modCheck);

            // In order to "steal cipher text" the domain must not contain ONLY block sized amounts
            if (parameters.PayloadLen.DomainSegments.All(a => a.RangeMinMax.Increment % 128 == 0) &&
                parameters.PayloadLen.GetDomainMinMax().Minimum % 128 == 0)
            {
                errorResults.Add("PayloadLen domain must contain values that are not the AES block size.");
            }

            parameters.PayloadLen.SetRangeOptions(RangeDomainSegmentOptions.Random);
        }
    }
}
