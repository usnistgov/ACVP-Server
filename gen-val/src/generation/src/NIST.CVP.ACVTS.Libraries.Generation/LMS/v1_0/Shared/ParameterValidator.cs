using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.Shared;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static LmsMode[] VALID_LMS_TYPES = EnumHelpers.GetEnumsWithoutDefault<LmsMode>().ToArray();
    public static LmOtsMode[] VALID_LMOTS_TYPES = EnumHelpers.GetEnumsWithoutDefault<LmOtsMode>().ToArray();

    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        // TODO do we want to restrict in some manner the valid heights of merkle trees to test?
        // the difference between a height 15 and height 20 tree is quite pronounced, and the algorithm supports up to height 25.
        // height 15 = 1 << 15 =       32_768
        // height 20 = 1 << 20 =    1_048_576
        // height 25 = 1 << 25 =   33_554_432
        // The generation time/storage constraints for the increasing tree sizes grows at the same rate as the number of nodes 

        if (!ValidateAlgoMode(parameters, new[] { AlgoMode.LMS_KeyGen_v1_0, AlgoMode.LMS_SigGen_v1_0, AlgoMode.LMS_SigVer_v1_0 }, errors))
            return new ParameterValidateResponse(errors);

        // Validate only specific *or* general are used, not both
        ValidateGeneralSpecific(parameters, errors);

        // General validation - ensure that at least one LmOts matches hash functions/output from Lms, and vice verse
        ValidateGeneral(parameters.Capabilities, errors);

        // Specific validation - ensure that Lms LmOts pairs match hash functions/output
        ValidateSpecific(parameters.SpecificCapabilities, errors);

        return new ParameterValidateResponse(errors);
    }

    private void ValidateGeneralSpecific(Parameters parameters, List<string> errors)
    {
        if (parameters.SpecificCapabilities?.Length == 0 &&
            (parameters.Capabilities?.LmOtsModes?.Length == 0 || parameters.Capabilities?.LmsModes?.Length == 0))
        {
            errors.Add("Either specificCapabilities or Capabilities must be provided.");
            return;
        }

        if (parameters.SpecificCapabilities?.Length > 0 &&
            (parameters.Capabilities?.LmOtsModes?.Length > 0 || parameters.Capabilities?.LmsModes?.Length > 0))
        {
            errors.Add("Either specificCapabilities or Capabilities must be provided, not both.");
        }
    }

    private void ValidateGeneral(GeneralCapabilities parametersCapabilities, List<string> errors)
    {
        if (parametersCapabilities == null)
            return;

        if (parametersCapabilities.LmsModes?.Length == 0 && parametersCapabilities.LmOtsModes?.Length == 0)
            return;

        if (parametersCapabilities.LmsModes == null)
        {
            errors.Add("No LMS types provided for general capabilities");
            return;
        }
        if (parametersCapabilities.LmOtsModes == null)
        {
            errors.Add("No LM-OTS types provided");
            return;
        }

        if (errors.AddIfNotNullOrEmpty(ValidateArray(parametersCapabilities.LmsModes, VALID_LMS_TYPES,
                "LmsTypes")))
            return;
        if (errors.AddIfNotNullOrEmpty(ValidateArray(parametersCapabilities.LmOtsModes, VALID_LMOTS_TYPES,
                "LmOtsTypes")))
            return;

        // Get "attributes" of each registered LMS and LM-OTS.
        var lmsAttributes = parametersCapabilities.LmsModes
            .Select(AttributesHelper.GetLmsAttribute)
            .Select(item => new
            {
                Function = item.ShaMode,
                OutputLength = item.M
            }).Distinct().ToList();
        var lmOtsAttributes = parametersCapabilities.LmOtsModes
            .Select(AttributesHelper.GetLmOtsAttribute)
            .Select(item => new
            {
                Function = item.ShaMode,
                OutputLength = item.N
            }).Distinct().ToList();

        // Each LMS hash function and output length needs to have a matching hash function and output length from LM-OTS.
        foreach (var lmsAttribute in lmsAttributes)
        {
            if (!lmOtsAttributes.Any(a =>
                    a.OutputLength == lmsAttribute.OutputLength && a.Function == lmsAttribute.Function))
            {
                errors.Add($"No LM-OTS matching function was found for the given LMS function with an outputLength of {lmsAttribute.OutputLength} and hashFunction of {lmsAttribute.Function}");
            }
        }

        // The same is true in the opposite direction, each LM-OTS needs to have a matching LMS.
        foreach (var lmOtsAttribute in lmOtsAttributes)
        {
            if (!lmsAttributes.Any(a =>
                    a.OutputLength == lmOtsAttribute.OutputLength && a.Function == lmOtsAttribute.Function))
            {
                errors.Add($"No LMS matching function was found for the given LM-OTS function with an outputLength of {lmOtsAttribute.OutputLength} and hashFunction of {lmOtsAttribute.Function}");
            }
        }
    }

    private void ValidateSpecific(SpecificCapability[] parametersSpecificCapabilities, List<string> errors)
    {
        if (parametersSpecificCapabilities == null || parametersSpecificCapabilities.Length == 0)
            return;

        foreach (var capability in parametersSpecificCapabilities)
        {
            if (errors.AddIfNotNullOrEmpty(ValidateArray(new[] { capability.LmsMode }, VALID_LMS_TYPES,
                    "LmsTypes")))
                return;
            if (errors.AddIfNotNullOrEmpty(ValidateArray(new[] { capability.LmOtsMode }, VALID_LMOTS_TYPES,
                    "LmOtsTypes")))
                return;

            var lmsAttribute = AttributesHelper.GetLmsAttribute(capability.LmsMode);
            var lmOtsAttribute = AttributesHelper.GetLmOtsAttribute(capability.LmOtsMode);

            if (lmsAttribute.ShaMode != lmOtsAttribute.ShaMode || lmsAttribute.M != lmOtsAttribute.N)
                errors.Add($"For specific capabilities each entry must have a matching hash function and length. Found mismatch on {capability.LmsMode}/{capability.LmOtsMode}.");
        }
    }
}
