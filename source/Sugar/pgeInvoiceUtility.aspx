<%@ Page Title="Invoice Utility" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeInvoiceUtility.aspx.cs" Inherits="Sugar_pgeInvoiceUtility" %>
    <%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Invoice Utility   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table style="width: 60%;" align="center">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Branch Code:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtBranchCode" runat="server" Width="70px" CssClass="txt"></asp:TextBox>&nbsp;
                            <asp:TextBox ID="txtBranchname" runat="server" Width="200px" CssClass="txt"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Invoice Header:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtInvoiceHeader" runat="server" Width="250px" CssClass="txt" TextMode="MultiLine"
                                Height="100px"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Invoice Left Side:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtInvLeft" runat="server" Width="250px" CssClass="txt" TextMode="MultiLine"
                                Height="100px" TabIndex="1"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Invoice Right Side:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtInvRight" runat="server" Width="250px" CssClass="txt" TextMode="MultiLine"
                                Height="100px" TabIndex="2"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Bank name:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtbankName" runat="server" Width="250px" CssClass="txt" TabIndex="3"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Invoice Footer:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtInvfooter" runat="server" Width="250px" CssClass="txt" TextMode="MultiLine"
                                Height="100px" TabIndex="4"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnUpdate" runat="server" Text="update" CssClass="btnHelp" Width="90px"
                                ValidationGroup="save" OnClick="btnUpdate_Click" TabIndex="5" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
