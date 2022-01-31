using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX963
{
    public interface IAnsiX963Factory
    {
        IAnsiX963 GetInstance(HashFunction hashFunction);
    }
}
