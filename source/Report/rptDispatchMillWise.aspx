<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDispatchMillWise.aspx.cs"
    Inherits="Report_rptDispatchMillWise" %>

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
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnPrint" OnClientClick="PrintPanel();" Text="Print" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            OnClientClick="CheckEmail();" Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri">
            <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" Width="90%" Text="MILL WISE DISPATCH" CssClass="lblName"
                Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <table cellpadding="1" cellspacing="0" width="80%" align="center">
                <tr>
                    <td align="center" style="width: 4%;">
                        <asp:Label ID="lblDate" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblQuintal" runat="server" Text="Quintal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="lblMillRate" runat="server" Text="Mill Rate" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="lblAmount" runat="server" Text="Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%;">
                        <asp:Label ID="lblNarration" runat="server" Text="Narration" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <div align="left">
                <asp:DataList runat="server" ID="dtl" Width="80%" align="center" OnItemDataBound="DataList_itemDataBound">
                    <ItemTemplate>
                        <table align="center" width="100%">
                            <tr>
                                <td style="width: 30%;" align="right">
                                    <asp:Label runat="server" ID="lblMillCode" Text="millcode"></asp:Label>
                                </td>
                                <td style="width: 70%;" align="left">
                                    <asp:Label runat="server" ID="lblMillName" Text="millname" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="11">
                                    <table width="100%" align="center" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td colspan="9">
                                                <%--<asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                    <ItemTemplate>
                                                        <table width="100%" align="center" cellpadding="1" cellspacing="0">
                                                            <tr>
                                                                <td align="left" style="width: 10%;">
                                                                    <asp:Label runat="server" ID="lblDetailid" Text="<%#Eval("Detail_ID") %>"></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 5%;">
                                                                    <asp:Label runat="server" ID="lblDate" Text="<%#Eval("D_Date") %>"></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 8%;">
                                                                    <asp:Label runat="server" ID="lblQntl" Text="<%#Eval("Quantal") %>"></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 9%;">
                                                                    <asp:Label runat="server" ID="lblMillrate" Text="<%#Eval("Mill_Rate") %>"></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 12%;">
                                                                    <asp:Label runat="server" ID="lblAmount" Text="<%#Eval("Amount") %>"></asp:Label>
                                                                </td>
                                                                <td align="left" style="width: 15%;">
                                                                    <asp:Label runat="server" ID="lblNarration" Text="<%#Eval("Narration") %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
