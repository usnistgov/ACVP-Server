using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers
{
    public static class SlhdsaHelpers
    {
        /// <summary>
        /// Algorithm 1 from FIPS 205. Converts the byte string x of length n to a ulong integer.
        ///
        /// Note: because the ulong return type is a 64-bit unsigned integer, n must be lte 8; 8 is the maximum supported
        /// value for n supported by this implementation of ToInt().
        /// </summary>
        /// <param name="x">The byte string/array to convert to an integer</param>
        /// <param name="n">The length in bytes of the byte string/array <see cref="x"/></param>
        /// <returns>The integer value of x</returns>
        public static ulong ToInt(byte[] x, int n)
        {
            /* 1: total ← 0
               2:
               3: for i from 0 to n − 1 do
               4:   total ← 256·total + X[i]
               5: end for
               6: return total */
            if (n > 8 || x.Length > 8)
                throw new ArgumentException($"{nameof(n)} ({n}) should be set to the length of {nameof(x)} ({x}) and must be <= 8.");
            
            // 1: total ← 0
            ulong total = 0;
            
            // 3: for i from 0 to n − 1 do
            for (int i = 0; i < n; i++)
            {
                // 4: total ← 256·total + X[i]
                total = 256 * total + (ulong) Convert.ToInt32(x[i]);
            }

            // 6: return total
            return total;
        }
        
        /// <summary>
        /// Algorithm 2 from FIPS 205. Converts the integer x to a byte string/array of length n.  
        /// </summary>
        /// <param name="x">The integer to convert to a byte string. NOTE: as of implementing the XMSS layer, ToByte()
        /// will never be asked to handle an x that is larger than 512. As such, an int type (-2,147,483,648 to 2,147,483,647)
        /// should be fine for x's parameter type.</param>
        /// <param name="n">How long the resulting byte string should be.</param>
        /// <returns>The binary representation of x in big-endian byte order.</returns>
        public static byte[] ToByte(ulong x, int n)
        {
            /*
            total ← x
            
            for i from 0 to n−1 do
                S[n − 1 − i] ← total mod 256 
                total ← total ≫ 8
            end for 
            
            return S         
             */
            var s = new byte[n];
            ulong total = x;

            for (var i = 0; i < n; i++)
            {
                s[n - 1 - i] = Convert.ToByte(total % 256);
                total = total >> 8;
            }

            return s;
        }

        /// <summary>
        /// Algorithm 3 from FIPS 205. Converts the byte string x its base 2^b representation. 
        /// </summary>
        /// <param name="x">the byte string to convert</param>
        /// <param name="b">the value of b used to compute 2^b</param>
        /// <param name="outLen">The size of the output array. x will be considered as a string of outLen 2^b values.
        /// The number of b-bit segments to break x into. </param>
        /// <returns>integer array of outLen 2^b values. </returns>
        public static int[] Base2B(byte[] x, int b, int outLen)
        {
            // Requirement: byte string x must be of length at least ceiling(out_len·b/8)
            var ceiling = (int) System.Math.Ceiling((double)outLen * b / 8);
            if (x.Length < ceiling)
                throw new ArgumentException($"The length of {nameof(x)} is less than ceiling(out_len * b / 8).");
            /* 
               in ← 0
               bits ← 0
               total ← 0
               
               for out from 0 to out_len−1 do 
                   while bits < b do
                       total ← (total ≪ 8) + X[in] 
                       in←in+1
                       bits←bits+8
                   end while
                   bits←bits−b
                   baseb[out] ← (total ≫ bits) mod 2^b 
               end for
               return baseb
             */
            var baseB = new int[outLen];
            int i = 0;
            int bits = 0;
            BigInteger total = 0;
            
            for (int o = 0; o < outLen; o++)
            {
                while (bits < b)
                {
                    total = (total << 8) + Convert.ToInt32(x[i]);
                    i++;
                    bits += 8;
                }
                bits -= b;
                baseB[o] = (int)((total >> bits) % BigInteger.Pow(2, b));
            }

            return baseB;
        }
        
        /// <summary>
        /// Algorithm 4 from FIPS 205. Performs the chaining function (particular to WOTS+) described in Section 5 of the FIPS.
        /// </summary>
        /// <param name="x">The n-byte string that the chaining will be performed on</param>
        /// <param name="i">the index at which the iteration will begin </param>
        /// <param name="s">the number of times F() will be iterated on the input</param>
        /// <param name="pkSeed">PK.seed (part of the SLH-DSA public key)</param>
        /// <param name="adrs">An ADRS of type WOTS_HASH, i.e., a WotsHashAdrs. The layer address, tree address, key pair address,
        /// and chain address components of ADRS identify the chain being computed. </param>
        /// <param name="slhdsaParameterSetAttributes">the parameter set in use. See Table 1 of the FIPS</param>
        /// <param name="f">An implementation of F(). F() is defined in Sections 10.1, 10.2, and 10.3 of the FIPS. F() is defined for
        /// SHAKE and for SHA2. When the parameter set in use calls for SHA2, F() has 2 definitions: one for use in Security
        /// Category 1 parameter sets; and a second for use in Security Category 3 and 5 parameter sets.</param>
        /// <returns></returns>
        public static byte[] Chain(byte[] x, int i, int s, byte[] pkSeed, WotsHashAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes, IFHOrT f)
        {
            /*
               if (i+s) ≥ w then return NULL
               
               tmp ← X
               for j from i to i + s−1 do
                 ADRS.setHashAddress(j)
                 tmp ← F(PK.seed,ADRS,tmp)
               end for 
               
               return tmp            
            */

            // Requirement: (i+s) ≥ w must result in an error condition
            if (i + s >= slhdsaParameterSetAttributes.W) 
                throw new ArgumentException($"{nameof(i)} + {nameof(s)} ≥ {nameof(slhdsaParameterSetAttributes.W)}");
            
            // it also follows that 0 <= i < w and 0 <= s < w 
            if (i < 0 || i > slhdsaParameterSetAttributes.W-1) 
                throw new ArgumentException($"{nameof(i)} must be ≥ 0 and < w, but {nameof(i)} = {i}");
            if (s < 0 || s > slhdsaParameterSetAttributes.W-1) 
                throw new ArgumentException($"{nameof(s)} must be ≥ 0 and < w, but {nameof(s)} = {s}");
            
            // no need to create a separate byte[] tmp. just do our work using the x that was passed in.
            // X is an n-byte string. F() takes as input the n-byte string M and also returns an n-byte string.
            // n is part of the parameter set definition; see FIPS 205 Section 10, Table 1. 
            //var tmp = new byte[x.Length];
            // tmp ← X
            //x.CopyTo(tmp, 0);
            
            // grab an instance of F() to use
            // IFHOrT f = FHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.F);
            
            for (int j = i; j < i + s; j++)
            {
                // ADRS.setHashAddress(j)
                adrs.HashAddress = ToByte((ulong)j, adrs.HashAddress.Length);
                // tmp ← F(PK.seed,ADRS,tmp)
                x = f.Hash(pkSeed, adrs, x);
            }

            return x;
        }     
        
        
    }
}
