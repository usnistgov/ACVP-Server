using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public interface IAlgoArrayResponse : ICryptoResult
    {
        BitString CipherText { get; }
        BitString IV { get; }
        BitString Key { get; }
        BitString PlainText { get; }
    }
}