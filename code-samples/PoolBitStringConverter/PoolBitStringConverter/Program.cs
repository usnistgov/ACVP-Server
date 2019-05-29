using System;

namespace PoolBitStringConverter
{
    /// <summary>
    /// Application is used to convert hex string representations of bitStrings into an appropriate object containing both the hex, as well as bitLength.
    /// Currently several poolTypes of data include the hex, but without the bitLength, the hex is interpreted as a full byte.
    ///
    /// This "full byte" works for ***most*** of the pool data, but there are some outliers like:
    ///     AES-CFB1
    ///     TDES-CFB1
    ///     TDES-CFBP1
    ///     AES-CBC-CS1
    ///     AES-CBC-CS2
    ///     AES-CBC-CS3
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
