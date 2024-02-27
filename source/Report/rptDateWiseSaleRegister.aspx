<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDateWiseSaleRegister.aspx.cs"
    Inherits="Report_rptDateWiseSaleRegister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Wise Sale Register</title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head> <link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body class="largsize">');
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
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri" CssClass="largsize">
            <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" Width="90%" Text="Sale Details Report" CssClass="lblName"
                Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="border-bottom: 1px solid black;
                table-layout: fixed;" class="largsize">
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblBrokerName" Text="Broker" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblDate1" Font-Bold="true" Text="Date"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="border-bottom: 1px solid black;
                table-layout: fixed;" class="largsize">
                <tr>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="lblNo" runat="server" Text="Our.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblMill" runat="server" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 6%;">
                        <asp:Label ID="Label1" runat="server" Text="party" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblNetQntl" runat="server" Text="Net Quintal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblRate" runat="server" Text="Net Rate" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="lblSubTotal" runat="server" Text="Subtotal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblExtraExp" runat="server" Text="Bank Comm" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="Label3" runat="server" Text="Frieght" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="Label4" runat="server" Text="Other Amt" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblBillAmount" runat="server" Text="Bill Amount" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="border-bottom: 1px solid black;
                table-layout: fixed;" class="largsize">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtl" Width="100%" CellPadding="1" OnItemDataBound="dtl_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;"
                                    class="largsize">
                                    <tr>
                                        <td align="center" style="width: 5%; background-color: #FFFFCC;">
                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date")%>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 9%;">
                                        </td>
                                        <td align="left" style="width: 3%;">
                                        </td>
                                        <td align="left" style="width: 3%;">
                                        </td>
                                        <td align="left" style="width: 3%;">
                                        </td>
                                        <td align="left" style="width: 3%;">
                                        </td>
                                        <td align="left" style="width: 3%;">
                                        </td>
                                        <td align="left" style="width: 4%;">
                                        </td>
                                        <td align="left" style="width: 3%;">
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellpadding="1" cellspacing="0" class="largsize">
                                    <tr>
                                        <td style="width: 100%;" colspan="7">
                                            <asp:DataList runat="server" ID="dtlDetails" Width="100%" CellPadding="1">
                                                <ItemTemplate>
                                                    <table width="100%" align="center" cellpadding="3" cellspacing="0" style="border-bottom: 1px dashed black;
                                                        table-layout: fixed;" class="largsize">
                                                        <tr>
                                                            <td align="left" style="width: 2%;">
                                                                <asp:Label ID="lbldtlNo" runat="server" Text='<%#Eval("P_No")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="lbldtlMill" runat="server" Text='<%#Eval("Mill")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 6%;">
                                                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("party")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="lbldtlNetQntl" runat="server" Text='<%#Eval("qntl")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="lbldtlRate" runat="server" Text='<%#Eval("Rate")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 4%;">
                                                                <asp:Label ID="lbldtlSubTotal" runat="server" Text='<%#Eval("Subtotal")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="lbldtlExtraExp" runat="server" Text='<%#Eval("bank_commission")%>'
                                                                    Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("freight")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("OTHER_AMT")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 3%;">
                                                                <asp:Label ID="lbldtlBillAmount" runat="server" Text='<%#Eval("Bill_Amount")%>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" colspan="8">
                                            <table width="100%" align="center" cellpadding="3" cellspacing="0" style="border-bottom: 1px solid  black;
                                                background-color: #CCFFFF; table-layout: fixed;" class="largsize">
                                                <tr>
                                                    <td align="left" style="width: 2%;">
                                                        <asp:Label ID="lbldtlNo" runat="server" Text="" Font-Bold="false"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 9%;">
                                                        <asp:Label ID="lbldtlMill" runat="server" Text="Total:" Font-Bold="false"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="lbldtlDetailsNetQntl" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="lbldtlDetailsRate" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 4%;">
                                                        <asp:Label ID="lbldtlDetailsSubTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="lbldtlDetailsExtraExp" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="lbldtlDetailfrieght" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="lbldtlDetailotheramt" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="lbldtlDetailsBillAmount" runat="server" Text="" Font-Bold="true"></asp:Label>
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
                <tr>
                    <td style="width: 100%;">
                        <table width="100%" align="center" cellpadding="3" cellspacing="0" style="border-bottom: 1px solid  black;
                            background-color: #CCFFFF; table-layout: fixed;" class="largsize">
                            <tr>
                                <td align="left" style="width: 2%;">
                                    <asp:Label ID="lbldtlNo" runat="server" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 9%;">
                                    <asp:Label ID="lbldtlMill" runat="server" Text="Grand Total:" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblGrandNetQntl" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lbldtlDetailsRate" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 4%;">
                                    <asp:Label ID="lblGrandSubTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblGrandExtraExp" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblGrandfright" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblGrandotheramt" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%;">
                                    <asp:Label ID="lblGrandBillAmount" runat="server" Text="" Font-Bold="true"></asp:Label>
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
