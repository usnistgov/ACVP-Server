namespace NIST.CVP.Crypto.Common.KDF.Components.SRTP
{
    public class KdfResult
    {
        public SrtpResult SrtpResult { get; }
        public SrtpResult SrtcpResult { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KdfResult(SrtpResult srtp, SrtpResult srtcp)
        {
            SrtpResult = srtp;
            SrtcpResult = srtcp;
        }

        public KdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
