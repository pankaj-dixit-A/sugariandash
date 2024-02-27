<%@ Page Language="C#" Title="DO For GST" AutoEventWireup="true" CodeFile="rptDeliveryOrderForGST.aspx.cs"
    Inherits="Report_rptDeliveryOrderForGST" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <script src="../JS/emailValidation.js" type="text/javascript"></script>
    <link href="../print.css" type="text/css" media="print" rel="Stylesheet" />
    <script type="text/javascript">
        function print_invoice() {
            var printContents = document.getElementById("pnl").innerHTML;
            var originalContents = document.body.innerHTML;
            document.body.innerHTML = printContents;
            window.print();
            document.body.innerHTML = originalContents;
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnl.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" type="text/css" media="print" rel="Stylesheet" />');
            printWindow.document.write('</head><body class="printhalf">');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }

        function PrintPanel2() {
            var panel = document.getElementById("<%=pnl2.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" type="text/css" media="print" rel="Stylesheet" />');
            printWindow.document.write('</head><body class="printhalf">');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }
    </script>
    <title>DO Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="left">
        <%-- <input type="button" onclick="PrintPanel();" id="input" />--%>
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="return PrintPanel();"
            Width="80px" />&nbsp; &nbsp;<asp:Button runat="server" ID="btnPrePrinted" Text="Pre-Printed"
                OnClientClick="return PrintPanel2();" />
        &nbsp; &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;
        <asp:Button ID="btnMail" runat="server" Text="HTML Mail" Width="80px" OnClientClick="return CheckEmail();"
            OnClick="btnMail_Click" />
        &nbsp;
        <asp:Button ID="btnpdfmail" runat="server" Text="PDF Mail" Width="80px" OnClientClick="return CheckEmail();"
            OnClick="btnPDfDownload_Click" />
        <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
        <%-- <asp:Button runat="server" ID="btn" Text="export" OnClientClick="print_invoice();" />--%>
    </div>
    <div align="center" id="export" runat="server" style="width: 80%;">
        <asp:Panel ID="pnl" runat="server" Width="90%" align="center" Font-Names="Calibri"
            ForeColor="Black" Font-Size="Medium" BorderColor="Black" BorderStyle="Solid"
            CssClass="printhalf" BorderWidth="0px">
            <asp:DataList ID="DataList1" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound"
                align="center" CssClass="printhalf">
                <ItemTemplate>
                    <table id="tbHead" width="100%" cellspacing="3" style="table-layout: fixed;" align="center"
                        runat="server" class="print9pt">
                        <tr>
                            <td colspan="3" class="printhalf">
                                <table width="100%" style="table-layout: fixed; height: 90px;" class="printhalf">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center" class="printhalf">
                                            <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/DashLogo.jpg" Width="100%"
                                                Height="90%" />
                                        </td>
                                        <td style="width: 80%; vertical-align: top;" align="left" class="printhalf">
                                            <table width="100%" style="table-layout: fixed;" class="print7pt">
                                                <tr>
                                                    <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="printhalf">
                                                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="Large"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana; font-size: medium;" class="printhalf">
                                                        <asp:Label runat="server" ID="lblAl1" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana; font-size: medium;" class="printhalf">
                                                        <asp:Label runat="server" ID="lblAl2" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana; font-size: medium;" class="printhalf">
                                                        <asp:Label runat="server" ID="lblAl3" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana; font-size: medium;" class="printhalf">
                                                        <asp:Label runat="server" ID="lblAl4" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana; font-size: medium;" class="printhalf">
                                                        <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="border-top: solid 1px black; height: 18px; width: 32.5%"
                                class="printhalf">
                                D.O. No.:
                                <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                            </td>
                            <td align="center" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="printhalf">
                                <asp:Label ID="lblReportName" runat="server" Text="Delivery Order" CssClass="lblName"
                                    Font-Bold="true" Style="text-align: center; text-decoration: underline;"></asp:Label>
                            </td>
                            <td align="right" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="printhalf">
                                Date:
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("doc_date1") %>'></asp:Label>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" class="printhalf">
                                <table width="100%" cellpadding="0" cellspacing="2" class="printhalf" style="table-layout: fixed;">
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black; border-top: solid 1px black;
                                            height: 20px;" align="left">
                                            Mill Name:
                                            <asp:Label ID="lblMillName" runat="server" Text='<%#Eval("millName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black; vertical-align: top;">
                                            <table width="100%" align="left" cellspacing="2" class="printhalf">
                                                <tr>
                                                    <td style="width: 50%; vertical-align: top;" align="center" rowspan="5" class="printhalf">
                                                        <table width="100%" align="left" style="vertical-align: top;" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    Buyer,
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="lblGetPassName" runat="server" Font-Bold="true" Text='<%#Eval("GetPassName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="lblgetpassAddr" runat="server" Text='<%#Eval("getpassAddress") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="lblGetpassCity" runat="server" Font-Bold="false" Text='<%#Eval("getpasscityCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    State Code:<asp:Label ID="Label12" runat="server" Font-Bold="false" Text='<%#Eval("GetpassGstStateCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <table style="width: 100%;" class="printhalf">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label15" runat="server" Text='<%#Eval("Gst_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label13" runat="server" Font-Bold="false" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label2" runat="server" Font-Bold="false" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblLic" runat="server" Font-Bold="false" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label7" runat="server" Font-Bold="false" Text='<%#Eval("PAN_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label26" runat="server" Font-Bold="true" Text='<%#Eval("FSSAI") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label3" runat="server" Font-Bold="false" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 50%; vertical-align: top; border-left: 1px solid black;" align="left"
                                                        rowspan="5" class="printhalf">
                                                        <table width="100%" align="left" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    Shipped To / Consigned To,
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="Label16" runat="server" Font-Bold="true" Text='<%#Eval("SalebilltoName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="Label17" runat="server" Text='<%#Eval("SalebilltoAddress") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="Label18" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoCityState") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    State Code:<asp:Label ID="Label19" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoGstStateCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <table style="width: 100%;" class="printhalf">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label20" runat="server" Text='<%#Eval("SalebilltoGST") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label23" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoPAN_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label21" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoECC_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label22" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoLocal_Lic_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label6" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoTin_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label29" runat="server" Font-Bold="false" Text='<%#Eval("Salebillto_FSSAI") %>'></asp:Label>
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
                                </table>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid Black; vertical-align: top;"
                                class="printhalf">
                                <table width="100%" style="table-layout: fixed; vertical-align: top; height: 35px;"
                                    class="printhalf">
                                    <tr>
                                        <td>
                                            <tr>
                                                <td>
                                                    Purchase Rate:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWithExciseRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:Label runat="server" ID="lblSaleNote" Text='<%#Eval("SaleNoteHead") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:Label runat="server" ID="lblSellNoteNo" Font-Bold="true" Text='<%#Eval("Sell_Note_No") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    QUINTAL:&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                                </td>
                                                <td align="right" style="width: 25%;">
                                                    Total Amount:
                                                </td>
                                                <td align="center" style="width: 25%;">
                                                    <asp:Label ID="Label8" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </td>
                                        <td>
                                            <tr>
                                                <td align="left">
                                                    Add GST:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblExciseRate" runat="server" Text='<%#Eval("Gst_Rate") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    Grade:<asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    Bags:&nbsp;&nbsp;<asp:Label ID="Label5" runat="server" Font-Bold="true" Text='<%#Eval("bags") %>'></asp:Label>
                                                </td>
                                                <td align="right">
                                                    Less Amount:
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </td>
                                        <td class="printhalf">
                                            <tr>
                                                <td align="left">
                                                    Mill Rate:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label14" runat="server" Text='<%#Eval("Net_Rate") %>'></asp:Label>
                                                    <asp:Label ID="lblWithoutExcise" runat="server" Text="" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    HSN:
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label24" runat="server" Font-Bold="true" Text='<%#Eval("HSN") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    Truck No: &nbsp;<asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td align="right">
                                                    Final Amount:
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label4" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="3" align="left" class="printhalf" style="border-bottom: 1px solid black;">
                                <table width="100%" cellpadding="0" cellspacing="2" class="printhalf" style="table-layout: fixed;
                                    padding-left: 0px; padding-right: 0px;">
                                    <tr>
                                        <td>
                                            <table align="left" style="width: 100%;">
                                                <tr>
                                                    <td style="font-size: small;">
                                                        Purchase Rate:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="lblWithExciseRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px;">
                                                        <asp:Label runat="server" ID="lblSaleNote" Text='<%#Eval("SaleNoteHead") %>'></asp:Label>
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label runat="server" ID="lblSellNoteNo" Font-Bold="true" Text='<%#Eval("Sell_Note_No") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 215px; font-size: small;">
                                                        QUINTAL:
                                                        <asp:Label ID="Label9" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Total Amount:
                                                    </td>
                                                    <td align="right" style="font-size: medium;">
                                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="font-size: small;">
                                                        Add GST:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="lblExciseRate" runat="server" Text='<%#Eval("Gst_Rate") %>'></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px;">
                                                        Grade:
                                                        <asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 215px; font-size: small;">
                                                        Bags:
                                                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text='<%#Eval("bags") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Less Amount:
                                                    </td>
                                                    <td style="font-size: small;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="font-size: small;">
                                                        Mill Rate:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="Label14" runat="server" Text='<%#Eval("Net_Rate") %>'></asp:Label>
                                                        <asp:Label ID="lblWithoutExcise" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px;">
                                                        HSN:
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="Label24" runat="server" Font-Bold="true" Text='<%#Eval("HSN") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 215px; font-size: small;">
                                                        Truck No: &nbsp;
                                                        <asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>&nbsp;&nbsp;
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Final Amount:
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="font-size: small;">
                                                        TCS Rate:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="lblTCSRate" runat="server" Text='<%#Eval("TCS_Rate") %>'></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px;">
                                                        TCS Amount:
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="lblTCSAmt" runat="server" Font-Bold="true" Text='<%#Eval("TCSAmt") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 215px; font-size: small;">
                                                        TDS%:
                                                        <asp:Label ID="lblTDSRate" runat="server" Font-Bold="true" Text='<%#Eval("PSTDS_Rate") %>'></asp:Label>&nbsp;
                                                        TDSAmt:<asp:Label ID="lblTDSAmt" runat="server" Font-Bold="true" Text='<%#Eval("TDSAmt") %>'></asp:Label>
                                                    </td>
                                                   
                                                    <td align="right" style="font-size: small;">
                                                        Net Amount With TCS:
                                                    </td>
                                                    <td align="right" style="font-size: medium;">
                                                        <asp:Label ID="Label25" runat="server" Text='<%#Eval("TCS_NetAmt") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid black; vertical-align: middle;">
                                In Words:&nbsp;<asp:Label runat="server" ID="lblInwords" Text='<%#Eval("InWords") %>'
                                    Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="lblNarration" runat="server" Font-Bold="true" Text='<%#Eval("narration1") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="Label11" runat="server" Font-Bold="true" Text='<%#Eval("narration2") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid black;">
                                <asp:Label ID="lblTenderNar" runat="server" Font-Bold="true" Text='<%#Eval("narration3") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="lblTenderDate" runat="server" Font-Bold="true" Text='<%#Eval("TenderDO") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid black;">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text='<%#Eval("narration3") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="Label10" runat="server" Font-Bold="true" Text='<%#Eval("TenderDO") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Image runat="server" ID="imgSign" Height="80px" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Label ID="lblDOfrom" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Label ID="lblCompanyBottom" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    <div style="display: none;">
        <asp:Panel ID="pnl2" runat="server" Width="70%" align="center" Font-Names="Calibri"
            ForeColor="Black" Font-Size="Medium" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="0px" CssClass="printhalf">
            <asp:DataList ID="DataList2" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table id="tbHead" width="100%" cellspacing="3" class="printhalf" style="table-layout: fixed;"
                        align="center" runat="server">
                        <tr>
                            <td colspan="3">
                                <table width="100%" style="table-layout: fixed; height: 90px;" class="noprinthalf">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center" class="noprinthalf">
                                        </td>
                                        <td style="width: 80%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed;" class="noprinthalf">
                                                <tr>
                                                    <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprinthalf">
                                                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprintprinthalf">
                                                        <asp:Label runat="server" ID="lblAl1" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprintprinthalf">
                                                        <asp:Label runat="server" ID="lblAl2" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprintprinthalf">
                                                        <asp:Label runat="server" ID="lblAl3" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprintprinthalf">
                                                        <asp:Label runat="server" ID="lblAl4" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width: 100%; font-family: Verdana;" class="noprintprinthalf">
                                                        <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue"> </asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="border-top: solid 1px black; height: 18px; width: 32.5%"
                                class="printhalf">
                                D.O. No.:
                                <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                            </td>
                            <td align="center" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="printhalf">
                                <asp:Label ID="lblReportName" runat="server" Text="Delivery Order" CssClass="lblName"
                                    Font-Bold="true" Style="text-align: center; text-decoration: underline;"></asp:Label>
                            </td>
                            <td align="right" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="printhalf">
                                Date:
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("doc_date1") %>'></asp:Label>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" class="printhalf">
                                <table width="100%" cellpadding="0" cellspacing="2" class="printhalf" style="table-layout: fixed;">
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black; border-top: solid 1px black;
                                            height: 20px;" align="left">
                                            Mill Name:
                                            <asp:Label ID="lblMillName" runat="server" Text='<%#Eval("millName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black; vertical-align: top;">
                                            <table width="100%" align="left" cellspacing="2" class="printhalf">
                                                <tr>
                                                    <td style="width: 50%; vertical-align: top;" align="center" rowspan="5" class="printhalf">
                                                        <table width="100%" align="left" style="vertical-align: top;" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    Buyer,
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="lblGetPassName" runat="server" Font-Bold="true" Text='<%#Eval("GetPassName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="lblgetpassAddr" runat="server" Text='<%#Eval("getpassAddress") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="lblGetpassCity" runat="server" Font-Bold="false" Text='<%#Eval("getpasscityCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    State Code:<asp:Label ID="Label12" runat="server" Font-Bold="false" Text='<%#Eval("GetpassGstStateCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <table style="width: 100%;" class="printhalf">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label15" runat="server" Text='<%#Eval("Gst_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label13" runat="server" Font-Bold="false" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label2" runat="server" Font-Bold="false" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblLic" runat="server" Font-Bold="false" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label7" runat="server" Font-Bold="false" Text='<%#Eval("PAN_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label27" runat="server" Font-Bold="true" Text='<%#Eval("FSSAI") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label3" runat="server" Font-Bold="false" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 50%; vertical-align: top; border-left: 1px solid black;" align="left"
                                                        rowspan="5" class="printhalf">
                                                        <table width="100%" align="left" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    Shipped To / Consigned To,
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="Label16" runat="server" Font-Bold="true" Text='<%#Eval("SalebilltoName") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="Label17" runat="server" Text='<%#Eval("SalebilltoAddress") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <asp:Label ID="Label18" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoCityState") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    State Code:<asp:Label ID="Label19" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoGstStateCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="printhalf">
                                                                    <table style="width: 100%;" class="printhalf">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label20" runat="server" Text='<%#Eval("SalebilltoGST") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label23" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoPAN_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label21" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoECC_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label22" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoLocal_Lic_No") %>'></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label6" runat="server" Font-Bold="false" Text='<%#Eval("SalebilltoTin_No") %>'></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label28" runat="server" Font-Bold="false" Text='<%#Eval("Salebillto_FSSAI") %>'></asp:Label>
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
                                </table>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid Black; vertical-align: top;"
                                class="printhalf">
                                <table width="100%" style="table-layout: fixed; vertical-align: top; height: 35px;"
                                    class="printhalf">
                                    <tr>
                                        <td>
                                            <tr>
                                                <td>
                                                    Purchase Rate:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblWithExciseRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:Label runat="server" ID="lblSaleNote" Text='<%#Eval("SaleNoteHead") %>'></asp:Label>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:Label runat="server" ID="lblSellNoteNo" Font-Bold="true" Text='<%#Eval("Sell_Note_No") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    QUINTAL:&nbsp;&nbsp;<asp:Label ID="Label9" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                                </td>
                                                <td align="right" style="width: 25%;">
                                                    Total Amount:
                                                </td>
                                                <td align="center" style="width: 25%;">
                                                    <asp:Label ID="Label8" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </td>
                                        <td>
                                            <tr>
                                                <td align="left">
                                                    Add GST:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblExciseRate" runat="server" Text='<%#Eval("Gst_Rate") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    Grade:<asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    Bags:&nbsp;&nbsp;<asp:Label ID="Label5" runat="server" Font-Bold="true" Text='<%#Eval("bags") %>'></asp:Label>
                                                </td>
                                                <td align="right">
                                                    Less Amount:
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </td>
                                        <td class="printhalf">
                                            <tr>
                                                <td align="left">
                                                    Mill Rate:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label14" runat="server" Text='<%#Eval("Net_Rate") %>'></asp:Label>
                                                    <asp:Label ID="lblWithoutExcise" runat="server" Text="" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    HSN:
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label24" runat="server" Font-Bold="true" Text='<%#Eval("HSN") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                    Truck No: &nbsp;<asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                                <td align="right">
                                                    Final Amount:
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="Label4" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="3" align="left" class="printhalf" style="border-bottom: 1px solid black;">
                                <table width="100%" cellpadding="0" cellspacing="2" class="printhalf" style="table-layout: fixed;
                                    padding-left: 0px; padding-right: 0px;">
                                    <tr>
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="font-size: small;">
                                                        Purchase Rate:
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="lblWithExciseRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label runat="server" ID="lblSaleNote" Text='<%#Eval("SaleNoteHead") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label runat="server" ID="lblSellNoteNo" Font-Bold="true" Text='<%#Eval("Sell_Note_No") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2" style="font-size: small;">
                                                        QUINTAL:
                                                        <asp:Label ID="Label9" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Total Amount:
                                                    </td>
                                                    <td align="center" style="font-size: medium;">
                                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="font-size: small;">
                                                        Add GST:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="lblExciseRate" runat="server" Text='<%#Eval("Gst_Rate") %>'></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px;">
                                                        Grade:
                                                        <asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" width="5%;" style="font-size: small;">
                                                        Bags:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text='<%#Eval("bags") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Less Amount:
                                                    </td>
                                                    <td style="font-size: small;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="font-size: small;">
                                                        Mill Rate:
                                                    </td>
                                                    <td align="left" style="font-size: small;">
                                                        <asp:Label ID="Label14" runat="server" Text='<%#Eval("Net_Rate") %>'></asp:Label>
                                                        <asp:Label ID="lblWithoutExcise" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px;">
                                                        HSN:
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="Label24" runat="server" Font-Bold="true" Text='<%#Eval("HSN") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2" style="width: 215px; font-size: small;">
                                                        Truck No: &nbsp;
                                                        <asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>&nbsp;&nbsp;
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Final Amount:
                                                    </td>
                                                    <td align="center" style="font-size: small;">
                                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="font-size: small;">
                                                        TCS Rate:
                                                    </td>
                                                    <td style="font-size: small;">
                                                        <asp:Label ID="lblTCSRate" runat="server" Text='<%#Eval("TCS_Rate") %>'></asp:Label>
                                                    </td>
                                                    <td style="font-size: small; padding-left: 30px; width: 20%">
                                                        TCS Amount:
                                                    </td>
                                                    <td style="font-size: small; width: 20%">
                                                        <asp:Label ID="lblTCSAmt" runat="server" Font-Bold="true" Text='<%#Eval("TCSAmt") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" style="width: 215px; font-size: small;">
                                                        TDS%:
                                                        <asp:Label ID="lblTDSRate" runat="server" Font-Bold="true" Text='<%#Eval("PSTDS_Rate") %>'></asp:Label>&nbsp;
                                                        TDSAmt:<asp:Label ID="lblTDSAmt" runat="server" Font-Bold="true" Text='<%#Eval("TDSAmt") %>'></asp:Label>
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                    </td>
                                                    <td align="right" style="font-size: small;">
                                                        Net Amount With TCS:
                                                    </td>
                                                    <td align="center" style="font-size: medium;">
                                                        <asp:Label ID="Label25" runat="server" Text='<%#Eval("TCS_NetAmt") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid black; vertical-align: middle;">
                                In Words:&nbsp;<asp:Label runat="server" ID="lblInwords" Text='<%#Eval("InWords") %>'
                                    Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="lblNarration" runat="server" Font-Bold="true" Text='<%#Eval("narration1") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="Label11" runat="server" Font-Bold="true" Text='<%#Eval("narration2") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" style="border-bottom: 1px solid black;">
                                <asp:Label ID="lblTenderNar" runat="server" Font-Bold="true" Text='<%#Eval("narration3") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="lblTenderDate" runat="server" Font-Bold="true" Text='<%#Eval("TenderDO") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Image runat="server" ID="imgSign" Height="80px" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Label ID="lblDOfrom" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Label ID="lblCompanyBottom" runat="server" Text=""></asp:Label>
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
