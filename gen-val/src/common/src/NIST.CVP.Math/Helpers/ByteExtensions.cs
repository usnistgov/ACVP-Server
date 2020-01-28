namespace NIST.CVP.Math.Helpers
{
    public static class ByteExtensions
    {
        public static BitString ToBitString(this byte value)
        {
            return BitString.To8BitString(value);
        }
    }
}