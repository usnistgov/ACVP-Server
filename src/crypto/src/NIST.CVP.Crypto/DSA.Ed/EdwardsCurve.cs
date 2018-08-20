using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class EdwardsCurve : IEdwardsCurve
    {
        private readonly PrimeFieldOperator _operator;
        
        public BigInteger CoefficientA { get; }
        public BigInteger CoefficientD { get; }
        public EdPoint BasePointG { get; }
        public BigInteger OrderN { get; }
        public BigInteger FieldSizeQ { get; }

        // Cofactor is always 1 for a prime curve
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

        public EdPoint Negate(EdPoint point)
        {
            return new EdPoint(_operator.Negate(point.X), point.Y);
        }

        public EdPoint Subtract(EdPoint pointA, EdPoint pointB)
        {
            return Add(pointA, Negate(pointB));
        }

        public EdPoint Double(EdPoint point)
        {
            return Add(point, point);
        }

        private EdPoint Multiply(EdPoint startPoint, NonAdjacentBitString nafBs)
        {
            var point = new EdPoint(0 , 1);
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

            return point;
        }

        public EdPoint Multiply(EdPoint startPoint, BigInteger scalar)
        {
            // Find scalar within group and convert to NABS, normal modulo here, not on the field, like CAVS
            return Multiply(startPoint, new NonAdjacentBitString(scalar % OrderN));
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

        // Prime Field Operator for use in Edwards Curves
        private class PrimeFieldOperator
        {
            private readonly BigInteger _m;

            public PrimeFieldOperator(BigInteger modulo)
            {
                _m = modulo;
            }

            public BigInteger Add(BigInteger a, BigInteger b)
            {
                return Modulo(a + b);
            }

            public BigInteger Divide(BigInteger a, BigInteger b)
            {
                return Multiply(a, Inverse(b));
            }

            public BigInteger Negate(BigInteger a)
            {
                return Modulo(_m - a);
            }

            public BigInteger Inverse(BigInteger a)
            {
                return a.ModularInverse(_m);
            }

            public BigInteger Modulo(BigInteger a)
            {
                return a.PosMod(_m);
            }

            public BigInteger Multiply(BigInteger a, BigInteger b)
            {
                return Modulo(a * b);
            }

            public BigInteger Subtract(BigInteger a, BigInteger b)
            {
                return Modulo(a - b);
            }
        }
    }
}
