namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IRijndaelKeySchedule
    {
        int BlockCount { get; }
        string ErrorMessage { get; }
        bool IsValid { get; }
        int KeyCount { get; }
        int Rounds { get; }
        byte[,,] Schedule { get; }

        int GetCount(int size);
    }
}