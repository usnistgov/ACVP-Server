using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;
using XmssSignatureResult = NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.XmssSignatureResult;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xmss;

public class OracleObserverXmssVerifyCaseGrain : ObservableOracleGrainBase<VerifyResult<XmssSignatureResult>>, IOracleObserverXmssVerifyCaseGrain
{
    private XmssSignatureParameters _param;
    private readonly IRandom800_90 _rand;
    private readonly IXmssSigner _xmssSigner;

    public OracleObserverXmssVerifyCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IXmssSigner xmssSigner,
        IRandom800_90 rand) : base(nonOrleansScheduler)
    {
        _rand = rand;
        _xmssSigner = xmssSigner;
    }

    public async Task<bool> BeginWorkAsync(XmssSignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var message = _rand.GetRandomBitString(_param.MessageLength);

        // We don't actually track the state of the tree, so use what the gen/vals tells us which should be unique
        _param.XmssKeyPair.PrivateKey.SetIdx(_param.Idx);

        var signature = _xmssSigner.Sign(_param.XmssKeyPair.PrivateKey, message.ToBytes());
        var result = new XmssSignatureResult
        {
            Message = message,
            Signature = new BitString(signature.Signature)
        };

        switch (_param.Disposition)
        {
            case XmssSignatureDisposition.None:
                break;

            case XmssSignatureDisposition.ModifyIndex:
                /* XMSS signatures carry no parameter set identifier, so the closest analogue to a
                   header modification is pointing idx_sig at a different leaf: the signed message
                   digest and authentication path no longer correspond to the encoded index. */
                var attribute = AttributesHelper.GetXmssAttribute(_param.XmssMode);
                var oldSignature = result.Signature.ToBytes();
                var newIdx = (int)((_param.Idx + 1) % (1L << attribute.H));

                var newSignature = new byte[oldSignature.Length];
                Array.Copy(oldSignature, newSignature, oldSignature.Length);
                Array.Copy(newIdx.GetBytes(), 0, newSignature, 0, 4);
                result.Signature = new BitString(newSignature);

                break;

            case XmssSignatureDisposition.ModifyMessage:
                // Flip the last bit in the message, Bits is Lsb
                result.Message.Bits.Set(0, !result.Message.Bits.Get(0));
                break;

            case XmssSignatureDisposition.ModifySignature:
                // Flip the last bit in the signature, Bits is Lsb
                result.Signature.Bits.Set(0, !result.Signature.Bits.Get(0));
                break;
        }

        // Notify observers of result
        await Notify(new VerifyResult<XmssSignatureResult>
        {
            Result = _param.Disposition == XmssSignatureDisposition.None,
            VerifiedValue = result
        });
    }
}
