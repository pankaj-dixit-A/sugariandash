<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDispatchRegister.aspx.cs"
    Inherits="Report_rptDispatchRegister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('', '', 'height=660,width=1350');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body style="font-family:Calibri;font-size:12px;width:1100px; text-align:center;" >');
            printWindow.document.write('<style type = "text/css">thead {display:table-header-group;} tfoot{display:table-footer-group;}</style>');
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
        function vp(VNO, type) {
            window.open('../Report/rptVoucher.aspx?VNO=' + VNO + '&type=' + type);
        }
    </script>
</head>
<body>
    <form id="frmDispatchRegister" runat="server">
    <div align="left" style="width: 80%;">
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />&nbsp;&nbsp;<asp:Button runat="server"
                ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click" Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnPrintOrMail" Text="Print or Mail"
            Width="120px" OnClick="btnPrintOrMail_Click" />
        <%-- <asp:TextBox runat="server" ID="txtSearch" onkeyup="return findString(this);"></asp:TextBox>
        <asp:Button runat="server" ID="btnSearch" Text="find" />--%>
    </div>
    &nbsp;
    <div align="center">
        <asp:Panel ID="pnlMain" runat="server" align="center" Font-Names="Calibri" ForeColor="Black"
            Font-Size="Medium" BorderColor="Black" BorderStyle="Solid" BorderWidth="0px">
            <asp:Label ID="lblReportName" runat="server" Text="Date Wise Dispatch Register" CssClass="lblName"
                Font-Bold="true" Font-Size="Large" Style="text-align: center; width: 100%;"></asp:Label>
            <table width="80%" style="table-layout: fixed;">
                <tr>
                    <td align="left" style="width: 5%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        #
                    </td>
                    <td align="left" style="width: 9%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Mill
                    </td>
                    <td align="left" style="width: 5%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Rate
                    </td>
                    <td align="left" style="width: 5%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Quantal
                    </td>
                    <td align="left" style="width: 19%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Party
                    </td>
                    <td align="left" style="width: 10%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Truck
                    </td>
                    <td align="left" style="width: 18%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Transport
                    </td>
                    <td align="left" style="width: 12%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        DO
                    </td>
                    <td align="right" style="width: 4%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Print
                    </td>
                    <td align="right" style="width: 4%; border-bottom: 1px solid black; border-top: 1px solid black;">
                        Mail
                    </td>
                </tr>
            </table>
            <table width="80%">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList ID="DataList1" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
                            <ItemTemplate>
                                <table width="100%" style="table-layout: fixed;">
                                    <%------------------------  Line 1 -----------------------------%>
                                    <tr>
                                        <td align="left" style="width: 5%;">
                                            <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="lblMill" runat="server" Text='<%#Eval("millShortName") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5%;">
                                            <asp:Label ID="lblRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5%;">
                                            <asp:Label ID="lblQuantal" runat="server" Text='<%#Eval("quantal") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 20%;">
                                            <asp:Label ID="lblParty" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="lblTruck" runat="server" Text='<%#Eval("truck_no") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 20%;">
                                            <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("TransportName") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="lblDO" runat="server" Text='<%#Eval("DOName") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <%------------------------  Line 2 -----------------------------%>
                                    <tr>
                                        <td align="left" style="width: 5%;">
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            Broker
                                        </td>
                                        <td align="left" colspan="3" style="width: 30%;">
                                            <asp:Label ID="lblBroker" runat="server" Text='<%#Eval("BrokerName") %>'></asp:Label>
                                            S.R. &nbsp;<asp:Label ID="Label1" runat="server" Text='<%#Eval("sale_rate") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="lblGrade" runat="server" Text='<%#Eval("grade") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            Freight:<asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FreightPerQtl") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 20%;">
                                            Ref No:<asp:Label ID="lblRefNo" runat="server"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 4%;">
                                            <asp:CheckBox runat="server" ID="chkPrint" />
                                        </td>
                                        <td align="center" style="width: 4%;">
                                            <asp:CheckBox runat="server" ID="chkMail" />
                                        </td>
                                    </tr>
                                    <%------------------------  Line 3 -----------------------------%>
                                    <tr>
                                        <td align="left" style="width: 5%;">
                                            ||
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            <asp:Label ID="lblPurcNo" runat="server" Text='<%#Eval("purc_no") %>'></asp:Label>&nbsp;<asp:Label
                                                ID="lblpurcorder" runat="server" Text='<%#Eval("purc_order") %>'></asp:Label>
                                        </td>
                                        <td align="left" colspan="2" style="width: 10%;">
                                            ||
                                            <asp:Label ID="lblNarration" runat="server" Text='<%#Eval("Narration1") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 20%;">
                                            Advance Freight:
                                            <asp:Label runat="server" ID="lblAdvanceFrieght" Font-Bold="true" Text='<%#Eval("Memo_Advance") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="width: 10%;">
                                            Voucher No:
                                            <asp:Label runat="server" ID="lblVoucherNo" Font-Bold="true" Text='<%#Eval("voucher_no") %>'></asp:Label>
                                        </td>
                                        <td align="left" colspan="3" style="width: 30%;">
                                            Final:
                                            <asp:Label runat="server" ID="lblFinalAmount" Font-Bold="true" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <%--------------------------- Line 4 -------------------------------------%>
                                    <tr>
                                        <td align="left" colspan="5" style="width: 45px;">
                                            <asp:Label ID="lblDispatchTo" Font-Bold="true" Text='<%#Eval("GetPassName") %>' runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5" style="width: 45px;">
                                            <asp:Label ID="lblPaymentTo" Font-Bold="true" runat="server" Text='<%#Eval("VoucherByname") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="5" style="width: 45px;">
                                        </td>
                                        <td align="right" colspan="3" style="width: 40px;">
                                            <asp:Label ID="lblBillTo" Font-Bold="true" runat="server" Text='<%#Eval("narration4") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10" style="width: 100%; border-bottom: dashed 1px black;">
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
    <div id="vouchermail" style="display: none;">
        <asp:Panel ID="pnlVoucher" runat="server" align="center" Font-Names="Calibri">
            <asp:DataList ID="dtlVoucher" runat="server" Width="100%" OnItemDataBound="dtlVoucher_ItemDataBound">
                <ItemTemplate>
                    <table id="tbMain" runat="server" width="1000px" align="center" style="page-break-after: avoid;">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblCompany" runat="server" Font-Bold="true" Text="Shri Hingalj Enterprises"
                                    Font-Size="Large"></asp:Label>
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
                                Voucher No.:
                                <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("Doc_No") %>'></asp:Label>
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
                            <td align="left">
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
                                <asp:Label ID="lblMillNameFull" runat="server" Text='<%#Eval("MillName") %>'></asp:Label><br />
                                Credit the same to our account & debit to mill's account
                            </td>
                            <td align="right" style="border-bottom: solid 1px black;">
                                <asp:Label ID="lblMillAmount" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-bottom: solid 1px black;">
                                <table width="60%" align="right">
                                    <tr>
                                        <td align="left">
                                            Rate Diff Debit/Credit your A/C
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("RATEDIFF") %>'></asp:Label>
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
                                <table width="60%" align="right">
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
                                            For Shri Hingalj Enterprises.
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
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
