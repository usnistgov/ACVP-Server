using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;

public class DilithiumParameters
{
    public DilithiumParameterSet ParameterSet { get; }
    
    /// <summary>
    /// Modulus.
    /// Valid option is 8380417
    /// </summary>
    public int Q { get; }

    /// <summary>
    /// The number of dropped bits from t.
    /// Valid option is 13
    /// </summary>
    public int D { get; }
    
    /// <summary>
    /// Number of +- 1s in polynomial c.
    /// Valid options are 39, 49, 60
    /// </summary>
    public int Tau { get; }
    
    /// <summary>
    /// The collision strength of cTilde.
    /// Valid options are 128, 192, 256
    /// </summary>
    public int Lambda { get; }
    
    /// <summary>
    /// Coefficient range of y.
    /// Valid options are 2^17 or 2^19
    /// </summary>
    public int Gamma1 { get; }
    
    /// <summary>
    /// Low order rounding range.
    /// Valid options are based on Q, (Q - 1) / 88 or (Q - 1) / 32
    /// </summary>
    public int Gamma2 { get; }

    /// <summary>
    /// (K, L) are the dimensions of A.
    /// Valid options are 4, 6, 8
    /// </summary>
    public int K { get; }

    /// <summary>
    /// (K, L) are the dimensions of A.
    /// Valid options are 4, 5, 7
    /// </summary>
    public int L { get; }

    /// <summary>
    /// Private key range.
    /// Valid options are 2 or 4
    /// </summary>
    public int Eta { get; }
    
    /// <summary>
    /// Tau * Eta.
    /// Valid options are 78, 196, 120
    /// </summary>
    public int Beta { get; }
    
    /// <summary>
    /// The maximum number of 1s in hint h.
    /// Valid options are 80, 55, 75
    /// </summary>
    public int Omega { get; }
    
    public int PrivateKeyLength { get; set; }
    public int PublicKeyLength { get; set; }
    public int SignatureLength { get; set; }
    
    public DilithiumParameters(DilithiumParameterSet param)
    {
        ParameterSet = param;
        switch (param)
        {
            case DilithiumParameterSet.ML_DSA_44:
                Q = 8380417;
                D = 13;
                Tau = 39;
                Lambda = 128;
                Gamma1 = 1 << 17;
                Gamma2 = 95232;
                K = 4;
                L = 4;
                Eta = 2;
                Beta = 78;
                Omega = 80;
                PrivateKeyLength = 2560;
                PublicKeyLength = 1312;
                SignatureLength = 2420;
                break;
            
            case DilithiumParameterSet.ML_DSA_65:
                Q = 8380417;
                D = 13;
                Tau = 49;
                Lambda = 192;
                Gamma1 = 1 << 19;
                Gamma2 = 261888;
                K = 6;
                L = 5;
                Eta = 4;
                Beta = 196;
                Omega = 55;
                PrivateKeyLength = 4032;
                PublicKeyLength = 1952;
                SignatureLength = 3309;
                break;
            
            case DilithiumParameterSet.ML_DSA_87:
                Q = 8380417;
                D = 13;
                Tau = 60;
                Lambda = 256;
                Gamma1 = 1 << 19;
                Gamma2 = 261888;
                K = 8;
                L = 7;
                Eta = 2;
                Beta = 120;
                Omega = 75;
                PrivateKeyLength = 4896;
                PublicKeyLength = 2592;
                SignatureLength = 4627;
                break;
            
            default:
                throw new ArgumentException("Invalid parameter set provided");
        }
    }
}
