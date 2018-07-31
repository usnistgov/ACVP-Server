using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IEdwardsCurve
    {
        /// <summary>
        /// Type of curve, prime or binary
        /// </summary>
        CurveType CurveType { get; }

        /// <summary>
        /// Name of the curve as an enum
        /// </summary>
        Curve CurveName { get; }

        /// <summary>
        /// Field size, either a q = prime > 3, or q = 2^m where m is prime. Represents the total number of points
        /// </summary>
        BigInteger FieldSizeQ { get; }

        /// <summary>
        /// Coefficient a
        /// </summary>
        BigInteger CoefficientA { get; }

        /// <summary>
        /// Coefficient d
        /// </summary>
        BigInteger CoefficientD { get; }

        /// <summary>
        /// Base point of the curve
        /// </summary>
        EdPoint BasePointG { get; }

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
        EdPoint Multiply(EdPoint startPoint, BigInteger scalar);

        /// <summary>
        /// Double a point over a curve
        /// </summary>
        EdPoint Double(EdPoint point);

        /// <summary>
        /// Add two points together over a curve, a + b
        /// </summary>
        EdPoint Add(EdPoint pointA, EdPoint pointB);

        /// <summary>
        /// Subtracts two points over a curve, a - b
        /// </summary>
        EdPoint Subtract(EdPoint pointA, EdPoint pointB);

        /// <summary>
        /// Negates a point, a => -a
        /// </summary>
        EdPoint Negate(EdPoint point);

        /// <summary>
        /// Determines if a given point exists on the curve
        /// </summary>
        bool PointExistsOnCurve(EdPoint point);

        /// <summary>
        /// Determines if a given point exists in the field
        /// </summary>
        bool PointExistsInField(EdPoint point);
    }
}
