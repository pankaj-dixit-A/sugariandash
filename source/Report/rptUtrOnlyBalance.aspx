<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptUtrOnlyBalance.aspx.cs"
    Inherits="Report_rptUtrOnlyBalance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body class="print">');
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
        <asp:Button runat="server" ID="btnPrint" Text="Print" OnClientClick="PrintPanel();" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            OnClientClick="CheckEmail();" Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" CssClass="print">
            <table width="80%" align="center" style="table-layout: fixed;" class="print">
                <tr>
                    <td align="center">
                        <asp:Label runat="server" ID="lblCmpName" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label runat="server" ID="lblReport" Text="Utr Report" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblAcName"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblFromTo"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="80%" align="center" style="table-layout: fixed; border-bottom: 1px Solid black;
                border-top: 1px Solid black;" class="print">
                <tr>
                    <td style="width: 5%;" align="center">
                        Doc_No
                    </td>
                    <td style="width: 9%;" align="center">
                        Date
                    </td>
                    <td style="width: 12%;" align="left">
                        Utr.No/Ref.No
                    </td>
                    <td style="width: 28%;" align="left">
                        Mill Name
                    </td>
                    <td style="width: 17%;" align="left">
                        Bank
                    </td>
                    <td style="width: 13%;" align="center">
                        Balance
                    </td>
                </tr>
            </table>
            <table width="80%" align="center" class="print">
                <tr>
                    <td style="width: 100%">
                        <asp:DataList runat="server" ID="dtlist" Width="100%">
                            <ItemTemplate>
                                <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px dashed black;"
                                    cellspacing="3" class="print">
                                    <tr>
                                        <td style="width: 5%;" align="center">
                                            <asp:Label runat="server" ID="Label1" Text='<%#Eval("#") %>'></asp:Label>
                                        </td>
                                        <td style="width: 9%;" align="center">
                                            <asp:Label runat="server" ID="lblDate" Text='<%#Eval("dt") %>'></asp:Label>
                                        </td>
                                        <td style="width: 12%; word-wrap: break-word; text-wrap: normal;" align="left">
                                            <asp:Label runat="server" ID="lblUtrNo" Text='<%#Eval("UTRNo") %>'></asp:Label>
                                        </td>
                                        <td style="width: 28%;" align="left">
                                            <asp:Label runat="server" ID="lblMillName" Text='<%#Eval("Mill") %>'></asp:Label>
                                        </td>
                                        <td style="width: 17%;" align="left">
                                            <asp:Label runat="server" ID="lblBank" Text='<%#Eval("Bank") %>'></asp:Label>
                                        </td>
                                        <td style="width: 13%;" align="center">
                                            <asp:Label runat="server" ID="lblAmount" Text='<%#Eval("balance") %>'></asp:Label>
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
