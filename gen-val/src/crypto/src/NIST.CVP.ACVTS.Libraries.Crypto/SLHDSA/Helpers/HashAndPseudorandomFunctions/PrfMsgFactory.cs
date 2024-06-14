using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

public class PrfMsgFactory
{
    private readonly IShaFactory _shaFactory;

    public PrfMsgFactory(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    /// <summary>
    /// Return the correct PRFmsg() to use. Three are defined in FIPS 205 Sections 10.1 - 10.3. The definitions of PRFmsg()
    /// in sections 10.2 and 10.3 differ by which HMAC function is used, i.e., HMAC-SHA-256 vs HMAC-SHA-512.
    /// Selection criteria is SHAKE vs SHA2, and, if SHA2, security level 1 vs 3 or 5.
    /// </summary>
    /// <param name="slhdsaParameterSetAttributes">The SLH-DSA parameter set being used. See FIPS 205 Section 10 Table 1.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IPrfMsg GetPrfMsg(SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        switch (slhdsaParameterSetAttributes.ShaMode)
        {
            case ModeValues.SHAKE:
                return new PrfMsgShake(slhdsaParameterSetAttributes.N, _shaFactory);
            case ModeValues.SHA2:
                switch (slhdsaParameterSetAttributes.SecurityLevel) // aka Security Category
                {
                    case SecurityLevel.One:
                        return new PrfMsgSha2(slhdsaParameterSetAttributes.N, new HashFunction(ModeValues.SHA2, DigestSizes.d256), _shaFactory);
                    case SecurityLevel.Three:
                    case SecurityLevel.Five:
                        return new PrfMsgSha2(slhdsaParameterSetAttributes.N, new HashFunction(ModeValues.SHA2, DigestSizes.d512), _shaFactory);     
                }
                break;
        }
        throw new ArgumentException($"{nameof(slhdsaParameterSetAttributes)}");
    }
}
