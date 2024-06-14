using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

public class FHOrTFactory
{
    private readonly IShaFactory _shaFactory;

    public FHOrTFactory(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    /// <summary>
    /// Return the correct F(), H() or Tl() to use. F(), H() and Tl() are each defined three times in FIPS 205; once in
    /// Section 10.1, once in Section 10.2, and once in Section 10.3. For the Section 10.1 (SLH-DSA Using SHAKE) and
    /// Section 10.2 (SLH-DSA Using SHA2 for Security Category 1) definitions, F() = H() = Tl(). For the
    /// Section 10.3 (SLH-DSA Using SHA2 for Security Categories 3 and 5) definitions, H() = Tl().
    /// Selection criteria is:
    /// * SHAKE vs SHA2 and,
    /// * if SHA2, the security level/category (could also be the value of n) and,
    /// * if the security level/category == 3 || 5, the function being asked for, i.e., F(), H() or Tl(). 
    /// </summary>
    /// <param name="slhdsaParameterSetAttributes">SlhdsaParameterSetAttributes</param>
    /// <param name="fHOrTType">FHOrTType (what type of function is required?)</param>
    /// <returns>IFHOrT</returns>
    public IFHOrT GetFHOrT(SlhdsaParameterSetAttributes slhdsaParameterSetAttributes, FHOrTType fHOrTType)
    {
        if (slhdsaParameterSetAttributes.ShaMode == ModeValues.SHAKE) 
            return new FHOrTShake(slhdsaParameterSetAttributes.N, _shaFactory);
        if (slhdsaParameterSetAttributes.ShaMode == ModeValues.SHA2)
        {
            if (slhdsaParameterSetAttributes.SecurityLevel == SecurityLevel.One)
                return new FHOrTSha2SecurityCategory1(slhdsaParameterSetAttributes.N, _shaFactory);
            if (slhdsaParameterSetAttributes.SecurityLevel == SecurityLevel.Three || slhdsaParameterSetAttributes.SecurityLevel == SecurityLevel.Five)
            {
                switch (fHOrTType)
                {
                    case FHOrTType.F:
                        return new FSha2SecurityCategories3and5(slhdsaParameterSetAttributes.N, _shaFactory);
                    case FHOrTType.H:
                    case FHOrTType.T:
                        return new HOrTSha2SecurityCategories3and5(slhdsaParameterSetAttributes.N, _shaFactory);
                }
            }
        }
        throw new ArgumentException($"{nameof(slhdsaParameterSetAttributes)}");
    }
}
