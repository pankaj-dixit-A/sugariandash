<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptUtrPrint.aspx.cs" Inherits="Report_rptUtrPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('s.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><link rel="Stylesheet" href="../print.css" media="print" type="text/css" /><head>');
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
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="Email" OnClick="btnSendEmail_Click"
            Width="79px" />&nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px" Height="23px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel runat="server" ID="pnlMain" CssClass="print">
            <table width="1000px" style="table-layout: fixed; height: 125px;" class="print9pt"
                align="center">
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
            <table width="1000px" align="center" class="print">
                <tr>
                    <td style="width: 100%;">
                        <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound">
                            <ItemTemplate>
                                <table width="100%" align="center" cellspacing="5" class="print">
                                    <tr>
                                        <td style="width: 50%; border-top: 2px solid black; border-bottom: 2px solid black;
                                            text-align: left;">
                                            Ref.No.:<asp:Label runat="server" ID="Label1" Text='<%#Eval("#") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 50%; border-bottom: 2px solid black; border-top: 2px solid black;
                                            text-align: right;">
                                            Date:
                                            <asp:Label runat="server" ID="lblDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Label runat="server" ID="lblsub" Text="Subject: Payment Details" Font-Underline="true"
                                                Font-Size="Larger" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="Label11" Text="To, " Font-Size="Larger" />&nbsp;<asp:Label
                                                runat="server" ID="lblMillName" Font-Bold="true" Text='<%#Eval("mill") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="Label10" Text="Address:" Font-Size="Larger" />&nbsp;<asp:Label
                                                runat="server" ID="lblMillAddress" Text='<%#Eval("milladdress") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="Label9" Text="City:" Font-Size="Larger" />&nbsp;<asp:Label
                                                runat="server" ID="lblMillCity" Text='<%#Eval("millcity") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="Label8" Text="Pincode:" Font-Size="Larger" />
                                            &nbsp;<asp:Label runat="server" ID="lblMillPin" Text='<%#Eval("millpin") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="Label6" Text="state:" Font-Size="Larger" />
                                            &nbsp;<asp:Label runat="server" ID="lblMillState" Text='<%#Eval("millstate") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 40px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" colspan="3">
                                            <asp:Label runat="server" ID="Label4" Text="Respected Sir," Font-Size="Larger"></asp:Label>
                                        </td>S
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" colspan="3">
                                            <asp:Label runat="server" ID="lblan" Text="Here with we deposited Rs.: " Font-Size="Larger"></asp:Label>
                                            <asp:Label runat="server" ID="Label7" Text='<%#Eval("amt") %>' Font-Size="Larger"
                                                Font-Bold="true"></asp:Label>
                                            (<asp:Label runat="server" ID="lblInwordsamount" Font-Bold="true" Font-Size="Larger"></asp:Label>)
                                            <asp:Label runat="server" ID="Label2" Text="in your Account By Ref.No/Utr No.: "
                                                Font-Size="Larger"></asp:Label>
                                            <asp:Label runat="server" ID="lblUtrNumber" Text='<%#Eval("utrno") %>' Font-Bold="true"
                                                Font-Size="Larger"></asp:Label>&nbsp;
                                            <asp:Label runat="server" ID="Label3" Text="Please Credit the same to our account and acknowledge the reciept."
                                                Font-Size="Larger"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: left; border-top: 1px solid black;">
                                            <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                            <br />
                                            <asp:Label runat="server" ID="Label5" Text="Thanking You," Font-Size="Larger"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: left;">
                                            <asp:Label runat="server" ID="lblThankCmpName" Font-Bold="true" Text='<%#Eval("Company") %>'
                                                Font-Size="Larger"></asp:Label>
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
