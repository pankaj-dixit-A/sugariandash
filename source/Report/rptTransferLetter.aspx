<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptTransferLetter.aspx.cs"
    Inherits="Report_rptTransferLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('sugarStock.html', 'st', 'height=400,width=800');
            printWindow.document.write('<html><head><link href="../print.css" rel="stylesheet" type="text/css" media="print" />');
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
    <title></title>
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
    <div>
        <asp:Panel runat="server" ID="pnlMain">
            <table width="75%" align="center" cellpadding="2" class="print">
                <tr>
                    <td align="left" style="width: 32.5%;">
                        <asp:Label runat="server" ID="lblSubjToCity"></asp:Label>
                    </td>
                    <td align="center" style="width: 32.5%;">
                        <h3>
                            <asp:Label runat="server" ID="lblGodName" Font-Bold="true"></asp:Label></h3>
                    </td>
                    <td align="right" style="width: 32.5%;">
                        Ph.No:<asp:Label runat="server" ID="lblCompanyTelepfone"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Sugar LIC/No:<asp:Label runat="server" ID="lblCmpSLN"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label runat="server" ID="lblCompanyName" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right">
                        Mobile:<asp:Label ID="lblCompanyMobile" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        TIN No:
                        <asp:Label runat="server" ID="lblCmpTINNo"></asp:Label><br />
                        CST No:<asp:Label runat="server" ID="lblbCmpTINNo"></asp:Label>
                    </td>
                    <td align="center" style="text-align: left; vertical-align: top;">
                        <asp:Label runat="server" ID="lblCmpAddress"></asp:Label>
                    </td>
                    <td align="right">
                        Email:<asp:Label runat="server" ID="lblCmpEmail"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="center">
                        <asp:Label runat="server" ID="lblCmpCityName"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="75%" align="center" cellspacing="1" cellpadding="0" class="print">
                <tr>
                    <td style="width: 100%">
                        <asp:DataList runat="server" ID="dtl" Width="100%" OnItemDataBound="dtl_ItemDataBound">
                            <ItemTemplate>
                                <table width="100%" style="table-layout: fixed;" cellspacing="3" class="print">
                                    <tr>
                                        <td align="left" style="border-bottom: 1px Solid black; border-top: 1px Solid Black;
                                            width: 32.5%;">
                                            Our Ref No:<asp:Label runat="server" ID="lblDocNo" Font-Bold="true" Text='<%#Eval("DoNo") %>'></asp:Label>
                                        </td>
                                        <td align="center" style="border-bottom: 1px Solid black; border-top: 1px Solid Black;
                                            width: 32.5%;">
                                            <asp:Label runat="server" ID="tl" Text="TRANSFER LETTER" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="right" style="border-bottom: 1px Solid black; border-top: 1px Solid Black;
                                            width: 32.5%;">
                                            Date:<asp:Label runat="server" ID="lblDate" Text='<%#Eval("Do_Date") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 100%;" colspan="3">
                                            Respected Sir,
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 100%;" colspan="3">
                                            <asp:Label runat="server" ID="lblMillName" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="width: 100%; border-bottom: 2px Solid Black;" colspan="3">
                                            <h4>
                                                WE HAVE GIVEN A RTGS IN YOUR BANK ACCOUNT SO PLZ TRANSFER THE AMOUNT TO THESE PARTYEES</h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="border-right: 2px Solid Black;">
                                            <asp:Label runat="server" ID="lblgetPass" Text="Get Pass"></asp:Label>
                                        </td>
                                        <td align="left">
                                            Qntl:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblQntl" Font-Bold="true"
                                                Text='<%#Eval("Qntl") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1">
                                            <asp:Label runat="server" ID="lblGetpassName" Text='<%#Eval("Getpass") %>' Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="border-right: 2px Solid Black;">
                                            CST No:<asp:Label runat="server" ID="lblgetpassCstno" Text='<%#Eval("GetPassCST") %>'
                                                Font-Bold="true"></asp:Label>
                                        </td>
                                        <td align="left">
                                            Grade:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblGrade"
                                                Text='<%#Eval("Grade") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label runat="server" ID="lblGetPassAddress" Text='<%#Eval("GetPassAddress") %>'></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="border-right: 2px Solid Black;">
                                            <asp:Label runat="server" ID="lblGetPassMobile" Text='<%#Eval("GetPassMobile") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            Truck No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblTruckNo" Text='<%#Eval("Truck") %>'></asp:Label>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left" colspan="1" style="border-right: 2px Solid Black;">
                                            TIN No:<asp:Label runat="server" ID="lblGetPassTin" Text='<%#Eval("GetPassTin") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            M.R:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblMillRate" Font-Bold="true"
                                                Text='<%#Eval("MR") %>'></asp:Label>
                                    </tr>
                                    <tr>
                                        <td align="left" style="border-bottom: 2px Solid Black;">
                                            City:<asp:Label runat="server" ID="lblGetpassCityName" Font-Bold="true" Text='<%#Eval("GetPassCity") %>'></asp:Label>
                                        </td>
                                        <td align="left" colspan="1" style="border-right: 2px Solid Black; border-bottom: 2px Solid Black;">
                                            State:<asp:Label runat="server" ID="lblGetpassState" Text='<%#Eval("GetPassState") %>'></asp:Label>
                                        </td>
                                        <td align="left" style="border-bottom: 2px Solid Black;">
                                            Amount:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                runat="server" ID="lblAmount" Text='<%#Eval("Amount") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td style="height: 50px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="3">
                                            For,<asp:Label runat="server" ID="lblForCompany" Font-Bold="true" Text=""></asp:Label>
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
