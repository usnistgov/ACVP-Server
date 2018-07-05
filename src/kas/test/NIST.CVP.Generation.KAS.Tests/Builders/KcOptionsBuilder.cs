namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class KcOptionsBuilder
    {
        private string[] _kcRole;
        private string[] _kcType;
        private string[] _nonceType;

        public KcOptionsBuilder()
        {
            _kcRole = new string[] { "provider", "recipient" };
            _kcType = new string[] { "unilateral", "bilateral" };
            _nonceType = new string[] { "randomNonce", "sequence", "timestamp" } ;
        }

        public KcOptionsBuilder WithKcRole(string[] value)
        {
            _kcRole = value;
            return this;
        }

        public KcOptionsBuilder WithKcType(string[] value)
        {
            _kcType = value;
            return this;
        }

        public KcOptionsBuilder WithNonceType(string[] value)
        {
            _nonceType = value;
            return this;
        }

        public KcOptions BuildKcOptions()
        {
            return new KcOptions()
            {
                KcRole = _kcRole,
                KcType = _kcType,
                NonceType = _nonceType
            };
        }
    }
}