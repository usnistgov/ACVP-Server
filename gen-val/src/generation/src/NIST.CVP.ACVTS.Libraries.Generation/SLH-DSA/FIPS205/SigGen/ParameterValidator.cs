using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigGen;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static readonly int MinMsgLen = 8;
    public static readonly int MaxMsgLen = 65536;

    public static readonly SlhdsaParameterSet[] FastSigningParameterSets = new[] 
    { 
        SlhdsaParameterSet.SLH_DSA_SHA2_128f, SlhdsaParameterSet.SLH_DSA_SHA2_192f, SlhdsaParameterSet.SLH_DSA_SHA2_256f, 
        SlhdsaParameterSet.SLH_DSA_SHAKE_128f, SlhdsaParameterSet.SLH_DSA_SHAKE_192f, SlhdsaParameterSet.SLH_DSA_SHAKE_256f
    };
    public static readonly SlhdsaParameterSet[] SmallSignatureParameterSets = new[] 
    { 
        SlhdsaParameterSet.SLH_DSA_SHA2_128s, SlhdsaParameterSet.SLH_DSA_SHA2_192s, SlhdsaParameterSet.SLH_DSA_SHA2_256s, 
        SlhdsaParameterSet.SLH_DSA_SHAKE_128s, SlhdsaParameterSet.SLH_DSA_SHAKE_192s, SlhdsaParameterSet.SLH_DSA_SHAKE_256s
    };
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.SLH_DSA_SigGen_FIPS205 }, errors);
        ValidateDeterministic(parameters, errors);
        ValidateCapabilities(parameters, errors);
        
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
        foreach (bool boolValue in new[] { true, false })
        {
            if (parameters.Deterministic.Count(t => t == boolValue) > 1)
                errors.Add($"{nameof(parameters.Deterministic)} may not include {boolValue.ToString()} more than once");
        }
    }

    private void ValidateCapabilities(Parameters parameters, List<string> errors)
    {
        // 1) was Capabilities included in the registration?
        if (parameters.Capabilities == null)
        {
            errors.Add($"{nameof(parameters.Capabilities)} was not provided.");
            return;
        }
        
        var capabilitiesElementType = parameters.Capabilities.GetType().GetElementType();
        var capabilitiesElementTypeString = capabilitiesElementType != null ? capabilitiesElementType.ToString() : "Capability"; 
        
        // 2) was Capabilities included, but empty?
        if (!parameters.Capabilities.Distinct().Any())
        {
            errors.Add($"Expected {nameof(parameters.Capabilities)} to contain at least one {capabilitiesElementTypeString}");
            return;
        }

        // 3) examine each Capability that was provided
        var shouldReturn = false;
        foreach (var capability in parameters.Capabilities)
        {
            // i) Were ParameterSets and MessageLength provided?
            if (capability.ParameterSets == null)
            {
                errors.Add($"{nameof(capability.ParameterSets)} was not provided");
                shouldReturn = true;
            }
            if (capability.MessageLength == null)
            {
                errors.Add($"{nameof(capability.MessageLength)} was not provided");
                shouldReturn = true;
            }

            if (shouldReturn)
                return;
            
            // ii) is ParameterSets non-empty?
            if (!capability.ParameterSets.Distinct().Any())
            {
                errors.Add($"Expected {nameof(capability.ParameterSets)} to contain at least one valid SLH-DSA parameter set");
                shouldReturn = true;
            }
            
            // iii) are the MessageLength values valid?
            ValidateDomain(capability.MessageLength, errors, "message length", MinMsgLen, MaxMsgLen);
            ValidateMultipleOf(capability.MessageLength, errors, 8, "complete byte messages");
            
            if (shouldReturn)
                return;
            
            //iv) ParameterSets shouldn't contain any repeats
            if (capability.ParameterSets.Length != capability.ParameterSets.Distinct().Count())
                errors.Add($"{nameof(capability.ParameterSets)} must not contain the same SLH-DSA parameter set more than once");
        }
    }
}
