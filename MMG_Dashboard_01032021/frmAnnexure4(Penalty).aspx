<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmAnnexure4(Penalty).aspx.cs" Inherits="MMG_Dashboard_01032021.frmAnnexure4_Penalty_" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="sm" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="panel-group">
                <div class="panel">
                    <div class="panel-heading">
                        <div class="panel-title" style="text-align: center;">
                            <b>ANNEXURE-4<br />
                                Deductions/Retention Sheet</b>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSearch" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlSearch" runat="server">                               
                                    <div class="row">
                                        <div style="overflow-x: auto; width: 100%; height: 500px;">
                                            <asp:GridView ID="grdPenalty" runat="server" CssClass="grid table-condensed mb0 cf demo"
                                                AutoGenerateColumns="false" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true"
                                                AllowPaging="false" DataKeyNames="PENALTYID">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex +1  %>
                                                            <asp:Label ID="lblPenaltyId" runat="server" Text='<%# Eval("PENALTYID") %>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nature of Non compliance" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPenalty" runat="server" Text='<%# Eval("PENALTY") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Penalty (Rs.)*" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="litPenaltyInfo"   runat="server" Text='<%# Eval("PENALTY_INFO") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Recommended Penalty (In Rs.)*" HeaderStyle-Width="10px"
                                                        ItemStyle-CssClass="text-left">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPenaltyAmount" runat="server" Text='<%# Eval("Amount") %>'
                                                                MaxLength="8" onkeyup="return isNumber(this,'Amount');" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" id="tabid" >
                                        <div class="col-md-12 form-group" style="padding-top: 20px; text-align: center;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save Bill" CssClass="btn btn-sm btn-info" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="btn btn-sm btn-primary" ValidationGroup="search" OnClick="btnCancel_Click" />&nbsp;&nbsp;
                                            <asp:Button ID="btnGenerate" runat="server" Text="Download Bill" CssClass="btn btn-sm btn-info" ValidationGroup="search" Visible="false" OnClick="btnGenerate_Click" />
                                        </div>
                                    </div>
                                      <div class="row">
                                        <asp:GridView ID="GridView6" AllowSorting="true" CssClass="gvwWhite" runat="server"
                                            Width="100%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="PENALTYID" HeaderText="Sr.No." ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PENALTY" HeaderText="Nature of Non compliance" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PENALTY_INFO" HeaderText="Penalty (Rs.)*" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="AMOUNT" HeaderText="Recommended Penalty (In Rs.)*" ItemStyle-HorizontalAlign="Left" />
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
