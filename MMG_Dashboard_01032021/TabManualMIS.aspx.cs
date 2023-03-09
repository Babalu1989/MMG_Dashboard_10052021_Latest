using common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Threading;

namespace MMG_Dashboard_01032021
{
    public partial class TabManualMIS : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {           
            if (Session["UserName"] != null && Session["VENDOR_ID"] != null)
            {
                if (!IsPostBack)
                {
                    txtActivityFromDate.Text = DateTime.Now.AddDays(-7).ToString("dd-MM-yyyy");
                    txtActivityToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    Division();

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
        private void Division()
        {
            try
            {
                DataTable dtdiv = objBL.getDivisionDetails();
                if (dtdiv.Rows.Count > 0)
                {
                    ddlDiv.DataSource = dtdiv;
                    ddlDiv.DataTextField = "DIVISION_NAME";
                    ddlDiv.DataValueField = "DIST_CD";
                    ddlDiv.DataBind();
                    ddlDiv.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        private void Vendor(string Vendorid)
        {
            try
            {
                DataTable dtdiv = objBL.getVendorDetails(Vendorid);
                if (dtdiv.Rows.Count > 0)
                {
                    ddlVend.DataSource = dtdiv;
                    ddlVend.DataTextField = "NAME";
                    ddlVend.DataValueField = "VENDOR_ID";
                    ddlVend.DataBind();
                    ddlVend.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string orderNo = txtOrderNoSearch.Text.Trim().Replace("'", "`").Replace("$", "").Replace(",", " ").ToUpper();
            try
            {
                DataTable dtorder = objBL.GetDetailsMIS(orderNo, ddlPunchType.SelectedValue, ddlDiv.SelectedValue, ddlVend.SelectedValue,
                                                        txtActivityFromDate.Text, txtActivityToDate.Text);
                if (dtorder.Rows.Count > 0)
                {
                    if (dtorder.Rows.Count < 100)
                    {
                        GridView1.DataSource = dtorder;
                        GridView1.DataBind();
                        if (ViewState["Main"] != null)
                        {
                            ViewState["Main"] = null;
                        }
                        ViewState["Main"] = dtorder;
                        btnExport.Visible = true;
                    }
                    else
                    {
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                        ViewState["Main"] = null;
                        string _sfilename = "Details" + DateTime.Now.ToString() + ".xls";
                        ExportToExcel(dtorder, _sfilename);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('No Record Found!');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
                return;
            }
        }
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("TabManualMIS.aspx");
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable _dtExport = objBL.GetDetailsMIS(txtOrderNoSearch.Text, ddlPunchType.SelectedValue, ddlDiv.SelectedValue, ddlVend.SelectedValue,
                                                        txtActivityFromDate.Text, txtActivityToDate.Text);
            string _sfilename = "MISDetails" + DateTime.Now.ToString() + ".xls";
            ExportToExcel(_dtExport, _sfilename);
        }
        protected void ddlDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            Vendor(ddlDiv.SelectedValue);
        }
        public void ExportToExcel(DataTable _dtDetails, string _ExportFile)
        {
            HtmlForm form = new HtmlForm();
            Response.Clear();
            Response.Buffer = true;
            if (_dtDetails.Rows.Count > 0)
            {
                GridView grddetails = new GridView();
                grddetails.DataSource = _dtDetails;
                grddetails.DataBind();
                string filename = "Details" + DateTime.Now.ToString() + ".xls";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                form.Controls.Add(grddetails);
                this.Controls.Add(form);
                form.RenderControl(hw);
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                //HttpContext.Current.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.Close();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}