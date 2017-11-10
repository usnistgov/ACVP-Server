using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class SchemesBuilder
    {
        private DhEphem _dhEphem;
        private Mqv1 _mqv1;

        public SchemesBuilder WithDhEphem(DhEphem value)
        {
            _dhEphem = value;
            return this;
        }

        public SchemesBuilder WithMqv1(Mqv1 value)
        {
            _mqv1 = value;
            return this;
        }

        public Schemes BuildSchemes()
        {
            return new Schemes()
            {
                DhEphem = _dhEphem,
                Mqv1 = _mqv1
            };
        }
    }
}