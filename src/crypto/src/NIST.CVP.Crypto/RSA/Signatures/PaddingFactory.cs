using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.Signatures.Ansx;
using NIST.CVP.Crypto.RSA.Signatures.Pkcs;
using NIST.CVP.Crypto.RSA.Signatures.Pss;
using NIST.CVP.Math.Entropy;
using System;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class PaddingFactory : IPaddingFactory
    {
        private readonly IMaskFactory _maskFactory;

        public PaddingFactory(IMaskFactory maskFactory)
        {
            _maskFactory = maskFactory;
        }
        
        /// Always correct
        public IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, PssMaskTypes maskType = PssMaskTypes.None, IEntropyProvider entropyProvider = null, int saltLength = 0)
        {
            switch (sigMode)
            {
                case SignatureSchemes.Ansx931:
                    return new AnsxPadder(sha);

                case SignatureSchemes.Pkcs1v15:
                    return new PkcsPadder(sha);

                case SignatureSchemes.Pss:
                    var mask = _maskFactory.GetMaskInstance(maskType, sha.HashFunction);
                    return new PssPadder(sha, mask, entropyProvider, saltLength);

                default:
                    throw new ArgumentException("Invalid signature scheme");
            }
        }

        /// Could introduce errors
        public IPaddingScheme GetSigningPaddingScheme(SignatureSchemes sigMode, ISha sha, SignatureModifications errors, PssMaskTypes maskType = PssMaskTypes.None, IEntropyProvider entropyProvider = null, int saltLength = 0)
        {
            if (sigMode == SignatureSchemes.Ansx931)
            {
                switch (errors)
                {
                    case SignatureModifications.None:
                        return new AnsxPadder(sha);

                    case SignatureModifications.E:
                        return new AnsxPadderWithModifiedPublicExponent(sha);

                    case SignatureModifications.Message:
                        return new AnsxPadderWithModifiedMessage(sha);

                    case SignatureModifications.ModifyTrailer:
                        return new AnsxPadderWithModifiedTrailer(sha);

                    case SignatureModifications.MoveIr:
                        return new AnsxPadderWithMovedIr(sha);

                    case SignatureModifications.Signature:
                        return new AnsxPadderWithModifiedSignature(sha);

                    default:
                        throw new ArgumentException("Signature modification does not exist for selected scheme");
                }
            }
            else if (sigMode == SignatureSchemes.Pkcs1v15)
            {
                switch (errors)
                {
                    case SignatureModifications.None:
                        return new PkcsPadder(sha);

                    case SignatureModifications.E:
                        return new PkcsPadderWithModifiedPublicExponent(sha);

                    case SignatureModifications.Message:
                        return new PkcsPadderWithModifiedMessage(sha);

                    case SignatureModifications.ModifyTrailer:
                        return new PkcsPadderWithModifiedTrailer(sha);

                    case SignatureModifications.MoveIr:
                        return new PkcsPadderWithMovedIr(sha);

                    case SignatureModifications.Signature:
                        return new PkcsPadderWithModifiedSignature(sha);

                    default:
                        throw new ArgumentException("Signature modification does not exist for selected scheme");
                }
            }
            else if (sigMode == SignatureSchemes.Pss)
            {
                var mask = _maskFactory.GetMaskInstance(maskType, sha.HashFunction);
                
                switch (errors)
                {
                    case SignatureModifications.None:
                        return new PssPadder(sha, mask, entropyProvider, saltLength);

                    case SignatureModifications.E:
                        return new PssPadderWithModifiedPublicExponent(sha, mask, entropyProvider, saltLength);

                    case SignatureModifications.Message:
                        return new PssPadderWithModifiedMessage(sha, mask, entropyProvider, saltLength);

                    case SignatureModifications.ModifyTrailer:
                        return new PssPadderWithModifiedTrailer(sha, mask, entropyProvider, saltLength);

                    case SignatureModifications.MoveIr:
                        return new PssPadderWithMovedIr(sha, mask, entropyProvider, saltLength);

                    case SignatureModifications.Signature:
                        return new PssPadderWithModifiedSignature(sha, mask, entropyProvider, saltLength);

                    default:
                        throw new ArgumentException("Signature modification does not exist for selected scheme");
                }
            }

            throw new ArgumentException("Invalid signature scheme");
        }
    }
}
