<%@ Page Title="Stock Book" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeStockBook.aspx.cs" Inherits="Report_pgeStockBook" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function sb(fromDT, toDT) {
            window.open('../Report/rptStockBook.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
        function ss(fromDT, toDT) {
            window.open('../Report/rptStockSummary.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
        function MWS(fromDT, toDT) {
            window.open('../Report/rptMonthWiseStock.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Stock Book   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <br />
    <table align="center" width="40%" cellspacing="5">
        <tr>
            <td align="left">
                From Date:
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"></asp:TextBox>
                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                    Width="25px" Height="15px" />
                <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                </ajax1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td align="left">
                To Date:
            </td>
            <td align="left">
                <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" Height="24px"></asp:TextBox>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                    Height="15px" />
                <ajax1:CalendarExtender ID="CalendarExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                    PopupButtonID="Image1" Format="dd/MM/yyyy">
                </ajax1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button runat="server" ID="btnStockBook" Text="Stock Detail" Height="24px" Width="100px"
                    Font-Bold="true" CssClass="btnHelp" OnClick="btnStockBook_Click" />&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnStockSummary" Text="Stock Summary" Height="24px"
                    Width="100px" Font-Bold="true" CssClass="btnHelp" OnClick="btnStockSummary_Click" />
                <asp:Button runat="server" ID="btnMonthWiseStock" Text="Month Wise Stock Summary"
                    Height="24px" Width="200px" Font-Bold="true" CssClass="btnHelp" OnClick="btnMonthWiseStock_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
