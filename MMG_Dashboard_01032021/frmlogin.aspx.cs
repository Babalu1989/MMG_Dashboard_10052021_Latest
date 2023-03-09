///////////////////////////////////////////
/* Developer Name: Babalu Kumar
   PCN No.:411202005
   Req. No.:REQ04092020227611
   PCN Start Date:01-March-2021
   PCN Type:New PCN
    */
/////////////////////////////////////////////

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
    public partial class frmlogin : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string convertDivisionName(string DivisionName)
        {
            string result = string.Empty;
            if (DivisionName != "")
            {
                result = DivisionName.Replace(",", "','");
            }
            return result;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {             
                if (txtUserName.Text.Trim() != "UserID" && txtPassword.Text.Trim() != "Password")
                {
                    DataTable _gdtDetails = objBL.getLoginDetails(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                    if (_gdtDetails.Rows.Count > 0)
                    {
                        if (_gdtDetails.Rows[0][0].ToString() == "-1")
                        {
                            SimpleMethods.show("Unable to Connect with Database Server. Please Try Again Later...");
                            return;
                        }
                        else
                        {
                            Session["UserName"] = Convert.ToString(_gdtDetails.Rows[0]["LOGIN_ID"]);
                            Session["EMP_NAME"] = Convert.ToString(_gdtDetails.Rows[0]["EMP_NAME"]);
                            Session["COMPANY"] = Convert.ToString(_gdtDetails.Rows[0]["COMPANY"]);
                            Session["ROLE"] = Convert.ToString(_gdtDetails.Rows[0]["ROLE"]);
                            if (_gdtDetails.Rows[0]["VENDOR_ID"].ToString() != "")
                            {
                                Session["VENDOR_ID"] = Convert.ToString(_gdtDetails.Rows[0]["VENDOR_ID"]);
                                Session["Vendorname"] = Convert.ToString(_gdtDetails.Rows[0]["Vendorname"]);
                            }
                            else
                            {
                                Session["VENDOR_ID"] = "";
                                Session["Vendorname"] = "";
                            }
                            Session["Divison"] = convertDivisionName(Convert.ToString(_gdtDetails.Rows[0]["division"]));
                            Session["LOGIN_TYPE"] = Convert.ToString(_gdtDetails.Rows[0]["LOGIN_TYPE"]);
                            Response.Redirect("frmHome.aspx", false);
                        }
                    }
                    else
                    {
                        SimpleMethods.show("User Id or password invalid please try again.");
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
    }
}