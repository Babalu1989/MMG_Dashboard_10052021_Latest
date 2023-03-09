using common;
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
    public partial class TabPunch : System.Web.UI.Page
    {
        SimpleBL objBL = new SimpleBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null && Session["VENDOR_ID"] != null)
            {
                if (!IsPostBack)
                {
                    txtActivityFromDate.Text = DateTime.Now.AddDays(-3).ToString("dd-MM-yyyy");
                    txtActivityToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    Division();
                    Division1();
                   // GetOrder_Details("", "", "", "", "", "", txtActivityFromDate.Text, txtActivityToDate.Text);
                    pnlAddNew.Visible = false;
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
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearControls();
            pnlAddNew.Visible = true;
            grdid.Visible = false;
            pnlSearch.Visible = false;
            if (btnSave.Text == "Update")
            {
                btnSave.Text = "Save";
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
                    ddlvendor.DataSource = dtdiv;
                    ddlvendor.DataTextField = "NAME";
                    ddlvendor.DataValueField = "VENDOR_ID";
                    ddlvendor.DataBind();
                    ddlvendor.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        private void GetOrder_Details(string Division, string Vendorid, string Orderid, string CA, string Month, string Year, string Activityfromdate, string Activitytodate)
        {
            string orderNo = txtOrderNoSearch.Text.Trim().Replace("'", "`").Replace("$", "").Replace(",", " ").ToUpper();
            try
            {
                DataTable dtorder = objBL.GetOrderTabDetails(Division, Vendorid, orderNo, CA, Month, Year, txtActivityFromDate.Text, txtActivityToDate.Text);
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
        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            Vendor(ddlDivision.SelectedValue);
        }
        private void ActivityType(string orderType)
        {
            ddlActivityType.Items.Clear();
            ddlActivityType.Items.Add(new ListItem("Select", ""));
            try
            {
                DataTable dtact = objBL.GetActivityType(orderType);
                if (dtact.Rows.Count > 0)
                {
                    ddlActivityType.DataSource = dtact;
                    ddlActivityType.DataTextField = "ACTIVITY_DESC";
                    ddlActivityType.DataValueField = "ACTIVITYtYPE";
                    ddlActivityType.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActivityType(ddlOrderType.SelectedValue);
        }
        public void Save_Data()
        {
            int Result = 0;
            DAL objDal = new DAL();
            string ACTIVITY_DATE = Utility.GetMMDDYYYY(txtActivityDate.Text);
            string orderMonth = ACTIVITY_DATE.Split('/').GetValue(0).ToString();
            string orderYear = ACTIVITY_DATE.Split('/').GetValue(2).ToString();
            try
            {
                Result = objBL.Insert_Manual_Data(ddlcompany.SelectedValue, ddlDivision.SelectedValue, ddlvendor.SelectedValue, txtorderno.Text.ToString().Trim(), txtActivityDate.Text.ToString(), ddlOrderType.SelectedValue,
                ddlActivityType.SelectedValue, ddlAccountclass.SelectedValue, ddlPlannergroup.SelectedValue, txtBasicstartdate.Text.ToString(), txtBasicfinishdate.Text.ToString(),
                txtCustName.Text.ToString(), txtActivityreason.Text.ToString(), txtMobileno.Text.ToString(), txtBpno.Text.ToString(), txtCANo.Text.ToString(), txtMeterNo.Text.ToString(),
                ddlMeterPhase.SelectedValue, ddlBoxType.SelectedValue, txtBoxNo.Text.ToString(), txtAddress.Text.ToString(), txtTerminalSeal1.Text.ToString(), txtTerminalSeal2.Text.ToString(),
                txtMtrBoxSeal1.Text.ToString(), txtMtrBoxSeal2.Text.ToString(), txtBusBarSeal1.Text.ToString(), txtBusBarSeal2.Text.ToString(), rdoIsInstalledBusBar.SelectedValue, rdoBusBarUse.SelectedValue, rdoBusBarRemoveFromSite.SelectedValue,
                ddlBusBarType.SelectedValue, txtBusBarNo.Text.ToString(), txtBusbardrumno.Text.ToString(), ddlBusBarCableSize.SelectedValue, txtRemovalbusbarcablelength.Text.ToString(), txtBusBarCableLength.Text.ToString(), txtBusBarRunninglengthFrom.Text.ToString(),
                txtBusBarRunninglengthTo.Text.ToString(), txtRemovalBusbarcablesize.Text.ToString(), txtRemovalBusBarSeal1.Text.ToString(), txtRemovalBusBarSeal2.Text.ToString(), txtBuabarcablenotinstalledreason.Text.ToString(),
                rdoIsInstalledCable.SelectedValue, rdoInstalCableUsedType.SelectedValue, rdoCableInstallationType.SelectedValue, rdbCablerequired.SelectedValue, rdoOutputCableType.SelectedValue, rdoIsCableRemovefromsite.SelectedValue,
                rdoELCBInstalled.SelectedValue, txtDrumno.Text.ToString(), ddlRemovecablesize.SelectedValue, txtrmvcablelength.Text.ToString(), ddlMatdesc.Text.ToString(),
                txtRemoveCableRemarks.Text.ToString(), ddlInstalledCableSize.SelectedValue, txtInstalledCableLength.Text.ToString(), ddlOutputCableSize.SelectedValue, txtOutputCableLength.Text.ToString(), txtRunninglengthFrom.Text.ToString(),
                txtRunninglengthTo.Text.ToString(), txtRemoveCableLength.Text.ToString(), txtcableinstalledreason.Text.ToString(), txtRmovalboxseal1.Text.ToString(), txtRmovalboxseal2.Text.ToString(), txtRemovalotherseal.Text.ToString(),
                txtEarthingconnector.Text.ToString(), txtjubleeclamp.Text.ToString(), txtHelpername.Text.ToString(), txtClosehookbolt.Text.ToString(), txtNylon.Text.ToString(),
                txtFastner.Text.ToString(), txtPolecondition.Text.ToString(), txtSaddle.Text.ToString(), txtHazardous.Text.ToString(), txtNOScableat.Text.ToString(), txtAdditionalpole.Text.ToString(), rdbisrecordprocess.SelectedValue,
                txtAdditionalpoleno.Text.ToString(), txtServiceprovider.Text.ToString(), txtDrivername.Text.ToString(), txtSuperwisername.Text.ToString(), txtnoofmtr.Text.ToString(), txtconnectedmtr.Text.ToString(), txtOtherSticker.Text.ToString(),
                ddlDbtype.Text.ToString(), txtGunnyBagNumber.Text.ToString(), txtGunnyBagSeal.Text.ToString(), txtGunnyBagNoticeNo.Text.ToString(), rdoIsGunnyBag.SelectedValue, ddlvendor.SelectedItem.Text.ToString(), orderMonth, orderYear,
                txtPiercingConnector.Text.ToString(), txtPVCGland.Text.ToString(), txtThimble.Text.ToString(), txtAnchorpoleendqty.Text.ToString(), Convert.ToString(Session["UserName"]));
                if (Result > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('Record saved successfully');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
                return;
            }
            finally
            {
                objDal.CloseConnection(objDal.cnnConnection);
                objDal = null;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int Result = 0;
            try
            {
                if (btnSave.Text == "Save")
                {
                    Save_Data();
                }
                else if (btnSave.Text == "Update")
                {
                    Result = objBL.Update_Manual_Data(ddlcompany.SelectedValue, ddlDivision.SelectedValue, ddlvendor.SelectedValue, txtorderno.Text.ToString().Trim(), txtActivityDate.Text.ToString(), ddlOrderType.SelectedValue,
                                  ddlActivityType.SelectedValue, ddlAccountclass.SelectedValue, ddlPlannergroup.SelectedValue, txtBasicstartdate.Text.ToString(), txtBasicfinishdate.Text.ToString(),
                                  txtCustName.Text.ToString(), txtActivityreason.Text.ToString(), txtMobileno.Text.ToString(), txtBpno.Text.ToString(), txtCANo.Text.ToString(), txtMeterNo.Text.ToString(),
                                  ddlMeterPhase.SelectedValue, ddlBoxType.SelectedValue, txtBoxNo.Text.ToString(), txtAddress.Text.ToString(), txtTerminalSeal1.Text.ToString(), txtTerminalSeal2.Text.ToString(),
                                  txtMtrBoxSeal1.Text.ToString(), txtMtrBoxSeal2.Text.ToString(), txtBusBarSeal1.Text.ToString(), txtBusBarSeal2.Text.ToString(), rdoIsInstalledBusBar.SelectedValue, rdoBusBarUse.SelectedValue, rdoBusBarRemoveFromSite.SelectedValue,
                                  ddlBusBarType.SelectedValue, txtBusBarNo.Text.ToString(), txtBusbardrumno.Text.ToString(), ddlBusBarCableSize.SelectedValue, txtRemovalbusbarcablelength.Text.ToString(), txtBusBarCableLength.Text.ToString(), txtBusBarRunninglengthFrom.Text.ToString(),
                                  txtBusBarRunninglengthTo.Text.ToString(), txtRemovalBusbarcablesize.Text.ToString(), txtRemovalBusBarSeal1.Text.ToString(), txtRemovalBusBarSeal2.Text.ToString(), txtBuabarcablenotinstalledreason.Text.ToString(),
                                  rdoIsInstalledCable.SelectedValue, rdoInstalCableUsedType.SelectedValue, rdoCableInstallationType.SelectedValue, rdbCablerequired.SelectedValue, rdoOutputCableType.SelectedValue, rdoIsCableRemovefromsite.SelectedValue,
                                  rdoELCBInstalled.SelectedValue, txtDrumno.Text.ToString(), ddlRemovecablesize.SelectedValue, txtrmvcablelength.Text.ToString(), ddlMatdesc.Text.ToString(),
                                  txtRemoveCableRemarks.Text.ToString(), ddlInstalledCableSize.SelectedValue, txtInstalledCableLength.Text.ToString(), ddlOutputCableSize.SelectedValue, txtOutputCableLength.Text.ToString(), txtRunninglengthFrom.Text.ToString(),
                                  txtRunninglengthTo.Text.ToString(), txtRemoveCableLength.Text.ToString(), txtcableinstalledreason.Text.ToString(), txtRmovalboxseal1.Text.ToString(), txtRmovalboxseal2.Text.ToString(), txtRemovalotherseal.Text.ToString(),
                                  txtEarthingconnector.Text.ToString(), txtjubleeclamp.Text.ToString(), txtHelpername.Text.ToString(), txtClosehookbolt.Text.ToString(), txtNylon.Text.ToString(),
                                  txtFastner.Text.ToString(), txtPolecondition.Text.ToString(), txtSaddle.Text.ToString(), txtHazardous.Text.ToString(), txtNOScableat.Text.ToString(), txtAdditionalpole.Text.ToString(), rdbisrecordprocess.SelectedValue,
                                  txtAdditionalpoleno.Text.ToString(), txtServiceprovider.Text.ToString(), txtDrivername.Text.ToString(), txtSuperwisername.Text.ToString(), txtnoofmtr.Text.ToString(), txtconnectedmtr.Text.ToString(), txtOtherSticker.Text.ToString(),
                                  ddlDbtype.Text.ToString(), txtGunnyBagNumber.Text.ToString(), txtGunnyBagSeal.Text.ToString(), txtGunnyBagNoticeNo.Text.ToString(), rdoIsGunnyBag.SelectedValue, txtPiercingConnector.Text.ToString(), txtPVCGland.Text.ToString(),
                                  txtThimble.Text.ToString(), txtAnchorpoleendqty.Text.ToString(), Convert.ToString(Session["UserName"]));
                    if (Result > 0)
                    {
                        txtorderno.Enabled = false;
                        pnlAddNew.Visible = false;
                        grdid.Visible = true;
                        pnlSearch.Visible = true;
                        GetOrder_Details("", "", "", "", "", "", "", "");
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
        protected void grdSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            btnSave.Text = "Update";
            pnlAddNew.Visible = true;
            grdid.Visible = false;
            pnlSearch.Visible = false;
            try
            {
                string s2 = e.CommandArgument.ToString();
                DataTable dt = objBL.GetDetails(e.CommandArgument.ToString());
                if (dt.Rows.Count > 0)
                {
                    ddlcompany.SelectedItem.Text = dt.Rows[0]["COMPANY_CODE"].ToString();
                    for (int f = 0; f < ddlDivision.Items.Count; f++)
                    {
                        if (ddlDivision.Items[f].Value == dt.Rows[0]["DIVISION"].ToString())
                        {
                            ddlDivision.SelectedIndex = f;
                            break;
                        }
                    }
                    Vendor(ddlDivision.SelectedValue);
                    for (int d = 0; d < ddlvendor.Items.Count; d++)
                    {
                        if (ddlvendor.Items[d].Value == dt.Rows[0]["VENDOR_CODE"].ToString())
                        {
                            ddlvendor.SelectedIndex = d;
                            break;
                        }
                    }
                    txtorderno.Text = dt.Rows[0]["ORDERID"].ToString();
                    txtActivityDate.Text = Convert.ToDateTime(dt.Rows[0]["ACTIVITY_DATE"]).ToString("dd/MM/yyyy");
                    txtActidate.Text = Convert.ToDateTime(dt.Rows[0]["ACTIVITY_DATE"]).ToString("dd/MM/yyyy");

                    ddlOrderType.SelectedValue = dt.Rows[0]["ORDER_TYPE"].ToString();
                    ActivityType(ddlOrderType.SelectedValue);

                    ddlActivityType.SelectedValue = dt.Rows[0]["PM_ACTIVITY"].ToString();
                    ddlAccountclass.SelectedValue = dt.Rows[0]["ACCOUNT_CLASS"].ToString();
                    ddlPlannergroup.SelectedValue = dt.Rows[0]["PLANNER_GROUP"].ToString();
                    if ((!string.IsNullOrEmpty(dt.Rows[0]["BASIC_START_DATE"].ToString())) && (!String.IsNullOrEmpty(dt.Rows[0]["BASIC_FINISH_DATE"].ToString())))
                    {
                        txtBasicstartdate.Text = Convert.ToDateTime(dt.Rows[0]["BASIC_START_DATE"]).ToString("dd/MM/yyyy");
                        txtBasicfinishdate.Text = Convert.ToDateTime(dt.Rows[0]["BASIC_FINISH_DATE"]).ToString("dd/MM/yyyy");
                    }
                    txtCustName.Text = dt.Rows[0]["CUSTOMER_NAME"].ToString();
                    txtActivityreason.Text = dt.Rows[0]["ACTIVITY_REASON"].ToString();
                    txtMobileno.Text = dt.Rows[0]["MOBILE_NO"].ToString();
                    txtBpno.Text = dt.Rows[0]["BP_NO"].ToString();
                    txtCANo.Text = dt.Rows[0]["CA"].ToString();
                    txtMeterNo.Text = dt.Rows[0]["DEVICENO"].ToString();
                    for (int c = 0; c < ddlMeterPhase.Items.Count; c++)
                    {
                        if (ddlMeterPhase.Items[c].Value == dt.Rows[0]["METER_PHASE"].ToString())
                        {
                            ddlMeterPhase.SelectedIndex = c;
                            break;
                        }
                    }
                    for (int i = 0; i < ddlBoxType.Items.Count; i++)
                    {
                        if (ddlBoxType.Items[i].Value == dt.Rows[0]["BOX_TYPE"].ToString())
                        {
                            ddlBoxType.SelectedIndex = i;
                            break;
                        }
                    }
                    txtBoxNo.Text = dt.Rows[0]["BOX_NO"].ToString();
                    txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString();
                    txtTerminalSeal1.Text = dt.Rows[0]["TERMINAL_SEAL1"].ToString();
                    txtTerminalSeal2.Text = dt.Rows[0]["TERMINAL_SEAL2"].ToString();
                    txtMtrBoxSeal1.Text = dt.Rows[0]["METERBOXSEAL1"].ToString();
                    txtMtrBoxSeal2.Text = dt.Rows[0]["METERBOXSEAL2"].ToString();
                    txtBusBarSeal1.Text = dt.Rows[0]["BUSBARSEAL1"].ToString();
                    txtBusBarSeal2.Text = dt.Rows[0]["BUSBARSEAL2"].ToString();
                    for (int j = 0; j < rdoIsInstalledBusBar.Items.Count; j++)
                    {
                        if (rdoIsInstalledBusBar.Items[j].Value == dt.Rows[0]["INSTALLEDBUSBAR"].ToString())
                        {
                            rdoIsInstalledBusBar.SelectedIndex = j;
                            break;
                        }
                    }
                    for (int b = 0; b < rdoBusBarUse.Items.Count; b++)
                    {
                        if (rdoBusBarUse.Items[b].Value == dt.Rows[0]["BB_CABLE_USED"].ToString())
                        {
                            rdoBusBarUse.SelectedIndex = b;
                            break;
                        }
                    }
                    for (int a = 0; a < rdoBusBarRemoveFromSite.Items.Count; a++)
                    {
                        if (rdoBusBarRemoveFromSite.Items[a].Value == dt.Rows[0]["BB_CAB_REMOVE_FRM_SITE"].ToString())
                        {
                            rdoBusBarRemoveFromSite.SelectedIndex = a;
                            break;
                        }
                    }
                    for (int w = 0; w < ddlBusBarType.Items.Count; w++)
                    {
                        if (ddlBusBarType.Items[w].Value == dt.Rows[0]["BUSBARSIZE"].ToString())
                        {
                            ddlBusBarType.SelectedIndex = w;
                            break;
                        }
                    }
                    txtBusBarNo.Text = dt.Rows[0]["BUS_BAR_NO"].ToString();
                    txtBusbardrumno.Text = dt.Rows[0]["BUS_BAR_DRUM_NO"].ToString();
                    for (int v = 0; v < ddlBusBarCableSize.Items.Count; v++)
                    {
                        if (ddlBusBarCableSize.Items[v].Value == dt.Rows[0]["B_BAR_CABLE_SIZE"].ToString())
                        {
                            ddlBusBarCableSize.SelectedIndex = v;
                            break;
                        }
                    }
                    txtRemovalbusbarcablelength.Text = dt.Rows[0]["RMVD_BB_CBL_LENTH"].ToString();
                    txtBusBarCableLength.Text = dt.Rows[0]["BUS_BAR_CABLE_LENG"].ToString();
                    txtBusBarRunninglengthFrom.Text = dt.Rows[0]["RUNNING_LENGTH_FROM_BB"].ToString();
                    txtBusBarRunninglengthTo.Text = dt.Rows[0]["RUNNING_LENGTH_TO_BB"].ToString();
                    txtRemovalBusbarcablesize.Text = dt.Rows[0]["RMVD_BB_CBL_SIZE"].ToString();
                    txtRemovalBusBarSeal1.Text = dt.Rows[0]["REM_BUSBAR_SEAL1"].ToString();
                    txtRemovalBusBarSeal2.Text = dt.Rows[0]["REM_BUSBAR_SEAL2"].ToString();
                    txtBuabarcablenotinstalledreason.Text = dt.Rows[0]["BB_CABLE_NOT_INSTALL_REASON"].ToString();
                    for (int u = 0; u < rdoIsInstalledCable.Items.Count; u++)
                    {
                        if (rdoIsInstalledCable.Items[u].Value == dt.Rows[0]["CABLE_REQD1"].ToString())
                        {
                            rdoIsInstalledCable.SelectedIndex = u;
                            break;
                        }
                    }
                    for (int t = 0; t < rdoInstalCableUsedType.Items.Count; t++)
                    {
                        if (rdoInstalCableUsedType.Items[t].Value == dt.Rows[0]["CABLE_LEN_USED"].ToString())
                        {
                            rdoInstalCableUsedType.SelectedIndex = t;
                            break;
                        }
                    }
                    for (int s = 0; s < rdoCableInstallationType.Items.Count; s++)
                    {
                        if (rdoCableInstallationType.Items[s].Value == dt.Rows[0]["CABLEINSTALLTYPE"].ToString())
                        {
                            rdoCableInstallationType.SelectedIndex = s;
                            break;
                        }
                    }
                    for (int r = 0; r < rdbCablerequired.Items.Count; r++)
                    {
                        if (rdbCablerequired.Items[r].Value == dt.Rows[0]["CABLE_REQD"].ToString())
                        {
                            rdbCablerequired.SelectedIndex = r;
                            break;
                        }
                    }
                    for (int q = 0; q < rdoOutputCableType.Items.Count; q++)
                    {
                        if (rdoOutputCableType.Items[q].Value == dt.Rows[0]["OUTPUT_CABLE_LEN_USED"].ToString())
                        {
                            rdoOutputCableType.SelectedIndex = q;
                            break;
                        }
                    }
                    for (int p = 0; p < rdoIsCableRemovefromsite.Items.Count; p++)
                    {
                        if (rdoIsCableRemovefromsite.Items[p].Value == dt.Rows[0]["CAB_REMOVE_FRM_SITE"].ToString())
                        {
                            rdoIsCableRemovefromsite.SelectedIndex = p;
                            break;
                        }
                    }
                    for (int o = 0; o < rdoELCBInstalled.Items.Count; o++)
                    {
                        if (rdoELCBInstalled.Items[o].Value == dt.Rows[0]["ELCB_INSTALLED"].ToString())
                        {
                            rdoELCBInstalled.SelectedIndex = o;
                            break;
                        }
                    }
                    txtDrumno.Text = dt.Rows[0]["DURM_NO"].ToString();
                    for (int n = 0; n < ddlRemovecablesize.Items.Count; n++)
                    {
                        if (ddlRemovecablesize.Items[n].Value == dt.Rows[0]["CABLESIZE_OLD"].ToString())
                        {
                            ddlRemovecablesize.SelectedIndex = n;
                            break;
                        }
                    }
                    txtrmvcablelength.Text = dt.Rows[0]["CABLELENGTH_OLD"].ToString();
                   // ddlMatdesc.Text = dt.Rows[0]["MCR_NO"].ToString();
                    txtRemoveCableRemarks.Text = dt.Rows[0]["REMARKS"].ToString();
                    for (int g = 0; g < ddlInstalledCableSize.Items.Count; g++)
                    {
                        if (ddlInstalledCableSize.Items[g].Value == dt.Rows[0]["CABLESIZE2"].ToString())
                        {
                            ddlInstalledCableSize.SelectedIndex = g;
                            break;
                        }
                    }
                    txtInstalledCableLength.Text = dt.Rows[0]["CABLELENGTH"].ToString();
                    for (int m = 0; m < ddlOutputCableSize.Items.Count; m++)
                    {
                        if (ddlOutputCableSize.Items[m].Value == dt.Rows[0]["OUTPUTCABLESIZE"].ToString())
                        {
                            ddlOutputCableSize.SelectedIndex = m;
                            break;
                        }
                    }
                    txtOutputCableLength.Text = dt.Rows[0]["OUTPUTCABLELENGTH"].ToString();
                    txtRunninglengthFrom.Text = dt.Rows[0]["RUNNINGLENGTHFROM"].ToString();
                    txtRunninglengthTo.Text = dt.Rows[0]["RUNNINGLENGTHTO"].ToString();
                    txtRemoveCableLength.Text = dt.Rows[0]["CORD_INSTALLED"].ToString();
                    txtcableinstalledreason.Text = dt.Rows[0]["CABLENOTINSTALLREASON"].ToString();
                    txtRmovalboxseal1.Text = dt.Rows[0]["REM_BOX_SEAL1"].ToString();
                    txtRmovalboxseal2.Text = dt.Rows[0]["REM_BOX_SEAL2"].ToString();
                    txtRemovalotherseal.Text = dt.Rows[0]["REM_OTHER_SEAL"].ToString();
                    txtEarthingconnector.Text = dt.Rows[0]["EARTHING_CONNECTOR"].ToString();
                    txtjubleeclamp.Text = dt.Rows[0]["JUBLIEE_CLAMPS"].ToString();
                    txtHelpername.Text = dt.Rows[0]["HELPERNAME"].ToString();
                    txtClosehookbolt.Text = dt.Rows[0]["CLOSEHOOKBOLT"].ToString();
                    txtNylon.Text = dt.Rows[0]["NYLON_TIE"].ToString();
                    txtFastner.Text = dt.Rows[0]["FASTNER"].ToString();
                    txtPolecondition.Text = dt.Rows[0]["POLE_CONDITION"].ToString();
                    txtSaddle.Text = dt.Rows[0]["SADDLES"].ToString();
                    txtHazardous.Text = dt.Rows[0]["HAZARDOUS_TYPE"].ToString();
                    txtNOScableat.Text = dt.Rows[0]["NOS_CBLAT_POLE"].ToString();
                    txtAdditionalpole.Text = dt.Rows[0]["ADDITIONAL_POLE_REQUIRED"].ToString();
                    for (int l = 0; l < rdbisrecordprocess.Items.Count; l++)
                    {
                        if (rdbisrecordprocess.Items[l].Value == dt.Rows[0]["IS_RECORD_PROCESSED"].ToString())
                        {
                            rdbisrecordprocess.SelectedIndex = l;
                            break;
                        }
                    }
                    txtAdditionalpoleno.Text = dt.Rows[0]["ADDITIONAL_POLE_NUMBER"].ToString();
                    txtServiceprovider.Text = dt.Rows[0]["SERVICE_PROVIDER"].ToString();
                    txtDrivername.Text = dt.Rows[0]["DRIVER_NAME"].ToString();
                    txtSuperwisername.Text = dt.Rows[0]["SUPERVISOR_NAME"].ToString();
                    txtnoofmtr.Text = dt.Rows[0]["NOOFMETERS"].ToString();
                    txtconnectedmtr.Text = dt.Rows[0]["CONNECTEDMETERS"].ToString();
                    txtOtherSticker.Text = dt.Rows[0]["OTHERSTICKER"].ToString();
                    ddlDbtype.Text = dt.Rows[0]["DB_TYPE"].ToString();
                    txtGunnyBagNumber.Text = dt.Rows[0]["GUNNYBAG_OLD"].ToString();
                    txtGunnyBagSeal.Text = dt.Rows[0]["GUNNYBAGSEAL_OLD"].ToString();
                    txtGunnyBagNoticeNo.Text = dt.Rows[0]["LAB_TSTNG_NTC"].ToString();
                    for (int k = 0; k < rdoIsGunnyBag.Items.Count; k++)
                    {
                        if (rdoIsGunnyBag.Items[k].Value == dt.Rows[0]["IS_GNY_BAG_PREPD"].ToString())
                        {
                            rdoIsGunnyBag.SelectedIndex = k;
                            break;
                        }
                    }
                    txtPiercingConnector.Text = dt.Rows[0]["PIERCING_CONNECTOR"].ToString();
                    txtPVCGland.Text = dt.Rows[0]["PVC_GLAND"].ToString();
                    txtThimble.Text = dt.Rows[0]["THIMBLE"].ToString();
                    txtAnchorpoleendqty.Text = dt.Rows[0]["ANCHOR_POLE_END_QTY"].ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("TabPunch.aspx");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetOrder_Details(ddlDiv.SelectedValue, ddlVend.SelectedValue, txtOrderNoSearch.Text.ToString(), txtCa.Text,
                 ddlMonth.SelectedValue, ddlYear.SelectedValue, txtActivityFromDate.Text.ToString(), txtActivityToDate.Text.ToString());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Javascritp", "alert('" + ex.Message.ToString() + "');", true);
            }
        }
        private void Division1()
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
        private void Vendor1(string Vendorid)
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
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("TabPunch.aspx");
        }
        private void ClearControls()
        {
            ddlcompany.SelectedIndex = 0;
            ddlDivision.SelectedIndex = 0;
            //ddlvendor.SelectedIndex = 0;
            txtorderno.Text = "";
            txtActivityDate.Text = "";
            ddlOrderType.SelectedIndex = 0;
            ddlActivityType.SelectedItem.Text = "";
            ddlAccountclass.SelectedIndex = 0;
            ddlPlannergroup.SelectedIndex = 0;

            txtBasicstartdate.Text = "";
            txtBasicfinishdate.Text = "";

            txtCustName.Text = "";
            txtActivityreason.Text = "";
            txtMobileno.Text = "";
            txtBpno.Text = "";
            txtCANo.Text = "";
            txtMeterNo.Text = "";
            ddlMeterPhase.SelectedIndex = 0;
            ddlBoxType.SelectedIndex = 0;
            txtBoxNo.Text = "";
            txtAddress.Text = "";
            txtTerminalSeal1.Text = "";
            txtTerminalSeal2.Text = "";
            txtMtrBoxSeal1.Text = "";
            txtMtrBoxSeal2.Text = "";
            txtBusBarSeal1.Text = "";
            txtBusBarSeal2.Text = "";
            rdoIsInstalledBusBar.SelectedIndex = 0;
            rdoBusBarUse.SelectedIndex = 0;
            rdoBusBarRemoveFromSite.SelectedIndex = 0;
            ddlBusBarType.SelectedIndex = 0;
            txtBusBarNo.Text = "";
            txtBusbardrumno.Text = "";
            ddlBusBarCableSize.SelectedIndex = 0;
            txtRemovalbusbarcablelength.Text = "";
            txtBusBarCableLength.Text = "";
            txtBusBarRunninglengthFrom.Text = "";
            txtBusBarRunninglengthTo.Text = "";
            txtRemovalBusbarcablesize.Text = "";
            txtRemovalBusBarSeal1.Text = "";
            txtRemovalBusBarSeal2.Text = "";
            txtBuabarcablenotinstalledreason.Text = "";
            rdoIsInstalledCable.SelectedIndex = 0;
            rdoInstalCableUsedType.SelectedIndex = 0;
            rdoCableInstallationType.SelectedIndex = 0;
            rdbCablerequired.SelectedIndex = 0;
            rdoOutputCableType.SelectedIndex = 0;
            rdoIsCableRemovefromsite.SelectedIndex = 0;
            rdoELCBInstalled.SelectedIndex = 0;
            txtDrumno.Text = "";
            ddlRemovecablesize.SelectedIndex = 0;
            txtrmvcablelength.Text = "";
            ddlMatdesc.Text = "";
            txtRemoveCableRemarks.Text = "";
            ddlInstalledCableSize.SelectedIndex = 0;
            txtInstalledCableLength.Text = "";
            ddlOutputCableSize.SelectedValue = "";
            txtOutputCableLength.Text = "";
            txtRunninglengthFrom.Text = "";
            txtRunninglengthTo.Text = "";
            txtRemoveCableLength.Text = "";
            txtcableinstalledreason.Text = "";
            txtRmovalboxseal1.Text = "";
            txtRmovalboxseal2.Text = "";
            txtRemovalotherseal.Text = "";
            txtEarthingconnector.Text = "";
            txtjubleeclamp.Text = "";
            txtHelpername.Text = "";
            txtClosehookbolt.Text = "";
            txtNylon.Text = "";
            txtFastner.Text = "";
            txtPolecondition.Text = "";
            txtSaddle.Text = "";
            txtHazardous.Text = "";
            txtNOScableat.Text = "";
            txtAdditionalpole.Text = "";
            rdbisrecordprocess.SelectedIndex = 0;
            txtAdditionalpoleno.Text = "";
            txtServiceprovider.Text = "";
            txtDrivername.Text = "";
            txtSuperwisername.Text = "";
            txtnoofmtr.Text = "";
            txtconnectedmtr.Text = "";
            txtOtherSticker.Text = "";
            ddlDbtype.Text = "";
            txtGunnyBagNumber.Text = "";
            txtGunnyBagSeal.Text = "";
            txtGunnyBagNoticeNo.Text = "";
            rdoIsGunnyBag.SelectedIndex = 0;
            txtPiercingConnector.Text = "";
            txtPVCGland.Text = "";
            txtThimble.Text = "";
            txtAnchorpoleendqty.Text = "";
        }
        protected void ddlDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            Vendor1(ddlDiv.SelectedValue);
        }
    }
}