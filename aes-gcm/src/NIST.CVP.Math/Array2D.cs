using System;

namespace NIST.CVP.Math
{
    public class Array2D
    {
        private byte[,] _internal;


        public Array2D(byte[,] supply)
        {
            _internal = supply;
        }

        public Array2D(int dimension1Size, int dimension2Size)
        {
            _internal = new byte[dimension1Size, dimension2Size];
        }

        public byte[,] Array
        {
            get { return _internal; }
        }

        public int Dimension1Size
        {
            get { return _internal.GetLength(0); }
        }

        public int Dimension2Size
        {
            get { return _internal.GetLength(1); }
        }

    }
}
