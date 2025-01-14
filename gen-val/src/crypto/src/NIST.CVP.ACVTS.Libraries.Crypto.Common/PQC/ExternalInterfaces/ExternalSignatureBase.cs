using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.ExternalInterfaces;

public abstract class ExternalSignatureBase
{
    private readonly IShaFactory _shaFactory;
    
    protected ExternalSignatureBase(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    public abstract byte[] Sign(byte[] sk, byte[] message, byte[] rnd);

    public abstract bool Verify(byte[] pk, byte[] message, byte[] sig);

    protected abstract byte[] GetRnd(bool deterministic);
    
    public byte[] ExternalSign(byte[] sk, byte[] message, bool deterministic, byte[] context)
    {
        if (context.Length > 255)
        {
            throw new ArgumentException("Invalid context provided, must be less than 256 bytes.");
        }
        
        // IntermediateValueHelper.Print("message", message);
        // IntermediateValueHelper.Print("context", context);
        
        // M' = 0 || len(ctx) || ctx || M
        var mPrime = new byte[] { 0, (byte)context.Length }.Concatenate(context).Concatenate(message);

        // IntermediateValueHelper.Print("mPrime", mPrime);
        
        return Sign(sk, mPrime, GetRnd(deterministic));
    }

    public byte[] ExternalPreHashSign(byte[] sk, byte[] message, bool deterministic, byte[] context, HashFunction hashFunction)
    {
        // IntermediateValueHelper.Print("message", message);
        // IntermediateValueHelper.Print("context", context);
        // Console.WriteLine("hashAlg: " + hashFunction.Name);
        
        if (context.Length > 255)
        {
            throw new ArgumentException("Invalid context provided, must be less than 256 bytes.");
        }

        var xofHashFunction = new HashFunction(hashFunction.Mode, hashFunction.DigestSize, true); // Need to force pss to use the SHAKE OutputLen properties included there
        
        var sha = _shaFactory.GetShaInstance(xofHashFunction);
        var ph = sha.HashMessage(new BitString(message), xofHashFunction.OutputLen).Digest.ToBytes();

        // IntermediateValueHelper.Print("ph", ph);
        // IntermediateValueHelper.Print("oid", xofHashFunction.OID);
        
        // M' = 1 || len(ctx) || ctx || OID || PH
        var mPrime = new byte[] { 1, (byte)context.Length }.Concatenate(context).Concatenate(xofHashFunction.OID).Concatenate(ph);

        // IntermediateValueHelper.Print("mPrime", mPrime);
        
        return Sign(sk, mPrime, GetRnd(deterministic));
    }

    public bool ExternalVerify(byte[] pk, byte[] message, byte[] context, byte[] sig)
    {
        if (context.Length > 255)
        {
            return false;
        }
        
        // M' = 0 || len(ctx) || ctx || M
        var mPrime = new byte[] { 0, (byte)context.Length }.Concatenate(context).Concatenate(message);

        return Verify(pk, mPrime, sig);
    }

    public bool ExternalPreHashVerify(byte[] pk, byte[] message, byte[] context, byte[] sig, HashFunction hashFunction)
    {
        if (context.Length > 255)
        {
            return false;
        }

        var xofHashFunction = new HashFunction(hashFunction.Mode, hashFunction.DigestSize, true); // Need to force pss to use the SHAKE OutputLen properties included there
        
        var sha = _shaFactory.GetShaInstance(xofHashFunction);
        var ph = sha.HashMessage(new BitString(message), xofHashFunction.OutputLen).Digest.ToBytes();
        
        // M' = 1 || len(ctx) || ctx || OID || PH
        var mPrime = new byte[] { 1, (byte)context.Length }.Concatenate(context).Concatenate(xofHashFunction.OID).Concatenate(ph);
        
        return Verify(pk, mPrime, sig);
    }
}
