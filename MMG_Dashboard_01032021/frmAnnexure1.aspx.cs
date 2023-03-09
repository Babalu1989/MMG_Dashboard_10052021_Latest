using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmAnnexure1 : System.Web.UI.Page
    {
        double METERTYPE_1PH = 0;
        double METERTYPE_3PH = 0;
        double CABLE_INS_2X10 = 0;
        double CABLE_INS_2X25 = 0;
        double CABLE_INS_4X25 = 0;
        double CABLE_INS_4X50 = 0;
        double CABLE_INS_4X150 = 0;
        double CABLE_REMOVED_2X10 = 0;
        double CABLE_REMOVED_2X25 = 0;
        double CABLE_REMOVED_4X25 = 0;
        double CABLE_REMOVED_4X50 = 0;
        double CABLE_REMOVED_4X150 = 0;
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null && Session["VENDOR_ID"] != null)
            {
                if (!IsPostBack)
                {
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
        private void Get_Annexure1(string Vendor, string Month, string Year)
        {
            try
            {
                DataTable dtbill = objBL.GetAnnexure1(Vendor, Month, Year);
                if (dtbill.Rows.Count > 0)
                {
                    rptrMMGWorkSummty.DataSource = dtbill;
                    rptrMMGWorkSummty.DataBind();
                }
                else
                {
                    rptrMMGWorkSummty.DataSource = null;
                    rptrMMGWorkSummty.DataBind();
                    btnGenerate.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Records not found for select month and year!');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        private void Division()
        {
            try
            {
                DataTable dtdiv = objBL.getDivisionDetails();
                if (dtdiv.Rows.Count > 0)
                {
                    ddlDivision.DataSource = dtdiv;
                    ddlDivision.DataTextField = "DIVISION_NAME";
                    ddlDivision.DataValueField = "DIST_CD";
                    ddlDivision.DataBind();
                    ddlDivision.Items.Insert(0, new ListItem("Select", "0"));
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
                    ddlVendor.DataSource = dtdiv;
                    ddlVendor.DataTextField = "NAME";
                    ddlVendor.DataValueField = "VENDOR_ID";
                    ddlVendor.DataBind();
                    ddlVendor.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            Vendor(ddlDivision.SelectedValue);
        }
        protected void btnCreateinvoice_Click(object sender, EventArgs e)
        {
            int Result = 0;
            try
            {
                Result = objBL.Generate_Annexure1(ddlVendor.SelectedValue, ddlDivision.SelectedValue, "MMG",
                ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString());
                if (Result > 0)
                {
                    btnGenerate.Visible = true;
                    Get_Annexure1(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('No Record Found!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GridView3.DataSource = objBL.GetAnnexure_Excel1(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue);
            GridView3.DataBind();
            ExporttoExcel(objBL.GetAnnexure_Excel1(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue));
        }
        private void ExporttoExcel(DataTable table)
        {
            if (table.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=ANNEXURE-1.xls");
                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' width='100%' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;' colspan='16'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Large'>");
                HttpContext.Current.Response.Write("ANNEXURE-1 (MMG WORK SUMMARY)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'colspan='16' >");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("This certificate is for Bill No. " + txtBillNo.Text + " for Division  " + Session["Divison"].ToString() + " , Contractor M/S   " + Session["Vendorname"].ToString() + "");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;' colspan='16'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Against WC. No. " + txtWorkOrderNo.Text + " for the Month ………" + DateTime.Now.Year + ".");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='16'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='6' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("MMG Work Description");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='5' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Cable Consumption Summary");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("<BR>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Total Cable Insatlled");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='5' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Scrap Summary");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("<BR>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Total Cable Removed");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                int columnscount = GridView3.Columns.Count;
                decimal METERTYPE_1PH = 0;
                decimal METERTYPE_3PH = 0;
                decimal CABLE_INSTALLED_2X10 = 0;
                decimal CABLE_INSTALLED_2X25 = 0;
                decimal CABLE_INSTALLED_4X25 = 0;
                decimal CABLE_INSTALLED_4X50 = 0;
                decimal CABLE_INSTALLED_4X150 = 0;
                decimal CABLE_REMOVED_2X10 = 0;
                decimal CABLE_REMOVED_2X25 = 0;
                decimal CABLE_REMOVED_4X25 = 0;
                decimal CABLE_REMOVED_4X50 = 0;
                decimal CABLE_REMOVED_4X150 = 0;
                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td style='background:#ADD8E6;'>");
                    HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                    HttpContext.Current.Response.Write(GridView3.Columns[j].HeaderText.ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    METERTYPE_1PH += Convert.ToDecimal(row["METERTYPE_1PH"]);
                    METERTYPE_3PH += Convert.ToDecimal(row["METERTYPE_3PH"]);
                    CABLE_INSTALLED_2X10 += Convert.ToDecimal(row["CABLE_INS_2X10"]);
                    CABLE_INSTALLED_2X25 += Convert.ToDecimal(row["CABLE_INS_2X25"]);
                    CABLE_INSTALLED_4X25 += Convert.ToDecimal(row["CABLE_INS_4X25"]);
                    CABLE_INSTALLED_4X50 += Convert.ToDecimal(row["CABLE_INS_4X50"]);
                    CABLE_INSTALLED_4X150 += Convert.ToDecimal(row["CABLE_INS_4X150"]);
                    CABLE_REMOVED_2X10 += Convert.ToDecimal(row["CABLE_REMOVED_2X10"]);
                    CABLE_REMOVED_2X25 += Convert.ToDecimal(row["CABLE_REMOVED_2X25"]);
                    CABLE_REMOVED_4X25 += Convert.ToDecimal(row["CABLE_REMOVED_4X25"]);
                    CABLE_REMOVED_4X50 += Convert.ToDecimal(row["CABLE_REMOVED_4X50"]);
                    CABLE_REMOVED_4X150 += Convert.ToDecimal(row["CABLE_REMOVED_4X150"]);
                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write("<Td>");
                        HttpContext.Current.Response.Write(row[i].ToString());
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    HttpContext.Current.Response.Write("</TR>");
                }
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("Total MMG Jobs");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(METERTYPE_1PH);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(METERTYPE_3PH);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_INSTALLED_2X10);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_INSTALLED_2X25);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_INSTALLED_4X25);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_INSTALLED_4X50);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_INSTALLED_4X150);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_REMOVED_2X10);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_REMOVED_2X25);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_REMOVED_4X25);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_REMOVED_4X50);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(CABLE_REMOVED_4X150);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");


                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(MMG Manager)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td >");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Circle Head- MMG)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td >");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("</Table>");
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('No Data Found!');", true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            btnGenerate.Visible = true;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record saved successfully');", true);
        }
        protected void rptrMMGWorkSummty_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                METERTYPE_1PH += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "METERTYPE_1PH"));
                METERTYPE_3PH += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "METERTYPE_3PH"));
                CABLE_INS_2X10 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_INS_2X10"));
                CABLE_INS_2X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_INS_2X25"));
                CABLE_INS_4X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_INS_4X25"));
                CABLE_INS_4X50 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_INS_4X50"));
                CABLE_INS_4X150 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_INS_4X150"));
                CABLE_REMOVED_2X10 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_REMOVED_2X10"));
                CABLE_REMOVED_2X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_REMOVED_2X25"));
                CABLE_REMOVED_4X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_REMOVED_4X25"));
                CABLE_REMOVED_4X50 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_REMOVED_4X50"));
                CABLE_REMOVED_4X150 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CABLE_REMOVED_4X150"));
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                Literal Literal1 = (Literal)e.Item.FindControl("Literal1");
                Literal1.Text = METERTYPE_1PH.ToString();

                Literal Literal2 = (Literal)e.Item.FindControl("Literal2");
                Literal2.Text = METERTYPE_3PH.ToString();

                Literal Literal3 = (Literal)e.Item.FindControl("Literal3");
                Literal3.Text = CABLE_INS_2X10.ToString();

                Literal Literal4 = (Literal)e.Item.FindControl("Literal4");
                Literal4.Text = CABLE_INS_2X25.ToString();

                Literal Literal5 = (Literal)e.Item.FindControl("Literal5");
                Literal5.Text = CABLE_INS_4X25.ToString();

                Literal Literal6 = (Literal)e.Item.FindControl("Literal6");
                Literal6.Text = CABLE_INS_4X50.ToString();

                Literal Literal7 = (Literal)e.Item.FindControl("Literal7");
                Literal7.Text = CABLE_INS_4X150.ToString();

                Literal Literal8 = (Literal)e.Item.FindControl("Literal8");
                Literal8.Text = CABLE_REMOVED_2X10.ToString();

                Literal Literal9 = (Literal)e.Item.FindControl("Literal9");
                Literal9.Text = CABLE_REMOVED_2X25.ToString();

                Literal Literal10 = (Literal)e.Item.FindControl("Literal10");
                Literal10.Text = CABLE_REMOVED_4X25.ToString();

                Literal Literal11 = (Literal)e.Item.FindControl("Literal11");
                Literal11.Text = CABLE_REMOVED_4X50.ToString();

                Literal Literal12 = (Literal)e.Item.FindControl("Literal12");
                Literal12.Text = CABLE_REMOVED_4X150.ToString();
            }
        }
    }
}