using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccCurveFactory : IEccCurveFactory
    {
        public EccCurveBase GetCurve(Curve curve)
        {
            throw new NotImplementedException();
        }

        #region PCurves
        //private PCurve P192 = new PCurve();
        //private PCurve P224 = new PCurve();
        //private PCurve P256 = new PCurve();
        //private PCurve P384 = new PCurve();
        //private PCurve P521 = new PCurve();
        #endregion PCurves

        #region KCurves
        //private KCurve K163 = new KCurve();
        //private KCurve K233 = new KCurve();
        //private KCurve K283 = new KCurve();
        //private KCurve K409 = new KCurve();
        //private KCurve K571 = new KCurve();
        #endregion KCurves

        #region BCurves
        //private BCurve B163 = new BCurve();
        //private BCurve B233 = new BCurve();
        //private BCurve B283 = new BCurve();
        //private BCurve B409 = new BCurve();
        //private BCurve B571 = new BCurve();
        #endregion BCurves
    }
}
