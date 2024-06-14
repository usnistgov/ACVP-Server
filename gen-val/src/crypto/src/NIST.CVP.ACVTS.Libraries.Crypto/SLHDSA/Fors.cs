using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.ADRS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA.Helpers.HashAndPseudorandomFunctions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;

public class Fors : IFors
{
    private readonly IShaFactory _shaFactory;

    public Fors(IShaFactory shaFactory)
    {
        _shaFactory = shaFactory;
    }
    
    public byte[] ForsSkGen(byte[] skSeed, byte[] pkSeed, ForsTreeAdrs adrs, int idx,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: skADRS ← ADRS     ▷ Copy address to create key generation address
           2: skADRS.setTypeAndClear(FORS_PRF)
           3: skADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
           4: skADRS.setTreeIndex(idx)
           5: return PRF(PK.seed, SK.seed, skADRS) */

        var prf = new PrfFactory(_shaFactory).GetPrf(slhdsaParameterSetAttributes);
        
        // 1: skADRS ← ADRS     ▷ Copy address to create key generation address
        // 2: skADRS.setTypeAndClear(FORS_PRF)
        var skAdrs = new ForsPrfAdrs(adrs.TreeAddress);
        // 3: skADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
        adrs.KeyPairAddress.CopyTo(skAdrs.KeyPairAddress, 0);
        // 4: skADRS.setTreeIndex(idx)
        skAdrs.TreeIndex = SlhdsaHelpers.ToByte((ulong)idx, skAdrs.TreeIndex.Length);
        // 5: return PRF(PK.seed, SK.seed, skADRS)
        return prf.GetPseudorandomByteString(pkSeed, skSeed, skAdrs);
    }
    
