using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TLS
{
    public class TlsKdfv12_ExtendedMasterSecret : TlsKdfv12
    {
        protected override string MasterSecretLabel() => "extended master secret";

        public TlsKdfv12_ExtendedMasterSecret(IHmac hmac) : base(hmac)
        {
        }
    }
}
