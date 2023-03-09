using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmHome : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string _sDiv1 = string.Empty;
            string _sDiv2 = string.Empty;
            if (Session["UserName"] != null)
            {
                if (!IsPostBack)
                {
                    lblCompany.Text = Session["COMPANY"].ToString();
                    lblEmpCode.Text = Session["UserName"].ToString();
                    lblName.Text = Session["EMP_NAME"].ToString();
                    if (Session["ROLE"].ToString() == "R" || Session["ROLE"].ToString() == "A")
                    {
                        string Division = Convert.ToString(Session["Divison"]).Replace("'", "");
                        string[] Division1 = Division.Split(',');
                        if (Division1.Length > 11)
                        {
                            for (int i = 0; i < 11; i++)
                            {
                                _sDiv1 += Division1[i] + ",";
                            }
                            lblDivision.Text = _sDiv1;

                            for (int i = 11; i < Division1.Length; i++)
                            {
                                _sDiv2 += Division1[i] + ",";
                            }
                            lblDivision1.Text = _sDiv2;
                        }
                        else if (Division1.Length > 1)
                        {
                            for (int i = 0; i < Division1.Length; i++)
                            {
                                _sDiv1 += Division1[i] + ",";
                            }
                            lblDivision.Text = _sDiv1;
                        }
                    }
                    else
                    {
                        lblDivision.Text = Convert.ToString(Session["Divison"]);
                    }
                    GetRole_Description();
                    if (Session["ROLE"].ToString() == "S")
                    {
                        TRDiv_Row.Visible = false;
                    }
                }
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                Response.Redirect("frmlogin.aspx");
            }
        }
        private void GetRole_Description()
        {
            DataTable _dtRole = new DataTable();
            _dtRole = objBL.getRoleRightName_IDWise(Session["LOGIN_TYPE"].ToString());

            if (_dtRole.Rows.Count > 0)
            {
                lblEmployeeType.Text = _dtRole.Rows[0][0].ToString();
            }
        }
    }
}