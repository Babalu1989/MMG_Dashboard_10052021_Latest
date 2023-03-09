<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmAnnexure1.aspx.cs" Inherits="MMG_Dashboard_01032021.frmAnnexure1" %>

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
        function validateNumber(event) {
            var key = window.event ? event.keyCode : event.which;
            if (event.keyCode === 8 || event.keyCode === 46) {
                return true;
            } else if (key < 48 || key > 57) {
                alert('Please enter only numeric value 0 to 9');
                return false;
            } else {
                return true;
            }
        };
    </script>
<%--    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>--%>
    <script src="JS/16.js"></script>
    <script src="JS/13.js"></script>
    <script src="JS/12.js"></script>
    <link href="css/15.css" rel="stylesheet" />
    <script>
        function calculate(val) {
            let price = $(val).closest('tr').find('[id*=txtPrvMonthBalance]').val();
            let price1 = $(val).closest('tr').find('[id*=txtReceivedFromOtherDivision]').val();
            let price2 = $(val).closest('tr').find('[id*=txtRemovedFromSiteReused]').val();
            let price3 = $(val).closest('tr').find('[id*=txtReceivedFromStore]').val();
            $(val).closest('tr').find('[id*=lblTotal]').text(parseInt(price) + parseInt(price1) + parseInt(price2) + parseInt(price3));
        }

        function calculate1(val) {
            let price = $(val).closest('tr').find('[id*=lblTotal]').val();
            let price1 = $(val).closest('tr').find('[id*=lblQtyConsumption]').val();
            // let price2 = $(val).closest('tr').find('[id*=txtReturnedToStore]').val();
            //let price3 = $(val).closest('tr').find('[id*=txtTransferToOtherDivision]').val();
            $(val).closest('tr').find('[id*=lblBalance]').text(parseInt(price) - parseInt(price1));
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
                            <b>ANNEXURE-1<br />
                                (MMG Work Summary)</b>
                        </div>
                    </div>
                    <div class="panel-body">
<%--                        <asp:UpdatePanel ID="upMsg" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlMsg" runat="server" CssClass="alert alert-info" Visible="false">
                                    <asp:Label ID="lblMsg" runat="server" />
                                    <asp:TextBox ID="txtMsg" runat="server" Width="1px" Height="1px" Style="border: none; background-color: transparent" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
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
                                            <asp:Repeater ID="rptrMMGWorkSummty" runat="server" OnItemDataBound="rptrMMGWorkSummty_ItemDataBound">
                                                <HeaderTemplate>
                                                    <table id="tblMMGWorkSummry" class="grid table-condensed mb0 cf demo" style="border-spacing: 0px !important;">
                                                        <thead class="bg-info text-white cf">
                                                            <tr>
                                                                <th colspan="7" style="text-align: center; height: 30px;">MMG Work Description
                                                                </th>
                                                                <th colspan="5" style="text-align: center">Cable Consumption Summary
                                                                </th>
                                                                <th colspan="5" style="text-align: center">Scrap Summary
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">Sr.No.
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">Order Type
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">Activity Type
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">MAT Description
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">Old Meter
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">1PH
                                                                </th>
                                                                <th rowspan="2" style="text-align: center; background-color: #99b8de !important">3PH
                                                                </th>
                                                                <th colspan="5" style="text-align: center; background-color: #99b8de !important">Total Cable Insatlled (In Mtr)
                                                                </th>
                                                                <th colspan="5" style="text-align: center; background-color: #99b8de !important">Total Cable Removed (In Mtr)
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <th style="text-align: center; background-color: #99b8de !important">2x10
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">2x25
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">4x25
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">4x50
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">4x150
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">2x10
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">2x25
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">4x25
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">4x50
                                                                </th>
                                                                <th style="text-align: center; background-color: #99b8de !important">4x150
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("ORDERTYPE") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblActivityType" runat="server" Text='<%# Eval("ACTIVITYTYPE") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMatDesc" runat="server" Text='<%# Eval("MAT_DESC") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblOldMeter" runat="server" Text='<%# Eval("ISOLDMETER") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblMeter_1PH" runat="server" Text='<%# Eval("METERTYPE_1PH") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblMeter_3PH" runat="server" Text='<%# Eval("METERTYPE_3PH") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCableIns_2x10" runat="server" Text='<%# Eval("CABLE_INS_2X10") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCableIns_2x25" runat="server" Text='<%# Eval("CABLE_INS_2X25") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCableIns_4x25" runat="server" Text='<%# Eval("CABLE_INS_4X25") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCableIns_4x50" runat="server" Text='<%# Eval("CABLE_INS_4X50") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCableIns_4x150" runat="server" Text='<%# Eval("CABLE_INS_4X150") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCable_Remove_2x10" runat="server" Text='<%# Eval("CABLE_REMOVED_2X10") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCable_Remove_2x25" runat="server" Text='<%# Eval("CABLE_REMOVED_2X25") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCable_Remove_4x25" runat="server" Text='<%# Eval("CABLE_REMOVED_4X25") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCable_Remove_4x50" runat="server" Text='<%# Eval("CABLE_REMOVED_4X50") %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblCable_Remove_4x150" runat="server" Text='<%# Eval("CABLE_REMOVED_4X150") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <tr style="background-color: #8cf191">
                                                        <td colspan="5" style="text-align: right">
                                                            <b>Total MMG Jobs</b>
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal1" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal2" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal3" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal4" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal5" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal6" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal7" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal8" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal9" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal10" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal11" runat="server" />
                                                        </td>
                                                        <td style="text-align: center">
                                                            <asp:Literal ID="Literal12" runat="server" />
                                                        </td>
                                                    </tr>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <asp:GridView ID="GridView3" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                            Width="100%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="ORDERTYPE" HeaderText="Order Type" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ACTIVITYTYPE" HeaderText="Activity Type" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="MAT_DESC" HeaderText="MAT Description" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ISOLDMETER" HeaderText="Old Meter" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="METERTYPE_1PH" HeaderText="1Ph" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="METERTYPE_3PH" HeaderText="3Ph" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_INS_2X10" HeaderText="2X10" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_INS_2X25" HeaderText="2X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_INS_4X25" HeaderText="4X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_INS_4X50" HeaderText="4X50" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_INS_4X150" HeaderText="4X150" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_REMOVED_2X10" HeaderText="2X10" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_REMOVED_2X25" HeaderText="2X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_REMOVED_4X25" HeaderText="4X25" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_REMOVED_4X50" HeaderText="4X50" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CABLE_REMOVED_4X150" HeaderText="4X150" ItemStyle-HorizontalAlign="Left" />
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
