<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptUTR.aspx.cs" Inherits="Report_rptUTR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script src="../JS/emailValidation.js" type="text/javascript"></script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', 'st', 'height=400,width=800');
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
        <asp:Button runat="server" ID="btnPrint" Text="Print" OnClientClick="PrintPanel();" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;
        <asp:Button ID="btnMail" runat="server" Text="Mail" Width="80px" OnClientClick="return CheckEmail();"
            OnClick="btnMail_Click" />
        &nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox></div>
    <div>
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri" CssClass="largsize">
            <asp:Label ID="lblCompanyName" Width="90%" runat="server" Text="" Style="text-align: center;"
                CssClass="lblName" Font-Size="16px" Font-Bold="true"></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" Width="90%" Text="UTR REPORT" CssClass="lblName"
                Style="text-align: center;" Font-Size="14px" Font-Bold="true"></asp:Label>
            <table width="90%" align="center" cellpadding="1" cellspacing="0" class="largsize"
                style="border-bottom: 1px solid black; border-top: 1px solid black; table-layout: fixed;">
                <tr>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="Label1" runat="server" Text="UTR" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 2%;">
                        <asp:Label ID="Label2" runat="server" Text="UTR_DATE" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 5%;">
                        <asp:Label ID="Label6" runat="server" Text="Bank" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="Label9" runat="server" Text="Mill" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 5%;">
                        <asp:Label ID="Label3" runat="server" Text="UTR_NUMBER" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 4%;">
                        <asp:Label ID="Label4" runat="server" Text="UTR_AMOUNT" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 3%;">
                        <asp:Label ID="Label8" runat="server" Text="Narration" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="90%" align="center" class="largsize">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList ID="dtl" runat="server" Width="100%" OnItemDataBound="DataList_ItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" cellpadding="1" cellspacing="0" class="largsize"
                                    style="table-layout: fixed; border-bottom: 1px solid black; border-top: 1px solid black;">
                                    <tr style="background-color: #FFFFCC;">
                                        <td align="left" style="width: 2%;">
                                            <asp:Label ID="lblUtrNo" runat="server" Text='<%#Eval("UTR_NO") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="width: 2%;">
                                            <asp:Label ID="lblUtrDate" runat="server" Text='<%#Eval("UTR_DATE") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="width: 5%;">
                                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("bank_ac") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="width: 4%;">
                                            <asp:Label ID="Label10" runat="server" Text='<%#Eval("MillShort") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5%; word-wrap: break-word; text-wrap: normal;">
                                            <asp:Label ID="lblUtrBankNumber" runat="server" Text='<%#Eval("UTR_BANK_NUMBER") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 4%;">
                                            <asp:Label ID="lblUtrAmount" runat="server" Text='<%#Eval("UTR_AMOUNT") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 3%; text-wrap: normal; word-wrap: break-word;">
                                            <asp:Label ID="Label7" runat="server" Text='<%#Eval("Narration") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellpadding="1" cellspacing="0" class="largsize">
                                    <tr>
                                        <td style="width: 100%;">
                                            <table width="100%" align="center" class="largsize" style="table-layout: fixed; border-bottom: 1px solid black;">
                                                <tr>
                                                    <td align="left" style="width: 7%;">
                                                        DO_NO
                                                    </td>
                                                    <td align="left" style="width: 10%;">
                                                        DO_DATE
                                                    </td>
                                                    <td align="left" style="width: 25%;">
                                                        GETPASS
                                                    </td>
                                                    <td align="left" style="width: 10%;">
                                                        LORRY
                                                    </td>
                                                    <td align="left" style="width: 11%;">
                                                        Voucher By
                                                    </td>
                                                    <td align="left" style="width: 9%;">
                                                        QNTL
                                                    </td>
                                                    <td align="left" style="width: 10%;">
                                                        MR
                                                    </td>
                                                    <td align="left">
                                                        AMOUNT
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList runat="server" ID="dtlDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%" align="center" style="background-color: #CCFFFF; border-bottom: 1px dashed black;"
                                                        class="largsize">
                                                        <tr>
                                                            <td align="left" style="width: 7%;">
                                                                <asp:Label runat="server" ID="lblDO_Utrno" Text='<%#Eval("DO_NO") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 10%;">
                                                                <asp:Label runat="server" ID="lblDO_Date" Text='<%#Eval("DO_DATE") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 25%;">
                                                                <asp:Label runat="server" ID="lblDO_Getpassname" Text='<%#Eval("DO_GETPASSCODE") %>'
                                                                    Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 10%;">
                                                                <asp:Label runat="server" ID="Label12" Text='<%#Eval("LORRY") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 11%;">
                                                                <asp:Label runat="server" ID="Label11" Text='<%#Eval("DO_VoucherBy") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 9%;">
                                                                <asp:Label runat="server" ID="lblDO_Quintal" Text='<%#Eval("QNTL") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 10%;">
                                                                <asp:Label runat="server" ID="lblDO_Millrate" Text='<%#Eval("MR ") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label runat="server" ID="lblDO_Amount" Text='<%#Eval("USEDAmount") %>' Font-Bold="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" class="largsize">
                                    <tr>
                                        <td align="center" style="width: 100%;">
                                            <asp:Label runat="server" ID="lblUtrAmountTotal" Font-Bold="true" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="lblUtrUsedAmount" Font-Bold="true" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="lblUtrBal" Font-Bold="true" Text=""></asp:Label>
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
