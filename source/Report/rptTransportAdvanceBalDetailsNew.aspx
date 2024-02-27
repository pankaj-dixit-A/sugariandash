<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptTransportAdvanceBalDetailsNew.aspx.cs"
    Inherits="Report_rptTransportAdvanceBalDetailsNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print"/>');
            printWindow.document.write('</head><body >');
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
        <asp:Button runat="server" ID="btnPrint" OnClientClick="return PrintPanel();" Text="Print" />
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" CssClass="print2">
            <table width="90%" align="center" cellspacing="4" style="table-layout: fixed;" class="print2">
                <tr>
                    <td style="width: 100%; text-align: center;">
                        <asp:Label runat="server" ID="lblCmpName" ForeColor="Navy" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: center;">
                        <asp:Label runat="server" ID="lblReportName" ForeColor="Black" Text="Trnasport Balance Report"
                            Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; text-align: left; border-bottom: 2px solid black;">
                        <asp:Label runat="server" ID="lblTransportName" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellspacing="1" style="table-layout: fixed; border-bottom: 2px solid black;"
                class="print2">
                <tr>
                    <td style="width: 2%;">
                        <asp:Label runat="server" ID="lblNo" Text="Tran.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 2%;">
                        <asp:Label runat="server" ID="llbType" Text="Tran.Type" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lbldt" Text="Tran.Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 10%;" align="center">
                        <asp:Label runat="server" ID="lblMill" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="lblQntl" Text="Qntl" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblAdvance" Text="Advance" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblFrtPaid" Text="Frieght Paid" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblPaid" Text="Total Paid" Font-Bold="true"> </asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblBal" Text="Balance" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" class="print2">
                <tr>
                    <td style="width: 100%;" align="center">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table id="Table1" runat="server" width="100%" style="background-color: #FFFFCC;
                                    table-layout: fixed;" class="print2">
                                    <tr>
                                        <td style="width: 2%;">
                                            <asp:Label runat="server" ID="lblTransportCode" Text='<%#Eval("TransportCode") %>'
                                                Font-Bold="false" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 16%;">
                                            <asp:Label runat="server" ID="llbType" Text='<%#Eval("TransportName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 3%;" align="center">
                                            <asp:Label runat="server" ID="lblAllQntlTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllAdvanceTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllFrieghtPaid" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllPaidTotal" Text="" Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllBalTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table3" runat="server" width="100%" class="print2">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList runat="server" Width="100%" ID="dtlDoDetails" OnItemDataBound="dtlDoDetails_OnItemDataBound">
                                                <ItemTemplate>
                                                    <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;"
                                                        class="print2">
                                                        <tr>
                                                            <td style="width: 2%;">
                                                                <asp:Label runat="server" ID="lblDONo" Text='<%#Eval("DO") %>' Font-Bold="false"></asp:Label>&nbsp;&nbsp;
                                                                <asp:Label runat="server" ID="lblTran_Type" Text='<%#Eval("tran_type") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 5%;" align="center">
                                                                <%-- <asp:Label runat="server" ID="lblParty" Text='<%#Eval("PartyShortName") %>' Font-Bold="false"></asp:Label>--%>
                                                            </td>
                                                            <td style="width: 5%;" align="center">
                                                                <%--<asp:Label runat="server" ID="Label2" Text='<%#Eval("GetpassShortName") %>' Font-Bold="false"></asp:Label>--%>
                                                            </td>
                                                            <td style="width: 6%;">
                                                                <asp:Label runat="server" ID="Label1" Text='<%#Eval("millShortName") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="lblQntl" Text='<%#Eval("Qntl") %>' Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblAdvance" Text='<%#Eval("Advance") %>' Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblFrieghtPaid" Text='<%#Eval("FrtPaid") %>' Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblPaidTotal" Text='<%#Eval("Paid") %>' Font-Bold="true"> </asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblBalTotal" Text='<%#Eval("Balance") %>' Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="Table2" runat="server" width="100%" class="print2">
                                                        <tr>
                                                            <td style="width: 100%;">
                                                                <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                                    <ItemTemplate>
                                                                        <table width="100%" cellspacing="1" align="center" style="table-layout: fixed; font-size: 11pt;
                                                                            background-color: #CCFFFF; border-bottom: 1px dashed black;" class="print2">
                                                                            <tr>
                                                                                <td style="width: 2%;">
                                                                                    <asp:Label runat="server" ID="lblNo" Text='<%#Eval("#") %>' Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 2%;">
                                                                                    <asp:Label runat="server" ID="llbType" Text='<%#Eval("ttype") %>' Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;" align="center">
                                                                                    <asp:Label runat="server" ID="lbldt" Text='<%#Eval("dt") %>' Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 13%;">
                                                                                    <asp:Label runat="server" ID="lblMill" Text='<%#Eval("narration") %>' Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;" align="center">
                                                                                    <asp:Label runat="server" ID="lblAdvance" Text="" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;" align="center">
                                                                                    <asp:Label runat="server" ID="Label2" Text="" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;" align="center">
                                                                                    <asp:Label runat="server" ID="lblPaid" Text='<%#Eval("Paid") %>' Font-Bold="false"> </asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;" align="center">
                                                                                    <asp:Label runat="server" ID="lblBal" Text="" Font-Bold="false"></asp:Label>
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
