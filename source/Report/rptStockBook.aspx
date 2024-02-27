<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptStockBook.aspx.cs" Inherits="Report_rptStockBook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detail Stock Book</title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
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
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" align="center" Font-Names="calibri" CssClass="largsize">
            <table width="80%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;"
                class="print">
                <tr>
                    <td style="width: 100%;" align="center">
                        <asp:Label runat="server" ID="lblCompanyName" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" align="center">
                        <asp:Label runat="server" ID="Label11" Font-Bold="true" Text="Detail Stock Book"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                border-bottom: solid 2px black; border-top: solid 2px black;" class="print">
                <tr>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblMill" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblParty" runat="server" Text="Tran_Type" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblMemono" runat="server" Text="#" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 7%;">
                        <asp:Label ID="Label1" runat="server" Text="Customer/Supplier" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblQntl" runat="server" Text="Op Qty" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label8" runat="server" Text="Op Val" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label3" runat="server" Text="Inward" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label9" runat="server" Text="Inward val" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label4" runat="server" Text="Outward" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label10" runat="server" Text="Outward Val" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label5" runat="server" Text="Bal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblVehNo" runat="server" Text="Value" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="Label12" runat="server" Text="DO #" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList ID="dtl" runat="server" Width="100%" CellSpacing="4" OnItemDataBound="dtl_OnItemDataBound">
                            <ItemTemplate>
                                <table align="center" width="100%" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                                    border-bottom: dashed 2px black;" class="print">
                                    <tr>
                                        <td style="width: 100%; border-bottom: solid 1px black;">
                                            <asp:Label ID="lblItemCode" runat="server" Text='<%#Eval("item_code") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("item_name") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table align="center" width="100%" cellpadding="1" cellspacing="2" style="table-layout: fixed;
                                                        border-bottom: dashed 1px black;" class="print">
                                                        <tr>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="lblMill" runat="server" Text='<%#Eval("date") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="lblParty" runat="server" Text='<%#Eval("Tran_Type") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="lblMemono" runat="server" Text='<%#Eval("#") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 7%;">
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("supplier") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="lblQntl" runat="server" Text='<%#Eval("OpeningQty") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="Label7" runat="server" Text='<%#Eval("OpeningVal") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("PurcQty") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("PurcVal") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("SaleQty") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("SaleVal") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 2%;">
                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("BalQty") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 3%;">
                                                                <asp:Label ID="lblVehNo" runat="server" Text='<%#Eval("BalValue") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 3%;">
                                                                <asp:Label ID="Label13" runat="server" Text='<%#Eval("DONo") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <table width="100%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                                                border-bottom: solid 1px black; border-top: solid 1px black;" class="print">
                                                <tr>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblMill" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblParty" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblMemono" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 7%;">
                                                        <asp:Label ID="Label1" runat="server" Text="Grand Total:" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblopqty" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="Label8" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblNetInwardQntl" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblNetInwardValue" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblNetOutwardQntl" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="lblNetOutwardValue" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 2%;">
                                                        <asp:Label ID="Label5" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 3%;">
                                                        <asp:Label ID="lblVehNo" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 3%;">
                                                        <asp:Label ID="Label14" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
