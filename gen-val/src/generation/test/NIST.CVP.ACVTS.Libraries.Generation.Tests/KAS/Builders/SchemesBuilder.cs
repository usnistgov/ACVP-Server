using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.Builders
{
    public class SchemesBuilder
    {
        private FfcDhEphem _dhEphem;
        private FfcMqv1 _mqv1;

        public SchemesBuilder WithDhEphem(FfcDhEphem value)
        {
            _dhEphem = value;
            return this;
        }

        public SchemesBuilder WithMqv1(FfcMqv1 value)
        {
            _mqv1 = value;
            return this;
        }

        public Schemes BuildSchemes()
        {
            return new Schemes()
            {
                FfcDhEphem = _dhEphem,
                FfcMqv1 = _mqv1
            };
        }
    }
}
