<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmAnnexure2.aspx.cs" Inherits="MMG_Dashboard_01032021.frmAnnexure2" %>

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
    <script src="JS/12.js"></script>
    <script src="JS/13.js"></script>
    <script src="JS/16.js"></script>
    <script>
        function calculate(val) {
            let price = $(val).closest('tr').find('[id*=txtPrvMonthBalance]').val();
            let price1 = $(val).closest('tr').find('[id*=txtReceivedFromOtherDivision]').val();
            let price2 = $(val).closest('tr').find('[id*=txtRemovedFromSiteReused]').val();
            let price3 = $(val).closest('tr').find('[id*=txtReceivedFromStore]').val();
            $(val).closest('tr').find('[id*=lblTotal]').text(parseInt(price) + parseInt(price1) + parseInt(price2) + parseInt(price3));
            let price6 = $(val).closest('tr').find('[id*=lblQtyConsumption]').val();
            let price4 = $(val).closest('tr').find('[id*=txtReturnedToStore]').val();
            let price5 = $(val).closest('tr').find('[id*=txtTransferToOtherDivision]').val();
            $(val).closest('tr').find('[id*=lblBalance]').text((parseInt(price) + parseInt(price1) + parseInt(price2) + parseInt(price3)) - (parseInt(price4) + parseInt(price5) + parseInt(price6)));
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
                            <b>ANNEXURE-2<br />
                                (Material Reconciliation Statement)</b>
                        </div>
                    </div>
                    <div class="panel-body">
                       <%-- <asp:UpdatePanel ID="upMsg" runat="server">
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
                                            <asp:Repeater ID="rptrMaterialReconciliation1" runat="server">
                                                <HeaderTemplate>
                                                    <table class="grid table-condensed mb0 cf demo" style="border-spacing: 0px !important;">
                                                        <thead class="bg-info text-white cf">
                                                            <tr>
                                                                <th>S.No.
                                                                </th>
                                                                <th>Items
                                                                </th>
                                                                <th>Unit
                                                                </th>
                                                                <th>Prev. Month Balance
                                                                </th>
                                                                <th>Rec. From other Division
                                                                </th>
                                                                <th>Removed From Site
                                                                </th>
                                                                <th>Rec. From Store
                                                                </th>
                                                                <th>Total
                                                                </th>
                                                                <th>Quantity Consumed
                                                                </th>
                                                                <th>Returned to Store
                                                                </th>
                                                                <th>Transfer to other Div
                                                                </th>
                                                                <th>Balance
                                                                </th>
                                                                <th>Remarks
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%# Container.ItemIndex+1 %>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblItemId" runat="server" Text='<%# Eval("ITEMID") %>' Visible="false" />
                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ITEMNAME") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UNIT") %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPrvMonthBalance" runat="server" Width="100px" Text='<%# Eval("PREVIOUS_MONTH_BALANCE") %>'
                                                                onchange="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                MaxLength="10" onkeyup="calculate(this)" CssClass="aside-sm" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReceivedFromOtherDivision" runat="server" Width="100px" Text='<%# Eval("RECEIVED_FROM_DIVISION") %>'
                                                                onchange="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                MaxLength="10" onkeyup="calculate(this)" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRemovedFromSiteReused" runat="server" Width="100px" Text='<%# Eval("REMOVED_FROM_SITE") %>'
                                                                onchange="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                MaxLength="10" onkeyup="calculate(this)" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReceivedFromStore" runat="server" Width="100px" Text='<%# Eval("RECEIVED_FROM_STORE") %>'
                                                                onchange="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                MaxLength="10" onkeyup="calculate(this)" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TOTAL") %>' />
                                                            <%-- <asp:TextBox ID="lblTotal"  BorderColor="Transparent"  runat="server" Width="100px" Text='<%# Eval("TOTAL")--%>
                                                        </td>
                                                        </td>
                                                                <td>
                                                                    <%-- <asp:Label ID="lblQtyConsumption" runat="server" Text='<%# Eval("CONSUMPTION") %>' />--%>
                                                                    <asp:TextBox ID="lblQtyConsumption" runat="server"
                                                                        BorderColor="Transparent" Width="100px" ReadOnly="true" Text='<%# Eval("CONSUMPTION") %>' />
                                                                </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReturnedToStore" runat="server" Width="100px" Text='<%# Eval("RETURNED_TO_STORE") %>'
                                                                onchange="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                MaxLength="10" onkeyup="calculate(this)" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtTransferToOtherDivision" runat="server" Width="100px" Text='<%# Eval("TRANSFER_TO_DIVISION") %>'
                                                                onchange="return isNumber(this,'Value');" onkeypress="return validateFloatKeyPress(this,event);"
                                                                MaxLength="10" onkeyup="calculate(this)" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblBalance" runat="server" Text='<%# Eval("BALANCE") %>' />
                                                        </td>
                                                        </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="200px" Text='<%# Eval("REMARKS") %>'
                                                                        MaxLength="90" />
                                                                </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" id="tabid" visible="false">
                                        <div class="col-md-12 form-group" style="padding-top: 20px; text-align: center;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save Bill" CssClass="btn btn-sm btn-info" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="btn btn-sm btn-primary" ValidationGroup="search" OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <asp:GridView ID="GridView1" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                            Width="100%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="ITEMID" HeaderText="SI.No." ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ITEMNAME" HeaderText="Items" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="UNIT" HeaderText="Unit" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PREVIOUS_MONTH_BALANCE" HeaderText="Previous Month Balance" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="RECEIVED_FROM_DIVISION" HeaderText="Received From Other Division"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="REMOVED_FROM_SITE" HeaderText="Removed From Site (Reused)"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="RECEIVED_FROM_STORE" HeaderText="Recieved From Store"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TOTAL" HeaderText="Total" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CONSUMPTION" HeaderText="Consumption" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="RETURNED_TO_STORE" HeaderText="Returned To Store" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TRANSFER_TO_DIVISION" HeaderText="Transfer To Other Division"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="BALANCE" HeaderText="Balance" ItemStyle-HorizontalAlign="Left" />
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

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMode" runat="server" Visible="false" />
            <asp:Label ID="lblPK" runat="server" Visible="false" />
            <asp:Label ID="lblUserId" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

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
