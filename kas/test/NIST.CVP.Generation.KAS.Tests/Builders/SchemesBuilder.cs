using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Generation.KAS.Tests.Builders
{
    public class SchemesBuilder
    {
        private DhEphem _dhEphem;

        public SchemesBuilder()
        {
            // TODO add more schemes as they're added
            _dhEphem = SchemeBuilder.GetBaseDhEphemBuilder().BuildDhEphem();
        }

        public SchemesBuilder WithDhEphem(DhEphem value)
        {
            _dhEphem = value;
            return this;
        }

        public Schemes Build()
        {
            return new Schemes()
            {
                DhEphem = _dhEphem
            };
        }
    }
}