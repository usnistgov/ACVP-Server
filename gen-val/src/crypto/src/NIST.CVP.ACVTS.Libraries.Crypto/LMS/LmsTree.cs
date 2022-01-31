using System;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS
{
    public class LmsTree
    {
        private readonly BitString D_LEAF = new BitString("8282");
        private readonly BitString D_INTR = new BitString("8383");
        private readonly int _height;
        private readonly int _m;
        private readonly byte[] _tree;

        public LmsTree(int height, int m, byte[] tree)
        {
            _height = height;
            _tree = tree;
            _m = m;
        }

        public byte[] GetTreeBytes()
        {
            return _tree;
        }

        public BitString GetRoot()
        {
            byte[] result = new byte[_m];
            Array.Copy(_tree, 0, result, 0, _m);
            return new BitString(result);
        }

        // Note that in rfc 8554 the first node is 1
        public BitString GetNode(int i)
        {
            byte[] result = new byte[_m];
            Array.Copy(_tree, (i - 1) * _m, result, 0, _m);
            return new BitString(result);
        }

        // Starts leaf numbering at 0
        public BitString GetLeaf(int i)
        {
            byte[] result = new byte[_m];
            Array.Copy(_tree, ((1 << _height) + i - 1) * _m, result, 0, _m);
            return new BitString(result);
        }

        // Note that in rfc 8554 the first node is 1
        public BitString GetSibling(int i)
        {
            if (i == 1)
            {
                return GetRoot();
            }
            if (i % 2 == 0)
            {
                return GetNode(i + 1);
            }
            else
            {
                return GetNode(i - 1);
            }
        }
    }
}
