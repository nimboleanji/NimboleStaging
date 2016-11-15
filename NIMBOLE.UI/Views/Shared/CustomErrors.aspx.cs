using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NIMBOLE.UI.Views.Shared
{
    public partial class CustomErrors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var CommingException=Server.GetLastError();
            if (CommingException != null)
            {
                lblError.Text = "ERROR: "+CommingException.Message+" \n InnerException: "+CommingException.InnerException.Message;
            }
        }
    }
}