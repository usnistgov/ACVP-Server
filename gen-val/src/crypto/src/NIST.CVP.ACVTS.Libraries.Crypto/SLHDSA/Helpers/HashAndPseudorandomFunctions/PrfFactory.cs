using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

public class PrfFactory
{
    private readonly IShaFactory _shaFactory;

    public PrfFactory(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    /// <summary>
    /// Return the correct Prf() to use. Three are defined in FIPS 205 Sections 10.1 - 10.3. The definitions of Prf()
    /// in sections 10.2 and 10.3 are the same. Selection criteria is SHAKE vs SHA2.
    /// </summary>
    /// <param name="slhdsaParameterSetAttributes">The SLH-DSA parameter set being used. See FIPS 205 Section 10 Table 1.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IPrf GetPrf(SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        if (slhdsaParameterSetAttributes.ShaMode == ModeValues.SHAKE)
            return new PrfShake(slhdsaParameterSetAttributes.N, _shaFactory);
        if (slhdsaParameterSetAttributes.ShaMode == ModeValues.SHA2)
            return new PrfSha2(slhdsaParameterSetAttributes.N, _shaFactory);
        throw new ArgumentException($"{nameof(slhdsaParameterSetAttributes)}");
    }
}
