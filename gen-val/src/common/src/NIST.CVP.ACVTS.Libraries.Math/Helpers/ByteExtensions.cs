namespace NIST.CVP.ACVTS.Libraries.Math.Helpers
{
    public static class ByteExtensions
    {
        public static BitString ToBitString(this byte value)
        {
            return BitString.To8BitString(value);
        }
    }
}
