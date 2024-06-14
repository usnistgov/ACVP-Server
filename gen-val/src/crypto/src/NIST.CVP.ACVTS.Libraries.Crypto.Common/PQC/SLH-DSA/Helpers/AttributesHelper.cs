using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;

public static class AttributesHelper
{
    /// <summary>
    /// The actual SLH-DSA Parameter Set definitions from FIPS 205 Section 10
    /// </summary>
    private static readonly Dictionary<SlhdsaParameterSet, SlhdsaParameterSetAttributes> SlhdsaParameterSetAttributes = new()
    {
        { 
            SlhdsaParameterSet.SLH_DSA_SHA2_128s, 
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHA2_128s, 16, 63, 7, 9, 12, 14, 4, 16,35,32,3, 30, SecurityLevel.One, 32, 7856, ModeValues.SHA2)
        },
        {
            SlhdsaParameterSet.SLH_DSA_SHAKE_128s,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHAKE_128s,  16, 63, 7, 9, 12, 14, 4, 16,35,32,3,30, SecurityLevel.One, 32, 7856, ModeValues.SHAKE)
        },
        { 
            SlhdsaParameterSet.SLH_DSA_SHA2_128f, 
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHA2_128f, 16, 66, 22, 3, 6, 33, 4, 16,35,32,3, 34, SecurityLevel.One, 32, 17088, ModeValues.SHA2)
        },
        {
            SlhdsaParameterSet.SLH_DSA_SHAKE_128f,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHAKE_128f,  16, 66, 22, 3, 6, 33, 4, 16,35,32,3,34, SecurityLevel.One, 32, 17088, ModeValues.SHAKE)
        },
        { 
            SlhdsaParameterSet.SLH_DSA_SHA2_192s, 
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHA2_192s, 24, 63, 7, 9, 14, 17, 4, 16,51,48,3, 39, SecurityLevel.Three, 48, 16224, ModeValues.SHA2)
        },
        {
            SlhdsaParameterSet.SLH_DSA_SHAKE_192s,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHAKE_192s,  24, 63, 7, 9, 14, 17, 4, 16,51,48,3, 39, SecurityLevel.Three, 48, 16224, ModeValues.SHAKE)
        },
        { 
            SlhdsaParameterSet.SLH_DSA_SHA2_192f, 
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHA2_192f, 24, 66, 22, 3, 8, 33, 4, 16,51,48,3, 42, SecurityLevel.Three, 48, 35664, ModeValues.SHA2)
        },
        {
            SlhdsaParameterSet.SLH_DSA_SHAKE_192f,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHAKE_192f,  24, 66, 22, 3, 8, 33, 4, 16,51,48,3, 42, SecurityLevel.Three, 48, 35664, ModeValues.SHAKE)
        },
        
        {
            SlhdsaParameterSet.SLH_DSA_SHA2_256s,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHA2_256s, 32, 64, 8, 8, 14, 22, 4, 16, 67,64,3,47, SecurityLevel.Five, 64, 29792, ModeValues.SHA2)
        },
        {
            SlhdsaParameterSet.SLH_DSA_SHAKE_256s,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHAKE_256s, 32, 64, 8, 8, 14, 22, 4, 16,67,64,3,47, SecurityLevel.Five, 64, 29792, ModeValues.SHAKE)
        },
        
        {
            SlhdsaParameterSet.SLH_DSA_SHA2_256f,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHA2_256f, 32, 68, 17, 4, 9, 35, 4, 16, 67,64,3,49, SecurityLevel.Five, 64, 49856, ModeValues.SHA2)
        },
        {
            SlhdsaParameterSet.SLH_DSA_SHAKE_256f,
            new SlhdsaParameterSetAttributes(SlhdsaParameterSet.SLH_DSA_SHAKE_256f, 32, 68, 17, 4, 9, 35, 4, 16,67,64,3,49, SecurityLevel.Five, 64, 49856, ModeValues.SHAKE)
        }
    };

    /// <summary>
    /// Get the SLH-DSA parameter set values based on <see cref="SlhdsaParameterSet"/>
    /// </summary>
    /// <param name="slhdsaParameterSet"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static SlhdsaParameterSetAttributes GetParameterSetAttribute(SlhdsaParameterSet slhdsaParameterSet)
    {
        if (!SlhdsaParameterSetAttributes.TryFirst(w => w.Key == slhdsaParameterSet, out var result))
        {
            throw new ArgumentException(
                $"Couldn't map {nameof(slhdsaParameterSet)} for retrieving parameter set attributes.");
        }

        return result.Value;
    }
    
}
