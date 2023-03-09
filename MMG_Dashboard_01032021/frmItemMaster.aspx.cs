using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMG_Dashboard_01032021
{
    public partial class frmItemMaster : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlMsg.Visible = false;
            if (Session["UserName"] != null && Session["VENDOR_ID"] != null)
            {
                if (!IsPostBack)
                {
                    txtPeriodFromSearch.Text = DateTime.Now.AddDays(-15).ToString("dd/MM/yyyy");
                    txtPeriodToSearch.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    if (Session["UserName"] != null)
                    {
                        lblUserId.Text = Session["UserName"].ToString();
                        GetItem_Details("", "", "", "", "");
                    }
                    else
                    {
                        Response.Redirect("/default.aspx");
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
        private void GetItem_Details(string ItemCode, string ItemDesc, string Periodfromdate, string Periodtodate, string Status)
        {
            try
            {
                DataTable dtorder = objBL.GetItemDetails(ItemCode, ItemDesc, Periodfromdate, Periodtodate, Status);
                if (dtorder.Rows.Count > 0)
                {
                    grdSearch.DataSource = dtorder;
                    grdSearch.DataBind();
                }
                else
                {
                    grdSearch.DataSource = null;
                    grdSearch.DataBind();
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('No Record Found!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            txtItemCode.Enabled = true;
            txtItemDescription.Enabled = true;
            txtUnit.Enabled = true;
            pnlAddNew.Visible = true;
            pnlSearch.Visible = false;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetItem_Details(txtItemCodeSearch.Text.ToString(), txtItemDescriptionSearch.Text.ToString(), txtPeriodFromSearch.Text.ToString(), txtPeriodToSearch.Text.ToString(), ddlStatusSearch.SelectedValue);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void grdSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            btnSave.Text = "Update";
            pnlAddNew.Visible = true;
            pnlSearch.Visible = false;
            txtItemCode.Enabled = false;
            txtItemDescription.Enabled = false;
            txtUnit.Enabled = false;
            try
            {
                string s2 = e.CommandArgument.ToString();
                DataTable dt = objBL.GetItemDetails(e.CommandArgument.ToString());
                if (dt.Rows.Count > 0)
                {
                    txtItemCode.Text = dt.Rows[0]["ITEMCODE"].ToString();
                    txtItemDescription.Text = dt.Rows[0]["DESCRIPTION"].ToString();
                    txtUnit.Text = dt.Rows[0]["UNIT"].ToString();
                    txtUnitRate.Text = dt.Rows[0]["UNITRATE"].ToString();
                    if (!String.IsNullOrEmpty(dt.Rows[0]["PERIOD_FROM"].ToString()))
                    {
                        txtPeriodFrom.Text = Convert.ToDateTime(dt.Rows[0]["PERIOD_FROM"]).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtPeriodFrom.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmItemMaster.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int result = 0;
            try
            {
                if (btnSave.Text == "Save")
                {
                    result = objBL.Insert_Item_Data(txtItemCode.Text.ToString(), txtItemDescription.Text.ToString(), txtUnit.Text.ToString(),
                     txtUnitRate.Text.ToString(), txtPeriodFrom.Text.ToString());
                    if (result > 0)
                    {
                        GetItem_Details("", "", "", "", "");
                        pnlSearch.Visible = true;
                        pnlAddNew.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record saved successfully');", true);
                        return;
                    }
                }
                else if (btnSave.Text == "Update")
                {
                    result = objBL.Update_Item_Data(txtItemCode.Text.ToString(), txtItemDescription.Text.ToString(), txtUnit.Text.ToString(),
                     txtUnitRate.Text.ToString(), txtPeriodFrom.Text.ToString());
                    if (result > 0)
                    {
                        pnlSearch.Visible = true;
                        pnlAddNew.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record updated successfully');", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
    }
}