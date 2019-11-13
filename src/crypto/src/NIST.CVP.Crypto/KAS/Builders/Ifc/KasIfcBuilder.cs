using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;

namespace NIST.CVP.Crypto.KAS.Builders.Ifc
{
    public class KasIfcBuilder : IKasIfcBuilder
    {
        private ISchemeIfcBuilder _schemeBuilder;
        
        public IKasIfcBuilder WithSchemeBuilder(ISchemeIfcBuilder value)
        {
            _schemeBuilder = value;
            return this;
        }

        public IKasIfc Build()
        {
            return new KasIfc(_schemeBuilder.Build());
        }
    }
}