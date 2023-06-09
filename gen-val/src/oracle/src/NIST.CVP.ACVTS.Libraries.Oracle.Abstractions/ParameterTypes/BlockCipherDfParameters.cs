using System;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

public class BlockCipherDfParameters : IParameters
{
    public int KeyLength { get; set; }
    public int DataLength { get; set; }
    public int OutputLength { get; set; }
    
    public override bool Equals(object other)
    {
        if (other is AesParameters p)
        {
            return GetHashCode() == p.GetHashCode();
        }

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(OutputLength, KeyLength, DataLength);
}
