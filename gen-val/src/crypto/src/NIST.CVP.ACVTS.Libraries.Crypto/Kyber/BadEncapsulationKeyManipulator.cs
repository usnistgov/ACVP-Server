using System;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Kyber;

public class BadEncapsulationKeyManipulator
{
    private Kyber Kyber { get; }

    /// <summary>
    /// Manipulates EncapsulationKeys to fail the EncapsulationKeyCheck 
    /// </summary>
    /// <param name="kyber"></param>
    public BadEncapsulationKeyManipulator(Kyber kyber)
    {
        Kyber = kyber;
    }

    public byte[] ManipulateEncapsulationKey(byte[] ek)
    {
        // Values in tHat are encoded modulo q, which is assumed by ByteEncode.
        // ByteDecode reads the values in ek to go into tHat and performs the modulo q.
        // We want keys where the ByteDecode would lead to an improper tHat because the modulo is missing.
        // This means we need to start with a valid key, decode tHat, increase some values to be above q, then reapply ByteEncode
        var tHat = new int[Kyber.Param.K][];
        for (var i = 0; i < Kyber.Param.K; i++)
        {
            tHat[i] = Kyber.ByteDecode(12, ek[(i * 384)..(i * 384 + 384)]);
        }

        // Modify a value in tHat to be larger than Q
        tHat[0][0] = Kyber.Param.Q + 1;
        
        // Re-encode with the new tHat
        var newEk = Array.Empty<byte>();
        for (var i = 0; i < Kyber.Param.K; i++)
        {
            newEk = ek.Concatenate(Kyber.ByteEncode(12, tHat[i]));
        }
        
        // Need to grab rho from the original key
        newEk = newEk.Concatenate(ek[(Kyber.Param.K * 384)..]);
        
        return newEk;
    }
}
