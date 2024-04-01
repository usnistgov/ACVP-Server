namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

public interface IShake : ISha
{
    void Absorb(byte[] message, int bitLength);
    void Squeeze(byte[] output, int outputBitLength);
}
