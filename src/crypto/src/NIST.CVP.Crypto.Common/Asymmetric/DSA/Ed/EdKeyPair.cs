using NIST.CVP.Math;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPair : IDsaKeyPair
    {
        public EdPoint PublicQ { get; set; }
        public BigInteger PrivateD { get; set; }
        public BigInteger PublicQEncoded {
            get
            {
                return EdPointEncoder.Encode(PublicQ, VariableB);
            }
            set
            {
                try
                {
                    PublicQ = EdPointEncoder.Decode(value, FieldSizeP, CoefficientA, CoefficientD, VariableB);
                    PublicQOffCurve = false;
                }
                catch (Exception e)
                {
                    PublicQOffCurve = true;
                }
            }
        }

        public bool PublicQOffCurve { get; set; } = false;

        private BigInteger FieldSizeP { get; }
        private BigInteger CoefficientA { get; }
        private BigInteger CoefficientD { get; }
        private int VariableB { get; }

        public EdKeyPair()
        {
            
        }

        public EdKeyPair(EdPoint q, BigInteger d, EdDomainParameters domainParameters)
        {
            FieldSizeP = domainParameters.CurveE.FieldSizeQ;
            CoefficientA = domainParameters.CurveE.CoefficientA;
            CoefficientD = domainParameters.CurveE.CoefficientD;
            VariableB = domainParameters.CurveE.VariableB;

            PublicQ = q;
            PrivateD = d;
        }

        public EdKeyPair(BigInteger q, BigInteger d, EdDomainParameters domainParameters)
        {
            FieldSizeP = domainParameters.CurveE.FieldSizeQ;
            CoefficientA = domainParameters.CurveE.CoefficientA;
            CoefficientD = domainParameters.CurveE.CoefficientD;
            VariableB = domainParameters.CurveE.VariableB;

            PublicQEncoded = q;
            PrivateD = d;
        }

        public EdKeyPair(EdPoint q, EdDomainParameters domainParameters)
        {
            FieldSizeP = domainParameters.CurveE.FieldSizeQ;
            CoefficientA = domainParameters.CurveE.CoefficientA;
            CoefficientD = domainParameters.CurveE.CoefficientD;
            VariableB = domainParameters.CurveE.VariableB;

            PublicQ = q;
        }

        public EdKeyPair(BigInteger q, EdDomainParameters domainParameters)
        {
            FieldSizeP = domainParameters.CurveE.FieldSizeQ;
            CoefficientA = domainParameters.CurveE.CoefficientA;
            CoefficientD = domainParameters.CurveE.CoefficientD;
            VariableB = domainParameters.CurveE.VariableB;

            PublicQEncoded = q;
        }
    }
}