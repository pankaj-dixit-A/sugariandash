<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDuePayment.aspx.cs" Inherits="Report_rptDuePayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
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
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;
        <asp:Button ID="btnMail" runat="server" Text="Mail" CssClass="btnHelp" Width="80px"
            OnClientClick="CheckEmail();" OnClick="btnMail_Click" />
        &nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox></div>
    <div>
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri">
            <table align="center" width="90%" class="print">
                <tr>
                    <td style="width: 100%;" align="center">
                        <asp:Label ID="lblCompanyName" Width="70%" runat="server" Text="" Style="text-align: center;"
                            CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" align="left">
                        <asp:Label ID="Label15" runat="server" Text="Due Payment Upto: " CssClass="lblName"
                            Style="text-align: left;" Font-Size="14px" Font-Bold="true"></asp:Label>
                        <asp:Label ID="rptDate" Text="" Font-Size="14px" Font-Bold="true" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;
                background-color: Black;" class="print">
                <tr>
                    <td align="center" style="width: 4%;">
                        <asp:Label ID="Label1" runat="server" Text="Date" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                    <td align="center" style="width: 5%;">
                        <asp:Label ID="Label2" runat="server" ForeColor="White" Text="No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 20%;">
                        <asp:Label ID="Label3" runat="server" ForeColor="White" Text="Party Name" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label4" runat="server" ForeColor="White" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label5" runat="server" ForeColor="White" Text="Broker" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:Label ID="Label6" runat="server" ForeColor="White" Text="Qntl" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="Label8" runat="server" ForeColor="White" Text="Total" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="Label9" runat="server" ForeColor="White" Text="Recieved" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="Label10" runat="server" ForeColor="White" Text="Balance" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 2%;">
                        <asp:Label ID="Label11" runat="server" ForeColor="White" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" class="print">
                <tr>
                    <td align="left" style="width: 100%">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_ItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="print">
                                    <tr>
                                        <td align="center" style="width: 4%;">
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("doc_date") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 5%;">
                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("Doc_No") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 20%;" class="toosmall">
                                            <asp:Label ID="Label3" runat="server" Text='<%#Eval("PartyName") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%;">
                                            <asp:Label ID="Label4" runat="server" Text='<%#Eval("From_Station") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 10%;">
                                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("Broker") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 5%;">
                                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("Qntl") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 7%;">
                                            <asp:Label ID="Label8" runat="server" Text='<%#Eval("Voc_Amt") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 7%;">
                                            <asp:Label ID="Label9" runat="server" Text='<%#Eval("recieved") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 7%;">
                                            <asp:Label ID="Label10" runat="server" Text='<%#Eval("short") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 2%;">
                                            <asp:Label ID="Label11" runat="server" Text='<%#Eval("Type") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" style="table-layout: fixed; background-color: #CCFFFF;"
                class="print">
                <tr>
                    <td align="center" style="width: 4%;">
                        <asp:Label ID="Label7" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 5%;">
                        <asp:Label ID="Label12" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 20%;">
                        <asp:Label ID="Label13" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label14" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label16" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:Label ID="lblTotalQntl" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="lblTotalAmt" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="lblTotalRecieved" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="lblTotalBal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 2%;">
                        <asp:Label ID="Label21" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
