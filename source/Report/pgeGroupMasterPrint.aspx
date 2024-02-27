<%@ Page Title="Group Master Print" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeGroupMasterPrint.aspx.cs" Inherits="Report_pgeGroupMasterPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
                float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
                border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Group Master Print   " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <br />
            <br />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table cellspacing="5" align="center">
                    <tr>
                        <td colspan="2" align="left">
                            Group Section:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" CssClass="ddl" Height="24px" Width="250px" ID="drpGroupSelection">
                                <asp:ListItem Text="Trading" Value="T"></asp:ListItem>
                                <asp:ListItem Text="Profit & Loss" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Balnce Sheet" Value="B"></asp:ListItem>
                                <asp:ListItem Text="All" Value="A"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
