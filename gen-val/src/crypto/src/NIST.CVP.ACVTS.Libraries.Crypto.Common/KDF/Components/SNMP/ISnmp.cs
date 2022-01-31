using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SNMP
{
    public interface ISnmp
    {
        SnmpResult KeyLocalizationFunction(BitString snmpEngineId, string password);
    }
}
