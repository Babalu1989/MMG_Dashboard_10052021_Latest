using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmAnnexure2 : System.Web.UI.Page
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
        private void Get_Annexure2(string Vendor,string Month,string Year)
        {
            try
            {
                DataTable dtbill = objBL.Get_Before_Gen_Annexure2(Vendor,Month,Year);
                if (dtbill.Rows.Count > 0)
                {
                    rptrMaterialReconciliation1.DataSource = dtbill;
                    rptrMaterialReconciliation1.DataBind();
                }
                else
                {
                    rptrMaterialReconciliation1.DataSource = null;
                    rptrMaterialReconciliation1.DataBind();
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
                Result = objBL.Generate_Annexure2(ddlVendor.SelectedValue, ddlDivision.SelectedValue, "MMG",
                ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString());
                if (Result > 0)
                {
                    Get_Annexure2(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue);
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
            GridView1.DataSource = objBL.GetAnnexure2(ddlVendor.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue);
            GridView1.DataBind();
            ExporttoExcel(objBL.GetAnnexure2(ddlVendor.SelectedValue,ddlMonth.SelectedValue,ddlYear.SelectedValue));           
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Annexure-2.xls");

                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' width='100%' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                // HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;' colspan='13'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Large'>");
                HttpContext.Current.Response.Write("Annexure-2");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("<BR>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("(Material Reconciliation Statement)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Contractor: M/s");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(Session["Vendorname"].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Bill No:.");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='4' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(txtBillNo.Text.Trim());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Division:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(Session["Divison"].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Month:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='4' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(ddlMonth.SelectedItem.Text);
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Type:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write("MMG");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                HttpContext.Current.Response.Write("Period:");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='4' style='Text-Align:Left;'>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(DateTime.DaysInMonth(Convert.ToInt32(ddlYear.SelectedValue),Convert.ToInt32(ddlMonth.SelectedValue)));
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                int columnscount = GridView1.Columns.Count;
                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td style='background:#ADD8E6;'>");
                    HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                    HttpContext.Current.Response.Write(GridView1.Columns[j].HeaderText.ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
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
                HttpContext.Current.Response.Write("<Td colspan='13'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='13'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='13'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='6' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Authorized Signatory)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td colspan='7' style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(MMG Manager)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='13'>");
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
            string Bill_No = txtBillNo.Text.ToString();
            string WorkOrder_No = txtWorkOrderNo.Text.ToString();
            string VendorId = ddlVendor.SelectedValue;
            string month = ddlMonth.SelectedValue;
            string year = ddlYear.SelectedValue;
            string itemCode = string.Empty;
            double prevMonthBalance;
            double receivedFromOtherDivision;
            double removedFromSite;
            double receivedFromStore;
            double total;
            double quantityConsumed;
            double returnToStore;
            double transferToOtherDivision;
            double balance;
            string remarks;
            try
            {
                Result = objBL.Delete_Old_Annexure2_1(month, year, VendorId, Bill_No, WorkOrder_No);
                foreach (RepeaterItem item in rptrMaterialReconciliation1.Items)
                {
                    Label lblUnit = item.FindControl("lblUnit") as Label;
                    Label lblItemName = item.FindControl("lblItemName") as Label;
                    Label lblItemId = item.FindControl("lblItemId") as Label;
                    TextBox txtPrvMonthBalance = item.FindControl("txtPrvMonthBalance") as TextBox;
                    TextBox txtReceivedFromOtherDivision = item.FindControl("txtReceivedFromOtherDivision") as TextBox;
                    TextBox txtRemovedFromSiteReused = item.FindControl("txtRemovedFromSiteReused") as TextBox;
                    TextBox txtReceivedFromStore = item.FindControl("txtReceivedFromStore") as TextBox;
                    Label lblTotal = item.FindControl("lblTotal") as Label;
                    TextBox lblQtyConsumption = item.FindControl("lblQtyConsumption") as TextBox;
                    TextBox txtReturnedToStore = item.FindControl("txtReturnedToStore") as TextBox;
                    TextBox txtTransferToOtherDivision = item.FindControl("txtTransferToOtherDivision") as TextBox;
                    Label lblBalance = item.FindControl("lblBalance") as Label;
                    TextBox txtRemarks = item.FindControl("txtRemarks") as TextBox;
                    prevMonthBalance = string.IsNullOrEmpty(txtPrvMonthBalance.Text) ? 0 : Convert.ToDouble(txtPrvMonthBalance.Text);
                    receivedFromOtherDivision = string.IsNullOrEmpty(txtReceivedFromOtherDivision.Text) ? 0 : Convert.ToDouble(txtReceivedFromOtherDivision.Text.Trim());
                    removedFromSite = string.IsNullOrEmpty(txtRemovedFromSiteReused.Text.Trim()) ? 0 : Convert.ToDouble(txtRemovedFromSiteReused.Text.Trim());
                    receivedFromStore = string.IsNullOrEmpty(txtReceivedFromStore.Text.Trim()) ? 0 : Convert.ToDouble(txtReceivedFromStore.Text.Trim());
                    total = string.IsNullOrEmpty(lblTotal.Text) ? 0 : Convert.ToDouble(lblTotal.Text);
                    quantityConsumed = string.IsNullOrEmpty(lblQtyConsumption.Text) ? 0 : Convert.ToDouble(lblQtyConsumption.Text);
                    returnToStore = string.IsNullOrEmpty(txtReturnedToStore.Text.Trim()) ? 0 : Convert.ToDouble(txtReturnedToStore.Text.Trim());
                    transferToOtherDivision = string.IsNullOrEmpty(txtReturnedToStore.Text.Trim()) ? 0 : Convert.ToDouble(txtTransferToOtherDivision.Text.Trim());
                    balance = string.IsNullOrEmpty(lblBalance.Text) ? 0 : Convert.ToDouble(lblBalance.Text);
                    remarks = txtRemarks.Text.Trim().Replace("'", "`").Replace("$", "").Replace(",", " ").ToUpper();
                    Result = objBL.Delete_Old_Annexure2(lblItemId.Text, month, year, VendorId);
                    total = (Convert.ToDouble(prevMonthBalance) + Convert.ToDouble(receivedFromOtherDivision)+ Convert.ToDouble(removedFromSite) + Convert.ToDouble(receivedFromStore));
                    balance = (total + quantityConsumed) - (returnToStore + transferToOtherDivision);
                    if (Result >= 0)
                    {
                         Result = objBL.Insert_ModifyAnnexure2_Data(lblItemId.Text,Convert.ToString(prevMonthBalance), Convert.ToString(receivedFromOtherDivision), Convert.ToString(removedFromSite), 
                             Convert.ToString(receivedFromStore), Convert.ToString(total), Convert.ToString(quantityConsumed), Convert.ToString(returnToStore), Convert.ToString(transferToOtherDivision), 
                             Convert.ToString(balance), remarks, month, year, VendorId, Session["UserName"].ToString(), lblItemName.Text, lblUnit.Text);
                         Result = objBL.Insert_ModifyAnnexure2_Data_1(lblItemId.Text, Convert.ToString(prevMonthBalance), Convert.ToString(receivedFromOtherDivision), Convert.ToString(removedFromSite), 
                             Convert.ToString(receivedFromStore), Convert.ToString(total), Convert.ToString(quantityConsumed), Convert.ToString(returnToStore), Convert.ToString(transferToOtherDivision), 
                             Convert.ToString(balance), remarks, month, year, VendorId, Session["UserName"].ToString(),Bill_No, WorkOrder_No, lblItemName.Text, lblUnit.Text);
                    }
                    if (Result > 0)
                    {
                        btnGenerate.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record save successfully');", true);
                    }
                }
                objBL.Insert_ModifyInvoce_Log(ddlDivision.SelectedValue, "MMG", ddlMonth.SelectedValue, ddlYear.SelectedValue, txtBillNo.Text.ToString(), txtWorkOrderNo.Text.ToString(), ddlVendor.SelectedValue, "ANNEXURE2 SUBMITED", Session["UserName"].ToString());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAnnexure2.aspx");
        }
    }
}