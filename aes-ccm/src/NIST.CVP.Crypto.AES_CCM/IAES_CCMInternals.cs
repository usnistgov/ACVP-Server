namespace NIST.CVP.Crypto.AES_CCM
{
    public interface IAES_CCMInternals
    {
        void CCM_counter_80211(byte[] nonce, int Nlen, ref byte[] InitCtr);
        void CCM_format_80211(ref byte[] B, byte[] nonce, int Nlen, byte[] payload, int Plen, byte[] assocData, int Alen, int Tlen, ref byte[] InitCtr, ref int r);
    }
}