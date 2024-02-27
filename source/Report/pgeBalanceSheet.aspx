<%@ Page Title="Balance Sheet" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeBalanceSheet.aspx.cs" Inherits="Report_pgeBalanceSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function sp(dt) {
            var tn;

            window.open('rptBalanceSheet.aspx?Cwhere=' + dt, '_blank');    //R=Redirected  O=Original
        }
        function PL(dt) {
            var tn;

            window.open('rptProfitLoss.aspx?dt=' + dt, '_blank');    //R=Redirected  O=Original
        }
        function sp1(dt) {
            var tn;

            window.open('rptBalanceSheetNew.aspx?Cwhere=' + dt, '_blank');    //R=Redirected  O=Original
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
            <asp:Label ID="label1" runat="server" Text="   Balance Sheet  " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:Panel ID="pnlMain" runat="server" ForeColor="Black" Font-Size="14px">
        <table width="60%" align="center" cellpadding="10" cellspacing="5">
            <tr>
                <td align="left">
                    Upto Date:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" MaxLength="10"
                        onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                        Height="24px"></asp:TextBox>
                    <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                        Width="25px" Height="15px" />
                    <ajax1:CalendarExtender ID="calenderExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                        PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                </td>
                <td align="center">
                    <asp:Button ID="btnBalanceSheet" runat="server" Width="130px" Text="Balance Sheet"
                        CssClass="btnHelp" OnClick="btnBalanceSheet_Click" Height="24px" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnProfitLoss" runat="server" Width="130px" Text="Profit & Loss"
                        CssClass="btnHelp" OnClick="btnProfitLoss_Click" Height="24px" />
                </td>
                <td align="center">
                    <asp:Button ID="btnBalanceSheetNew" runat="server" Width="130px" Text="Balance Sheet New"
                        CssClass="btnHelp" OnClick="btnBalanceSheetNew_Click" Height="24px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
