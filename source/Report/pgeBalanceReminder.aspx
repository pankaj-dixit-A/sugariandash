<%@ Page Title="Balance Reminder" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeBalanceReminder.aspx.cs" Inherits="Report_pgeBalanceReminder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <link href="../CSS/tooltip.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/tooltip.js">
    </script>
    <script type="text/javascript" src="../JS/select all.js"></script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Balance Reminder   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <table runat="server" width="30%" align="center">
        <tr>
            <td align="left">
                UpTo Date:
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
            <td colspan="2" align="center">
                <asp:Button runat="server" ID="btnShow" CssClass="btnHelp" Width="100px" Height="25px"
                    Text="SHOW" OnClick="btnShow_Click" />
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" BorderColor="Blue" BorderWidth="1px" align="left" ScrollBars="Both"
        DefaultButton="btnEnter" ID="pnlReport" Height="300px" Width="900px" BackColor="Aqua"
        Style="text-align: left">
        <asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false" CellPadding="5"
            EmptyDataText="No Records Found" Font-Bold="false" ForeColor="Black" GridLines="Both"
            HeaderStyle-BackColor="#397CBB" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px"
            RowStyle-Height="30px" BackColor="White" RowStyle-Wrap="false" Style="table-layout: fixed;"
            OnRowDataBound="grdReport_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="Sale Party" DataField="saleparty" />
                <asp:BoundField HeaderText="" DataField="message" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtMobile" Height="20px" MaxLength="10" Width="120px"
                            BorderStyle="None" Text='<%#Eval("mobile") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="chkAll" Text="SMS" OnClick="selectAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="grdCB" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button runat="server" ID="btnEnter" Style="display: none" OnClick="btnEnter_Click" />
    </asp:Panel>
    <br />
    <table align="center">
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSendSms" Text="Send SMS" Width="100px" Height="25px"
                    CssClass="btnHelp" OnClick="btnSendSms_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
