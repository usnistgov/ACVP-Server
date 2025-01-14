using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.ExternalInterfaces;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Dilithium.Tests;

public class FakeExternalSignatureBase : ExternalSignatureBase
{
    public FakeExternalSignatureBase(IShaFactory shaFactory) : base(shaFactory) { }

    public override byte[] Sign(byte[] sk, byte[] message, byte[] rnd)
    {
        return message;
    }

    public override bool Verify(byte[] pk, byte[] message, byte[] sig)
    {
        return message.SequenceEqual(sig);
    }

    protected override byte[] GetRnd(bool deterministic)
    {
        return null;
    }
}
