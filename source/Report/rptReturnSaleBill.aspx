<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptReturnSaleBill.aspx.cs"
    Inherits="Report_rptReturnSaleBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sale Bill</title>
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
        <asp:Button ID="btnPrint" runat="server" Text="Print" Width="80px" OnClientClick="return PrintPanel();"
            OnClick="btnPrint_Click" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnPriPrinted" Text="Pre-Printed"
            OnClick="btnPriPrinted_Click" OnClientClick="return PrintPanel2();" />
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
                                <table width="100%" style="table-layout: fixed; height: 125px;" class="print9pt">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center">
                                            <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/Logo.jpg" Width="100%"
                                                Height="30%" />
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
                                                        <asp:Label ID="Label8" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
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
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;
                                    border-top: 1px solid black;" class="print">
                                    <tr>
                                        <td align="left">
                                            Invoice No: &nbsp;
                                            <asp:Label runat="server" ID="lblSB_No" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right">
                                            Invoice Date:&nbsp;<asp:Label runat="server" ID="lblBillDate" Text='<%#Eval("dt") %>'
                                                Font-Bold="true"></asp:Label>
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
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label7" Text='<%#Eval("Party_PAN") %>'></asp:Label>
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
                                                        <asp:Label runat="server" ID="lblPartyAddress" Text='<%#Eval("Party_Address") %>'></asp:Label><br />
                                                        <asp:Label runat="server" ID="lblPhone" Text='<%#Eval("Party_Phone") %>'></asp:Label>
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
                                        <td style="width: 60%;" align="left">
                                            Mill Name:&nbsp;<asp:Label runat="server" ID="lblMillName" Text='<%#Eval("Mill_Name") %>'></asp:Label>
                                        </td>
                                        <td style="width: 40%;" align="left">
                                            <asp:Label runat="server" ID="Label9" Text='<%#Eval("PODetails") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 60%;" align="left">
                                            Dispatched From:&nbsp;<asp:Label runat="server" ID="lblDispatchedFrom" Text='<%#Eval("From_Place") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 40%;" align="left">
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
                                        <td style="width: 100%; vertical-align: top; height: 95px;">
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
                                            <asp:Label runat="server" ID="lblLessFrieght" Text='<%#Eval("Less_Frieght").ToString()=="0.00" || Eval("Less_Frieght").ToString()=="0"?"":Eval("Less_Frieght","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Cash Advance:</b>
                                        </td>
                                        <td style="width: 20%;" align="right">
                                            <asp:Label runat="server" ID="lblCashAdvance" Text='<%#Eval("Cash_Advance").ToString()=="0.00" || Eval("Cash_Advance").ToString()=="0"?"":Eval("Cash_Advance","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 80%;" align="right">
                                            <b>Bank Commission:</b>
                                            <%-- <b>Rate Diff:</b><asp:Label runat="server" ID="lblBankCommRate" Text='<%#Eval("RateDiff").ToString()=="0.00"?"":Eval("RateDiff","{0}") %>'></asp:Label>/Qntl:--%>
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
                                <table width="100%" align="center" cellspacing="0" class="print9pt" style="table-layout: fixed;
                                    height: 120px;">
                                    <tr>
                                        <td align="left">
                                            Our TIN No.:
                                            <asp:Label runat="server" ID="lblCmptinNo" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 70%;" align="left">
                                            <b><u>Note:</u></b>&nbsp;After Dispatch of the goods we are not responsible for
                                            non delivery or any kind of damage.
                                        </td>
                                        <td rowspan="3" align="right">
                                            <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" /><br />
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
                                    <tr>
                                        <td style="width: 100%; height: 30px;">
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
                                <table width="100%" style="table-layout: fixed; height: 125px;" class="noprint9pt">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center">
                                            <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/Logo.jpg" Width="100%"
                                                Height="30%" />
                                        </td>
                                        <td style="width: 80%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed;">
                                                <tr>
                                                    <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprint9pt">
                                                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprint9pt">
                                                        <asp:Label ID="Label8" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                        <asp:Label runat="server" ID="lblAl1" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                        <asp:Label runat="server" ID="lblAl2" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                        <asp:Label runat="server" ID="lblAl3" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                        <asp:Label runat="server" ID="lblAl4" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                        <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;
                                    border-top: 1px solid black;" class="print">
                                    <tr>
                                        <td align="left">
                                            Invoice No: &nbsp;
                                            <asp:Label runat="server" ID="lblSB_No" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right">
                                            Invoice Date:&nbsp;<asp:Label runat="server" ID="lblBillDate" Text='<%#Eval("dt") %>'
                                                Font-Bold="true"></asp:Label>
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
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" ID="Label7" Text='<%#Eval("Party_PAN") %>'></asp:Label>
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
                                        <td style="width: 100%; vertical-align: top; height: 95px;">
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
                                            <b>Bank Commission:</b><%--<b>Rate Diff:</b><asp:Label runat="server" ID="lblBankCommRate" Text='<%#Eval("RateDiff").ToString()=="0.00"?"":Eval("RateDiff","{0}") %>'></asp:Label>/Qntl:--%>
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
                                <table width="100%" align="center" cellspacing="0" class="print9pt" style="table-layout: fixed;
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
                                            <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" /><br />
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
                                    <tr>
                                        <td style="width: 100%; height: 30px;">
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
