<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptMotorMemoBlank.aspx.cs"
    Inherits="Report_rptMotorMemoBlank" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Motor Memo</title>
    <link href="../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../print.css" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel1() {
            var panel = document.getElementById("<%=DataList1.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=400,width=800');
            printWindow.document.write('<html><head><link rel="stylesheet" href="../print.css" type="text/css" media="print" />');
            printWindow.document.write('</head><body>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }

        function PrintPanel2() {
            var panel = document.getElementById("<%=DataList2.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=400,width=800');
            printWindow.document.write('<html><head><link rel="stylesheet" href="../print.css" type="text/css" media="print" />');
            printWindow.document.write('</head><body>');
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
    <div align="left" style="width: 80%;" class="noprint">
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="return PrintPanel1();"
            Width="80px" />&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Text="Pre-Printed" OnClientClick="return PrintPanel2();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;
        <asp:Button ID="btnMail" runat="server" Text="Mail" Width="80px" OnClick="btnMail_Click" />
        &nbsp;Email:<asp:TextBox runat="server" ID="txtEmail" Width="244px"></asp:TextBox>
    </div>
    <div align="center">
        <asp:Panel ID="pnl" runat="server" Width="70%" align="center" Font-Names="Calibri"
            ForeColor="Black" Font-Size="Medium" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="0px">
            <asp:DataList ID="DataList1" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table width="100%" border="0px" style="table-layout: fixed;" class="printhalf">
                        <tr>
                            <td style="width: 100%;">
                                <table width="100%" style="table-layout: fixed; height: 90px;">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center" class="print9pt">
                                            <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/DashLogo.jpg" Width="100%"
                                                Height="90%" />
                                        </td>
                                        <td style="width: 80%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed; height: 90px;" class="print7pt">
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
                            <td colspan="2" style="border-bottom: solid 1px black; border-top: solid 1px black;
                                vertical-align: top;">
                                <table width="100%" style="table-layout: fixed;" class="printhalf">
                                    <tr>
                                        <td align="left">
                                            &nbsp;&nbsp;Our Ref No.:<asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Freight Confirmation Letter"></asp:Label>
                                        </td>
                                        <td align="right">
                                            Date:
                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("doc_date1") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="width: 100%; border-bottom: solid 1px black;">
                                <table width="100%" style="table-layout: fixed; height: 140px;" class="printhalf">
                                    <tr>
                                        <td style="width: 60%; vertical-align: top;">
                                            <table width="100%" class="printsmall">
                                                <tr>
                                                    <td align="left">
                                                        Getpass By,
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="printhalf">
                                                        <asp:Label ID="lblGetPassName" Font-Bold="true" runat="server" Text='<%#Eval("GetPassName") %>'></asp:Label>
                                                        <asp:Label ID="lblGetpassCode" Visible="false" runat="server" Text='<%#Eval("GETPASSCODE") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="printhalf">
                                                        <asp:Label ID="lblGetPassAddr" runat="server" Text='<%#Eval("getpassAddress") %>'></asp:Label>
                                                        <asp:Label ID="lblgetpasscityCode" Visible="false" runat="server" Text='<%#Eval("getpasscityCode") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="print9pt">
                                                        City:&nbsp;<asp:Label ID="lblGetpassCity" runat="server" Font-Bold="true"></asp:Label>
                                                        &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblGetpassState" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; font-size: medium;" class="printhalf">
                                                        <table width="100%" align="center" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCSTNo" runat="server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblSLNo" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblTinNo" runat="server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblPan" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="print9pt">
                                                        <asp:Label ID="lblgetpassMobile" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 40%; vertical-align: top; border-left: 1px solid black;">
                                            <table width="100%" style="table-layout: fixed;" class="printhalf">
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Dispatch From:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label runat="server" ID="lblMillShortName" Text='<%#Eval("millShortName") %>'
                                                            Font-Bold="true"></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Quintal:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label ID="lblQuantal" Font-Bold="true" runat="server" Text='<%#Eval("quantal") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Truck No:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label ID="lblTruck" Font-Bold="true" runat="server" Text='<%#Eval("truck_no") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Transport:
                                                    </td>
                                                    <td style="width: 70%;" align="right" class="toosmall">
                                                        <asp:Label ID="lblTransport" Font-Bold="true" runat="server" Text='<%#Eval("TransportName") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Driver Mobile:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text='<%#Eval("driver_no") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100%;">
                                <table width="100%" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="printhalf">
                                    <tr>
                                        <td style="width: 60%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed; vertical-align: top;" class="printhalf">
                                                <tr>
                                                    <td align="left">
                                                        Rate/Quantal:
                                                        <asp:Label ID="lblrateperQtl" runat="server" Text='<%#Eval("FreightPerQtl") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 40%;">
                                            <table width="100%" style="border-left: 1px solid black; table-layout: fixed;" class="printhalf">
                                                <tr>
                                                    <td align="left">
                                                        Total Amount:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotalAmt" runat="server" Text='<%#Eval("Freight_Amount") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Less Amount:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblLessAmt" runat="server" Text='<%#Eval("less") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Final Amount:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFinalAmt" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100%;">
                                <table width="100%" style="table-layout: fixed;" class="printhalf">
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black;" align="left">
                                            In Words:<asp:Label runat="server" ID="lblInWords" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100;" align="left" class="print7pt">
                                            * This is the confirmation letter about the frieght fixed with the transporters.Please
                                            Confirm the R.R./L.R as above and Pay the Same.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100;" align="left" class="print7pt">
                                            * After Dispatch Of Sugar we are not responsible for non-delivery and any damage
                                            or loss to the goods in the transit.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" align="left" class="print7pt">
                                            * I will deliver the consignments in good condition without any damage or shortage.I
                                            am responsible for all the losses in transit.
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <table width="100%" style="table-layout: fixed; height: 60px;" class="print7pt">
                                    <tr>
                                        <td style="width: 60%;" align="right">
                                            Name & Signature of Transport<br />
                                            <asp:Label ID="Label5" Font-Bold="true" runat="server" Text='<%#Eval("TransportName") %>'
                                                CssClass="print7pt"></asp:Label>
                                        </td>
                                        <td style="width: 40%;" align="right">
                                            <asp:Image runat="server" ID="imgSign" Height="30px" Width="150px" /><br />
                                            <asp:Label ID="lblCompanyFooter" Font-Bold="true" runat="server" Text="" CssClass="print7pt"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    <div align="center" style="display: none;">
        <asp:Panel ID="Panel1" runat="server" Width="70%" align="center" Font-Names="Calibri"
            ForeColor="Black" Font-Size="Medium" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="0px">
            <asp:DataList ID="DataList2" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table width="100%" border="0px" style="table-layout: fixed;" class="printhalf">
                        <tr>
                            <td style="width: 100%;">
                                <table width="100%" style="table-layout: fixed; height: 90px;" class="noprint7pt">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;" align="center">
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
                            <td colspan="2" style="border-bottom: solid 1px black; border-top: solid 1px black;
                                vertical-align: top;">
                                <table width="100%" style="table-layout: fixed;" class="printhalf">
                                    <tr>
                                        <td align="left">
                                            &nbsp;&nbsp;Our Ref No.:<asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Freight Confirmation Letter"></asp:Label>
                                        </td>
                                        <td align="right">
                                            Date:
                                            <asp:Label ID="Label2" runat="server" Text='<%#Eval("doc_date1") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left" style="width: 100%; border-bottom: solid 1px black;">
                                <table width="100%" style="table-layout: fixed; height: 140px;" class="printhalf">
                                    <tr>
                                        <td style="width: 60%; vertical-align: top;">
                                            <table width="100%" class="printsmall">
                                                <tr>
                                                    <td align="left">
                                                        Getpass By,
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="printhalf">
                                                        <asp:Label ID="lblGetPassName" Font-Bold="true" runat="server" Text='<%#Eval("GetPassName") %>'></asp:Label>
                                                        <asp:Label ID="lblGetpassCode" Visible="false" runat="server" Text='<%#Eval("GETPASSCODE") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="printhalf">
                                                        <asp:Label ID="lblGetPassAddr" runat="server" Text='<%#Eval("getpassAddress") %>'></asp:Label>
                                                        <asp:Label ID="lblgetpasscityCode" Visible="false" runat="server" Text='<%#Eval("getpasscityCode") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="print9pt">
                                                        City:&nbsp;<asp:Label ID="lblGetpassCity" runat="server" Font-Bold="true"></asp:Label>
                                                        &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblGetpassState" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top; font-size: medium;" class="printhalf">
                                                        <table width="100%" align="center" class="printhalf">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCSTNo" runat="server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblSLNo" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblTinNo" runat="server"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Label ID="lblPan" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="print9pt">
                                                        <asp:Label ID="lblgetpassMobile" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 40%; vertical-align: top; border-left: 1px solid black;">
                                            <table width="100%" style="table-layout: fixed;" class="printhalf">
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Dispatch From:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label runat="server" ID="lblMillShortName" Text='<%#Eval("millShortName") %>'
                                                            Font-Bold="true"></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Quintal:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label ID="lblQuantal" Font-Bold="true" runat="server" Text='<%#Eval("quantal") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Truck No:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label ID="lblTruck" Font-Bold="true" runat="server" Text='<%#Eval("truck_no") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Transport:
                                                    </td>
                                                    <td style="width: 70%;" align="right" class="toosmall">
                                                        <asp:Label ID="lblTransport" Font-Bold="true" runat="server" Text='<%#Eval("TransportName") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 30%;" align="left">
                                                        Driver Mobile:
                                                    </td>
                                                    <td style="width: 70%;" align="right">
                                                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text='<%#Eval("driver_no") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100%;">
                                <table width="100%" style="table-layout: fixed; border-bottom: 1px solid black;"
                                    class="printhalf">
                                    <tr>
                                        <td style="width: 60%; vertical-align: top;" align="left">
                                            <table width="100%" style="table-layout: fixed; vertical-align: top;" class="printhalf">
                                                <tr>
                                                    <td align="left">
                                                        Rate/Quantal:
                                                        <asp:Label ID="lblrateperQtl" runat="server" Text='<%#Eval("FreightPerQtl") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 40%;">
                                            <table width="100%" style="border-left: 1px solid black; table-layout: fixed;" class="printhalf">
                                                <tr>
                                                    <td align="left">
                                                        Total Amount:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotalAmt" runat="server" Text='<%#Eval("Freight_Amount") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Less Amount:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblLessAmt" runat="server" Text='<%#Eval("less") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Final Amount:
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFinalAmt" runat="server" Text='<%#Eval("final_amount") %>'></asp:Label>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 100%;">
                                <table width="100%" style="table-layout: fixed;" class="printhalf">
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black;" align="left">
                                            In Words:<asp:Label runat="server" ID="lblInWords" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100;" align="left" class="print7pt">
                                            * This is the confirmation letter about the frieght fixed with the transporters.Please
                                            Confirm the R.R./L.R as above and Pay the Same.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100;" align="left" class="print7pt">
                                            * After Dispatch Of Sugar we are not responsible for non-delivery and any damage
                                            or loss to the goods in the transit.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" align="left" class="print7pt">
                                            * I will deliver the consignments in good condition without any damage or shortage.I
                                            am responsible for all the losses in transit.
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <table width="100%" style="table-layout: fixed; height: 60px;" class="print7pt">
                                    <tr>
                                        <td style="width: 60%;" align="right">
                                            Name & Signature of Transport<br />
                                            <asp:Label ID="Label5" Font-Bold="true" runat="server" Text='<%#Eval("TransportName") %>'
                                                CssClass="print7pt"></asp:Label>
                                        </td>
                                        <td style="width: 40%;" align="right">
                                            <asp:Image runat="server" ID="imgSign" Height="30px" Width="150px" /><br />
                                            <asp:Label ID="lblCompanyFooter" Font-Bold="true" runat="server" Text="" CssClass="print7pt"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
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
