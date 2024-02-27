<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptsalebill.aspx.cs" Inherits="Reports_rptsalebill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sale Bill </title>
    <link href="../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlReport.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=400,width=800');
            printWindow.document.write('<html><head>');
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
    <div align="center">
        <div style="width: 90%;" align="left">
            <asp:Button ID="btnPrint" runat="server" Text="Print" Width="80px" OnClientClick="return PrintPanel();" />
            &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
                Width="120px" OnClick="btnExportToExcel_Click" />
            &nbsp; &nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Mail" OnClick="btnSendEmail_Click"
                Width="58px" />
            Email:<asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
        </div>
        <asp:Panel ID="pnlReport" runat="server" Font-Names="Verdana" align="center" Width="90%">
            <asp:DataList ID="dtl" Width="100%" runat="server" OnItemDataBound="dtl_ItemDataBound"
                Style="text-align: center;">
                <ItemTemplate>
                    <table width="100%" align="center" style="page-break-after: always;">
                        <tr>
                            <td colspan="2" align="center" style="width: 100%;">
                                <asp:Label ID="lblCompanyName" runat="server" Font-Bold="true" Font-Size="14px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="width: 100%;">
                                <asp:Label ID="lblCompanyAddress" runat="server" Font-Bold="true" Font-Size="12px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="width: 100%;">
                                <asp:Label ID="lblPhone" runat="server" Font-Bold="true" Font-Size="15px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-top: solid 3px black; border-bottom: solid 2px black;
                                width: 100%;">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label11" Text="Bill No.:" runat="server" Font-Size="15px"></asp:Label>
                                            <asp:Label ID="lblbillNo" Text='<%#Eval("doc_no") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label12" Text="Date:" runat="server" Font-Size="15px"></asp:Label>
                                            <asp:Label ID="lblDt" runat="server" Text='<%#Eval("doc_date") %>' Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 60%;">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label13" Text="To," runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 60%;">
                                            <asp:Label ID="lblpartyname" runat="server" Text='<%#Eval("PartyName") %>' Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 60%;">
                                            <asp:Label ID="lblpartyAddress" runat="server" Text='<%#Eval("partyAddress") %>'
                                                Font-Bold="true" Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 60%;">
                                            <asp:Label ID="lblpartycity" runat="server" Text='<%#Eval("PartyCity") %>' Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 100%;">
                                            <asp:Label ID="Label14" Text="From:" runat="server" Font-Size="15px"></asp:Label><asp:Label
                                                ID="lblFrom" runat="server" Text='<%#Eval("FROM_STATION") %>' Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                            <asp:Label ID="Label15" Text="To:" runat="server" Font-Size="15px"></asp:Label><asp:Label
                                                ID="lblTo" runat="server" Text='<%#Eval("TO_STATION") %>' Font-Bold="true" Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" Text="Broker:" runat="server" Font-Size="15px"></asp:Label><asp:Label
                                                ID="lblBroker" Text='<%#Eval("brokername") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right" rowspan="6" valign="top" style="width: 40%;">
                                <asp:Label ID="Label17" Text="TIN NO.:" runat="server" Font-Size="15px"></asp:Label><asp:Label
                                    ID="lblTin" Text='<%#Eval("partyTin") %>' runat="server" Font-Bold="true" Font-Size="15px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2" style="width: 100%;">
                                <table width="100%">
                                    <tr>
                                        <td colspan="4" style="width: 100%; border-bottom: solid 1px black;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 30%;">
                                            <asp:Label ID="Label18" Text="Mill Name/Item Description" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%;">
                                            <asp:Label ID="Label19" Text="Bags" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%;">
                                            <asp:Label ID="Label20" Text="Rate" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="Label21" Text="Value" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="width: 100%; border-bottom: solid 1px black;">
                                        </td>
                                    </tr>
                                    <tr style="border-top: 1px solid black; border-bottom: 1px solid black;">
                                        <td align="left" style="width: 30%;">
                                            <asp:Label ID="Label1" Text='<%#Eval("millname") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%;">
                                        </td>
                                        <td align="left" style="width: 30%;">
                                        </td>
                                        <td align="left" style="width: 10%;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="width: 100%;">
                                            <asp:DataList ID="dtlItems" runat="server" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%" align="left">
                                                        <td align="left" style="width: 30%;">
                                                            <asp:Label ID="Label1" Text='<%#Eval("itemname") %>' runat="server" Font-Bold="true"
                                                                Font-Size="15px"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 30%;">
                                                            <asp:Label ID="Label2" Text='<%#Eval("bags") %>' runat="server" Font-Bold="true"
                                                                Font-Size="15px"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 30%;">
                                                            <asp:Label ID="Label3" Text='<%#Eval("rate") %>' runat="server" Font-Bold="true"
                                                                Font-Size="15px"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 10%;">
                                                            <asp:Label ID="Label4" Text='<%#Eval("item_Amount") %>' runat="server" Font-Bold="true"
                                                                Font-Size="15px"></asp:Label>
                                                        </td>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label29" Text="Bank Commission:" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" Text='<%#Eval("cash_advance") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label22" Text="Bank Commission:" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" Text='<%#Eval("bank_commission") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%;">
                                <table width="100%">
                                    <tr>
                                        <td colspan="4" style="width: 100%; border-bottom: solid 1px black;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" Text="Sale Rate" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label24" Text="Less Freight" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label25" Text="Bill Amount" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" Text='<%#Eval("bank_commission") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label8" Text='<%#Eval("LESS_FRT_RATE") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                        <%--<td>
                                            <asp:Label ID="Label9" Text='<%#Eval("rate") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>--%>
                                        <td>
                                            <%--<asp:Label ID="Label26" Text="Bill Amount:" runat="server" Font-Size="15px"></asp:Label>--%>
                                            <asp:Label ID="Label10" Text='<%#Eval("Bill_Amount") %>' runat="server" Font-Bold="true"
                                                Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="width: 100%; border-bottom: solid 1px black;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label27" Text="Rs. In Words:" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label28" Text="For" runat="server" Font-Size="15px"></asp:Label>
                                            <asp:Label ID="companynamebottom" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
