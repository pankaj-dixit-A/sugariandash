<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptTransportAdvanceOnlyBalNew.aspx.cs"
    Inherits="Report_rptTransportAdvanceOnlyBalNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
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
        <asp:Panel runat="server" ID="pnlMain" CssClass="toosmall">
            <table width="90%" align="center" cellspacing="4" class="toosmall">
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
                class="toosmall">
                <tr>
                    <td style="width: 2%;">
                        <asp:Label runat="server" ID="lblNo" Text="Voc.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 2%;">
                        <asp:Label runat="server" ID="llbType" Text="Voc.Type" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lbldt" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="left">
                        <asp:Label runat="server" ID="lblMill" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 10%;" align="center">
                        <asp:Label runat="server" ID="Label1" Text="Dispatch To" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="Label2" Text="Lorry No" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 3%;" align="center">
                        <asp:Label runat="server" ID="lblQntl" Text="Qntl" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblAdvance" Text="Advance" Font-Bold="true"></asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblPaid" Text="Paid" Font-Bold="true"> </asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblBal" Text="Balance" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" class="toosmall">
                <tr>
                    <td style="width: 100%;" align="center">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" style="background-color: #FFFFCC;table-layout:fixed;" class="toosmall">
                                    <tr>
                                        <td style="width: 2%;">
                                            <asp:Label runat="server" ID="lblTransportCode" Text='<%#Eval("TransportCode") %>'
                                                Font-Bold="false" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 16%;">
                                            <asp:Label runat="server" ID="llbType" Text='<%#Eval("TransportName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="Label5" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="Label6" Text="" Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 3%;" align="center">
                                            <asp:Label runat="server" ID="lblAllQntlTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllAdvanceTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllPaidTotal" Text="" Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAllBalTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="1" class="toosmall">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table id="tbl2" width="100%" align="center" cellspacing="3" style="table-layout: fixed;
                                                        font-size: 11pt; background-color: #CCFFFF; border-top: 1px dashed black;" class="toosmall">
                                                        <tr>
                                                            <td style="width: 2%;">
                                                                <asp:Label runat="server" ID="lblNos" Text='<%#Eval("#") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 2%;">
                                                                <asp:Label runat="server" ID="llbTypes" Text='<%#Eval("ttype") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lbldts" Text='<%#Eval("dt") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="left" class="toosmall">
                                                                <asp:Label runat="server" ID="lblMills" Text='<%#Eval("Mill") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="Label3" Text='<%#Eval("DispatchTo") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="Label4" Text='<%#Eval("lorry") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="lblQntls" Text='<%#Eval("Qntl") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblAdvances" Text='<%#Eval("Advance") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblPaids" Text='<%#Eval("Paid") %>' Font-Bold="false"> </asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblBals" Text='<%#Eval("Balance") %>' Font-Bold="false"></asp:Label>
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
