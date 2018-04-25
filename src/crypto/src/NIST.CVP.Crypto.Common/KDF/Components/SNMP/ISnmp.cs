using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.SNMP
{
    public interface ISnmp
    {
        SnmpResult KeyLocalizationFunction(BitString snmpEngineId, string password);
    }
}
