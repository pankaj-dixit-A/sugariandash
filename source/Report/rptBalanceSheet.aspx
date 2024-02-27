﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptBalanceSheet.aspx.cs"
    Inherits="Report_rptBalanceSheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <title>Balance Sheet</title>
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body style="font-family:Calibri;font-size:15px;text-align:center;">');
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
        <div align="left">
            <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
                Width="80px" />
            &nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Mail" OnClick="btnSendEmail_Click"
                OnClientClick="CheckEmail();" Width="77px" />
            &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
            <asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel" Width="120px"
                OnClick="btnExportToExcel_Click" />
        </div>
        <div>
            <asp:Panel ID="PrintPanel" runat="server" align="center" Font-Names="Calibri" CssClass="largsize"
                Height="100%">
                <asp:Label ID="Label3" runat="server" Text="Balance Sheet" Width="100%" CssClass="largsize"
                    Font-Bold="true" Font-Size="Larger" Style="text-align: center;"></asp:Label>
                <table id="tbMain" runat="server" width="1000px" align="center" style="page-break-after: avoid;"
                    class="largsize">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblCompanyAddr" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td align="center">
                            <asp:Label ID="lblCompanyMobile" runat="server" Text="" ></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="1000px" align="center" class="largsize">
                    <tr>
                        <td style="width: 50%; border-right: solid 1px black; border-left: solid 1px black;">
                            <table width="100%" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%; border-bottom: 1px solid black; border-top: 1px solid black;">
                                        <b>Liabilities</b>
                                    </td>
                                    <td align="right" style="width: 30%; border-bottom: 1px solid black; border-top: 1px solid black;">
                                        <b>Amount</b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" style="width: 50%; border-right: solid 1px black;">
                            <table width="100%" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%; border-bottom: 1px solid black; border-top: 1px solid black;">
                                        <b>Asset</b>
                                    </td>
                                    <td align="right" style="width: 30%; border-bottom: 1px solid black; border-top: 1px solid black;">
                                        <b>Amount</b>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 50%; border-right: solid 1px black; border-left: solid 1px black;">
                            <%-------------------------  Left Datalist ------------------------%>
                            <asp:DataList ID="dtl_Left" runat="server" Width="100%" OnItemDataBound="dtLeft_ItemDataBound">
                                <ItemTemplate>
                                    <table width="100%" class="largsize">
                                        <tr>
                                            <td align="left" style="width: 70%;">
                                                <asp:Label ID="lblGroupCodeL" runat="server" Text='<%#Eval("Group_Code") %>' Font-Bold="true"
                                                    Visible="false" Font-Size="11px"></asp:Label><asp:Label ID="lblGroupNameR" runat="server"
                                                        Style="text-transform: uppercase; font-size: medium; text-decoration: underline;"
                                                        Text='<%#Eval("groupname") %>' Font-Bold="true" Font-Size="12px"></asp:Label>&nbsp;<asp:Label
                                                            ID="lblsummaryL" runat="server" Font-Size="11px" Text='<%#Eval("summary") %>'
                                                            Font-Bold="true" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 30%;">
                                                <asp:Label ID="lblGroupAmount" runat="server" Text='<%#Eval("groupamount") %>' Font-Bold="true"
                                                    Style="text-transform: uppercase; font-size: medium;" Font-Size="14px"></asp:Label>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataList ID="dtLeftInner" runat="server" Width="100%">
                                        <ItemTemplate>
                                            <table width="100%" class="largsize">
                                                <tr>
                                                    <td align="left" style="width: 70%;">
                                                        <asp:Label ID="lblAcNameL" runat="server" Text='<%#Eval("acname") %>' Font-Bold="false"
                                                            Font-Size="12px"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 30%; padding-right: 80px;">
                                                        <asp:Label ID="lblACAmountL" runat="server" Text='<%#Eval("acamount") %>' Font-Bold="false"
                                                            Font-Size="12px"></asp:Label>
                                                        <%--<asp:Label ID="lblblank" runat="server" Width="80px"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ItemTemplate>
                            </asp:DataList>
                            <%-------------------------  Left Datalist ------------------------%>
                        </td>
                        <td valign="top" style="width: 50%; border-right: solid 1px black;">
                            <%-------------------------  Right Datalist ------------------------%>
                            <asp:DataList ID="dtl_Right" runat="server" Width="100%" OnItemDataBound="dtl_Right_ItemDataBound">
                                <ItemTemplate>
                                    <table width="100%" class="largsize">
                                        <tr>
                                            <td align="left" style="width: 70%;">
                                                <asp:Label ID="lblGroupCodeR" runat="server" Text='<%#Eval("Group_Code") %>' Font-Bold="true"
                                                    Visible="false"></asp:Label><asp:Label ID="lblGroupNameR" runat="server" Font-Size="12px"
                                                        Style="text-transform: uppercase; font-size: medium; text-decoration: underline;"
                                                        Text='<%#Eval("groupname") %>' Font-Bold="true"></asp:Label>&nbsp;<asp:Label ID="lblsummaryR"
                                                            runat="server" Text='<%#Eval("summary") %>' Style="text-transform: uppercase;
                                                            font-size: medium;" Font-Bold="true" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 30%;">
                                                <asp:Label ID="lblGroupAmountR" runat="server" Text='<%#Eval("groupamount") %>' Font-Bold="true"
                                                    Style="text-transform: uppercase; font-size: medium;" Font-Size="14px"></asp:Label>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:DataList ID="dtRightInner" runat="server" Width="100%">
                                        <ItemTemplate>
                                            <table width="100%" class="largsize">
                                                <tr>
                                                    <td align="left" style="width: 70%;">
                                                        <asp:Label ID="lblAcNameR" runat="server" Text='<%#Eval("acname") %>' Font-Bold="false"
                                                            Font-Size="12px"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 30%; padding-right: 80px;">
                                                        <asp:Label ID="lblACAmountR" runat="server" Text='<%#Eval("acamount") %>' Font-Bold="false"
                                                            Font-Size="12px"></asp:Label><%--<asp:Label ID="lblblank" runat="server" Width="80px"></asp:Label>--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ItemTemplate>
                            </asp:DataList>
                            <%-------------------------  Right Datalist ------------------------%>
                        </td>
                    </tr>
                    <%------------------------------- Footer Part ----------------------%>
                    <tr>
                        <td colspan="2" style="width: 100%; border-bottom: solid 1px black;">
                        </td>
                    </tr>
                    <%-----------------------------Group Total ------------------------%>
                    <tr>
                        <td align="right" style="width: 50%; border-right: solid 1px black; border-left: solid 1px black;">
                            <table width="100%" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%;">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Total:" Style="text-decoration: underline;"
                                            Font-Size="16px"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 30%;">
                                        <asp:Label ID="lblNetCredit" runat="server" Font-Bold="true" Text="Net Credit" Font-Size="16px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="width: 50%; border-right: solid 1px black;">
                            <table width="100%" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%;">
                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Total:" Style="text-decoration: underline;"
                                            Font-Size="16px"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 30%;">
                                        <asp:Label ID="lblNetDebit" runat="server" Font-Bold="true" Text="Net debit" Font-Size="16px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-----------------------------Net Profit & loss ------------------------%>
                    <tr>
                        <td align="right" style="width: 50%; border-right: solid 1px black; min-height: 50px;
                            table-layout: fixed; border-left: solid 1px black;">
                            <table width="100%" style="table-layout: fixed; height: 40px;" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%; border-bottom: solid 1px black;">
                                        <asp:Label ID="lblnetprofithead" runat="server" Font-Bold="true" Text="Net Profit"
                                            Font-Size="16px" Style="text-decoration: underline;"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 30%; border-bottom: solid 1px black;">
                                        <asp:Label ID="lblnetProfit" runat="server" Font-Bold="true" Text="Net Profit" Font-Size="16px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="width: 50%; border-right: solid 1px black; min-height: 50px;
                            table-layout: fixed; vertical-align: bottom;">
                            <table width="100%" style="table-layout: fixed; height: 40px; vertical-align: bottom;"
                                class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%; border-bottom: solid 1px black;">
                                        <asp:Label ID="lblnetlosshead" runat="server" Font-Bold="true" Text="Net Loss" Font-Size="16px"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 30%; border-bottom: solid 1px black;">
                                        <asp:Label ID="lblnetLoss" runat="server" Font-Bold="true" Text="Net Loss" Font-Size="16px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%-----------------------------Last amount ------------------------%>
                    <tr>
                        <td align="right" style="width: 50%; border-right: solid 1px black; border-left: solid 1px black;">
                            <table width="100%" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%; border-bottom: solid 1px black;">
                                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Total Credit" Style="text-decoration: underline;"
                                            Font-Size="16px"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 30%; border-bottom: solid 1px black;">
                                        <asp:Label ID="lbltotalCredit" runat="server" Font-Bold="true" Text="" Font-Size="16px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="width: 50%; border-right: solid 1px black;">
                            <table width="100%" class="largsize">
                                <tr>
                                    <td align="left" style="width: 70%; border-bottom: solid 1px black;">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Total Debit" Style="text-decoration: underline;"
                                            Font-Size="16px"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 30%; border-bottom: solid 1px black;">
                                        <asp:Label ID="lbltotalDebit" runat="server" Font-Bold="true" Text="" Font-Size="16px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
    </form>
</body>
</html>
