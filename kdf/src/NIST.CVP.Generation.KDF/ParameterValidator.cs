using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KDF
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        // TODO reduce repeated data, this already appears in the KdfModes enum (top 3 VALID variables here come from enums)
        public static string[] VALID_KDF_MODES = {"counter", "feedback", "double pipeline iteration"};
        public static string[] VALID_MAC_MODES =
        {
            "cmac-aes128", "cmac-aes192", "cmac-aes256", "cmac-tdes",
            "hmac-sha1", "hmac-sha224", "hmac-sha256","hmac-sha384", "hmac-sha512"
        };
        public static string[] VALID_DATA_ORDERS = {"none", "before fixed data", "middle fixed data", "after fixed data", "before iterator"};
        public static int[] VALID_COUNTER_LENGTHS = {0, 8, 16, 24, 32};
        public static int MAX_DATA_LENGTH = 4096;
        public static int MIN_DATA_LENGTH = 1;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            foreach (var capability in parameters.Capabilities)
            {
                string result;
                result = ValidateValue(capability.KdfMode, VALID_KDF_MODES, "KDF Mode");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateArray(capability.MacMode, VALID_MAC_MODES, "MAC Modes");
                errors.AddIfNotNullOrEmpty(result);

                result = ValidateArray(capability.CounterLength, VALID_COUNTER_LENGTHS, "Counter Lenghts");
                errors.AddIfNotNullOrEmpty(result);

                ValidateFixedDataOrder(capability, errors);

                if (!capability.SupportedLengths.DomainSegments.Any())
                {
                    errors.Add("No supported lengths provided");
                    continue;
                }

                if (capability.SupportedLengths.GetDomainMinMax().Minimum < MIN_DATA_LENGTH)
                {
                    errors.Add("Minimum output length must be greater than 0");
                }

                if (capability.SupportedLengths.GetDomainMinMax().Maximum > MAX_DATA_LENGTH)
                {
                    errors.Add("Maximum output length must be less than or equal to 4096");
                }
            }

            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }

        private void ValidateFixedDataOrder(Capability capability, List<string> errors)
        {
            var result = ValidateArray(capability.FixedDataOrder, VALID_DATA_ORDERS, "Data Orders");
            if (!string.IsNullOrEmpty(result))
            {
                errors.Add(result);
                return;
            }

            if (capability.KdfMode.Equals("counter", StringComparison.OrdinalIgnoreCase))
            {
                if (capability.FixedDataOrder.Contains("none"))
                {
                    errors.Add("none FixedDataOrder is not valid with Counter KDF");
                }

                if (capability.FixedDataOrder.Contains("before iterator"))
                {
                    errors.Add("before iterator FixedDataORder is not valid with Counter KDF");
                }
            }
            else
            {
                if (capability.FixedDataOrder.Contains("middle fixed data"))
                {
                    errors.Add("middle fixed data FixedDataOrder is not valid with non-Counter KDFs");
                }
            }
        }
    }
}
