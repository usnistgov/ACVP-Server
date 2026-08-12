using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.tr1.SigGen;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.tr1.SigGen;

public class ParameterBuilder
{
    private Parameters _param = new()
    {
        Algorithm = "ML-DSA",
        Mode = "SigGen",
        Revision = "FIPS204-tr1",
        PreHash = [PreHash.Pure, PreHash.PreHash],
        Deterministic = [true, false],
        SignatureInterfaces = [SignatureInterface.External, SignatureInterface.Internal],
        ExternalMu = [true, false],
        Capabilities =
        [
            new Capability
            {
                ParameterSets = [DilithiumParameterSet.ML_DSA_44],
                MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                ContextLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                HashAlgs = [HashFunctions.Sha2_d256, HashFunctions.Sha3_d512, HashFunctions.Shake_d128]
            }
        ],
        KeyFormats = [PrivateKeyFormat.Seed, PrivateKeyFormat.Expanded]
    };

    public ParameterBuilder WithContextLength(MathDomain contextLength)
    {
        _param.Capabilities.First().ContextLength = contextLength;
        return this;
    }

    public ParameterBuilder WithPreHash(PreHash[] preHashes)
    {
        _param.PreHash = preHashes;
        return this;
    }

    public ParameterBuilder WithSignatureInterfaces(SignatureInterface[] signatureInterfaces)
    {
        _param.SignatureInterfaces = signatureInterfaces;
        return this;
    }

    public ParameterBuilder WithHashAlgs(HashFunctions[] hashFunctions)
    {
        _param.Capabilities.First().HashAlgs = hashFunctions;
        return this;
    }

    public ParameterBuilder WithKeyFormat(PrivateKeyFormat[] keyFormats)
    {
        _param.KeyFormats = keyFormats;
        return this;
    }
    
    public ParameterBuilder WithExternalMu(bool[] externalMu)
    {
        _param.ExternalMu = externalMu;
        return this;
    }
    
    public Parameters Build()
    {
        return _param;
    }
}
