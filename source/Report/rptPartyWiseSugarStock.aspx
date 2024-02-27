<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptPartyWiseSugarStock.aspx.cs"
    Inherits="Report_rptPartyWiseSugarStock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', 'st', 'height=400,width=800');
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

        function pst() {
            window.open('../Report/rptSugarBalanceStocks.aspx');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnPrint" OnClientClick="PrintPanel();" Text="Print" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain">
            <table width="90%" align="center" cellspacing="4">
                <tr>
                    <td style="width: 100%; text-align: center;">
                        <asp:Label runat="server" ID="lblCmpName" ForeColor="Navy" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: center;">
                        <asp:Label runat="server" ID="lblReportName" ForeColor="Black" Text="Partywise Sugar Balance Stock"
                            Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: left;">
                        <asp:Label runat="server" ID="lblTransportName" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellspacing="1" style="table-layout: fixed; border-top: 2px solid black;">
                <tr>
                    <td style="width: 6%;" align="left">
                        <asp:Label runat="server" ID="label1" Text="MillName" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 5%;" align="center">
                        <asp:Label runat="server" ID="label2" Text="Grade" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="label3" Text="M Rate" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="label4" Text="S Rate" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="label5" Text="Lifting" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="left">
                        <asp:Label runat="server" ID="label6" Text="Do" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="label7" Text="Quintal" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="label8" Text="Desp" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="label9" Text="Bal" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellspacing="4" style="border-top: 2px solid black;">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" cellpadding="0" cellspacing="1" align="center" style="background-color: #FFFFCC;
                                    table-layout: fixed; border-bottom: 1px solid black;">
                                    <tr>
                                        <td style="width: 2%;" align="left">
                                            <asp:Label runat="server" ID="lblPartyCode" Text='<%#Eval("PartyCode") %>' Visible="false"> </asp:Label>
                                        </td>
                                        <td style="width: 62%;" align="left">
                                            <asp:Label runat="server" ID="lblPartyName" Text='<%#Eval("PartyName") %>' Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 7%;" align="center">
                                            <asp:Label runat="server" ID="lblDispTotal" Text="" Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 7%;" align="center">
                                            <asp:Label runat="server" ID="lblBalTotal" Text="" Font-Bold="true"> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" cellpadding="0" cellspacing="3" align="center">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; background-color: #CCFFFF;
                                                        border-bottom: 1px dashed black;">
                                                        <tr>
                                                            <td style="width: 6%;" align="left">
                                                                <asp:Label runat="server" ID="label1" Text='<%#Eval("MillName") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 5%;" align="center">
                                                                <asp:Label runat="server" ID="label2" Text='<%#Eval("Grade") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="label3" Text='<%#Eval("MR") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="label4" Text='<%#Eval("SR") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="label5" Text='<%#Eval("LD") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="left">
                                                                <asp:Label runat="server" ID="label6" Text='<%#Eval("TDO") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="label7" Text='<%#Eval("Qntl") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="label8" Text='<%#Eval("Disp") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="label9" Text='<%#Eval("Balance") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;
                            border-top: 1px solid black;">
                            <tr>
                                <td style="width: 6%;" align="left">
                                    <asp:Label runat="server" ID="label10" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 5%;" align="center">
                                    <asp:Label runat="server" ID="label11" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 3%;" align="center">
                                    <asp:Label runat="server" ID="label12" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 3%;" align="center">
                                    <asp:Label runat="server" ID="label13" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 3%;" align="center">
                                    <asp:Label runat="server" ID="label14" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 4%;" align="right">
                                    <asp:Label runat="server" ID="label15" Text="Grand Total:-" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 3%;" align="center">
                                    <asp:Label runat="server" ID="lblQntlGrandTotal" Text="Qntltotal" Font-Bold="true"></asp:Label>
                                </td>
                                <td style="width: 3%;" align="center">
                                    <asp:Label runat="server" ID="label16" Text="" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 3%;" align="center">
                                    <asp:Label runat="server" ID="label17" Text="" Font-Bold="false"></asp:Label>
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
