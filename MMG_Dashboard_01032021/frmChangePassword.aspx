<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="frmChangePassword.aspx.cs" Inherits="MMG_Dashboard_01032021.frmChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <br />
      <br />
    <br />
     <h2 style="background: #eaecef; margin: 0px; text-align:center;">Please Enter Your login Details.</h2>
        <br />
                <table align="center" border="2" style="font-weight: bold; width:50%;">
                    <tr>
                        <td>User Name
                        </td>
                        <td>
                            <asp:TextBox type="text" ID="txtUserName"  runat="server" placeholder="User Id" />
                        </td>
                    </tr>
                    <tr>
                        <td>Old Password
                        </td>
                        <td>
                            <asp:TextBox type="password" ID="txtPassword" value="Old Password" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>New Password
                        </td>
                        <td>
                            <asp:TextBox type="password" ID="txtNPassword" value="New Password" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <br />
                            <asp:Button ID="btnLogin" Width="180px" runat="server" Text="Update Password" CssClass="btn btn-sm btn-info" 
                                OnClick="btnLogin_Click1" />
                            &nbsp;&nbsp;
                                <asp:Button ID="btnCancel" Width="180px" runat="server" Text="Exit" CssClass="btn btn-sm btn-primary"
                                    OnClick="btnCancel_Click"  />
                        </td>
                    </tr>
                </table>
</asp:Content>
