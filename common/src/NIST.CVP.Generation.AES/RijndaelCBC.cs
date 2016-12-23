using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.LayoutRenderers;

namespace NIST.CVP.Generation.AES
{
    public class RijndaelCBC : Rijndael
    {
        protected readonly IRijndaelInternals _iRijndaelInternals;

        public RijndaelCBC(IRijndaelInternals iRijndaelInternals)
            : base(iRijndaelInternals) { }

        protected override void BlockEncryptWorker(Cipher cipher, Key key, byte[] plainText, int numBlocks, byte[,] block, byte[] outBuffer)
        {
            throw new NotImplementedException();
        }
    }
}
