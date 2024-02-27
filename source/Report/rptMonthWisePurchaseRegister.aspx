<%@ Page Language="C#" Title="Month Wise Purchase Register" AutoEventWireup="true" CodeFile="rptMonthWisePurchaseRegister.aspx.cs" Inherits="Report_rptMonthWisePurchaseRegister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body style="font-family:Calibri;font-size:12px;text-align:center;" >');
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
            <asp:Label ID="Label15" runat="server" Width="90%" Text="Monthwise Purchase Register Summary"
                CssClass="lblName" Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <br />
            <table id="Table3" runat="server" align="center" width="40%" style="table-layout: fixed;">
                <tr>
                    <td align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table id="Table1" runat="server" align="center" width="40%" style="table-layout: fixed;">
                <tr>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="lblYear" Text="Year" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="Label1" Text="Month" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="Label2" Text="QNT." Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="Label3" Text="Amount" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table id="Table2" runat="server" align="center" width="40%">
                <tr>
                    <td align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataList runat="server" ID="dtl" CellSpacing="4" Width="100%">
                            <ItemTemplate>
                                <table id="Table2" runat="server" align="center" width="100%" style="table-layout: fixed;
                                    background-color: #CCFFFF;">
                                    <tr>
                                        <td align="left" style="width: 10%">
                                            <asp:Label runat="server" ID="Label4" Text='<%#Eval("yea") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%">
                                            <asp:Label runat="server" ID="Label5" Text='<%#Eval("mon") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%">
                                            <asp:Label runat="server" ID="Label6" Text='<%#Eval("quantal") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%">
                                            <asp:Label runat="server" ID="Label7" Text='<%#Eval("amount") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="right" style="border-bottom: dashed 2px black;">
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <table id="Table33" runat="server" align="center" width="40%" style="table-layout: fixed;
                background-color: #FFFFCC;">
                <tr>
                    <td colspan="4" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="Label8" Text="Net." Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="Label9" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="lblQnttotal" Text="qntltotal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label runat="server" ID="lblamounttotal" Text="amount" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
