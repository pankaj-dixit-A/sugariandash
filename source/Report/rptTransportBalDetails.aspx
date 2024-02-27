<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptTransportBalDetails.aspx.cs"
    Inherits="Report_rptTransportBalDetails" %>

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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnPrint" OnClientClick="PrintPanel();" Text="Print" />
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
            <table width="90%" align="center" cellspacing="1" style="table-layout: fixed; border-bottom: 2px solid black;">
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
                        <asp:Label runat="server" ID="lblPaid" Text="Paid" Font-Bold="true"> </asp:Label>
                    </td>
                    <td style="width: 4%;" align="center">
                        <asp:Label runat="server" ID="lblBal" Text="Balance" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center">
                <tr>
                    <td style="width: 100%;" align="center">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table id="Table1" runat="server" width="100%" style="background-color: #FFFFCC;
                                    table-layout: fixed;">
                                    <tr>
                                        <td style="width: 2%;">
                                            <asp:Label runat="server" ID="lblTransportCode" Text='<%#Eval("TransportCode") %>'
                                                Font-Bold="false" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Label runat="server" ID="llbType" Text='<%#Eval("TransportName") %>' Font-Bold="true"
                                                Style="font-size: 12pt;"></asp:Label>
                                        </td>
                                        <%-- <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lbldt" Text="" Font-Bold="false"></asp:Label>
                                        </td>--%>
                                        <td style="width: 6%;">
                                            <asp:Label runat="server" ID="lblMill" Text='<%#Eval("MillShort") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td style="width: 3%;" align="center">
                                            <asp:Label runat="server" ID="lblQntl" Text='<%#Eval("Qntl") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblAdvance" Text='<%#Eval("Advance") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblPaidTotal" Text="" Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 4%;" align="center">
                                            <asp:Label runat="server" ID="lblBalTotal" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table2" runat="server" width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%" cellspacing="1" align="center" style="table-layout: fixed; font-size: 11pt;
                                                        background-color: #CCFFFF;">
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
                                                            <td style="width: 10%;">
                                                                <asp:Label runat="server" ID="lblMill" Text="" Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 3%;" align="center">
                                                                <asp:Label runat="server" ID="lblQntl" Text="" Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 4%;" align="center">
                                                                <asp:Label runat="server" ID="lblAdvance" Text="" Font-Bold="false"></asp:Label>
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
                                    <tr>
                                        <td style="width: 100%; border-top: 1px dashed black;">
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
