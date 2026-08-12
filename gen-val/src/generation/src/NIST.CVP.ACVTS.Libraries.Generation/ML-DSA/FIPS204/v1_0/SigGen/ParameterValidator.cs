using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.v1_0.SigGen;

public class ParameterValidator : PqcParameterValidator, IParameterValidator<Parameters>
{
    public static readonly DilithiumParameterSet[] VALID_PARAMETER_SETS = EnumHelpers.GetEnums<DilithiumParameterSet>().Except(new [] { DilithiumParameterSet.None }).ToArray();

    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.ML_DSA_SigGen_FIPS204 }, errors);
        ValidateDeterministic(parameters, errors);
        ValidateSignatureInterfacesAndPreHash(parameters, errors);
        ValidateCapabilities(parameters, errors);
        ValidateExternalMu(parameters, errors);

        return errors.Any() ? new ParameterValidateResponse(errors) : new ParameterValidateResponse();
    }

    private void ValidateDeterministic(Parameters parameters, List<string> errors)
    {
        // 1) was Deterministic included in the registration?
        if (parameters.Deterministic == null)
        {
            errors.Add($"{nameof(parameters.Deterministic)} was not provided.");
            return;
        }

        // 2) was Deterministic included, but empty?
        if (!parameters.Deterministic.Distinct().Any())
        {
            errors.Add("Expected at least one deterministic option provided");
            return;
        }

        // 3) the values of 'true' and 'false' should only be included up to once a piece
        if (parameters.Deterministic.Distinct().Count() != parameters.Deterministic.Count())
        {
            errors.Add("Expected no duplicate deterministic options");
        }
    }
    
    private void ValidateCapabilities(Parameters parameters, List<string> errors)
    {
        // 1) was Capabilities included, but empty?
        if (!parameters.Capabilities.Distinct().Any())
        {
            errors.Add($"Expected {nameof(parameters.Capabilities)} to not be empty");
            return;
        }
        
        // 2) examine each Capability that was provided
        foreach (var capability in parameters.Capabilities)
        {
            // i) is ParameterSets non-empty?
            if (!capability.ParameterSets.Distinct().Any())
            {
                errors.Add($"Expected {nameof(capability.ParameterSets)} to contain at least one valid ML-DSA parameter set");
                return;
            }

            // ii) check no duplicates are provided
            if (capability.ParameterSets.Length != capability.ParameterSets.Distinct().Count())
            {
                errors.Add($"{nameof(capability.ParameterSets)} must not contain the same ML-DSA parameter set more than once");
            }
            
            // iii) run the base validator on each capability
            ValidateCapability(capability, parameters, errors);
        }
    }

    private void ValidateExternalMu(Parameters parameters, List<string> errors)
    {
        // ExternalMu is only allowed when SignatureInterface.Internal is present
        //      For which no duplicates are allowed and the length must be > 0
        // ExternalMu is not allowed in all other cases (so that it doesn't appear on the certificate when it is not relevant)
        if (parameters.SignatureInterfaces.Contains(SignatureInterface.Internal))
        {
            if (parameters.ExternalMu == null)
            {
                errors.Add("Internal signature interface was defined without providing externalMu");
                return;
            }

            if (parameters.ExternalMu.Length == 0)
            {
                errors.Add("No externalMu values found in array");
            }
            
            if (parameters.ExternalMu.Distinct().Count() != parameters.ExternalMu.Length)
            {
                errors.Add("Expected no duplicates in externalMu");
            }
        }
        else
        {
            if (parameters.ExternalMu != null)
            {
                errors.Add("ExternalMu was provided when only the external signature interface is included");
            }
        }
    }
}
