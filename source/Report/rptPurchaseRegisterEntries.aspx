<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptPurchaseRegisterEntries.aspx.cs"
    Inherits="Report_rptPurchaseRegisterEntries" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body style="font-family:Calibri;font-size:12px;width:1100px; text-align:center;" >');
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
            <asp:Label ID="Label15" runat="server" Width="90%" Text="Purchase Register Summary"
                CssClass="lblName" Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <br />
            <table width="80%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td colspan="7" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="80%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblMemono" runat="server" Text="B.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblMill" runat="server" Text="B.Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblParty" runat="server" Text="Rate" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblQntl" runat="server" Text="Qntl" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblVehNo" runat="server" Text="Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 6%;">
                        <asp:Label ID="lblAmount" runat="server" Text="Supplier" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="80%" align="center" cellpadding="1" cellspacing="0">
                <tr>
                    <td colspan="7" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" align="center">
                        <asp:DataList ID="dtl" runat="server" Width="100%" OnItemDataBound="DataList_ItemDataBound"
                            CellSpacing="4">
                            <ItemTemplate>
                                <table align="center" width="100%" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                                    background-color: #CCFFFF;">
                                    <tr>
                                        <td align="center" style="width: 2%;">
                                            <asp:Label ID="lblMemonod" runat="server" Text='<%#Eval("purc_no") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 3%;">
                                            <asp:Label ID="lblMilld" runat="server" Text='<%#Eval("purc_date") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 3%;">
                                            <asp:Label ID="lblPartyd" runat="server" Text='<%#Eval("rate") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 3%;">
                                            <asp:Label ID="lblQntld" runat="server" Text='<%#Eval("quantal") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 3%;">
                                            <asp:Label ID="lblVehNod" runat="server" Text='<%#Eval("amount") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 6%;">
                                            <asp:Label ID="lblAmountd" runat="server" Text='<%#Eval("supplier") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td colspan="7" align="right" style="border-bottom: dashed 2px black;">
                    </td>
                </tr>
            </table>
            <%--  <tr>
                <td colspan="7" align="right" style="border-bottom: double 2px black;">
                </td>
            </tr>
            <tr>
                <td>--%>
            <table align="center" width="80%" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                background-color: #FFFFCC;">
                <tr>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblMemonods" runat="server" Text="" Font-Bold="false"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblMillds" runat="server" Text="" Font-Bold="false"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblPartyds" runat="server" Text="" Font-Bold="false"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblQnttotal" runat="server" Text="qntltotal" Font-Bold="false"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblamounttotal" runat="server" Text="amount" Font-Bold="false"></asp:Label>
                    </td>
                    <td align="left" style="width: 6%;">
                        <asp:Label ID="lblAmountd" runat="server" Text="" Font-Bold="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
