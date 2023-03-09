using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        SimpleBL OBJbL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }       
        protected void lnkbutton_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("frmlogin.aspx");
        }
    }
}