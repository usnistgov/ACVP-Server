using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Math;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.Ed
{
    public class EdwardsCurve : IEdwardsCurve
    {
        private readonly PrimeFieldOperator _operator;

        public BigInteger CoefficientA { get; }
        public BigInteger CoefficientD { get; }
        public EdPoint BasePointG { get; }
        public BigInteger OrderN { get; }
        public BigInteger FieldSizeQ { get; }

        // Cofactor is always 1 for a prime curve (TODO check for 25519 and Montgomery-448)
        public int CofactorH { get { return 1; } }

        // Coefficients used for EdDSA as specified in IETF RFC 8032
        public int VariableB { get; }
        public int VariableN { get; }
        public int VariableC { get; }

        public Curve CurveName { get; }

        public EdwardsCurve(Curve curveName, BigInteger p, BigInteger a, BigInteger d, EdPoint g, BigInteger n, int b, int dSAn, int c)
        {
            CurveName = curveName;

            FieldSizeQ = p;
            CoefficientA = a;
            CoefficientD = d;
            BasePointG = g;
            OrderN = n;
            VariableB = b;
            VariableN = dSAn;
            VariableC = c;

            _operator = new PrimeFieldOperator(p);
        }

        public EdPoint Add(EdPoint pointA, EdPoint pointB)
        {
            var numeratorX = _operator.Add(_operator.Multiply(pointA.X, pointB.Y), _operator.Multiply(pointA.Y, pointB.X));
            var numeratorY = _operator.Subtract(_operator.Multiply(pointA.Y, pointB.Y), _operator.Multiply(CoefficientA, _operator.Multiply(pointA.X, pointB.X)));
            var denominatorMult = _operator.Multiply(CoefficientD, _operator.Multiply(_operator.Multiply(pointA.X, pointB.X), _operator.Multiply(pointA.Y, pointB.Y)));
            var denominatorX = _operator.Add(1, denominatorMult);
            var denominatorY = _operator.Subtract(1, denominatorMult);

            var x = _operator.Divide(numeratorX, denominatorX);
            var y = _operator.Divide(numeratorY, denominatorY);

            return new EdPoint(x, y);
        }

        /// <summary>
        /// https://eprint.iacr.org/2008/522.pdf uses extended twisted coordinates
        /// </summary>
        private ExtendedEdPoint Add(ExtendedEdPoint pointA, ExtendedEdPoint pointB)
        {
            var A = _operator.Multiply(pointA.X, pointB.X);
            var B = _operator.Multiply(pointA.Y, pointB.Y);
            var C = _operator.Multiply(pointA.Z, pointB.T);
            var D = _operator.Multiply(pointA.T, pointB.Z);
            var E = _operator.Add(D, C);
            var F = _operator.Multiply(_operator.Subtract(pointA.X, pointA.Y), _operator.Add(pointB.X, pointB.Y));
            F = _operator.Subtract(_operator.Add(F, B), A);
            var G = _operator.Add(B, _operator.Multiply(CoefficientA, A));
            var H = _operator.Subtract(D, C);
            var X3 = _operator.Multiply(E, F);
            var Y3 = _operator.Multiply(G, H);
            var T3 = _operator.Multiply(E, H);
            var Z3 = _operator.Multiply(F, G);
            return new ExtendedEdPoint(X3, Y3, T3, Z3);
        }

        public EdPoint Negate(EdPoint point)
        {
            return new EdPoint(_operator.Negate(point.X), point.Y);
        }

        private ExtendedEdPoint Negate(ExtendedEdPoint point)
        {
            return new ExtendedEdPoint(_operator.Negate(point.X), point.Y, _operator.Negate(point.T), point.Z);
        }

        public EdPoint Subtract(EdPoint pointA, EdPoint pointB)
        {
            return Add(pointA, Negate(pointB));
        }

        private ExtendedEdPoint Subtract(ExtendedEdPoint pointA, ExtendedEdPoint pointB)
        {
            return Add(pointA, Negate(pointB));
        }

        public EdPoint Double(EdPoint point)
        {
            return Add(point, point);
        }

        /// <summary>
        /// https://eprint.iacr.org/2008/522.pdf uses extended twisted coordinates
        /// </summary>
        private ExtendedEdPoint Double(ExtendedEdPoint point)
        {
            var A = _operator.Multiply(point.X, point.X);
            var B = _operator.Multiply(point.Y, point.Y);
            var C = _operator.Multiply(2, _operator.Multiply(point.Z, point.Z));
            var D = _operator.Multiply(CoefficientA, A);
            var Esub = _operator.Add(point.X, point.Y);
            var E = _operator.Subtract(_operator.Subtract(_operator.Multiply(Esub, Esub), A), B);
            var G = _operator.Add(D, B);
            var F = _operator.Subtract(G, C);
            var H = _operator.Subtract(D, B);
            var X3 = _operator.Multiply(E, F);
            var Y3 = _operator.Multiply(G, H);
            var T3 = _operator.Multiply(E, H);
            var Z3 = _operator.Multiply(F, G);
            return new ExtendedEdPoint(X3, Y3, T3, Z3);
        }

        private EdPoint Multiply(ExtendedEdPoint startPoint, NonAdjacentBitString nafBs)
        {
            var point = new ExtendedEdPoint(0, 1, 0, 1);
            var naBits = nafBs.Bits;

            for (var i = naBits.Length - 1; i >= 0; i--)
            {
                point = Double(point);
                if (naBits[i] == 1)
                {
                    point = Add(point, startPoint);
                }
                else if (naBits[i] == -1)
                {
                    point = Subtract(point, startPoint);
                }
            }

            return ExtendedToEdPoint(point);
        }

        public EdPoint Multiply(EdPoint startPoint, BigInteger scalar)
        {
            // Find scalar within group and convert to NABS, normal modulo here, not on the field, like CAVS
            return Multiply(EdPointToExtended(startPoint), new NonAdjacentBitString(scalar % OrderN));
        }

        public bool PointExistsOnCurve(EdPoint point)
        {
            // Point is out of bounds
            if (!PointExistsInField(point))
            {
                return false;
            }

            var lhs = _operator.Add(_operator.Multiply(CoefficientA, _operator.Multiply(point.X, point.X)), _operator.Multiply(point.Y, point.Y));
            var rhs = _operator.Add(1, _operator.Multiply(CoefficientD, _operator.Multiply(_operator.Multiply(point.X, point.X), _operator.Multiply(point.Y, point.Y))));

            return (lhs == rhs);
        }

        public bool PointExistsInField(EdPoint point)
        {
            if (point.X < 0 || point.X > FieldSizeQ - 1)
            {
                return false;
            }

            if (point.Y < 0 || point.Y > FieldSizeQ - 1)
            {
                return false;
            }

            return true;
        }

        public BitString Encode(EdPoint point)
        {
            return EdPointEncoder.Encode(point, VariableB);
        }

        public EdPoint Decode(BitString encoded)
        {
            return EdPointEncoder.Decode(encoded, FieldSizeQ, CoefficientA, CoefficientD, VariableB);
        }

        private ExtendedEdPoint EdPointToExtended(EdPoint point)
        {
            return new ExtendedEdPoint(point.X, point.Y, _operator.Multiply(point.X, point.Y), 1);
        }

        private EdPoint ExtendedToEdPoint(ExtendedEdPoint point)
        {
            return new EdPoint(_operator.Divide(point.X, point.Z), _operator.Divide(point.Y, point.Z));
        }
    }
}
