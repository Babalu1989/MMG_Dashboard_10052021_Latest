<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmScrapformat.aspx.cs" Inherits="MMG_Dashboard_01032021.frmScrapformat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function fncheck() {
            var ddldiv = document.getElementById("<%=ddlDivision.ClientID %>");
            var ddlvend = document.getElementById("<%=ddlVendor.ClientID %>");
            var ddlmonth = document.getElementById("<%=ddlMonth.ClientID %>");
            var ddlyear = document.getElementById("<%=ddlYear.ClientID %>");
            var Billno = document.getElementById("<%=txtBillNo.ClientID%>").value.trim();
            var Workorderno = document.getElementById("<%=txtWorkOrderNo.ClientID%>").value.trim();

            if (ddldiv.value == "0") {
                alert("Please select division option.");
                document.getElementById("<%=ddlDivision.ClientID%>").focus();
                return false;
            }
            if (ddlvend.value == "0") {
                alert("Please select vendor option.");
                document.getElementById("<%=ddlVendor.ClientID%>").focus();
                return false;
            }
            if (ddlmonth.value == "0") {
                alert("Please select month option!");
                document.getElementById("<%=ddlMonth.ClientID%>").focus();
                return false;
            }
            if (ddlyear.value == "0") {
                alert('Please select year option.');
                document.getElementById("<%=ddlYear.ClientID%>").focus();
                return false;
            }
            if (Billno == "") {
                alert('Please enter bill number');
                document.getElementById("<%=txtBillNo.ClientID%>").focus();
                return false;
            }
            if (Workorderno == "") {
                alert('Please enter work order number...');
                document.getElementById("<%=txtWorkOrderNo.ClientID%>").focus();
                return false;
            }

        }
    </script>
    <script src="JS/12.js"></script>
    <script src="JS/13.js"></script>
    <link href="css/15.css" rel="stylesheet" />
    <script src="JS/16.js"></script>
    <script>
        function calculate(val) {
            let price = $(val).closest('tr').find('[id*=txtCableSize_2X10_OldCable]').val();
            let price1 = $(val).closest('tr').find('[id*=txtCableSize_2X10_CableReused]').val();
            let price2 = $(val).closest('tr').find('[id*=txtCableSize_2X10_OldCableReused]').val();
            let price3 = $(val).closest('tr').find('[id*=txtCableSize_2X10_NewCable]').val();
            document.getElementById('Label1').value = parseInt(price) + parseInt(price1) + parseInt(price2) + parseInt(price3);
            //$(val).closest('tr').find('[id*=Label1]').text(parseInt(price) + parseInt(price1) + parseInt(price2) + parseInt(price3));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="sm" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="panel-group">
                <div class="panel">
                    <div class="panel-heading">
                        <div class="panel-title" style="text-align: center;">
                            <b>Scrap Verification Report</b>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSearch" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlSearch" runat="server">
                                    <div class="row">
                                        <div class="col-md-3 form-group">
                                            <b>Division</b><span class="required">*</span>
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ddlDivision"
                                                ErrorMessage="Select Division" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <b>Vendor</b><span class="required">*</span>
                                            <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlVendor"
                                                ErrorMessage="Select Vendor" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Month</b><span class="required">*</span>
                                            <asp:DropDownList ID="ddlMonth" CssClass="form-control input-sm" runat="server">
                                                <asp:ListItem Text="Select" Value="0" />
                                                <asp:ListItem Text="January" Value="01" />
                                                <asp:ListItem Text="February" Value="02" />
                                                <asp:ListItem Text="March" Value="03" />
                                                <asp:ListItem Text="April" Value="04" />
                                                <asp:ListItem Text="May" Value="05" />
                                                <asp:ListItem Text="June" Value="06" />
                                                <asp:ListItem Text="July" Value="07" />
                                                <asp:ListItem Text="August" Value="08" />
                                                <asp:ListItem Text="September" Value="09" />
                                                <asp:ListItem Text="October" Value="10" />
                                                <asp:ListItem Text="November" Value="11" />
                                                <asp:ListItem Text="December" Value="12" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlMonth"
                                                ErrorMessage="Select Month" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Year</b><span class="required">*</span>
                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0" />
                                                <asp:ListItem Text="2021" Value="2021" />
                                                <asp:ListItem Text="2020" Value="2020" />

                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlYear"
                                                ErrorMessage="Select Year" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Bill No.</b><span class="required">*</span>
                                            <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control input-sm" placeholder="Bill No"
                                                MaxLength="48" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBillNo"
                                                ErrorMessage="Bill No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                    </div>
                                    <div class="row">

                                        <div class="col-md-3 form-group">
                                            <b>Work Order No.</b><span class="required">*</span>
                                            <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="form-control input-sm"
                                                placeholder="Work Order No" MaxLength="48" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtWorkOrderNo"
                                                ErrorMessage="Work Order No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-6 form-group" style="padding-top: 20px;">
                                            <asp:Button ID="btnCreateinvoice" runat="server" Text="Generate Bill" CssClass="btn btn-sm btn-info" OnClick="btnCreateinvoice_Click" OnClientClick="javascript: return fncheck()" />&nbsp;&nbsp;
                                            <asp:Button ID="btnGenerate" runat="server" Text="Download Bill" CssClass="btn btn-sm btn-primary" ValidationGroup="search" Visible="false" OnClick="btnGenerate_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div style="overflow-x: auto; width: 100%; height: 500px;">
                                            <asp:Repeater ID="rptrScrapVerification" runat="server" OnItemDataBound="rptrScrapVerification_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table id="tblScrapVerification" class="grid table-condensed mb0 cf demo" style="border-spacing: 0px !important;">
                                                        <thead class="bg-info text-white cf">
                                                            <tr>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">S.No
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">Job Description
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">In %
                                                                </th>
                                                                <th colspan="5" style="text-align: center; white-space: normal; background-color: #ea8d8d !important; font-size: larger;">Total Cosnumed Cable Size (In Mtr)
                                                                </th>
                                                                <th colspan="5" style="text-align: center; white-space: normal">Scarp Cable Size (In Mtr)
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">Remarks
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <th style="text-align: center; background-color: #ea8d8d !important">2x10
                                                                </th>
                                                                <th style="text-align: center; background-color: #ea8d8d !important">2x25
                                                                </th>
                                                                <th style="text-align: center; background-color: #ea8d8d !important">4x25
                                                                </th>
                                                                <th style="text-align: center; background-color: #ea8d8d !important">4x50
                                                                </th>
                                                                <th style="text-align: center; background-color: #ea8d8d !important">4x150
                                                                </th>
                                                                <th style="text-align: center;">2x10
                                                                </th>
                                                                <th style="text-align: center;">2x25
                                                                </th>
                                                                <th style="text-align: center;">4x25
                                                                </th>
                                                                <th style="text-align: center;">4x50
                                                                </th>
                                                                <th style="text-align: center;">4x150
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <%# Container.ItemIndex+1 %>
                                                        </td>
                                                        <td style="font-weight: bold">
                                                            <asp:Label ID="lblJobName" runat="server" Text='<%# Eval("JOB_NAME") %>' />
                                                            <asp:Label ID="lblScrapJobId" runat="server" Text='<%# Eval("SCRAP_JOB_ID") %>' Visible="false" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblPercValue" runat="server" Text='<%# Eval("VALUE") %>' />%
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblConsumedCable_2X10" runat="server" Text='<%# Eval("CONSUMED_CABLE_2X10") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblConsumedCable_2X25" runat="server" Text='<%# Eval("CONSUMED_CABLE_2X25") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblConsumedCable_4X25" runat="server" Text='<%# Eval("CONSUMED_CABLE_4X25") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblConsumedCable_4X50" runat="server" Text='<%# Eval("CONSUMED_CABLE_4X50") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblConsumedCable_4X150" runat="server" Text='<%# Eval("CONSUMED_CABLE_4X150") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblScrapCable_2X10" runat="server" Text='<%# Eval("SCRAP_CABLE_2X10") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblScrapCable_2X25" runat="server" Text='<%# Eval("SCRAP_CABLE_2X25") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblScrapCable_4X25" runat="server" Text='<%# Eval("SCRAP_CABLE_4X25") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblScrapCable_4X50" runat="server" Text='<%# Eval("SCRAP_CABLE_4X50") %>' />
                                                        </td>
                                                        <td class="text-right">
                                                            <asp:Label ID="lblScrapCable_4X150" runat="server" Text='<%# Eval("SCRAP_CABLE_4X150") %>' />
                                                        </td>
                                                        <td style="text-align: center;">
                                                            <asp:TextBox ID="txtScrapReportRemarks" runat="server" CssClass="scropReportRemarks"
                                                                Text='<%# Eval("REMARKS") %>' MaxLength="90" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                                                            <tfoot>
                                                                <tr style="background-color: #c0c8c8">
                                                                    <td colspan="2" style="text-align: center">
                                                                        <b>Total</b>
                                                                    </td>
                                                                    <td style="text-align: center"></td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ConsumedCable_2X10" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ConsumedCable_2X25" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ConsumedCable_4X25" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ConsumedCable_4X50" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ConsumedCable_4X150" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ScrapCable_2X10" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ScrapCable_2X25" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ScrapCable_4X25" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ScrapCable_4X50" runat="server" />
                                                                    </td>
                                                                    <td class="text-right">
                                                                        <asp:Label ID="lblTotal_ScrapCable_4X150" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: center;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="font-weight: bold">
                                                                        <asp:Label ID="lblFooterPrevBalOldCable" runat="server" Text="Previous Balance Old Cable" />
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X10_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X25_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X25_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X50_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X150_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X10_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);"  MaxLength="10" /><%-- OnTextChanged="txtCableSize_2X10_OldCable_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X25_OldCable" runat="server" Width="80px" Text='<%# Eval("SCRAP_CABLE_4X150") %>'
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" /><%-- OnTextChanged="txtCableSize_2X25_OldCable_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X25_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_4X25_OldCable_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X50_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_4X50_OldCable_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X150_OldCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_4X150_OldCable_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td style="text-align: center;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="font-weight: bold">
                                                                        <asp:Label ID="lblFooterCableReused" runat="server" Text="Cable Reused" />
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X10_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X25_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X25_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X50_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X150_CableReused" runat="server" Width="80px"
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X10_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);"  MaxLength="10" /><%-- OnTextChanged="txtCableSize_2X10_CableReused_TextChanged" AutoPostBack="true"/>--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X25_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_2X25_CableReused_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X25_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_4X25_CableReused_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X50_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_4X50_CableReused_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X150_CableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /> <%--OnTextChanged="txtCableSize_4X150_CableReused_TextChanged" AutoPostBack="true"/>--%>
                                                                    </td>
                                                                    <td style="text-align: center;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="font-weight: bold; white-space: normal">
                                                                        <asp:Label ID="lblFooterOldCableReused" runat="server" Text="Balance Old Cable to be Reused" />
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X10_OldCableReused" runat="server" Width="80px"
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X25_OldCableReused" runat="server" Width="80px"
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X25_OldCableReused" runat="server" Width="80px"
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X50_OldCableReused" runat="server" Width="80px"
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X150_OldCableReused" runat="server" Width="80px"
                                                                            onkeyup="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                            MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X10_OldCableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);"  MaxLength="10" /><%-- OnTextChanged="txtCableSize_2X10_OldCableReused_TextChanged" AutoPostBack="true"/>--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X25_OldCableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /><%-- OnTextChanged="txtCableSize_2X25_OldCableReused_TextChanged" AutoPostBack="true"/>--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X25_OldCableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /> <%--OnTextChanged="txtCableSize_4X25_OldCableReused_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X50_OldCableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /> <%--OnTextChanged="txtCableSize_4X50_OldCableReused_TextChanged" AutoPostBack="true" />--%>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X150_OldCableReused" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" /> <%--OnTextChanged="txtCableSize_4X150_OldCableReused_TextChanged" AutoPostBack="true"/>--%>
                                                                    </td>
                                                                    <td style="text-align: center;"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="font-weight: bold; white-space: normal">
                                                                        <asp:Label ID="lblFooterNewCable" runat="server" Text="New Cables Return To Store As Scrap" />
                                                                    </td>
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X10_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_2X25_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X25_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X50_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtConsumedCable_4X150_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X10_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);"  MaxLength="10" OnTextChanged="txtCableSize_2X10_NewCable_TextChanged" AutoPostBack="true"/>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_2X25_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" OnTextChanged="txtCableSize_2X25_NewCable_TextChanged" AutoPostBack="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X25_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" OnTextChanged="txtCableSize_4X25_NewCable_TextChanged" AutoPostBack="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X50_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" OnTextChanged="txtCableSize_4X50_NewCable_TextChanged" AutoPostBack="true" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCableSize_4X150_NewCable" runat="server" Width="80px" onkeyup="return isNumber(this,'Value');"
                                                                            onkeypress="return validateFloatKeyPress(this,event);" MaxLength="10" OnTextChanged="txtCableSize_4X150_NewCable_TextChanged" AutoPostBack="true"/>
                                                                    </td>
                                                                    <td style="text-align: center;"></td>
                                                                </tr>
                                                                <tr style="background-color: #cae8f3">
                                                                    <td colspan="8" style="text-align: center">
                                                                        <b>OVERALL Total</b>
                                                                    </td>
                                                              <%--      <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>
                                                                    <td></td>--%>
                                                                    <td style="text-align: right">
                                                                        <asp:Label ID="Label1" runat="server"/>
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:Label ID="Label2" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:Label ID="Label3" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:Label ID="Label4" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right">
                                                                        <asp:Label ID="Label5" runat="server" />
                                                                    </td>
                                                                    <td></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="8" style="text-align: right">
                                                                        <b>Required SCRAP IN KG</b>
                                                                    </td>
                                                                    <td style="text-align: right; background-color:yellow"><asp:Label ID="Label6" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right; background-color: yellow"><asp:Label ID="Label7" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right; background-color: yellow"><asp:Label ID="Label8" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right; background-color: yellow"><asp:Label ID="Label9" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right; background-color: yellow"><asp:Label ID="Label10" runat="server" />
                                                                    </td>
                                                                    <td style="text-align: right; background-color: yellow"><asp:Label ID="Label11" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="8" style="text-align: right"></td>
                                                                    <td colspan="2" style="text-align: center; background-color: #c0c8c8; font-weight: bold">FLEXIBLE WIRE
                                                                    </td>
                                                                    <td colspan="3" style="text-align: center; background-color: #c0c8c8; font-weight: bold">XLPE OFF SIZE
                                                                    </td>
                                                                    <td style="text-align: center; background-color: #c0c8c8; font-weight: bold">Total
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="8" style="text-align: right">
                                                                        <b>Required SCRAP IN KG</b>
                                                                    </td>
                                                                    <td colspan="2" style="text-align: right; background-color: yellow"> <asp:Label ID="Label12" runat="server" />
                                                                    </td>
