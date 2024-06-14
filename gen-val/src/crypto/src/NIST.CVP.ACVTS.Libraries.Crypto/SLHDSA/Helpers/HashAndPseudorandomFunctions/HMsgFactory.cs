using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

public class HMsgFactory
{
    private readonly IShaFactory _shaFactory;

    public HMsgFactory(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    /// <summary>
    /// Return the correct Hmsg() to use. Three are defined in FIPS 205 Sections 10.1 - 10.3. Selection criteria is
    /// SHAKE vs SHA2 and, if SHA2, the security level/category (could also be the value of n). 
    /// </summary>
    /// <param name="slhdsaParameterSetAttributes"></param>
    /// <returns>IHMsg</returns>
    public IHMsg GetHMsg(SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        if (slhdsaParameterSetAttributes.ShaMode == ModeValues.SHAKE)
        {
            return new HMsgShake(slhdsaParameterSetAttributes.M, _shaFactory);
        }

        if (slhdsaParameterSetAttributes.ShaMode == ModeValues.SHA2)
        {
            if (slhdsaParameterSetAttributes.SecurityLevel == SecurityLevel.One)
                return new HMsgSha2SecurityCategory1(slhdsaParameterSetAttributes.M, _shaFactory);
            if (slhdsaParameterSetAttributes.SecurityLevel == SecurityLevel.Three || slhdsaParameterSetAttributes.SecurityLevel == SecurityLevel.Five)
                return new HMsgSha2SecurityCategories3And5(slhdsaParameterSetAttributes.M, _shaFactory);
        }
        
        throw new ArgumentException($"{nameof(slhdsaParameterSetAttributes)}");
    }
}
