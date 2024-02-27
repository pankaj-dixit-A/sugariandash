<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptPurchaseRegister.aspx.cs"
    Inherits="Report_rptPurchaseRegister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body sstyle="font-family:Calibri;font-size:12px;width:1100px; text-align:center;"  >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri">
            <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" Width="90%" Text="Purchase Details Report"
                CssClass="lblName" Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <table width="90%" align="center" cellpadding="1" cellspacing="0">
                <tr>
                    <td colspan="7" align="right">
                        <table align="left">
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:Label runat="server" ID="lblBrokerName" Text="Broker" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="6">
                                    <asp:Label runat="server" ID="lblDate" Font-Bold="true" Text="Date"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="9" style="border-bottom: double 2px black;">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="lblNo" runat="server" Text="Our.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblPurchaseDate" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="lblBillNo" runat="server" Text="Bill.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 8%;">
                        <asp:Label ID="lblMill" runat="server" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblNetQntl" runat="server" Text="Qntl" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblRate" runat="server" Text="Rate" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="lblSubTotal" runat="server" Text="Subtotal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="lblExtraExp" runat="server" Text="Extra Expenses" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblBillAmount" runat="server" Text="Bill Amt" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="9" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0">
                <tr>
                    <td>
                        <asp:DataList runat="server" ID="dtl" Width="100%" CellPadding="5">
                            <ItemTemplate>
                                <table width="100%" align="center" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td align="left" style="width: 2%;">
                                            <asp:Label ID="lbldtlNo" runat="server" Text='<%#Eval("P_No")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lbldtlPurchaseDate" runat="server" Text='<%#Eval("P_Date")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 2%;">
                                            <asp:Label ID="lbldtlBillNo" runat="server" Text='<%#Eval("Bill_No")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 8%;">
                                            <asp:Label ID="lbldtlMill" runat="server" Text='<%#Eval("Mill")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lbldtlNetQntl" runat="server" Text='<%#Eval("qntl")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lbldtlRate" runat="server" Text='<%#Eval("Rate")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 4%;">
                                            <asp:Label ID="lbldtlSubTotal" runat="server" Text='<%#Eval("Subtotal")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 4%;">
                                            <asp:Label ID="lbldtlExtraExp" runat="server" Text='<%#Eval("Extra_Expense")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lbldtlBillAmount" runat="server" Text='<%#Eval("Bill_Amount")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="9" style="border-bottom: dotted 2px black;">
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td style="border-bottom: double 2px black;">
                        <table width="100%" align="center" style="background-color: Yellow;" cellpadding="1"
                            cellspacing="0">
                            <tr>
                                <td colspan="9" style="border-bottom: double 2px black;">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 2%;">
                                    <asp:Label ID="Label1" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="Label2" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 2%;">
                                    <asp:Label ID="Label3" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%;">
                                    <asp:Label ID="Label4" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblQntlTotal" runat="server" Text="Qntl" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblRateTotal" runat="server" Text="Rate" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 4%;">
                                    <asp:Label ID="lblSubtotalTotal" runat="server" Text="Subtotal" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 4%;">
                                    <asp:Label ID="lblExpsTotal" runat="server" Text="Extra Expenses" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblBillTotal" runat="server" Text="Bill Amt" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
