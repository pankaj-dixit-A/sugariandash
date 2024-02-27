<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptVouchers.aspx.cs" Inherits="Report_rptVoucher_s" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../print.css" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById('<%=dtlDetails.ClientID %>');
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head><link rel="stylesheet" href="../print.css" type="text/css" media="print" />');
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
    <script type="text/javascript">
        function PrintPanel2() {
            var panel = document.getElementById('<%=dtlDetails2.ClientID %>');
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head><link rel="stylesheet" href="../print.css" type="text/css" media="print" />');
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
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="//cdn.rawgit.com/MrRio/jsPDF/master/dist/jspdf.min.js"></script>
    <script type="text/javascript" src="//cdn.rawgit.com/niklasvh/html2canvas/0.5.0-alpha2/dist/html2canvas.min.js"></script>
    <script type="text/javascript">
        (function () {
            var 
	form = $('.form'),
	cache_width = form.width(),
	a4 = [595.28, 841.89];  // for a4 size paper width and height

            $('#create_pdf').on('click', function () {
                $('body').scrollTop(0);
                createPDF();
            });
            //create pdf
            function createPDF() {
                getCanvas().then(function (canvas) {
                    var 
		img = canvas.toDataURL("image/png"),
		doc = new jsPDF({
		    unit: 'px',
		    format: 'a4'
		});
                    doc.addImage(img, 'JPEG', 20, 20);
                    doc.save('techumber-html-to-pdf.pdf');
                    form.width(cache_width);
                });
            }

            // create canvas object
            function getCanvas() {
                form.width((a4[0] * 1.33333) - 80).css('max-width', 'none');
                return html2canvas(form, {
                    imageTimeout: 2000,
                    removeContainer: true
                });
            }

        } ());
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="return PrintPanel();"
            Width="80px" />&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnPriPrinted" Text="Pre-Printed"
                OnClientClick="return PrintPanel2();" OnClick="btnPriPrinted_Click" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>&nbsp;<asp:Button
            runat="server" ID="btnPDF" Text="PDF" OnClientClick="return getCanvas();" Visible="false" />
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain">
            <asp:DataList runat="server" ID="dtlDetails" Width="100%" OnItemDataBound="dtlDetails_OnItemDataBound">
                <ItemTemplate>
                    <%--<div class="print">--%>
                    <asp:Label runat="server" ID="lblDocno" Visible="false" Text='<%#Eval("Doc_No") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:Label runat="server" ID="lbltype" Visible="false" Text='<%#Eval("Tran_Type") %>'
                        CssClass="lbl"></asp:Label>
                    <%--</div>--%>
                    <asp:DataList runat="server" ID="dtl" Width="100%" OnItemDataBound="dtl_OnItemDataBound">
                        <ItemTemplate>
                            <table id="tbMain" runat="server" align="center" style="table-layout: fixed;" width="1000px"
                                class="print">
                                <tr>
                                    <td style="width: 100%;" colspan="2">
                                        <table width="100%" style="table-layout: fixed; height: 130px;" class="print">
                                            <tr id="trcmpname" runat="server">
                                                <td align="center">
                                                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <td style="width: 100%;">
                                                    <table width="100%" align="center" style="table-layout: fixed;" cellspacing="2" cellpadding="1"
                                                        class="print9pt">
                                                        <tr>
                                                            <td style="width: 38.5%; word-wrap: break-word; text-wrap: normal;" align="left">
                                                                <asp:Label runat="server" ID="lblLeftAddress" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td style="width: 25%; word-wrap: break-word; text-wrap: normal;" align="center">
                                                                <asp:Label runat="server" ID="lblMiddlePart" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td style="width: 38.5%; word-wrap: break-word; text-wrap: normal;" align="left">
                                                                <asp:Label runat="server" ID="lblRightAddress" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                        Voucher No:<asp:Label runat="server" ID="lblVoucherNo" Font-Bold="true" Text='<%#Eval("VoucherNo") %>'></asp:Label>
                                        <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("Doc_No") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                        Date:
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Doc_Date") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" class="toosmall">
                                        <table width="100%" align="center" style="height: 150px; table-layout: fixed;" class="toosmall">
                                            <tr>
                                                <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                    <table width="100%" align="center" class="toosmall" cellspacing="2">
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
                                                            <td style="vertical-align: top; font-size: medium;" class="toosmall">
                                                                <table width="100%" align="center" class="toosmall">
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
                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                    <table width="100%" align="center" class="toosmall" cellspacing="2">
                                                        <tr>
                                                            <td align="left" style="font-size: small;">
                                                                <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text='<%#Eval("PartyNameC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("PartyAddressC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="font-size: small;" class="toosmall">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("party_cityC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; font-size: medium;" class="toosmall">
                                                                <table width="100%" align="center" class="toosmall">
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
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <table width="100%" cellspacing="2" cellpadding="0" style="table-layout: fixed; height: 30px;"
                                            class="print">
                                            <tr>
                                                <td style="width: 50%" align="left">
                                                    Dispatched From: &nbsp;<asp:Label ID="lblDispatchFrom" runat="server" Text='<%#Eval("From_Place") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    To:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTo" runat="server"
                                                        Text='<%#Eval("To_Place") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50%" align="left">
                                                    Lorry No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblLorryNo" runat="server" Text='<%#Eval("Lorry_No") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    Broker:
                                                    <asp:Label runat="server" ID="lblBroker" Text='<%#Eval("BrokerShort") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 25px;">
                                        Particulars:-&nbsp;Quantal : &nbsp;&nbsp;<asp:Label ID="lblQuantal" runat="server"
                                            Text='<%#Eval("Quantal") %>'></asp:Label>&nbsp;&nbsp; Bags:&nbsp;&nbsp;<asp:Label
                                                ID="lblBags" runat="server" Text='<%#Eval("BAGS") %>'></asp:Label>&nbsp;&nbsp;
                                        Grade:&nbsp;&nbsp;<asp:Label ID="lblGrade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>&nbsp;&nbsp;
                                        Mill Rate:&nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text='<%#Eval("Mill_Rate") %>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: solid 1px black; height: 70px; vertical-align: top;"
                                        class="print9pt">
                                        We have paid on your behalf in account of<br />
                                        <asp:Label ID="lblMillNameFull" runat="server" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>&nbsp;Credit
                                        the same to our account & debit to mill's account
                                    </td>
                                    <td align="right" style="border-bottom: solid 1px black;" class="print9pt">
                                        <asp:Label ID="lblMillAmount" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;" align="right">
                                        <table width="50%" align="right" class="small" style="table-layout: fixed; height: 150px;">
                                            <tr>
                                                <td align="left">
                                                    rate diff debit/credit your a/c:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("LESSDIFF").ToString()=="0.00"?"":Eval("LESSDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Bank Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBankCommission" runat="server" Text='<%#Eval("BANK_COMMISSION").ToString()=="0.00"?"":Eval("BANK_COMMISSION","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Brokrage:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBrokrage" runat="server" Text='<%#Eval("Brokrage").ToString()=="0.00"?"":Eval("Brokrage","{0}")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Quality Difference:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblQualityDiff" runat="server" Text='<%#Eval("RATEDIFF").ToString()=="0.00"?"":Eval("RATEDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCommission" runat="server" Text='<%#Eval("Commission_Amount").ToString()=="0.00"?"":Eval("Commission_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Freight & Other Exp:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FREIGHT").ToString()=="0.00"?"":Eval("FREIGHT","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Post and Phone:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblPostAmt" runat="server" Text='<%#Eval("Postage").ToString()=="0.00"?"":Eval("Postage","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Interest:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest").ToString()=="0.00"?"":Eval("Interest","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Transport:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("Cash_Ac_Amount").ToString()=="0.00"?"":Eval("Cash_Ac_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Other:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblOther" runat="server" Text='<%#Eval("OTHER_Expenses").ToString()=="0.00"?"":Eval("OTHER_Expenses","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" align="right" class="print" style="table-layout: fixed; height: 20px;">
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Total:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" align="right" style="table-layout: fixed; height: 20px;" class="print">
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Credit account total
                                                </td>
                                                <td align="left" style="width: 40%;">
                                                    RTGS Rs.:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblTotalAmt" Font-Bold="true" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="border-bottom: 1px solid black;">
                                        In Words:<asp:Label runat="server" ID="lblInWords" Font-Bold="true" Text='<%#Eval("InWords") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <asp:Label ID="lblNote" Font-Bold="true" runat="server" Text="Note:"></asp:Label>&nbsp;&nbsp;
                                        After dispatch of the goods we are not responsible for non-delivery or any kind
                                        of damage or demand.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" class="print" style="table-layout: fixed; height: 50px;">
                                            <tr>
                                                <td style="height: 50px; width: 100%; vertical-align: top;">
                                                    <table width="100%" align="left" style="table-layout: fixed;" class="print">
                                                        <tr>
                                                            <td style="width: 70%;" align="left">
                                                                <asp:Label ID="lblNarration1" Font-Bold="true" runat="server" Text='<%#Eval("Narration1") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 30%;" rowspan="2" align="right">
                                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%;" align="left">
                                                                <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("Narration2") %>'
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;" align="right">
                                                    For,<asp:Label runat="server" ID="lblSignCmpName"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 30px;" class="print9pt">
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
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    <div style="display: none;">
        <asp:Panel runat="server" ID="Panel1">
            <asp:DataList runat="server" ID="dtlDetails2" Width="100%" OnItemDataBound="dtlDetails_OnItemDataBound"
                Style="table-layout: fixed;">
                <ItemTemplate>
                    <%--<div class="print">--%>
                    <asp:Label runat="server" ID="lblDocno" Visible="false" Text='<%#Eval("Doc_No") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:Label runat="server" ID="lbltype" Visible="false" Text='<%#Eval("Tran_Type") %>'
                        CssClass="lbl"></asp:Label>
                    <%--</div>--%>
                    <asp:DataList runat="server" ID="dtl" Width="100%" OnItemDataBound="dtl_OnItemDataBound">
                        <ItemTemplate>
                            <table id="tbMain" runat="server" align="center" style="table-layout: fixed;" width="1000px"
                                class="print">
                                <tr>
                                    <td style="width: 100%;" colspan="2">
                                        <table width="100%" style="table-layout: fixed; height: 130px;" class="noprint">
                                            <tr id="trcmpname" runat="server">
                                                <td align="center">
                                                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <td style="width: 100%;">
                                                    <table width="100%" align="center" style="table-layout: fixed;" cellspacing="2" cellpadding="1"
                                                        class="noprint">
                                                        <tr>
                                                            <td style="width: 38.5%; word-wrap: break-word; text-wrap: normal;" align="left">
                                                                <asp:Label runat="server" ID="lblLeftAddress" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td style="width: 25%; word-wrap: break-word; text-wrap: normal;" align="center">
                                                                <asp:Label runat="server" ID="lblMiddlePart" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td style="width: 38.5%; word-wrap: break-word; text-wrap: normal;" align="left">
                                                                <asp:Label runat="server" ID="lblRightAddress" Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                        Voucher No:<asp:Label runat="server" ID="lblVoucherNo" Font-Bold="true" Text='<%#Eval("VoucherNo") %>'></asp:Label>
                                        <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("Doc_No") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                        Date:
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Doc_Date") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" class="toosmall">
                                        <table width="100%" align="center" style="height: 150px; table-layout: fixed;" class="toosmall">
                                            <tr>
                                                <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                    <table width="100%" align="center" class="toosmall" cellspacing="2">
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
                                                            <td style="vertical-align: top; font-size: medium;" class="toosmall">
                                                                <table width="100%" align="center" class="toosmall">
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
                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                    <table width="100%" align="center" class="toosmall" cellspacing="2">
                                                        <tr>
                                                            <td align="left" style="font-size: small;">
                                                                <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text='<%#Eval("PartyNameC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("PartyAddressC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="font-size: small;" class="toosmall">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("party_cityC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; font-size: medium;" class="toosmall">
                                                                <table width="100%" align="center" class="toosmall">
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
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <table width="100%" cellspacing="2" cellpadding="0" style="table-layout: fixed; height: 30px;"
                                            class="print">
                                            <tr>
                                                <td style="width: 50%" align="left">
                                                    Dispatched From: &nbsp;<asp:Label ID="lblDispatchFrom" runat="server" Text='<%#Eval("From_Place") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    To:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTo" runat="server"
                                                        Text='<%#Eval("To_Place") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50%" align="left">
                                                    Lorry No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblLorryNo" runat="server" Text='<%#Eval("Lorry_No") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    Broker:
                                                    <asp:Label runat="server" ID="lblBroker" Text='<%#Eval("BrokerShort") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 25px;">
                                        Particulars:-&nbsp;Quantal : &nbsp;&nbsp;<asp:Label ID="lblQuantal" runat="server"
                                            Text='<%#Eval("Quantal") %>'></asp:Label>&nbsp;&nbsp; Bags:&nbsp;&nbsp;<asp:Label
                                                ID="lblBags" runat="server" Text='<%#Eval("BAGS") %>'></asp:Label>&nbsp;&nbsp;
                                        Grade:&nbsp;&nbsp;<asp:Label ID="lblGrade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>&nbsp;&nbsp;
                                        Mill Rate:&nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text='<%#Eval("Mill_Rate") %>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: solid 1px black; height: 70px; vertical-align: top;"
                                        class="print9pt">
                                        We have paid on your behalf in account of<br />
                                        <asp:Label ID="lblMillNameFull" runat="server" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>&nbsp;Credit
                                        the same to our account & debit to mill's account
                                    </td>
                                    <td align="right" style="border-bottom: solid 1px black;" class="print9pt">
                                        <asp:Label ID="lblMillAmount" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;" align="right">
                                        <table width="50%" align="right" class="small" style="table-layout: fixed; height: 150px;">
                                            <tr>
                                                <td align="left">
                                                    rate diff debit/credit your a/c:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("LESSDIFF").ToString()=="0.00"?"":Eval("LESSDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Bank Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBankCommission" runat="server" Text='<%#Eval("BANK_COMMISSION").ToString()=="0.00"?"":Eval("BANK_COMMISSION","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Brokrage:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBrokrage" runat="server" Text='<%#Eval("Brokrage").ToString()=="0.00"?"":Eval("Brokrage","{0}")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Quality Difference:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblQualityDiff" runat="server" Text='<%#Eval("RATEDIFF").ToString()=="0.00"?"":Eval("RATEDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCommission" runat="server" Text='<%#Eval("Commission_Amount").ToString()=="0.00"?"":Eval("Commission_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Freight & Other Exp:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FREIGHT").ToString()=="0.00"?"":Eval("FREIGHT","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Post and Phone:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblPostAmt" runat="server" Text='<%#Eval("Postage").ToString()=="0.00"?"":Eval("Postage","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Interest:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest").ToString()=="0.00"?"":Eval("Interest","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Transport:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("Cash_Ac_Amount").ToString()=="0.00"?"":Eval("Cash_Ac_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Other:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblOther" runat="server" Text='<%#Eval("OTHER_Expenses").ToString()=="0.00"?"":Eval("OTHER_Expenses","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" align="right" class="print" style="table-layout: fixed; height: 20px;">
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Total:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" align="right" style="table-layout: fixed; height: 20px;" class="print">
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Credit account total
                                                </td>
                                                <td align="left" style="width: 40%;">
                                                    RTGS Rs.:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblTotalAmt" Font-Bold="true" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="border-bottom: 1px solid black;">
                                        In Words:<asp:Label runat="server" ID="lblInWords" Font-Bold="true" Text='<%#Eval("InWords") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <asp:Label ID="lblNote" Font-Bold="true" runat="server" Text="Note:"></asp:Label>&nbsp;&nbsp;
                                        After dispatch of the goods we are not responsible for non-delivery or any kind
                                        of damage or demand.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" class="print" style="table-layout: fixed; height: 50px;">
                                            <tr>
                                                <td style="height: 50px; width: 100%; vertical-align: top;">
                                                    <table width="100%" align="left" style="table-layout: fixed;" class="print">
                                                        <tr>
                                                            <td style="width: 70%;" align="left">
                                                                <asp:Label ID="lblNarration1" Font-Bold="true" runat="server" Text='<%#Eval("Narration1") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 30%;" rowspan="2" align="right">
                                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%;" align="left">
                                                                <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("Narration2") %>'
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;" align="right">
                                                    For,<asp:Label runat="server" ID="lblSignCmpName"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 30px;" class="print9pt">
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
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
