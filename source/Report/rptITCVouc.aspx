<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptITCVouc.aspx.cs" Inherits="Report_rptITCVouc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../print.css" type="text/css" media="print" />
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
    <script type="text/javascript">
        function PrintPanel2() {
            var panel = document.getElementById("<%=pnlMain2.ClientID %>");
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;
        <asp:Button ID="btnPreprinted" runat="server" Text="Pre-Printed" CssClass="btnHelp"
            OnClientClick="return PrintPanel2();" Width="80px" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            Width="79px" />
        <asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" class="print">
            <table width="70%" style="table-layout: fixed; height: 125px;" class="print9pt" align="center">
                <tr>
                    <td style="width: 20%; vertical-align: top;" align="center">
                        <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/VoLogo.jpg" Width="100%"
                            Height="90%" />
                    </td>
                    <td style="width: 80%; vertical-align: top;" align="left">
                        <table width="100%" style="table-layout: fixed;">
                            <tr>
                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                    <asp:Label ID="Label5" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="lblAl1" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="lblAl2" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="lblAl3" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="lblAl4" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="70%" align="center" cellpadding="1" cellspacing="2" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" cellspacing="2" style="table-layout: fixed;">
                                    <tr>
                                        <td style="width: 70%; text-align: left; border-bottom: 1px solid black; border-top: 1px solid black;">
                                            Voucher No.:<asp:Label runat="server" ID="lblVoucherNo" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 30%; text-align: right; border-bottom: 1px solid black; border-top: 1px solid black;">
                                            Date.:<asp:Label runat="server" ID="lblDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="toosmall" style="border-bottom: 1px solid black;">
                                            <table width="100%" align="center" style="height: 165px; table-layout: fixed;" class="toosmall">
                                                <tr>
                                                    <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                        <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed;
                                                            height: 100%;">
                                                            <tr>
                                                                <td align="left" style="font-size: small;">
                                                                    Buyer,
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="lblPartyName" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="lblPartyAddr" runat="server" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="font-size: small;" class="toosmall">
                                                                    City:&nbsp;<asp:Label ID="lblPartyCity" runat="server" Font-Bold="true" Text='<%#Eval("party_city") %>'></asp:Label>
                                                                    &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblPartyState" Text='<%#Eval("party_state") %>'
                                                                        Font-Bold="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top; height: 50px;" class="toosmall">
                                                                    <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblCSTNo" runat="server" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblGSTNo" runat="server" Text='<%#Eval("Gst_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblTinNo" runat="server" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblLICNo" runat="server" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblECCNo" runat="server" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label13" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                        <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed;
                                                            height: 100%;">
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="Label14" runat="server" Font-Bold="true" Text='<%#Eval("PartyNameC") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="Label15" runat="server" Text='<%#Eval("PartyAddressC") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="font-size: small;" class="toosmall">
                                                                    <asp:Label ID="Label16" runat="server" Text='<%#Eval("party_cityC") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top; height: 50px;" class="toosmall">
                                                                    <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("Cst_noC") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label7" runat="server" Text='<%#Eval("Gst_NoC") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("Tin_NoC") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("Local_Lic_NoC") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label10" runat="server" Text='<%#Eval("ECC_NoC") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label11" runat="server" Text='<%#Eval("CompanyPanC") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="border-bottom: 1px solid black;">
                                            <table width="100%" cellspacing="3" class="print9pt">
                                                <tr>
                                                    <td style="width: 40%;" align="left" class="print9pt">
                                                        Dispatched From :<asp:Label runat="server" ID="lblDispatchFrom" Font-Bold="true"
                                                            Text='<%#Eval("Dispatch_From") %>'></asp:Label>
                                                    </td>
                                                    <td style="width; 60%;" align="left" class="print9pt">
                                                        To :<asp:Label runat="server" ID="lblDispatchto" Font-Bold="true" Text='<%#Eval("Dispatch_To") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        Dri.Mobile:<asp:Label runat="server" ID="Label3" Text='<%#Eval("driverMobile") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 40%;" align="left" class="print9pt">
                                                        R.R.No. L.R.No.:<asp:Label runat="server" ID="lblLorryNo" Text='<%#Eval("lorry") %>'></asp:Label>
                                                    </td>
                                                    <td style="width; 60%;" align="left" class="print9pt">
                                                        Broker:&nbsp;<asp:Label runat="server" ID="lblBrokershort" Text='<%#Eval("brokshort") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" colspan="2">
                                            <table width="100%" align="left" style="border-bottom: 1px solid black; height: 20px;
                                                table-layout: fixed;">
                                                <tr>
                                                    <td style="width: 40%;">
                                                        Particulars:-&nbsp;&nbsp; Quantal :<asp:Label runat="server" ID="lblQuintal" Text='<%#Eval("Qntl") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 30%;">
                                                        Bags :<asp:Label runat="server" ID="lblBags" Text='<%#Eval("Bags") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 40%;">
                                                        Grade :<asp:Label runat="server" ID="lblGrade" Text='<%#Eval("Grade") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            We have paid on your behalf in account of
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="small">
                                            <asp:Label runat="server" ID="lblMillName" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" style="border-bottom: 1px solid black;">
                                            Credit the same to our account & debit to mills account.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <table width="100%" align="left" style="border-bottom: 1px solid black;" cellspacing="1"
                                                class="print9pt">
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        P.O.No.:<asp:Label runat="server" ID="lblPONO" Text='<%#Eval("PODetails") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Rate diff debit/credit your account:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="Label1" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        Buyer,
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Bank Commission:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblBankCommission" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        <asp:Label runat="server" ID="lblBroker" Font-Bold="true" Text='<%#Eval("BrokerName") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Brokrage:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblBrokrage" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        <asp:Label runat="server" ID="lblBrokerAddress" Text='<%#Eval("brokAddress") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Quality Difference:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblQualityDiff" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        City:<asp:Label runat="server" ID="lblBrokerCity" Text='<%#Eval("BrokerCity") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Commission:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblCommission" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        State:<asp:Label runat="server" ID="lblBrokerState" Text='<%#Eval("BrokerState") %>'></asp:Label>&nbsp;&nbsp;PinCode:&nbsp;<asp:Label
                                                            runat="server" ID="Label4" Text='<%#Eval("brokpin") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Freight & Other Exp.:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblOtherExpenses" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Post & Phone:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblPhone" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Interest:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblInterest" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Transports:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblTransports" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        I/We hereby certify that food/foods mentioned in this<br />
                                                        invoice is/are warranted to be of the nature and quality which it/these purported
                                                        to be
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Other:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblOther" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%; border-bottom: 1px solid black;" align="left">
                                                    </td>
                                                    <td style="width: 25%; border-bottom: 1px solid black;" align="right">
                                                        Total:
                                                    </td>
                                                    <td style="width: 40%; border-bottom: 1px solid black;" align="right">
                                                        <asp:Label runat="server" ID="lblVoucherAmount" Text='<%#Eval("VoucherAmount") %>'></asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="height: 25px;">
                                                    <td style="width: 55%;" align="left">
                                                        Credit Our Account Total
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        RTGS Rs:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="Label2" Text='<%#Eval("VoucherAmount") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" style="border-bottom: 1px solid black;">
                                            Rupees :<asp:Label runat="server" ID="lblInWords" Font-Bold="true" Text='<%#Eval("InWords") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="vertical-align: top;">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 80%; text-align: left;">
                                                        Note:After dispatch of the goods we are not responsible for nondelivery or any kind
                                                        of damage or demand
                                                    </td>
                                                    <td style="width: 20%;">
                                                        <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td align="left" style="border-bottom: 1px solid black; width: 40%;">
                                                        <asp:Label runat="server" ID="lblMillSRBroker" Text='<%#Eval("SRPerKg") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="right" style="border-bottom: 1px solid black; width: 60%;">
                                                        For,
                                                        <asp:Label runat="server" ID="lblCmpName" Font-Bold="true" Text='<%#Eval("CmpName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="small">
                                            1)Please credit the amount in our account and send the amount by RTGS immediately.
                                            <br />
                                            2)If the amount is not sent before the due date payment charges 24% will be charged.
                                            <br />
                                            3)This is computer generated print No Signature Required.
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
    <div style="display: none;">
        <asp:Panel runat="server" ID="pnlMain2" class="print">
            <table width="70%" style="table-layout: fixed; height: 125px;" class="noprint9pt"
                align="center">
                <tr>
                    <td style="width: 20%; vertical-align: top;" align="center">
                        <asp:Image runat="server" ID="Image1" ImageUrl="~/Images/Logo.jpg" Width="100%" Height="30%" />
                    </td>
                    <td style="width: 80%; vertical-align: top;" align="left">
                        <table width="100%" style="table-layout: fixed;">
                            <tr>
                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                    <asp:Label ID="Label6" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                    <asp:Label ID="Label7" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="Label8" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="Label9" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="Label10" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="Label11" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                    <asp:Label runat="server" ID="Label12" ForeColor="Blue"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="70%" align="center" cellpadding="1" cellspacing="2" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist2" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" cellspacing="2" style="table-layout: fixed;">
                                    <tr>
                                        <td style="width: 70%; text-align: left; border-bottom: 1px solid black; border-top: 1px solid black;">
                                            Voucher No.:<asp:Label runat="server" ID="lblVoucherNo" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 30%; text-align: right; border-bottom: 1px solid black; border-top: 1px solid black;">
                                            Date.:<asp:Label runat="server" ID="lblDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="toosmall" style="border-bottom: 1px solid black;">
                                            <table width="100%" align="center" style="height: 165px; table-layout: fixed;" class="toosmall">
                                                <tr>
                                                    <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                        <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed;
                                                            height: 100%;">
                                                            <tr>
                                                                <td align="left" style="font-size: small;">
                                                                    Buyer,
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="lblPartyName" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="lblPartyAddr" runat="server" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="font-size: small;" class="toosmall">
                                                                    City:&nbsp;<asp:Label ID="lblPartyCity" runat="server" Font-Bold="true" Text='<%#Eval("party_city") %>'></asp:Label>
                                                                    &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblPartyState" Text='<%#Eval("party_state") %>'
                                                                        Font-Bold="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top; height: 50px;" class="toosmall">
                                                                    <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblCSTNo" runat="server" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblGSTNo" runat="server" Text='<%#Eval("Gst_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblTinNo" runat="server" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblLICNo" runat="server" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblECCNo" runat="server" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label13" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                        <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed;
                                                            height: 100%;">
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="Label14" runat="server" Font-Bold="true" Text='<%#Eval("PartyNameC") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="print9pt">
                                                                    <asp:Label ID="Label15" runat="server" Text='<%#Eval("PartyAddressC") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="font-size: small;" class="toosmall">
                                                                    <asp:Label ID="Label16" runat="server" Text='<%#Eval("party_cityC") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top; height: 50px;" class="toosmall">
                                                                    <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("Cst_noC") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label7" runat="server" Text='<%#Eval("Gst_NoC") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("Tin_NoC") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("Local_Lic_NoC") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label10" runat="server" Text='<%#Eval("ECC_NoC") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="Label11" runat="server" Text='<%#Eval("CompanyPanC") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="border-bottom: 1px solid black;">
                                            <table width="100%" cellspacing="3" class="print9pt">
                                                <tr>
                                                    <td style="width: 40%;" align="left" class="print9pt">
                                                        Dispatched From :<asp:Label runat="server" ID="lblDispatchFrom" Font-Bold="true"
                                                            Text='<%#Eval("Dispatch_From") %>'></asp:Label>
                                                    </td>
                                                    <td style="width; 60%;" align="left" class="print9pt">
                                                        To :<asp:Label runat="server" ID="lblDispatchto" Font-Bold="true" Text='<%#Eval("Dispatch_To") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        Dri.Mobile:<asp:Label runat="server" ID="Label3" Text='<%#Eval("driverMobile") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 40%;" align="left" class="print9pt">
                                                        R.R.No. L.R.No.:<asp:Label runat="server" ID="lblLorryNo" Text='<%#Eval("lorry") %>'></asp:Label>
                                                    </td>
                                                    <td style="width; 60%;" align="left" class="print9pt">
                                                        Broker:&nbsp;<asp:Label runat="server" ID="lblBrokershort" Text='<%#Eval("brokshort") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" colspan="2">
                                            <table width="100%" align="left" style="border-bottom: 1px solid black; height: 20px;
                                                table-layout: fixed;">
                                                <tr>
                                                    <td style="width: 40%;">
                                                        Particulars:-&nbsp;&nbsp; Quantal :<asp:Label runat="server" ID="lblQuintal" Text='<%#Eval("Qntl") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 30%;">
                                                        Bags :<asp:Label runat="server" ID="lblBags" Text='<%#Eval("Bags") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 40%;">
                                                        Grade :<asp:Label runat="server" ID="lblGrade" Text='<%#Eval("Grade") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            We have paid on your behalf in account of
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="small">
                                            <asp:Label runat="server" ID="lblMillName" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" style="border-bottom: 1px solid black;">
                                            Credit the same to our account & debit to mills account.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <table width="100%" align="left" style="border-bottom: 1px solid black;" cellspacing="1"
                                                class="print9pt">
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        P.O.No.:<asp:Label runat="server" ID="lblPONO" Text='<%#Eval("PODetails") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Rate diff debit/credit your account:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="Label1" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        Buyer,
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Bank Commission:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblBankCommission" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        <asp:Label runat="server" ID="lblBroker" Font-Bold="true" Text='<%#Eval("BrokerName") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Brokrage:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblBrokrage" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        <asp:Label runat="server" ID="lblBrokerAddress" Text='<%#Eval("brokAddress") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Quality Difference:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblQualityDiff" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        City:<asp:Label runat="server" ID="lblBrokerCity" Text='<%#Eval("BrokerCity") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Commission:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblCommission" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        State:<asp:Label runat="server" ID="lblBrokerState" Text='<%#Eval("BrokerState") %>'></asp:Label>&nbsp;&nbsp;PinCode:&nbsp;<asp:Label
                                                            runat="server" ID="Label4" Text='<%#Eval("brokpin") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Freight & Other Exp.:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblOtherExpenses" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Post & Phone:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblPhone" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Interest:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblInterest" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Transports:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblTransports" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%;" align="left">
                                                        I/We hereby certify that food/foods mentioned in this<br />
                                                        invoice is/are warranted to be of the nature and quality which it/these purported
                                                        to be
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        Other:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="lblOther" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 55%; border-bottom: 1px solid black;" align="left">
                                                    </td>
                                                    <td style="width: 25%; border-bottom: 1px solid black;" align="right">
                                                        Total:
                                                    </td>
                                                    <td style="width: 40%; border-bottom: 1px solid black;" align="right">
                                                        <asp:Label runat="server" ID="lblVoucherAmount" Text='<%#Eval("VoucherAmount") %>'></asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr style="height: 25px;">
                                                    <td style="width: 55%;" align="left">
                                                        Credit Our Account Total
                                                    </td>
                                                    <td style="width: 25%;" align="right">
                                                        RTGS Rs:
                                                    </td>
                                                    <td style="width: 40%;" align="right">
                                                        <asp:Label runat="server" ID="Label2" Text='<%#Eval("VoucherAmount") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" style="border-bottom: 1px solid black;">
                                            Rupees :<asp:Label runat="server" ID="lblInWords" Font-Bold="true" Text='<%#Eval("InWords") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="vertical-align: top;">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td style="width: 80%; text-align: left;">
                                                        Note:After dispatch of the goods we are not responsible for nondelivery or any kind
                                                        of damage or demand
                                                    </td>
                                                    <td style="width: 20%;">
                                                        <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td align="left" style="border-bottom: 1px solid black; width: 40%;">
                                                        <asp:Label runat="server" ID="lblMillSRBroker" Text='<%#Eval("SRPerKg") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td align="right" style="border-bottom: 1px solid black; width: 60%;">
                                                        For,
                                                        <asp:Label runat="server" ID="lblCmpName" Font-Bold="true" Text='<%#Eval("CmpName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="small">
                                            1)Please credit the amount in our account and send the amount by RTGS immediately.
                                            <br />
                                            2)If the amount is not sent before the due date payment charges 24% will be charged.
                                            <br />
                                            3)This is computer generated print No Signature Required.
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
