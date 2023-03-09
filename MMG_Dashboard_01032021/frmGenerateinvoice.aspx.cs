using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmGenerateinvoice : System.Web.UI.Page
    {
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
        private void Get_Invoice(string Month, string Year, string Vendorid)
        {
            try
            {
                DataTable dtbill = objBL.GetGenerate_Invoice(Month, Year, Vendorid);
                GrdInvoice.DataSource = dtbill;
                GrdInvoice.DataBind();
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
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            ExporttoExcel(objBL.GetGenerate_Invoice1(ddlMonth.SelectedValue, ddlYear.SelectedValue, ddlVendor.SelectedValue));
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Invoice Sheet.xls");
                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' width='80%' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;' colspan='6'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Large'>");
                HttpContext.Current.Response.Write("Invoice Sheet");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Contractor: M/s");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(Session["Vendorname"].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Bill No:.");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(txtBillNo.Text.Trim());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Division:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(Session["Divison"].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Month:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(ddlMonth.SelectedItem.Text);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Type:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write("MMG");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Period:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='2' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue)));
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Work Order No:.");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='5' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(txtWorkOrderNo.Text.Trim());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                int columnscount = GrdInvoice.Columns.Count;
                decimal Sum = 0;
                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td style='background:#ADD8E6;Text-Align:Center;'>");
                    HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                    HttpContext.Current.Response.Write(GrdInvoice.Columns[j].HeaderText.ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    Sum += Convert.ToDecimal(row["Amount"]);
                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
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
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("Total");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write(Sum);
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
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Contractor's)");
                HttpContext.Current.Response.Write("</B>");
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
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Circle Head- MMG)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Authorized Signatory)");
                HttpContext.Current.Response.Write("</B>");
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
        protected void GrdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblItemCode = e.Row.FindControl("lblItemCode") as Label;
                TextBox txtItemcode = e.Row.FindControl("txtItemcode") as TextBox;
                Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;

                Label lblAmount = e.Row.FindControl("lblAmount") as Label;
                //TextBox txtAmount = e.Row.FindControl("txtAmount") as TextBox;

                if (lblItemCode.Text == "3012321" || lblItemCode.Text == "4060121" ||
                    lblItemCode.Text == "4060663" || lblItemCode.Text == "4060664" ||
                    lblItemCode.Text == "NA1" || lblItemCode.Text == "NA2" || lblItemCode.Text == "NA3")
                {
                    txtQuantity.Visible = true;
                    lblQuantity.Visible = false;

                    //txtAmount.Visible = true;
                    //lblAmount.Visible = false;
                }
                if (lblItemCode.Text == "NA1" || lblItemCode.Text == "NA2" || lblItemCode.Text == "NA3")
                {
                    txtItemcode.Visible = true;
                    lblItemCode.Visible = false;
                }
            }
        }
        protected void btnCreateinvoice_Click(object sender, EventArgs e)
        {
            int Result = 0;
            try
            {
                Result = objBL.Insert_Generate_Invoice(ddlVendor.SelectedValue, ddlDivision.SelectedValue, "MMG",
                ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString());
                if (Result > 0)
                {
                    Get_Invoice(ddlMonth.SelectedValue, ddlYear.SelectedValue, ddlVendor.SelectedValue);
                    btnSubmit.Visible = true;
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int Result = 0;
            string Bill_No = txtBillNo.Text.ToString();
            string WorkOrder_No = txtWorkOrderNo.Text.ToString();
            string VendorId = ddlVendor.SelectedValue;
            string month = ddlMonth.SelectedValue;
            string year = ddlYear.SelectedValue;
            string itemCode = string.Empty;
            double unitRate;
            double quantity;
            double amount;
            try
            {
                Result = objBL.Delete_Old_Invoice_Data_1(month, year, VendorId, Bill_No, WorkOrder_No);
                foreach (GridViewRow row in GrdInvoice.Rows)
                {
                    Label lblDesc = row.FindControl("lblDesc") as Label;
                    Label lblUnit = row.FindControl("lblunit") as Label;
                    Label lblItemCode = row.FindControl("lblItemCode") as Label;
                    TextBox txtItemCode = row.FindControl("txtItemcode") as TextBox;
                    Label lblUnitRate = row.FindControl("lblUnitRate") as Label;
                    Label lblQuantity = row.FindControl("lblQuantity") as Label;
                    Label lblAmount = row.FindControl("lblAmount") as Label;
                    TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
                    itemCode = lblItemCode.Text.Trim();
                    unitRate = !string.IsNullOrEmpty(lblUnitRate.Text) ? Convert.ToDouble(lblUnitRate.Text) : 0;
                    if (lblItemCode.Text == "3012321" || lblItemCode.Text == "4060121" ||
                    lblItemCode.Text == "4060663" || lblItemCode.Text == "4060664" ||
                    lblItemCode.Text == "NA1" || lblItemCode.Text == "NA2" || lblItemCode.Text == "NA3")
                    {
                        quantity = !string.IsNullOrEmpty(txtQuantity.Text) ? Convert.ToDouble(txtQuantity.Text) : 0;
                        amount = (Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(lblUnitRate.Text));
                    }
                    else
                    {
                        quantity = !string.IsNullOrEmpty(lblQuantity.Text) ? Convert.ToDouble(lblQuantity.Text) : 0;
                        amount = (Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(lblUnitRate.Text));
                    }                
                    Result = objBL.Delete_Old_Invoice_Data(lblItemCode.Text, month, year, VendorId);
                    //Result = objBL.Delete_Old_Invoice_Data_1(lblItemCode.Text, month, year, VendorId, Bill_No, WorkOrder_No);                   
                    if (Result >= 0)
                    {
                        Result = objBL.Insert_ModifyInvoce_Data(lblItemCode.Text, lblUnitRate.Text, lblQuantity.Text, lblAmount.Text, txtQuantity.Text, month, year, VendorId, Session["UserName"].ToString(), txtItemCode.Text);
                        Result = objBL.Insert_ModifyInvoce_Data_1(lblItemCode.Text, lblUnitRate.Text, lblQuantity.Text, lblAmount.Text, txtQuantity.Text, month, year, VendorId, Session["UserName"].ToString(), txtItemCode.Text, Bill_No, WorkOrder_No, lblDesc.Text, lblUnit.Text, Convert.ToString(amount));
                    }
                    if (Result > 0)
                    {
                        btnGenerate.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record save successfully');", true);
                    }
                }
                objBL.Insert_ModifyInvoce_Log(ddlDivision.SelectedValue, "MMG", ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString(), ddlVendor.SelectedValue, "BILL SUBMITED", Session["UserName"].ToString());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmGenerateinvoice.aspx");
        }
    }
}