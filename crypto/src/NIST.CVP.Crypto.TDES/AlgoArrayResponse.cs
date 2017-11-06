using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public class AlgoArrayResponse
    {
        public BitString Keys { get; set; }
        public BitString IV { get; set; }
        private BitString _plainText;
        public BitString PlainText
        {
            get => _plainText;

            set
            {
                _plainText = value;
                if (_plainText != null && (_plainText.BitLength == 0 || _plainText.BitLength % 8 != 0))
                {
                    PlainTextLength = _plainText.BitLength;
                }
            }
        }
        public int? PlainTextLength { get; private set; }
        private BitString _cipherText;
        public BitString CipherText
        {
            get => _cipherText;

            set
            {
                _cipherText = value;
                if (_cipherText != null && (_cipherText.BitLength == 0 || _cipherText.BitLength % 8 != 0))
                {
                    CipherTextLength = _cipherText.BitLength;
                }
            }
        }
        public int? CipherTextLength { get; private set; }

        public BitString Key1 { get; set; }
        public BitString Key2 { get; set; }
        public BitString Key3 { get; set; }
    }
}