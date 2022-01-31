using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric
{
    public interface IAlgoArrayResponse : ICryptoResult
    {
        BitString CipherText { get; }
        BitString IV { get; }
        BitString Key { get; }
        BitString PlainText { get; }
    }
}
