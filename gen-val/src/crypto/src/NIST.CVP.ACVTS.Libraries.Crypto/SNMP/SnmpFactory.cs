using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SNMP;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SNMP
{
    public class SnmpFactory : ISnmpFactory
    {
        private readonly IShaFactory _shaFactory;

        public SnmpFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public ISnmp GetInstance()
        {
            return new Snmp(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160)));
        }
    }
}
