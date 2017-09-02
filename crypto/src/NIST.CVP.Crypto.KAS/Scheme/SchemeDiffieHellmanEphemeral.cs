using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class SchemeDiffieHellmanEphemeral : SchemeBase
    {
        private readonly IDiffieHellman<FfcDomainParameters> Dh;

        public SchemeDiffieHellmanEphemeral(
            IDsaFfc dsa, 
            IKdfFactory kdfFactory, 
            IKeyConfirmationFactory keyConfirmationFactory, 
            IOtherInfoFactory otherInfoFactory, 
            KasParameters kasParameters, 
            KdfParameters kdfParameters, 
            MacParameters macParameters,
            IDiffieHellman<FfcDomainParameters> dh
        ) 
            : base(dsa, kdfFactory, keyConfirmationFactory, otherInfoFactory, kasParameters, kdfParameters, macParameters)
        {
            Dh = dh;
        }

        public override FfcScheme FfcScheme => FfcScheme.DhEphem;

        protected override void GenerateKasKeyNonceInformation()
        {
            if (DomainParameters == null)
            {
                GenerateDomainParameters();
            }

            EphemeralKeyPair = Dsa.GenerateKeyPair(DomainParameters);
        }

        protected override IOtherInfo GenerateOtherInformation(FfcSharedInformation otherPartyInformation)
        {
            throw new NotImplementedException();
        }

        protected override BitString ComputeSharedSecret(FfcSharedInformation otherPartyInformation)
        {
            throw new NotImplementedException();
        }

        protected override ComputeKeyMacResult ComputeKeyMac(FfcSharedInformation otherPartyInformation)
        {
            throw new NotImplementedException();
        }
    }
}
