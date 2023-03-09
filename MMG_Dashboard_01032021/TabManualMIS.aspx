<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TabManualMIS.aspx.cs" Inherits="MMG_Dashboard_01032021.TabManualMIS" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="sm" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="panel-group">
                <div class="panel">
                    <div class="panel-heading">
                        <div class="panel-title" style="text-align: center;">
                            <b>Tab/Manual MIS</b>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSearch" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlSearch" runat="server">
                                    <div class="row" style="text-align: center;">
                                        <div class="col-md-2 form-group">
                                            <b>Division</b>
                                            <asp:DropDownList ID="ddlDiv" runat="server" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlDiv_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Vendor Code</b>
                                            <asp:DropDownList ID="ddlVend" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Punched Type</b>
                                            <asp:DropDownList ID="ddlPunchType" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                <asp:ListItem Text="Tab" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Manual" Value="M"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Order No</b>
                                            <asp:TextBox ID="txtOrderNoSearch" runat="server" CssClass="form-control input-sm"
                                                placeholder="Order No" MaxLength="12" />
                                        </div>
                                        <div class="col-md-2 form-group" style="text-align: center;">
                                            <b>From Date</b>
                                            <asp:TextBox ID="txtActivityFromDate" runat="server" CssClass="form-control input-sm"
                                                placeholder="From Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtActivityFromDate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtActivityFromDate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="search"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>To Date</b>
                                            <asp:TextBox ID="txtActivityToDate" runat="server" CssClass="form-control input-sm"
                                                placeholder="To Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtActivityToDate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtActivityToDate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="search"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 form-group" style="padding-top: 20px; text-align: center;">
                                            <asp:Button ID="btnSearch" runat="server" Text="Show" CssClass="btn btn-sm btn-primary"
                                                ValidationGroup="search" OnClick="btnSearch_Click" />&nbsp;&nbsp;

                                            <asp:Button ID="btnrefresh" runat="server" Text="Refresh" CssClass="btn btn-sm btn-info"
                                                ValidationGroup="Refresh" OnClick="btnrefresh_Click" />
                                            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" CssClass="btn btn-sm btn-primary"
                                                ValidationGroup="Export" OnClick="btnExport_Click" Visible="false" />
                                        </div>
                                    </div>
                                    <div class="row" id="grdid" runat="server">
                                        <div style="overflow-x: auto; width: 100%; height: 400px;">
                                            <div class="col-md-12 form-group">
                                                 <asp:GridView ID="GridView1" runat="server" CssClass="grid table-condensed mb0 cf demo"
                                                    AutoGenerateColumns="false" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" >
                                                    <Columns>                                                      
                                                        <asp:BoundField DataField="COMPANY_CODE" HeaderText="Company" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DIVISION" HeaderText="Division" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="VENDOR_CODE" HeaderText="Vendor Id" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ORDERID" HeaderText="Order Id" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ACTIVITY_DATE" HeaderText="Activity Date" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ORDER_TYPE" HeaderText="Order Type" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PM_ACTIVITY" HeaderText="Activity Type" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ACCOUNT_CLASS" HeaderText="Account Class" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PLANNER_GROUP" HeaderText="Planner Group" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BASIC_START_DATE" HeaderText="Basic Start Date" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BASIC_FINISH_DATE" HeaderText="Basic Finish Date" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Name" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ACTIVITY_REASON" HeaderText="Activity Reason" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="MOBILE_NO" HeaderText="Mobile No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BP_NO" HeaderText="BP No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CA" HeaderText="CA" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DEVICENO" HeaderText="Meter No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="METER_PHASE" HeaderText="Meter Phase" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BOX_TYPE" HeaderText="Box Type" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BOX_NO" HeaderText="Box No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ADDRESS" HeaderText="Address" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="TERMINAL_SEAL1" HeaderText="Terminal Seal1" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="TERMINAL_SEAL2" HeaderText="Terminal Seal2" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="METERBOXSEAL1" HeaderText="Mtr Box Seal1" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="METERBOXSEAL2" HeaderText="Mtr Box Seal2" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BUSBARSEAL1" HeaderText="Bus Bar Seal1" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BUSBARSEAL2" HeaderText="Bus Bar Seal2" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="INSTALLEDBUSBAR" HeaderText="Installed Bus Bar" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BB_CABLE_USED" HeaderText="Bus Bar Cable Used"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BB_CAB_REMOVE_FRM_SITE" HeaderText="Bus Bar Remove Frm Site" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BUSBARSIZE" HeaderText="Bus Bar Size" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BUS_BAR_NO" HeaderText="Bus Bar No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BUS_BAR_DRUM_NO" HeaderText="Bus Bar Drum" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="B_BAR_CABLE_SIZE" HeaderText="BB Cable Size"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="RMVD_BB_CBL_LENTH" HeaderText="Rmvd BB Cable Len" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BUS_BAR_CABLE_LENG" HeaderText="BB Cable Len" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="RUNNING_LENGTH_FROM_BB" HeaderText="BB Run. Len Frm" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="RUNNING_LENGTH_TO_BB" HeaderText="BB Run. Len To" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="RMVD_BB_CBL_SIZE" HeaderText="Rmvd BB Cable Size" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REM_BUSBAR_SEAL1" HeaderText="Rem BB Seal1" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REM_BUSBAR_SEAL2" HeaderText="Rem BB Seal2" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="BB_CABLE_NOT_INSTALL_REASON" HeaderText="BB Not Int. Reason" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLE_REQD1" HeaderText="Cable Req." ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLE_LEN_USED" HeaderText="Cable Length Used"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLEINSTALLTYPE" HeaderText="Cable Inst. Type" ItemStyle-HorizontalAlign="Left" />
                                                        <%--<asp:BoundField DataField="CABLE_REQD" HeaderText="Terminal Seal 1" ItemStyle-HorizontalAlign="Left" />--%>
                                                        <asp:BoundField DataField="OUTPUT_CABLE_LEN_USED" HeaderText="Output Cable Len Used" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CAB_REMOVE_FRM_SITE" HeaderText="Cable Rem Frm Site" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ELCB_INSTALLED" HeaderText="ELCB Inst" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DURM_NO" HeaderText="Drum No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLESIZE_OLD" HeaderText="Calbe Size Old"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLELENGTH_OLD" HeaderText="Cable Len Old" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="MCR_NO" HeaderText="MCR No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REMARKS" HeaderText="Remarks" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLESIZE2" HeaderText="Cable Size" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLELENGTH" HeaderText="Cable Len" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="OUTPUTCABLESIZE" HeaderText="Output Cable Size" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="OUTPUTCABLELENGTH" HeaderText="OutPut Cable Len" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="RUNNINGLENGTHFROM" HeaderText="Running Len Frm" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="RUNNINGLENGTHTO" HeaderText="Running Len To" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CORD_INSTALLED" HeaderText="Card Inst." ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CABLENOTINSTALLREASON" HeaderText="Cable Not Inst. Reason" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REM_BOX_SEAL1" HeaderText="Rem Box Seal1" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REM_BOX_SEAL2" HeaderText="Rem Box Seal2" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REM_OTHER_SEAL" HeaderText="Rem Other Seal" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="EARTHING_CONNECTOR" HeaderText="Earthing Conn" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="JUBLIEE_CLAMPS" HeaderText="Jubliee Clamps" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="HELPERNAME" HeaderText="Helper Name"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CLOSEHOOKBOLT" HeaderText="Close Hook Bolt" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="NYLON_TIE" HeaderText="Nylon Tie"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="FASTNER" HeaderText="Fastner"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="POLE_CONDITION" HeaderText="Pole Condition" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="SADDLES" HeaderText="Saddles" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="HAZARDOUS_TYPE" HeaderText="Hazardous Type" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="NOS_CBLAT_POLE" HeaderText="NOC Cable Len" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ADDITIONAL_POLE_REQUIRED" HeaderText="Add Pole Req." ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="IS_RECORD_PROCESSED" HeaderText="Is Rec. Processed" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ADDITIONAL_POLE_NUMBER" HeaderText="Add. Pole No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="SERVICE_PROVIDER" HeaderText="Serv. Provider" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DRIVER_NAME" HeaderText="Driver Name" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="SUPERVISOR_NAME" HeaderText="Superviser Name" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="NOOFMETERS" HeaderText="No Of Meter" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="CONNECTEDMETERS" HeaderText="Connected Meters"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="OTHERSTICKER" HeaderText="Other Sticker" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="DB_TYPE" HeaderText="DB Type" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="GUNNYBAG_OLD" HeaderText="GunnyBag No" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="GUNNYBAGSEAL_OLD" HeaderText="GunnyBag Seal"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="LAB_TSTNG_NTC" HeaderText="Lab Testing NTC" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="IS_GNY_BAG_PREPD" HeaderText="Is GNY Bag Pred." ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="VENDOR_NAME" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="MONTH" HeaderText="Month" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="YEAR" HeaderText="Year" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="REM_CABLE_LEN" HeaderText="Rmd Cable Len" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PIERCING_CONNECTOR" HeaderText="Piercing Connector"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PVC_GLAND" HeaderText="PVC Gland" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="THIMBLE" HeaderText="Thimle"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                         <asp:BoundField DataField="PUNCH_STATUS" HeaderText="Punched Type"
                                                            ItemStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                              <Triggers>
                                <asp:PostBackTrigger ControlID="btnExport" />
                                   <asp:PostBackTrigger ControlID="btnSearch" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdateProgress ID="updateProgressMaster" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div id="light" class="white_content">
                <img src="../images/ajax-loader.gif" alt="Loading" />
            </div>
            <div id="fade" class="black_overlay">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script src="content/js/bootstrap.min.js"></script>
    <script src="content/js/jquery.min.js"></script>
    <script src="JS/Validation.js"></script>   
</asp:Content>
