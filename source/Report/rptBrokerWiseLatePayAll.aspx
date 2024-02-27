<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptBrokerWiseLatePayAll.aspx.cs"
    Inherits="Report_rptBrokerWiseLatePayAll" %>

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
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                            CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblBrokerName" Width="90%" runat="server" Text="" CssClass="lblName"
                            Font-Size="16px" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td align="right">
                        <table align="left">
                            <tr>
                                <td align="left">
                                    <asp:Label runat="server" ID="lblDate" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    herewith sending payment and shortpayment list.Please verify and send D.D Amount
                                    Urgently.
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
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
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="lblAmount" runat="server" Text="Voc.Amount" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblShort" runat="server" Text="Short" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="lblDelayDays" runat="server" Text="Delay Days" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label4" runat="server" Text="Interest" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0">
                <tr>
                    <td style="width: 100%;" align="center">
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
                                        <td align="left" style="width: 2%;">
                                            <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("amount") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 3%;">
                                            <asp:Label ID="lblShort" runat="server" Text='<%#Eval("short") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 2%;">
                                            <asp:Label ID="lblDelay" runat="server" Text='<%#Eval("DelayDays") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 2%;">
                                            <asp:Label ID="lblInteres" runat="server" Text='<%#Eval("Interest") %>' Font-Bold="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="border-bottom: dashed 2px black;">
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;
                background-color: #FFFFCC;">
                <tr>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="Label1" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="Label2" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 10%;">
                        <asp:Label ID="Label3" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="lblqntltotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="lblvoctotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 3%;">
                        <asp:Label ID="lblShortTotal" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="center" style="width: 2%;">
                        <asp:Label ID="dfdfsd" runat="server" Text="" Font-Bold="false"></asp:Label>
                    </td>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="lblIntesdfsdfres" runat="server" Text="" Font-Bold="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" style="table-layout: fixed;">
                <tr>
                    <td align="right" style="border-bottom: double 2px black;">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                        Your's Faithfully,
                    </td>
                </tr>
                <tr>
                    <td>
                        For
                        <asp:Label ID="lblCompanyNameBottom" runat="server"></asp:Label>
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        Accountant
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
