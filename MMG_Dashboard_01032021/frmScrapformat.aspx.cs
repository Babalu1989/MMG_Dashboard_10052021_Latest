using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmScrapformat : System.Web.UI.Page
    {
        double CONSUMED_CABLE_2X10 = 0;
        double CONSUMED_CABLE_2X25 = 0;
        double CONSUMED_CABLE_4X25 = 0;
        double CONSUMED_CABLE_4X50 = 0;
        double CONSUMED_CABLE_4X150 = 0;
        double SCRAP_CABLE_2X10 = 0;
        double SCRAP_CABLE_2X25 = 0;
        double SCRAP_CABLE_4X25 = 0;
        double SCRAP_CABLE_4X50 = 0;
        double SCRAP_CABLE_4X150 = 0;
        string REMARKS = string.Empty;

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
        private void Get_ScrapVerification(string Vendor, string Month, string Year)
        {
            try
            {
                DataTable dtbill = objBL.GetScrapVerification(Vendor, Month, Year);
                if (dtbill.Rows.Count > 0)
                {
                    rptrScrapVerification.DataSource = dtbill;
                    rptrScrapVerification.DataBind();
                }
                else
                {
                    rptrScrapVerification.DataSource = null;
                    rptrScrapVerification.DataBind();
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
                return;
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
                Result = objBL.Generate_Scrap_Verificaion(ddlVendor.SelectedValue, ddlDivision.SelectedValue, "MMG",
                ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString());
                if (Result > 0)
                {
                    Get_ScrapVerification(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue);
                    tabid.Visible = true;
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
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GridView5.DataSource = objBL.Get_Scrap_Verification(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue);
            GridView5.DataBind();
            ExporttoExcel(objBL.Get_Scrap_Verification(ddlVendor.SelectedValue,ddlMonth.SelectedValue,ddlYear.SelectedValue));
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Scrap Format.xls");
                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' width='100%' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                //HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;' colspan='13'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Large'>");
                HttpContext.Current.Response.Write("SCRAP VERIFICATION REPORT");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write("");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='5' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Total Cosnumed Cable Size (In Mtr)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='5' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Scarp Cable Size (Mtr)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                int columnscount = GridView5.Columns.Count;
                double str1 = 0;
                double str2 = 0;
                double str3 = 0;
                double str4 = 0;
                double str5 = 0;
                double str6 = 0;
                double str7 = 0;
                double str8 = 0;
                double str9 = 0;
                double str10 = 0;
                double str11 = 0;
                double str12 = 0;
                double str13 = 0;
                double str14 = 0;
                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td style='background:#ADD8E6;'>");
                    HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                    HttpContext.Current.Response.Write(GridView5.Columns[j].HeaderText.ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    if (row["JOB_NAME"].ToString() == "Balance Old Cable to be Reused" || row["JOB_NAME"].ToString() == "Cable Reused"
                        || row["JOB_NAME"].ToString() == "New Cables Return To Store As Scrap" || row["JOB_NAME"].ToString() == "Previous Balance Old Cable")
                    {
                        str1 += Convert.ToDouble(row["SCRAP_CABLE_2X10"]);
                        str2 += Convert.ToDouble(row["SCRAP_CABLE_2X25"]);
                        str3 += Convert.ToDouble(row["SCRAP_CABLE_4X25"]);
                        str4 += Convert.ToDouble(row["SCRAP_CABLE_4X50"]);
                        str5 += Convert.ToDouble(row["SCRAP_CABLE_4X150"]);
                    }
                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write("<Td>");
                        HttpContext.Current.Response.Write(row[i].ToString());
                        HttpContext.Current.Response.Write("</Td>");
                    }
                    HttpContext.Current.Response.Write("</TR>");
                }
                str6 =(str1 * 0.4);
                str7 = (str2 * 0.6);
                str8 = (str3 * 1);
                str9 = (str4 * 1.5);
                str10 = (str5 * 2.5);
                str11 = str6 + str7 + str8 + str9 + str10;
                str12 = str6 + str7;
                str13= str8 + str9 + str10;
                str14 = str12 + str13;
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='7' style='Text-Align:left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("OVERALL Total ");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str1);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str2);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str3);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td >");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str4);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str5);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='7' style='Text-Align:Right;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Required SCRAP IN KG");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str6);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str7);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str8);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str9);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str10);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str11);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='7'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("FLEXIBLE WIRE");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("XLPE OFF SIZE");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Total");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='7' style='Text-Align:Right;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Required SCRAP IN KG");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str12);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str13);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(str14);
                HttpContext.Current.Response.Write("</B>");
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
            int Result = 0;
            string month = ddlMonth.SelectedValue;
            string year = ddlYear.SelectedValue;
            string Bill_No = txtBillNo.Text.ToString();
            string WorkOrder_No = txtWorkOrderNo.Text.ToString();
            string VendorId = ddlVendor.SelectedValue;
            string Jobdesc = string.Empty;
            int SCRAP_JOB_ID;
            double CONSUMED_CABLE_2X10;
            double CONSUMED_CABLE_2X25;
            double CONSUMED_CABLE_4X25;
            double CONSUMED_CABLE_4X50;
            double CONSUMED_CABLE_4X150;
            double SCRAP_CABLE_2X10;
            double SCRAP_CABLE_2X25;
            double SCRAP_CABLE_4X25;
            double SCRAP_CABLE_4X50;
            double SCRAP_CABLE_4X150;

            try
            {
                Result = objBL.Delete_Old_Scrap_Verification(month, year, VendorId, Bill_No, WorkOrder_No);
                //foreach (RepeaterItem item in rptrScrapVerification.Items)
                //{
                //    Label lblScrapJobId = item.FindControl("lblScrapJobId") as Label;
                //    Label lblJobName = item.FindControl("lblJobName") as Label;
                //    Label lblConsumedCable_2X10 = item.FindControl("lblConsumedCable_2X10") as Label;
                //    Label lblConsumedCable_2X25 = item.FindControl("lblConsumedCable_2X25") as Label;
                //    Label lblConsumedCable_4X25 = item.FindControl("lblConsumedCable_4X25") as Label;
                //    Label lblConsumedCable_4X50 = item.FindControl("lblConsumedCable_4X50") as Label;
                //    Label lblConsumedCable_4X150 = item.FindControl("lblConsumedCable_4X150") as Label;
                //    Label lblScrapCable_2X10 = item.FindControl("lblScrapCable_2X10") as Label;
                //    Label lblScrapCable_2X25 = item.FindControl("lblScrapCable_2X25") as Label;
                //    Label lblScrapCable_4X25 = item.FindControl("lblScrapCable_4X25") as Label;
                //    Label lblScrapCable_4X50 = item.FindControl("lblScrapCable_4X50") as Label;
                //    Label lblScrapCable_4X150 = item.FindControl("lblScrapCable_4X150") as Label;
                //    TextBox txtScrapReportRemarks = item.FindControl("txtScrapReportRemarks") as TextBox;


                //    // SCRAP_JOB_ID = string.IsNullOrEmpty(lblScrapJobId.Text) ? 0 : Convert.ToInt32(lblScrapJobId.Text);
                //    CONSUMED_CABLE_2X10 = string.IsNullOrEmpty(lblConsumedCable_2X10.Text) ? 0 : Convert.ToDouble(lblConsumedCable_2X10.Text);
                //    CONSUMED_CABLE_2X25 = string.IsNullOrEmpty(lblConsumedCable_2X25.Text) ? 0 : Convert.ToDouble(lblConsumedCable_2X25.Text);
                //    CONSUMED_CABLE_4X25 = string.IsNullOrEmpty(lblConsumedCable_4X25.Text) ? 0 : Convert.ToDouble(lblConsumedCable_4X25.Text);
                //    CONSUMED_CABLE_4X50 = string.IsNullOrEmpty(lblConsumedCable_4X50.Text) ? 0 : Convert.ToDouble(lblConsumedCable_4X50.Text);
                //    CONSUMED_CABLE_4X150 = string.IsNullOrEmpty(lblConsumedCable_4X150.Text) ? 0 : Convert.ToDouble(lblConsumedCable_4X150.Text);
                //    SCRAP_CABLE_2X10 = string.IsNullOrEmpty(lblScrapCable_2X10.Text) ? 0 : Convert.ToDouble(lblScrapCable_2X10.Text);
                //    SCRAP_CABLE_2X25 = string.IsNullOrEmpty(lblScrapCable_2X25.Text) ? 0 : Convert.ToDouble(lblScrapCable_2X25.Text);
                //    SCRAP_CABLE_4X25 = string.IsNullOrEmpty(lblScrapCable_4X25.Text) ? 0 : Convert.ToDouble(lblScrapCable_4X25.Text);
                //    SCRAP_CABLE_4X50 = string.IsNullOrEmpty(lblScrapCable_4X50.Text) ? 0 : Convert.ToDouble(lblScrapCable_4X50.Text);
                //    SCRAP_CABLE_4X150 = string.IsNullOrEmpty(lblScrapCable_4X150.Text) ? 0 : Convert.ToDouble(lblScrapCable_4X150.Text);
                //    REMARKS = txtScrapReportRemarks.Text.Trim().Replace("'", "`").Replace("$", "").Replace(",", " ").ToUpper();
                //    //Result = objBL.Insert_ScrapVerification_Data(month, year, VendorId, lblJobName.Text, Convert.ToString(CONSUMED_CABLE_2X10), Convert.ToString(CONSUMED_CABLE_2X25), Convert.ToString(CONSUMED_CABLE_4X25),
                //    //Convert.ToString(CONSUMED_CABLE_4X50), Convert.ToString(CONSUMED_CABLE_4X150), Convert.ToString(SCRAP_CABLE_2X10),
                //    //Convert.ToString(SCRAP_CABLE_2X25), Convert.ToString(SCRAP_CABLE_4X25), Convert.ToString(SCRAP_CABLE_4X50), Convert.ToString(SCRAP_CABLE_4X150),
                //    //REMARKS, Session["UserName"].ToString());
                //}
                foreach (Control ctrl in rptrScrapVerification.Controls)
                {
                    RepeaterItem item = ctrl as RepeaterItem;
                    if (item.ItemType == ListItemType.Footer)
                    {
                        TextBox txtConsumedCable_2X10_OldCable = item.FindControl("txtConsumedCable_2X10_OldCable") as TextBox;
                        TextBox txtConsumedCable_2X25_OldCable = item.FindControl("txtConsumedCable_2X25_OldCable") as TextBox;
                        TextBox txtConsumedCable_4X25_OldCable = item.FindControl("txtConsumedCable_4X25_OldCable") as TextBox;
                        TextBox txtConsumedCable_4X50_OldCable = item.FindControl("txtConsumedCable_4X50_OldCable") as TextBox;
                        TextBox txtConsumedCable_4X150_OldCable = item.FindControl("txtConsumedCable_4X150_OldCable") as TextBox;
                        TextBox txtCableSize_2X10_OldCable = item.FindControl("txtCableSize_2X10_OldCable") as TextBox;
                        TextBox txtCableSize_2X25_OldCable = item.FindControl("txtCableSize_2X25_OldCable") as TextBox;
                        TextBox txtCableSize_4X25_OldCable = item.FindControl("txtCableSize_4X25_OldCable") as TextBox;
                        TextBox txtCableSize_4X50_OldCable = item.FindControl("txtCableSize_4X50_OldCable") as TextBox;
                        TextBox txtCableSize_4X150_OldCable = item.FindControl("txtCableSize_4X150_OldCable") as TextBox;

                        TextBox txtConsumedCable_2X10_CableReused = item.FindControl("txtConsumedCable_2X10_CableReused") as TextBox;
                        TextBox txtConsumedCable_2X25_CableReused = item.FindControl("txtConsumedCable_2X25_CableReused") as TextBox;
                        TextBox txtConsumedCable_4X25_CableReused = item.FindControl("txtConsumedCable_4X25_CableReused") as TextBox;
                        TextBox txtConsumedCable_4X50_CableReused = item.FindControl("txtConsumedCable_4X50_CableReused") as TextBox;
                        TextBox txtConsumedCable_4X150_CableReused = item.FindControl("txtConsumedCable_4X150_CableReused") as TextBox;
                        TextBox txtCableSize_2X10_CableReused = item.FindControl("txtCableSize_2X10_CableReused") as TextBox;
                        TextBox txtCableSize_2X25_CableReused = item.FindControl("txtCableSize_2X25_CableReused") as TextBox;
                        TextBox txtCableSize_4X25_CableReused = item.FindControl("txtCableSize_4X25_CableReused") as TextBox;
                        TextBox txtCableSize_4X50_CableReused = item.FindControl("txtCableSize_4X50_CableReused") as TextBox;
                        TextBox txtCableSize_4X150_CableReused = item.FindControl("txtCableSize_4X150_CableReused") as TextBox;

                        TextBox txtConsumedCable_2X10_OldCableReused = item.FindControl("txtConsumedCable_2X10_OldCableReused") as TextBox;
                        TextBox txtConsumedCable_2X25_OldCableReused = item.FindControl("txtConsumedCable_2X25_OldCableReused") as TextBox;
                        TextBox txtConsumedCable_4X25_OldCableReused = item.FindControl("txtConsumedCable_4X25_OldCableReused") as TextBox;
                        TextBox txtConsumedCable_4X50_OldCableReused = item.FindControl("txtConsumedCable_4X50_OldCableReused") as TextBox;
                        TextBox txtConsumedCable_4X150_OldCableReused = item.FindControl("txtConsumedCable_4X150_OldCableReused") as TextBox;
                        TextBox txtCableSize_2X10_OldCableReused = item.FindControl("txtCableSize_2X10_OldCableReused") as TextBox;
                        TextBox txtCableSize_2X25_OldCableReused = item.FindControl("txtCableSize_2X25_OldCableReused") as TextBox;
                        TextBox txtCableSize_4X25_OldCableReused = item.FindControl("txtCableSize_4X25_OldCableReused") as TextBox;
                        TextBox txtCableSize_4X50_OldCableReused = item.FindControl("txtCableSize_4X50_OldCableReused") as TextBox;
                        TextBox txtCableSize_4X150_OldCableReused = item.FindControl("txtCableSize_4X150_OldCableReused") as TextBox;

                        TextBox txtConsumedCable_2X10_NewCable = item.FindControl("txtConsumedCable_2X10_NewCable") as TextBox;
                        TextBox txtConsumedCable_2X25_NewCable = item.FindControl("txtConsumedCable_2X25_NewCable") as TextBox;
                        TextBox txtConsumedCable_4X25_NewCable = item.FindControl("txtConsumedCable_4X25_NewCable") as TextBox;
                        TextBox txtConsumedCable_4X50_NewCable = item.FindControl("txtConsumedCable_4X50_NewCable") as TextBox;
                        TextBox txtConsumedCable_4X150_NewCable = item.FindControl("txtConsumedCable_4X150_NewCable") as TextBox;
                        TextBox txtCableSize_2X10_NewCable = item.FindControl("txtCableSize_2X10_NewCable") as TextBox;
                        TextBox txtCableSize_2X25_NewCable = item.FindControl("txtCableSize_2X25_NewCable") as TextBox;
                        TextBox txtCableSize_4X25_NewCable = item.FindControl("txtCableSize_4X25_NewCable") as TextBox;
                        TextBox txtCableSize_4X50_NewCable = item.FindControl("txtCableSize_4X50_NewCable") as TextBox;
                        TextBox txtCableSize_4X150_NewCable = item.FindControl("txtCableSize_4X150_NewCable") as TextBox;

                        double PREVBAL_CONSUMED_CABLE_2X10 = string.IsNullOrEmpty(txtConsumedCable_2X10_OldCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X10_OldCable.Text);
                        double PREVBAL_CONSUMED_CABLE_2X25 = string.IsNullOrEmpty(txtConsumedCable_2X25_OldCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X25_OldCable.Text);
                        double PREVBAL_CONSUMED_CABLE_4X25 = string.IsNullOrEmpty(txtConsumedCable_4X25_OldCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X25_OldCable.Text);
                        double PREVBAL_CONSUMED_CABLE_4X50 = string.IsNullOrEmpty(txtConsumedCable_4X50_OldCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X50_OldCable.Text);
                        double PREVBAL_CONSUMED_CABLE_4X150 = string.IsNullOrEmpty(txtConsumedCable_4X150_OldCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X150_OldCable.Text);
                        double PREVBAL_SCRAP_CABLE_2X10 = string.IsNullOrEmpty(txtCableSize_2X10_OldCable.Text) ? 0 : Convert.ToDouble(txtCableSize_2X10_OldCable.Text);
                        double PREVBAL_SCRAP_CABLE_2X25 = string.IsNullOrEmpty(txtCableSize_2X25_OldCable.Text) ? 0 : Convert.ToDouble(txtCableSize_2X25_OldCable.Text);
                        double PREVBAL_SCRAP_CABLE_4X25 = string.IsNullOrEmpty(txtCableSize_4X25_OldCable.Text) ? 0 : Convert.ToDouble(txtCableSize_4X25_OldCable.Text);
                        double PREVBAL_SCRAP_CABLE_4X50 = string.IsNullOrEmpty(txtCableSize_4X50_OldCable.Text) ? 0 : Convert.ToDouble(txtCableSize_4X50_OldCable.Text);
                        double PREVBAL_SCRAP_CABLE_4X150 = string.IsNullOrEmpty(txtCableSize_4X150_OldCable.Text) ? 0 : Convert.ToDouble(txtCableSize_4X150_OldCable.Text);

                        double CABLEREUSED_CONSUMED_CABLE_2X10 = string.IsNullOrEmpty(txtConsumedCable_2X10_CableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X10_CableReused.Text);
                        double CABLEREUSED_CONSUMED_CABLE_2X25 = string.IsNullOrEmpty(txtConsumedCable_2X25_CableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X25_CableReused.Text);
                        double CABLEREUSED_CONSUMED_CABLE_4X25 = string.IsNullOrEmpty(txtConsumedCable_4X25_CableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X25_CableReused.Text);
                        double CABLEREUSED_CONSUMED_CABLE_4X50 = string.IsNullOrEmpty(txtConsumedCable_4X50_CableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X50_CableReused.Text);
                        double CABLEREUSED_CONSUMED_CABLE_4X150 = string.IsNullOrEmpty(txtConsumedCable_4X150_CableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X150_CableReused.Text);
                        double CABLEREUSED_SCRAP_CABLE_2X10 = string.IsNullOrEmpty(txtCableSize_2X10_CableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_2X10_CableReused.Text);
                        double CABLEREUSED_SCRAP_CABLE_2X25 = string.IsNullOrEmpty(txtCableSize_2X25_CableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_2X25_CableReused.Text);
                        double CABLEREUSED_SCRAP_CABLE_4X25 = string.IsNullOrEmpty(txtCableSize_4X25_CableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_4X25_CableReused.Text);
                        double CABLEREUSED_SCRAP_CABLE_4X50 = string.IsNullOrEmpty(txtCableSize_4X50_CableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_4X50_CableReused.Text);
                        double CABLEREUSED_SCRAP_CABLE_4X150 = string.IsNullOrEmpty(txtCableSize_4X150_CableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_4X150_CableReused.Text);

                        double OLDCABLE_CONSUMED_CABLE_2X10 = string.IsNullOrEmpty(txtConsumedCable_2X10_OldCableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X10_OldCableReused.Text);
                        double OLDCABLE_CONSUMED_CABLE_2X25 = string.IsNullOrEmpty(txtConsumedCable_2X25_OldCableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X25_OldCableReused.Text);
                        double OLDCABLE_CONSUMED_CABLE_4X25 = string.IsNullOrEmpty(txtConsumedCable_4X25_OldCableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X25_OldCableReused.Text);
                        double OLDCABLE_CONSUMED_CABLE_4X50 = string.IsNullOrEmpty(txtConsumedCable_4X50_OldCableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X50_OldCableReused.Text);
                        double OLDCABLE_CONSUMED_CABLE_4X150 = string.IsNullOrEmpty(txtConsumedCable_4X150_OldCableReused.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X150_OldCableReused.Text);
                        double OLDCABLE_SCRAP_CABLE_2X10 = string.IsNullOrEmpty(txtCableSize_2X10_OldCableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_2X10_OldCableReused.Text);
                        double OLDCABLE_SCRAP_CABLE_2X25 = string.IsNullOrEmpty(txtCableSize_2X25_OldCableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_2X25_OldCableReused.Text);
                        double OLDCABLE_SCRAP_CABLE_4X25 = string.IsNullOrEmpty(txtCableSize_4X25_OldCableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_4X25_OldCableReused.Text);
                        double OLDCABLE_SCRAP_CABLE_4X50 = string.IsNullOrEmpty(txtCableSize_4X50_OldCableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_4X50_OldCableReused.Text);
                        double OLDCABLE_SCRAP_CABLE_4X150 = string.IsNullOrEmpty(txtCableSize_4X150_OldCableReused.Text) ? 0 : Convert.ToDouble(txtCableSize_4X150_OldCableReused.Text);

                        double NEWCABLE_CONSUMED_CABLE_2X10 = string.IsNullOrEmpty(txtConsumedCable_2X10_NewCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X10_NewCable.Text);
                        double NEWCABLE_CONSUMED_CABLE_2X25 = string.IsNullOrEmpty(txtConsumedCable_2X25_NewCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_2X25_NewCable.Text);
                        double NEWCABLE_CONSUMED_CABLE_4X25 = string.IsNullOrEmpty(txtConsumedCable_4X25_NewCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X25_NewCable.Text);
                        double NEWCABLE_CONSUMED_CABLE_4X50 = string.IsNullOrEmpty(txtConsumedCable_4X50_NewCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X50_NewCable.Text);
                        double NEWCABLE_CONSUMED_CABLE_4X150 = string.IsNullOrEmpty(txtConsumedCable_4X150_NewCable.Text) ? 0 : Convert.ToDouble(txtConsumedCable_4X150_NewCable.Text);
                        double NEWCABLE_SCRAP_CABLE_2X10 = string.IsNullOrEmpty(txtCableSize_2X10_NewCable.Text) ? 0 : Convert.ToDouble(txtCableSize_2X10_NewCable.Text);
                        double NEWCABLE_SCRAP_CABLE_2X25 = string.IsNullOrEmpty(txtCableSize_2X25_NewCable.Text) ? 0 : Convert.ToDouble(txtCableSize_2X25_NewCable.Text);
                        double NEWCABLE_SCRAP_CABLE_4X25 = string.IsNullOrEmpty(txtCableSize_4X25_NewCable.Text) ? 0 : Convert.ToDouble(txtCableSize_4X25_NewCable.Text);
                        double NEWCABLE_SCRAP_CABLE_4X50 = string.IsNullOrEmpty(txtCableSize_4X50_NewCable.Text) ? 0 : Convert.ToDouble(txtCableSize_4X50_NewCable.Text);
                        double NEWCABLE_SCRAP_CABLE_4X150 = string.IsNullOrEmpty(txtCableSize_4X150_NewCable.Text) ? 0 : Convert.ToDouble(txtCableSize_4X150_NewCable.Text);

                        Result = objBL.Insert_ScrapVerification_Data(month, year, VendorId, "",
                        Convert.ToString(PREVBAL_CONSUMED_CABLE_2X10), Convert.ToString(PREVBAL_CONSUMED_CABLE_2X25), Convert.ToString(PREVBAL_CONSUMED_CABLE_4X25), Convert.ToString(PREVBAL_CONSUMED_CABLE_4X50), Convert.ToString(PREVBAL_CONSUMED_CABLE_4X150),
                        Convert.ToString(PREVBAL_SCRAP_CABLE_2X10), Convert.ToString(PREVBAL_SCRAP_CABLE_2X25), Convert.ToString(PREVBAL_SCRAP_CABLE_4X25), Convert.ToString(PREVBAL_SCRAP_CABLE_4X50), Convert.ToString(PREVBAL_SCRAP_CABLE_4X150), Convert.ToString(CABLEREUSED_CONSUMED_CABLE_2X10),
                        Convert.ToString(CABLEREUSED_CONSUMED_CABLE_2X25), Convert.ToString(CABLEREUSED_CONSUMED_CABLE_4X25), Convert.ToString(CABLEREUSED_CONSUMED_CABLE_4X50), Convert.ToString(CABLEREUSED_CONSUMED_CABLE_4X150), Convert.ToString(CABLEREUSED_SCRAP_CABLE_2X10),
                        Convert.ToString(CABLEREUSED_SCRAP_CABLE_2X25), Convert.ToString(CABLEREUSED_SCRAP_CABLE_4X25), Convert.ToString(CABLEREUSED_SCRAP_CABLE_4X50), Convert.ToString(CABLEREUSED_SCRAP_CABLE_4X150), Convert.ToString(OLDCABLE_CONSUMED_CABLE_2X10),
                        Convert.ToString(OLDCABLE_CONSUMED_CABLE_2X25), Convert.ToString(OLDCABLE_CONSUMED_CABLE_4X25), Convert.ToString(OLDCABLE_CONSUMED_CABLE_4X50), Convert.ToString(OLDCABLE_CONSUMED_CABLE_4X150), Convert.ToString(OLDCABLE_SCRAP_CABLE_2X10),
                        Convert.ToString(OLDCABLE_SCRAP_CABLE_2X25), Convert.ToString(OLDCABLE_SCRAP_CABLE_4X25), Convert.ToString(OLDCABLE_SCRAP_CABLE_4X50), Convert.ToString(OLDCABLE_SCRAP_CABLE_4X150), Convert.ToString(NEWCABLE_CONSUMED_CABLE_2X10),
                        Convert.ToString(NEWCABLE_CONSUMED_CABLE_2X25), Convert.ToString(NEWCABLE_CONSUMED_CABLE_4X25), Convert.ToString(NEWCABLE_CONSUMED_CABLE_4X50), Convert.ToString(NEWCABLE_CONSUMED_CABLE_4X150), Convert.ToString(NEWCABLE_SCRAP_CABLE_2X10),
                        Convert.ToString(NEWCABLE_SCRAP_CABLE_2X25), Convert.ToString(NEWCABLE_SCRAP_CABLE_4X25), Convert.ToString(NEWCABLE_SCRAP_CABLE_4X50), Convert.ToString(NEWCABLE_SCRAP_CABLE_4X150),
                        REMARKS, Session["UserName"].ToString());
                    }
                }
                objBL.Insert_ModifyInvoce_Log(ddlDivision.SelectedValue, "MMG", ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString(), ddlVendor.SelectedValue, "SCRAP SUBMITED", Session["UserName"].ToString());
                btnGenerate.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record saved successfully');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
                return;
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmScrapformat.aspx");
        }

        protected void rptrScrapVerification_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CONSUMED_CABLE_2X10 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CONSUMED_CABLE_2X10"));
                CONSUMED_CABLE_2X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CONSUMED_CABLE_2X25"));
                CONSUMED_CABLE_4X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CONSUMED_CABLE_4X25"));
                CONSUMED_CABLE_4X50 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CONSUMED_CABLE_4X50"));
                CONSUMED_CABLE_4X150 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "CONSUMED_CABLE_4X150"));
                SCRAP_CABLE_2X10 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "SCRAP_CABLE_2X10"));
                SCRAP_CABLE_2X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "SCRAP_CABLE_2X25"));
                SCRAP_CABLE_4X25 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "SCRAP_CABLE_4X25"));
                SCRAP_CABLE_4X50 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "SCRAP_CABLE_4X50"));
                SCRAP_CABLE_4X150 += Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "SCRAP_CABLE_4X150"));
            }

            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotal_ConsumedCable_2X10 = (Label)e.Item.FindControl("lblTotal_ConsumedCable_2X10");
                lblTotal_ConsumedCable_2X10.Text = CONSUMED_CABLE_2X10.ToString();

                Label lblTotal_ConsumedCable_2X25 = (Label)e.Item.FindControl("lblTotal_ConsumedCable_2X25");
                lblTotal_ConsumedCable_2X25.Text = CONSUMED_CABLE_2X25.ToString();

                Label lblTotal_ConsumedCable_4X25 = (Label)e.Item.FindControl("lblTotal_ConsumedCable_4X25");
                lblTotal_ConsumedCable_4X25.Text = CONSUMED_CABLE_4X25.ToString();

                Label lblTotal_ConsumedCable_4X50 = (Label)e.Item.FindControl("lblTotal_ConsumedCable_4X50");
                lblTotal_ConsumedCable_4X50.Text = CONSUMED_CABLE_4X50.ToString();

                Label lblTotal_ConsumedCable_4X150 = (Label)e.Item.FindControl("lblTotal_ConsumedCable_4X150");
                lblTotal_ConsumedCable_4X150.Text = CONSUMED_CABLE_4X150.ToString();

                Label lblTotal_ScrapCable_2X10 = (Label)e.Item.FindControl("lblTotal_ScrapCable_2X10");
                lblTotal_ScrapCable_2X10.Text = SCRAP_CABLE_2X10.ToString();

                Label lblTotal_ScrapCable_2X25 = (Label)e.Item.FindControl("lblTotal_ScrapCable_2X25");
                lblTotal_ScrapCable_2X25.Text = SCRAP_CABLE_2X25.ToString();

                Label lblTotal_ScrapCable_4X25 = (Label)e.Item.FindControl("lblTotal_ScrapCable_4X25");
                lblTotal_ScrapCable_4X25.Text = SCRAP_CABLE_4X25.ToString();

                Label lblTotal_ScrapCable_4X50 = (Label)e.Item.FindControl("lblTotal_ScrapCable_4X50");
                lblTotal_ScrapCable_4X50.Text = SCRAP_CABLE_4X50.ToString();

                Label lblTotal_ScrapCable_4X150 = (Label)e.Item.FindControl("lblTotal_ScrapCable_4X150");
                lblTotal_ScrapCable_4X150.Text = SCRAP_CABLE_4X150.ToString();
            }
        }

        //protected void txtCableSize_2X10_OldCable_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X10_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X10_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X10_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X10_NewCable");
        //    Label l1 = (Label)item.FindControl("Label1");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data+= string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data+= string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data+= string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l1.Text =Convert.ToString(Data);
        //}
        //protected void txtCableSize_2X10_CableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X10_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X10_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X10_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X10_NewCable");
        //    Label l1 = (Label)item.FindControl("Label1");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l1.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_2X10_OldCableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X10_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X10_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X10_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X10_NewCable");
        //    Label l1 = (Label)item.FindControl("Label1");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l1.Text = Convert.ToString(Data);
        //}
        protected void txtCableSize_2X10_NewCable_TextChanged(object sender, EventArgs e)
        {
            double Data = 0;
            double Totaldata = 0;
            double TotaldataKG = 0;
            double TotaldataKG1 = 0;
            double FinaldataKG = 0;
            TextBox textBox = sender as TextBox;
            string theText = textBox.Text;
            var item = (RepeaterItem)textBox.NamingContainer;
            TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X10_OldCable");
            TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X10_CableReused");
            TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X10_OldCableReused");
            TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X10_NewCable");
            Label lt6 = (Label)item.FindControl("Label6");
            Label lt7 = (Label)item.FindControl("Label7");
            Label lt8 = (Label)item.FindControl("Label8");
            Label lt9 = (Label)item.FindControl("Label9");
            Label lt10 = (Label)item.FindControl("Label10");
            Label l1 = (Label)item.FindControl("Label1");
            Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToDouble(str1.Text);
            Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToDouble(str2.Text);
            Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToDouble(str3.Text);
            Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToDouble(str4.Text);
            l1.Text = Convert.ToString(Data);
            Label l6 = (Label)item.FindControl("Label6");
            l6.Text = Convert.ToString(Data * 0.4);

            Totaldata = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            Totaldata += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Totaldata += string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            Totaldata += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            Totaldata += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l11 = (Label)item.FindControl("Label11");
            l11.Text = Convert.ToString(Totaldata);

            TotaldataKG = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            TotaldataKG += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Label l12 = (Label)item.FindControl("Label12");
            l12.Text = Convert.ToString(TotaldataKG);

            TotaldataKG1 = string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l13 = (Label)item.FindControl("Label13");
            l13.Text = Convert.ToString(TotaldataKG1);

            FinaldataKG = string.IsNullOrEmpty(l12.Text) ? 0 : Convert.ToDouble(l12.Text);
            FinaldataKG += string.IsNullOrEmpty(l13.Text) ? 0 : Convert.ToDouble(l13.Text);
            Label l14 = (Label)item.FindControl("Label14");
            l14.Text = Convert.ToString(FinaldataKG);
        }

        //protected void txtCableSize_2X25_OldCable_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X25_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X25_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X25_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X25_NewCable");
        //    Label l2 = (Label)item.FindControl("Label2");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l2.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_2X25_CableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X25_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X25_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X25_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X25_NewCable");
        //    Label l2 = (Label)item.FindControl("Label2");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l2.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_2X25_OldCableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X25_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X25_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X25_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X25_NewCable");
        //    Label l2 = (Label)item.FindControl("Label2");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l2.Text = Convert.ToString(Data);
        //}
        protected void txtCableSize_2X25_NewCable_TextChanged(object sender, EventArgs e)
        {
            double Data = 0;
            double Totaldata = 0;
            double TotaldataKG = 0;
            double TotaldataKG1 = 0;
            TextBox textBox = sender as TextBox;
            string theText = textBox.Text;
            var item = (RepeaterItem)textBox.NamingContainer;
            TextBox str1 = (TextBox)item.FindControl("txtCableSize_2X25_OldCable");
            TextBox str2 = (TextBox)item.FindControl("txtCableSize_2X25_CableReused");
            TextBox str3 = (TextBox)item.FindControl("txtCableSize_2X25_OldCableReused");
            TextBox str4 = (TextBox)item.FindControl("txtCableSize_2X25_NewCable");
            Label lt6 = (Label)item.FindControl("Label6");
            Label lt7 = (Label)item.FindControl("Label7");
            Label lt8 = (Label)item.FindControl("Label8");
            Label lt9 = (Label)item.FindControl("Label9");
            Label lt10 = (Label)item.FindControl("Label10");
            Label l2 = (Label)item.FindControl("Label2");
            Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToDouble(str1.Text);
            Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToDouble(str2.Text);
            Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToDouble(str3.Text);
            Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToDouble(str4.Text);
            l2.Text = Convert.ToString(Data);
            Label l7 = (Label)item.FindControl("Label7");
            l7.Text = Convert.ToString(Data * 0.6);

            Totaldata = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            Totaldata += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Totaldata += string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            Totaldata += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            Totaldata += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l11 = (Label)item.FindControl("Label11");
            l11.Text = Convert.ToString(Totaldata);

            TotaldataKG = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            TotaldataKG += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Label l12 = (Label)item.FindControl("Label12");
            l12.Text = Convert.ToString(TotaldataKG);

            TotaldataKG1 = string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l13 = (Label)item.FindControl("Label13");
            l13.Text = Convert.ToString(TotaldataKG1);
        }

        //protected void txtCableSize_4X25_OldCable_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X25_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X25_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X25_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X25_NewCable");
        //    Label l3 = (Label)item.FindControl("Label3");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l3.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_4X25_CableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X25_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X25_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X25_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X25_NewCable");
        //    Label l3 = (Label)item.FindControl("Label3");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l3.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_4X25_OldCableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X25_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X25_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X25_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X25_NewCable");
        //    Label l3 = (Label)item.FindControl("Label3");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l3.Text = Convert.ToString(Data);
        //}
        protected void txtCableSize_4X25_NewCable_TextChanged(object sender, EventArgs e)
        {
            double Data = 0;
            double Totaldata = 0;
            double TotaldataKG = 0;
            double TotaldataKG1 = 0;
            TextBox textBox = sender as TextBox;
            string theText = textBox.Text;
            var item = (RepeaterItem)textBox.NamingContainer;
            TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X25_OldCable");
            TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X25_CableReused");
            TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X25_OldCableReused");
            TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X25_NewCable");
            Label lt6 = (Label)item.FindControl("Label6");
            Label lt7 = (Label)item.FindControl("Label7");
            Label lt8 = (Label)item.FindControl("Label8");
            Label lt9 = (Label)item.FindControl("Label9");
            Label lt10 = (Label)item.FindControl("Label10");
            Label l3 = (Label)item.FindControl("Label3");
            Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToDouble(str1.Text);
            Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToDouble(str2.Text);
            Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToDouble(str3.Text);
            Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToDouble(str4.Text);
            l3.Text = Convert.ToString(Data);
            Label l8 = (Label)item.FindControl("Label8");
            l8.Text = Convert.ToString(Data * 1);

            Totaldata = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            Totaldata += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Totaldata += string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            Totaldata += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            Totaldata += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l11 = (Label)item.FindControl("Label11");
            l11.Text = Convert.ToString(Totaldata);

            TotaldataKG = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            TotaldataKG += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Label l12 = (Label)item.FindControl("Label12");
            l12.Text = Convert.ToString(TotaldataKG);

            TotaldataKG1 = string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l13 = (Label)item.FindControl("Label13");
            l13.Text = Convert.ToString(TotaldataKG1);
        }

        //protected void txtCableSize_4X50_OldCable_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X50_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X50_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X50_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X50_NewCable");
        //    Label l4 = (Label)item.FindControl("Label4");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l4.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_4X50_CableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X50_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X50_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X50_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X50_NewCable");
        //    Label l4 = (Label)item.FindControl("Label4");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l4.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_4X50_OldCableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X50_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X50_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X50_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X50_NewCable");
        //    Label l4 = (Label)item.FindControl("Label4");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l4.Text = Convert.ToString(Data);
        //}
        protected void txtCableSize_4X50_NewCable_TextChanged(object sender, EventArgs e)
        {
            double Data = 0;
            double Totaldata = 0;
            double TotaldataKG = 0;
            double TotaldataKG1 = 0;
            TextBox textBox = sender as TextBox;
            string theText = textBox.Text;
            var item = (RepeaterItem)textBox.NamingContainer;
            TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X50_OldCable");
            TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X50_CableReused");
            TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X50_OldCableReused");
            TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X50_NewCable");
            Label lt6 = (Label)item.FindControl("Label6");
            Label lt7 = (Label)item.FindControl("Label7");
            Label lt8 = (Label)item.FindControl("Label8");
            Label lt9 = (Label)item.FindControl("Label9");
            Label lt10 = (Label)item.FindControl("Label10");
            Label l4 = (Label)item.FindControl("Label4");
            Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToDouble(str1.Text);
            Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToDouble(str2.Text);
            Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToDouble(str3.Text);
            Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToDouble(str4.Text);
            l4.Text = Convert.ToString(Data);
            Label l9 = (Label)item.FindControl("Label9");
            l9.Text = Convert.ToString(Data * 1.5);

            Totaldata = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            Totaldata += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Totaldata += string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            Totaldata += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            Totaldata += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l11 = (Label)item.FindControl("Label11");
            l11.Text = Convert.ToString(Totaldata);

            TotaldataKG = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            TotaldataKG += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Label l12 = (Label)item.FindControl("Label12");
            l12.Text = Convert.ToString(TotaldataKG);

            TotaldataKG1 = string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l13 = (Label)item.FindControl("Label13");
            l13.Text = Convert.ToString(TotaldataKG1);

        }

        //protected void txtCableSize_4X150_OldCable_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X150_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X150_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X150_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X150_NewCable");
        //    Label l5 = (Label)item.FindControl("Label5");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l5.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_4X150_CableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X150_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X150_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X150_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X150_NewCable");
        //    Label l5 = (Label)item.FindControl("Label5");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l5.Text = Convert.ToString(Data);
        //}
        //protected void txtCableSize_4X150_OldCableReused_TextChanged(object sender, EventArgs e)
        //{
        //    int Data = 0;
        //    TextBox textBox = sender as TextBox;
        //    string theText = textBox.Text;
        //    var item = (RepeaterItem)textBox.NamingContainer;
        //    TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X150_OldCable");
        //    TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X150_CableReused");
        //    TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X150_OldCableReused");
        //    TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X150_NewCable");
        //    Label l5 = (Label)item.FindControl("Label5");
        //    Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToInt32(str1.Text);
        //    Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToInt32(str2.Text);
        //    Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToInt32(str3.Text);
        //    Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToInt32(str4.Text);
        //    l5.Text = Convert.ToString(Data);
        //}
        protected void txtCableSize_4X150_NewCable_TextChanged(object sender, EventArgs e)
        {
            double Data = 0;
            double Totaldata = 0;
            double TotaldataKG = 0;
            double TotaldataKG1 = 0;
            TextBox textBox = sender as TextBox;
            string theText = textBox.Text;
            var item = (RepeaterItem)textBox.NamingContainer;
            TextBox str1 = (TextBox)item.FindControl("txtCableSize_4X150_OldCable");
            TextBox str2 = (TextBox)item.FindControl("txtCableSize_4X150_CableReused");
            TextBox str3 = (TextBox)item.FindControl("txtCableSize_4X150_OldCableReused");
            TextBox str4 = (TextBox)item.FindControl("txtCableSize_4X150_NewCable");
            Label lt6 = (Label)item.FindControl("Label6");
            Label lt7 = (Label)item.FindControl("Label7");
            Label lt8 = (Label)item.FindControl("Label8");
            Label lt9 = (Label)item.FindControl("Label9");
            Label lt10 = (Label)item.FindControl("Label10");
            Label l5 = (Label)item.FindControl("Label5");
            Data = string.IsNullOrEmpty(str1.Text) ? 0 : Convert.ToDouble(str1.Text);
            Data += string.IsNullOrEmpty(str2.Text) ? 0 : Convert.ToDouble(str2.Text);
            Data += string.IsNullOrEmpty(str3.Text) ? 0 : Convert.ToDouble(str3.Text);
            Data += string.IsNullOrEmpty(str4.Text) ? 0 : Convert.ToDouble(str4.Text);
            l5.Text = Convert.ToString(Data);
            Label l10 = (Label)item.FindControl("Label10");
            l10.Text = Convert.ToString(Data * 2.5);


            Totaldata = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            Totaldata += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Totaldata += string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            Totaldata += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            Totaldata += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l11 = (Label)item.FindControl("Label11");
            l11.Text = Convert.ToString(Totaldata);

            TotaldataKG = string.IsNullOrEmpty(lt6.Text) ? 0 : Convert.ToDouble(lt6.Text);
            TotaldataKG += string.IsNullOrEmpty(lt7.Text) ? 0 : Convert.ToDouble(lt7.Text);
            Label l12 = (Label)item.FindControl("Label12");
            l12.Text = Convert.ToString(TotaldataKG);

            TotaldataKG1 = string.IsNullOrEmpty(lt8.Text) ? 0 : Convert.ToDouble(lt8.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt9.Text) ? 0 : Convert.ToDouble(lt9.Text);
            TotaldataKG1 += string.IsNullOrEmpty(lt10.Text) ? 0 : Convert.ToDouble(lt10.Text);
            Label l13 = (Label)item.FindControl("Label13");
            l13.Text = Convert.ToString(TotaldataKG1);
        }
    }
}