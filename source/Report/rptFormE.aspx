<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptFormE.aspx.cs" Inherits="Report_rptFormE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><link rel="stylesheet" href="../print.css" type="text/css" media="print" /><head>');
            printWindow.document.write('</head><body class="print">');
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
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            OnClientClick="CheckEmail();" Width="79px" />&nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" class="print">
            <table width="70%" align="center" style="table-layout: fixed;" cellspacing="3" class="print">
                <tr>
                    <td align="center">
                        <b style="font-size: large;">Form E</b>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <b style="font-size: large;">(Refer Regulation 2.1.14(2))</b>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <b style="font-size: large;">Form of GUARANTEE</b>
                    </td>
                </tr>
            </table>
            <table width="70%" align="center" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%">
                            <ItemTemplate>
                                <table width="100%" align="center" style="table-layout: fixed; border-top: 1px solid black;">
                                    <tr>
                                        <td style="width: 50%; text-align: left; border-bottom: 1px solid black;">
                                            Voucher No.:<asp:Label runat="server" ID="lblVoucNo" Text='<%#Eval("#")%>' Font-Bold="true"> </asp:Label>
                                        </td>
                                        <td style="width: 50%; text-align: right; border-bottom: 1px solid black;">
                                            Date:<asp:Label runat="server" ID="lblDate" Text='<%#Eval("dt")%>' Font-Bold="true"> </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            From
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblCmpName" Text='<%#Eval("CmpName")%>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblCmpAddress" Text='<%#Eval("CmpAddress")%>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblCmpCity" Text='<%#Eval("CmpCity")%>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            To
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 65%; text-align: left;">
                                            <asp:Label runat="server" ID="lblPartyName" Font-Bold="true" Text='<%#Eval("Party") %>'></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            L.I.C No:<asp:Label runat="server" ID="lblCSTNo" Font-Bold="false" Text='<%#Eval("LIC") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 65%; text-align: left;">
                                            <asp:Label runat="server" ID="lblPartyAddress" Font-Bold="false" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            C.S.T No:<asp:Label runat="server" ID="lblCST" Font-Bold="false" Text='<%#Eval("CST") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 65%; text-align: left;">
                                        </td>
                                        <td style="width: 35%; text-align: left;">
                                            G.S.T No:<asp:Label runat="server" ID="lblGST" Font-Bold="false" Text='<%#Eval("GST") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 65%; text-align: left; border-bottom: 1px solid black;">
                                            <asp:Label runat="server" ID="lblPartyCity" Font-Bold="false" Text='<%#Eval("PartyCity") %>'></asp:Label>
                                        </td>
                                        <td style="width: 35%; text-align: left; border-bottom: 1px solid black;">
                                            TIN No:<asp:Label runat="server" ID="lblTin" Font-Bold="false" Text='<%#Eval("TIN") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;">
                                                <tr>
                                                    <td style="width: 15%;" align="center">
                                                        Date of Sale
                                                    </td>
                                                    <td style="width: 30%;" align="center">
                                                        Nature & Quality of<br />
                                                        Article/Brand Name,If Any
                                                    </td>
                                                    <td style="width: 20%;" align="center">
                                                        Batch No or Code No
                                                    </td>
                                                    <td style="width: 20%;" align="center">
                                                        Quantity
                                                    </td>
                                                    <td style="width: 25%;" align="center">
                                                        Price
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;">
                                                <tr>
                                                    <td style="width: 15%;" align="center">
                                                        1
                                                    </td>
                                                    <td style="width: 30%;" align="center">
                                                        2
                                                    </td>
                                                    <td style="width: 20%;" align="center">
                                                        3
                                                    </td>
                                                    <td style="width: 20%;" align="center">
                                                        4
                                                    </td>
                                                    <td style="width: 25%;" align="center">
                                                        5
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;">
                                                <tr>
                                                    <td style="width: 15%;" align="center">
                                                        <asp:Label runat="server" ID="lblDates" Text='<%#Eval("dt") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 30%;" align="center">
                                                        Sugar
                                                    </td>
                                                    <td style="width: 20%;" align="center">
                                                    </td>
                                                    <td style="width: 20%;" align="center">
                                                        <asp:Label runat="server" ID="lblQntl" Text='<%#Eval("Qntl") %>'></asp:Label>&nbsp;QT
                                                    </td>
                                                    <td style="width: 25%;" align="center">
                                                        <asp:Label runat="server" ID="lblVoucAmt" Text='<%#Eval("VoucherAmount") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            I/We hereby certify that food/foods mentioned in this invoice is/are warranted to
                                            be of the nature and quality which it/these purports/purported to be
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 50%;" align="left">
                                                    </td>
                                                    <td style="width: 50%; height: 150px; vertical-align: bottom;" align="right">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%;" align="left">
                                                        Name and Address of<br />
                                                        Manufacturer/Packer<br />
                                                        (In Case Of Packed Article)
                                                    </td>
                                                    <td style="width: 50%;" align="right">
                                                        Signature of<br />
                                                        Manufacturer/Distributer/Dealer
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        Lic No.: 38/2009<br />
                                                        Wherever Applicable
                                                    </td>
                                                </tr>
                                            </table>
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
