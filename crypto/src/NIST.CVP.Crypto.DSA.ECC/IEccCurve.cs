using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public interface IEccCurve
    {
        /// <summary>
        /// Type of curve, prime or binary
        /// </summary>
        CurveType CurveType { get; }

        /// <summary>
        /// Field size, either a q = prime > 3, or q = 2^m where m is prime. Represents the total number of points
        /// </summary>
        BigInteger FieldSizeQ { get; }

        /// <summary>
        /// Coefficient a
        /// </summary>
        BigInteger CoefficientA { get; }

        /// <summary>
        /// Coefficient b
        /// </summary>
        BigInteger CoefficientB { get; }

        /// <summary>
        /// Base point of the curve
        /// </summary>
        EccPoint BasePointG { get; }

        /// <summary>
        /// Cofactor of the curve, a curve has h * <see cref="OrderN"/> possible points. Value is usually 1, 2, or 4.
        /// </summary>
        int CofactorH { get; }

        /// <summary>
        /// Order of <see cref="BasePointG"/> which is a large prime
        /// </summary>
        BigInteger OrderN { get; }

        /// <summary>
        /// Multiply a point by a scalar value over a curve
        /// </summary>
        EccPoint Multiply(EccPoint startPoint, BigInteger scalar);

        /// <summary>
        /// Multiply a point by an non-adjacent form scalar over a curve
        /// </summary>
        //EccPoint Multiply(EccPoint startPoint, NonAdjacentBitString nafBs);

        /// <summary>
        /// Double a point over a curve
        /// </summary>
        EccPoint Double(EccPoint point);

        /// <summary>
        /// Add two points together over a curve, a + b
        /// </summary>
        EccPoint Add(EccPoint pointA, EccPoint pointB);

        /// <summary>
        /// Subtracts two points over a curve, a - b
        /// </summary>
        EccPoint Subtract(EccPoint pointA, EccPoint pointB);

        /// <summary>
        /// Negates a point, a => -a
        /// </summary>
        EccPoint Negate(EccPoint point);

        /// <summary>
        /// Determines if a given point exists on the curve
        /// </summary>
        bool PointExistsOnCurve(EccPoint point);

        /// <summary>
        /// Determines if a given point exists in the field
        /// </summary>
        bool PointExistsInField(EccPoint point);
    }
}
