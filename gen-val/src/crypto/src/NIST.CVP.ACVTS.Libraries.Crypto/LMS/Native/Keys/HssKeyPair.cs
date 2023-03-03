using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys
{
    public record HssKeyPair : IHssKeyPair
    {
        public int Levels { get; init; }
        public IHssPrivateKey PrivateKey { get; init; }
        public IHssPublicKey PublicKey { get; init; }

        private bool _isExhausted;
        public bool IsExhausted
        {
            get => _isExhausted;
            set
            {
                if (_isExhausted && !value)
                {
                    throw new InvalidOperationException("Once an HSS key pair has been exhausted, it may not be updated again.");
                }

                _isExhausted = value;
            }
        }
    }
}
