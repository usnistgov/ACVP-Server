namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys
{
    public interface IHssKeyPair
    {
        int Levels { get; }
        IHssPrivateKey PrivateKey { get; }
        IHssPublicKey PublicKey { get; }
        bool IsExhausted { get; set; }
    }
}
