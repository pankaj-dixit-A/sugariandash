<%@ Page Title="Bill Summary" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeBillSummary.aspx.cs" Inherits="Report_pgeBillSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function prs(fromDT, toDT) {
            window.open('rptPurchaseRegisterEntries.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
        function srs(fromDT, toDT) {
            window.open('rptSaleRegisterEntries.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
        function mprs(fromDT, toDT) {
            window.open('rptMonthWisePurchase.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
        function msrs(fromDT, toDT) {
            window.open('rptMonthWiseSale.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }

        function mwpr(fromDT, toDT) {
            window.open('rptMonthWisePurchaseRegister.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
        function mwsr(fromDT, toDT) {
            window.open('rptMonthWiseSaleRegister.aspx?fromDT=' + fromDT + '&toDT=' + toDT);
        }
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Bill Summary   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <br />
    <br />
    <table width="80%" cellspacing="5">
        <tr>
            <td align="left">
                From Date:
            </td>
            <td align="left">
                <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
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
                <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                    Height="15px" />
                <ajax1:CalendarExtender ID="CalendarExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                    PopupButtonID="Image1" Format="dd/MM/yyyy">
                </ajax1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button runat="server" ID="btnProcess" CssClass="btnHelp" Text="Process" Width="100px"
                    Height="25px" OnClick="btnProcess_Click" />
            </td>
            <td align="center">
                <asp:Button runat="server" ID="btnMonthWisePurchaseSale" CssClass="btnHelp" Text="Month Wise P & S"
                    Width="130px" Height="25px" OnClick="btnMonthWisePurchaseSale_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
