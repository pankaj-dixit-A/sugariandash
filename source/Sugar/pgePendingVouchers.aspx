<%@ Page Title="Pending Vouchers" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgePendingVouchers.aspx.cs" Inherits="Sugar_pgePendingVouchers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/Grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ChangeQuantityEnable(id, enable) {
            document.getElementById(id).disabled = !enable;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <asp:Label runat="server" ID="lblErrormsg" ForeColor="Red"></asp:Label>
    <asp:GridView ID="grdPendingVouchers" runat="server" AutoGenerateColumns="false"
        EmptyDataText="No Pending Vouchers" Width="800px" PageSize="20" HeaderStyle-BackColor="#397CBB"
        CssClass="Grid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
        OnRowDataBound="grdPendingVouchers_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Tender_No" HeaderText="Tender No" />
            <asp:BoundField DataField="DO_Date" HeaderText="Date" />
            <asp:BoundField DataField="Mill" HeaderText="Mill" />
            <asp:BoundField DataField="Qntl" HeaderText="Qntl" />
            <asp:BoundField DataField="Tender_From" HeaderText="Tender From" />
            <asp:BoundField DataField="Voucher_By" HeaderText="Voucher By" />
            <asp:BoundField DataField="Mill_Rate" HeaderText="M.R" />
            <asp:BoundField DataField="Purc_Rate" HeaderText="P.R" />
            <asp:BoundField DataField="Amount" HeaderText="Vouc.Amt" />
            <asp:TemplateField HeaderText="Create">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="grdCB" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Bill No">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtBillNo" Width="50px" Enabled="true"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtBillDate" Width="100px" Enabled="true"></asp:TextBox>
                    <ajax1:CalendarExtender runat="server" ID="ajxCalender" TargetControlID="txtBillDate"
                        Format="yyyy/MM/dd">
                    </ajax1:CalendarExtender>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button runat="server" ID="btnCreateVoucher" Text="Generate Voucher" CssClass="btnHelp"
        Height="24px" OnClick="btnCreateVoucher_Click" />
</asp:Content>
