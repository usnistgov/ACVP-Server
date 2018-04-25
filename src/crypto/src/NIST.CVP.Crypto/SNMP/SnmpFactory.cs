using NIST.CVP.Crypto.Common.KDF.Components.SNMP;

namespace NIST.CVP.Crypto.SNMP
{
    public class SnmpFactory : ISnmpFactory
    {
        public ISnmp GetInstance()
        {
            return new Snmp();
        }
    }
}