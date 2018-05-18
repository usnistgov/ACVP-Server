using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class MacOptionsBaseBuilder
    {
        private MathDomain _keyLen;
        private int _macLen;
        private int _nonceLen;

        public MacOptionsBaseBuilder(bool nonceRequired)
        {
            _keyLen = new MathDomain().AddSegment(new ValueDomainSegment(256));
            _macLen = 128;

            if (nonceRequired)
            {
                _nonceLen = 64;
            }
        }

        public MacOptionsBaseBuilder WithKeyLen(MathDomain value)
        {
            _keyLen = value;
            return this;
        }

        public MacOptionsBaseBuilder WithMacLen(int value)
        {
            _macLen = value;
            return this;
        }

        public MacOptionsBaseBuilder WithNonceLen(int value)
        {
            _nonceLen = value;
            return this;
        }

        public MacOptionAesCcm BuildAesCcm()
        {
            return new MacOptionAesCcm()
            {
                KeyLen = _keyLen,
                MacLen = _macLen,
                NonceLen = _nonceLen
            };
        }

        public MacOptionCmac BuildAesCmac()
        {
            return new MacOptionCmac()
            {
                KeyLen = _keyLen,
                MacLen = _macLen,
                NonceLen = _nonceLen
            };
        }

        public MacOptionHmacSha2_d224 BuildHmac2_224()
        {
            return new MacOptionHmacSha2_d224()
            {
                KeyLen = _keyLen,
                MacLen = _macLen,
                NonceLen = _nonceLen
            };
        }

        public MacOptionHmacSha2_d256 BuildHmac2_256()
        {
            return new MacOptionHmacSha2_d256()
            {
                KeyLen = _keyLen,
                MacLen = _macLen,
                NonceLen = _nonceLen
            };
        }

        public MacOptionHmacSha2_d384 BuildHmac2_384()
        {
            return new MacOptionHmacSha2_d384()
            {
                KeyLen = _keyLen,
                MacLen = _macLen,
                NonceLen = _nonceLen
            };
        }

        public MacOptionHmacSha2_d512 BuildHmac2_512()
        {
            return new MacOptionHmacSha2_d512()
            {
                KeyLen = _keyLen,
                MacLen = _macLen,
                NonceLen = _nonceLen
            };
        }
    }
}