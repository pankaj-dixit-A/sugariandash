<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDO.aspx.cs" Inherits="Report_rptDO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnl.ClientID %>");
            var printWindow = window.open('do.html', 'do', 'height=400,width=800');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 100);
            return false;
        }
    </script>
    <script type="text/javascript" src="../JS/emailValidation.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" OnClientClick="return PrintPanel();"
            Width="80px" />
        &nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel"
            Width="120px" OnClick="btnExportToExcel_Click" />
        &nbsp;
        <asp:Button ID="btnMail" runat="server" Text="Mail" CssClass="btnHelp" Width="80px"
            OnClientClick="CheckEmail();" OnClick="btnMail_Click" />&nbsp;<asp:TextBox runat="server"
                ID="txtEmail" Width="226px"></asp:TextBox>
    </div>
    <div>
        <asp:Panel ID="pnl" runat="server" Width="95%" align="center" ForeColor="Black" Font-Size="Medium"
            BorderColor="Black" BorderStyle="Solid" BorderWidth="0px">
            <asp:Label ID="lblReportName" runat="server" Text="Delivery Order" CssClass="lblName"
                Font-Bold="true" Font-Size="Large" Style="text-align: center; width: 100%;"></asp:Label>
            <asp:DataList ID="DataList1" runat="server" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table width="100%" border="0px" style="page-break-after: always;">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblCompanyName" Font-Bold="true" Font-Size="Medium" runat="server"
                                    Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblCompanyAddr" runat="server" Text="Kolhapur"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblCompanyMobile" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                Doc No.:
                                <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("doc_no") %>'></asp:Label>
                            </td>
                            <td align="right" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                Date:
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("doc_date") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Mill Name:
                                <asp:Label ID="lblMillName" runat="server" Text='<%#Eval("millName") %>'></asp:Label>
                            </td>
                            <td align="right">
                                sale Note No.:
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: solid 1px black;">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            GETPASS:
                                        </td>
                                        <td align="left">
                                            LIC No.:
                                            <asp:Label ID="lblLic" runat="server" Font-Bold="true" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblGetPassName" runat="server" Font-Bold="true" Text='<%#Eval("GetPassName") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            CST No.:
                                            <asp:Label ID="Label2" runat="server" Font-Bold="true" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" rowspan="3">
                                            <asp:Label ID="lblgetpassAddr" runat="server" Font-Bold="true" Text='<%#Eval("getpassAddress") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            ECC No.::
                                            <asp:Label ID="Label3" runat="server" Font-Bold="true" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td align="left">

</td>--%>
                                        <td align="left">
                                            TIN No.::
                                            <asp:Label ID="Label5" runat="server" Font-Bold="true" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="border-bottom: solid 1px black;">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            Quantal :&nbsp;&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblQntl" runat="server" Font-Bold="true" Text='<%#Eval("quantal") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Grade :&nbsp;&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblGrade" runat="server" Font-Bold="true" Text='<%#Eval("grade") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Truck No. :&nbsp;&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblTruckNo" runat="server" Font-Bold="true" Text='<%#Eval("truck_no") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width: 100%;">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            With Excise rate:
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            Total Amount
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Excise:
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("excise_rate") %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            Less Amount:
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Without Excise:
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            Final Amount:
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text='<%#Eval("diff_amount") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Email ID:
                                <asp:Label ID="lblMillEmail" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <hr style="color: Black; width: 1000px;" />
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
