<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptVoucherForGST.aspx.cs"
    Inherits="Report_rptVoucherForGST" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voucher For GST</title>
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
</head>
<body>
    <form id="form1" runat="server">
    <div class="noprint">
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
                    <asp:Label runat="server" ID="lblDocno" Visible="false" Text='<%#Eval("Doc_No") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:Label runat="server" ID="lbltype" Visible="false" Text='<%#Eval("Tran_Type") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:DataList runat="server" ID="dtl" Width="100%" OnItemDataBound="dtl_OnItemDataBound">
                        <ItemTemplate>
                            <table id="tbMain" runat="server" align="center" style="table-layout: fixed;" width="1000px"
                                class="print">
                                <tr>
                                    <%--  <td style="width: 20%;" colspan="2">
                                        <img alt="image" src="../Images/Logo.jpg" class="img img-responsive" height="40px"
                                            width="100%" />
                                    </td>--%>
                                    <td style="width: 20%; vertical-align: top;" align="center" class="toosmall">
                                        <asp:Image runat="server" ID="imgLogo" ImageUrl="../Images/Logo.jpg" Width="100%"
                                            Height="40%" />
                                    </td>
                                    <td style="width: 80%; vertical-align: top;" align="left" class="toosmall">
                                        <table width="100%" style="table-layout: fixed;" class="toosmall">
                                            <tr>
                                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="toosmall">
                                                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" CssClass="toosmall"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red; font-size: medium"
                                                    class="toosmall">
                                                    <asp:Label ID="Label22" runat="server" Text="" Font-Bold="true" CssClass="toosmall"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                    <asp:Label runat="server" ID="lblAl1" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                    <asp:Label runat="server" ID="lblAl2" ForeColor="Blue" CssClass="toosmall">  </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                    <asp:Label runat="server" ID="lblAl3" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                    <asp:Label runat="server" ID="lblAl4" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                    <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: solid 1px black;">
                                        Voucher No:<asp:Label runat="server" ID="lblVoucherNo" Font-Bold="true" Text='<%#Eval("VoucherNo") %>'></asp:Label>
                                        <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("Doc_No") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right" style="border-bottom: solid 1px black;">
                                        Date:
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Doc_Date") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;
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
                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
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
                                    <td align="left" colspan="2">
                                        <table width="100%" cellspacing="2" cellpadding="0" style="table-layout: fixed; height: 25px;"
                                            class="print9pt">
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
                                                    Lorry No:&nbsp;&nbsp;
                                                    <asp:Label ID="lblLorryNo" runat="server" Text='<%#Eval("Lorry_No") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                        ID="lblDriverMobile" runat="server" Text='<%#Eval("driver_no") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    <asp:Label runat="server" ID="lblBroker" Text='<%#Eval("BrokerShortNew") %>'></asp:Label>
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
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 15px;">
                                        Particulars:-&nbsp;Quintal : &nbsp;&nbsp;<asp:Label ID="lblQuantal" runat="server"
                                            Text='<%#Eval("Quantal") %>'></asp:Label>&nbsp;&nbsp; Bags:&nbsp;&nbsp;<asp:Label
                                                ID="lblBags" runat="server" Text='<%#Eval("BAGS") %>'></asp:Label>&nbsp;&nbsp;
                                        Grade:&nbsp;&nbsp;<asp:Label ID="lblGrade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>&nbsp;&nbsp;
                                        Mill Rate:&nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text='<%#Eval("Mill_Rate") %>'></asp:Label>&nbsp;&nbsp;
                                        &nbsp;&nbsp;<asp:Label ID="Label13" runat="server" Text='<%#Eval("SR_PR") %>'></asp:Label>&nbsp;&nbsp;
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
                                        <asp:Label ID="lblMillAmount" Visible="false" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;" align="right">
                                        <table align="left">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label12" runat="server" Text='<%#Eval("PO_Details") %>'></asp:Label>
                                                </td>
                                                <%-- <td align="left">
                                                    <asp:Label ID="Label14" runat="server" Text='<%#Eval("ASN_GRN") %>'></asp:Label>
                                                </td>--%>
                                            </tr>
                                        </table>
                                        <table width="50%" align="right" class="small" style="table-layout: fixed; height: 145px;">
                                            <tr>
                                                <td align="left">
                                                    rate diff debit/credit your a/c:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("Diff_Rate").ToString()=="0.00" || Eval("Diff_Rate").ToString()=="0"?"":Eval("Diff_Rate","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    CGST%:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label15" runat="server" Text='<%#Eval("CGSTRate").ToString()=="0.00" || Eval("CGSTRate").ToString()=="0"?"":Eval("CGSTRate","{0}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label20" runat="server" Text='<%#Eval("CGSTAmount").ToString()=="0.00" || Eval("CGSTAmount").ToString()=="0"?"":Eval("CGSTAmount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    SGST%:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label16" runat="server" Text='<%#Eval("SGSTRate").ToString()=="0.00" || Eval("SGSTRate").ToString()=="0"?"":Eval("SGSTRate","{0}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label19" runat="server" Text='<%#Eval("SGSTAmount").ToString()=="0.00" || Eval("SGSTAmount").ToString()=="0"?"":Eval("SGSTAmount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    IGST %:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label18" runat="server" Text='<%#Eval("IGSTRate").ToString()=="0.00" || Eval("IGSTRate").ToString()=="0"?"":Eval("IGSTRate","{0}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label17" runat="server" Text='<%#Eval("IGSTAmount").ToString()=="0.00" || Eval("IGSTAmount").ToString()=="0"?"":Eval("IGSTAmount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Bank Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBankCommission" runat="server" Text='<%#Eval("BANK_COMMISSION").ToString()=="0.00" || Eval("BANK_COMMISSION").ToString()=="0"?"":Eval("BANK_COMMISSION","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Brokrage:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBrokrage" runat="server" Text='<%#Eval("Brokrage").ToString()=="0.00" || Eval("Brokrage").ToString()=="0"?"":Eval("Brokrage","{0}")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Tender Fees:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblQualityDiff" runat="server" Text='<%#Eval("RATEDIFF").ToString()=="0.00" || Eval("RATEDIFF").ToString()=="0"?"":Eval("RATEDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCommission" runat="server" Text='<%#Eval("Commission_Amount").ToString()=="0.00" || Eval("Commission_Amount").ToString()=="0"?"":Eval("Commission_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Freight & Other Exp:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FREIGHT").ToString()=="0.00" || Eval("FREIGHT").ToString()=="0"?"":Eval("FREIGHT","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Post and Phone:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblPostAmt" runat="server" Text='<%#Eval("Postage").ToString()=="0.00" || Eval("Postage").ToString()=="0"?"":Eval("Postage","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Interest:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest").ToString()=="0.00" || Eval("Interest").ToString()=="0"?"":Eval("Interest","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Frieght Advance:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("Cash_Ac_Amount").ToString()=="0.00" || Eval("Cash_Ac_Amount").ToString()=="0"?"":Eval("Cash_Ac_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Other:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblOther" runat="server" Text='<%#Eval("OTHER_Expenses").ToString()=="0.00" || Eval("OTHER_Expenses").ToString()=="0"?"":Eval("OTHER_Expenses","{0}") %>'></asp:Label>
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
                                                    Credit/Debit account total
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
                                        <asp:Label ID="lblNarration" Font-Bold="true" runat="server" Text="Narration:"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="Label21" runat="server" Text='<%#Eval("Narration5") %>'></asp:Label>
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
                                                                <asp:Label ID="lblNarration1" Font-Bold="true" runat="server" Text='<%#Eval("Narration1") %>'
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 30%;" rowspan="2" align="right">
                                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%;" align="left">
                                                                <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("ASN_No") %>'
                                                                    Visible="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;" align="right">
                                                    <span style="float: left;">Our TIN:
                                                        <asp:Label runat="server" ID="lblCmptinNo" Text="" Font-Bold="true"></asp:Label>&nbsp;&nbsp;FSSAI
                                                        No:<asp:Label runat="server" ID="lblCompnayFSSAI_No" Text="" Font-Bold="true"></asp:Label></span>For,<asp:Label
                                                            runat="server" ID="lblSignCmpName"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCompanyGSTNo" Text="" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 25px;" class="print">
                                        1)Please credit the amount in our account and send the amount by RTGS immediately.
                                        <br />
                                        2)This is computer generated print No Signature Required.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 110px; width: 100%;">
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
                    <asp:Label runat="server" ID="lblDocno" Visible="false" Text='<%#Eval("Doc_No") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:Label runat="server" ID="lbltype" Visible="false" Text='<%#Eval("Tran_Type") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:DataList runat="server" ID="dtl" Width="100%" OnItemDataBound="dtl_OnItemDataBound">
                        <ItemTemplate>
                            <table id="tbMain" runat="server" align="center" style="table-layout: fixed;" width="1000px"
                                class="print">
                                <tr>
                                    <td style="width: 100%;" colspan="2">
                                        <table width="100%" style="table-layout: fixed; height: 150px;" class="noprint9pt">
                                            <tr>
                                                <td style="width: 20%; vertical-align: top;" align="center" class="noprint9pt">
                                                </td>
                                                <td style="width: 80%; vertical-align: top;" align="left">
                                                    <table width="100%" style="table-layout: fixed; margin-top: 20px;">
                                                        <tr>
                                                            <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprint9pt">
                                                                <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprint9pt">
                                                                <asp:Label ID="Label5" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                                <asp:Label runat="server" ID="lblAl1" ForeColor="White"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                                <asp:Label runat="server" ID="lblAl2" ForeColor="White">  </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                                <asp:Label runat="server" ID="lblAl3" ForeColor="White"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                                <asp:Label runat="server" ID="lblAl4" ForeColor="White"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                                <asp:Label runat="server" ID="lblOtherDetails" ForeColor="White"> </asp:Label>
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
                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
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
                                    <td align="left" colspan="2">
                                        <table width="100%" cellspacing="2" cellpadding="0" style="table-layout: fixed; height: 25px;"
                                            class="print9pt">
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
                                                    Lorry No:&nbsp;&nbsp;
                                                    <asp:Label ID="lblLorryNo" runat="server" Text='<%#Eval("Lorry_No") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                        ID="lblDriverMobile" runat="server" Text='<%#Eval("driver_no") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    <asp:Label runat="server" ID="lblBroker" Text='<%#Eval("BrokerShortNew") %>'></asp:Label>
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
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 15px;">
                                        Particulars:-&nbsp;Quintal : &nbsp;&nbsp;<asp:Label ID="lblQuantal" runat="server"
                                            Text='<%#Eval("Quantal") %>'></asp:Label>&nbsp;&nbsp; Bags:&nbsp;&nbsp;<asp:Label
                                                ID="lblBags" runat="server" Text='<%#Eval("BAGS") %>'></asp:Label>&nbsp;&nbsp;
                                        Grade:&nbsp;&nbsp;<asp:Label ID="lblGrade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>&nbsp;&nbsp;
                                        Mill Rate:&nbsp;&nbsp;<asp:Label ID="Label14" runat="server" Text='<%#Eval("Mill_Rate") %>'></asp:Label>&nbsp;&nbsp;
                                        &nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text='<%#Eval("SR_PR") %>'></asp:Label>&nbsp;&nbsp;
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
                                        <asp:Label ID="lblMillAmount" Visible="false" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;" align="right">
                                        <table align="left">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label12" runat="server" Text='<%#Eval("PO_Details") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="50%" align="right" class="small" style="table-layout: fixed; height: 145px;">
                                            <tr>
                                                <td align="left">
                                                    rate diff debit/credit your a/c:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("Diff_Rate").ToString()=="0.00" || Eval("Diff_Rate").ToString()=="0"?"":Eval("Diff_Rate","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    CGST%:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label15" runat="server" Text='<%#Eval("CGSTRate").ToString()=="0.00" || Eval("CGSTRate").ToString()=="0"?"":Eval("CGSTRate","{0}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label20" runat="server" Text='<%#Eval("CGSTAmount").ToString()=="0.00" || Eval("CGSTAmount").ToString()=="0"?"":Eval("CGSTAmount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    SGST%:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label16" runat="server" Text='<%#Eval("SGSTRate").ToString()=="0.00" || Eval("SGSTRate").ToString()=="0"?"":Eval("SGSTRate","{0}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label19" runat="server" Text='<%#Eval("SGSTAmount").ToString()=="0.00" || Eval("SGSTAmount").ToString()=="0"?"":Eval("SGSTAmount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    IGST %:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label18" runat="server" Text='<%#Eval("IGSTRate").ToString()=="0.00" || Eval("IGSTRate").ToString()=="0"?"":Eval("IGSTRate","{0}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label17" runat="server" Text='<%#Eval("IGSTAmount").ToString()=="0.00" || Eval("IGSTAmount").ToString()=="0"?"":Eval("IGSTAmount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Bank Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBankCommission" runat="server" Text='<%#Eval("BANK_COMMISSION").ToString()=="0.00" || Eval("BANK_COMMISSION").ToString()=="0"?"":Eval("BANK_COMMISSION","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Brokrage:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBrokrage" runat="server" Text='<%#Eval("Brokrage").ToString()=="0.00" || Eval("Brokrage").ToString()=="0"?"":Eval("Brokrage","{0}")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Tender Fees:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblQualityDiff" runat="server" Text='<%#Eval("RATEDIFF").ToString()=="0.00" || Eval("RATEDIFF").ToString()=="0"?"":Eval("RATEDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCommission" runat="server" Text='<%#Eval("Commission_Amount").ToString()=="0.00" || Eval("Commission_Amount").ToString()=="0"?"":Eval("Commission_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Freight & Other Exp:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FREIGHT").ToString()=="0.00" || Eval("FREIGHT").ToString()=="0"?"":Eval("FREIGHT","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Post and Phone:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblPostAmt" runat="server" Text='<%#Eval("Postage").ToString()=="0.00" || Eval("Postage").ToString()=="0"?"":Eval("Postage","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Interest:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest").ToString()=="0.00" || Eval("Interest").ToString()=="0"?"":Eval("Interest","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Frieght Advance:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("Cash_Ac_Amount").ToString()=="0.00" || Eval("Cash_Ac_Amount").ToString()=="0"?"":Eval("Cash_Ac_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Other:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblOther" runat="server" Text='<%#Eval("OTHER_Expenses").ToString()=="0.00" || Eval("OTHER_Expenses").ToString()=="0"?"":Eval("OTHER_Expenses","{0}") %>'></asp:Label>
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
                                        <asp:Label ID="lblNarration" Font-Bold="true" runat="server" Text="Narration:"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="Label21" runat="server" Text='<%#Eval("Narration5") %>'></asp:Label>
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
                                                                <asp:Label ID="lblNarration1" Font-Bold="true" runat="server" Text='<%#Eval("Narration1") %>'
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                            <td style="width: 30%;" rowspan="2" align="right">
                                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%;" align="left">
                                                                <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("ASN_No") %>'
                                                                    Visible="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;" align="right">
                                                    <span style="float: left;">Our TIN:
                                                        <asp:Label runat="server" ID="lblCmptinNo" Text="" Font-Bold="true"></asp:Label>&nbsp;&nbsp;FSSAI
                                                        No:<asp:Label runat="server" ID="lblCompnayFSSAI_No" Text="" Font-Bold="true"></asp:Label></span>
                                                    For,<asp:Label runat="server" ID="lblSignCmpName"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCompanyGSTNo" Text="" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 25px;" class="print">
                                        1)Please credit the amount in our account and send the amount by RTGS immediately.
                                        <br />
                                        2)This is computer generated print No Signature Required.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 110px; width: 100%;">
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
