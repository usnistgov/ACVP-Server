using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.AES.KATs
{
    public class KATDataCbcCts
    {
        #region GFSBox
        public static List<AlgoArrayResponse> GetGFSBox128BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F34481EC3CC627BACD5DC3FB08F273E60000000000000000"),
                    CipherText = new BitString("D9492AAFC53406FCD852F32A99EE0AC00336763E966D9259")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("9798C4640BAD75C7C3227DB910174E720000000000000000"),
                    CipherText = new BitString("EBFCC06CF6B23AF48D6A5D2942F20C6BA9A1631BF4996954")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("96AB5C2FF612D9DFAAE8C31F30C421680000000000000000"),
                    CipherText = new BitString("D34F6DA593795486577FA43729500228FF4F8391A6A40CA5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("6A118A874519E64E9963798A503F1D350000000000000000"),
                    CipherText = new BitString("A6BEEB3883A98C467E257EAF3DDCDAA1DC43BE40BE0E5371")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("CB9FCEEC81286CA3E989BD979B0CB2840000000000000000"),
                    CipherText = new BitString("0B5D3426DAC8918264BD7BF948F7268E92BEEDAB1895A94F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("B26AEB1874E47CA8358FF22378F091440000000000000000"),
                    CipherText = new BitString("41195B7BE9CF7A3B1B6CD6C53587650A459264F4798F6A78")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("58C8E00B2631686D54EAB84B91F0ACA10000000000000000"),
                    CipherText = new BitString("2A81E8ADF6457034FA94E2D83B3A3FC208A4E2EFEC8A8E33")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetGFSBox192BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("1B077A6AF4B7F98229DE786D7516B6390000000000000000"),
                    CipherText = new BitString("9F88A422F49262C8B0B54CFD7C95DD5E275CFC0413D8CCB7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("9C2D8842E5F48F57648205D39A239AF10000000000000000"),
                    CipherText = new BitString("451F9BD836DA05F0617890A11C860F5EC9B8135FF1B5ADC4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("BFF52510095F518ECCA60AF4205444BB0000000000000000"),
                    CipherText = new BitString("34F462CD9414739AC80DC7C8846414084A3650C3371CE2EB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("51719783D3185A535BD75ADC65071CE10000000000000000"),
                    CipherText = new BitString("9EE4C7B8DCEDE4BCCBC800CE572E53DA4F354592FF7C8847")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("26AA49DCFE7629A8901A69A9914E6DFD0000000000000000"),
                    CipherText = new BitString("B32552A7FE31B0C5FD037769D71C850FD5E08BF9A182E857")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("941A4773058224E1EF66D10E0A6EE7820000000000000000"),
                    CipherText = new BitString("CB6D98CE6F6D91A47A9CA9F57F2798D0067CD9D374920779")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetGFSBox256BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("014730F80AC625FE84F026C60BFD547D0000000000000000"),
                    CipherText = new BitString("6D7EC318F1818C0FED8329964B0C393B5C9D844ED46F9885")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("0B24AF36193CE4665F2825D7B4749C980000000000000000"),
                    CipherText = new BitString("EC055A877BB04CDCFAB8E9273B5B296BA9FF75BD7CF6613D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("761C1FE41A18ACF20D241650611D90F10000000000000000"),
                    CipherText = new BitString("00ECC4DD5B93D10E573E9995A93F98F4623A52FCEA5D443E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("8A560769D605868AD80D819BDBA037710000000000000000"),
                    CipherText = new BitString("CBD1637565244285960E779EBFB6A2BB38F2C7AE10612415")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("91FBEF2D15A97816060BEE1FEAA49AFE0000000000000000"),
                    CipherText = new BitString("DB3FCE4A240D94E006826DCB26FD98FC1BC704F1BCE135CE")
                },
            };

            return result;
        }
        #endregion GFSBox

        #region KeySBox
        public static List<AlgoArrayResponse> GetKeySBox128BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("10A58869D74BE5A374CF867CFB473859"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A75F0E73E576B5E7318A059549BB077A6D251E6944B051E0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("CAEA65CDBB75E9169ECD22EBE6E54675"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F05E492633E06C962AA9EC33FA8043A06E29201190152DF4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("A2E2FA9BAF7D20822CA9F0542F764A41"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("132BD38EB7606D9AF73040D20748C45EC3B44B95D9D2F256")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("B6364AC4E1DE1E285EAF144A2415F7A0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D95555079C6DF19816276E450DE14A225D9B05578FC944B3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("64CF9C7ABC50B888AF65F49D521944B2"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("63EB14ACD1109B6BA5CA6929B80390ADF7EFC89D5DBA5781")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("47D6742EEFCC0465DC96355E851B64D9"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("906AE0E3503E3989D458A3165741C3C10306194F666D1836")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("3EB39790678C56BEE34BBCDECCF6CDB5"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("93CFA92312ECCA85CD7EA9C92C97B8E4858075D536D79CCE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("64110A924F0743D500CCADAE72C13427"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2C7568D1C0C9C59C25622736DF95CFB335870C6A57E9E923")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("18D8126516F8A12AB1A36D9F04D68E51"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("59D073A639241DC5291FFF838EB678736C68E9BE5EC41E22")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F530357968578480B398A3C251CD1093"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4604D4615ECB4B9061987AB07BE22316F5DF39990FC688F1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("DA84367F325D42D601B4326964802E8E"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A35DFAB8FC80324EC6B213C5F4E1E366BBA071BCB470F8F6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("E37B1C6AA2846F6FDB413F238B089F23"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("29EA812AA8B15A83A00D3795FFE880AF43C9F7E62F5D288B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("6C002B682483E0CABCC731C253BE5674"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F10298A616EDCFCB1F0E178A48FC47EC3580D19CFF44F101")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("143AE8ED6555ABA96110AB58893A8AE1"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("58CCA9DB19458D529E803619E22B6C76806DA864DD29D48D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("B69418A85332240DC82492353956AE0C"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7D7DAE2FDF36DC878F22C2FE0E3E2D6AA303D940DED8F0BA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("71B5C08A1993E1362E4D0CE9B22B78D5"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DCC0E95D87250F53A62B9A9EB2808A9EC2DABD117F8A3ECA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("E234CDCA2606B81F29408D5F6DA21206"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("19764959B06C137178D8B7152B484A91FFF60A4740086B3B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("13237C49074A3DA078DC1D828BB78C6F"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("66B3E372608F9705DEAA9D333B8344E28146A08E2357F0CA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("3071A2A48FE6CBD04F1A129098E308F8"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("157BF5C7ACD844FE11D37F5CA6D6413A4B98E06D356DEB07")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("90F42EC0F68385F2FFC5DFC03A654DCE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("42D0CC62DAC2D6064DEF2EFA439B96DD7A20A53D460FC9CE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FEBD9A24D8B65C1C787D50A4ED3619A9"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6B9131EE95F3578CD985876025B996BFF4A70D8AF877F9B0")
                },

            };

            return result;
        }

        public static List<AlgoArrayResponse> GetKeySBox192BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("E9F065D7C13573587F7875357DFBB16C53489F6A4BD0F7CD"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C21AC145B59290DC403664A93F23F7600956259C9CD5CFD0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("15D20F6EBC7E649FD95B76B107E6DABA967C8A9484797F29"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FFE17A6434EF903B3DF5F3D45B11A1718E4E18424E591A3D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("A8A282EE31C03FAE4F8E9B8930D5473C2ED695A347E88B7C"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C521BC253852823DCEEE2999393F1CA293F3270CFC877EF1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("CD62376D5EBB414917F0C78F05266433DC9192A1EC943300"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2194F18522B54D1B37A71DD9CEF750997F6C25FF41858561")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("502A6AB36984AF268BF423C7F509205207FC1552AF4A91E5"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B0988CF89B1FD68EE7175F83434F1FF08E06556DCBB00B80")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("25A39DBFD8034F71A81F9CEB55026E4037F8F6AA30AB44CE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CF1338E88B5DA664CBCC37B9C41329803608C344868E9455")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("E08C15411774EC4A908B64EADC6AC4199C7CD453F3AAEF53"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FAA4255BC57538CC049F8B834318ED4677DA2021935B840B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("3B375A1FF7E8D44409696E6326EC9DEC86138E2AE010B980"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("70A442DCAB6ACE5C553709473CC474ED3B7C24F825E3BF98")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("950BB9F22CC35BE6FE79F52C320AF93DEC5BC9C0C2F9CD53"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F62D48B078BABC091DB01AC77849CB2564EBF95686B35350")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("7001C487CC3E572CFC92F4D0E697D982E8856FDCC957DA40"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3593A25E90A2149A71D83092B3D40435FF558C5D27210B79")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F029CE61D4E5A405B41EAD0A883CC6A737DA2CF50A6C92AE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A7220EB53ABEF89DC345938731714FF0A2C3B2A818075490")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("61257134A518A0D57D9D244D45F6498CBC32F2BAFC522D79"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A14DE21792CC53D523D0D3C8DC9C330FCFE4D74002696CCF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("B0AB0A6A818BAEF2D11FA33EAC947284FB7D748CFB75E570"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DD19EB103C9084EAEFF5D21FA588B6AAD2EAFD86F63B109B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("EE053AA011C8B428CDCC3636313C54D6A03CAC01C71579D6"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A6EEB6E88B094B37DDE3190EC3750AF29B9FDD1C5975655F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("D2926527E0AA9F37B45E2EC2ADE5853EF807576104C7ACE3"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("75C5CA76C5D65AD29ED0286908339B0FDD619E1CF2044461")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("982215F4E173DFA0FCFFE5D3DA41C4812C7BCC8ED3540F93"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("56CD8C3126474CF27F5CCD211BE0A638D4F0AAE13C8FE933")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("98C6B8E01E379FBD14E61AF6AF891596583565F2A27D59E9"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("55D0F94F7FB6EC6F4A700AE0135F363719C80EC4A6DEB7E5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("B3AD5CEA1DDDC214CA969AC35F37DAE1A9A9D1528F89BB35"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2A955779887F1EC4B155772FD8A816B93CF5E1D21A17956D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("45899367C3132849763073C435A9288A766C8B9EC2308516"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("291B5EBF21F8B080E31F292ADA67733369FD12E8505F8DED")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("EC250E04C3903F602647B85A401A1AE7CA2F02F67FA4253E"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FE11ED9E5A7BFDEC105E3ADB92536A618AA584E2CC4D1741")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("D077A03BD8A38973928CCAFE4A9D2F455130BD0AF5AE46A9"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8CFA25BAFD09DEFFC4164331B1892DD5ABC786FB1EDB5045")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("D184C36CF0DDDFEC39E654195006022237871A47C33D3198"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("90B562E039BC75C6FC27BE95AE1A241D2E19FB60A3E1DE01")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("4C6994FFA9DCDC805B60C2C0095334C42D95A8FC0CA5B080"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4A3A8292132DB9C87A2374B950C780F17656709538DD5FEC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("C88F5B00A4EF9A6840E2ACAF33F00A3BDC4E25895303FA72"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F1F8CC6E243AF10EF3AD805AE3D7F0B9A67CF333B314D411")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetKeySBox256BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("C47B0294DBBBEE0FEC4757F22FFEEE3587CA4730C3D33B691DF38BAB076BC558"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2AB137A528600CF88E5EC6C8DA2A149C46F2FB342D6F0AB4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("28D46CFFA158533194214A91E712FC2B45B518076675AFFD910EDECA5F41AC64"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("94F87C7DB9813248E4BE974BE443865D4BF3B0A69AEB6657")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("C1CC358B449909A19436CFBB3F852EF8BCB5ED12AC7058325F56E6099AAB1A1C"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5AFBDB932C134B2AFBF2C497BB71EF55352065272169ABF9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("984CA75F4EE8D706F46C2D98C0BF4A45F5B00D791C2DFEB191B5ED8E420FD627"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A4BD52CFC3D4698F5E0702F3A6F259234307456A9E67813B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("B43D08A447AC8609BAADAE4FF12918B9F68FC1653F1269222F123981DED7A92F"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6EC1B317251AF9351EA73383EA82D9E24663446607354989")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("1D85A181B54CDE51F0E098095B2962FDC93B51FE9B88602B3F54130BF76A5BD9"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B1D22123D7A681F296292DC173534035531C2C38344578B8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("DC0EBA1F2232A7879DED34ED8428EEB8769B056BBAF8AD77CB65C3541430B4CF"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9EA5E0E332C6E6CAC4808A435FB2A0CFFC6AEC9063234800")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F8BE9BA615C5A952CABBCA24F68F8593039624D524C816ACDA2C9183BD917CB9"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5A9B15E78958FF05C1D0EA83EC70B1DAA3944B95CA0B5204")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("797F8B3D176DAC5B7E34A2D539C4EF367A16F8635F6264737591C5C07BF57A3E"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D71F2A7D362F22C6F9B57D75767D3FEBA74289FE73A4C123")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("6838D40CAF927749C13F0329D331F448E202C73EF52C5F73A37CA635D4C47707"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8CC676B2BBE0860D523AC7E8E6EF5A29B91D4EA4488644B5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("CCD1BC3C659CD3C59BC437484E3C5C724441DA8D6E90CE556CD57D0752663BBC"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("09B3B1527CA77F58AF1D4C8E6BA433A6304F81AB61A80C2E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("13428B5E4C005E0636DD338405D173AB135DEC2A25C22C5DF0722D69DCC43887"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C37A146B0DB509DC0363188413821B73649A71545378C783")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("07EB03A08D291D1B07408BF3512AB40C91097AC77461AAD4BB859647F74F00EE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("778044193F6118791D78CB18765453D847CB030DA2AB051D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("90143AE20CD78C5D8EBDD6CB9DC1762427A96C78C639BCCC41A61424564EAFE1"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("30D9BE8622E214A5D43A90718FE5A7A3798C7C005DEE432B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("B7A5794D52737475D53D5A377200849BE0260A67A2B22CED8BBEF12882270D07"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BAA417DFD17B772CC44DCC4F44FB8DCB637C31DC2591A076")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FCA02F3D5011CFC5C1E23165D413A049D4526A991827424D896FE3435E0BF68E"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ADBB80FD3BB218F0CB2933D1D85B5D27179A49C712154BBF")
                },
            };

            return result;
        }
        #endregion KeySBox

        #region VarTxt
        public static List<AlgoArrayResponse> GetVarTxt128BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("80000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D3D78DD7387922F7A2B7647077BCAA0A0EDD33D3C621E546")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("C0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("93203AC3EF66E2BA7CCAFF4BD3C795874BC3F883450C113C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("E0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("73B3DB03202ED6851DB9ACDCDE7BE02D72A1DA770F5D7AC4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A9D49F0EE83FF5DA83D8D5F13F3307CD970014D634E2B765")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F8000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("59C64F2692368B8F3193491BD1477AE7F17E79AED0DB7E27")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FC000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4B90325F8EA0ACA8F9FA7388F565891E9ED5A75136A940D0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FE000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F4168580239E1D4BB44960F46413C164C4295F83465C7755")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FF000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("93C2092E6CC991B6AC4759C9FC297000B1D758256B28FD85")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FF800000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("46E05AFA83AC8B26C62CFCF1F770024F42FFB34C743DE4D8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFC00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6ED151E8ADAB2168302E70A0EF7D3EC49958F0ECEA8B2172")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFE00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CBC8F44F07B9499F2938B687DD1C89DB956D7798FAC20F82")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFF00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2087FB7A355649B20AF6A34E1B5B2730A01BF44F2D16BE92")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFF80000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F0D8E8F2E7771D7132B449CB7FCFC629B5F1A33E50D40D10")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFC0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B3D50BC8D82BDD3B8B5D02774D08821C2637050C9FC0D481")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFE0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("65E0311D8D5288D0B1D22C50DD97A3B0113ECBE4A453269A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFF0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A28A171F15B4A416AFC2FD9B07336AFC97D0754FE68F11B9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFF8000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6746954CA69DC0AF4DCF747B10D360D3C6A0B3E998D05068")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFC000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AAD8AFD7C217D4ACB13281827267C228DF556A33438DB87B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFE000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4DA5043C1DE98FA513C3839B8402A76F90FB128D3A1AF6E5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFF000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8F7208873631C24E44C28C395347EA3626298E9C1DB517C2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFF800000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EDF020D35C5B6996F30A8355AFD28CEBA6CB761D61F8292D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFC00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8E55C89B6FD50FA2B6B709276CB86E2A12ACD89B13CD5F87")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFE00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BF8E53DD3EC581EAFD12EE506C38EBC795B1703FC57BA09F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFF00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E21FBB78242730A67075D6ACA3D37805DE11722D893E9F91")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFF80000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2D86D72510ED3A0672206A4C48AE225B6D114CCB27BF3910")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFC0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("81DF20A76A6072AF2940120F2B9BAF8E5CE37E17EB4646EC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFE0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8F7CA8AE8D747C6977A82D6FD739F0AA18C1B6E215712205")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFF0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("93DEB588125FC993E122C1E8FDDFD65A99693E6A59D1366C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFF8000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4D401297E011B20A165CB3F390D91A666C7C64DC84A8BBA7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFC000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3786125549E47B70A4815AE98D146009E17BC79F30EAAB2F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFE000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4027BED207D35BD7744058811655B1B61114BC2028009B92")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFF000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A6F086CC1B0385D9B8B7EA1E000280D99C28524A16A1E1C1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFF800000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("777FD5EDC04D698864D5886FEE6F2484ED62E16363638360")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFC00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("60828F942FFACD674442858A01E321295A8688F0B2A2C162")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFE00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B894786F40B6D291439E32F151DDFABC23F710842B9BB9C3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFF00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DF82732C23A75D8FDDDEA4926212728344A98BF11E163F63")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFF80000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8A941738A4BCA10D9C29832FFCF0FD9F0F18AFF94274696D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFC0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3951DE30F48E3CE3E67E6DB5D7AD611782408571C3E24245")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFE0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2A3143DE7ABF700975654F16370DE76C303FF996947F0C7D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFF0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7FAEB41B825115CBC8FBD96F7A8525B47DF4DAF4AD29A361")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFF8000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8E0CB8F84699B7E64D1F2C7B6458A211C72954A48D0774DB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFC000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("88CCD84A4B4C07EBEEDC1D3C827E42201DF9B76112DC6531")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFE000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("670A2D325853C7AD4854EEE83A2716A78E4D8E699119E1FC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFF000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("050EF883DF5F378361E2E4E8A0E44888E6C4807AE11F36F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFF800000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7BB7DB396851F00628F0AE1ED21AD3428EBF73AAD49C8200")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFC00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E3B643EA0A4B467CA3AB5B2F9312C5AC4FB288CC20400490")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFE00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E2F5C31EA46DDED1F2B482A1A64A957D04497110EFB9DCEB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFF00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0B2D49C78B47AC81702AA4E3919EABC675550E6CB5A88E49")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFF80000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1B2AD7BAF4EFCAE42093F7B16A8EB242B6768473CE9843EA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFC0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DEC729A91CA98A5B197A8B8C70C71F8BCB2F430383F9084E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFE0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2574EF85141140448C3A197FE0FEF471FF4E66C07BAE3E79")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFF0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("85EE917BD20073654C9511A5B57431657B90785125505FAD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFF8000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B29C63EE1BD8D9C50B1A7FA5DA679CC08B527A6AEBDAEC9E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFC000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2FA701E5FAE3E1A4368B78B1E00E73A943FDAF53EBBC9880")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFE000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("20B73BC6CAA3417E9837A57DFED270A153786104B9744B98")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFF000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3534433BC8E163EC59EB4D8116AD8051B5AB3013DD1E61DF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFF800000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7F370132C4D26C3ACA89B7006B7217FF7470469BE9723030")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFC00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F724CE961A865C60D91197ADD279858FA35A63F5343EBE9E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFE00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DC6CAC583C10A13FB4FD90A66ED20EA4FD8687F0757A210E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFF00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0189467AA01414F647ABE44D45BA5F637A181E84BD5457D2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFF80000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B828083B74D038FD01754C90DD0469E2653317B9362B6F9B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFC0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CD521622236D38F20B875E6290263044995C9DC0B689F03C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFE0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F33B9919169B8CDC1BB67D561D39EAAC77A4D96D56DDA398")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFF0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1AA36683832D5F481CB2F6A7F105686184BE19E053635F09")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFF8000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("20EF8B7FBAF682C035324D09C7FD613D32CD652842926AEA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFC000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4B7C705A8DFF72A2E4DFC9499EB48223493D4A4F38EBB337")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFE000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4906E09D026EBA48ABFEAAAB8BC35783D9BFF7FF454B0EC5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFF000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B92FB44EECA8B17D3840FB8A71CD3B943535D565ACE3F31E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFF800000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A7D979D4B464EC3B615882BDB2F41C20F60E91FC3269EECF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFC00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A57746DA61B5692CF5D777C1BFB5BBE5AB69CFADF51F8E60")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFE00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AF190AF8F05C980CDDE6B2F65B69ED6D7866373F24A0B6ED")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFF00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5C9C2598ABC46E1E6A51C75828879D4C1EA448C2AAC954F5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFF80000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("02335E7B33622ABDEF4EE4ACE21D9541ACC5599DD8AC0223")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFC0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("35932E4E3FF32C24FD7CDC0F9F1977A5D8764468BB103828")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFE0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4E22988D029E1957D9800F04A95955411B0D02893683B9F1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFF0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A5EDB30D66E8871A5F40A0517CF5EFF696D9B017D302DF41")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFF8000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7E144D6A130B09C830C72D980ECB1E02EF1623CC44313CFF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFC000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("807CADFA03D94B004E7C6B45F294C997284CA2FA35807B8B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFE000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AE01D8C7A2FDC81A4C37D9E52A2F2852F2E976875755F940")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFF000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("63435A933B671ECC9A6A25EDF0A05CCAEC198A18E10E5324")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFF800000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("35BC4DE8435D06862FA172CF6B30F513545D50EBD919E4A6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFC00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("36F5C42D2812F2A64F4A5A5153E7B970DBDFB527060E0A71")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFE00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0F42109381E5F418B6027481835ED08E9CFA1322EA33DA21")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFF00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("66D9B2FB85A3AE199424F9C858E531648785B1A75B0F3BD9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFF80000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("397E70B60B8231D95BB128270038B08F38F67B9E98E4A97B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFC0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7DA650C02AC828C6F60274FB055DCF59192AFFFB2C880E82")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFE0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8D3CD1AD3B3E5602ECF297ED6DBA7DEA6A7980CE7B105CF5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFF0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DDD42B5A7DF3601CD3DC104EBC47D245EA3695E1351B9D68")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFF8000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F86892B0E21993799D6CBCF08F7BB61D6DA0490BA0BA0343")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFC000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("659195327D0862480B037EB53B0F6F3BF0EA23AF08534011")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFE000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B677430E72A938ED5C11EB1FFE377F42FF13806CF19CC387")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFF000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("835219CFE6E2DAC6D8A522EFC96A07466838AF1F4F69BAE9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFF800000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C0339B19B046698A34869DA77EB6582936CF44C92D550BFB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFC00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2F68440E344B497C872D06F284ABED89D06E3195B5376F10")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFE00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4F14A2A19585ED7CB28F3616396D5FF1C440DE014D3D6107")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6ED850D6490DF0841D04F60EFDF901C0F0C5C6FFA5E0BD3A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF80000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CD612FDDC8C8D7C8D92AC0E2220F6E443E40C3901CD7EFFC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFC0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C3769E73929BB252BCFA62BA7F6A7920B63305C72BEDFAB9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFE0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EACD21E57916E9FAE818C44B341A2C1836BBAAB22A6BD492")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("598FCA6C0A022D59D2947AB2A8CB0407307C5B8FCD0533AB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF8000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A54E468379CD26BA52C0CF19D40FDA6D829C04FF4C07513C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFC000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("56F046835DB141C24823E3E44189694FF17AF0E895DDA5EB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFE000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4E97E17BBD0BED232E7AA30C44328C1F277167F3812AFFF1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EF102F58F441ABEC31E8BF59FA74B7802CB1DC3A9C72972E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF800000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("41C4A23EF90279B465326B5899A7370C36AEAA3A213E968D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFC00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D7D5C6034E50B07383D65D46859BD7719241DACA4FDD034A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFE00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D6B9C49638D38DA7B5CD7D9844545665C14574D9CD00CF2B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D2AB3683AA891E0FDCB6630F01590AD7793DE39236570ABA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF80000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ACCE5489E906C425D5AF7EBF61D203AC16591C0F27D60E29")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFC0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8F20A6C1593CE739B47B7321D5C2CC5044FB5C4D4F5CB79B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFE0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CE797F7F412E29CDF640995AA717A74E674D2B61633D162B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AEAA0D887D52149061EA573E3CF744C1B4750FF263A65E1F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF8000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("85BE18E9E54AE5EA4AEA26BD048B843262D0662D6EAEDDED")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFC000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E800A0228E03F29504286E4B5D1F1A0970C46BB30692BE65")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFE000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E34CCDC474877D12762B3D46F138028E323994CFB9DA285A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7D2CDC160B2BA297B34936F2E7DDAE4E1DBF57877B7B1738")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF800"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FDA22EC0CC29389C38888E3B504464D2DFA5C097CDC1532A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B1BFF69153B03F6D07AE96831692EB6F3A0C53FA37311FC1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AB84345780DEBD99A91A3353ADF59BD0BA4F970C0A25C418")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E044FA0040714AA92D361BE2215CFE9E2DCE3ACB727CD13C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("955FEE38A8954ED3D8F2DB2792BC62D25160474D504B9B3E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6E08D7C9B164133A44124E30CF51D42A41A8A947766635DE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ADD0D83D6B6EDAB289C680367350920F25D6CFE6881F2BF4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B7C50AC867A314E364DE7924F32D44C941C78C135ED9E98C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5AFEB0D891A45B2ACE8552FC52FE2CAF5A4D404D8917E353")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2CED5B05C40C70BF6F5F6049C8E8516D02BC96846B3FDC71")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9042E4979706BFC9C1631D55FAA626CC9BA4A9143F4E5D40")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("064CBD944E9D5F6E21CEAB8F34F74E3DA1F6258C877D5FCD")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetVarTxt192BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("800000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("026D402E4D798129F55FAAE031BFF414DE885DC87F5A9259")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("C00000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A232687B36F277C757EC6A5FCD95578D132B074E80F2A597")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("E00000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("64A3575F5E8D3304C62D9ACC54FB33306ECCEDF8DE592C22")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F00000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EB7447505FCC2B3E84C8FF38EBE11238180B09F267C45145")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F80000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("83FF816D290BD5EAB7791277ABF8B37DEDD807EF7652D7EB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FC0000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6B694F8F824D97BE698BB40D654877F79978BCF8DD8FD722")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FE0000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5AB4484C78A39E1BA0BE82C4B5E2FB935310F654343E8F27")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FF0000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FAAB521774C1F14AD2D64876F5B37FC3833F71258D53036B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FF8000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5CECA42630B8F645062504AB14C35F02EBA83FF200CFF931")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFC000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A2FEA2FC4F3CBC706F062617EE427EFAFF620CCBE9F3292A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFE000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BD23F454E755197E0A4772E3C3F75CA97ABABC4B3F516C9A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFF000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B81635481FE51BF65CEC565C47E8D425AA187824D9C4582B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFF800000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FA188D91D7978DB314AE4DB1D8711FC91C0AD553177FD5EA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFC00000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1A9A4DC665AF53734FACCE49E9DDFB90A5DC46C372611941")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFE00000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("75D52321EA05C64AADBFF6133B773DA9E4F2F2AE23E9B10B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFF00000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("37306F725DC61A2054F037E1EA2204AFB7D67CF1A1E91E8F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFF80000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FDEAD3E45F5CDD2307DD54A4642F846726706BE06967884E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFC0000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F42BFE0D17293221B7AB676B5447149EB2F8B409B0585909")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFE0000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BC63E8CD414C4AA7BC2A5E57F6FF4A2C5E4B7BFF0290C783")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFF0000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BD253B4E2F9EF41A2B4A22CCFB292DD207093657552D4414")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFF8000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6BF85877C5759C8B0999D781F4E1BB39E1AF1E7D8BC225ED")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFC000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DC8706E2D29A848B9CA9CDF63C3C3A92EF6555253635D843")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFE000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("07D30F79B2BBBF9551F8728E3F07714FFB4035074A5D4260")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFF000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EAEC0F1443BBF4DD49E583B2D539D885446EE416F9AD1C10")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFF800000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9963153090AB82E72E6630C57DEAFAC2198AE2A4637AC0A7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFC00000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4D22D641FB0175461575A06E492A997B562012EC8FADED08")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFE00000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8C5A6610095F8961EBDCBD4AB0838156CC8A64B46B5D88BF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFF00000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FD2FAE5474567BA4A4C025903E0287F7A168253762E2CC81")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFF80000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5F0132A360BE71C1CC65629AEF363F7B1B41F83B38CE5032")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFC0000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9F2C27D9AEB24D81338B094C199E710161A89990CD141175")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFE0000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6AF1E410817234263389C4F4C90E3F7AB5ACCC8ED629EDF8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFF0000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BD4C022BE30BC2DA25FB588F74B9A279B16FA71F846B81A1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFF8000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("607465045F96A7826F2409F829365C924FAD6EFDFF5975AE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFC000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8E897C537B5714A96050D5860D9134E5EBFDB05A783D0308")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFE000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2F562B45D24B358AC35F85052EADDE67EB81B584766997AF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFF000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("636D0DD3BE41C13E5871214EF621626C0CF4FF4F49C8A0CA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFF800000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("658935CE002A196803FD19E86A90C892CC4BA8A8E029F8B2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFC00000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("705B6D0896BB4076E352BB7ADBAC7F00FEFEBF64360F38E4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFE00000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("929E0C6CFFBD848B4EEBE0B0DFCBF98F12AD98CBF725137D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFF00000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("246B4970B8DD4083BA9EB39CAD02BAEC6AFAA996226198B3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFF80000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3D365E9A44797B1E39E23E874619B94D2A8CE6747A7E3936")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFC0000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FB5CAF9C5682D1190D9A1F1DC7185CBF223736E8B8F89CA1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFE0000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("62D64EE51ECFD269F21082DA3763122FC0F797E50418B95F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFF0000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4DEB9C3D83FE25CE64987AA9809E74ADA758DE37C2ECE2A0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFF8000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E5D065A8FBCA6A022DC300398960EBB63A9B87AE77BAE706")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFC000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("871A874A830B5BF6B836C99496F85045D365AB8DF8FFD782")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFE000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E6063F0D29698960B2197BCEAC7343DBC8DCD9E6F75E6C36")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFF000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CBFB9E73F1BBC9390283349360E17B87C79A637BEB1C0304")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFF800000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6C86A84CF3ACBC6529446927C2B34981105F0A25E84AC930")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFC00000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5E850752855EEC11C3B59D89CB51E17542E4074B2927973E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFE00000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CC3EFC4ABF6284DD6147AE71B32FC4C64FE2A9D2C1824449")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFF00000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C952AC35AEBDC07A5AD3691CB348F4FEB7F29C1E1F62847A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFF80000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7E671B1CC2312E39C89C9BD80055C6EB36ED5D29B903F31E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFC0000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("69BB9EB5A92318C400148596DBEEB01427B8070270810F9D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFE0000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A49C5B0A878C470B60451055146FDCB994D46E155C1228F6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFF0000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("78D8F88A63E9781E13990910A4C232D0CA6108D1D9807142")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFF8000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("91F8D5F512F4F6BD00681AF7415005D3DC5B25B71B6296CF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFC000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A162B51D078C605BA486A08A3502279C44ABA95E8A06A2D9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFE000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2856956BE1C5DE589A18515DA53E6D8DA570D20E89B467E8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFF000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4C65F1AF8E13F6229478F14BC4646CAB758F4467A5D8F1E7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFF800000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DCE3A7E3E922B105E6239A782B3BB008BCEA28E9071B5A23")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFC00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("266D7BCF70D0DC7302BA9313D6EB3B727523C00BC177D331")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFE00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A16DCC09639DE9286C132B6B4815BF27CCAC61E3183747B3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFF00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BEB459A0174F8B17038CB8079DC0A5FD707B075791878880")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFF80000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0888B3E35FC47C2A732397F260972E7B7132D0C0E4A07593")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFC0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E7F7847079BA295CB2F3E0B2D5A2DDA7EFFBAC1644DEB0C7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFE0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1F5F06541AFDB16CFE15B8421726211FA005063F30F4228B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFF0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5E6C3B15C57F9E46DA364DFFAD7C008929975B5F48BB68FC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFF8000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3C4E79F203EA270F0202CD0057742904CF3F2576E2AFEDC7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFC000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("968691570FD910E011D23D5F1398377807C403F5F966E0E3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFE000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4DC384575FE9CC7C94B71F4342FD568DC8C20908249AB4A3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFF000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("33ACC5FD8B53138869451B64C08BD664C0541329ECB6159A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFF800000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("422097BC63CEA7FD0AD811212F3E7C7B7AA1ACF1A2ED9BA7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFC00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("611F8EA5F4BABF8385143BC9A8458AD8808BD8EDDABB6F3B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFE00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8C4B012DC73F564E5426664E10071BCF273C7D7685E14EC6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFF00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("72C12CA5CA60D2662078CA4158341EC532752EEFC8C2A93F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFF80000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DD3E968D58C3FDE06F50817D29BBF75FD893E7D62F6CE502")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFC0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4A38897FD3AB266A56C94F90F12870CF8DFD999BE5D0CFA3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFE0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C6A5D3DB79400F9DB9D9E7DDF775C58702647C76A300C317")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFF0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F9CA22AD596388EC057D871C9804D0D1172DF8B02F04B53A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFF8000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("45A038CD53D76D67A3291577961EB8B8054B3BF4998AEB05")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFC000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2BD832D8567AFE13675B7B64F834E5783783F7BF44C97F06")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFE000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F7A67AE097C7EBF844D094F7FB19DE8FAAD4C8A63F809541")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFF000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2081BD66BF1A784B9F0BA1D01253CD18CBFE61810FD5467C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFF800000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6EF4BD202EB3BE1C001D1E0825BBCA09830D8A2590F7D8E1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("99B2EB0C75ED153218F92F6A502BD5E5FFFCD4683F858058")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E56E694840EA63586E2418D40E7DCD7F523D0BABBB82F46E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FB2103EF569132C22F699BD8B9BC90D4344AAB37080D7486")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D9F82C9A30A72B8FB7B5FA9B3360205656C5609D0906B23A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E89724FE329FBE5450983AD7442491F47026026EEDD91ADC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F2C71DF538EDA179B3615D923294C05588330BAA4F2B618F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1BB51E2420E8BA1F9E1CA9CE6BFC383DFC9E0EA22480B0BA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("90E5B029B0DDA3D23177DF9FCBC1B14229CA779F398FB04F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8DCAF5601284584F0F1134C75F717BC851F89C42985786BF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("769AEA21580CBD815B28F5AF47118A246AC1DE5FB8F21D87")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BBD294727E4A08301B1DEE6229378FE003AA9058490EDA30")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("20822928EE5A1FFEB7E9DCF5DB4BA2E7E34EC71D6128D487")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("603FDF20FB4A60A9BD01AEAB9307699014BE1C535B17CABD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EA3BC5395F207A41EE4BBD9649DD9445C9EF67756507BEEC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A89C1016E51A622F931D3EF36DE3240040E231FA5A5948CE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4CCC64C0D506E74D1F3830030F933B3903194B8E5DDA5530")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("882532B08DC69C9FD59804257378060F90BD086F237CC4FD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9EB797DEA2F59D7E978A7AE5B1BA071919259761CA17130D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7D20EB5336A57F344F867FA6D59D8F83D7CBB3F34B9B450F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B7FC15C40664F97E35505D8B3A1530BA725B9CAEBE9F7F41")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1B834C173721694E32A951BD64815F209D924B934A90CE1F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3C4D12BF7B1502FFE083B46D39E7B214C50562BF094526A9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B21DCCEE4B4D30048D1F65A6B8B1A8FFD2F11805046743BD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D15A542D61594A7E2A41398BAB5F14538DD274BD0F1B58AE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8B55D17FD0FAD196A3104990507094349D6BDC8F4CE5FEB0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F3B5E90527DA4A43BBB96ADC5EF3C652FD5548BCF3F42565")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3906560005162A49961D33DB31EF439ED2CCAEBD3A4C3E80")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("194E81CE80C851ECAD318AEA6ADCD76BE03CB23D9E11C9D9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3CEA0DB2C4C561815E0921D6728786D678F933A2081AC1DB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A0547F74BE14D915AE36F28A162D30254061F7412ED320DE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D8DB71F5AC799FBD0F145716D2AF634E9064BA1CD04CE6BA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2BF91CF47CD888816915A083258CD77848391BFFB9CFFF80")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("51D321435A4A5C6D847555FDEBDEF074B8D2A67DF5A999FD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("50CE72662D0666751675A1B39C7CE90AAACA7367396B69A2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("59E2E48DABD19D29D6DF32D9EC36B23FA80FD5020DFE65F5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EAEC68FF4FEABC0EE2EC5F866FD414812162995B8217A67F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A033FAD2D5FF57B33053EBF44B664C51C6A6164B7A60BAE4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E85D58694B95C27CFAD14DA2C478F70F64E0D7F900E3D9C8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E5432AE537CAFF18B0E7311432E18FAA1AD2561DE8C1232F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FC1F043EE3F93F1450CF00CD139C8F0B279689E9A557F58B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8A8583DA9517CAEFC0FCC32B5496C20BC4637E4A5E6377F9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("35AEA469061AD1BBCFAEA055B64F194F492E607E5AEA4688")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DC4725452CAB63F5F3B5F6258C1F5C3DE8C4E4381FEEC740")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A1DDEC598DDC4D82261A48D933141BF591549514605F3824")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B0768F09989AFD69E5E4A35108ED2FAB74B24E3B6FEFE40A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0083C87E41E4EE40EC66BBECD465AD182437A683DC5D4B52")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E4DC9E4A859C3675770F0CF99E3035BBBB2852C891C5947D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C49F0EE7B5C5AED50A6979B25E5B23F41B9F5FBD5E8A4264")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("47A3C4DBA745743DF8C728F4B524D1A830DAB809F85A917F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("76C74A7F6471A7EDCCBB3205631C7500EAEF5C1F8D605192")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3F16BAF0806963EA9191DF68470BE427B8AA90040B4C15A1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3A58D370DE879354C5DECD8C2426115D97FAC8297CEAABC8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ABF3B3495FD1A12EA6006F474CE55F0F9B47EF567AC28DFE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EFA18366C2957167DFA2FE29998F7F901B8426027DDB962B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B21EDC497E5BBC4873FD2692A47FFB30E917FC77E71992A1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("223A5F5F5141A665E82FE2FE80234F57DCEEBBC98840F8AE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5140F79E545C4A3723DC653CCF83678A4E11A9F74205125B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E6C910B0FE39E1E91B0AEFF632556A38F60467F55A1F17EA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5768086B37E143BA87E9F65E91C02566D436649F600B449E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D22D40F82A807E2DD4DC53FFBA1E63353BC0E3656A9E3AC7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AFD08B1B3ABB67D939C916789BC99AAB6BACAE63D33B928A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("992B219B7011B996D2F470B3260B0BA78935FFBC75AE6251")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("412AAA39215DF570C3EB24C933A2577093DC4970FE35F677")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DBD8C287BBC2E00646582273E7356FF314F9DF8589758517")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1FBE14923D32B4C069C356354F3F87D302EA0C98DCA10B38")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9FFCAC20B83F5139B2AACF8C791267808F091B1B5B0749B2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("695D3081EE6FD7D3EFD3CF3AA8374DE505B389E3322C6DA0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F943EEFAA4E4AFF6BF374A3FCDC13C8C381308C438F35B39")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CAABC0031E7F5F571D64C47D58F619CA68C230FCFA9279C3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F904CDA39658C40F23B7036E5A2374121C84A475ACB011F3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("235753A75620E9F9A703E47E09EF6FA845119B68CB3F8399")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0EBC792D0C02F4BC92D90CE654880DAF9423762F527A4060")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D6B9E4D917CFA5B52FB97D0BCDFF6AFEF361A2745A33F056")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8406FADADABA5C9BB812B85B4ED0A2645EF145766ECA849F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DDD8E017BB429A0F4FE8B10988085A0FC9AF27B2C89C9B4C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("36925CC741BC16DDBAA9C2059ED1FDB3FB9C4F16C621F4EA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("198706C9AE18D50AC6056533B6D29CB2138E06FBA466FA70")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("52F0C758C8D8C00EA8A59539769DDB91FB4BC78B22507077")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2DB7A21069C4BC8E0085027170B166888B2CBFF1ED0150FE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4050409266AF734233ED9C7850194C4A08B30D7B3F279627")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("085C550E4D16F1F153B69503D2ADFF76FDF6D32E044D77AD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6E49B0AB9C6F593F5C0E9B85B09C2E5593CB284ECDCFD781")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A37D59B55D7A84E3A9616E76C6A7EB847B017BB02EC87B2B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A63B71CC351E4FD0F496133D7D2CD354C5C038B6990664AB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("26D2FB920C075758D25858BAC403C6894B7020BE37FAB625")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4E5B4D0004458AB9A019B936912C76B060136703374F64E8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C64BB71A0DAE4677406771024F226ACD8D63A269B14D506C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("39D82DD21279C7EADAEBAAA0EF5C6C7ED317F81DC6AA454A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0F05E8E26FF3FC98E0AD204B167B7375DDDECECD5354F04D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2CC721E8BC07E4D091245C87E0E33D6F41C5205CC8FD8EDA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D6522D0876B1797B57FC1B55FCD3F146CF42FB474293D96E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4A68B24D2E50288E5165AD9D7287F50BA231692607169B4E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("70FD4FDCB0088A5316F9032556584C6EACE4B91C9C669E77")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("97BBD540CB6884BC717A323B06856BA175DB7CFD4A7B2B62")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9C824DC063602EC9B8B4B126025A17F6C1FABA2D46E259CF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DE92D0EFDC6B6E697EB3A19D48CFF349241C45BC6AE16DEE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0F5E922D7D9E89F91F40533F7DDDF3608FD03057CF136442")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("84FD41A484BB6A5F4397829E6869D4DEDDB505E6CC1384CB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("653EFD5EA91812AAAE2F4DB2EC254B1D5674A3BED27BF4BD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D2E7F537EC19B3E4D3082D5C942D3A0FB687F26A89CFBFBB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D0DD05F97D4523F35F01C6D937F507A30547DD32D3B29AB6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("262FEC5F7F42894D2C826567C419C486186861F8BC5386D3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0088130C8251B2E47A0E7B69237D7C74EACF1E6C4224EFB3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("84F899D197C1EE870DC0CEDEA6DED50BD241AAB05A42D319")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2B6F40981805714549E0F6308B33DB465EB9BC759E2AD8D2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CC127793D741A63F06A851B27B7ECA6F018596E15E78E2C0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0EF0CAE508775D41157849FE3F4EF870DD8A493514231CBF")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetVarTxt256BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("8000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C15D2174271758673EB86C1FCD0C72D0E35A6DCB19B201A0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("C000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("349C266DA62B3027F28B21A362FE7045B29169CDCF2D83E8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("E000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9DB764709A517BBB4BB5EA60DCC0E42FD8F3A72FC3CDF74D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("088C6C17B4C6071CD03BAE27242EDE9D1C777679D50037C7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("F800000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A93BF450C257684508B348E1D03749FC9CF4893ECAFA0A02")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FC00000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E821E54FE599AB06F55AD359A81B84A38FBB413703735326")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FE00000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4D57132842C56D27B2CFC83EA954DA1E60E32246BED2B0E8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FF00000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A2B807636474FAF27D7ED3DC49FC5753EC52A212F80A09DF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FF80000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AD2909F2AD5240BD25D3F1BF3FB9186CF23E5B600EB70DBC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFC0000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("807DB27D649F3027AC764F33E8256066A3F599D63A82A968")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFE0000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("82BDFCC921A86BD3CAFFA46624C80FEED1CCB9B1337002CB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFF0000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CF3432743D60004D7FA2BED11BF3848ECC111F6C37CF40A1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFF8000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("562DBC26827CBAC13364929AAAC91A73DC43B51AB6090523")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFC000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D855DF91A9B8FC5E8B75D0896D490E354DCEDE8DA9E2578F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFE000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1CA324FFEB2085DEE3C9AB2E52F715691A4C1C263BBCCFAF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFF000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EFB1847D73BB28968D969072E9F38D5E937AD84880DB5061")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFF800000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9C09269F7B24DD5EE16BB2A35733FB81610B71DFC688E150")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFC00000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("946F605FCA94BDE585B052C40EB7F4DD27EF2495DABF3238")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFE00000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EA30CA961AE36C3F316F6FC234536D6B633CAFEA395BC03A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFF00000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CDBB36C7994C0D989DA0F4CC3FA98A2C6E1B482B53761CF6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFF80000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("170D41208F308AC1B215E00457125748976E6F851AB52C77")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFC0000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AE69457002188903B5CE0D15D7B218D185F2BA84F8C307CF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFE0000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E9C688F3E62728DF965EFC216ECBD56C6BCCA98BF6A835FA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFF0000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1ED8930273B348242B3A1702F0CC312E2C75E2D36EEBD654")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFF8000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B555EFA052EB2D396AA87E2261F692FEBD49295006250FFC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFC000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("42313E605945EE7078557B2BC1F72723A190527D0EF7C70F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFE000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("075B919EC7FDC22AF6884939FD9E1246BBD1097A62433F79")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFF000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2C7A6321BC14C05007773A8766EADC6707058E408F5B99B0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFF800000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CC14236E69B96E32E47AE841AFF7EB885FD1F13FA0F31E37")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFC00000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0B3F5CABF43F2F5E36C49C3CF7D6F8D7FC4AF7C948DF26E2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFE00000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("71C6EDEE428EE75FA55A4FC5A6EDB780829FD7208FB92D44")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFF00000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4A50B82F0E6BB1E0CCCD5E8BE8EEE180AD9FC613A703251B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFF80000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ECB5D8ED64400914BAD9FB80F18D26D833AC9ECCC4CC75E2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFC0000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0162125EF9366062C40EC3E3796869BE2025C74B8AD8F4CD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFE0000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5DFBD84FE949D2CE51A6B881895AF195F85CA05FE528F1CE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFF0000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("02179490EB0FE8FD56C335A51738DBA66F6238D8966048D4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFF8000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("988849427C37DD2FAB918861CAB09D88F2B21B4E7640A9B3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFC000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("55C5EBB52C1E20196DFF38427AC58784F836F251AD1D11D4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFE000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8FC9CF067F398885CD4B07104BF7038B077E9470AE7ABEA5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFF000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D64CA72F26941FF0E2F451CE51BD3629E0DCC2D27FC98656")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFF800000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D77976F8F1EDE806541F3136C0C76224BE66CFEA2FECD6BF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFC00000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AAF8F63D93F670FD1F618A6C9A7459C5DF31144F87A2EF52")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFE00000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("01AC4AEFBD0E62EC2090AFEF992E20ACB5BB0F5629FB6AAE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFF00000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6CA20E0E1669378CB22921EC7346666F3C9DB3335306FE1E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFF80000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DCA070922208F136F06181B2D00BBA6D3DD5C34634A79D3C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFC0000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("71F465D591B50796A4FB41C90A27229282BDA118A3ED7AF3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFE0000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AEADA0AEF369085CE68BC64A0071985B2937A64F7D4F46FE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFF0000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A572C9DFF9DD47395B2CD425E152DE60225F068C28476605")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFF8000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("794FED7C9B6590164E0B5EEBA5E77435AE682C5ECD71898E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFC000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0F122F94ACF0606437DC547FB789386A5E031CB9D676C302")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFE000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0C5F239BAA343BCCCFE8AA53E968C190A78463FB064DB5D5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFF000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7669C7D9FF1805C026567739FCD755308AA9B75E78459387")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFF800000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("15313B269A3556B3EFEB3CB75E78D98B3F84566DF23DA48A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFC00000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AE817A7E301ADB3FAA9D65C91FA3BB7131690B5ED41C7EB4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFE00000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5AAA3D5483EAF3087C84841DDA1ACD7B77DD7702646D55F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFF00000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E891F01113489B44EC5B6BA85A72686C4C022AC62B3CB78D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFF80000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3A8DDE8784C6E7DE10EC37AE8D006722092FA137CE18B5DF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFC0000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("416B0E46897A5BA8A6AFA813E3C535263E0CDADF2E68353C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFE0000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("606D8B5FA2FC0D81041036546157415DD8C4B200B383FC1F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFF0000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EF51FD6D19760CB3C606DB8F3205A66011825F99B0E9BB34")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFF8000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D5627AEDE964CF0E3680D2D64AFD2E90F8B9FFFB5C187F7D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFC000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9D1858B486DC6416383DC176C638FB0BFFB4E87A32B37D6F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFE000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("873F8E34EE48306366C4E7CD13104194D276C13A5D220F4D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9D43671ED7A5E0867D5FA75E51D3AA1694EFE7A0E2E031E2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFF800000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("06DE23E8654C97B5A6550D1FC8E3296C8F8FD822680A8597")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFC00000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5BEA7E0CEFA6831CCA4D1B88E23E63BAE0F0A91B2E45F8CC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFE00000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("93ADEADB8A61AA491E7864D537FE360D597A6252255E46D6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("739FFC4BE076CAECC6F581B348D036ACF51A0F694442B8F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFF80000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("412D813B9F1278D7F671BBF1C6D69BEC9FF071B165B5198A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFC0000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0E7857B08553DFD0ACE4B247633EC636C20A19FD5758B0C4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFE0000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C9C83C40800F43E4C5643B83EC414BD497120166307119CA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0B5415C656D895A847B586309A7E1AED4B3B9F1E099C2A09")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFF8000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1305E0BE0064E3EB9FDC9D2452F4848BEB040B891D4B37F6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFC000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D6DCA2787606CAE7FE432F3D3B326F929F0FDEC08B7FD79A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFE000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("533D6A8DC24477C1DC0414530136875A2E70F168FC74BF91")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFF000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E0907F1D33758F8722967E3CD7E1A715462CCD7F5FD1108D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFF800000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9FC4A4706DF1AE27CD7005BCD5DC97A7A4AF534A7D0B643A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFC00000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("70C347BFDF685A40DEF052F84A748015AB980296197E1A50")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFE00000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5B8B1820B64FEFEA78CCAAC2E8EAFC21F97D57B3333B6281")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A3DADFF5C0E27E60D72F0AB355826241F33FA36720231AFE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFF80000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("84787A00407C590693FE0CC0CAFFEC23FDCFAC0C02CA5383")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFC0000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CD14968384C3BDAF320883127C792902AD4916F5EE5772BE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("435F1867FE0DBDEEC705F3FC89325D602E16873E1678610D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C4145C0AFC1A15AE12E044FB790F08934E6E627C1ACC5134")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFF8000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("51D3D3C3C2B8E6305F42CACDED55A0EEAB0C8410AEEEAD92")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFC000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("98D6294C72740F6B8E2A9CAEA84FFF9AE86F7E23E835E114")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFE000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1321579FC042316579EB358017E60506E68AD5055A367041")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFF000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C6E1C940CD5FC6BDC5F789F6A2D34EDF0791823A3C666BB6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFF800000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5107F568D78986DF199DAF3E1111894ADCCA366A9BF47B7B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E3576691AB31EC7573F2BFC074723811684C9EFC237E4A44")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("57104095DAEF6926D134AB2A5DAEAF5BA858411FFBE63FDB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("62A92F464C0D014FDD443BF136AC35AF04BC3DA2179C3015")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C5658BD15E337235F7BE30E457F38AC240071EEAB3F935DB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("98695CA30D01A4C42B51882E00F951880EBD7C30ED2016E0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B8D19FB1841A3C1E2B0FB732A128B14E15C6BECF0F4CEC71")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0C6EA1B163AE2935909E39EE1CB140CF0AEEDE5B91F72170")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6F99AA028320DF911AF5C5D2F05CBABF266581AF0DCFBED1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DC5F0EE27F315C0BF6E6643E9732D1CE6693DC911662AE47")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4DB95601157E0F72D8B1FCD9D25B3DF17606FA36D86473E6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8FBC4C54EFC8A743A8CFD6522F15BFB1112078E9E11FBB78")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D120AD4DB811A9DF9D78FD6B1167E22A40B264E921E9E4A8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("14D54FE6B100771DBBC507E28E9AB7A58D4595CB4FA70267")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5A618F3FF88182DDFE054079934E96D9B588A302BDBC0919")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2074A3126E0D351BB0C1C95AB85949AE33F7502390B8A4A2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EEEC1FEFD8FE093DDDDDD72BBCA701D23D20253ADBCE3BE2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FDBBB6963644B9443A57BA94293528D9A42734A3929BF84C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DFB0F40F91387F37334E1F67C90ADE1BE3ABC4939457422B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A70840F5C3B3785822698F28CFE09B69972BDD2E7C525130")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("21CB62A7E43CFCC2C2194CC31C243AA584A83D7B94C699CB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("39A57BF0F364C583A36164C24DE1EE2DCE61D63514ADED03")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("07C9210DB5951C16D8E64506DDD235336C839DD58EEAE6B8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("72A31B2A25CD38367D918493F03A7A50CD5ECE55B8DA3BF6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("402F0E2871B2DDE5CAED7DBEA8003A053B6F46F40E0AC5FC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("490388586004F120ACD4A0B78B1D4DEABA26D47DA3AEB028")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8DD217808BEC662451E007EBEAC773DB87F53BF620D36772")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6D23FD4AC633ABCAADABA823A4479AA510617D28B5E0F460")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B11E299A597DB5344CFB64D5DF3421D59AAEC4FABBF6FAE2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9B8C1C8895E2F76F7616350C92261B863A90C62D88B5C428")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BCFAB5A859CE4A600149D23830151405F1F1C5A40899E157")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FA0A37AE567F9695A505D4124360ED46190843D29B25A389")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("75F3105AB56596E156F0E06E38413AFFA866BC65B6941D86")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A2A4E9BFEBC68DDF146D25FC91B54EC68193C6FF85225CED")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8773FEA7B00F6D86E322566F6CCE9E719661CB2424D7D4A3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("71F400259FB9102DE714568242A335D686F93D9EC08453A0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9F7E38F782CA3191A1A049A6C23775A827EEFA80CE6A4A9D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F54BC65F974E02F52FF0A4CB6A2AB4C9D62068444578E3AB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("09488D6E4992CC2DB5926D2F9A1D4A01B5F71D4DD9A71FE5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E59FC47A410221DF6F70EBA222A05B036825A347AC479D4F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E1A4459773CF50680AA6F01B627FD7D6E3714E94A5778955")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("313871E37FAA98BD043586BB9ADAE399D836B44BB29E0C7D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ECA26D638B562412256A26F899F8F1635D454B75021D76D4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("577FF67C5CE67DA8F275E7CB4A91833BC3498F7ECED20953")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0D324433A62F0B79D0A86810784309C66E668856539AD8E4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6D39DF3A4E1657E2835E483303E764C98680DB7F3A87B860")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("48077DE03FA369782C082DFE87D461F36C5D03B13069C365")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C2230F18D4596980622794CB2E2126FCEF1B384AC4D93EDA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F11D3C346C273A168CEFCBD303363D50BF8115805471741B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("75539C553826C4A0E121D0F3E81A34ACC64C24B6894B038B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("62A687CD4774D735CBB63C7E6E21CAB83967A10CFFE27D01")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("37EFE9ED66460E70956E80ED1432F8A67C85E9C95DE1A9EC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("294495BE5948D786C4738B21A9B20A50A9EEC03C8ABEC7BA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C09B0AC3E4A0A144336A06E3CFE7D8B7CAC8E414C2F38822")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D7B18106B1FE02B83E64A4166129F67E5D942B7F4622CE05")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D5B3D4D7F15D02C75DF1B985B71715F8D240D648CE21A302")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("22EFD2BC031059EA3466FA2240064C2045D089C36D5C5A4E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BD9DCD3D20CCC20695EF07EBA7CF9976B4DA5DF4BECB5462")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("64400086F6950D9F4CDA985A48B31B56DCF4E129136C1A4B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("17B2BC551DE48E8718DC79ED7FD809A2D9A4C7618B0CE48A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DCB4E635DA015666D3894BC4C6A8862DCA352DF025C65C7B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("90E587B48C8B7FE97287DB9153D27D18238ACA23FD3409F3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("56E45852E239E355C13485DD0583272C59836A0E06A79691")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D2EBA2B09969657F581FEC44700FB45E33905080F7ACF1CD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("18460F4A6C8DEBD4D88EAC8CDB290D1672C9E4646DBC3D63")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CC7FD3201B852FF686EFACD7C691983CBA77413DEA5925B7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A7429D21850007478701E701DEBEE0EF6CAE8129F843D86D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EB80D0C486006878C823FE5195F03432FCFEFB534100796E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("61F02E1FC9F2DA2E709CFCD84F29E0D38C791D5FDDDF470D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("95583A13C095D83837124B98BD0C19E0C93BBDC07A4611AE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D914CBD317418AAE07C0BAE55A33E670C102E38E489AA747")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9E5ED2A4ED27B50C4CDA75E4CCE9ADAC93201481665CBAFC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5C731D89998FD184947F5CD6C45D01894960757EC6CE68CF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("91F91677E83A7BCE6DC890BDC308F615FEEC7CE6A6CBD07C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("01FE52B07051C770CCB8B06F4A3305F911C5413904487A80")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5EF2AE270B8A592816B4890563DBDF4A347846B2B2E36F1F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("56812D433608E39E7545D89867155F3C332EEE1A0CBD19CA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("290C8E4E7AC5847734FFE0F7BCC7FCC8866B5B3977BA6EFA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("57CCDF644B9AD71B4E623FCB070B256DCC1445EE94C0F08C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2ACBE59D05D12963A23D075070EE5402BE288319029363C2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0704F06A006AB41B46BC9846B1B9A5D3CFD1875523F3CD21")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B686AD05E3CFA615D11178DE531B5E96CB5A408657837C53")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4459331C8499FDCB071AA7C76B402FD2CA0BF42CB107F55C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CF70F86A112EE50278CD3B46474979AFFDD9BBB4A7DC2E4A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("29124CEE27F9F1AE838C22749FFA0387EDE447B362C48499")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("77E7C72E309C6026F1650E6FF2AE096310DFFB05904BFF7C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("44E08EBCE7B6F7E1858290332CCF7618C33BC13E8DE88AC2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AB248795E6F434A795F6D0AF0940AEC3CA359C70803A3B2A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BCDE4CEB01B7ADC3BB0BE36AFB8CA257BCC65B526F88D05B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("96344C89D8E494CC1AD5961854206A4ADB91A38855C8C464")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C100655481E3D3259C3F531E3B2460FECA6E8893A114AE8E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ABF3EE11DDBAC5208C9883C0DBB0B09C6629D2B8DF97DA72")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("28B82BDF8CDBC6C5BECEF3FD25CD121B4570A5A18CFC0DD5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("75727FF6A833A6DA30D10EDA76036F8272BC65AA8E89562E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("311994D7A8C93C04C820D81F6E5F7AB398551DA1A6503276")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D924A94B6088EDDC28118AA0A6EA2BA30DDFE51CED7E3F4A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DCC2464943100A5A8ED97F862933CD22DB826251E4CE384B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4142A209AC35B330EA67292977A1F3FB2CACF728B88ABBAD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A995BFA5D74B0A67D8F4A3E91419B569330D8EE7C5677E09")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("846A5A44853D7217907CEAF843184BB0EDF61AE362E882DD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F80195C8E2CF3742CCE9A9514416EFFB6168B00BA7859E09")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0B76E54667355F888B5A6554EBDA7973D1415447866230D2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2D82A002CBE2B97AC421CCDC5877E5B3516183392F7A8763")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("595F13670AE6043A731CB566623C8DCC77565C8D73CFD413")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4DCC2CC62620F642E07E0A300ED5968237232A4ED21CCC27")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4F1EACB56F9C837364DAFC3E15090D9C804F32EA71828C7D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B239B90179232103DDBF483A32DB317CD64424F23CB97215")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FC8774786AF701AC7CAEAF70D7BB39DC023E82B533F68C75")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("348FA8C3ACA4C42D67A6D20D82D6EEE2193A3D24157A51F1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A2BCA009F59F892E46DA82F0A8FA07E384ECACFCD400084D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("195AFE8D449D9CC80C2D32A4E38E22C41DCD8BB173259EB3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3F0F418659E0995A279E90936527632135E9EDDBC375E792")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F6826DF025F1C479150811F47FA0935A8A772231C01DFDD7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E5E6AAD2763B60E709FEF443176B2A4D6EDA7FF6B8319180")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4089788A82FB4A2450D1A2185F0011E5C267EF0E2D01A993")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6942864A3EC9E44BA11AE398105B5DF9E9F80E9D845BCC0F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7BAC898EB91A75F16A12B8471235C2DC6702990727AA0878")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("588F16EDBE75C0DFD9DCF8F044374C3C2E2E647D5360E092")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CE7027F229D3EB268B9B521A4EA99D6B1F56413C7ADD6F43")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("CF808A4C8C8EE35557404C2C9734C7E469CD0606E15AF729")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("44F08F52E168FEFBAC08F7BDD99A3D70A085D7C1A500873A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7105AE743732B75B6F0A268127CBA8494FC0D230F8891415")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DDD16AF1C5ABCA269E54BA5380F848DE4327D08C523D8EBA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EF7D8739D24E1884676DF8F481DF50197A15AAB82701EFA5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4F2C1B67F4AA2B1F114B5310DC352C905BF0051893A18BB3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3B6E9F7033C506552EC7E93D83950B4B97E8ADF65638FD9C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("41DCB72C4FA7C1F1FF942CAFCF5E98AD1EE6EE326583A058")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F6E19C7EC0BA53D6D942E778C67927BA26B549C2EC756F82")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B598316EE587524067ED2DC76691EDA770377B6DA669B072")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C21C2B2F36CB56EAF8D54E33B08BC4AB9C94B8B0CB8BCC91")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("535A2829F444FB7A97135552C2D6C1EC2FBB83DFD0D7ABCB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("59CA261F1F213B91C72AEA134F71F9F996877803DE77744B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("516DF272CD90B52C515C0967B859D73A7379F3370CF6E5CE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0D4D1B30F52DC5BEDB92F146524AB9B202DC99FA3D4F98CE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("209F1B29C2593136AE3159DF2EB786D71E38E759075BA5CA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4A9FA543EC2A3B429AF9FA861AD340A770BED8DBF615868A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7A1A8D665F160859A154C82C1F797FE5234B148B8CB1D8C3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8CBE42EF3038532F8430BC3BCBF14C4A294B033DF4DA853F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FC234D6CF20C17B3FAB8FB0D8E59DE3E3F58C950F0367160")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("628E4CD31B71C57653E7C930C80F113137F655536A704E5A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5646744CA751A718CCADC7349CA44512EA7BD6BB63418731")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9838EC3A4D578AFCD070AC60235755A9E74A4C999B4C064E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BDC7351628176EF078FA427AD19F43B7BA9EBEFDB4CCF30F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C0955E38CCA4C707279CA65B610E41A03194367A4898C502")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4CCC55EE8E010D1D5EF70C423EE9DE43DA797713263D6F33")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7354E74CF5D6E98ED06550534D3570A5D1AC39BB1EF86B9C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F55A3BCBE6C4E8AA9702AF715F506A232FDEA9E650532BE5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A9F27AF6B74A44D99C88CF9E9A1239FFD3A204DBD9C2AF15")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("496628AC344EDBC257F76F1A9EF0D8A13A0A0E75A8DA3673")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("42681691D45757E9C1F08C3ED2F7C62A52FC3E620492EA99")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7EE20AFDB2579D19E35C0B29F3F42A44D2E0C7F15B477246")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A5A1825355F1397CA7A2B986BCF264E4563531135E0C4D70")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F2480FE46CF73E28F254EE466FDF72EDA8A39A0F5663F4C0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("332156C5D2C7206C1BEDC7E7E7076452D94B5E90DB354C1E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("509199FF195658F9F9556544E4D1BB8A50E6D3C9B6698A7C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C57C793C1689F0FDA9DDB5738C454C1E9338F08E0EBEE969")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("648A70295EB9779A099A657903B534DC8B378C86672AA54A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3E3E405423ADA7FB87B23FD093E10EDECCA7C3086F5F9511")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("203C6BCDF98465F068D5CFD31A181A8A5B40FF4EC9BE536B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9BFFBAD7DAB689EAFF0ED55DF79271A560EB5AF8416B2571")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D7551CE2C41470FA91DC9A0E540A91EC2F005A8AED8A361C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FACDEE23919115383F8100325B90F1F47B03627611678A99")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1A472A180E87753ED6B24F1D58C93772CF78618F74F6F369")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9B63BAFAFC500CCA8F606E183188039603720371A04962EA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF8"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("90EC71621BBC84604443A7D577CA1E051F8A8133AA8CCF70")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0B96D5100D4400A7ED5903717EDB62E227936BD27FB1468F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4C58D2BDBA2C1EB20921D8FCF8D256B6B07D4F3E2CD2EF2E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C0A8BFEE6A70A9EA3BD9ECD912C681CC4BF85F1B5D54ADBC")
                },
            };

            return result;
        }
        #endregion VarTxt

        #region VarKey
        public static List<AlgoArrayResponse> GetVarKey128BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("800000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("BA2B81F42865BB6AB6F09EA79A7BDBDD3AD78E726C1EC02B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("C00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D21549CF0B4FA6A468E51FED2A2C3D2BAAE5939C8EFDF2F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("E00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("76DD34AA2FFEF2FE8AAD60774D273944F031D4D74F5DCBF3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4A895CF82B1B01BAC94BE5D5F7FBB04396D9FD5CC4F07441")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F80000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5C684654765305B7AACA623BC2009F6C30CCDB044646D7E1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FC0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3479B06FC16964AC99159E88EA3F0BA616AE4CE5042A67EE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FE0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3314B6E69E3E664D00937878252F9A2AB6DA0BB11A23855D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FF0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ADC832EFF9B2EF6D203BADEEE80C5AB7DB4F1AA530967D67")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FF8000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2075230CC00F999A238F69AC1E2B3746A81738252621DD18")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFC000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("45EE0123AD96B281A19393CA024F0D5577E2B508DB7FD892")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFE000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("697AED60DD2C985903D37865A380032DB8499C251F8442EE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFF000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("027A6F495AEF2BEB2A0F786D4229DD89965135F8A81F25C9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFF800000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E7BBFF178F113F0C4F2369798D30E5998B87145A01AD1C6C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFC00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6153F458627C9422C4BE0C6771BD4A7B8EAE3B10A0C8CA6D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFE00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F1A1504121F1121F06E7303DF17C6A9A64B4D629810FDA6B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFF00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8742EA96D87733058B21FC4B9431E2A8D7E5DBD3324595F8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFF80000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0917AE552956099283426A25D8330D2FF3F72375264E167F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFC0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F87CDE22BC5629F5261347B44F658AFD8EE79DD4F401FF9B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFE0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("909AF8E25FCCC6C3F639D890175AEDDCDD35CEA2799940B4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFF0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("B971098D17EF0EC9FD2F43E141AE5F376941CB6B3E08C2B7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFF8000000000000000000000000000000000000000000"),
                    CipherText = new BitString("AF5D365B59B83E0A58E46BB1A98A0A2D2C20F439F6BB097B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFC000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A834DEA8F77042E4B942F1EA7507F525625D01F058E565F7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFE000000000000000000000000000000000000000000"),
                    CipherText = new BitString("F37AFAC0371B0B05FA7AB3F9745B8515C0B5FD98190EF45F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFF000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DA127CC5E732DE0D567F3F0284CA762913001FF5D99806EF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFF800000000000000000000000000000000000000000"),
                    CipherText = new BitString("45EFEB377360F8DB6765EE27FD6C540C3B594C60F5C8277A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFC00000000000000000000000000000000000000000"),
                    CipherText = new BitString("1A92F635E4B05838696205C71B561179E9C0FC1818E4AA46")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFE00000000000000000000000000000000000000000"),
                    CipherText = new BitString("C279C4A98F77C61D719C140F7E079A63F8023EE9C3FDC45A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFF00000000000000000000000000000000000000000"),
                    CipherText = new BitString("53DBF2B040B81667B6CC6E1FD7C4F45B35F40182AB4662F3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFF80000000000000000000000000000000000000000"),
                    CipherText = new BitString("42D59DDE278A1AD99D9AF938757D22133AEBBAD7303649B4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFC0000000000000000000000000000000000000000"),
                    CipherText = new BitString("93EC42A18F118520BC07D69C7435585BA2124BEA53EC2834")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFE0000000000000000000000000000000000000000"),
                    CipherText = new BitString("D56C003ABE7B990410E1348AFD249DEEB9FB4399FA4FACC7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFF0000000000000000000000000000000000000000"),
                    CipherText = new BitString("11D159C023138A098AE39ABF12CA8EEEC26277437420C5D6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFF8000000000000000000000000000000000000000"),
                    CipherText = new BitString("7F65178A580BF8D2F36B3C46E536C63E171A0E1B2DD424F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFC000000000000000000000000000000000000000"),
                    CipherText = new BitString("E61CBAD2F5164B9A5EC2D2672B201C857CADBE402D1B208F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFE000000000000000000000000000000000000000"),
                    CipherText = new BitString("52BEE605F567781F4DE1D9B52D81195543B02FF929A1485A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFF000000000000000000000000000000000000000"),
                    CipherText = new BitString("D177745A897C7BCA9DAE836B44E8A630092FAACC9BF43508")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFF800000000000000000000000000000000000000"),
                    CipherText = new BitString("547759C6FFCC0A0D7C96625E38C15052CB2BF8280F3F9742")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFC00000000000000000000000000000000000000"),
                    CipherText = new BitString("424B0661C84D3EEBD70C46398094F7C9215A41EE442FA992")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFE00000000000000000000000000000000000000"),
                    CipherText = new BitString("3AE155080D499C3D91D38718BC307A17F21E99CF4F0F77CE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFF00000000000000000000000000000000000000"),
                    CipherText = new BitString("A32D981E622FADFD49F1E6BA7C239ECB95E3A0CA9079E646")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFF80000000000000000000000000000000000000"),
                    CipherText = new BitString("E5825430DA572190C860D9636095A21C4AFE7F120CE7613F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFC0000000000000000000000000000000000000"),
                    CipherText = new BitString("7F7466B72489058969AF3E66063EF96B827F000E75E2C8B9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFE0000000000000000000000000000000000000"),
                    CipherText = new BitString("BC6ADD3BB8C1A707071877CB40423AC835830C8E7AAEFE2D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFF0000000000000000000000000000000000000"),
                    CipherText = new BitString("13FE9E50BAD008A3C62D62708DED5656191AA0F2C8570144")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFF8000000000000000000000000000000000000"),
                    CipherText = new BitString("27B356D3EF79F0AD1DCC692CC8ECD60E85062C2C909F15D9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFC000000000000000000000000000000000000"),
                    CipherText = new BitString("FD085A0ECBE1D3CF427B199A9AD7C574678034DC9E41B5A5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFE000000000000000000000000000000000000"),
                    CipherText = new BitString("06D5AAA8FD647828CB51A8814792C732C2F93A4CE5AB6D5D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFF000000000000000000000000000000000000"),
                    CipherText = new BitString("DB04D7E6818C7D9BFEC6529DC33C3B641C3112BCB0C1DCC7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFF800000000000000000000000000000000000"),
                    CipherText = new BitString("66B3E83467740F82D2AB5A993F71DA8000C55BD75C7F9C88")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFC00000000000000000000000000000000000"),
                    CipherText = new BitString("2196B7295108C60866E717EB5656D417EA2E6B5EF182B7DF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFE00000000000000000000000000000000000"),
                    CipherText = new BitString("8F917B60DBCD6D5E16D56D5B6AF1840122322327E01780B1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFF00000000000000000000000000000000000"),
                    CipherText = new BitString("0022C9E5EB72FB345CD197595764E403C9CACB5CD11692C3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFF80000000000000000000000000000000000"),
                    CipherText = new BitString("771965D5067F9AC4B0848D8B3C8A6412A18E3DBBCA577860")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFC0000000000000000000000000000000000"),
                    CipherText = new BitString("07ADA8E290CA5B4BAC57674151F2117679B61C37BF328ECC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFE0000000000000000000000000000000000"),
                    CipherText = new BitString("A8CC5CF46B6B7137170D6375819A4607D2D99C6BCC1F06FD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFF0000000000000000000000000000000000"),
                    CipherText = new BitString("C37EB9B52BC2089FD4580F2C0D98559A1BFD4B91C701FD6B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFF8000000000000000000000000000000000"),
                    CipherText = new BitString("4741C839230403F8311EC72D1937C00511005D52F25F16BD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFC000000000000000000000000000000000"),
                    CipherText = new BitString("29131F73F925F90771BDE87FA84917DF3A4D354F02BB5A5E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFE000000000000000000000000000000000"),
                    CipherText = new BitString("F52250B8220A3B79E26E0DAD357B99DED451B8D6E1E1A0EB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFF000000000000000000000000000000000"),
                    CipherText = new BitString("4393C79C66B4DFD00B34D7C26B36977B6898D4F42FA7BA6A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFF800000000000000000000000000000000"),
                    CipherText = new BitString("ABC59DDCD5F6B4D7D5E60CB80EDEE9FAB611295E739CA7D9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFC00000000000000000000000000000000"),
                    CipherText = new BitString("DF13419F88B306071EC3040AFDEE68737D33FC7D8ABE3CA1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFE00000000000000000000000000000000"),
                    CipherText = new BitString("BCC87CCB713596DAFCB4081AD39D6E3B3B5E0F566DC96C29")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFF00000000000000000000000000000000"),
                    CipherText = new BitString("5CDC9DF320A03DEB1FBB416CB3F37EFCF807C3E7985FE0F5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFF80000000000000000000000000000000"),
                    CipherText = new BitString("DB5758D124D01DB46F08465392A3F04D41F992A856FB278B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFC0000000000000000000000000000000"),
                    CipherText = new BitString("F7CA7F3C531D9347A5FF762FBCB8E96A10D3ED7A6FE15AB4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFE0000000000000000000000000000000"),
                    CipherText = new BitString("A8F522B813E5C06E9C5495E6F7953BBA21FEECD45B2E6759")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFF0000000000000000000000000000000"),
                    CipherText = new BitString("B3E5D9331745D208EE195D9989A959B91480CB3955BA62D0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFF8000000000000000000000000000000"),
                    CipherText = new BitString("F266DCEF308F5C8F0AB2FD7C1B43456366404033D6B72B60")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFC000000000000000000000000000000"),
                    CipherText = new BitString("D43F80BDDAC2573F53CA6B7319EF3D911C317A220A7D700D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFE000000000000000000000000000000"),
                    CipherText = new BitString("5C426F5D23FCDB7A8D7F7976C53512DAAB3B89542233F127")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFF000000000000000000000000000000"),
                    CipherText = new BitString("A1D535E162B23988BCA5BEF9C4626A02D93EAE966FAC46DC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFF800000000000000000000000000000"),
                    CipherText = new BitString("E086645DB1953A09008935163CA4BF181BDEC521316503D9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFC00000000000000000000000000000"),
                    CipherText = new BitString("5ACFFF1B89D18796416564D6C00EABBDEEF456431DEA8B4A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFE00000000000000000000000000000"),
                    CipherText = new BitString("4AEED8311754C9AE751E9BC6A5DBC63906F2519A2FAFAA59")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFF00000000000000000000000000000"),
                    CipherText = new BitString("9A881831DDFF44FCF7EEE1724D435355251A7EAC7E2FE809")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFF80000000000000000000000000000"),
                    CipherText = new BitString("FB630ACD44A60841ECEDAA05EB49231B3BFFC16E4C49B268")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFC0000000000000000000000000000"),
                    CipherText = new BitString("07798BF9D9AFE9CFC2C4E2A74572570CE886F9281999C5BB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFE0000000000000000000000000000"),
                    CipherText = new BitString("5615CBC0711499C603521AE57688D4E8563BF90D61BEEF39")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFF0000000000000000000000000000"),
                    CipherText = new BitString("C89FA0D378AF6FE0ACFA8953C4C96E2C4D37C850644563C6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFF8000000000000000000000000000"),
                    CipherText = new BitString("070F9A5C56CCB6DB5B8EEE8A661BCC56B87C921B91829EF3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFC000000000000000000000000000"),
                    CipherText = new BitString("A1D289D6076B4A94FF801B90E9A364652E65EB6B6EA383E1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFE000000000000000000000000000"),
                    CipherText = new BitString("F7BD4ADE953F2E38D7B61BFBB46E55A79CA547F7439EDC3E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFF000000000000000000000000000"),
                    CipherText = new BitString("E8C7D1AD62BE4D7D8A729B06FC042D6FA5E652614C9300F3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFF800000000000000000000000000"),
                    CipherText = new BitString("5649A3E3730FF6E0D34E47AE9B8E840414954F0B4697776F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000"),
                    CipherText = new BitString("127FCEBDCC1E48927EF16F8F8A84D8B47C8D9AB6C2761723")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000"),
                    CipherText = new BitString("DD5B13766582599881C19284B93DE6D8DB7E1932679FDD99")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000"),
                    CipherText = new BitString("EB3472B277F06E9FCB1E756304C44D5F4C6A1C83E568CD10")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000"),
                    CipherText = new BitString("CFE55E118AA180AD9553AF513F91B2B590ECBE6177E674C9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000"),
                    CipherText = new BitString("034FED5AD6843BB30C409453156BC6A990684A2AC55FE1EC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000"),
                    CipherText = new BitString("50D8B402A99389E419A86F42F78651287472F9A7988607CA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000"),
                    CipherText = new BitString("AFA8DE47ABD4FDCDC6518CE7F3EC32AC56AFF089878BF335")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000"),
                    CipherText = new BitString("41FF34B41A3ED004C1428A8E5AB9BC0765C0526CBE40161B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000"),
                    CipherText = new BitString("39200D4B013A1AF04898B1E8F0803169377BE0BE33B4E3E3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000"),
                    CipherText = new BitString("ED22EC0806DBF7F6D9657EBD943E23E59402E9AA6F69DE65")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000"),
                    CipherText = new BitString("CB720C36FCB920FEF71CD711ED4AC45B123C1F4AF313AD8C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000"),
                    CipherText = new BitString("84AA2877C83E62CE61981AB3679724A81FFC626D30203DCD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000"),
                    CipherText = new BitString("D662006A6DC74DF2E2765CB269356F9576DA1FBE3A50728C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000"),
                    CipherText = new BitString("DD6EBC082F15344CB8DE9792CA97014D082EB8BE35F442FB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000"),
                    CipherText = new BitString("64B0EEDB567AA0C0DB62BB4C5E0B2336E656F9ECF5FE27EC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000"),
                    CipherText = new BitString("94F8CB6874DB7CED861CE57B11463C372CA8209D63274CD9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000"),
                    CipherText = new BitString("862B06B7617B9EF258C92571DA63518079BF5DCE14BB7DD7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000"),
                    CipherText = new BitString("DF8094CC3EE7E6AA51B6FBE813CD00BD3C849939A5D29399")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000"),
                    CipherText = new BitString("4647E69588A7B4C2614B020C113102D4ED3C0A94D59BECE9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000"),
                    CipherText = new BitString("12C0A53C207F25C0A070A28CC97119D363919ED4CE101964")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000"),
                    CipherText = new BitString("D27A545AE8E4A00D5CAC8DF38CB386267678F3A833F19FEA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000"),
                    CipherText = new BitString("8D4EBA93C0ABE8F5BA19C6E9DDEC412E3AA426831067D36B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000"),
                    CipherText = new BitString("65965259F00A475714797FF39580CE3F9272E2D2CDD11050")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000"),
                    CipherText = new BitString("388780BF07FA3E7936F0BC1104A4BD0B088C4B53F5EC0FF8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000"),
                    CipherText = new BitString("ACC587AD3369CB9749675C2C10806E604010A5E401FDF0A0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000"),
                    CipherText = new BitString("97DDE407DD19DBD6BD2CD464A5F13F69A87A385736C0A618")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000"),
                    CipherText = new BitString("C58CC80D19CB4EEB4D0F3707BEA13624545F2B83D9616DCC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000"),
                    CipherText = new BitString("7B00DE2394B2AA408314A8BE85CB7EBC4B706F7F92406352")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000"),
                    CipherText = new BitString("53062CCFB48903AE18332CB1D48DCBC7B7972B3941C44B90")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000"),
                    CipherText = new BitString("866083E0B1B7BEC899AE837798A1DA996F45732CF1088154")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000"),
                    CipherText = new BitString("52D8314FFE0D3E340A0573FAF3F169992E3579CA15AF27F6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000"),
                    CipherText = new BitString("169BE064810A3AE0935D529D957C1BD734A2C5A91AE2AEC9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000"),
                    CipherText = new BitString("98C92CC4366C92BDF8276CBAF5FED5F1A4D6616BD04F8733")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000"),
                    CipherText = new BitString("83A7A58FFD886E437B14A03E10A889BF7F692B03945867D1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000"),
                    CipherText = new BitString("5F0A42197EBE3B5FA51D768CEB69CF933BD141EE84A0E641")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000"),
                    CipherText = new BitString("4ADD055C7F0BFAE2BB98A80DDE3D9C57D1788F572D98B2B1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000"),
                    CipherText = new BitString("DC1BE017B83714D25A4EA40F19A6FEB40833FF6F61D98A57")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000"),
                    CipherText = new BitString("AEBE098E9B79FFF6844A2557D87984698568261797DE176B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000"),
                    CipherText = new BitString("00051932C595E37BB37449EFAB7BC338F9B0FDA0C4A898F5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000"),
                    CipherText = new BitString("6DCE0F6762E33F33E8591C5B1F9C109E8ADE895913685C67")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000"),
                    CipherText = new BitString("BB333B12C3AC0AC7866C74EE66E9FB6F39BDE67D5C8ED8A8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000"),
                    CipherText = new BitString("EF05FA5D61AF3F79EC7A3E8B944C32725C005E72C1418C44")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("00000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000"),
                    CipherText = new BitString("DC0FE3009A76F343FDF22EFD9847254F3F5B8CC9EA855A0A")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetVarKey192BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("800000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0A4EF31EAE2429B5677E4D1D816C5FD06CD02513E8D4DC98")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("C00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("45FD1E825345AB7EEFADDA3BB53F07532CE1F8B7E30627C1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("E00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DE49B809CFEA86A9E2B599E2DA6CA2499946B5F87AF446F5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5B247B601E49FF97306CCB2C3C360E372A560364CE529EFC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F80000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0E2EC0AD12A26ADB6FCFFF612EA430A435C1471837AF4461")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FC0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9A2F528AE1887A255B233F60E5BBC303CE60BC52386234F1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FE0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3A169CE226CF65094B51C2316CB31DC78C7C27FF32BCF8DC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FF0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("406CE54AA6A7766BEE0995320697B5D832BB6A7EC84499E1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FF8000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("297BE15B6F464D047F49379736F625C7A5C772E5C62631EF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFC000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7FC9D1D24E913B0FD9675F4D28B46DC3030D7E5B64F380A7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFE000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6C433504374FBB3772C4EB0EB628880A0DC9A2610037009B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFF000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("30EB1478B35CA58D9A70DF9B2A7F85ED0046612C766D1840")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFF800000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A445C0020FD5A05F3812230C46029CD64880C7E08F27BEFE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFC00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EDA1DDD48B2948992C3070ACA28AA7AD2520CE829A26577F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFE00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4D7573D857E8920C74ABCAD9C3D9C1918765E8ACC1697583")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFF00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("215150F6F0AE7310B105C9F00C297807E98F4BA4F073DF4B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFF80000000000000000000000000000000000000000000"),
                    CipherText = new BitString("69F93D44B2207DE06BC8A7CB5E32AE94F378F68C5DBF59E2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFC0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5F9DFA61EB6A634A5F26F016849A9BB3283D3B069D8EB9FB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFE0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4E46B45A46E322E6C2389FDA2FA8716EA7E1842E8A87861C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFF0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("66FCDDB31D4FF45EA7CE83D9AA66878677AA270471881BE0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFF8000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C3C48BB76EE49602861E2BD962D1078001B0F476D484F43F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFC000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2B6EF149BC53596A158FE637738C02B51C3A94F1C052C55C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFE000000000000000000000000000000000000000000"),
                    CipherText = new BitString("80C40162FF39DA7932B99026B4A45701E8A067B604D5373D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFF000000000000000000000000000000000000000000"),
                    CipherText = new BitString("EF21F562B904B080733C7E200093A5B4A7876EC87F5A09BF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFF800000000000000000000000000000000000000000"),
                    CipherText = new BitString("2CF53ACBC02495B61651CD2C93CEE70F0CF3E9D3A42BE5B8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFC00000000000000000000000000000000000000000"),
                    CipherText = new BitString("6F2FEC83A17522613E64622B75FC01236C62F6BBCAB7C3E8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFE00000000000000000000000000000000000000000"),
                    CipherText = new BitString("CBBDDF0873AC01ABF26F4E963C5D3A107F5E05BD20687381")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFF00000000000000000000000000000000000000000"),
                    CipherText = new BitString("D010C2F2320B6DFC2398D7DE17609653440E0D733255CDA9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFF80000000000000000000000000000000000000000"),
                    CipherText = new BitString("301EE14EA0A33FC33C6F0B1ACDE21F79AA5D5B1C4EA1B7A2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFC0000000000000000000000000000000000000000"),
                    CipherText = new BitString("24CBC3C3E8E839FE212ED74AC062E50D77E537E89E8491E8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFE0000000000000000000000000000000000000000"),
                    CipherText = new BitString("D1C4B81A93C16BB1B221A5DE54E621B6997DD3E9F1598BFA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFF0000000000000000000000000000000000000000"),
                    CipherText = new BitString("71E5249FAACB922ECFEC9B4CCDFC884D1B38D4F7452AFEFC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFF8000000000000000000000000000000000000000"),
                    CipherText = new BitString("040DC363B0E54C80F130ABCCDBDD450C0BE2B18252E774DD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFC000000000000000000000000000000000000000"),
                    CipherText = new BitString("ACF3CBB5B4F4CF0A96B2416A62E72BB9D2695E59C20361D8")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFE000000000000000000000000000000000000000"),
                    CipherText = new BitString("B3FDE1FD5FE0127D969C45DF460E11E3902D88D13EAE5208")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFF000000000000000000000000000000000000000"),
                    CipherText = new BitString("596807304D50BB51EEF24E98FEB78813D49BCEB3B823FEDD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFF800000000000000000000000000000000000000"),
                    CipherText = new BitString("8FFF6A5BC827FFB4849003EFD0083B91707B1DBB0FFA40EF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFC00000000000000000000000000000000000000"),
                    CipherText = new BitString("E0A7EC4B33941789B86D02DCF0906FFB7CA0C1D93356D9EB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFE00000000000000000000000000000000000000"),
                    CipherText = new BitString("7E96628059BBDEEA5BD80E7B863290D7F2CBF9CB186E270D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFF00000000000000000000000000000000000000"),
                    CipherText = new BitString("4AF6A495838804BB0E86E55AEE5B3ED6C94337C37C4E790A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFF80000000000000000000000000000000000000"),
                    CipherText = new BitString("C069DFD36F8E43A5AB552E3281B85CFB8E3558C135252FB9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFC0000000000000000000000000000000000000"),
                    CipherText = new BitString("351EF72B3EC883268D5CC1AB612E97D31B72EEAEE4899B44")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFE0000000000000000000000000000000000000"),
                    CipherText = new BitString("18160D22D05FA43BE5147712C367F77E011865F91BC56868")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFF0000000000000000000000000000000000000"),
                    CipherText = new BitString("9FAE2078E902083E134C68E24D4336E4E4771318AD7A63DD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFF8000000000000000000000000000000000000"),
                    CipherText = new BitString("DFE68F5DFB5FD6F29F818E386147A07061E3D194088DC8D9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFC000000000000000000000000000000000000"),
                    CipherText = new BitString("6D5DF6BB11D4701B7E4EC289ACB891DD36FF1EC9CCFBC349")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFE000000000000000000000000000000000000"),
                    CipherText = new BitString("3B30F3E0D563FD0BB6ACC528908CC1863CC9E9A9BE8CC3F6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFF000000000000000000000000000000000000"),
                    CipherText = new BitString("A2E12EDCD0EBE0A06820F7D8D995B3FE1EE5AB003DC8722E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFF800000000000000000000000000000000000"),
                    CipherText = new BitString("A937761216A3DAE7F33E3805BE1757BE245339319584B0A4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFC00000000000000000000000000000000000"),
                    CipherText = new BitString("E5F26CC01BCBE6C2AEA78E6FD3F26B317BD496918115D14E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFE00000000000000000000000000000000000"),
                    CipherText = new BitString("B0F560A364C8332387F428CF30B77654273AB2F2B4A366A5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFF00000000000000000000000000000000000"),
                    CipherText = new BitString("5885A1A45B9D18C159DCABD366408C72113365A9FFBE3B0C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFF80000000000000000000000000000000000"),
                    CipherText = new BitString("AA8007CC37552743B1FB1DC54DDF32CCAFA99C997AC478A0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFC0000000000000000000000000000000000"),
                    CipherText = new BitString("C2F79FA78D75923F26335D717209D22D9216309A7842430B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFE0000000000000000000000000000000000"),
                    CipherText = new BitString("C16888CC0F4D2D81B500C89825D3D71462ABC79228825849")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFF0000000000000000000000000000000000"),
                    CipherText = new BitString("9C61A96187C8D6497881158EEEF2DDE1534923C169D504D7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFF8000000000000000000000000000000000"),
                    CipherText = new BitString("B8D5223BCAB1734F1FA99B7BC2A3F249FA75E05BCDC7E00C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFC000000000000000000000000000000000"),
                    CipherText = new BitString("BD647F4DF0360A7C1AE958CF3215F5AB7D350FA6057080F1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFE000000000000000000000000000000000"),
                    CipherText = new BitString("23790D487A2EA78C259845BDE9D4E741F34E4A6324EA4A5C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFF000000000000000000000000000000000"),
                    CipherText = new BitString("EBBCA446C4EF827E95A4CC01AA67D4710882A16F44088D42")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFF800000000000000000000000000000000"),
                    CipherText = new BitString("F5125528A403ACBA638E5259794493EA3A3C15BFC11A9537")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFC00000000000000000000000000000000"),
                    CipherText = new BitString("0F2EFD0E945CB811FAD2661B6A21164F22C0A7678DC6D8CF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFE00000000000000000000000000000000"),
                    CipherText = new BitString("44FCC26B236D002E1F8061026A4E72A7B46B09809D68B9A4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFF00000000000000000000000000000000"),
                    CipherText = new BitString("969AE8A6BEEA8F9676A697388F50154D93BAAFFB35FBE739")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFF80000000000000000000000000000000"),
                    CipherText = new BitString("9322188A0F5645B70E088C7AEEA7E225C8AA80A7850675BC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFC0000000000000000000000000000000"),
                    CipherText = new BitString("CA2EA80F87F143D3B59BA8F26EEBD8D212C6F3877AF421A9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFE0000000000000000000000000000000"),
                    CipherText = new BitString("03EE7371696AE3707EF584F516E5BFCF33F123282C5D6339")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFF0000000000000000000000000000000"),
                    CipherText = new BitString("F7BD68298BEC4F55D37556B04B3225F7A8F161002733E93C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFF8000000000000000000000000000000"),
                    CipherText = new BitString("61E5D98EF1E6C089D7F3EEBC12A8039AB72F70EBF3E3FDA2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFC000000000000000000000000000000"),
                    CipherText = new BitString("B8216D3971006BD2D0BC54524AC590276A9D965E6274143F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFE000000000000000000000000000000"),
                    CipherText = new BitString("047530290D85DD7F4A097857AFDD9085A0C74FD0B9361764")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFF000000000000000000000000000000"),
                    CipherText = new BitString("583485A2B9ED3024938CAF650D257533091D1FDC2BD2C346")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFF800000000000000000000000000000"),
                    CipherText = new BitString("ACE5252ECA1F107C107ABBB1AAC10628E2A37580116CFB71")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFC00000000000000000000000000000"),
                    CipherText = new BitString("6D0FE540A236BEAD4976F6204EF66D1BE0B3A00785917C7E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFE00000000000000000000000000000"),
                    CipherText = new BitString("7E27766A9085C55B156FFD0856BB68DD733D41F4727B5EF0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFF00000000000000000000000000000"),
                    CipherText = new BitString("414E7F3C7627CF963FCBE8426BD075EDA99EBB030260826F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFF80000000000000000000000000000"),
                    CipherText = new BitString("CA16DCEA7110B21604A271CD3BFA5AB473F34C7D3EAE5E80")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFC0000000000000000000000000000"),
                    CipherText = new BitString("DC61AC1C50578C187E549E29254FD1AE40EBD5AD082345B7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFE0000000000000000000000000000"),
                    CipherText = new BitString("EDEA1C9B79C118BCCC32C20E6451AEB77CC4AE9A424B2CEC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFF0000000000000000000000000000"),
                    CipherText = new BitString("2CB8B5D7FF5FA1458FABC799D411A30B54D632D03ABA0BD0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFF8000000000000000000000000000"),
                    CipherText = new BitString("605BF340B9111EB561DB8074D79F4EEAD3427BE7E4D27CD5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFC000000000000000000000000000"),
                    CipherText = new BitString("6DB42A1A41426DA826E69B8197FF9504B2099795E88CC158")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFE000000000000000000000000000"),
                    CipherText = new BitString("108196A0AB4D01774FB12FB8C9F488C6A6CAE46FB6FADFE7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFF000000000000000000000000000"),
                    CipherText = new BitString("A5E4F13E1879AF74DF607B3E63D99D0D026A7024D6A902E0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFF800000000000000000000000000"),
                    CipherText = new BitString("B8541C7D05461AA1ED5CDA03DCA89001156F07767A85A431")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000"),
                    CipherText = new BitString("BF81A2B87A8F9F6504AA929C38A952AE15EEC9EBF42B9CA7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000"),
                    CipherText = new BitString("BF7C6EA718D3BB63FC7EDDF23E0AEFB4DB0D3A6FDCC13F91")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000"),
                    CipherText = new BitString("76FB66B59E34BA15661A61B6A74E7AE171DBF37E87A2E34D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000"),
                    CipherText = new BitString("0F5EB2B343CB2C186F2BF2701D1F3CAEC745C451E96FF3C0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000"),
                    CipherText = new BitString("2DBB55ED1C65E417A255323630230651340DA09C2DD11C3B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000"),
                    CipherText = new BitString("153F278C34CAD603C9A17F32191D68248279F7C0C2A03EE6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000"),
                    CipherText = new BitString("C6DB35F78C6F4B464801C60E04EE5182A4B2C7D8EBA531FF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000"),
                    CipherText = new BitString("97AD81016BA32CB5D89B235FF259851A74569A2CA5A7BD51")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000"),
                    CipherText = new BitString("0D0EB792F93DCB37296B0BDC27C97B023713DA0C0219B634")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000"),
                    CipherText = new BitString("7AC88F4E7FAF51301246E0FD7D8C5E938827551DDCC9DF23")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000"),
                    CipherText = new BitString("A2BA0CD44E0CF7CFAA35FB5D5CAFD0CF2E3FEBFD625BFCD0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000"),
                    CipherText = new BitString("7B157892A99A7BD4EE2A21BC6D5FC010EE82E6BA488156F7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000"),
                    CipherText = new BitString("1AD132B189786FA6F090A7B03DC459A04770446F01D1F391")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000"),
                    CipherText = new BitString("290B25579B3535577ACAFE96A8296AB0AF04B68F104F21EF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000"),
                    CipherText = new BitString("665D2B9B4B119F8ACDCA4E66B29D6C79CF3579A9BA38C8E4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000"),
                    CipherText = new BitString("32C42F533CAF48ED08CCDE783C3EB7FDB3BBA904F4953E09")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000"),
                    CipherText = new BitString("BFD0207CB50FF8DE14A2148B96CB9484FC4249656E14B29E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000"),
                    CipherText = new BitString("4978D20E9B26E7EFF2EA16E7E2F8A29B9B31568FEBE81CFC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000"),
                    CipherText = new BitString("B361D990BBDF95A511749196E820D9B49CA09C25F273A766")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000"),
                    CipherText = new BitString("24A174077543C853817194AA75485F03B909925786F34C3C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000"),
                    CipherText = new BitString("D5D5899AFFC3BD31DDD887618958FA1382647F1332FE570A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000"),
                    CipherText = new BitString("69F080607626404AF4393953C7FB05D83604A7E80832B3A9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000"),
                    CipherText = new BitString("622B40EA826A5403B9BD11EFA78C3416884607B128C5DE3A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000"),
                    CipherText = new BitString("E34B673A7656C30E5BDAB1BFD2FD0B43670CFA093D1DBDB2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000"),
                    CipherText = new BitString("C787AE938428617363198631ED8D19F87A867195F3CE8769")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000"),
                    CipherText = new BitString("863818BC22BA059169D260777F93364E52EFCF64C72B2F7C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000"),
                    CipherText = new BitString("B38E3796EB0BA8BB88ED326059850DEF4019250F6EEFB2AC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000"),
                    CipherText = new BitString("FEF4D249A32E89A27C1560D13A8EAE5C022C4F6F5A017D29")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000"),
                    CipherText = new BitString("088A28DD4638C0E2DC5B2AAAD7806619E9C21078A2EB7E03")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000"),
                    CipherText = new BitString("7E53B38DEADD69F6E1A1EE175BBA6577A13EAEEB9CD391DA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000"),
                    CipherText = new BitString("7903A96F9A789F11FC1E7599A3C7A3B8C958A171DCA1D4ED")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000"),
                    CipherText = new BitString("BCCA8193DC258B1A7549293566CB65C821442E07A110667F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000"),
                    CipherText = new BitString("43D668040FBDD95A75387EA6F78386F759BBB353CF1DD867")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000"),
                    CipherText = new BitString("99224058FCCD032B5A6DEEB8ED92BA3443CD3B25375D0CE4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000"),
                    CipherText = new BitString("6743FFBBAF448F8BD8B474A68080CB0E6B98B17E80D1118E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000"),
                    CipherText = new BitString("B3EF862F52A489C905F5DC089321B6CFAE47ED3676CA0C08")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000"),
                    CipherText = new BitString("85C91BE861C37ACA700CEC80CC299C2D34EC40DC20413795")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000"),
                    CipherText = new BitString("D7E4E1A7302CF9105BFF075D973C282B4DC68163F8E98354")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000"),
                    CipherText = new BitString("2607413D510AC06AF3B7B7548E9832032AABB999F4369317")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000"),
                    CipherText = new BitString("0F56FC40B7FB3CA1B0CA7468D1FDC441E01F94499DAC3547")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000"),
                    CipherText = new BitString("C5FA1DD0E696FAD5E13CF4C23F30AAB99D12435A46480CE0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000"),
                    CipherText = new BitString("C119B47E4A54CF42B298319A1DB743BBCEF41D16D266BDFE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000"),
                    CipherText = new BitString("8E8DD724FF38345DEE4441A91E442485B13DB4DA1F718BC6")
                },
            };

            return result;
        }

        public static List<AlgoArrayResponse> GetVarKey256BitKey()
        {
            var result = new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("800000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("907F2D7DB1FC390FE56BDE026B85A8F6DDC6BF790C15760D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("C00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A98193967F6A55F7CB3A0AE08999BCEC0A6BDC6D4C1E6280")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("E00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("11E2334DD7D40AAF222738CAB9DA28059B80EEFB7EBE2D2B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("97CBB26FFBB5EB6EA714EC2A2C049E787F2C5ECE07A98D8B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("F80000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D5C6F2BDDDF126D813920D60F46469467818D800DCF6F4BE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FC0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("06264831BB8461411443430241DEEE27E74CD1C92F0919C3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FE0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6F228218D652D708C17C4BC83703A8CF8092A4DCF2DA7E77")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FF0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7C4F0EB880773BAC9A5F492BBCA26B7049AF6B372135ACEF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FF8000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("44E9A31A3EF01A85C48B258EE0AD80378BCD40F94EBB63B9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFC000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A3475B13496DCD9AE68D88550D533788FE1CFFB83F45DCFB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFE000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("72DED9666CA034658E35E45468F5F83E0DC58A8D88662370")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFF000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("88B9274D74DFD95EF70F2C8C59FE8E75C218FAA16056BD07")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFF800000000000000000000000000000000000000000000"),
                    CipherText = new BitString("87891FB7625CB4EF00016917B5D4F585047BBA83F7AA8417")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFC00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("C94FA9A2B73B6A3F8B972CE482B60D27DC8F0E4915FD81BA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFE00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1152BBA2081C9B89E8F3F917DD3A24131569859EA6B7206C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFF00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("E77B5C94C883D649346CAF1CDE1C7CCF300ADE92F88F48FA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFF80000000000000000000000000000000000000000000"),
                    CipherText = new BitString("A74D6244F288B0E2049C3153E0983A691FE6CC3C05965DC0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFC0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("DB8AA469C6A17118D0EF85AA396F7F1359E858EAAA97FEC3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFE0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("FA9D18A8DCFB0E9D78091E43CC5A09E42239455E7AFE3B06")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFF0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("D6878BAE31AF0A24E460F4B5779E399C3EE500C5C8D63479")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFF8000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8F5386809BC55391B4ADC559EBD8028FD5E38BF15F16D90E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFC000000000000000000000000000000000000000000"),
                    CipherText = new BitString("43F116F98404385D405D5BB9A643BD23B1F4066E6F4F187D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFE000000000000000000000000000000000000000000"),
                    CipherText = new BitString("38F370211C8C399A6CDBF9B393B849396EF4CC4DE49B1106")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFF000000000000000000000000000000000000000000"),
                    CipherText = new BitString("17B1B54D94F3833A4C36DD258ED2975BAC86BC606B6640C3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFF800000000000000000000000000000000000000000"),
                    CipherText = new BitString("4D96CBE18637B9AA6985FBA22AFF247936AFF0EF7BF32807")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFC00000000000000000000000000000000000000000"),
                    CipherText = new BitString("8E4D1A4BFDFFF9FE34BD26E79D04C4151F8EEDEA0F62A140")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFE00000000000000000000000000000000000000000"),
                    CipherText = new BitString("FE225D6B70F6E568BA19C2E7631027C6ABF4154A3375A1D3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFF00000000000000000000000000000000000000000"),
                    CipherText = new BitString("DB334737FE4C26BA6DF22947E03FE55E96F96E9D607F6615")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFF80000000000000000000000000000000000000000"),
                    CipherText = new BitString("ABB2158A928D8CECBDEABBE4ED9759DDCF37CDAAA0D2D536")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFC0000000000000000000000000000000000000000"),
                    CipherText = new BitString("09833D90DBFAF1390288A3C70D435FC9FBD6640C80245C2B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFE0000000000000000000000000000000000000000"),
                    CipherText = new BitString("731BEA51C470FC466EE9DDB5E4210F848D6A8AFE55A6E481")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFF0000000000000000000000000000000000000000"),
                    CipherText = new BitString("C9891B1098115C1EEF813C5EFEC488B26A4981F2915E3E68")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFF8000000000000000000000000000000000000000"),
                    CipherText = new BitString("AB3012ED85FBF8986409A6600FCD915042A1136E5F8D8D21")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFC000000000000000000000000000000000000000"),
                    CipherText = new BitString("5B689E4EA6E9A10B262A6295255FB9B09B471596DC69AE15")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFE000000000000000000000000000000000000000"),
                    CipherText = new BitString("A7216EF52951975700C982AAECA1F658753665C4AF1EFF33")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFF000000000000000000000000000000000000000"),
                    CipherText = new BitString("290DB46D85E169EF9AEE583E0ABD04ED9A682ACF40BE01F5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFF800000000000000000000000000000000000000"),
                    CipherText = new BitString("7C7622C03B0CFF5A05B9829F4AE03EE654FAFE26E4287F17")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFC00000000000000000000000000000000000000"),
                    CipherText = new BitString("B1BA96A1F4AD999DA5B3A573558399A049D541B2E74CFE73")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFE00000000000000000000000000000000000000"),
                    CipherText = new BitString("972AACECAC1DF7F0A0E4A9A20B785DC811A45530F624FF6F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFF00000000000000000000000000000000000000"),
                    CipherText = new BitString("925B9EFD313C3B02DFE54C4AB13E3FF2F96B0C4A8BC6C861")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFF80000000000000000000000000000000000000"),
                    CipherText = new BitString("5B168AE5698865E4424A204492535FF948C7D0E80834EBDC")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFC0000000000000000000000000000000000000"),
                    CipherText = new BitString("1298F6B7C021518710DBC87B53E00B9C2463531AB54D6695")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFE0000000000000000000000000000000000000"),
                    CipherText = new BitString("F55611DD099F46F524CCA95E34334453AC9BD8E253046913")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFF0000000000000000000000000000000000000"),
                    CipherText = new BitString("803383E312E05EDF1F76DF72CBE180173F5F9106D0E52F97")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFF8000000000000000000000000000000000000"),
                    CipherText = new BitString("9ECC8BF75E28FA9F74972FAB78DC466120EBC86F1304D272")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFC000000000000000000000000000000000000"),
                    CipherText = new BitString("9FC8D0CC6528112AEDDB2C6DBFD04D97E67AE6426BF9526C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFE000000000000000000000000000000000000"),
                    CipherText = new BitString("5BC805569EF36E1A416E7FAF78B483891A518DDDAF9EFA0D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFF000000000000000000000000000000000000"),
                    CipherText = new BitString("140DFBB7A54036D16F900F41314F3F18EAD731AF4D3A2FE3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFF800000000000000000000000000000000000"),
                    CipherText = new BitString("99F4ACFA8C80476738D6605961D85DE0B1D4EFE40242F83E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFC00000000000000000000000000000000000"),
                    CipherText = new BitString("439A93F2CE5B31585544C3BFF48A3132CD2B1FEC11FD906C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFE00000000000000000000000000000000000"),
                    CipherText = new BitString("3C4DD06893EDFF795D032BC89CB9F276A1853FE47FE29289")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFF00000000000000000000000000000000000"),
                    CipherText = new BitString("7F5D8CA8577B5A98A149204D5C4D1A6C4632154179A555C1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFF80000000000000000000000000000000000"),
                    CipherText = new BitString("7E1169312CED1E28954375ABAFC8E310DD27CAC6401A022E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFC0000000000000000000000000000000000"),
                    CipherText = new BitString("E1191FABE767AE18CCF1A4DFF6D38C1DC090313EB98674F3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFE0000000000000000000000000000000000"),
                    CipherText = new BitString("141E9933F2ACFDD8DBC7D20B6F5A2A39CC3526262B92F02E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFF0000000000000000000000000000000000"),
                    CipherText = new BitString("B83B09152651D7BD1D77BAC8D8FD096CC0838D1A2B16A7C7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFF8000000000000000000000000000000000"),
                    CipherText = new BitString("D45B277F2A514EF9EE26430E489FC2CF0D9AC756EB297695")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFC000000000000000000000000000000000"),
                    CipherText = new BitString("BF906D64072866BC5C22C38D11A5869F56EDE9DDA3F6F141")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFE000000000000000000000000000000000"),
                    CipherText = new BitString("750015FEF7D327D9B6DAD81AB8BB4F19768F520EFE0F23E6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFF000000000000000000000000000000000"),
                    CipherText = new BitString("EDF888D1A80124F04B7A79E4FBD6CA02B1144DDFA7575521")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFF800000000000000000000000000000000"),
                    CipherText = new BitString("16DB96F71CA5F2013FC38952C652F1931D7C0C4040B355B9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFC00000000000000000000000000000000"),
                    CipherText = new BitString("EAC5A464787F1ACF463E4383EE070C5ED8E2BB1AE8EE3DCF")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFE00000000000000000000000000000000"),
                    CipherText = new BitString("219BE5F577976BD973DD04B00820686EFAF82D178AF25A98")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFF00000000000000000000000000000000"),
                    CipherText = new BitString("8122B5A1882C6FE512716603AB25E44B9B58DBFD77FE5ACA")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFF80000000000000000000000000000000"),
                    CipherText = new BitString("3B9B3AD27E55E7E760B80CB390A63DF577F392089042E478")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFC0000000000000000000000000000000"),
                    CipherText = new BitString("8B8BC100F85BF8FAC72CFF4329DC59BD19F08E3420EE69B4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFE0000000000000000000000000000000"),
                    CipherText = new BitString("C4E715942C3E261212BD8AECC315E5BEA1B19BEEE4E11713")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFF0000000000000000000000000000000"),
                    CipherText = new BitString("00E7FAB76BB7915D32067DA49CA576A6A37A5869B218A9F3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFF8000000000000000000000000000000"),
                    CipherText = new BitString("7C0AC4716472C1834CE15DD48843E596BC3594E865BCD026")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFC000000000000000000000000000000"),
                    CipherText = new BitString("6AEE8C936B0647A470FBF5E02D0DC695811441CE1D309EEE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFE000000000000000000000000000000"),
                    CipherText = new BitString("44D22BCA2BEEAF35FBCF45FDDAFC07BD959971CE41341905")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFF000000000000000000000000000000"),
                    CipherText = new BitString("590DCB898248C19D593EB80056D7F92B76B5614A042707C9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFF800000000000000000000000000000"),
                    CipherText = new BitString("9D59D82BA18EA7EAE8BDB5D4C38E94A77D9FA6A57530D0F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFC00000000000000000000000000000"),
                    CipherText = new BitString("F8A91D37BE2D4A71E346B99F938A5B0C964153A83BF6989A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFE00000000000000000000000000000"),
                    CipherText = new BitString("23128E7D5BBF5B04E606F02CFF994EF4A013014D4CE8054C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFF00000000000000000000000000000"),
                    CipherText = new BitString("79078E5716E5D83A5BE972CCAA2AEE34D1C5F6399BF38250")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFF80000000000000000000000000000"),
                    CipherText = new BitString("370CCD3A50CCAE62717E0880A335CFAA0007E20B8298EC35")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFC0000000000000000000000000000"),
                    CipherText = new BitString("B993A9A051E2D381FB8D860FB74D61DEB95BA05B332DA61E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFE0000000000000000000000000000"),
                    CipherText = new BitString("1E8D39365A2A23BD51F809CE3896C3944620A49BD9674915")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFF0000000000000000000000000000"),
                    CipherText = new BitString("AE9D735ED7B283F3319C77F12D39852A12E71214AE8E04F0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFF8000000000000000000000000000"),
                    CipherText = new BitString("A88F2519DF2F8CE249A29C86411234C14CC42FC1407B008F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFC000000000000000000000000000"),
                    CipherText = new BitString("E929DC4EFA6483DEF378E96F7223A17E08B244CE7CBC8EE9")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFE000000000000000000000000000"),
                    CipherText = new BitString("BC870AECE726F8C28901FA558B95BAEC39B333E8694F2154")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFF000000000000000000000000000"),
                    CipherText = new BitString("3E0C6E2A5BFABA5E7DD927787582ACF53B271F8AB2E6E4A2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFF800000000000000000000000000"),
                    CipherText = new BitString("D113A58691D0FA624EC78B3600BEEC9A9AD983F3BF651CD0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFC00000000000000000000000000"),
                    CipherText = new BitString("AEBAF996025A7E6AA67DF020A87554848F476CBFF75C1F72")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000"),
                    CipherText = new BitString("E5ED7EB2486081521A329E6C15BDC571905B6267F1D6AB53")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000"),
                    CipherText = new BitString("84D517CF8F48135EAE009F6D17206452145B60D6D0193C23")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFF80000000000000000000000000"),
                    CipherText = new BitString("451DFBFC4E2B7972720CB34DD6EC834555CFB3FB6D75CAD0")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000000"),
                    CipherText = new BitString("F1CBBF3B4EB52D59DDDFCF25257FE4247B8E7098E357EF71")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000"),
                    CipherText = new BitString("0AD55468B50068EA9BAB6272FFEFDEB22BF27229901EB40F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000000"),
                    CipherText = new BitString("D9DF9A53DB2B46E9038A687884E0DB5783A63402A77F9AD5")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000000"),
                    CipherText = new BitString("E3A650B4B49E2860242FB12EEB4862186F8BA6521152D31F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000000"),
                    CipherText = new BitString("352AC6FE6BE75E3F7008B84BB49D7C53E5C3B8E30FD2D8E6")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000000"),
                    CipherText = new BitString("520B5207AD4BAD41E92B5E3E1CBBE6301AC1F7102C59933E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000000"),
                    CipherText = new BitString("15EF8F7A5D42273FA7273C715B8C609B21D9BA49F276B45F")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000000"),
                    CipherText = new BitString("8E0CD557F13568914C62E2638D852BA2649F1CDDC3792B46")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000000"),
                    CipherText = new BitString("E3F6703321F4935D7C5B5D099755C1D0E2775E4B59C1BC2E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000"),
                    CipherText = new BitString("DAFB53FEF9DF8E76016FC68E6406ABD92BE1FAE5048A2558")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000"),
                    CipherText = new BitString("114F076CE8F2ACB957877CB2CBC0023BDA86F292C6F41EA3")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000000"),
                    CipherText = new BitString("BAC534D63A263A9594DD951F5694186F220DF19F85D69B1B")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000000"),
                    CipherText = new BitString("5FCAF8693646C70BF036768FA4AF01841F11D5D0355E0B55")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000"),
                    CipherText = new BitString("3AAB08E23C199C10ACD858E408BBD6F662526B78BE79CB38")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000000"),
                    CipherText = new BitString("4655D78509E38C4185930E406BDB689290DDBCB950843592")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000000"),
                    CipherText = new BitString("06370B200B3A33315D6DAAE80E4181502FD0E41C5B840227")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000000"),
                    CipherText = new BitString("96E2D02291E1FC9607828D06A64C74E93CDF13E72DEE4C58")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000000"),
                    CipherText = new BitString("97A2856E7027F904A8B31A73D1D8851DAFA2FFC137577092")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000"),
                    CipherText = new BitString("97E72A5A346A0E0815B10891BCF84B088D683EE63E60D208")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000000"),
                    CipherText = new BitString("14EBA3183C7A5F0231572040384382C2705A4EF8BA213372")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000000"),
                    CipherText = new BitString("0C5E4181EC49FA0059E28ADE6F7FA3C20861A861C3DB4E94")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000"),
                    CipherText = new BitString("E2A18847798BFA584EC779A91F30CFA84B00C27E8B26DA7E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000"),
                    CipherText = new BitString("1372ADFD77253D996FBA02F993CC91145F397BF03084820C")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000000"),
                    CipherText = new BitString("210999A6AF9360EEE9180C81C4AB86E263FAFABB72C07BFB")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000000"),
                    CipherText = new BitString("92B7AE06C2B4D9721AD3B1BEFE355774683E2140585B1845")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000"),
                    CipherText = new BitString("334FC3EFB739DF736B3FB0C9B980CF72286894E48E537F87")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000000"),
                    CipherText = new BitString("53E6EE662FEA9D597DD100974FE89495A423DEABC173DCF7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFF8000000000000000000"),
                    CipherText = new BitString("DBF169500E049055ECBFB68955C0B7C8EB8168313E1CFDFD")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFC000000000000000000"),
                    CipherText = new BitString("94593951211B1EC0502EB6D9BDE8BA8E27127DAAFC9ACCD2")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE000000000000000000"),
                    CipherText = new BitString("CB9DB8EE4D62AD8FC346593AB351C588EE0715B96F72E3F7")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000"),
                    CipherText = new BitString("78DE45EACE04EDA3B6A61D93C5A18E5829EE526770F2A11D")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF800000000000000000"),
                    CipherText = new BitString("58D7B7B1FAEC4FA1D4954DC5CDFB4C6B0493370E054B0987")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC00000000000000000"),
                    CipherText = new BitString("16253CE10D1ABC27BE7926B58BD67A859B7B940F6C509F9E")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000"),
                    CipherText = new BitString("2BEE01A3E99A7DA7FC46C02034EB4C742915BE4A1ECFDCBE")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000"),
                    CipherText = new BitString("094F81C5F74A0FF3770A5B99757981CE7240E524BC51D8C4")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF80000000000000000"),
                    CipherText = new BitString("C1B16E2A8E170F05D4DF5DF486F70E5DDA63039D38CB4612")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC0000000000000000"),
                    CipherText = new BitString("CCF68C4726D45B468A4EC56B49CB38B70F59CB5A4B522E2A")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000"),
                    CipherText = new BitString("5D054CFA019023601009E010ABE1304E7BFE9D876C6D63C1")
                },
                new AlgoArrayResponse()
                {
                    Key = new BitString("0000000000000000000000000000000000000000000000000000000000000000"),
                    IV = new BitString("00000000000000000000000000000000"),
                    PlainText = new BitString("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0000000000000000"),
                    CipherText = new BitString("92D2CBFE7BF8F175EBC0D77CC66BAB48ACDACE8078A32B1A")
                },
            };

            return result;
        }
        #endregion VarKey
    }
}
