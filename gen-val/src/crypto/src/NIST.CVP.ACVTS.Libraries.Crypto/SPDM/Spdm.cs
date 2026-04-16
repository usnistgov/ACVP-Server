using System.Text;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SPDM;

public class Spdm
{
    private readonly IHkdf _hkdf;
    private readonly HashFunction _hash;

    public Spdm(HashFunction hashType)
    {
        _hash = hashType;
        
        var hkdfFactory = new HkdfFactory(new HmacFactory(new NativeShaFactory()));
        _hkdf = hkdfFactory.GetKdf(hashType);
    }

    public SPDMReturn KeySchedule(BitString key, bool psk, SPDMVersions version, BitString TH1 = null, BitString TH2 = null)
    {
        var hashLenBytes = _hash.OutputLen / 8;
        SPDMReturn result = new SPDMReturn();
        BitString salt_0;
        if (psk)
        {
            salt_0 = BitString.Ones(hashLenBytes * 8);
        }
        else
        {
            salt_0 = BitString.Zeroes(hashLenBytes * 8);
        }

        BitString handshakeSecret = _hkdf.Extract(salt_0, key);
        var versionString = EnumHelpers.GetEnumDescriptionFromEnum(version);
        
        BitString bin_str1 = BinConcat(hashLenBytes, versionString, "req hs data", TH1);
        result.RequestDirectionHandshake = _hkdf.Expand(handshakeSecret, bin_str1, hashLenBytes);

        BitString bin_str2 = BinConcat(hashLenBytes, versionString, "rsp hs data", TH1);
        result.ResponseDirectionHandshake = _hkdf.Expand(handshakeSecret, bin_str2, hashLenBytes);

        BitString bin_str0 = BinConcat(hashLenBytes, versionString, "derived");
        BitString salt_1 = _hkdf.Expand(handshakeSecret, bin_str0, hashLenBytes);

        BitString masterSecret = _hkdf.Extract(salt_1, BitString.Zeroes(hashLenBytes * 8));

        BitString bin_str3 = BinConcat(hashLenBytes, versionString, "req app data", TH2);
        result.RequestDirectionData = _hkdf.Expand(masterSecret, bin_str3, hashLenBytes);

        BitString bin_str4 = BinConcat(hashLenBytes, versionString, "rsp app data", TH2);
        result.ResponseDirectionData = _hkdf.Expand(masterSecret, bin_str4, hashLenBytes);

        //Not sure if included
        BitString bin_str8 = BinConcat(hashLenBytes, versionString, "exp master", TH2);
        result.ExportMaster = _hkdf.Expand(masterSecret, bin_str8, hashLenBytes);

        return result;
    }

    private BitString BinConcat(int length, string version, string label, BitString context = null)
    {
        BitString output = new BitString(0);
        output = output.ConcatenateBits(BitString.ReverseByteOrder(BitString.To16BitString((short) length)));
        output = output.ConcatenateBits(new BitString(Encoding.ASCII.GetBytes(version + " ")));         // Always a space after the version
        output = output.ConcatenateBits(new BitString(Encoding.ASCII.GetBytes(label)));
        if (context != null)
        {
            output = output.ConcatenateBits(context);
        }

        return output;
    }
}
