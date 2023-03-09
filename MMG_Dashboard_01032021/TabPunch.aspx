<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TabPunch.aspx.cs" Inherits="MMG_Dashboard_01032021.TabPunch" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" lang="en">
    function isNumber(event) {
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
                            <b>Tab Punching</b>
                        </div>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upSearch" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlSearch" runat="server">
                                    <div class="row" style="text-align: center;">
                                        <div class="col-md-2 form-group">
                                            <b>Division</b>
                                            <asp:DropDownList ID="ddlDiv" runat="server" CssClass="form-control input-sm" OnSelectedIndexChanged="ddlDiv_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Vendor Code</b>
                                            <asp:DropDownList ID="ddlVend" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Order No</b>
                                            <asp:TextBox ID="txtOrderNoSearch" runat="server" CssClass="form-control input-sm"
                                                placeholder="Order No" MaxLength="12" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>CA Number</b>
                                            <asp:TextBox ID="txtCa" runat="server" CssClass="form-control input-sm"
                                                placeholder="CA No" MaxLength="12" />
                                        </div>

                                        <div class="col-md-2 form-group">
                                            <b>Month</b>
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="ddlMonth"
                                                ErrorMessage="Select Month" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Year</b>
                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="0" />
                                                <asp:ListItem Text="2021" Value="2021" />
                                                <asp:ListItem Text="2020" Value="2020" />

                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="ddlYear"
                                                ErrorMessage="Select Year" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2 form-group" style="text-align: center;">
                                            <b>Activity From Date</b>
                                            <asp:TextBox ID="txtActivityFromDate" runat="server" CssClass="form-control input-sm"
                                                placeholder="Activity From Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtActivityFromDate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtActivityFromDate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="search"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <b>Activity To Date</b>
                                            <asp:TextBox ID="txtActivityToDate" runat="server" CssClass="form-control input-sm"
                                                placeholder="Activity To Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtActivityToDate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtActivityToDate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="search"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-2 form-group" style="padding-top: 20px; text-align: center;">
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-primary"
                                                ValidationGroup="search" OnClick="btnSearch_Click" />&nbsp;&nbsp;

                                            <asp:Button ID="btnrefresh" runat="server" Text="Refresh" CssClass="btn btn-sm btn-info"
                                                ValidationGroup="Refresh" OnClick="btnrefresh_Click" />
                                        </div>
                                    </div>
                                    <div class="row" id="grdid" runat="server">
                                        <div style="overflow-x: auto; width: 100%; height: 400px;">
                                            <div class="col-md-12 form-group">
                                                <asp:GridView ID="grdSearch" runat="server" CssClass="grid table-condensed mb0 cf demo"
                                                    AutoGenerateColumns="false" EmptyDataText="No Record Found" ShowHeaderWhenEmpty="true" OnRowCommand="grdSearch_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ORDER NO" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("ORDERID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CA" HeaderText="CA" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="DIVISION" HeaderText="Division" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="VENDOR_CODE" HeaderText="VENDOR CODE" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="ORDER_TYPE" HeaderText="ORDER TYPE" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="PM_ACTIVITY" HeaderText="ACTIVITY TYPE" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="ACTIVITY_DATE" HeaderText="ACTIVITY DATE" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="MONTH" HeaderText="Month" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="YEAR" HeaderText="Year" HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="CREATEDON" HeaderText="APPROVED DATE" HeaderStyle-Width="120px" />
                                                        <asp:TemplateField HeaderText="APPROVAL STATUS" HeaderStyle-Width="120px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# Eval("Status") %>' />
                                                                <a id="lnkRejectionRemarks" runat="server" data-toggle="modal" data-target="#myModal_Remarks"
                                                                    onclick="return ShowRemarks(this);"><i class="fa fa-comments fa-lg green" aria-hidden="true"></i></a>
                                                                <asp:HiddenField ID="hdnRejectionRemarks" runat="server" Value='<%# Eval("APPROVER_REMARKS") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EDIT" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="E" CommandArgument='<%#Eval("ORDERID")%>' Text="Edit" />
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
                                        <div class="col-md-3 form-group">
                                            Company:
                                            <asp:DropDownList ID="ddlcompany" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Selected="True" Text="BRPL" Value="BRPL"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Division:
                                            <asp:DropDownList ID="ddlDivision" runat="server" CssClass="form-control input-sm"
                                                OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Vendor Code:
                                             <asp:DropDownList ID="ddlvendor" runat="server" CssClass="form-control input-sm">
                                             </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Order No:
                                            <asp:TextBox ID="txtorderno" runat="server" CssClass="form-control input-sm" placeholder="Order No"
                                                MaxLength="12" onkeypress="return validateFloatKeyPress(this,event);"/>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtorderno"
                                                ErrorMessage="Customer Name cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3 form-group">
                                            Activity Date<span class="required">*</span>
                                            <asp:TextBox ID="txtActivityDate" runat="server" CssClass="form-control input-sm"
                                                placeholder="Activity Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtActivityDate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtActivityDate"
                                                ErrorMessage="Activity Date cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtActivityDate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="save"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Order Type:<span class="required">*</span>
                                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control input-sm"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged">
                                                <asp:ListItem Text="Select" Value="" />
                                                <asp:ListItem Text="ZDIN" Value="ZDIN" />
                                                <asp:ListItem Text="ZDRP" Value="ZDRP" />
                                                <asp:ListItem Text="ZDRM" Value="ZDRM" />
                                                <asp:ListItem Text="ZMSO" Value="ZMSO" />
                                                <asp:ListItem Text="ZMSC" Value="ZMSC" />
                                                <asp:ListItem Text="ZDIV" Value="ZDIV" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="ddlOrderType"
                                                ErrorMessage="Select Order Type" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Activity Type<span class="required">*</span>
                                            <asp:DropDownList ID="ddlActivityType" runat="server" CssClass="form-control input-sm"
                                                AppendDataBoundItems="true">
                                                <asp:ListItem Value="">Select</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlActivityType"
                                                ErrorMessage="Select Activity Type" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Account Class<span class="required">*</span>
                                            <asp:DropDownList ID="ddlAccountclass" runat="server" CssClass="form-control input-sm"
                                                AutoPostBack="true">
                                                <asp:ListItem Text="Select" Value="" />
                                                <asp:ListItem Text="GCC" Value="GCC" />
                                                <asp:ListItem Text="KCC" Value="KCC" />
                                                <asp:ListItem Text="MLCC" Value="MLCC" />
                                                <asp:ListItem Text="SLCC" Value="SLCC" />
                                                <asp:ListItem Text="Other" Value="Other" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlAccountclass"
                                                ErrorMessage="Select Order Type" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3 form-group">
                                            Planner Group<span class="required">*</span>
                                            <asp:DropDownList ID="ddlPlannergroup" runat="server" CssClass="form-control input-sm">
                                                <asp:ListItem Text="Select" Value="" />
                                                <asp:ListItem Text="AMP" Value="GCC" />
                                                <asp:ListItem Text="ANL" Value="KCC" />
                                                <asp:ListItem Text="BMG" Value="MLCC" />
                                                <asp:ListItem Text="CAC" Value="SLCC" />
                                                <asp:ListItem Text="CHK" Value="Other" />
                                                <asp:ListItem Text="CHD" Value="MLCC" />
                                                <asp:ListItem Text="ENF" Value="GCC" />
                                                <asp:ListItem Text="GCC" Value="KCC" />
                                                <asp:ListItem Text="KBL" Value="MLCC" />
                                                <asp:ListItem Text="MMG" Value="SLCC" />
                                                <asp:ListItem Text="SRV" Value="Other" />
                                                <asp:ListItem Text="REC" Value="SLCC" />
                                                <asp:ListItem Text="O&M" Value="Other" />
                                                <asp:ListItem Text="PLANNER_GROUP" Value="Other" />
                                                <asp:ListItem Text="Other" Value="Other" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlPlannergroup"
                                                ErrorMessage="Select Order Type" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save"
                                                ForeColor="Red" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Basic Start Date<span class="required">*</span>
                                            <asp:TextBox ID="txtBasicstartdate" runat="server" CssClass="form-control input-sm"
                                                placeholder="Basic Start Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtBasicstartdate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtbasicstartdate"
                                                ErrorMessage="Activity Date cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtbasicstartdate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="save"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Basic Finish Date<span class="required">*</span>
                                            <asp:TextBox ID="txtBasicfinishdate" runat="server" CssClass="form-control input-sm"
                                                placeholder="Basic Finish Date" autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtBasicfinishdate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtbasicfinishdate"
                                                ErrorMessage="Activity Date cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtbasicfinishdate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="save"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Customer Name<span class="required">*</span>
                                            <asp:TextBox ID="txtCustName" runat="server" CssClass="form-control input-sm" placeholder=" Costomer Name"
                                                MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtCustName"
                                                ErrorMessage="Customer Name cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3 form-group">
                                            Activity Reason<span class="required">*</span>
                                            <asp:TextBox ID="txtActivityreason" runat="server" CssClass="form-control input-sm" placeholder="Activity Reason"
                                                autocomplete="off" MaxLength="100" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Posting Date<span class="required">*</span>
                                            <asp:TextBox ID="txtActidate" runat="server" CssClass="form-control input-sm" placeholder="Activity Date"
                                                autocomplete="off" />
                                            <ajax:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtActidate"
                                                Format="dd/MM/yyyy">
                                            </ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtActidate"
                                                ErrorMessage="Activity Date cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                ValidationGroup="save" ForeColor="Red" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtActidate"
                                                ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$"
                                                ErrorMessage="Invalid Date format. Valid Date Format dd/MM/yyyy" ValidationGroup="save"
                                                SetFocusOnError="true" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            Mobile No.<span class="required">*</span>
                                            <asp:TextBox ID="txtMobileno" runat="server" CssClass="form-control input-sm" placeholder="Mobile No."
                                                MaxLength="10" onkeypress="return validateFloatKeyPress(this,event);"/>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="txtMobileno"
                                                Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="save" ForeColor="Red" ErrorMessage="Please Enter Only Numbers" ValidationExpression="^\d+$" />
                                        </div>
                                        <div class="col-md-3 form-group">
                                            BP No.<span class="required">*</span>
                                            <asp:TextBox ID="txtBpno" runat="server" CssClass="form-control input-sm" placeholder="BP No."
                                                MaxLength="10" onkeypress="return validateFloatKeyPress(this,event);"/>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator207" runat="server" ControlToValidate="txtBpno"
                                                ErrorMessage="Customer Name cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="save" ForeColor="Red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6 form-group">
                                            <div class="row">
                                                <div class="col-md-6 form-group">
                                                    CA No.<span class="required">*</span>
                                                    <asp:TextBox ID="txtCANo" runat="server" CssClass="form-control input-sm" placeholder="CA No"
                                                        MaxLength="12" onkeypress="return validateFloatKeyPress(this,event);"/>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtCANo"
                                                        ErrorMessage="CA No cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                        ValidationGroup="save" ForeColor="Red" onkeypress="return isNumber(this,event);"/>
                                                </div>
                                                <div class="col-md-6 form-group">
                                                    Meter No<span class="required">*</span>
                                                    <asp:TextBox ID="txtMeterNo" runat="server" CssClass="form-control input-sm" placeholder="Meter No"
                                                        MaxLength="18" onkeypress="return validateFloatKeyPress(this,event);"/>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMeterNo"
                                                        ErrorMessage="Meter No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                        ValidationGroup="save" ForeColor="Red" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 form-group">
                                                    Meter Phase<span class="required">*</span>
                                                    <asp:DropDownList ID="ddlMeterPhase" runat="server" CssClass="form-control input-sm"
                                                        placeholder="Meter Installation Type">
                                                        <asp:ListItem Value="">Select</asp:ListItem>
                                                        <asp:ListItem Value="1 PHASE">1 Phase</asp:ListItem>
                                                        <asp:ListItem Value="2 PHASE">2 Phase</asp:ListItem>
                                                        <asp:ListItem Value="3 PHASE">3 Phase</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlMeterPhase"
                                                        ErrorMessage="Meter Phase cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                        ValidationGroup="save" ForeColor="Red" />
                                                </div>
                                                <div class="col-md-6 form-group">
                                                    Box Type<span class="required">*</span>
                                                    <asp:DropDownList ID="ddlBoxType" runat="server" CssClass="form-control input-sm">
                                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                                        <asp:ListItem Value="SP">SP</asp:ListItem>
                                                        <asp:ListItem Value="TP">TP</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="ddlBoxType"
                                                        ErrorMessage=" Box Type cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                        ValidationGroup="save" ForeColor="Red" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6 form-group">
                                                    Box No<span class="required">*</span>
                                                    <asp:TextBox ID="txtBoxNo" runat="server" CssClass="form-control input-sm" placeholder="Box No"
                                                        MaxLength="40" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtBoxNo"
                                                        ErrorMessage="Box No cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                        ValidationGroup="save" ForeColor="Red" />
                                                </div>
                                                <div class="col-md-6 form-group">
                                                    Address<span class="required">*</span>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control input-sm" placeholder="Address"
                                                        TextMode="MultiLine" MaxLength="499" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtAddress"
                                                        ErrorMessage="Address cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                        ValidationGroup="save" ForeColor="Red" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 form-group">
                                            <div class="row">
                                                <div class="col-md-12 form-group">
                                                    <div style="border: solid 1px #ccc; padding: 5px;">
                                                        <div class="row">
                                                            <div class="col-md-6 form-group">
                                                                Terminal Seal 1
                                                                <asp:TextBox ID="txtTerminalSeal1" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Terminal Seal1" MaxLength="49" />
                                                                <asp:RequiredFieldValidator ID="rfvTerminalSeal1" runat="server" ControlToValidate="txtTerminalSeal1" ErrorMessage="Terminal Seal 1 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                                            </div>
                                                            <div class="col-md-6 form-group">
                                                                Terminal Seal 2
                                                                <asp:TextBox ID="txtTerminalSeal2" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Terminal Seal2" MaxLength="49" />
                                                                <asp:RequiredFieldValidator ID="rfvTerminalSeal2" runat="server" ControlToValidate="txtTerminalSeal2" ErrorMessage="Terminal Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6 form-group">
                                                                Meter Box Seal 1
                                                                <asp:TextBox ID="txtMtrBoxSeal1" runat="server" CssClass="form-control input-sm" placeholder="Meter Box Seal 1"
                                                                    MaxLength="49" />
                                                                <asp:RequiredFieldValidator ID="rfvBoxSeal1" runat="server" ControlToValidate="txtMtrBoxSeal1" ErrorMessage="Box Seal 1 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                                            </div>
                                                            <div class="col-md-6 form-group">
                                                                Meter Box Seal 2
                                                                <asp:TextBox ID="txtMtrBoxSeal2" runat="server" CssClass="form-control input-sm" placeholder="Meter Box Seal 2"
                                                                    MaxLength="49" />
                                                                <asp:RequiredFieldValidator ID="rfvBoxSeal2" runat="server" ControlToValidate="txtMtrBoxSeal2" ErrorMessage="Box Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6 form-group">
                                                                Bus Bar Seal 1
                                                                <asp:TextBox ID="txtBusBarSeal1" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Bus Bar Seal 1" MaxLength="49" />
                                                                <asp:RequiredFieldValidator ID="rfvBusBarSeal1" runat="server" ControlToValidate="txtBusBarSeal1" ErrorMessage="Bus Bar Seal 1 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                                            </div>
                                                            <div class="col-md-6 form-group">
                                                                Bus Bar Seal 2
                                                                <asp:TextBox ID="txtBusBarSeal2" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Bus Bar Seal 2" MaxLength="49" />
                                                                <asp:RequiredFieldValidator ID="rfvBusBarSeal2" runat="server" ControlToValidate="txtBusBarSeal2" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 form-group">
                                            <b>Bus Bar/Bus Bar Cable</b>
                                            <div style="border: solid 1px #ccc; padding: 5px;">
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Have Installed Bus-Bar?<%--<asp:Label ID="Label69" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:RadioButtonList ID="rdoIsInstalledBusBar" runat="server" Width="120px" RepeatDirection="Horizontal"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Value="Yes" Selected="True">&nbsp;Yes</asp:ListItem>
                                                            <asp:ListItem Value="No">&nbsp;No</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    <%--    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rdoIsInstalledBusBar"
                                                            ErrorMessage="Please select any value" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus-bar Used
                                                                <%--<asp:Label ID="lbl" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:RadioButtonList ID="rdoBusBarUse" runat="server" RepeatDirection="Horizontal"
                                                            Width="120px">
                                                            <asp:ListItem Value="Old">&nbsp;Old</asp:ListItem>
                                                            <asp:ListItem Value="New">&nbsp;New</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="rdoBusBarUse"
                                                            ErrorMessage="Please select Bus-bar Used Type" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus Bar Remove From Site<%--<asp:Label ID="Label3" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:RadioButtonList ID="rdoBusBarRemoveFromSite" runat="server" RepeatDirection="Horizontal"
                                                            Width="120px">
                                                            <asp:ListItem Value="Yes">&nbsp;Yes</asp:ListItem>
                                                            <asp:ListItem Value="No">&nbsp;No</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator58" runat="server" ControlToValidate="rdoBusBarRemoveFromSite"
                                                            ErrorMessage="Please select any value" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus-bar Type
                                                                <%--<asp:Label ID="Label1" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:DropDownList ID="ddlBusBarType" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Value="">Select</asp:ListItem>
                                                            <asp:ListItem Value="1PH2W">1PH2W</asp:ListItem>
                                                            <asp:ListItem Value="1PH4W">1PH4W</asp:ListItem>
                                                            <asp:ListItem Value="1PH8W">1PH8W</asp:ListItem>
                                                            <asp:ListItem Value="3PH8W">3PH8W</asp:ListItem>
                                                            <asp:ListItem Value="3PH2W">3PH2W</asp:ListItem>
                                                            <asp:ListItem Value="3PH4W">3PH4W</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" InitialValue=""
                                                            ControlToValidate="ddlBusBarType" ErrorMessage="Bus-bar Size cannot be blank"
                                                            SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Bus-Bar No.<%--<asp:Label ID="Label2" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtBusBarNo" runat="server" CssClass="form-control input-sm" placeholder="Bus-Bar No"
                                                            MaxLength="49" />
                                                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBusBarNo"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus-Bar Drum No.<%--<asp:Label ID="Label38" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtBusbardrumno" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Bus-Bar Drum No" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtBusbardrumno"
                                                            ErrorMessage="Bus-Bar Drum No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus-bar Cable Size<%--<asp:Label ID="Label5" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:DropDownList ID="ddlBusBarCableSize" runat="server" CssClass="form-control input-sm">
                                                            <asp:ListItem Value="">Select</asp:ListItem>
                                                            <asp:ListItem Value="Cable Not Installed">Cable Not Installed</asp:ListItem>
                                                            <asp:ListItem Value="2*10">2*10</asp:ListItem>
                                                            <asp:ListItem Value="2*25">2*25</asp:ListItem>
                                                            <asp:ListItem Value="4*25">2x10</asp:ListItem>
                                                            <asp:ListItem Value="4*50">4*50</asp:ListItem>
                                                            <asp:ListItem Value="4*150">4*150</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ID="rfvBBCableSize" runat="server" ControlToValidate="ddlBusBarCableSize"
                                                            ErrorMessage="Bus-bar Cable Size cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Removed Bus Bar Cable Length
                                                                <asp:TextBox ID="txtRemovalbusbarcablelength" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removed Bus Bar Cable Length" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator73" runat="server" ControlToValidate="txtRemovalbusbarcablelength" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Bus-bar Cable Length<%--<asp:Label ID="Label6" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtBusBarCableLength" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Bus-bar Cable Length" MaxLength="49" />
                                               <%--         <asp:RequiredFieldValidator ID="rfvBBCableLength" runat="server" ControlToValidate="txtBusBarCableLength"
                                                            ErrorMessage="Bus-bar Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus-Bar Running Length From<%--<asp:Label ID="Label106" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtBusBarRunninglengthFrom" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Bus-Bar Running Length From" MaxLength="49" />
                                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator104" runat="server" ControlToValidate="txtBusBarRunninglengthFrom"
                                                            ErrorMessage="Bus-bar Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus-Bar Running Length To<%--<asp:Label ID="Label107" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtBusBarRunninglengthTo" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Bus-Bar Running Length To" MaxLength="49" />
                                                    <%--    <asp:RequiredFieldValidator ID="RequiredFieldValidator303" runat="server" ControlToValidate="txtBusBarRunninglengthTo"
                                                            ErrorMessage="Bus-bar Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Removed Bus Bar Cable Size
                                                                <asp:TextBox ID="txtRemovalBusbarcablesize" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removed Bus Bar Cable Size" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator64" runat="server" ControlToValidate="txtRemovalBusbarcablesize" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Removal Bus Bar Seal 1
                                                                <asp:TextBox ID="txtRemovalBusBarSeal1" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removal Bus Bar Seal 1" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator67" runat="server" ControlToValidate="txtRemovalBusBarSeal1" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Removal Bus Bar Seal 2
                                                                <asp:TextBox ID="txtRemovalBusBarSeal2" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removal Bus Bar Seal 2" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator78" runat="server" ControlToValidate="txtRemovalBusBarSeal2" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Bus Bar Cable Not Installed Reason
                                                                <asp:TextBox ID="txtBuabarcablenotinstalledreason" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Bus Bar Cable Not Installed Reason" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator60" runat="server" ControlToValidate="txtBuabarcablenotinstalledreason" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                            <div class="row">
                                                <div class="col-md-12 form-group">
                                                    <b>Cable</b>
                                                    <div style="border: solid 1px #ccc; padding: 5px;">
                                                        <div class="row">
                                                            <div class="col-md-3 form-group">
                                                                Have You Installed Cable?<%--<asp:Label ID="Label7" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdoIsInstalledCable" runat="server" RepeatDirection="Horizontal"
                                                                    Width="120px" AutoPostBack="true">
                                                                    <asp:ListItem Value="Y" Selected="True">&nbsp;Yes</asp:ListItem>
                                                                    <asp:ListItem Value="N">&nbsp;No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                               <%-- <asp:RequiredFieldValidator ID="rfvIsInstalledCable" runat="server" ControlToValidate="rdoIsInstalledCable"
                                                                    ErrorMessage="Please select any value" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Installation Cable Used Type
                                                                        <%--<asp:Label ID="Label8" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdoInstalCableUsedType" runat="server" RepeatDirection="Horizontal"
                                                                    Width="120px">
                                                                    <asp:ListItem Value="Old">&nbsp;Old</asp:ListItem>
                                                                    <asp:ListItem Value="New">&nbsp;New</asp:ListItem>
                                                                </asp:RadioButtonList>
<%--                                                                <asp:RequiredFieldValidator ID="rfvInstalCableUsedType" runat="server" ControlToValidate="rdoInstalCableUsedType"
                                                                    ErrorMessage="Please select any value" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>

                                                            <div class="col-md-3 form-group">
                                                                Cable Installation Type<%--<asp:Label ID="Label12" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdoCableInstallationType" runat="server" RepeatDirection="Horizontal"
                                                                    Width="250px">
                                                                    <asp:ListItem Value="Loop Connection">&nbsp;Loop Connection</asp:ListItem>
                                                                    <asp:ListItem Value="OH">&nbsp;OH</asp:ListItem>
                                                                    <asp:ListItem Value="UG">&nbsp;UG</asp:ListItem>
                                                                </asp:RadioButtonList>
<%--                                                                <asp:RequiredFieldValidator ID="rfvCableInstallationType" runat="server" ControlToValidate="rdoCableInstallationType"
                                                                    ErrorMessage="Installation type cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Cable Required<%--<asp:Label ID="Label36" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdbCablerequired" runat="server" RepeatDirection="Horizontal"
                                                                    Width="250px">
                                                                    <asp:ListItem Value="Y">&nbsp;Yes</asp:ListItem>
                                                                    <asp:ListItem Value="N">&nbsp;No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="rdbCablerequired"
                                                                    ErrorMessage="Installation type cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-3 form-group">
                                                                Output Cable Type<%--<asp:Label ID="Label16" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdoOutputCableType" runat="server" RepeatDirection="Horizontal"
                                                                    Width="120px">
                                                                    <asp:ListItem Value="Old">&nbsp;Old</asp:ListItem>
                                                                    <asp:ListItem Value="New">&nbsp;New</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator61" runat="server" ControlToValidate="rdoOutputCableType"
                                                                    ErrorMessage="Output Cable Used Type  cannot be blank" SetFocusOnError="true"
                                                                    Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Is Remove From Site?<%--<asp:Label ID="Label35" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdoIsCableRemovefromsite" runat="server" RepeatDirection="Horizontal"
                                                                    Width="120px" AutoPostBack="true">
                                                                    <asp:ListItem Value="No" Selected="True">&nbsp;No</asp:ListItem>
                                                                    <asp:ListItem Value="Yes">&nbsp;Yes</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator55" runat="server" ControlToValidate="rdoIsCableRemovefromsite"
                                                                    ErrorMessage="Cable Remove From Site cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                ELCB Installed<%--<asp:Label ID="Label20" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:RadioButtonList ID="rdoELCBInstalled" runat="server" RepeatDirection="Horizontal"
                                                                    Width="120px">
                                                                    <asp:ListItem Value="Yes">&nbsp;Yes</asp:ListItem>
                                                                    <asp:ListItem Value="No">&nbsp;No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator57" runat="server" ControlToValidate="rdoELCBInstalled"
                                                                    ErrorMessage="Please select any value" Display="Dynamic" SetFocusOnError="true"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Drum No.<%--<asp:Label ID="Label73" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtDrumno" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Drum No" MaxLength="49" />
                                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator59" runat="server" ControlToValidate="txtDrumno"
                                                                    ErrorMessage="Bus-Bar Drum No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-3 form-group">
                                                                Remove Cable Size<%--<asp:Label ID="Label18" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:DropDownList ID="ddlRemovecablesize" runat="server" CssClass="form-control input-sm">
                                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Cable Not Installed">Cable Not Installed</asp:ListItem>
                                                                    <asp:ListItem Value="2*10">2*10</asp:ListItem>
                                                                    <asp:ListItem Value="2*25">2*25</asp:ListItem>
                                                                    <asp:ListItem Value="4*25">4*25</asp:ListItem>
                                                                    <asp:ListItem Value="4*50">4*50</asp:ListItem>
                                                                    <asp:ListItem Value="4*150">4*150</asp:ListItem>
                                                                </asp:DropDownList>
                                                       <%--         <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="ddlRemovecablesize"
                                                                    ErrorMessage="Please select any value" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Remove Cable Length<%--<asp:Label ID="Label19" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtrmvcablelength" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Remove Cable Length" MaxLength="49" />
                                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator56" runat="server" ControlToValidate="txtrmvcablelength"
                                                                    ErrorMessage="Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                MAT Description<%--<asp:Label ID="Label9" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:DropDownList ID="ddlMatdesc" runat="server" CssClass="form-control input-sm" placeholder="MAT Description">
                                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                                    <asp:ListItem Value="SC">Separate Cable</asp:ListItem>
                                                                    <asp:ListItem Value="Loop Cable">Loop Cable</asp:ListItem>
                                                                    <asp:ListItem Value="WC">Without Cable</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:RequiredFieldValidator ID="rfvMatDesc" runat="server" ControlToValidate="ddlMatdesc"
                                                                    ErrorMessage="Select MAT Description" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>

                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Remark<%--<asp:Label ID="Label81" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtRemoveCableRemarks" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Remarks" MaxLength="198" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator77" runat="server" ControlToValidate="txtRemoveCableRemarks"
                                                                    ErrorMessage="Remarks cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-3 form-group">
                                                                Size<%--<asp:Label ID="Label10" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:DropDownList ID="ddlInstalledCableSize" runat="server" CssClass="form-control input-sm">
                                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Cable Not Installed">Cable Not Installed</asp:ListItem>
                                                                    <asp:ListItem Value="2*10">2*10</asp:ListItem>
                                                                    <asp:ListItem Value="2*25">2*25</asp:ListItem>
                                                                    <asp:ListItem Value="4*25">4*25</asp:ListItem>
                                                                    <asp:ListItem Value="4*50">4*50</asp:ListItem>
                                                                    <asp:ListItem Value="4*150">4*150</asp:ListItem>
                                                                </asp:DropDownList>
                                 <%--                               <asp:RequiredFieldValidator ID="rfvInstalledCableSize" runat="server" ControlToValidate="ddlInstalledCableSize"
                                                                    ErrorMessage="Please select any value" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Length<%--<asp:Label ID="Label11" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtInstalledCableLength" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Length" MaxLength="49" />
                                                               <%-- <asp:RequiredFieldValidator ID="rfvInstalledCableLength" runat="server" ControlToValidate="txtInstalledCableLength"
                                                                    ErrorMessage="Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Output Size
                                                                        <%--<asp:Label ID="Label13" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:DropDownList ID="ddlOutputCableSize" runat="server" CssClass="form-control input-sm">
                                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                                    <asp:ListItem Value="Cable Not Installed">Cable Not Installed</asp:ListItem>
                                                                    <asp:ListItem Value="2*10">2*10</asp:ListItem>
                                                                    <asp:ListItem Value="2*25">2*25</asp:ListItem>
                                                                    <asp:ListItem Value="4*25">4*25</asp:ListItem>
                                                                    <asp:ListItem Value="4*50">4*50</asp:ListItem>
                                                                    <asp:ListItem Value="4*150">4*150</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:RequiredFieldValidator ID="rfvOutputCableSize" runat="server" ControlToValidate="ddlOutputCableSize"
                                                                    ErrorMessage="Size cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Output Length<%--<asp:Label ID="Label14" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtOutputCableLength" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Output Length" MaxLength="49" />
                                                               <%-- <asp:RequiredFieldValidator ID="rfvOutputCableLength" runat="server" ControlToValidate="txtOutputCableLength"
                                                                    ErrorMessage="Output Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-3 form-group">
                                                                Running Length From<%--<asp:Label ID="Label15" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtRunninglengthFrom" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Running Length From" MaxLength="49" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtRunninglengthFrom"
                                                                    ErrorMessage="Bus-bar Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Running Length To<%--<asp:Label ID="Label17" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtRunninglengthTo" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Running Length To" MaxLength="49" />
                                                              <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="txtRunninglengthTo"
                                                                    ErrorMessage="Bus-bar Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Remove Cable Length<%--<asp:Label ID="Label31" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtRemoveCableLength" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Remove Cable Length" MaxLength="49" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtRemoveCableLength"
                                                                    ErrorMessage="Remove Cable Length cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Cable Not Installed Reason<%--<asp:Label ID="Label32" runat="server" CssClass="required">*</asp:Label>--%>
                                                                <asp:TextBox ID="txtcableinstalledreason" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Cable Not Installed Reason" MaxLength="100" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtcableinstalledreason"
                                                                    ErrorMessage="Cable Not Installed Reason be blank" SetFocusOnError="true" Display="Dynamic"
                                                                    ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-3 form-group">
                                                                Removal Box Seal 1
                                                                <asp:TextBox ID="txtRmovalboxseal1" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removal Box Seal 1" MaxLength="49" />
<%--                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator65" runat="server" ControlToValidate="txtRmovalboxseal1" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Removal Box Seal 2
                                                                <asp:TextBox ID="txtRmovalboxseal2" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removal Box Seal 1" MaxLength="49" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator66" runat="server" ControlToValidate="txtRmovalboxseal2" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                            <div class="col-md-3 form-group">
                                                                Removal Others Seal
                                                                <asp:TextBox ID="txtRemovalotherseal" runat="server" CssClass="form-control input-sm"
                                                                    placeholder="Removal Others Seal" MaxLength="49" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="txtRemovalotherseal" ErrorMessage="Bus Bar Seal 2 cannot be blank" SetFocusOnError="true" Display="Dynamic" ValidationGroup="save" ForeColor="Red" />--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                    </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 form-group">
                                            <b>Others Details</b>
                                            <div style="border: solid 1px #ccc; padding: 5px;">
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Earthing Connector<asp:Label ID="Label40" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtEarthingconnector" runat="server" CssClass="form-control input-sm" placeholder="Earthing Connector"
                                                            MaxLength="49" onkeypress="return validateFloatKeyPress(this,event);" />
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtEarthingconnector"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Jubliee Clamps<%--<asp:Label ID="Label50" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtjubleeclamp" runat="server" CssClass="form-control input-sm" placeholder="Jubliee Clamps"
                                                            MaxLength="49" />
                                       <%--                 <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="txtjubleeclamp"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Helper Name<%--<asp:Label ID="Label57" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtHelpername" runat="server" CssClass="form-control input-sm" placeholder="Helper Name"
                                                            MaxLength="49" />
                                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="txtHelpername"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Close Hook Bolt<%--<asp:Label ID="Label58" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtClosehookbolt" runat="server" CssClass="form-control input-sm" placeholder="Close Hook Bolt"
                                                            MaxLength="49" />
                                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="txtClosehookbolt"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Nylon Tie
                                                            <asp:Label ID="Label41" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtNylon" runat="server" CssClass="form-control input-sm" placeholder="Nylon Tie"
                                                            MaxLength="49"  onkeypress="return validateFloatKeyPress(this,event);"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtNylon"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Fastner
                                                            <%--<asp:Label ID="Label24" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtFastner" runat="server" CssClass="form-control input-sm" placeholder="Fastner"
                                                            MaxLength="49" />
                                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator68" runat="server" ControlToValidate="txtFastner"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Pole Condition
                                                            <%--<asp:Label ID="Label51" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtPolecondition" runat="server" CssClass="form-control input-sm" placeholder="Pole Condition"
                                                            MaxLength="49" />
                                                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="txtPolecondition"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Saddle Clamp
                                                           <asp:Label ID="Label56" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtSaddle" runat="server" CssClass="form-control input-sm" placeholder="Saddle Clamp"
                                                            MaxLength="49" onkeypress="return validateFloatKeyPress(this,event);"  />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtSaddle"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Hazardous Type<%--<asp:Label ID="Label42" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtHazardous" runat="server" CssClass="form-control input-sm" placeholder="Hazardous Type"
                                                            MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtHazardous"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        NOS Cable At Pole
                                                            <%--<asp:Label ID="Label43" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtNOScableat" runat="server" CssClass="form-control input-sm" placeholder="NOS Cable At Pole"
                                                            MaxLength="49" />
                                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtNOScableat"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Additional Pole Required<%--<asp:Label ID="Label4" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtAdditionalpole" runat="server" CssClass="form-control input-sm" placeholder="Additional Pole Required"
                                                            MaxLength="4" />
                                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtAdditionalpole"
                                                            ErrorMessage="Bus-Bar Drum No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                        </td>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Is Record Processed?<%--<asp:Label ID="Label21" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:RadioButtonList ID="rdbisrecordprocess" runat="server" Width="120px" RepeatDirection="Horizontal"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Value="No">&nbsp;No</asp:ListItem>
                                                            <asp:ListItem Value="Yes" Selected="True">&nbsp;Yes</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="rdbisrecordprocess"
                                                            ErrorMessage="Please select any value" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Additional Pole Number<%--<asp:Label ID="Label66" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtAdditionalpoleno" runat="server" CssClass="form-control input-sm" placeholder="Additional Pole Number"
                                                            MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" ControlToValidate="txtAdditionalpoleno"
                                                            ErrorMessage="Bus-Bar Drum No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Supervisor Name<%--<asp:Label ID="Label44" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtServiceprovider" runat="server" CssClass="form-control input-sm" placeholder="Supervisor Name"
                                                            MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtServiceprovider"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Driver Name
                                                            <%--<asp:Label ID="Label55" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtDrivername" runat="server" CssClass="form-control input-sm" placeholder="Driver Name"
                                                            MaxLength="49" />
                                              <%--          <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtDrivername"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Supervisor Name<asp:Label ID="Label62" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtSuperwisername" runat="server" CssClass="form-control input-sm" placeholder="Supervisor Name"
                                                            MaxLength="49" />
                                                   <%--     <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="txtSuperwisername"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        No. of Meter<%--<asp:Label ID="Label27" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtnoofmtr" runat="server" CssClass="form-control input-sm"
                                                            placeholder="No. of Meter" MaxLength="49" />
                                                      <%--  <asp:RequiredFieldValidator ID="rfvGLSlottedAngle" runat="server" ControlToValidate="txtnoofmtr"
                                                            ErrorMessage="GL Slotted Angle cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Connected Meters<%--<asp:Label ID="Label28" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtconnectedmtr" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Connected Meters" MaxLength="49" />
                                                        <%--<asp:RequiredFieldValidator ID="rfvServiceCableAngle" runat="server" ControlToValidate="txtconnectedmtr"
                                                            ErrorMessage="Sevice Cable Angle cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Other Sticker
                                                           <%-- <asp:Label ID="Label68" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtOtherSticker" runat="server" CssClass="form-control input-sm" placeholder="Other Sticker"
                                                            MaxLength="49" />
                                                  <%--      <asp:RequiredFieldValidator ID="RequiredFieldValidator54" runat="server" ControlToValidate="txtOtherSticker"
                                                            ErrorMessage="Bus-Bar No cannot be blank" SetFocusOnError="true" Display="Dynamic"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        DB Type
                                                                <asp:DropDownList ID="ddlDbtype" runat="server" CssClass="form-control input-sm">
                                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                                    <asp:ListItem Value="2*10">SP</asp:ListItem>
                                                                    <asp:ListItem Value="2*25">TP</asp:ListItem>
                                                                </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator90" runat="server" ControlToValidate="ddlDbtype" ErrorMessage="Select DB Type" Display="Dynamic" SetFocusOnError="true" ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                        Gunny Number<%--<asp:Label ID="Label79" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtGunnyBagNumber" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Gunny Number" MaxLength="49" />
                                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator71" runat="server" ControlToValidate="txtGunnyBagNumber"
                                                            ErrorMessage="Number cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Gunny Seal<%--<asp:Label ID="Label33" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtGunnyBagSeal" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Gunny Seal" MaxLength="49" />
                                                       <%-- <asp:RequiredFieldValidator ID="rfvGunnyBagSeal" runat="server" ControlToValidate="txtGunnyBagSeal"
                                                            ErrorMessage="Seal cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        LAb Testing Notice No<%--<asp:Label ID="Label80" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtGunnyBagNoticeNo" runat="server" CssClass="form-control input-sm"
                                                            placeholder="LAb Testing Notice No" MaxLength="49" />
                                                      <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator72" runat="server" ControlToValidate="txtGunnyBagNoticeNo"
                                                            ErrorMessage="Notice No cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Is Gunny Bag?<%--<asp:Label ID="Label30" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:RadioButtonList ID="rdoIsGunnyBag" runat="server" RepeatDirection="Horizontal"
                                                            Width="120px" AutoPostBack="true">
                                                            <asp:ListItem Value="No" Selected="True">&nbsp;No</asp:ListItem>
                                                            <asp:ListItem Value="Yes">&nbsp;Yes</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator70" runat="server" ControlToValidate="rdoIsGunnyBag"
                                                            ErrorMessage="Gunny Bag cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 form-group">
                                                    Piercing Connector<asp:Label ID="Label79" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtPiercingConnector" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Piercing Connector" MaxLength="49" onkeypress="return validateFloatKeyPress(this,event);" />
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator71" runat="server" ControlToValidate="txtPiercingConnector"
                                                            ErrorMessage="Number cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        Thimble<asp:Label ID="Label33" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtThimble" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Thimble" MaxLength="49" onkeypress="return validateFloatKeyPress(this,event);"/>
                                                       <asp:RequiredFieldValidator ID="rfvGunnyBagSeal" runat="server" ControlToValidate="txtThimble"
                                                            ErrorMessage="Seal cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                        PVC Gland<asp:Label ID="Label80" runat="server" CssClass="required">*</asp:Label>
                                                        <asp:TextBox ID="txtPVCGland" runat="server" CssClass="form-control input-sm"
                                                            placeholder="PVC Gland" MaxLength="49" onkeypress="return validateFloatKeyPress(this,event);" />
                                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator72" runat="server" ControlToValidate="txtPVCGland"
                                                            ErrorMessage="Notice No cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />
                                                    </div>
                                                    <div class="col-md-3 form-group">
                                                       Anchor Pole End Qty<%--<asp:Label ID="Label80" runat="server" CssClass="required">*</asp:Label>--%>
                                                        <asp:TextBox ID="txtAnchorpoleendqty" runat="server" CssClass="form-control input-sm"
                                                            placeholder="Anchor Pole End Qty" MaxLength="49"/>
                                                     <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator72" runat="server" ControlToValidate="txtGunnyBagNoticeNo"
                                                            ErrorMessage="Notice No cannot be blank" Display="Dynamic" SetFocusOnError="true"
                                                            ValidationGroup="save" ForeColor="Red" />--%>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-7 form-group" style="padding-top: 20px;">
                                            <div class="pull-right">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="save" CssClass="btn btn-sm btn-primary" OnClick="btnSave_Click" />&nbsp;&nbsp;
                                                <asp:Button ID="btnBack" runat="server" Text="Back" CausesValidation="false" CssClass="btn btn-sm btn-info" OnClick="btnBack_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--<div id="myModal" class="modal fade" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal">
                                            &times;</button>
                                        <h4 class="modal-title">Remarks</h4>
                                    </div>
                                    <div class="modal-body">
                                        <p id="pRemarks">
                                        </p>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">
                                            Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
  <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMode" runat="server" Visible="false" />
            <asp:Label ID="lblPKId" runat="server" Visible="false" />
            <asp:Label ID="lblUserId" runat="server" Visible="false" />
            <asp:Label ID="lblProfileCode" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>--%>
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

