using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SNMP
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public static int SNMP_MINIMUM_LENGTH = 9 * 8;
        public static int SNMP_MAXIMUM_LENGTH = 32 * 8;
        public static int PASSWORD_MINIMUM_LENGTH = 8 * 8;
        public static int PASSWORD_MAXIMUM_LENGTH = 1024 * 8;
        public static int PASSWORD_BYTE_BOUNDARY = 8;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!parameters.Algorithm.Equals("kdf-components", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect algorithm");
            }

            if (!parameters.Mode.Equals("snmp", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Incorrect mode");
            }

            if (parameters.EngineId == null)
            {
                errors.Add("Must contain engineId");
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            if (parameters.EngineId.Length != 1 && parameters.EngineId.Length != 2)
            {
                errors.Add("Must only have one or two engine IDs");
            }

            foreach (var engineId in parameters.EngineId)
            {
                if (engineId == null)
                {
                    errors.Add("engineId cannot be null");
                    continue;
                }

                if (engineId.Length * 4 < SNMP_MINIMUM_LENGTH || engineId.Length * 4 > SNMP_MAXIMUM_LENGTH)
                {
                    errors.Add("Engine ID must be between 9 bytes and 32 bytes");
                }

                errors.AddIfNotNullOrEmpty(ValidateHex(engineId, "engineId"));
            }

            if (parameters.EngineId.Distinct().Count() != parameters.EngineId.Length)
            {
                errors.Add("Engine IDs must be unique");
            }

            if (parameters.PasswordLength.GetDomainMinMax().Minimum < PASSWORD_MINIMUM_LENGTH)
            {
                errors.Add("Password length must be at least 8 bytes");
            }

            if (parameters.PasswordLength.GetDomainMinMax().Maximum > PASSWORD_MAXIMUM_LENGTH)
            {
                errors.Add("Password length must be at most 1024 bytes");
            }
            
            if (errors.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errors));
            }

            return new ParameterValidateResponse();
        }
    }
}

