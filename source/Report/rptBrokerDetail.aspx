<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptBrokerDetail.aspx.cs"
    Inherits="Report_rptBrokerDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=660,width=1360');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body class="largsize">');
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
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            OnClientClick="CheckEmail();" Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri" CssClass="largsize">
            <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" Width="90%" Text="Broker Detail Report" CssClass="lblName"
                Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <br />
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                border-bottom: 1px solid black;" class="largsize">
                <tr>
                    <td align="right">
                        <table align="left">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblBrokerName" Text="Broker" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblDate" Font-Bold="true" Text="Date"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                border-bottom: 1px solid black;" class="largsize">
                <tr>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblDoDate" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 2%;">
                        <asp:Label ID="lblDOType" runat="server" Text="Ref" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblDoNo" runat="server" Text="No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%;">
                        <asp:Label ID="lblGetpass" runat="server" Text="Customer Name" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblDoQntl" runat="server" Text="Quintal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblAmount" runat="server" Text="Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 8%;">
                        <asp:Label ID="lblMillName" runat="server" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" class="largsize"
                style="border-bottom: dashed 2px black;">
                <tr>
                    <td style="width: 100%" align="center">
                        <asp:DataList runat="server" Width="100%" ID="dtl" CellPadding="1" OnItemDataBound="DataList_ItemDataBound">
                            <ItemStyle VerticalAlign="Top" />
                            <ItemTemplate>
                                <table width="100%" align="center" cellpadding="2" cellspacing="0" style="table-layout: fixed;
                                    background-color: #CCFFFF;" class="largsize">
                                    <tr valign="top">
                                        <td align="center" style="width: 3%; vertical-align: top;">
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("DO_Date") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 2%; vertical-align: top;">
                                            <asp:Label ID="lblDo" runat="server" Text='<%# Eval("Do_Type") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 2%; vertical-align: top;">
                                            <asp:Label ID="lblDonumber" runat="server" Text='<%# Eval("Do_No") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%; vertical-align: top;">
                                            <asp:Label ID="lblDoGetPass" runat="server" Text='<%# Eval("Customer") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%; vertical-align: top;">
                                            <asp:Label ID="lblDOQuantal" runat="server" Text='<%# Eval("quantal") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%; vertical-align: top;">
                                            <asp:Label ID="lblDOAmount" runat="server" Text='<%# Eval("Amount") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 8%; vertical-align: top;">
                                            <asp:Label ID="lblDOMill" runat="server" Text='<%# Eval("Mill") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                background-color: #FFFFCC;" class="largsize">
                <tr>
                    <td align="right" style="width: 3%;">
                    </td>
                    <td align="right" style="width: 2%;">
                    </td>
                    <td align="center" style="width: 2%;">
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label4" runat="server" Text="Total:" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblQntlTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblAmountTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 8%;">
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
