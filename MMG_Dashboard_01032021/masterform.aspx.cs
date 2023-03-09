using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class masterform : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            //objBL.Generate_Masterdata();
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            objBL.Generate_Masterdata();
        }
    }
}