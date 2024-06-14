using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Helpers;

public class PreHashProperties
{
    public PreHashProperties()
    {
        // PreHash = false
    }
    
    public PreHashProperties(ModeValues mode, DigestSizes digest)
    {
        // Need the last flag to be true to indicate the hash is used for signing
        PreHashFunction = new HashFunction(mode, digest, true);
    }

    public bool PreHash => PreHashFunction != null;
    private HashFunction PreHashFunction { get; }

    public byte[] OID
    {
        get
        {
            return PreHash ? PreHashFunction.OID : null;
        }
    }

    public int HashOutputLength
    {
        get
        {
            return PreHash ? PreHashFunction.OutputLen : 0;
        }
    }
}
