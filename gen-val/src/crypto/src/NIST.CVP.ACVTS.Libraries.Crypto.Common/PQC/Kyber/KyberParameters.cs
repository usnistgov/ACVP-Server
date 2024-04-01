using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;

public class KyberParameters
{
    public KyberParameterSet ParameterSet { get; }
    
    /// <summary>
    /// Constant. Only valid value is 256.
    /// </summary>
    public int N { get; }
    
    /// <summary>
    /// Modulus. Only valid value is 3329.
    /// </summary>
    public int Q { get; }
    
    /// <summary>
    /// Dimensions of A and vectors. Valid values are 2, 3, 4.
    /// </summary>
    public int K { get; }
    
    /// <summary>
    /// Distribution for generating vectors. Valid values are 3, 2, 2.
    /// </summary>
    public int Eta1 { get; }
    
    /// <summary>
    /// Distribution for generating vectors. Only valid value is 2.
    /// </summary>
    public int Eta2 { get; }
    
    /// <summary>
    /// Parameters for functions. Valid values are 10, 10, 11.
    /// </summary>
    public int Du { get; }
    
    /// <summary>
    /// Parameters for functions. Valid values are 4, 4, 5.
    /// </summary>
    public int Dv { get; }
    
    /// <summary>
    /// Length in bytes of encapsulation key
    /// </summary>
    public int EncapsulationKeyLength { get; }
    
    /// <summary>
    /// Length in bytes of decapsulation key
    /// </summary>
    public int DecapsulationKeyLength { get; }
    
    /// <summary>
    /// Length in bytes of ciphertext
    /// </summary>
    public int CiphertextLength { get; }

    public KyberParameters(KyberParameterSet param)
    {
        ParameterSet = param;

        switch (param)
        {
            case KyberParameterSet.ML_KEM_512:
                N = 256;
                Q = 3329;
                K = 2;
                Eta1 = 3;
                Eta2 = 2;
                Du = 10;
                Dv = 4;
                EncapsulationKeyLength = 800;
                DecapsulationKeyLength = 1632;
                CiphertextLength = 768;
                break;
            
            case KyberParameterSet.ML_KEM_768:
                N = 256;
                Q = 3329;
                K = 3;
                Eta1 = 2;
                Eta2 = 2;
                Du = 10;
                Dv = 4;
                EncapsulationKeyLength = 1184;
                DecapsulationKeyLength = 2400;
                CiphertextLength = 1088;
                break;
            
            case KyberParameterSet.ML_KEM_1024:
                N = 256;
                Q = 3329;
                K = 4;
                Eta1 = 2;
                Eta2 = 2;
                Du = 11;
                Dv = 5;
                EncapsulationKeyLength = 1568;
                DecapsulationKeyLength = 3168;
                CiphertextLength = 1568;
                break;
            
            default:
                throw new ArgumentException($"Invalid {nameof(KyberParameterSet)} provided");
        }
    }
}
