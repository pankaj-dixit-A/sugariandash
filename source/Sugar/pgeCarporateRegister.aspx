<%@ Page Title="Carporate Register" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeCarporateRegister.aspx.cs" Inherits="Sugar_pgeCarporateRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function br(fromDt, toDt, Branch_Code) {
            window.open('../Report/rptCarporateBalance.aspx?fromDt=' + fromDt + '&toDt=' + toDt + '&Branch_Code=' + Branch_Code);
        }

        function csd(fromDt, toDt, PDS, Branch_Code) {
            window.open('../Report/rptCarporateSaleDetail.aspx?fromDt=' + fromDt + '&toDt=' + toDt + '&PDS=' + PDS + '&Branch_Code=' + Branch_Code);
        }
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Carporate Register   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel runat="server" ID="updatepnlMain" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="50%" align="center" cellspacing="10">
                <tr>
                    <td align="center" colspan="2" style="width: 100%">
                        <asp:Label ID="lblBranch" Text="Select Branch:" ForeColor="White" Font-Bold="true"
                            runat="server"></asp:Label>
                        &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="drpBranch" runat="server" Width="200px" CssClass="ddl">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="width: 100%">
                        <asp:Label ID="Label2" Text="Select Sell Type:" ForeColor="White" Font-Bold="true"
                            runat="server"></asp:Label>
                        <asp:DropDownList ID="drpSellingType" runat="server" Width="200px" CssClass="ddl">
                            <asp:ListItem Text="Carporate Sell" Value="C"></asp:ListItem>
                            <asp:ListItem Text="PDS Sell" Value="P"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        From Date:
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                        <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="calendertxtFromDate" runat="server" TargetControlID="txtFromDate"
                            PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                        To Date:
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                            Height="15px" />
                        <ajax1:CalendarExtender ID="CalendarExtendertxtToDate" runat="server" TargetControlID="txtToDate"
                            PopupButtonID="Image1" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 50%">
                        <asp:Button runat="server" ID="btnBalanceReport" Text="Balance Report" Width="150px"
                            Height="24px" CssClass="btnHelp" OnClick="btnBalanceReport_Click" />
                    </td>
                    <td align="center" style="width: 50%">
                        <asp:Button runat="server" ID="Button1" Text="Lotwise Detail" Width="150px" CssClass="btnHelp"
                            Height="24px" OnClick="Button1_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
