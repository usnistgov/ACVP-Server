using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.Shared;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static AlgoMode[] VALID_ALGO_MODES = [AlgoMode.XMSS_SigGen_SP800_208, AlgoMode.XMSS_SigVer_SP800_208];
    public static XmssMode[] VALID_XMSS_TYPES = EnumHelpers.GetEnumsWithoutDefault<XmssMode>().ToArray();

    public static int MAX_MESSAGE_LENGTH = 65536;
    public static int MIN_MESSAGE_LENGTH = 8;   // Not 0 because some SigVer manipulations try to modify the message

    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        if (!ValidateAlgoMode(parameters, VALID_ALGO_MODES, errors))
        {
            return new ParameterValidateResponse(errors);
        }

        // Validate that the registered XMSS modes are present and valid
        ValidateCapabilities(parameters.Capabilities, errors);

        ValidateDomain(parameters.MessageLength, errors, "MessageLength", MIN_MESSAGE_LENGTH, MAX_MESSAGE_LENGTH);

        return new ParameterValidateResponse(errors);
    }

    private void ValidateCapabilities(GeneralCapabilities parametersCapabilities, List<string> errors)
    {
        if (parametersCapabilities == null)
        {
            errors.Add("Capabilities must be provided.");
            return;
        }

        if (parametersCapabilities.XmssModes == null || parametersCapabilities.XmssModes.Length == 0)
        {
            errors.Add("No XMSS modes provided for capabilities");
            return;
        }

        errors.AddIfNotNullOrEmpty(ValidateArray(parametersCapabilities.XmssModes, VALID_XMSS_TYPES, "XmssModes"));
    }
}
