using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static string[] VALID_KDF_MODES = EnumHelpers.GetEnumDescriptions<KdfModes>().ToArray();
        public static string[] VALID_MAC_MODES = EnumHelpers.GetEnumDescriptions<MacModes>().ToArray();
        public static string[] VALID_DATA_ORDERS = EnumHelpers.GetEnumDescriptions<CounterLocations>().ToArray();
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

                result = ValidateArray(capability.CounterLength, VALID_COUNTER_LENGTHS, "Counter Lengths");
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

                if (capability.CounterLength.Contains(0))
                {
                    errors.Add("Counter KDF requires a non-zero counter length");
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

                if (!(capability.FixedDataOrder.Contains("none") && capability.CounterLength.Contains(0)))
                {
                    errors.Add("none FixedDataOrder must be paired with 0 counter length");
                }
            }
        }
    }
}
