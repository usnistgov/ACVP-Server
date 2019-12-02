using System;

namespace NIST.CVP.Math.Exceptions
{
    /// <summary>
    /// Thrown when a <see cref="BitString"/> as hex cannot be parsed due to invalid length.
    /// </summary>
    public class InvalidBitStringLengthException : Exception
    {
        public InvalidBitStringLengthException(string message)
            : base (message)
        {
                
        }
    }
}