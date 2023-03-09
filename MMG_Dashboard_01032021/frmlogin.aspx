<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.Master" AutoEventWireup="true" CodeBehind="frmlogin.aspx.cs" Inherits="MMG_Dashboard_01032021.frmlogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <br />
         <br />
    <br />
    <h2 style="background: #eaecef; margin: 0px; text-align: center;">Please Enter Your login Details.</h2>
    <br />
    <table align="center" border="2"  style="font-weight: bold; width: 40%; vertical-align: middle;">
        <tr>
            <td style="text-align:right;">User Id
            </td>
            <td>
                <asp:TextBox type="text" ID="txtUserName" runat="server" value="UserID" onblur="if(this.value=='')this.value='UserID'"
                    onfocus="if(this.value=='UserID')this.value='' " />
            </td>
        </tr>
        <tr>
            <td style="text-align:right;">Password
            </td>
            <td>
                <asp:TextBox type="password" ID="txtPassword" runat="server" value="Password" onblur="if(this.value=='')this.value='Password'"
                    onfocus="if(this.value=='Password')this.value='' " />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btnLogin" runat="server" Text="Login" Width="120px" CssClass="btn btn-sm btn-info" OnClick="btnLogin_Click"/>
                &nbsp;&nbsp;
                <a href="frmChangePassword.aspx" style="font-size: 12px; font-weight: bold">Change Password?</a>
            </td>
        </tr>
    </table>
</asp:Content>
