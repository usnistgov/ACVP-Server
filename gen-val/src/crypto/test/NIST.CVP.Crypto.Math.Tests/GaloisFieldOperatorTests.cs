using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.Math.Tests
{
    [TestFixture, FastCryptoTest]
    public class GaloisFieldOperatorTests
    {
        [Test]
        #region b-163Modulo
        [TestCase(Curve.B163,
            "000546A325845A5851264FC701F01BC24E1845A6D28B",
            "000506C978E106DC71966F160A7B55CE4FC070DCA3BC",
            "406A5D655C8420B020D10B8B4E0C01D8357A7137",
            TestName = "Modulo b-163 #1")]
        [TestCase(Curve.B163,
            "00016B4D151003B03B497485296926E7601A4CED2B16",
            "00065425457F265B58BD27A819DF32C234333B7C16A5",
            "00016B4D151003B03B497485296926E7601A4CED2B16",
            TestName = "Modulo b-163 #2")]
        [TestCase(Curve.B163,
            "00040C193CC71F6A5BE771ED235572E15E653D16D01C",
            "00073C540CCA1EA25C5B129F05F97C2B30C175AC277D",
            "0003304D300D01C807BC637226AC0ECA6EA448BAF761",
            TestName = "Modulo b-163 #3")]
        [TestCase(Curve.B163,
            "00030E4D530849CD4A0D4154425A5DE775D45B059406",
            "00031FEE6FAE3BDC263536EC202F3221734C6A3FCB7B",
            "11A33CA672116C3877B862756FC60698313A5F7D",
            TestName = "Modulo b-163 #4")]
        [TestCase(Curve.B163,
            "00041EC76D727ECA67245BA52D6842765BB76062CE2C",
            "0002287537C9453350F81F0C78591AC47DB936AC51E4",
            "4E2D02E0F4ACC6D465BDDDDA77FEA0C50D3A6DE4",
            TestName = "Modulo b-163 #5")]
        [TestCase(Curve.B163,
            "000157311DF42BC5417252BB38E345977A6841DD161E",
            "00036545009867C402021277183E236426B87FDD1DB4",
            "000157311DF42BC5417252BB38E345977A6841DD161E",
            TestName = "Modulo b-163 #6")]
        [TestCase(Curve.B163,
            "00017B904F7F7A8B6CCB440E5BAC064F14E354E7CD24",
            "067D3FAA27995BD847A9447D3F5C241B397165AF",
            "0165534BCD5950717898F9593D5321FF08DCA8C0",
            TestName = "Modulo b-163 #7")]
        [TestCase(Curve.B163,
            "0001527951B548DC45C425224D336933648B20AB3887",
            "0004041E6D2A7FB5367600B9694860C3114B7B7E796D",
            "0001527951B548DC45C425224D336933648B20AB3887",
            TestName = "Modulo b-163 #8")]
        [TestCase(Curve.B163,
            "00073DA12A6B231831203F514A962B2C5666313ADFB4",
            "000334AB06712C7F19E35DFF2C097541384629271657",
            "000154F726897BE602E684AF1284C1AE26EA6374F31A",
            TestName = "Modulo b-163 #9")]
        [TestCase(Curve.B163,
            "00056585500D39787A9F15A9650021CB06687542544C",
            "000135ED225372A50FD261CE33805AE874E60535B4E6",
            "87DCFB1381494A04F35F98811082A11664A13332",
            TestName = "Modulo b-163 #10")]
        #endregion b-163Modulo
        public void ShouldModuloOverGaloisFieldCorrectly(Curve curve, string xHex, string mHex, string yHex)
        {
            var x = LoadValue(xHex);
            var m = LoadValue(mHex);
            var y = LoadValue(yHex);

            var subject = new GaloisFieldOperator(m);
            var result = subject.Modulo(x);

            Assert.AreEqual(y, result);
        }

        [Test]
        #region b-163Addition
        [TestCase(Curve.B163,
            "00040ED077DB09D559CA50B00E5C2A2E6468281F47BA",
            "00021374104310754DF0357C5050371E19C335D513F4",
            "00061DA4679819A0143A65CC5E0C1D307DAB1DCA544E",
            TestName = "Addition b-163 #1")]
        [TestCase(Curve.B163,
            "00022D122DE4462C56C95B8520C73115109E6E9DB019",
            "00023ADD020015107D630F132C7705A22C081153E722",
            "17CF2FE4533C2BAA54960CB034B73C967FCE573B",
            TestName = "Addition b-163 #2")]
        [TestCase(Curve.B163,
            "00064B677D4431AF7D184CD0440376C5731C6733D0E7",
            "00013E320B7F4B0A7D471E61345364AF232F46608627",
            "00077555763B7AA5005F52B17050126A5033215356C0",
            TestName = "Addition b-163 #3")]
        [TestCase(Curve.B163,
            "00037AE97BD74F5C55CD73AC29E92D556F97466EFE1A",
            "2FD02970779D481D20F6051363B062292639A7B5",
            "0003553952A738C11DD0535A2CFA4EE50DBE605759AF",
            TestName = "Addition b-163 #4")]
        [TestCase(Curve.B163,
            "00032A053CEE7A967B6734CA598C2B2F1E7B542DEC7E",
            "20BA120841FD79F4403D11C62332434872A28FA0",
            "00030ABF2EE63B6B029374F7484A081D5D33268F63DE",
            TestName = "Addition b-163 #5")]
        [TestCase(Curve.B163,
            "000115D6092B40ED18D0365D460816453F3F70EC5E51",
            "000705E433F90B26678C5A05234B7E553DD66DEE5A15",
            "000610323AD24BCB7F5C6C586543681002E91D020444",
            TestName = "Addition b-163 #6")]
        [TestCase(Curve.B163,
            "0005112461A2242945FC53FA008A246F747467F4C415",
            "000248E71D5232BC55A831D943157FE3122772C6943F",
            "000759C37CF0169510546223439F5B8C66531532502A",
            TestName = "Addition b-163 #7")]
        [TestCase(Curve.B163,
            "0004479C3E0078D74F395492137870A1378803DB0CDC",
            "000446BC2A1452A358A121566A194E5F304D3EF77FB7",
            "012014142A74179875C479613EFE07C53D2C736B",
            TestName = "Addition b-163 #8")]
        [TestCase(Curve.B163,
            "0002281E12D46A43726C6F0338BE60FC4C244271692D",
            "000231BC52D01300009E23EA51822E7121ED3651ECA5",
            "19A24004794372F24CE9693C4E8D6DC974208588",
            TestName = "Addition b-163 #9")]
        [TestCase(Curve.B163,
            "551C62FF4B1057E333C964A8614566F427C693E0",
            "000522260A766BD1678D08E44B6E2395417D23994A43",
            "0005773A688920C1306E3B2D2FC642D02789045FD9A3",
            TestName = "Addition b-163 #10")]
        #endregion b-163Addition
        public void AdditionShouldAddTwoValuesOverGF(Curve curve, string xHex, string yHex, string resultHex)
        {
            var x = LoadValue(xHex);
            var y = LoadValue(yHex);
            var m = GetModulo(curve);
            var expectedResult = LoadValue(resultHex);

            var subject = new GaloisFieldOperator(m);
            var result = subject.Add(x, y);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        #region b-163Division
        [TestCase(Curve.B163,
            "0007580A4CA44F6A7D47094F3BCC1A4D6FD0635BAB62",
            "00055AA316DE12B94539547327C20B4D66F62517EE26",
            "0001",
            TestName = "Division b-163 #1")]
        [TestCase(Curve.B163,
            "7DAD739E20585035397002790B9F2C161CEEF4B0",
            "00076ED23E0B3D5E029F0A9F470441927BE26377CAD1",
            "00",
            TestName = "Division b-163 #2")]
        [TestCase(Curve.B163,
            "52F03D62621D7AAA43F379B804387F3B24DE9130",
            "0005279065D55AB44C3E299F5430424F4CAC14D35949",
            "00",
            TestName = "Division b-163 #3")]
        [TestCase(Curve.B163,
            "000123A106A5780438D54B061C33405E3FF42358977B",
            "00061C5841E51CF8713E29DA6DED6D6F0B661CFDC5F1",
            "00",
            TestName = "Division b-163 #4")]
        [TestCase(Curve.B163,
            "08891B3166814728381E5C693A2E706110F3A886",
            "000509EA168119C8130563852D0D104E142E7810D27E",
            "00",
            TestName = "Division b-163 #5")]
        [TestCase(Curve.B163,
            "0007596C2C9F7B552D2F18DB009E7DB30B462E4F47ED",
            "00066A35364C233C3B7E1D745F8D68C208573AE82E44",
            "0001",
            TestName = "Division b-163 #6")]
        [TestCase(Curve.B163,
            "000147B902586EA572B3735853F02FC073F46F18EB83",
            "0007296A6E194B77444B2D7323C1657644AC437D7EB8",
            "00",
            TestName = "Division b-163 #7")]
        [TestCase(Curve.B163,
            "0007185929332F58527D6944017C4B967BB32C0C8A4D",
            "0006179149E34290080A4DC90BFA6D4F57020C85B019",
            "0001",
            TestName = "Division b-163 #8")]
        [TestCase(Curve.B163,
            "00077162000766EF51DE698C32C019550637682614F3",
            "00020A56627D0F632192358B1A8047B22FDD5C489156",
            "0003",
            TestName = "Division b-163 #9")]
        [TestCase(Curve.B163,
            "00044EBB40D228EB52311B9B71DD110961606BB187C2",
            "00043A2523FD53EC056A4FFE2A4E27AA1B5F0AD1BE99",
            "0001",
            TestName = "Division b-163 #10")]
        #endregion b-163Division
        public void DivisionShouldDivideTwoValuesOverGF(Curve curve, string xHex, string yHex, string resultHex)
        {
            var x = LoadValue(xHex);
            var y = LoadValue(yHex);
            var m = GetModulo(curve);
            var expectedResult = LoadValue(resultHex);

            var subject = new GaloisFieldOperator(m);
            var result = subject.Divide(x, y);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        #region b-163Multiply
        [TestCase(Curve.B163,
            "0004629867A34C9F60823EC76EDB73E1033A1F67C1FD",
            "0001416E02ED64266B4941A976A17D2250535DD56433",
            "0006E3D40FE13B6B7F87993710770375FCA76D8BE88F",
            TestName = "Multiply b-163 #1")]
        [TestCase(Curve.B163,
            "486143FA735F0A517304198108B745BF16994785",
            "0003683E5FD575856A8B26EE2656359A1DC62804C7C8",
            "00030752933CC534842348C455E3BA3F5B6CAD4BDB36",
            TestName = "Multiply b-163 #2")]
        [TestCase(Curve.B163,
            "000105F872BE45FE353A4B853FD653B9423B422ED933",
            "000334B51D821B1206EA307E5C604F0F037232A11AD1",
            "0002D135161E76C08AEB783245CC901C0D21BBA65D45",
            TestName = "Multiply b-163 #3")]
        [TestCase(Curve.B163,
            "00031A3D67AE2FD40425065931F5496A615D36F9DF6F",
            "00056C5F5E7A75A91E8773D7729E6ED8726919204872",
            "000507FEA8FE2610FEF990BEDF7938C99C0FE3784273",
            TestName = "Multiply b-163 #4")]
        [TestCase(Curve.B163,
            "000722073E253DA616C44AFD5C47187765EB40F29CFC",
            "000608965B2831C5615F1CEF1D1A0E33078C1142FC89",
            "0006784CCD2E1979BB9B11C35F8943B24F31A2EB3690",
            TestName = "Multiply b-163 #5")]
        [TestCase(Curve.B163,
            "00023D5612046E4A27383D1A7BA438E66A2E602497A5",
            "000643A1191F5E520D21572B409C0CCE31170254D11E",
            "000332C58E4982590ED8C28A190ECA7C48408E4F9BB3",
            TestName = "Multiply b-163 #6")]
        [TestCase(Curve.B163,
            "00042F0A67381ACE5C1872C61309698C538A52AA04C0",
            "227A29E9791D55EA2F77256A04720AA71C4FF5F4",
            "00050C07E4A7D87FD4AC419F1FB52B34A47386C6887A",
            TestName = "Multiply b-163 #7")]
        [TestCase(Curve.B163,
            "00020CE4522F76A83364292904A139D3099B399A4D20",
            "000675C7191C45F51ED14A3869841CCB2E651A78CABF",
            "00013283E34F15FFEA55C17C37A1452B5E4D51163C06",
            TestName = "Multiply b-163 #8")]
        [TestCase(Curve.B163,
            "00070A58322A428E2D883D1C6E0908B80AF63D4D1E17",
            "00054F301FD64142255658C6355F3906272961290077",
            "0006E67CFC5963F09793F073359B678AC476F46C600E",
            TestName = "Multiply b-163 #9")]
        [TestCase(Curve.B163,
            "000205BC5C3830795D4808142A3879A4518C003B8063",
            "13E07B63758268A2507864435A316AEA0DE31349",
            "0005E11CD5C1028F631BDA87A3F58A172F62C9BA224F",
            TestName = "Multiply b-163 #10")]
        #endregion b-163Multiply
        public void MultiplyShouldMultiplyTwoValuesOverGF(Curve curve, string xHex, string yHex, string resultHex)
        {
            var x = LoadValue(xHex);
            var y = LoadValue(yHex);
            var m = GetModulo(curve);
            var expectedResult = LoadValue(resultHex);

            var subject = new GaloisFieldOperator(m);
            var result = subject.Multiply(x, y);

            Assert.AreEqual(expectedResult, result);
        }

        private BigInteger GetModulo(Curve curve)
        {
            switch (curve)
            {
                case Curve.B163:
                    return LoadValue("0800000000000000000000000000000000000000c9");
            }

            throw new ArgumentOutOfRangeException();
        }

        private BigInteger LoadValue(string value)
        {
            return new BitString(value).ToPositiveBigInteger();
        }
    }
}
