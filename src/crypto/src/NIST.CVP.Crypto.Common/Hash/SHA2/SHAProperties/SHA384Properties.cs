namespace NIST.CVP.Crypto.Common.Hash.SHA2.SHAProperties
{
    public class SHA384Properties : SHAPropertiesBase
    {
        public override int AppendedLength => 128;
        public override int BlockSize => 1024;
        public override int DigestBitSize => 384;
        public override int Rounds => 80;
        public override int WordSize => 64;
        public override int[,] SumShifts => new int[,] {{28, 34, 39}, {14, 18, 41}};
        public override int[,] SigmaShifts => new int[,] {{1, 8, 7}, {19, 61, 6}};
        public override string[] HValues => new [] { "cbbb9d5dc1059ed8", "629a292a367cd507", "9159015a3070dd17", "152fecd8f70e5939",
                                                    "67332667ffc00b31", "8eb44a8768581511", "db0c2e0d64f98fa7", "47b5481dbefa4fa4" };

        public override string[] KValues => new string[] { "428a2f98d728ae22", "7137449123ef65cd", "b5c0fbcfec4d3b2f", "e9b5dba58189dbbc", "3956c25bf348b538",
                                                    "59f111f1b605d019", "923f82a4af194f9b", "ab1c5ed5da6d8118", "d807aa98a3030242", "12835b0145706fbe",
                                                    "243185be4ee4b28c", "550c7dc3d5ffb4e2", "72be5d74f27b896f", "80deb1fe3b1696b1", "9bdc06a725c71235",
                                                    "c19bf174cf692694", "e49b69c19ef14ad2", "efbe4786384f25e3", "0fc19dc68b8cd5b5", "240ca1cc77ac9c65",
                                                    "2de92c6f592b0275", "4a7484aa6ea6e483", "5cb0a9dcbd41fbd4", "76f988da831153b5", "983e5152ee66dfab",
                                                    "a831c66d2db43210", "b00327c898fb213f", "bf597fc7beef0ee4", "c6e00bf33da88fc2", "d5a79147930aa725",
                                                    "06ca6351e003826f", "142929670a0e6e70", "27b70a8546d22ffc", "2e1b21385c26c926", "4d2c6dfc5ac42aed",
                                                    "53380d139d95b3df", "650a73548baf63de", "766a0abb3c77b2a8", "81c2c92e47edaee6", "92722c851482353b",
                                                    "a2bfe8a14cf10364", "a81a664bbc423001", "c24b8b70d0f89791", "c76c51a30654be30", "d192e819d6ef5218",
                                                    "d69906245565a910", "f40e35855771202a", "106aa07032bbd1b8", "19a4c116b8d2d0c8", "1e376c085141ab53",
                                                    "2748774cdf8eeb99", "34b0bcb5e19b48a8", "391c0cb3c5c95a63", "4ed8aa4ae3418acb", "5b9cca4f7763e373",
                                                    "682e6ff3d6b2b8a3", "748f82ee5defb2fc", "78a5636f43172f60", "84c87814a1f0ab72", "8cc702081a6439ec",
                                                    "90befffa23631e28", "a4506cebde82bde9", "bef9a3f7b2c67915", "c67178f2e372532b", "ca273eceea26619c",
                                                    "d186b8c721c0c207", "eada7dd6cde0eb1e", "f57d4f7fee6ed178", "06f067aa72176fba", "0a637dc5a2c898a6",
                                                    "113f9804bef90dae", "1b710b35131c471b", "28db77f523047d84", "32caab7b40c72493", "3c9ebe0a15c9bebc",
                                                    "431d67c49c100d4c", "4cc5d4becb3e42b6", "597f299cfc657e2a", "5fcb6fab3ad6faec", "6c44198c4a475817" };
    }
}
