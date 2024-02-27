<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptPartyUnitDetails.aspx.cs"
    Inherits="Report_rptPartyUnitDetails" %>

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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="btnPrint" OnClientClick="PrintPanel();" Text="Print" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" /></div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" align="center" Font-Names="Calibri">
            <table width="80%" align="center" style="table-layout: fixed;">
                <tr>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblPartyCode" runat="server" Text="Party Code" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 11%;">
                        <asp:Label ID="lblDispatchDate" runat="server" Text="Party Name" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 9%;">
                        <asp:Label ID="lblMill" runat="server" Text="Address" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 5%;">
                        <asp:Label ID="lblGrade" runat="server" Text="Mobile" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="80%" align="center">
                <tr>
                    <td style="border-bottom: 1px solid black;" width="100%">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" Width="100%" ID="dtlist" OnItemDataBound="dtlist_ItemDataBound">
                            <ItemTemplate>
                                <table width="100%" style="table-layout: fixed;" align="center">
                                    <tr>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lblPartyCode" runat="server" Text='<%#Eval("PartyCode") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 11%;">
                                            <asp:Label ID="lblDispatchDate" runat="server" Text='<%#Eval("PartyName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 9%;">
                                            <asp:Label ID="lblMill" runat="server" Text='<%#Eval("PartyAddress") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%;">
                                            <asp:Label ID="lblGrade" runat="server" Text='<%#Eval("PartyMobile") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <tr>
                                    <td style="border-bottom: 1px solid black;" width="100%">
                                    </td>
                                </tr>
                                <tr>
                                    <tr>
                                        <td style="width: 100%;" align="center">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td align="left" style="width: 3%;">
                                                        <asp:Label ID="Label1" runat="server" Text="Unit Code" Font-Bold="false"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 11%;">
                                                        <asp:Label ID="Label2" runat="server" Text="Unit Name" Font-Bold="false"></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 9%;">
                                                        <asp:Label ID="Label3" runat="server" Text="Address" Font-Bold="false"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 5%;">
                                                        <asp:Label ID="Label4" runat="server" Text="Mobile" Font-Bold="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-bottom: 1px solid black;" width="100%">
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                            <ItemTemplate>
                                                <table width="100%" align="center" style="table-layout: fixed;">
                                                    <tr>
                                                        <td align="left" style="width: 3%;">
                                                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("UnitCode") %>' Font-Bold="false"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 11%;">
                                                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("UnitName") %>' Font-Bold="false"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 9%;">
                                                            <asp:Label ID="Label7" runat="server" Text='<%#Eval("UnitAddress") %>' Font-Bold="false"></asp:Label>
                                                        </td>
                                                        <td align="center" style="width: 5%;">
                                                            <asp:Label ID="Label8" runat="server" Text='<%#Eval("UnitMobile") %>' Font-Bold="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: 1px solid black;" width="100%">
                                    </td>
                                </tr>
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
