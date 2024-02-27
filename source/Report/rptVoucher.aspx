<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptVoucher.aspx.cs" Inherits="Report_rptVoucher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .pagebreak
        {
            page-break-after: always;
        }
        #header
        {
            height: 1px;
            display: none;
        }
    </style>
    <link rel="stylesheet" href="../print.css" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
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
    <script language="javascript" type="text/javascript">
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'letf=0,top=0,width=1100,height=600,toolbar=0,scrollbars=0,status=0');
            WinPrint.document.write('<html><head><link rel="stylesheet" href="../print.css" type="text/css" media="print" />');
            WinPrint.document.write('</head><body>');
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write('</body></html>');
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            prtContent.innerHTML = strOldOne;
        }
    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function Print() {
            var count = $('[id*=tbMain]').length;
            var pagebreakcount = 1;
            var i = 1;
            $('[id*=tbMain]').each(function () {
                if (i % pagebreakcount == 0) {
                    $(this).attr('style', 'page-break-after: always');
                }
                i++;
            });
            var divContents = document.getElementById("PrintPanel").innerHTML;
            var printWindow = window.open('', '', 'height=200,width=400');
            printWindow.document.write('<html><link rel="stylesheet" href="../print.css" type="text/css" media="print" /><head>');
            printWindow.document.write('</head><body>');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
            Redirect();
        };
        function Redirect() {
            setTimeout(function () {
                location.href = location.href;
            }, 5000);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="left" style="border-bottom: 1px dashed black; height: 30px;" class="noprint">
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
        &nbsp;<asp:Button runat="server" ID="btnExportToPdf" Text="PDF" OnClick="btnExportToPdf_Click1" />
    </div>
    <div>
        <asp:Panel ID="PrintPanel" runat="server" align="center" Font-Names="Calibri" CssClass="print">
            <%--<div id="divPrint" class="print">--%>
            <asp:DataList ID="DataList1" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound"
                CssClass="print">
                <ItemTemplate>
                    <div class="noprint" style="width: 0.01px;">
                        <asp:Label runat="server" ID="lblvocno" Visible="false" Text='<%#Eval("Doc_No") %>'></asp:Label>
                        <asp:Label runat="server" ID="lblvoctype" Visible="false" Text='<%#Eval("Tran_Type") %>'></asp:Label>
                    </div>
                    <asp:Panel ID="pnlMain" runat="server">
                        <asp:DataList runat="server" ID="dtlDetails" Width="100%" OnItemDataBound="dtlDetails_ItemDataBound">
                            <ItemTemplate>
                                <table id="tbMain" runat="server" align="center" class="print" width="1000px">
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblCompanyAddr" runat="server" Text="Solapur"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="lblCompanyMobile" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                            <asp:Label runat="server" ID="lblVoucherNo" Font-Bold="true" Text='<%#Eval("VoucherNo") %>'></asp:Label>
                                            <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("Doc_No") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                            Date:
                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Doc_Date") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            To,
                                        </td>
                                        <td align="left">
                                            LIC No.:<asp:Label ID="lblLICNo" runat="server" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblPartyName" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            CST NO.:<asp:Label ID="lblCSTNo" runat="server" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 70%">
                                            <asp:Label ID="lblPartyAddr" runat="server" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            GST No.:<asp:Label ID="lblGSTNo" runat="server" Text='<%#Eval("Gst_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblPartyCity" runat="server" Font-Bold="true" Text='<%#Eval("City_Code") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            TIN No.:<asp:Label ID="lblTinNo" runat="server" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Dispatched From: &nbsp;<asp:Label ID="lblDispatchFrom" runat="server" Text='<%#Eval("From_Place") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            To:&nbsp;<asp:Label ID="lblTo" runat="server" Text='<%#Eval("To_Place") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Lorry NO:
                                            <asp:Label ID="lblLorryNo" runat="server" Text='<%#Eval("Lorry_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                            Particulars:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                            Quantal : &nbsp;&nbsp;<asp:Label ID="lblQuantal" runat="server" Text='<%#Eval("Quantal") %>'></asp:Label>&nbsp;&nbsp;
                                            Bags:&nbsp;&nbsp;<asp:Label ID="lblBags" runat="server" Text='<%#Eval("BAGS") %>'></asp:Label>&nbsp;&nbsp;
                                            Grade:&nbsp;&nbsp;<asp:Label ID="lblGrade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>&nbsp;&nbsp;
                                            Rate:&nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text='<%#Eval("Mill_Rate") %>'></asp:Label>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="border-bottom: solid 1px black;">
                                            We have paid on your behalf in account of<br />
                                            <asp:Label ID="lblMillNameFull" runat="server" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label><br />
                                            Credit the same to our account & debit to mill's account
                                        </td>
                                        <td align="right" style="border-bottom: solid 1px black;">
                                            <asp:Label ID="lblMillAmount" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;" align="right">
                                            <table width="50%" align="right">
                                                <tr>
                                                    <td align="left">
                                                        Rate Diff Debit/Credit your A/C:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("LESSDIFF") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Bank Commission:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblBankCommission" runat="server" Text='<%#Eval("BANK_COMMISSION") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Brokrage:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblBrokrage" runat="server" Text='<%#Eval("Brokrage") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Quality Difference:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblQualityDiff" runat="server" Text='<%#Eval("RATEDIFF") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Commission:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblCommission" runat="server" Text='<%#Eval("Commission_Amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Freight & Other Exp:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FREIGHT") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Post and Phone:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblPostAmt" runat="server" Text='<%#Eval("Postage") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Interest:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Transport:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("Transport_Amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 40%;">
                                                        Other:
                                                    </td>
                                                    <td align="right" style="width: 20%;">
                                                        <asp:Label ID="lblOther" runat="server" Text='<%#Eval("OTHER_Expenses") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                            <table width="100%" align="right">
                                                <tr>
                                                    <td align="left" style="width: 40%;">
                                                        Total:
                                                    </td>
                                                    <td align="right" style="width: 20%;">
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                            <table width="100%" align="right">
                                                <tr>
                                                    <td align="left" style="width: 40%;">
                                                        Please Debit/Credit account total
                                                    </td>
                                                    <td align="left" style="width: 40%;">
                                                        TT/Draft Rs.:
                                                    </td>
                                                    <td align="right" style="width: 20%;">
                                                        <asp:Label ID="lblTotalAmt" Font-Bold="true" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
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
                                            <table width="100%">
                                                <tr>
                                                    <td style="height: 30px;">
                                                        <asp:Label ID="lblNarration1" Font-Bold="true" runat="server" Text='<%#Eval("Narration1") %>'></asp:Label>
                                                    </td>
                                                    <td style="height: 30px;">
                                                        <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("Narration2") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="height: 30px;">
                                                        For
                                                        <asp:Label runat="server" ID="lblCompanyBottom"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-bottom: solid 1px black;">
                                            1)Please credit the amount in our account and send the amount by DD/TT immediately.
                                            <br />
                                            2)If the amount is not sent before the due date payment charges Rs. 2/- per bag
                                            per day will be charged.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 190px">
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList></asp:Panel>
                </ItemTemplate>
            </asp:DataList><%--</div>--%>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
