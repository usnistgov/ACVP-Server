using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public class AlgoArrayResponse : ICryptoResult
    {
        [JsonIgnore]
        public BitString Keys { get; set; }
        
        public BitString IV { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        public int? CipherTextLength => CipherText?.BitLength;
        public int? PlainTextLength => PlainText?.BitLength;

        public AlgoArrayResponse() { }

        public AlgoArrayResponse(string key, string plaintext, string ciphertext)
        {
            Keys = new BitString(key);
            PlainText = new BitString(plaintext);
            CipherText = new BitString(ciphertext);
        }

        public AlgoArrayResponse(string key, string iv, string plaintext, string ciphertext)
        {
            Keys = new BitString(key);
            IV = new BitString(iv);
            PlainText = new BitString(plaintext);
            CipherText = new BitString(ciphertext);
        }

        public BitString Key1
        {
            get => Keys?.MSBSubstring(0, 64);
            set => Keys = Keys == null ?
                value.ConcatenateBits(new BitString(128)) :
                value.ConcatenateBits(Keys.MSBSubstring(64, 128));
        }

        public BitString Key2
        {
            get
            {
                if (Keys == null) return null;
                if (Keys.BitLength == 64) return Key1;
                return Keys.MSBSubstring(64, 64);
            }
            set => Keys = Keys == null ?
                new BitString(64).ConcatenateBits(value).ConcatenateBits(new BitString(64)) :
                Keys.MSBSubstring(0, 64).ConcatenateBits(value).ConcatenateBits(Keys.MSBSubstring(128, 64));
        }

        public BitString Key3
        {
            get
            {
                if (Keys == null) return null;
                if (Keys.BitLength == 64) return Key1;
                if (Keys.BitLength == 128) return Key1;
                return Keys.MSBSubstring(128, 64);
            }
            set => Keys = Keys == null ?
                new BitString(128).ConcatenateBits(value) :
                Keys.MSBSubstring(0, 128).ConcatenateBits(value);
        }
    }
}