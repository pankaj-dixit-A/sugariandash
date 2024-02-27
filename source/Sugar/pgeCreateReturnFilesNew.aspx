<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pgeCreateReturnFilesNew.aspx.cs"
    Inherits="Sugar_pgeCreateReturnFilesNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GST Returns</title>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
       
</head>
<body style="background-color: White;">
      <script type="text/javascript" language="javascript">
          function Confirm() {
              var confirm_value = document.createElement("INPUT");
              confirm_value.type = "hidden";
              confirm_value.name = "confirm_value";
              if (confirm("Create CSV File?")) {
                  confirm_value.value = "Yes";
                  document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <form id="form1" runat="server">
         <asp:HiddenField ID="hdconfirm" runat="server" />
    <div>
        <ajax1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </ajax1:ToolkitScriptManager>
        <table border="0" cellpadding="5" cellspacing="5" style="margin: 0 auto;">
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
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button Text="CREATE B2B FILE" runat="server" CssClass="btnHelp" ID="btnCreateb2b"
                        OnClick="btnCreateb2b_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Button ID="btnCreatePurchaseBillSummary" Text="PURCHASE BILL SUMMARY" runat="server"
                        OnClick="btnCreatePurchaseBillSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button ID="btnCreateSaleBillSummary" Text="SALE BILL SUMMARY" runat="server"
                        OnClick="btnCreateSaleBillSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button ID="btnFrieghtSummary" Text="FRIEGHT SUMMARY" runat="server" OnClick="btnFrieghtSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button ID="btnDebitNoteSummary" Text="DEBIT NOTE SUMMARY" runat="server" OnClick="btnDebitNoteSummary_Click" />
                </td>
                <td>
                        <asp:DropDownList runat="server" ID="drpdebitcreditnote" Width="100px" CssClass="ddl">
                            <asp:ListItem Text="All" Value="All" Selected="False" />
                            <asp:ListItem Text="Debit note to Customer" Value="DN" />
                            <asp:ListItem Text="Credit note to Customer" Value="CN" />
                            <asp:ListItem Text="Debit note to Supplier" Value="DS" />
                            <asp:ListItem Text="Customer note to Supplier" Value="CS" />

                        </asp:DropDownList>
                        <asp:Button Text="Debit Credit Note" runat="server" ID="btnDebitCreditNote" OnClick="btnDebitCreditNote_Click" />
                    </td>
                   <td colspan="2">Rent TCS TDS:
                    <asp:DropDownList runat="server" ID="drpSaleTCS" OnSelectedIndexChanged="drpSaleTCS_SelectedIndexChanged">
                        <asp:ListItem Text="All" Value="All" /> 
                        <asp:ListItem Text="Rent Bill" Value="RB" Selected="True" /> 
                    </asp:DropDownList>
                        <asp:Button Text="Sale TCS" runat="server" ID="btnSaleTCS" OnClick="btnSaleTCS_Click" OnClientClick="Confirm();" />
                        <asp:Button Text="Sale TDS" runat="server" ID="btnSaleTDS" OnClick="btnSaleTDS_Click" OnClientClick="Confirm();" />
                    </td>
                 <td colspan="1">
                        <asp:Button Text="Service Bill" runat="server" ID="btnServiceBill" OnClick="btnServiceBill_Click" />
                    </td>
            </tr>
        </table>
        <br />
        <asp:Panel runat="server" ID="pnlSale" BorderColor="Blue" Style="margin: 0 auto;"
            Width="100%">
            <br />
            <h3>
                <asp:Label Text="" ID="lblSummary" runat="server" /></h3>
            <asp:Button Text="EXPORT TO EXCEL" ID="btnExportToexcel" OnClick="btnExportToexcel_Click"
                runat="server" />
            <asp:GridView runat="server" ID="grdAll" AutoGenerateColumns="true" GridLines="Both"
                HeaderStyle-Font-Bold="true" RowStyle-Height="30px" ShowFooter="true">
                <%--<Columns>
                    <asp:BoundField DataField="SR_No" HeaderText="SR_No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="30px" />
                    <asp:BoundField DataField="Invoice_No" HeaderText="Invoice No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="80px" />
                    <asp:BoundField DataField="PartyGSTNo" HeaderText="GSTIN/UIN of Recipient" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="140px" />
                    <asp:BoundField DataField="PartyStateCode" HeaderText="State Code" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="40px" />
                    <asp:BoundField DataField="Invoice_Date" HeaderText="Invoice Date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="100px" />
                    <asp:BoundField DataField="Vehicle_No" HeaderText="Vehicle No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="140px" />
                    <asp:BoundField DataField="Quintal" HeaderText="Quintal" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="80px" />
                    <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="100px" />
                    <asp:BoundField DataField="TaxableAmount" HeaderText="Taxable Amount" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="CGST" HeaderText="CGST" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="SGST" HeaderText="SGST" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="IGST" HeaderText="IGST" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="Payable_Amount" HeaderText="Final Amount" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                </Columns>--%>
                <FooterStyle BackColor="Yellow" Font-Bold="true" />
            </asp:GridView>
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlPurchaseSummary" BorderColor="Blue" Style="margin: 0 auto;"
            Width="100%">
            <br />
            <h3>
                Purchase Summary</h3>
            <asp:Button Text="EXPORT TO EXCEL" ID="btnPSExport" OnClick="btnPSExport_Click" runat="server" />
            <asp:GridView runat="server" ID="grdPurchaseSummary" AutoGenerateColumns="false"
                GridLines="Both" HeaderStyle-Font-Bold="true" RowStyle-Height="30px" ShowFooter="true">
                <Columns>
                    <asp:BoundField DataField="SR_No" HeaderText="SR_No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="30px" />
                    <asp:BoundField DataField="OurNo" HeaderText="Our No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="80px" />
                    <asp:BoundField DataField="MillInvoiceNo" HeaderText="Mill Invoice No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="80px" />
                    <asp:BoundField DataField="FromGSTNo" HeaderText="GSTIN" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="140px" />
                    <asp:BoundField DataField="FromStateCode" HeaderText="State Code" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="40px" />
                    <asp:BoundField DataField="Date" HeaderText="Invoice Date" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="100px" />
                    <asp:BoundField DataField="Vehicle_No" HeaderText="Vehicle No" ItemStyle-HorizontalAlign="Center"
                        ItemStyle-Width="140px" />
                    <asp:BoundField DataField="Quintal" HeaderText="Quintal" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="80px" />
                    <asp:BoundField DataField="Rate" HeaderText="Rate" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="100px" />
                    <asp:BoundField DataField="TaxableAmount" HeaderText="Taxable Amount" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="CGST" HeaderText="CGST" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="SGST" HeaderText="SGST" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="IGST" HeaderText="IGST" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                    <asp:BoundField DataField="Payable_Amount" HeaderText="Final Amount" ItemStyle-HorizontalAlign="Right"
                        ItemStyle-Width="120px" />
                </Columns>
                <FooterStyle BackColor="Yellow" Font-Bold="true" />
            </asp:GridView>
        </asp:Panel>--%>
    </div>
    </form>
</body>
</html>
