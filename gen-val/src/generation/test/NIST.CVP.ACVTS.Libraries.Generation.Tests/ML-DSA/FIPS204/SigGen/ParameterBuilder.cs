using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigGen;

public class ParameterBuilder
{
    private Parameters _param = new()
    {
        Algorithm = "ML-DSA",
        Mode = "SigGen",
        Revision = "FIPS204",
        PreHash = new[] { PreHash.Pure, PreHash.PreHash },
        Deterministic = new[] { true, false },
        SignatureInterfaces = new[] { SignatureInterface.External, SignatureInterface.Internal },
        ExternalMu = new [] { true, false },
        Capabilities = new[]
        {
            new Capability
            {
                ParameterSets = new[] { DilithiumParameterSet.ML_DSA_44 },
                MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                ContextLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                HashAlgs = new[] { HashFunctions.Sha2_d256, HashFunctions.Sha3_d512, HashFunctions.Shake_d128 }
            }
        }
    };

    public ParameterBuilder WithContextLength(MathDomain contextLength)
    {
        _param.Capabilities.First().ContextLength = contextLength;
        return this;
    }

    public ParameterBuilder WithPreHash(PreHash[] preHashes)
    {
        if (preHashes == null)
            return this;
        
        _param.PreHash = preHashes;
        return this;
    }

    public ParameterBuilder WithSignatureInterfaces(SignatureInterface[] signatureInterfaces)
    {
        if (signatureInterfaces == null)
            return this;
        
        _param.SignatureInterfaces = signatureInterfaces;
        return this;
    }

    public ParameterBuilder WithHashAlgs(HashFunctions[] hashFunctions)
    {
        if (hashFunctions == null)
            return this;
        
        _param.Capabilities.First().HashAlgs = hashFunctions;
        return this;
    }

    public ParameterBuilder WithExternalMu(bool[] externalMu)
    {
        if (externalMu == null)
            return this;

        _param.ExternalMu = externalMu;
        return this;
    }
    
    public Parameters Build()
    {
        return _param;
    }
}
