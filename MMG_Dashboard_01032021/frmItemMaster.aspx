<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmItemMaster.aspx.cs" Inherits="MMG_Dashboard_01032021.frmItemMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidateAlpha(evt) {
            var keyCode = (evt.which) ? evt.which : evt.keyCode
            if ((keyCode < 65 || keyCode > 90) && (keyCode < 97 || keyCode > 123) && keyCode != 32)
                return false;
            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;
            return true;
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
                            <b>Item Master</b>
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
                                        <div class="col-md-2 form-group">
                                            <b>Item Code</b>
                                            <asp:TextBox ID="txtItemCodeSearch" runat="server" CssClass="form-control input-sm" placeholder="Item Code" MaxLength="7" onkeypress="return isNumberKey(event);" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Description</b>
                                            <asp:TextBox ID="txtItemDescriptionSearch" runat="server" CssClass="form-control input-sm" placeholder="Item Description" MaxLength="49" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Period From</b>
                                            <asp:TextBox ID="txtPeriodFromSearch" runat="server" CssClass="form-control input-sm" placeholder="Period From" autocomplete="off" />
                                            <ajax:CalendarExtender ID="Calendar1" runat="server" TargetControlID="txtPeriodFromSearch" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPeriodFromSearch" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="search" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Period To</b>
                                            <asp:TextBox ID="txtPeriodToSearch" runat="server" CssClass="form-control input-sm" placeholder="Period To" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtPeriodToSearch" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPeriodToSearch" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="search" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Status</b>
                                            <asp:DropDownList ID="ddlStatusSearch" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Active" Value="Y" Selected="True" />
                                                <asp:ListItem Text="In-Active" Value="N" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 form-group" style="padding-top: 20px;">
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-primary" ValidationGroup="search" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                                    <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-sm btn-info" OnClick="btnAddNew_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div style="overflow-x: auto; width: 100%; height: 500px;">
                                            <div class="col-md-12 form-group">
                                                <asp:GridView ID="grdSearch" runat="server" CssClass="grid table-condensed mb0 cf demo" AutoGenerateColumns="false"
                                                    EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" OnRowCommand="grdSearch_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex +1  %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Code" HeaderStyle-Width="120" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblJobCode" runat="server" Text='<%# Eval("ITEMCODE") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" HeaderStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="UNIT" HeaderText="Unit" HeaderStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="UNITRATE" HeaderText="Unit Rate" HeaderStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="PERIODFROM" HeaderText="Period From" HeaderStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="ISACTIVE" HeaderText="Status" HeaderStyle-Width="120" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="50" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="E" CommandArgument='<%#Eval("ITEMCODE")%>' Text="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlAddNew" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-md-2 form-group">
                                            <b>Item Code</b><span class="required">*</span>
                                            <asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control input-sm" placeholder="Item Code" MaxLength="7" onkeypress="return isNumberKey(event);" />
                                            <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txtItemCode" ErrorMessage="Job Code cannot be blank" Display="Dynamic" SetFocusOnError="true" ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-4 form-group">
                                            <b>Item Description</b><span class="required">*</span>
                                            <asp:TextBox ID="txtItemDescription" runat="server" CssClass="form-control input-sm" placeholder="Item Description" MaxLength="49" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtItemDescription" ErrorMessage="Job Description cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Unit</b><span class="required">*</span>
                                            <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control input-sm" placeholder="Unit" MaxLength="10" onKeyPress="return ValidateAlpha(event);" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUnit" ErrorMessage="Unit cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Unit Rate</b><span class="required">*</span>
                                            <asp:TextBox ID="txtUnitRate" runat="server" CssClass="form-control input-sm" placeholder="Unit Rate" onkeyup="return isNumber(this,'Unit Rate');" onkeypress="return validateFloatKeyPress(this,event);" MaxLength="6" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUnitRate" ErrorMessage="Unit Rate cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Period From</b><span class="required">*</span>
                                            <asp:TextBox ID="txtPeriodFrom" runat="server" CssClass="form-control input-sm" placeholder="Period From" autocomplete="off" />
                                            <asp:Label ID="lblPeriodFrom" runat="server" Visible="false" />
                                            <asp:Label ID="lblLastPeriodFrom" runat="server" Visible="false" />
                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPeriodFrom" Format="dd/MM/yyyy"></ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPeriodFrom" ErrorMessage="Perioid From cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPeriodFrom" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="save" SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="row" style="text-align: center;">
                                        <div class="col-md-12 form-group" style="padding-top: 20px;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="save" CssClass="btn btn-sm btn-primary" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-sm btn-info" OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
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
