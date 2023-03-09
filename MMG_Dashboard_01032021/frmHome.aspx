<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="frmHome.aspx.cs" Inherits="MMG_Dashboard_01032021.frmHome" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modal {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .center img {
                height: 128px;
                width: 128px;
            }
    </style>
    <script src="Script/jsapi.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="css/Login.css" rel="stylesheet" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="divclass" style="height: auto">
        <table style="height: 300px; width: 100%; vertical-align: middle">
            <tr>
                <td style="text-align: center; width: 50%;">
                    <span style="font-size: 22px; font-style: italic; font-weight: bold; color: Red">Welcome
                        To MMG Invoice Management System</span>
                </td>
                <td style="text-align: right; width: 50%;">
                    <br />
                    <table style="border: 2px solid black; font-weight: bold; width: 60%; text-align: right;">
                        <tr>
                            <td colspan="2">
                                <h4 style="background: dimgray; color: #ffffff; padding: 5px; font-size: 16px; font-style: italic; font-weight: 100; display: block;">User information</h4>
                            </td>
                        </tr>
                        <tr>
                            <td>Name
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>User ID
                            </td>
                            <td>
                                <asp:Label ID="lblEmpCode" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Role
                            </td>
                            <td>
                                <asp:Label ID="lblEmployeeType" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="TRDiv_Row" runat="server">
                            <td>Division
                            </td>
                            <td>
                                <asp:Label ID="lblDivision" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblDivision1" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr id="tr1" runat="server" visible="false">
                            <td>Company
                            </td>
                            <td>
                                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
