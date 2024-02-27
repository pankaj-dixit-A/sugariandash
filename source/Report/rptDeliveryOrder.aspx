<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDeliveryOrder.aspx.cs"
    EnableEventValidation="false" Inherits="Report_rptDeliveryOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
        <asp:Button ID="btnMail" runat="server" Text="Mail" Width="80px" OnClientClick="return CheckEmail();"
            OnClick="btnMail_Click" />
        &nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
        <%-- <asp:Button runat="server" ID="btn" Text="export" OnClientClick="print_invoice();" />--%>
    </div>
    <div align="center" id="export" runat="server">
        <asp:Panel ID="pnl" runat="server" Width="70%" align="center" Font-Names="Calibri"
            ForeColor="Black" Font-Size="Medium" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="0px">
            <asp:DataList ID="DataList1" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound"
                align="center" CssClass="printhalf">
                <ItemTemplate>
                    <table id="tbHead" width="100%" cellspacing="3" class="printhalf" style="table-layout: fixed;"
                        align="center" runat="server">
                        <tr>
                            <td colspan="3">
                                <table width="100%" style="table-layout: fixed; height: 90px;">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center">
                                            <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/Logo.jpg" Width="100%"
                                                Height="25%" />
                                        </td>
                                        <td style="width: 80%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed;" class="print7pt">
                                                <tr>
                                                    <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
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
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="border-top: solid 1px black; height: 18px; width: 32.5%"
                                class="print9pt">
                                &nbsp;&nbsp; D.O. No.:
                                <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                            </td>
                            <td align="center" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="print9pt">
                                <asp:Label ID="lblReportName" runat="server" Text="Delivery Order" CssClass="lblName"
                                    Font-Bold="true" Style="text-align: center; text-decoration: underline;"></asp:Label>
                            </td>
                            <td align="right" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="print9pt">
                                Date:
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("doc_date1") %>'></asp:Label>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
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
                                                    <td style="width: 45%; vertical-align: top;" align="center" rowspan="5">
                                                        <table width="100%" align="left" style="vertical-align: top;" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    Getpass & Bill
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 25%; border-right: 1px solid black;" align="left" rowspan="5">
                                                        <table width="100%" align="left" style="vertical-align: top;" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLic" runat="server" Font-Bold="false" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="false" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="false" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label5" runat="server" Font-Bold="false" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="false" Text='<%#Eval("PAN_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 30%; vertical-align: top;" align="left" rowspan="5">
                                                        <table width="100%" align="left" class="printhalf">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label runat="server" ID="lblSaleNote" Text='<%#Eval("SaleNoteHead") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label runat="server" ID="lblSellNoteNo" Font-Bold="true" Text='<%#Eval("Sell_Note_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Grade:
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;
                                                                    <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Quintal:
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;
                                                                    <asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Bags:
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label ID="Label6" runat="server" Font-Bold="true" Text='<%#Eval("bags") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Truck No:
                                                                </td>
                                                                <td align="left" class="small">
                                                                    &nbsp;<asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>
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
                            <td colspan="3" align="left" style="border-bottom: 1px solid Black; vertical-align: top;">
                                <table width="100%" style="table-layout: fixed; vertical-align: top; height: 35px;"
                                    class="print9pt">
                                    <tr>
                                        <td>
                                            <tr>
                                                <td align="left" style="width: 25%;">
                                                    With Excise rate:
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:Label ID="lblWithExciseRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
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
                                                    Excise:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblExciseRate" runat="server" Text='<%#Eval("excise_rate") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                </td>
                                                <td align="right">
                                                    Less Amount:
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </td>
                                        <td>
                                            <tr>
                                                <td align="left">
                                                    Without Excise:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblWithoutExcise" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td align="center">
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
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text='<%#Eval("narration3") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="Label10" runat="server" Font-Bold="true" Text='<%#Eval("TenderDO") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
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
            BorderWidth="0px">
            <asp:DataList ID="DataList2" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table id="tbHead" width="100%" cellspacing="3" class="printhalf" style="table-layout: fixed;"
                        align="center" runat="server">
                        <tr>
                            <td colspan="3">
                                <table width="100%" style="table-layout: fixed; height: 90px;">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center" class="print7pt">
                                        </td>
                                        <td style="width: 80%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed;" class="noprint7pt">
                                                <tr>
                                                    <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprint7pt">
                                                        <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true"></asp:Label>
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
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="border-top: solid 1px black; height: 18px; width: 32.5%"
                                class="print9pt">
                                &nbsp;&nbsp; D.O. No.:
                                <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                            </td>
                            <td align="center" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="print9pt">
                                <asp:Label ID="lblReportName" runat="server" Text="Delivery Order" CssClass="lblName"
                                    Font-Bold="true" Style="text-align: center; text-decoration: underline;"></asp:Label>
                            </td>
                            <td align="right" style="width: 32.5%; height: 18px; border-top: solid 1px black;"
                                class="print9pt">
                                Date:
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("doc_date1") %>'></asp:Label>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
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
                                                    <td style="width: 45%; vertical-align: top;" align="center" rowspan="5">
                                                        <table width="100%" align="left" style="vertical-align: top;" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    Getpass & Bill
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 25%; border-right: 1px solid black;" align="left" rowspan="5">
                                                        <table width="100%" align="left" style="vertical-align: top;" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblLic" runat="server" Font-Bold="false" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="false" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="false" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label5" runat="server" Font-Bold="false" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label7" runat="server" Font-Bold="false" Text='<%#Eval("PAN_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 30%; vertical-align: top;" align="left" rowspan="5">
                                                        <table width="100%" align="left" class="printhalf">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label runat="server" ID="lblSaleNote" Text='<%#Eval("SaleNoteHead") %>'></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label runat="server" ID="lblSellNoteNo" Font-Bold="true" Text='<%#Eval("Sell_Note_No") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Grade:
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;
                                                                    <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Quintal:
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;
                                                                    <asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Bags:
                                                                </td>
                                                                <td align="left">
                                                                    &nbsp;<asp:Label ID="Label6" runat="server" Font-Bold="true" Text='<%#Eval("bags") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    Truck No:
                                                                </td>
                                                                <td align="left" class="small">
                                                                    &nbsp;<asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>
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
                            <td colspan="3" align="left" style="border-bottom: 1px solid Black; vertical-align: top;">
                                <table width="100%" style="table-layout: fixed; vertical-align: top; height: 35px;"
                                    class="print9pt">
                                    <tr>
                                        <td>
                                            <tr>
                                                <td align="left" style="width: 25%;">
                                                    With Excise rate:
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:Label ID="lblWithExciseRate" runat="server" Text='<%#Eval("mill_rate") %>'></asp:Label>
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
                                                    Excise:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblExciseRate" runat="server" Text='<%#Eval("excise_rate") %>'></asp:Label>
                                                </td>
                                                <td align="center">
                                                </td>
                                                <td align="right">
                                                    Less Amount:
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </td>
                                        <td>
                                            <tr>
                                                <td align="left">
                                                    Without Excise:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblWithoutExcise" runat="server" Text=""></asp:Label>
                                                </td>
                                                <td align="center">
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
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text='<%#Eval("narration3") %>'></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label
                                    ID="Label10" runat="server" Font-Bold="true" Text='<%#Eval("TenderDO") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
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
