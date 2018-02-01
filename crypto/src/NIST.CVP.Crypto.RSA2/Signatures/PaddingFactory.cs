using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Signatures.Ansx;
using NIST.CVP.Crypto.RSA2.Signatures.Pkcs;
using NIST.CVP.Crypto.RSA2.Signatures.Pss;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.Signatures
{
    public class PaddingFactory : IPaddingFactory
    {
        public IPaddingScheme GetPaddingScheme(SignatureSchemes sigMode, ISha sha, IEntropyProvider entropyProvider = null, int saltLength = 0)
        {
            switch (sigMode)
            {
                case SignatureSchemes.Ansx931:
                    return new AnsxPadder(sha);

                case SignatureSchemes.Pkcs1v15:
                    return new PkcsPadder(sha);

                case SignatureSchemes.Pss:
                    return new PssPadder(sha, entropyProvider, saltLength);

                default:
                    throw new ArgumentException("Invalid signature scheme");
            }
        }

        public IPaddingScheme GetSigningPaddingScheme(SignatureSchemes sigMode, ISha sha, SignatureModifications errors, IEntropyProvider entropyProvider = null, int saltLength = 0)
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
                switch (errors)
                {
                    case SignatureModifications.None:
                        return new PssPadder(sha, entropyProvider, saltLength);

                    case SignatureModifications.E:
                        return new PssPadderWithModifiedPublicExponent(sha, entropyProvider, saltLength);

                    case SignatureModifications.Message:
                        return new PssPadderWithModifiedMessage(sha, entropyProvider, saltLength);

                    case SignatureModifications.ModifyTrailer:
                        return new PssPadderWithModifiedTrailer(sha, entropyProvider, saltLength);

                    case SignatureModifications.MoveIr:
                        return new PssPadderWithMovedIr(sha, entropyProvider, saltLength);

                    case SignatureModifications.Signature:
                        return new PssPadderWithModifiedSignature(sha, entropyProvider, saltLength);

                    default:
                        throw new ArgumentException("Signature modification does not exist for selected scheme");
                }
            }

            throw new ArgumentException("Invalid signature scheme");
        }
    }
}
