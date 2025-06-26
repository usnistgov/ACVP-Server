using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Kyber;

public class BadDecapsulationKeyManipulator
{
    private Kyber Kyber { get; }

    /// <summary>
    /// Manipulates DecapsulationKeys to fail the DecapsulationKeyCheck 
    /// </summary>
    /// <param name="kyber"></param>
    public BadDecapsulationKeyManipulator(Kyber kyber)
    {
        Kyber = kyber;
    }

    public byte[] ManipulateDecapsulationKey(byte[] dk)
    {
        //      384 * k    384 * k + 32    32       32 bytes
        // dk = dk_pke  || ek           || H(ek) || z
        
        var newDk = new byte[dk.Length];
        Array.Copy(dk, newDk, dk.Length);
        
        // Flip a bit in H(ek), which is between (768 * k + 32) - (768 * k + 64)
        newDk[(768 * Kyber.Param.K) + 48] ^= 1;

        return newDk;
    }
}
