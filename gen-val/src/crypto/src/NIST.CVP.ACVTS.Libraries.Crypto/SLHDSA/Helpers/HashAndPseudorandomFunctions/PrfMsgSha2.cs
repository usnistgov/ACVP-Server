using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

/// <summary>
/// Implements the function PRFmsg()
/// from FIPS 205 section 10.2, PRFmsg(SK.prf,opt_rand,M) = Truncn(HMAC-SHA-256(SK.prf,opt_rand ∥ M)),
/// and FIPS 205 section 10.3, PRFmsg(SK.prf,opt_rand,M) = Truncn(HMAC-SHA-512(SK.prf,opt_rand ∥ M))
/// </summary>
public class PrfMsgSha2 : IPrfMsg
{
    private int _n;
    private IHmac _hmac;

    /// <param name="n">The n value for the parameter set in use.</param>
    /// <param name="hmacType">SHA2-256 for HMAC-SHA-256, SHA2-512 for HMAC-SHA-512</param>
    public PrfMsgSha2(int n, HashFunction hmacType, IShaFactory shaFactory)
    {
        _n = n;
        _hmac = new HmacFactory(shaFactory).GetHmacInstance(hmacType);
    }
    
    /// <summary>
    /// Performs PRFmsg(SK.prf,opt_rand,M) = Truncn(HMAC-SHA-256(SK.prf,opt_rand ∥ M)) or
    /// PRFmsg(SK.prf,opt_rand,M) = Truncn(HMAC-SHA-512(SK.prf,opt_rand ∥ M)) depending on the hmacType passed to
    /// the constructor.
    /// </summary>
    /// <param name="skPrf">SK.prf</param>
    /// <param name="optRand">opt_rand</param>
    /// <param name="M">M</param>
    /// <returns></returns>
    public byte[] GetPseudorandomByteString(byte[] skPrf, byte[] optRand, byte[] M)
    {
        // Build the byte string/text that will be MACed, i.e., opt_rand ∥ M
        var text = new byte[optRand.Length + M.Length];
        Array.Copy(optRand, 0, text, 0, optRand.Length);
        Array.Copy(M, 0, text, optRand.Length, M.Length);
        
        var macResult = _hmac.Generate(new BitString(skPrf),new BitString(text));
        var mac = macResult.Mac.ToBytes();
        
        return mac[.._n];
    }
}