<%--                                                                    <td style="text-align: right; background-color: yellow">0
                                                                    </td>--%>
                                                                    <td colspan="3" style="text-align: right; background-color: yellow"> <asp:Label ID="Label13" runat="server" />
                                                                    </td>
                                                                    <%--<td style="text-align: right; background-color: yellow">0
                                                                    </td>
                                                                    <td style="text-align: right; background-color: yellow">0
                                                                    </td>--%>
                                                                    <td style="text-align: right; background-color: yellow"> <asp:Label ID="Label14" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </tfoot>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" id="tabid" visible="false">
                                        <div class="col-md-12 form-group" style="padding-top: 20px; text-align: center;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-sm btn-info" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-sm btn-primary" ValidationGroup="search" OnClick="btnBack_Click" />
                                        </div>
                                    </div>
                                         <div class="row">
                                        <asp:GridView ID="GridView5" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                            Width="100%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="JOB_NAME" HeaderText="Job Description" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="VALUE" HeaderText="In %" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CONSUMED_CABLE_2X10" HeaderText="2X10" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CONSUMED_CABLE_2X25" HeaderText="2X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CONSUMED_CABLE_4X25" HeaderText="4X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CONSUMED_CABLE_4X50" HeaderText="4X50" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CONSUMED_CABLE_4X150" HeaderText="4X150" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="SCRAP_CABLE_2X10" HeaderText="2X10" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="SCRAP_CABLE_2X25" HeaderText="2X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="SCRAP_CABLE_4X25" HeaderText="4X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="SCRAP_CABLE_4X50" HeaderText="4X50" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="SCRAP_CABLE_4X150" HeaderText="4X150" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" ItemStyle-HorizontalAlign="Left" />
                                            </Columns>
                                            <HeaderStyle HorizontalAlign="Center" Height="35px" />
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                               <Triggers>
                                <asp:PostBackTrigger ControlID="btnGenerate" />
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
    <script src="content/js/aside-menu.js"></script>
    <script src="content/js/bootstrap.min.js"></script>
    <script src="content/js/jquery.min.js"></script>
    <script src="content/js/responsive-tables.js"></script>
    <script src="content/js/utils.js"></script>
    <script src="JS/Validation.js"></script>
</asp:Content>
