using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SNMP
{
    public interface ISnmp
    {
        SnmpResult KeyLocalizationFunction(BitString snmpEngineId, string password);
    }
}
