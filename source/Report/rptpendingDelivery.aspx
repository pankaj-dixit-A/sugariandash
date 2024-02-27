<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptpendingDelivery.aspx.cs"
    Inherits="Report_rptpendingDelivery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Delivery</title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body style="font-family:Calibri;font-size:12px;width:1100px; text-align:center;" >');
            printWindow.document.write('<style type = "text/css">thead {display:table-header-group; } tfoot{display:table-footer-group;}</style>');
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
    <div align="left">
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;
        <asp:Button runat="server" ID="btnSendEmail" Text="Mail" OnClick="btnSendEmail_Click"
            Width="73px" />
        Email:<asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel ID="PrintPanel" runat="server" align="center" Font-Names="Calibri">
            <table width="1000px" align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true"
                            Font-Size="Large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="Label1" runat="server" Text="Pending Delivery List" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
            <center>
                <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#397CBB"
                    HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" EmptyDataText="No Records found"
                    Width="1000px" CellPadding="0" CellSpacing="0" Font-Bold="false" ForeColor="Black"
                    ShowFooter="true" Font-Names="Verdana" Font-Size="12px" OnRowDataBound="grdDetail_RowDataBound"
                    GridLines="None" BorderStyle="Solid" BorderColor="Black" BorderWidth="1">
                    <Columns>
                        <asp:BoundField DataField="Tender_No" HeaderText="Tender NO" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="Tender_Date" HeaderText="Tender Date" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="millname" HeaderText="Mill Name" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="Buyer_Quantal" HeaderText="Party Qntl" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="salerate" HeaderText="SaleRate" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="salevalue" HeaderText="Amount" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="receivedDate" HeaderText="Received Date" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="received" HeaderText="Received Amount" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-CssClass="thead" />
                        <asp:BoundField DataField="balance" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-CssClass="thead" />
                    </Columns>
                    <RowStyle Height="10px" Wrap="true" ForeColor="Black" />
                    <FooterStyle BackColor="Yellow" Font-Bold="true" />
                    <HeaderStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1" />
                </asp:GridView>
            </center>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
