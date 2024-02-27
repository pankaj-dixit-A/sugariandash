<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptWayBill.aspx.cs" Inherits="Report_rptWayBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="../print.css" type="text/css" media="print" />
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            window.print();
        }
    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("#btnPrint").live("click", function () {
            var divContents = $("#div1").html();
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            Width="79px" />
        &nbsp;<asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div id="div1">
        <asp:Panel runat="server" ID="pnlMain" CssClass="print">
            <table width="70%" style="table-layout: fixed; height: 110px;" align="center" class="print9pt">
                <tr>
                    <td style="width: 20%; vertical-align: top;" align="center">
                        <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/DashLogo.jpg" Width="100%"
                            Height="90%" />
                    </td>
                    <td style="width: 80%; vertical-align: top;" align="left">
                        <table width="100%" style="table-layout: fixed;" class="print9pt">
                            <tr>
                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                    <asp:Label ID="Label10" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
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
            <table width="70%" align="center" cellpadding="1" class="print">
                <tr>
                    <td align="center">
                        <p style="text-decoration: underline;">
                            Detail to Be Provided for Advance E-Waybill / FORM 49 / E-Token
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%">
                            <ItemTemplate>
                                <table width="100%" style="table-layout: fixed; border-top: 1px solid black;" align="center">
                                    <tr>
                                        <td style="width: 100%; border-bottom: 1px solid black;" align="center" colspan="2">
                                            <asp:Label runat="server" ID="lblGetpassName" Text='<%#Eval("GetpassName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%; text-align: left; border-bottom: 1px solid black;">
                                            Our Ref No.:<asp:Label runat="server" ID="lblDocNo" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 50%; text-align: right; border-bottom: 1px solid black;">
                                            Date:<asp:Label runat="server" ID="lblDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">
                                            <table width="100%" align="center" cellpadding="2" cellspacing="4">
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        FULL NAME OF VENDOR
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="lblMillName" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        ADDRESS OF VENDOR
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label1" Text='<%#Eval("MillAddress") %>'></asp:Label>&nbsp;<asp:Label
                                                            runat="server" ID="lblMillState" Text='<%#Eval("MillState") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        VENDOR TIN NO.
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label2" Text='<%#Eval("MillTIN") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        INVOICE NO
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label3" Text='<%#Eval("Invoice_No") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        INVOICE DATE
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label4" Text='<%#Eval("dt") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        INVOICE VALUE
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label5" Text='<%#Eval("amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        VEHICLE NO
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label6" Text='<%#Eval("lorry") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 35%;" align="left">
                                                        PLACE FROM WHICH CONSIGNED
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label7" Text='<%#Eval("MillCity") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        PLACE TO WHICH CONSIGNED
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label8" Text='<%#Eval("GetpassCity") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        TRANSPORTER NAME
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label9" Text='<%#Eval("TransportName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        DRIVER MOBILE
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label13" Text='<%#Eval("driver_no") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        COMMODITY
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label10" Text="SUGAR"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        QUANTITY
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label11" Text='<%#Eval("Qntl") %>'></asp:Label>&nbsp;Qntl
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        UNIT OF MEASUREMENT
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label12" Text='<%#Eval("QntlToKg") %>'></asp:Label>&nbsp;Kgs
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 32%;" align="left">
                                                        CHECKPOST
                                                    </td>
                                                    <td style="width: 80%;" align="left">
                                                        <asp:Label runat="server" ID="Label14" Text='<%#Eval("CheckPost") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
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
