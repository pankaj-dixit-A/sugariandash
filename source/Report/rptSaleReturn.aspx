<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptSaleReturn.aspx.cs" Inherits="Report_rptSaleReturn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=dtlist.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print"/>');
            printWindow.document.write('</head><body class="print">');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }
    </script>
    <script type="text/javascript">
        function PrintPanel2() {
            var panel = document.getElementById("<%=dtlist1.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print"/>');
            printWindow.document.write('</head><body class="print">');
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
        <asp:Button ID="btnPrint" runat="server" Text="Print" Width="80px" OnClientClick="return PrintPanel();" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnPriPrinted" Text="Pre-Printed"
            OnClientClick="return PrintPanel2();" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Mail" OnClick="btnSendEmail_Click"
            Width="58px" />&nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
    </div>
    <br />
    <div>
        <asp:Panel runat="server" ID="pnlMain" CssClass="print">
            <table width="70%" align="center" cellspacing="4" cellpadding="0" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table id="tblMain" runat="server" width="100%" align="center" cellspacing="1" cellpadding="0"
                                    style="table-layout: fixed; height: 145px; border-bottom: 1px solid black;">
                                    <tr id="row1" runat="server">
                                        <td id="td1" runat="server" style="width: 20%; vertical-align: middle;" align="left"
                                            rowspan="3">
                                            TIN:
                                            <asp:Label runat="server" ID="lblCmpTIN"></asp:Label>
                                        </td>
                                        <td id="td2" runat="server" style="width: 50%;" align="center">
                                            <asp:Label runat="server" ID="lblCompanyName" Font-Bold="true" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td id="td3" runat="server" style="width: 30%;" align="left">
                                            Fax:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="lblCmpFAX"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="row2" runat="server">
                                        <td align="center" id="td4" runat="server" style="vertical-align: top;">
                                            <asp:Label runat="server" ID="lblCompanyAddress"></asp:Label>
                                        </td>
                                        <td align="left" id="td5" runat="server" style="vertical-align: top;">
                                            Mobile:
                                            <asp:Label runat="server" ID="lblCmpMobile"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="row3" runat="server">
                                        <td align="center" id="td6" runat="server" style="vertical-align: top;">
                                            <asp:Label runat="server" ID="lblCityStatePin"></asp:Label>
                                        </td>
                                        <td align="left" id="td7" runat="server" style="vertical-align: top;">
                                            E-Mail:
                                            <asp:Label runat="server" ID="lblCmpEmail"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="print">
                                    <tr>
                                        <td align="left">
                                            Credit Note No: &nbsp;
                                            <asp:Label runat="server" ID="lblSB_No" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right">
                                            Date:&nbsp;<asp:Label runat="server" ID="lblBillDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-top: 1px solid black;">
                                            <table width="100%" style="table-layout: fixed; height: 160px;">
                                                <tr>
                                                    <td align="left" style="width: 65%;">
                                                        Buyers Name & Address,
                                                    </td>
                                                    <td align="left" style="width: 35%; vertical-align: top;" rowspan="5">
                                                        <table width="100%" align="center" style="table-layout: fixed; vertical-align: text-top;">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="lblPartySLN" Text='<%#Eval("Party_SLN") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="lblPartyTIN" Text='<%#Eval("Party_TIN") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label5" Text='<%#Eval("Party_Cst") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label6" Text='<%#Eval("Party_Gst") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("Party_Ecc") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblBuyerName" Text='<%#Eval("Party_Name") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="3" align="left" style="vertical-align: top;">
                                                        <asp:Label runat="server" ID="lblPartyAddress" Text='<%#Eval("Party_Address") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        City:
                                                        <asp:Label runat="server" ID="lblPartyCity" Text='<%#Eval("Party_City") %>' Font-Bold="true"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State:
                                                        <asp:Label runat="server" ID="lblPartyState" Font-Bold="true" Text='<%#Eval("Party_State") %>'></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Pin_Code:<asp:Label runat="server"
                                                            ID="lblPartyPincode" Font-Bold="true" Text='<%#Eval("Party_Pin") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="print">
                                    <tr>
                                        <td colspan="2" align="left">
                                            Mill Name:&nbsp;<asp:Label runat="server" ID="lblMillName" Text='<%#Eval("Mill_Name") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;" align="left">
                                            Dispatched From:&nbsp;<asp:Label runat="server" ID="lblDispatchedFrom" Text='<%#Eval("From_Place") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 50%;" align="left">
                                            To:&nbsp;<asp:Label runat="server" ID="Label3" Text='<%#Eval("To_Place") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Lorry No:&nbsp;<asp:Label runat="server" ID="lbllorry" Text='<%#Eval("lorry") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            Driver Mobile:&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="print">
                                    <tr>
                                        <td style="width: 30%;" align="left">
                                            <b>Particulars</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Quintal</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Packing(kg)</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Bags</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Rate</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Value</b>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" style="table-layout: fixed;" align="center">
                                    <tr>
                                        <td style="width: 100%; vertical-align: top; height: 70px;">
                                            <asp:DataList runat="server" ID="dtItemDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%" align="center" cellspacing="0px" style="table-layout: fixed;
                                                        border-bottom: 1px dashed black;" class="print">
                                                        <tr>
                                                            <td style="width: 30%;" align="left">
                                                                <asp:Label runat="server" ID="lblGrade" Text='<%#Eval("Item") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblQntl" Text='<%#Eval("Qntl") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblPacking" Text='<%#Eval("Packing") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblBags" Text='<%#Eval("Bags") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblRate" Text='<%#Eval("Rate") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblvalue" Text='<%#Eval("Value") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="2" style="font-size: medium; table-layout: fixed;
                                    margin-top: 45px; border-bottom: 1px solid black; height: 190px;" class="print">
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Sub Total:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="Label2" Text='<%#Eval("Sub_Total") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Vat 0%:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="Label1" Text="Nil"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Less Frieght:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblLessFrieght" Text='<%#Eval("Less_Frieght").ToString()=="0.00"?"":Eval("Less_Frieght","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Cash Advance:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblCashAdvance" Text='<%#Eval("Cash_Advance").ToString()=="0.00"?"":Eval("Cash_Advance","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Bank Commission:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblBankComm" Text='<%#Eval("Bank_Commission").ToString()=="0.00"?"":Eval("Bank_Commission","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Other Expense:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblOtherExpe" Text='<%#Eval("Other_Expenses").ToString()=="0.00"?"":Eval("Other_Expenses","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%; border-bottom: 1px solid black;" align="right">
                                            <b>Total Amount:</b>
                                        </td>
                                        <td style="width: 20%; border-bottom: 1px solid black;" align="right">
                                            <asp:Label runat="server" ID="lblBillAmount" Text='<%#Eval("Bill_Amount") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            Rs.:<asp:Label runat="server" ID="lblInwords" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="0" class="print" style="table-layout: fixed;
                                    height: 120px;">
                                    <tr>
                                        <td align="left">
                                            Our TIN No.:
                                            <asp:Label runat="server" ID="lblCmptinNo" Text="27770980728" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 70%;" align="left">
                                            <b><u>Note:</u></b>&nbsp;After Dispatch of the goods we are not responsible for
                                            non delivery or any kind of damage.
                                        </td>
                                        <td rowspan="3" align="right">
                                            <asp:Image Width="200px" Height="50px" runat="server" ID="imgSign" /><br />
                                            For,<asp:Label runat="server" ID="lblNameCmp" Font-Italic="true" Font-Bold="true"></asp:Label><br />
                                            <p style="font-size: large; font-style: italic;">
                                                Authorised Signatory</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            1) Please credit the amount in our account and send the amount by RTGS immediately.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            2) If the amount is not sent before the due date payment Interest 24% will be charged.
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
        <asp:Panel runat="server" ID="Panel1" CssClass="print">
            <table width="70%" align="center" cellspacing="4" cellpadding="0" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist1" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table id="tblMain" runat="server" width="100%" align="center" cellspacing="1" cellpadding="0"
                                    style="table-layout: fixed; height: 145px;" class="noprint">
                                    <tr id="row1" runat="server">
                                        <td id="td1" runat="server" style="width: 20%; vertical-align: middle;" align="left"
                                            rowspan="3">
                                            TIN:
                                            <asp:Label runat="server" ID="lblCmpTIN"></asp:Label>
                                        </td>
                                        <td id="td2" runat="server" style="width: 50%;" align="center">
                                            <asp:Label runat="server" ID="lblCompanyName" Font-Bold="true" Font-Size="Large"></asp:Label>
                                        </td>
                                        <td id="td3" runat="server" style="width: 30%;" align="left">
                                            Fax:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="lblCmpFAX"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="row2" runat="server">
                                        <td align="center" id="td4" runat="server" style="vertical-align: top;">
                                            <asp:Label runat="server" ID="lblCompanyAddress"></asp:Label>
                                        </td>
                                        <td align="left" id="td5" runat="server" style="vertical-align: top;">
                                            Mobile:
                                            <asp:Label runat="server" ID="lblCmpMobile"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="row3" runat="server">
                                        <td align="center" id="td6" runat="server" style="vertical-align: top;">
                                            <asp:Label runat="server" ID="lblCityStatePin"></asp:Label>
                                        </td>
                                        <td align="left" id="td7" runat="server" style="vertical-align: top;">
                                            E-Mail:
                                            <asp:Label runat="server" ID="lblCmpEmail"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;
                                    border-top: 1px solid black;" class="print">
                                    <tr>
                                        <td align="left">
                                            Credit Note No: &nbsp;
                                            <asp:Label runat="server" ID="lblSB_No" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right">
                                            Date:&nbsp;<asp:Label runat="server" ID="lblBillDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="border-top: 1px solid black;">
                                            <table width="100%" style="table-layout: fixed; height: 160px;">
                                                <tr>
                                                    <td align="left" style="width: 65%;">
                                                        Buyers Name & Address,
                                                    </td>
                                                    <td align="left" style="width: 35%; vertical-align: top;" rowspan="5">
                                                        <table width="100%" align="center" style="table-layout: fixed; vertical-align: text-top;">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="lblPartySLN" Text='<%#Eval("Party_SLN") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="lblPartyTIN" Text='<%#Eval("Party_TIN") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label5" Text='<%#Eval("Party_Cst") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label6" Text='<%#Eval("Party_Gst") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label4" Text='<%#Eval("Party_Ecc") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label runat="server" ID="lblBuyerName" Text='<%#Eval("Party_Name") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td rowspan="3" align="left" style="vertical-align: top;">
                                                        <asp:Label runat="server" ID="lblPartyAddress" Text='<%#Eval("Party_Address") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        City:
                                                        <asp:Label runat="server" ID="lblPartyCity" Text='<%#Eval("Party_City") %>' Font-Bold="true"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; State:
                                                        <asp:Label runat="server" ID="lblPartyState" Font-Bold="true" Text='<%#Eval("Party_State") %>'></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Pin_Code:<asp:Label runat="server"
                                                            ID="lblPartyPincode" Font-Bold="true" Text='<%#Eval("Party_Pin") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="print">
                                    <tr>
                                        <td colspan="2" align="left">
                                            Mill Name:&nbsp;<asp:Label runat="server" ID="lblMillName" Text='<%#Eval("Mill_Name") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;" align="left">
                                            Dispatched From:&nbsp;<asp:Label runat="server" ID="lblDispatchedFrom" Text='<%#Eval("From_Place") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 50%;" align="left">
                                            To:&nbsp;<asp:Label runat="server" ID="Label3" Text='<%#Eval("To_Place") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Lorry No:&nbsp;<asp:Label runat="server" ID="lbllorry" Text='<%#Eval("lorry") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            Driver Mobile:&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="print">
                                    <tr>
                                        <td style="width: 30%;" align="left">
                                            <b>Particulars</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Quintal</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Packing(kg)</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Bags</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Rate</b>
                                        </td>
                                        <td style="width: 10%;" align="center">
                                            <b>Value</b>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" style="table-layout: fixed;" align="center">
                                    <tr>
                                        <td style="width: 100%; vertical-align: top; height: 70px;">
                                            <asp:DataList runat="server" ID="dtItemDetails" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%" align="center" cellspacing="0px" style="table-layout: fixed;
                                                        border-bottom: 1px dashed black;" class="print">
                                                        <tr>
                                                            <td style="width: 30%;" align="left">
                                                                <asp:Label runat="server" ID="lblGrade" Text='<%#Eval("Item") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblQntl" Text='<%#Eval("Qntl") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblPacking" Text='<%#Eval("Packing") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblBags" Text='<%#Eval("Bags") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblRate" Text='<%#Eval("Rate") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 10%;" align="center">
                                                                <asp:Label runat="server" ID="lblvalue" Text='<%#Eval("Value") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="2" style="font-size: medium; table-layout: fixed;
                                    margin-top: 45px; border-bottom: 1px solid black; height: 190px;" class="print">
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Sub Total:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="Label2" Text='<%#Eval("Sub_Total") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Vat 0%:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="Label1" Text="Nil"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Less Frieght:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblLessFrieght" Text='<%#Eval("Less_Frieght").ToString()=="0.00"?"":Eval("Less_Frieght","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Cash Advance:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblCashAdvance" Text='<%#Eval("Cash_Advance").ToString()=="0.00"?"":Eval("Cash_Advance","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Bank Commission:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblBankComm" Text='<%#Eval("Bank_Commission").ToString()=="0.00"?"":Eval("Bank_Commission","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Other Expense:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblOtherExpe" Text='<%#Eval("Other_Expenses").ToString()=="0.00"?"":Eval("Other_Expenses","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%; border-bottom: 1px solid black;" align="right">
                                            <b>Total Amount:</b>
                                        </td>
                                        <td style="width: 20%; border-bottom: 1px solid black;" align="right">
                                            <asp:Label runat="server" ID="lblBillAmount" Text='<%#Eval("Bill_Amount") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            Rs.:<asp:Label runat="server" ID="lblInwords" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="0" class="print" style="table-layout: fixed;
                                    height: 120px;">
                                    <tr>
                                        <td align="left">
                                            Our TIN No.:
                                            <asp:Label runat="server" ID="lblCmptinNo" Text="27770980728" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 70%;" align="left">
                                            <b><u>Note:</u></b>&nbsp;After Dispatch of the goods we are not responsible for
                                            non delivery or any kind of damage.
                                        </td>
                                        <td rowspan="3" align="right">
                                            <asp:Image Width="200px" Height="50px" runat="server" ID="imgSign" /><br />
                                            For,<asp:Label runat="server" ID="lblNameCmp" Font-Italic="true" Font-Bold="true"></asp:Label><br />
                                            <p style="font-size: large; font-style: italic;">
                                                Authorised Signatory</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            1) Please credit the amount in our account and send the amount by RTGS immediately.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            2) If the amount is not sent before the due date payment Interest 24% will be charged.
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
