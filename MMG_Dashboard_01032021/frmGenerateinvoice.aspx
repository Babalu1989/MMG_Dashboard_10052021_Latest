<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmGenerateinvoice.aspx.cs" Inherits="MMG_Dashboard_01032021.frmGenerateinvoice" %>

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
    <link href="css/15.css" rel="stylesheet" />
 <%--   <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>--%>
    <script src="JS/12.js"></script>
    <script src="JS/13.js"></script>
    <script>
        function calculate(val) {
            let Quantity = $("#" + val.id).val();
            let price = $(val).closest('tr').find('[id*=lblUnitRate]').text();
            $(val).closest('tr').find('[id*=lblAmount]').text(Quantity * price);
        }
    </script>
    <script src="JS/16.js"></script>
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            $("[id*=txtQuantity]").val("0");
        });
        $("body").on("change keyup", "[id*=txtQuantity]", function () {
            var quantity = parseFloat($.trim($(this).val()));
            if (isNaN(quantity)) {
                quantity = 0;
            }
            $(this).val(quantity);
            var row = $(this).closest("tr");
            $("[id*=lblAmount]", row).html(parseFloat($(".lblUnitRate", row).html()) * parseFloat($(this).val()));
        });
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
                            <b>Generate Invoice</b>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upMsg" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlMsg" runat="server" CssClass="alert alert-info" Visible="false">
                                    <asp:Label ID="lblMsg" runat="server" />
                                    <asp:TextBox ID="txtMsg" runat="server" Width="1px" Height="1px" Style="border: none; background-color: transparent" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upSearch" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlSearch" runat="server">
                                    <div class="row">
                                        <div class="col-md-3 form-group">
                                            <b>Division</b><span class="required">*</span>
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlDivision"
                                                ErrorMessage="Select Division" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <b>Vendor</b><span class="required">*</span>
                                            <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlVendor"
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
                                            <asp:Button ID="btnCreateinvoice" runat="server" Text="Generate Invoice" CssClass="btn btn-sm btn-info" OnClick="btnCreateinvoice_Click" OnClientClick="javascript: return fncheck()" />&nbsp;&nbsp;
                                            <asp:Button ID="btnGenerate" runat="server" Text="Download Invoice" CssClass="btn btn-sm btn-primary" ValidationGroup="search" OnClick="btnGenerate_Click" Visible="false" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div style="overflow-x: auto; width: 100%; height: 500px;">
                                            <div class="col-md-12 form-group">
                                                <asp:GridView ID="GrdInvoice" runat="server" CssClass="grid table-condensed mb0 cf demo"
                                                    AutoGenerateColumns="false" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" OnRowDataBound="GrdInvoice_RowDataBound"
                                                    AllowPaging="false" HeaderStyle-CssClass="text-center">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Item Code" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ITEMCODE") %>' />
                                                                <asp:TextBox ID="txtItemcode" runat="server" Text='<%# Eval("ITEMCODE") %>' Visible="false"
                                                                    Width="80px" onchange="return isNumber(this,'Quantity');"
                                                                    onkeypress="return validateFloatKeyPress(this,event);" MaxLength="7"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="100px" ItemStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("DESCRIPTION") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="100px" ItemStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UNIT") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Unit Rate" HeaderStyle-Width="100px" ItemStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUnitRate" runat="server" Text='<%# Eval("UNITRATE") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="100px" ItemStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("QUANTITY") %>' />
                                                                <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("QUANTITY") %>' Visible="false"
                                                                    Width="80px" onkeyup="calculate(this)" onchange="return isNumber(this,'Quantity');"
                                                                    onkeypress="return validateFloatKeyPress(this,event);" MaxLength="8"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="100px" ItemStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("AMOUNT") %>' />
                                                                <%--                                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("AMOUNT") %>' Visible="false"
                                                                    Width="80px" onchange="return isNumber(this,'Quantity');"
                                                                    onkeypress="return validateFloatKeyPress(this,event);" MaxLength="7"></asp:TextBox>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-7 form-group" style="padding-top: 20px;">
                                            <div class="pull-right">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Save Bill" ValidationGroup="save" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" Visible="false" />&nbsp;&nbsp;
                                                <asp:Button ID="btnBack" runat="server" Text="Back" CausesValidation="false" CssClass="btn btn-sm btn-info" OnClick="btnBack_Click" />
                                            </div>
                                        </div>
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