    public byte[] ForsNode(byte[] skSeed, byte[] pkSeed, int i, int z, ForsTreeAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: if z > a or i ≥ k*2^(a−z) then 
           2:   return NULL
           3: endif 
           4: if z = 0 then
           5:   sk ← fors_SKgen(SK.seed, PK.seed, ADRS, i)
           6:   ADRS.setTreeHeight(0) 
           7:   ADRS.setTreeIndex(i)
           8:   node ← F(PK.seed, ADRS, sk)
           9: else
           10:  lnode ←  fors_node(SK.seed, 2i, z − 1, PK.seed, ADRS) 
           11:  rnode ← fors_node(SK.seed, 2i + 1, z − 1, PK.seed, ADRS) 
           12:  ADRS.setTreeHeight(z)
           13:  ADRS.setTreeIndex(i)
           14:  node ← H(PK.seed, ADRS, lnode ∥ rnode)
           15: endif 
           16: return node */
        byte[] node;
        var fHOrTFactory = new FHOrTFactory(_shaFactory);
        
        // 1: if z > a or i ≥ k*2^(a−z) then 
        // 2:   return NULL
        if (z > slhdsaParameterSetAttributes.A)
            throw new ArgumentException($"z > a, i.e., {z} > {slhdsaParameterSetAttributes.A}");
        if (i >= slhdsaParameterSetAttributes.K * BigInteger.Pow(2, slhdsaParameterSetAttributes.A - z))
            throw new ArgumentException($"i ≥ k*2^(a-z), i.e., {i} ≥ {slhdsaParameterSetAttributes.K * BigInteger.Pow(2, slhdsaParameterSetAttributes.A - z)}");        
        // 4: if z = 0 then
        if (z == 0)
        {
            // 5: sk ← fors_SKgen(SK.seed, PK.seed, ADRS, i)
            var sk = ForsSkGen(skSeed, pkSeed, adrs, i, slhdsaParameterSetAttributes);
            // 6: ADRS.setTreeHeight(0) 
            adrs.TreeHeight = SlhdsaHelpers.ToByte(0, adrs.TreeHeight.Length);
            // 7: ADRS.setTreeIndex(i)
            adrs.TreeIndex = SlhdsaHelpers.ToByte((ulong)i, adrs.TreeIndex.Length);
            // 8: node ← F(PK.seed, ADRS, sk)
            var f = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.F);
            node = f.Hash(pkSeed, adrs, sk);
        }
        else
        {
            // 10: lnode ←  fors_node(SK.seed, 2i, z − 1, PK.seed, ADRS)
            var lNode = this.ForsNode(skSeed, pkSeed, 2 * i, z - 1, adrs, slhdsaParameterSetAttributes);
            // 11: rnode ← fors_node(SK.seed, 2i + 1, z − 1, PK.seed, ADRS) 
            var rNode = this.ForsNode(skSeed, pkSeed, (2 * i) + 1, z - 1, adrs, slhdsaParameterSetAttributes);
            byte[] lAndRNodes = new byte[lNode.Length + rNode.Length];
            lNode.CopyTo(lAndRNodes, 0);
            rNode.CopyTo(lAndRNodes, lNode.Length);            
            // 12: ADRS.setTreeHeight(z)
            adrs.TreeHeight = SlhdsaHelpers.ToByte((ulong)z, adrs.TreeHeight.Length);
            // 13: ADRS.setTreeIndex(i)
            adrs.TreeIndex = SlhdsaHelpers.ToByte((ulong)i, adrs.TreeIndex.Length);
            // 14: node ← H(PK.seed, ADRS, lnode ∥ rnode)            
            var h = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.H);
            node = h.Hash(pkSeed, adrs, lAndRNodes);
        }
        // 16: return node 
        return node;
    }

    public byte[] ForsSign(byte[] md, byte[] skSeed, byte[] pkSeed, ForsTreeAdrs adrs,
        SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1:  SIGFORS = NULL    ▷ Initialize SIGFORS as a zero-length byte string
           2:  indices ← base_2b(md, a, k)
           3:  for i from 0 to k − 1 do  ▷ Compute signature elements
           4:    SIGFORS ← SIGFORS ∥ fors_SKgen(SK.seed, PK.seed, ADRS, i · 2^a + indices[i])
           5:
           6:    for j from 0 to a - 1 do    ▷ Compute auth path
           7:      s ← floor(indices[i]/2^j) ⊕ 1
           8:      AUTH[j] ← fors_node(SK.seed, i · 2^(a− j) + s, j, PK.seed, ADRS)
           9:    end for
           10:   SIGFORS ← SIGFORS ∥ AUTH
           11: end for
           12: return SIGFORS */

        byte[] auth = new byte[slhdsaParameterSetAttributes.A * slhdsaParameterSetAttributes.N];
        // 1: SIGFORS = NULL  ▷ Initialize SIGFORS as a zero-length byte string
        // per Figure 16, the size of sigFors is k(1 + a)n bytes
        byte[] sigFors = new byte[slhdsaParameterSetAttributes.K * (1 + slhdsaParameterSetAttributes.A) * slhdsaParameterSetAttributes.N];
        // 2: indices ← base_2b(md, a, k)
        var indices = SlhdsaHelpers.Base2B(md, slhdsaParameterSetAttributes.A, slhdsaParameterSetAttributes.K);
        // 3: for i from 0 to k − 1 do  ▷ Compute signature elements
        for (int i = 0; i < slhdsaParameterSetAttributes.K; i++)
        {
            // 4: SIGFORS ← SIGFORS ∥ fors_SKgen(SK.seed, PK.seed, ADRS, i · 2^a + indices[i])
            var sk = ForsSkGen(skSeed, pkSeed, adrs, i * (int) BigInteger.Pow(2, slhdsaParameterSetAttributes.A) + indices[i], slhdsaParameterSetAttributes);
            // for each iteration of the loop, an n-byte sk value and an a*n-byte authentication path are appended to sigFor = (a + 1)*n bytes
            sk.CopyTo(sigFors, i * ((slhdsaParameterSetAttributes.A + 1) * slhdsaParameterSetAttributes.N));
            
            // 6: for j from 0 to a - 1 do    ▷ Compute auth path
            for (int j = 0; j < slhdsaParameterSetAttributes.A; j++)
            {
                // 7: s ← floor(indices[i]/2^j) ⊕ 1
                // Per Appendix B, floor(x/y) (<-- floating point arithmetic) = x/y (<-- integer division) when integer division is used
                var s = (indices[i] / (int) BigInteger.Pow(2, j)) ^ 1;
                // 8: AUTH[j] ← fors_node(SK.seed, i · 2^(a− j) + s, j, PK.seed, ADRS)
                ForsNode(skSeed, pkSeed, i * (int) BigInteger.Pow(2, slhdsaParameterSetAttributes.A - j) + s, j, adrs,
                    slhdsaParameterSetAttributes).CopyTo(auth, j * slhdsaParameterSetAttributes.N);
            }
            // 10: SIGFORS ← SIGFORS ∥ AUTH
            auth.CopyTo(sigFors, (i * ((slhdsaParameterSetAttributes.A + 1) * slhdsaParameterSetAttributes.N)) + slhdsaParameterSetAttributes.N);
        }
        // 12: return SIGFORS
        return sigFors;
    }

    public byte[] ForsPKFromSig(byte[] sigFors, byte[] md, byte[] pkSeed, ForsTreeAdrs adrs, SlhdsaParameterSetAttributes slhdsaParameterSetAttributes)
    {
        /* 1: indices ← base_2b(md, a, k)
           2: for i from 0 to k − 1 do
           3:   sk ← SIGFORS.getSK(i) ▷ SIGFORS[i · (a + 1) · n : (i · (a + 1) + 1) · n]
           4:   ADRS.setTreeHeight(0) ▷ Compute leaf
           5:   ADRS.setTreeIndex(i · 2^a + indices[i])
           6:   node[0] ← F(PK.seed, ADRS, sk)
           7:
           8:   auth ← SIGFORS.getAUTH(i) ▷ SIGFORS[(i · (a + 1) + 1) · n : (i + 1) · (a + 1) · n]
           9:   for j from 0 to a − 1 do ▷ Compute root from leaf and AUTH
           10:    ADRS.setTreeHeight( j + 1)
           11:    if floor(indices[i]/2^j) is even then
           12:      ADRS.setTreeIndex(ADRS.getTreeIndex()/2)
           13:      node[1] ← H(PK.seed, ADRS, node[0] ∥ auth[j])
           14:    else
           15:      ADRS.setTreeIndex((ADRS.getTreeIndex() − 1)/2)
           16:      node[1] ← H(PK.seed, ADRS, auth[j] ∥ node[0])
           17:    end if
           18:    node[0] ← node[1]
           19:  end for
           20:  root[i] ← node[0]
           21: end for
           22: forspkADRS ← ADRS ▷ Compute the FORS public key from the Merkle tree roots
           23: forspkADRS.setTypeAndClear(FORS_ROOTS)
           24: forspkADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
           25: pk ← Tk(PK.seed, forspkADRS, root)
           26: return pk; */
        var fHOrTFactory = new FHOrTFactory(_shaFactory);
        var f = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.F);
        var h = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.H);
        var t = fHOrTFactory.GetFHOrT(slhdsaParameterSetAttributes, FHOrTType.T);
        byte[] node1;
        byte[] node0AndAuthJ = new byte[2 * slhdsaParameterSetAttributes.N];
        byte[] root = new byte[slhdsaParameterSetAttributes.K * slhdsaParameterSetAttributes.N];
        
        // 1: indices ← base_2b(md, a, k)
        var indices = SlhdsaHelpers.Base2B(md, slhdsaParameterSetAttributes.A, slhdsaParameterSetAttributes.K);
        // 2: for i from 0 to k − 1 do
        for (int i = 0; i < slhdsaParameterSetAttributes.K; i++)
        {
            // 3: sk ← SIGFORS.getSK(i) ▷ SIGFORS[i · (a + 1) · n : (i · (a + 1) + 1) · n]
            var sk = sigFors[(i * (slhdsaParameterSetAttributes.A + 1) * slhdsaParameterSetAttributes.N)..((i * (slhdsaParameterSetAttributes.A + 1) + 1) * slhdsaParameterSetAttributes.N)];
            // 4: ADRS.setTreeHeight(0) ▷ Compute leaf
            adrs.TreeHeight = SlhdsaHelpers.ToByte(0, adrs.TreeHeight.Length);
            // 5: ADRS.setTreeIndex(i · 2^a + indices[i])
            adrs.TreeIndex = SlhdsaHelpers.ToByte((ulong)(i * (int)BigInteger.Pow(2, slhdsaParameterSetAttributes.A) + indices[i]), adrs.TreeIndex.Length);
            // 6: node[0] ← F(PK.seed, ADRS, sk)
            var node0 = f.Hash(pkSeed, adrs, sk);
            // 8: auth ← SIGFORS.getAUTH(i) ▷ SIGFORS[(i · (a + 1) + 1) · n : (i + 1) · (a + 1) · n]
            // auth should consist of a n-byte values
            var auth = sigFors[((i * (slhdsaParameterSetAttributes.A + 1) + 1) * slhdsaParameterSetAttributes.N)..((i + 1) * (slhdsaParameterSetAttributes.A + 1) * slhdsaParameterSetAttributes.N)];
            // 9: for j from 0 to a − 1 do ▷ Compute root from leaf and AUTH
            for (int j = 0; j < slhdsaParameterSetAttributes.A; j++)
            {
              // 10: ADRS.setTreeHeight( j + 1)
              adrs.TreeHeight = SlhdsaHelpers.ToByte((ulong)(j + 1), adrs.TreeHeight.Length);
              var authJ = auth[(j * slhdsaParameterSetAttributes.N)..((j + 1) * slhdsaParameterSetAttributes.N)];
              // 11: if floor(indices[i]/2^j) is even then
              // Per Appendix B, floor(x/y) (<-- floating point arithmetic) = x/y (<-- integer division) when integer division is used
              if (indices[i] / (int)BigInteger.Pow(2, j) % 2 == 0)
              {
                  // 12: ADRS.setTreeIndex(ADRS.getTreeIndex()/2)
                  adrs.TreeIndex = SlhdsaHelpers.ToByte(SlhdsaHelpers.ToInt(adrs.TreeIndex, adrs.TreeIndex.Length) / 2, adrs.TreeIndex.Length);
                  // 13: node[1] ← H(PK.seed, ADRS, node[0] ∥ auth[j])
                  node0.CopyTo(node0AndAuthJ, 0);
                  authJ.CopyTo(node0AndAuthJ, node0.Length);
                  node1 = h.Hash(pkSeed, adrs, node0AndAuthJ);
              }
              else
              {
                 // 15: ADRS.setTreeIndex((ADRS.getTreeIndex() − 1)/2)
                 adrs.TreeIndex = SlhdsaHelpers.ToByte((SlhdsaHelpers.ToInt(adrs.TreeIndex, adrs.TreeIndex.Length) - 1) / 2, adrs.TreeIndex.Length);
                 // 16: node[1] ← H(PK.seed, ADRS, auth[j] ∥ node[0]) 
                 authJ.CopyTo(node0AndAuthJ, 0);
                 node0.CopyTo(node0AndAuthJ, authJ.Length);
                 node1 = h.Hash(pkSeed, adrs, node0AndAuthJ);
              }
              // 18: node[0] ← node[1]
              node1.CopyTo(node0, 0);
            }
            // 20: root[i] ← node[0]
            node0.CopyTo(root, i * slhdsaParameterSetAttributes.N);
        }
        // 22: forspkADRS ← ADRS ▷ Compute the FORS public key from the Merkle tree roots
        // 23: forspkADRS.setTypeAndClear(FORS_ROOTS)
        var forsPKAdrs = new ForsRootsAdrs(adrs.TreeAddress);
        // 24: forspkADRS.setKeyPairAddress(ADRS.getKeyPairAddress())
        forsPKAdrs.KeyPairAddress = adrs.KeyPairAddress;
        // 25: pk ← Tk(PK.seed, forspkADRS, root)
        // 26: return pk;
        return t.Hash(pkSeed, forsPKAdrs, root);
    }
}
