using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MLKEM;

public class BadDecapsulationKeyManipulator
{
    private MLKEM Mlkem { get; }

    /// <summary>
    /// Manipulates DecapsulationKeys to fail the DecapsulationKeyCheck 
    /// </summary>
    /// <param name="mlkem"></param>
    public BadDecapsulationKeyManipulator(MLKEM mlkem)
    {
        Mlkem = mlkem;
    }

    public byte[] ManipulateDecapsulationKey(byte[] dk)
    {
        //      384 * k    384 * k + 32    32       32 bytes
        // dk = dk_pke  || ek           || H(ek) || z
        
        var newDk = new byte[dk.Length];
        Array.Copy(dk, newDk, dk.Length);
        
        // Flip a bit in H(ek), which is between (768 * k + 32) - (768 * k + 64)
        newDk[(768 * Mlkem.Param.K) + 48] ^= 1;

        return newDk;
    }
}
