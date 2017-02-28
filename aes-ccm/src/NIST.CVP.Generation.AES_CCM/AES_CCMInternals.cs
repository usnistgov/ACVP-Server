using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_CCM
{
    public class AES_CCMInternals : IAES_CCMInternals
    {
        public void CCM_format_80211(ref byte[] B, byte[] nonce, int Nlen, byte[] payload, int Plen,
                     byte[] assocData, int Alen, int Tlen, ref byte[] InitCtr, ref int r)
        {
            int t, t_enc, n, p, q, q_val, a, tmp_a, a_octets, u, i;
            int Two8 = 0x100, Two16 = 0x10000;
            byte flags;
            byte[] Q;

            if (Tlen % 16 != 0)
            {
                throw new ArgumentException(nameof(Tlen));
            }

            t = Tlen / 8;

            if ((t < 4) || (t > 16))
            {
                throw new ArgumentException(nameof(Tlen));
            }

            t_enc = (t - 2) / 2;
            
            if (Nlen % 8 != 0)//if expression is non zero goes into it.  Otherwise skips.
                throw new ArgumentException(nameof(Nlen));
            n = Nlen / 8;

            q = 15 - n;
            if ((q < 2) || (q > 8))
            {
                throw new ArgumentException(nameof(q));
            }
            Q = new byte[q];
            p = Plen >> 3;

            q_val = p;
            i = q - 1;
            while (q_val != 0 && (i >= 0))
            {
                Q[i] = (byte) (q_val % 0x100);
                i--;
                q_val >>= 8;
            }

            a = (Alen + 7) / 8;
            if (a == 0)
                a_octets = 0;
            else if (a < (Two16 - Two8))
                a_octets = 2;
            else
                a_octets = 6;

            u = ((a_octets + a) + 15) / 16;

            r = u + (p + 15) / 16;

            //  r has the length of space to encode Adata and payload
            //    + 1 so you can have space for B_0
            B = new byte[(r+1)*16];

            if (Alen == 0)
            {
                flags = 0x00;
            }
            else
            {
                flags = 0x40;
            }

            flags |= (byte)(t_enc << 3);
            flags |= (byte)(q - 1);
            B[0] = flags;

            for (i = 0; i < n; i++)
                B[i + 1] = nonce[i];

            for (i = 0; i < q; i++)
                B[n + 1 + i] = Q[i];

            if (a_octets == 6)
            {
                B[16] = 0xff;
                B[17] = 0xfe;
                tmp_a = a;
                for (i = 3; i >= 0; i--)
                {
                    B[18 + i] = (byte)(tmp_a % 256);
                    tmp_a >>= 8;
                }
            }
            else if (a_octets == 2)
            {   /* a_octets == 2  */
                B[17] = (byte) (a % 256);
                B[16] = (byte) ((a >> 8) % 256);
            }

            /* load the associated data  */
            for (i = 0; i < a; i++)
                B[16 + a_octets + i] = assocData[i];

            for (i = 0; i < p; i++)
                B[16 * (u + 1) + i] = payload[i];

            CCM_counter_80211(nonce, Nlen, ref InitCtr);
        }

        public void CCM_counter_80211(byte[] nonce, int Nlen, ref byte[] InitCtr)
        {
            byte i, n, q, flags;

            if (Nlen % 8 != 0)
            {
                throw new ArgumentException(nameof(Nlen));
            }
            n = (byte) (Nlen / 8);

            q = (byte) (15 - n);
            if ((q < 2) || (q > 8))
            {
                throw new ArgumentException(nameof(q));
            }

            /*  Initialize the Ctr  */
            InitCtr = new byte[16];
            flags = 0;
            flags |= (byte) (q - 1);
            InitCtr[0] = flags;
            for (i = 0; i < n; i++)
            {
                InitCtr[i + 1] = nonce[i];
            }
        }
    }
}
