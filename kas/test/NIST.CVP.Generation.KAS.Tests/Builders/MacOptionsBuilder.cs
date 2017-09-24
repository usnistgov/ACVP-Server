namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class MacOptionsBuilder
    {
        private MacOptionAesCcm _macOptionAesCcm;
        private MacOptionCmac _macOptionCmac;
        private MacOptionHmacSha2_d224 _macOptionHmac2_224;
        private MacOptionHmacSha2_d256 _macOptionHmac2_256;
        private MacOptionHmacSha2_d384 _macOptionHmac2_384;
        private MacOptionHmacSha2_d512 _macOptionHmac2_512;

        public MacOptionsBuilder()
        {
            _macOptionAesCcm = new MacOptionsBaseBuilder(true).BuildAesCcm();
            _macOptionCmac = new MacOptionsBaseBuilder(false).BuildAesCmac();
            _macOptionHmac2_224 = new MacOptionsBaseBuilder(false).BuildHmac2_224();
            _macOptionHmac2_256 = new MacOptionsBaseBuilder(false).BuildHmac2_256();
            _macOptionHmac2_384 = new MacOptionsBaseBuilder(false).BuildHmac2_384();
            _macOptionHmac2_512 = new MacOptionsBaseBuilder(false).BuildHmac2_512();
        }

        public MacOptionsBuilder WithAesCcm(MacOptionAesCcm value)
        {
            _macOptionAesCcm = value;
            return this;
        }

        public MacOptionsBuilder WithCmac(MacOptionCmac value)
        {
            _macOptionCmac = value;
            return this;
        }

        public MacOptionsBuilder WithHmac2_224(MacOptionHmacSha2_d224 value)
        {
            _macOptionHmac2_224 = value;
            return this;
        }

        public MacOptionsBuilder WithHmac2_256(MacOptionHmacSha2_d256 value)
        {
            _macOptionHmac2_256 = value;
            return this;
        }

        public MacOptionsBuilder WithHmac2_384(MacOptionHmacSha2_d384 value)
        {
            _macOptionHmac2_384 = value;
            return this;
        }

        public MacOptionsBuilder WithHmac2_512(MacOptionHmacSha2_d512 value)
        {
            _macOptionHmac2_512 = value;
            return this;
        }

        public MacOptions Build()
        {
            return new MacOptions()
            {
                AesCcm = _macOptionAesCcm,
                Cmac = _macOptionCmac,
                HmacSha2_D224 = _macOptionHmac2_224,
                HmacSha2_D256 = _macOptionHmac2_256,
                HmacSha2_D384 = _macOptionHmac2_384,
                HmacSha2_D512 = _macOptionHmac2_512
            };
        }
    }
}