using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class LmsPrivateKey
    {
        public int Q { get; set; } = 0;
        public byte[] OTS_PRIV { get; set; }
        /// <summary>
        /// The expensive part of generating an lms tree. Possibly saved for fast recovery.
        /// </summary>
        public byte[] Pub { get; set; } = null;
        private readonly int _n;
        private readonly int _p;

        public LmsPrivateKey(byte[] ots_priv, int n, int p)
        {
            OTS_PRIV = (byte[])ots_priv.Clone();
            _n = n;
            _p = p;
        }

        public LmsPrivateKey(byte[] ots_priv, byte[] pub, int n, int p)
        {
            OTS_PRIV = (byte[])ots_priv.Clone();
            Pub = (byte[])pub.Clone();
            _n = n;
            _p = p;
        }

        private LmsPrivateKey(byte[] ots_priv, byte[] pub, int q, int n, int p)
        {
            Q = q;
            OTS_PRIV = (byte[])ots_priv.Clone();
            Pub = (byte[])pub.Clone();
            _n = n;
            _p = p;
        }

        public byte[] GetLmotsPrivateKeyQ(int q)
        {
            var result = new byte[(_n * _p) + 24];
            Array.Copy(OTS_PRIV, q * ((_n * _p) + 24), result, 0, (_n * _p) + 24);
            return result;
        }

        public LmsPrivateKey GetDeepCopy()
        {
            var copy_ots = (byte[])OTS_PRIV.Clone();
            var copy_pub = (byte[])Pub.Clone();
            return new LmsPrivateKey(copy_ots, copy_pub, Q, _n, _p);
        }
    }
}
