namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.CTS
{
    public static class SwapBytesHelper
    {
        /// <summary>
        /// Swap two bytes within a byte array
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="byteA"></param>
        /// <param name="byteB"></param>
        public static void SwapBytes(byte[] bytes, int byteA, int byteB)
        {
            // x = x ^ y
            // y = x ^ y
            // x = x ^ y

            bytes[byteA] = (byte)(bytes[byteA] ^ bytes[byteB]);
            bytes[byteB] = (byte)(bytes[byteA] ^ bytes[byteB]);
            bytes[byteA] = (byte)(bytes[byteA] ^ bytes[byteB]);
        }
    }
}
