<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptGroupLedger.aspx.cs" Inherits="Report_rptGroupLedger" %>

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
            Width="80px" OnClick="btnPrint_Click" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSendEmail" runat="server" Text="Send To Email" CssClass="btnHelp"
            OnClick="btnSendEmail_Click" />&nbsp;&nbsp; Email:<asp:TextBox runat="server" ID="txtEmail"
                Width="200px"></asp:TextBox>&nbsp;&nbsp;<asp:Button runat="server" ID="btpGrdprint"
                    Text="PrintGrid" OnClick="btpGrdprint_Click" Visible="false" />
    </div>
    <div>
        <asp:Panel ID="PrintPanel" runat="server" align="center" Font-Names="Calibri">
            <table width="68%" align="center" class="printwithmargin" id="tblMain" runat="server"
                style="table-layout: fixed;" cellspacing="5">
                <tr>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70%;" align="left">
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Account Statement of :" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lblParty" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label><asp:Label
                            ID="lblAcCode" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="Label2" runat="server" Text="From:" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        &nbsp; &nbsp;<asp:Label ID="lblFromDt" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        &nbsp;
                        <asp:Label ID="Label3" runat="server" Text="To:" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        &nbsp;&nbsp;<asp:Label ID="lblToDt" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="White"
                            GridLines="Both" HeaderStyle-ForeColor="Black" HeaderStyle-Height="30px" EmptyDataText="No Records found"
                            Width="1000px" CellPadding="9" CellSpacing="0" Font-Bold="false" ForeColor="Black"
                            RowStyle-Height="30px" ShowFooter="true" Font-Names="Verdana" Font-Size="15px"
                            OnRowDataBound="grdDetail_RowDataBound" CssClass="print">
                            <Columns>
                                <asp:BoundField DataField="TranType" HeaderText="Type" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="40px" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="DocNo" HeaderText="No" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="50px" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="Date" HeaderText="Date" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="60px" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="Narration" HeaderText="Narration" ItemStyle-HorizontalAlign="Left"
                                    ItemStyle-VerticalAlign="Bottom" ItemStyle-Width="800px" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="Debit" HeaderText="Debit" ItemStyle-HorizontalAlign="Right"
                                    ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="Credit" HeaderText="Credit" ItemStyle-HorizontalAlign="Right"
                                    ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="Balance" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                                    ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="thead" />
                                <asp:BoundField DataField="DrCr" HeaderText="DrCr" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="50px" HeaderStyle-CssClass="thead" />
                            </Columns>
                            <RowStyle Height="10px" Wrap="true" ForeColor="Black" />
                            <FooterStyle BackColor="Yellow" Font-Bold="true" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
