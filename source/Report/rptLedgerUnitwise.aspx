<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptLedgerUnitwise.aspx.cs"
    Inherits="Report_rptLedgerUnitwise" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" media="print" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.css" media="print" rel="Stylesheet" type="text/css" />');
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
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSendEmail" runat="server" Text="Send To Email" CssClass="btnHelp"
            OnClick="btnSendEmail_Click" />&nbsp;&nbsp; Email:<asp:TextBox runat="server" ID="txtEmail"
                Width="200px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel ID="PrintPanel" runat="server" align="center" Font-Names="Calibri" CssClass="printsmall">
            <asp:Label ID="lblCompanyName" runat="server" Text="" Width="100%" CssClass="lblName"
                Font-Bold="true" Font-Size="16px" Style="text-align: center; font-size: large;
                width: 100%;"></asp:Label>
            <asp:Label ID="lblReportName" runat="server" Text="Unit Wise Ledger" Width="100%"
                CssClass="lblName" Font-Bold="true" Font-Size="14px" Style="text-align: center;
                font-size: large; width: 100%;"></asp:Label>
            <table id="tbMain" runat="server" width="70%" align="center" class="printsmall">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="14px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60%;" align="left">
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label18" runat="server" Text="Account Statement of :" Font-Bold="true"
                            Font-Size="Small"></asp:Label>
                        <asp:Label ID="lblParty" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label><asp:Label
                            ID="lblAcCode" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                    </td>
                    <td align="right" style="width: 40%;">
                        <asp:Label ID="Label20" runat="server" Text="From:" Font-Bold="true" Font-Size="Small"></asp:Label>
                        &nbsp; &nbsp;<asp:Label ID="lblFromDt" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        &nbsp;
                        <asp:Label ID="Label21" runat="server" Text="To:" Font-Bold="true" Font-Size="Small"></asp:Label>
                        &nbsp;&nbsp;<asp:Label ID="lblToDt" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <table width="70%" align="center" cellpadding="3" cellspacing="1" style="border-bottom: 1px solid black;
                border-top: 1px solid black; border-left: 1px solid black; border-right: 1px solid black;
                table-layout: fixed;" class="printsmall">
                <tr>
                    <td align="center" style="width: 10%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label1" Text="Type" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label2" Text="No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label3" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 40%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label4" Text="Narration" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label5" Text="Debit" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label6" Text="Credit" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%; border-right: 1px solid black;">
                        <asp:Label runat="server" ID="label7" Text="Balance" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label runat="server" ID="label8" Text="DrCr" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="70%" align="center" style="border-bottom: 1px solid black; border-top: 1px solid black;
                border-left: 1px solid black; border-right: 1px solid black;" class="printsmall">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" style="border-bottom: 1px solid black;" class="printsmall">
                                    <tr>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label runat="server" ID="lblUnitCode" Text='<%#Eval("Unit_Code") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%;">
                                        </td>
                                        <td align="left" style="width: 50%;">
                                            <asp:Label runat="server" ID="lblUnitName" Text='<%#Eval("Unit_Name") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%;">
                                        </td>
                                        <td align="center" style="width: 10%;">
                                        </td>
                                        <td align="center" style="width: 10%;">
                                        </td>
                                        <td align="center" style="width: 10%;">
                                        </td>
                                    </tr>
                                </table>
                                <%--  <table width="100%" align="center">
                                    <tr>
                                        <td style="width: 100%;">--%>
                                <asp:DataList runat="server" ID="dtDetails" Width="100%" OnItemDataBound="dtDetails_OnItemDataBound">
                                    <ItemTemplate>
                                        <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;"
                                            class="printsmall">
                                            <tr>
                                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label9" Text='<%#Eval("TranType") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label10" Text='<%#Eval("DocNo") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label11" Text='<%#Eval("Date") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 40%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label12" Text='<%#Eval("Narration") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label13" Text='<%#Eval("Debit") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label14" Text='<%#Eval("Credit") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                                    <asp:Label runat="server" ID="label15" Text='<%#Eval("Balance") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 10%;">
                                                    <asp:Label runat="server" ID="label16" Text='<%#Eval("DrCr") %>' Font-Bold="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                                <table width="100%" align="center" style="table-layout: fixed; background-color: #CCFFFF;
                                    border-bottom: 1px solid black;" class="printsmall">
                                    <tr>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="label17" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="label19" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="label22" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 40%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="label23" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="lblUnitDebit" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="lblUnitCredit" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="lblUnitBalance" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%; border-right: 1px solid black;">
                                            <asp:Label runat="server" ID="lblUnitDrCr" Text="" Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <%-- </td>
                                    </tr>
                                </table>--%>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <table width="100%" align="center" style="border-bottom: 1px solid black; border-left: 1px solid black;
                            border-right: 1px solid black; background-color: #FFFFCC; table-layout: fixed;"
                            class="printsmall">
                            <tr>
                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="labssel17" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="label1sss9" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="label2ss2" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="left" style="width: 40%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="labessl23" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="lblUnitDebitAll" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="lblUnitCreditAll" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 10%; border-right: 1px solid black;">
                                    <asp:Label runat="server" ID="lblUnitBalanceAll" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td align="center" style="width: 10%;">
                                    <asp:Label runat="server" ID="lblUnitDrCrAll" Text="" Font-Bold="false"></asp:Label>
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
