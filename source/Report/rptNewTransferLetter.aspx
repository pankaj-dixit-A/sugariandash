<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptNewTransferLetter.aspx.cs"
    Inherits="Report_rptNewTransferLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=dtlist.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body class="print2">');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }

        function PrintPanel2() {
            var panel = document.getElementById("<%=dtlist2.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
            printWindow.document.write('</head><body class="print2">');
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
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp; &nbsp;<asp:Button runat="server" ID="btnPrePrinted" Text="Pre-Printed" OnClientClick="return PrintPanel2();" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            Width="79px" />
        <asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain">
            <table width="70%" align="center" cellpadding="0" cellspacing="4" class="print2">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" cellpadding="0" cellspacing="1" class="printhalf">
                                    <tr>
                                        <td style="width: 100%;" colspan="3">
                                            <table width="100%" style="table-layout: fixed; height: 90px;">
                                                <tr>
                                                    <td style="width: 20%; vertical-align: top;" align="center">
                                                       <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/DashLogo.jpg" Width="100%"
                                                    Height="90%" />
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
                                        <td style="width: 35%; height: 35px; vertical-align: top; border-top: 1px solid black;"
                                            align="left">
                                            Our Ref No:<asp:Label runat="server" ID="Label1" Font-Bold="true" Text='<%#Eval("DoNo") %>'></asp:Label>
                                        </td>
                                        <td style="width: 30%; height: 35px; vertical-align: top; border-top: 1px solid black;"
                                            align="center">
                                        </td>
                                        <td style="width: 35%; height: 35px; vertical-align: top; border-top: 1px solid black;"
                                            align="right">
                                            Date:<asp:Label runat="server" ID="lblDate" Font-Bold="true" Text='<%#Eval("Do_Date") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table width="100%" align="center" cellpadding="0" cellspacing="3" class="print2">
                                                <tr>
                                                    <td style="width: 4%;" align="center">
                                                        To,
                                                    </td>
                                                    <td style="width: 96%; border-bottom: 1px solid black;" align="left">
                                                        <asp:Label runat="server" ID="lblMillName" Font-Bold="true" Text='<%#Eval("MillName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 4%;" align="center">
                                                    </td>
                                                    <td align="left" style="border-bottom: 1px solid black; width: 96%;">
                                                        <asp:Label runat="server" ID="lblCityStatePin" Text='<%#Eval("millcitystate") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 5px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" style="border-bottom: 1px solid black; border-top: 1px solid black;
                                                        border-right: 1px solid black; border-left: 1px solid black; height: 19px; background-color: #FEF0C9;">
                                                        <b>Amount Transfer Letter</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label runat="server" ID="lbl1" Text="Respected Sir,"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2" style="word-wrap: break-word; text-wrap: normal;">
                                                        <asp:Label runat="server" ID="Label3" Text="Enclosed herewith Rs." Font-Size="Larger"></asp:Label>&nbsp;<asp:Label
                                                            runat="server" ID="Label4" Text='<%#Eval("Amount") %>' Font-Bold="true"></asp:Label>&nbsp;
                                                        <asp:Label runat="server" ID="lkkl" Text=" Debit the same to our Account and credit to"
                                                            Font-Size="Larger"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label runat="server" ID="lblPartyName" Text='<%#Eval("Getpass") %>' Font-Bold="true"></asp:Label>
                                                        <asp:Label runat="server" ID="Label5" Text=" &nbsp;&nbsp;for the same"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table width="100%" align="center" cellpadding="0" cellspacing="4" style="border-top: 2px solid black;"
                                                class="print2">
                                                <tr>
                                                    <td style="width: 20%;" align="left">
                                                    </td>
                                                    <td style="width: 70%;" align="left">
                                                    </td>
                                                    <td style="width: 10%;" align="center">
                                                        <b>Amount</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;" align="left">
                                                        Party Name:
                                                    </td>
                                                    <td style="width: 70%;" align="left">
                                                        <asp:Label runat="server" ID="lblPartyNamed" Text='<%#Eval("Getpass") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 10%;" align="center" rowspan="5">
                                                        <asp:Label runat="server" ID="lblAMount" Text='<%#Eval("Amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right" style="height: 50px; vertical-align: bottom;">
                                            <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right" style="vertical-align: bottom;">
                                            Thanking You,
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3" style="width: 90%;">
                                            For M/s.<asp:Label runat="server" ID="lblCmpName" Text='<%#Eval("CmpName") %>'></asp:Label>
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
        <asp:Panel runat="server" ID="Panel1">
            <table width="70%" align="center" cellpadding="0" cellspacing="4" class="print2">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist2" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" cellpadding="0" cellspacing="1" class="print2">
                                    <tr>
                                        <td style="width: 100%;" colspan="3">
                                            <table width="100%" style="table-layout: fixed; height: 90px;">
                                                <tr>
                                                    <td style="width: 20%; vertical-align: top;" align="center">
                                                    </td>
                                                    <td style="width: 80%; vertical-align: top;" align="left">
                                                        <table width="100%" style="table-layout: fixed;" class="noprint7pt">
                                                            <tr>
                                                                <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprint9pt">
                                                                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
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
                                        <td style="width: 35%; height: 35px; vertical-align: top; border-top: 1px solid black;"
                                            align="left">
                                            Our Ref No:<asp:Label runat="server" ID="Label1" Font-Bold="true" Text='<%#Eval("DoNo") %>'></asp:Label>
                                        </td>
                                        <td style="width: 30%; height: 35px; vertical-align: top; border-top: 1px solid black;"
                                            align="center" rowspan="6">
                                        </td>
                                        <td style="width: 35%; height: 35px; vertical-align: top; border-top: 1px solid black;"
                                            align="right">
                                            Date:<asp:Label runat="server" ID="lblDate" Font-Bold="true" Text='<%#Eval("Do_Date") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table width="100%" align="center" cellpadding="0" cellspacing="3" class="print2">
                                                <tr>
                                                    <td style="width: 4%;" align="center">
                                                        To,
                                                    </td>
                                                    <td style="width: 96%; border-bottom: 1px solid black;" align="left">
                                                        <asp:Label runat="server" ID="lblMillName" Font-Bold="true" Text='<%#Eval("MillName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 4%;" align="center">
                                                    </td>
                                                    <td align="left" style="border-bottom: 1px solid black; width: 96%;">
                                                        <asp:Label runat="server" ID="lblCityStatePin" Text='<%#Eval("millcitystate") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 5px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center" style="border-bottom: 1px solid black; border-top: 1px solid black;
                                                        border-right: 1px solid black; border-left: 1px solid black; height: 19px; background-color: #FEF0C9;">
                                                        <b>Amount Transfer Letter</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label runat="server" ID="lbl1" Text="Respected Sir,"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2" style="word-wrap: break-word; text-wrap: normal;">
                                                        <asp:Label runat="server" ID="Label3" Text="Enclosed herewith Rs." Font-Size="Larger"></asp:Label>&nbsp;<asp:Label
                                                            runat="server" ID="Label4" Text='<%#Eval("Amount") %>' Font-Bold="true"></asp:Label>&nbsp;
                                                        <asp:Label runat="server" ID="lkkl" Text=" Debit the same to our Account and credit to"
                                                            Font-Size="Larger"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:Label runat="server" ID="lblPartyName" Text='<%#Eval("Getpass") %>' Font-Bold="true"></asp:Label>
                                                        <asp:Label runat="server" ID="Label5" Text="for the same"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <table width="100%" align="center" cellpadding="0" cellspacing="4" style="border-top: 2px solid black;"
                                                class="print2">
                                                <tr>
                                                    <td style="width: 20%;" align="left">
                                                    </td>
                                                    <td style="width: 70%;" align="left">
                                                    </td>
                                                    <td style="width: 10%;" align="center">
                                                        <b>Amount</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;" align="left">
                                                        Party Name:
                                                    </td>
                                                    <td style="width: 70%;" align="left">
                                                        <asp:Label runat="server" ID="lblPartyNamed" Text='<%#Eval("Getpass") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 10%;" align="center" rowspan="5">
                                                        <asp:Label runat="server" ID="lblAMount" Text='<%#Eval("Amount") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right" style="height: 50px; vertical-align: bottom;">
                                            <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right" style="vertical-align: bottom;">
                                            Thanking You,
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3" style="width: 90%;">
                                            For M/s.<asp:Label runat="server" ID="lblCmpName" Text='<%#Eval("CmpName") %>'></asp:Label>
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
