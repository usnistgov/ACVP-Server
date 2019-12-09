﻿using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfTls12
{
    public class KdfParameterTls12 : IKdfParameter
    {
        public KasKdf KdfType => KasKdf.Tls_v12;
        public bool RequiresAdditionalNoncePair => true;
        public BitString Salt { get; set; }
        public BitString Iv { get; set; }
        public BitString Z { get; set; }
        public int L { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInputEncoding { get; set; }
        public BitString AlgorithmId { get; set; }
        public BitString Label { get; set; }
        public BitString Context { get; set; }
        /// <summary>
        /// The HashFunction used by the KDF.
        /// </summary>
        public HashFunctions HashFunction { get; set; }
        /// <summary>
        /// The ephemeral data (Either a C value or Nonce) from the initiator.
        /// </summary>
        public BitString InitiatorEphemeralData { get; private set; }
        /// <summary>
        /// The ephemeral data (Either a C value or Nonce) from the responder.
        /// </summary>
        public BitString ResponderEphemeralData { get; private set; }
        public BitString AdditionalInitiatorNonce { get; set; }
        public BitString AdditionalResponderNonce { get; set; }
        public KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo)
        {
            return visitor.Kdf(this, fixedInfo);
        }

        public void SetEphemeralData(BitString initiatorData, BitString responderData)
        {
            InitiatorEphemeralData = initiatorData;
            ResponderEphemeralData = responderData;
        }
    }
}