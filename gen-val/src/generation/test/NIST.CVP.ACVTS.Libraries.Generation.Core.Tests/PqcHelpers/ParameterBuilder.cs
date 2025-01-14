using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.PqcHelpers;

public class ParameterBuilder
{
    private FakeParameters _param = new()
    {
        Algorithm = "Fake-DSA",
        Mode = "",
        Revision = "",
        PreHash = new[] { PreHash.Pure, PreHash.PreHash },
        SignatureInterfaces = new[] { SignatureInterface.External, SignatureInterface.Internal },
        Capabilities = new[]
        {
            new FakeCapability
            {
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
    
    public FakeParameters Build()
    {
        return _param;
    }
}
