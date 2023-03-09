using SimpleTest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmChangePassword : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click1(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text.Trim() != "" && txtPassword.Text.Trim() != "" && txtNPassword.Text.Trim() != "")
                {
                    DataTable _gdtDetails = objBL.getLoginDetails(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                    if (_gdtDetails.Rows.Count > 0)
                    {
                        int UpdateData = objBL.UpdatePassword(txtUserName.Text.Trim(), txtPassword.Text.Trim(), txtNPassword.Text.Trim());
                        if (UpdateData == 1)
                        {
                            SimpleMethods.MsgBoxWithLocation("Password has been Changed Successfully.", "frmlogin.aspx", this);
                            return;
                        }
                    }
                    else
                    {
                        SimpleMethods.show("Kindly Enter Correct User Name or Password.");
                        return;
                    }
                }
                else
                {
                    SimpleMethods.show("Kindly Enter User Name or Password.");
                    return;
                }
            }
            catch (Exception ex)
            {
                SimpleMethods.show(ex.Message.ToString());
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmHome.aspx");
        }
    }
}