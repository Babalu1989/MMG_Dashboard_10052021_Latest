using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmAnnexure4_Penalty_ : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null && Session["VENDOR_ID"] != null)
            {
                if (!IsPostBack)
                {
                    Get_Penalty();
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
        private void Get_Penalty()
        {
            try
            {
                DataTable dtbill = objBL.GetPenalty("");
                grdPenalty.DataSource = dtbill;
                grdPenalty.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }  
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GridView6.DataSource = objBL.GetPenalty("ForExcel");
            GridView6.DataBind();
            ExporttoExcel(objBL.GetPenalty("ForExcel"));
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Annexure- 4(Penalty Sheet).xls");
                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                HttpContext.Current.Response.Write("<BR><BR><BR>");
                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' width='80%' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;' colspan='4'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Large'>");
                HttpContext.Current.Response.Write("ANNEXURE-4");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;' colspan='4'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Medium'>");
                HttpContext.Current.Response.Write("Deductions/Retention Sheet");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                int columnscount = GridView6.Columns.Count;
                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td style='background:#ADD8E6;'>");
                    HttpContext.Current.Response.Write("<B style='Font-size:Small'>");
                    HttpContext.Current.Response.Write(GridView6.Columns[j].HeaderText.ToString());
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
                HttpContext.Current.Response.Write("<Td colspan='4'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='4'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='4'>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Authorized Signatory)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td  style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(MMG Manager)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Circle Head-MMG)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td  style='Text-Align:Center;'>");
                HttpContext.Current.Response.Write("<B style='Font-size:Small;'>");
                HttpContext.Current.Response.Write("(Head-MMG)");
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("</TR>");
                HttpContext.Current.Response.Write("<TR>");
                HttpContext.Current.Response.Write("<Td colspan='4'>");
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
            foreach (GridViewRow row in grdPenalty.Rows)
            {
                Label lblPenaltyId = row.FindControl("lblPenaltyId") as Label;
                TextBox txtPenaltyAmount = row.FindControl("txtPenaltyAmount") as TextBox;
                objBL.Update_Penalty_Data(Convert.ToInt32(lblPenaltyId.Text), txtPenaltyAmount.Text,Session["UserName"].ToString());
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record save successfully');", true);
            btnGenerate.Visible = true;
            objBL.Insert_ModifyInvoce_Log("", "MMG", "", "", "", "", "", "PENALTY SUBMITED", Session["UserName"].ToString());
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmAnnexure4(Penalty).aspx");
        }
    }
}