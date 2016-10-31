using System;
using NLog;

namespace NIST.CVP.Math
{
    public class Array3D
    {
        private byte[,,] _internal;

        public Array3D(byte[,,] supply)
        {
            _internal = supply;
        }

        public Array3D(int dimension1Size, int dimension2Size, int dimension3Size)
        {
            _internal = new byte[dimension1Size, dimension2Size, dimension3Size];
        }

        public int Dimension1Size
        {
            get { return _internal.GetLength(0); }
        }

        public int Dimension2Size
        {
            get { return _internal.GetLength(1); }
        }

        public int Dimension3Size
        {
            get { return _internal.GetLength(2); }
        }

        public byte[,,] Array
        {
            get { return _internal; }
        }

        public Array2D GetSubArray(int dimension1Index)
        {

            if (dimension1Index >= _internal.GetLength(0) || dimension1Index < 0)
            {
                throw new ArgumentException("Dimension Index out of bounds", nameof(dimension1Index));
            }
            var subArray = new byte[Dimension2Size, Dimension3Size];
            for (int j = 0; j < Dimension2Size; j++)
            {
                for (int k = 0; k < Dimension3Size; k++)
                {
                    subArray[j, k] = Array[dimension1Index, j, k];
                }
            }
            return new Array2D(subArray);
        }

        public static byte[,] GetSubArray(byte[,,] source, int dimension1Index)
        {
            var array = new Array3D(source);
            try
            {
                var subArray = array.GetSubArray(dimension1Index);
                return subArray.Array;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
           
        }

        private static Logger Logger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}
