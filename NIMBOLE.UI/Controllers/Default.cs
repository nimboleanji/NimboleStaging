using MyResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIMBOLE.UI.Controllers
{
    public class Default
    {
        public void setCultureByUser(string strCultureCode)
        {
            if (CultureHelper.CultureName == null)
            {
                CultureHelper.CultureName = strCultureCode;//Set Default Language
            }
            if (!strCultureCode.Equals(""))
            {
                CultureHelper.CultureName = strCultureCode;
            }
            else
            {
                CultureHelper.CultureName = "en";
            }
            //ReadCultureFromCookies();
            CultureHelper.ModifyCurrentCulture(CultureHelper.CultureName);
        }
    }
}