<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptBrokerWiseShort.aspx.cs"
    Inherits="Report_rptBrokerWiseShort" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body style="font-family:Calibri; width:100%;font-size:12px;width:1100px; text-align:center;" >');
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
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri">
            <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
            <br />
            <br />
            <table width="1000px" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td colspan="8" align="right">
                        <table align="left">
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:Label runat="server" ID="lblBrokerName" Text="Broker" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="6">
                                    <asp:Label runat="server" ID="lblDate" Font-Bold="true" Text="Date"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    herewith sending payment and shortpayment list.Please verify and send D.D Amount
                                    Urgently.
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="1000px" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblspdate" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblRefNo" runat="server" Text="Ref.No" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%;">
                        <asp:Label ID="lblParty" runat="server" Text="Customer Name" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblQntl" runat="server" Text="Quintal" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 6%;">
                        <asp:Label ID="lblMillName" runat="server" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblAmount" runat="server" Text="Voc.Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblRecieved" runat="server" Text="Recieved" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblShort" runat="server" Text="Short" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="1000px" align="center" cellpadding="1" cellspacing="0">
                <tr>
                    <td colspan="8">
                        <asp:DataList runat="server" ID="dtl" Width="100%">
                            <ItemTemplate>
                                <table align="left" width="100%" cellpadding="1" cellspacing="5" style="table-layout: fixed;
                                    background-color: #CCFFFF;">
                                    <tr>
                                        <td align="center" style="width: 3%;">
                                            <asp:Label ID="lblspdate" runat="server" Text='<%#Eval("Date") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 2%;">
                                            <asp:Label ID="lblRefNo" runat="server" Text='<%#Eval("RefNo") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="lblParty" runat="server" Text='<%#Eval("Customer_Name") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lblQntl" runat="server" Text='<%#Eval("Qntl") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 6%;">
                                            <asp:Label ID="lblMillName" runat="server" Text='<%#Eval("Mill") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%;">
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Bill_Amount") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 2%;">
                                            <asp:Label ID="lblRecieved" runat="server" Text='<%#Eval("Recieved") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 2%;">
                                            <asp:Label ID="lblShort" runat="server" Text='<%#Eval("Balance") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8" align="right" style="border-bottom: dashed 2px black;">
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="1000px" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                background-color: #FFFFCC;">
                <tr>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="Label1" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label2" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label3" runat="server" Text="Total:" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblqntltotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 6%;">
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblvoctotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblRecievedtotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblShortTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="1000px" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td align="left" colspan="8">
                        <br />
                        Your's Faithfully,
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        For
                        <asp:Label runat="server" ID="lblCompanyBottom"></asp:Label>
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        Accountant
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
