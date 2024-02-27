<%@ Page Title="Vasuli Register" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeVasuliRegister.aspx.cs" Inherits="Report_pgeVasuliRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../JQuery/jquery-1.4.1.js" type="text/javascript"></script>
    <style type="text/css">
        * .highlight
        {
            background-color: Yellow;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("# ?%=grdDetail.ClientID% ? td").hover(
        function () {
            $(this).addClass("highlight");
        },
function () {
    $(this).removeClass("highlight");
});
        });
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="  Vasuli Register   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <table align="center" width="700px">
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
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnShowVasuli" Text="SHOW" Height="24px" CssClass="btnHelp"
                    Width="80px" OnClick="btnShowVasuli_Click" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td align="left">
                <asp:Panel ID="pnlGrid" runat="server" align="left" BorderColor="Blue" BorderWidth="1px"
                    BackColor="#CCFFFF" Height="300px" ScrollBars="Both" Style="text-align: left"
                    Width="1100px" DefaultButton="btnEnter">
                    <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" CellPadding="6"
                        EmptyDataText="No Records Found" Font-Bold="true" ForeColor="Black" GridLines="Both"
                        HeaderStyle-BackColor="#397CBB" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px"
                        RowStyle-Height="30px" RowStyle-Wrap="false" Style="table-layout: fixed;" OnRowDataBound="grdDetail_RowDataBound">
                        <HeaderStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="doc_date" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="No." DataField="Doc_No" />
                            <asp:BoundField HeaderText="Tran_Type" DataField="Type" />
                            <asp:BoundField HeaderText="Ac_Code" DataField="Ac_Code" />
                            <asp:BoundField HeaderText="Party" DataField="PartyName" />
                            <asp:BoundField HeaderText="Broker" DataField="Broker" />
                            <asp:BoundField HeaderText="Qntl" DataField="Qntl" />
                            <asp:BoundField HeaderText="From" DataField="From_Station" />
                            <asp:BoundField HeaderText="V.Amount" DataField="Voc_Amt" />
                            <asp:TemplateField HeaderText="Cr.Days">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtDueDays" Text='<%#Eval("DueDays") %>' Width="60px"
                                        Style="text-align: center;" Height="20px" BorderStyle="None"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Recived" DataField="recieved" />
                            <asp:BoundField HeaderText="Shrot" DataField="short" />
                        </Columns>
                    </asp:GridView>
                    <asp:Button runat="server" ID="btnEnter" OnClick="btnEnter_Click" Style="display: none" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;" align="center">
                <asp:Button runat="server" ID="btnUpdateAll" Text="Update Data" CssClass="btnHelp"
                    Width="100px" Height="25px" OnClick="btnUpdateAll_Click" />
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <b>Total Voucher Amount:</b> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblVoucherAmount"
                    Font-Size="Medium" ForeColor="Black" BackColor="Yellow" Text="" Font-Bold="true"></asp:Label>
                <b>Total Recieved Amount:</b> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblRecieved"
                    Font-Size="Medium" ForeColor="Black" BackColor="Yellow" Text="" Font-Bold="true"></asp:Label>
                <b>Total Short Amount:</b> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblShort"
                    Font-Size="Medium" ForeColor="Black" BackColor="Yellow" Text="" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
