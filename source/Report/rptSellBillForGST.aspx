<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptSellBillForGST.aspx.cs"
    Inherits="Report_rptSellBillForGST" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>INVOICE</title>
    <link href="../print.css" rel="stylesheet" type="text/css" media="print" />
    <script type="text/javascript">
        function PrintPanel() {
            debugger;
            var panel = document.getElementById("<%=pnlMain.ClientID %>");
            var printWindow = window.open('salebill.html', 'do', 'height=660,width=1350');
            printWindow.document.write('<html><head><link href="../print.cssf" rel="stylesheet" type="text/css" media="print"/>');
            printWindow.document.write('</head><body class="print">');
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
            var printWindow = window.open('salebill.html', 'do', 'height=660,width=1350');
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
            &nbsp;&nbsp;<asp:Button runat="server" ID="btnSendEmail" Text="HTML Mail" OnClick="btnSendEmail_Click"
                Width="80px" />&nbsp;
        <asp:Button runat="server" ID="btnPDFMail" Text="PDF Mail" Width="80px" OnClick="btnPDFMail_Click" />&nbsp;
        <asp:TextBox runat="server" ID="txtEmail" Width="300px"></asp:TextBox>
        </div>
        <br />
        <div>
            <asp:Panel runat="server" ID="pnlMain" CssClass="toosmall" Style="width: 70%; margin: 0 auto;">
                <table width="100%" align="center" cellspacing="4" cellpadding="0" class="toosmall"
                    runat="server" id="tblmn">
                    <tr>
                        <td style="width: 100%;" class="toosmall">
                            <asp:DataList runat="server" ID="dtlist" Width="100%" OnItemDataBound="dtlist_OnItemDataBound"
                                CssClass="print9pt">
                                <ItemTemplate>
                                    <table width="100%" style="table-layout: fixed; height: 100px;" class="toosmall">
                                        <tr>
                                            <td style="width: 20%; vertical-align: top;" align="center" class="toosmall">
                                                <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/DashLogo.jpg" Width="100%"
                                                    Height="90%" />
                                            </td>
                                            <td style="width: 80%; vertical-align: top;" align="left" class="toosmall">
                                                <table width="100%" style="table-layout: fixed;" class="toosmall">
                                                    <tr>
                                                        <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="toosmall">
                                                            <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" CssClass="toosmall"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; text-transform: uppercase; color: Red; font-size: medium"
                                                            class="toosmall">
                                                            <asp:Label ID="Label8" runat="server" Text="" Font-Bold="true" CssClass="toosmall"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                            <asp:Label runat="server" ID="lblAl1" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                            <asp:Label runat="server" ID="lblAl2" ForeColor="Blue" CssClass="toosmall">  </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                            <asp:Label runat="server" ID="lblAl3" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                            <asp:Label runat="server" ID="lblAl4" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana; font-size: medium" class="toosmall">
                                                            <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue" CssClass="toosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; font-size: small; border-bottom: 1px solid black; border-top: 1px solid black;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 100%; height: 15px; text-align: center; padding-top: 4px;">
                                               <b>TAX</b>&nbsp <b>INVOICE</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="toosmall" style="height: 80px; border-top: 1px solid black;">
                                                <table cellspacing="0" style="width: 100%; table-layout: fixed; height: 80px;" class="toosmall">
                                                    <tr>
                                                        <td style="width: 50%; border-right: 1px solid black; border-bottom: 1px solid black;"
                                                            align="left" class="toosmall">
                                                            <table style="width: 100%; table-layout: fixed; font-size: small" class="toosmall">
                                                                <tr>
                                                                    <td style="width: 35%;">Reverse Charge:
                                                                    </td>
                                                                    <td align="left">NO
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Invoice No:
                                                                    </td>
                                                                    <td align="left">
                                                                          <b>2023-2024-</b><asp:Label runat="server" ID="lblNewSB" Text='<%#Eval("newsbno") %>' Font-Bold="true"></asp:Label>
                                                                        <asp:Label runat="server" ID="lblSB_No" Text='<%#Eval("#") %>' Font-Bold="true" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Invoice Date:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label runat="server" ID="lblBillDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>State:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label Text='<%#Eval("CompanyState") %>' Font-Bold="true" runat="server" />
                                                                    </td>
                                                                    <td>State Code:
                                                                    <asp:Label Text='<%#Eval("CompanyGSTStateCode") %>' runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 50%; border-bottom: 1px solid black;" align="left" class="toosmall">
                                                            <table style="width: 100%; font-size: small" class="toosmall ">
                                                                <tr>
                                                                    <td align="left" style="width: 40%">Our GST Number:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label19" Text='<%#Eval("CompanyGST") %>' runat="server" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">Transportation Mode:
                                                                    </td>
                                                                    <td align="left"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">Date Of Supply:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label Text='<%#Eval("dt") %>' runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">Place Of Supply:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label Text='<%#Eval("CompanyState") %>' runat="server" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="toosmall" style="height: 100px;">
                                                <table width="100%" align="center" style="height: 100px; table-layout: fixed; font-size: small"
                                                    class="toosmall">
                                                    <tr>
                                                        <td align="left" style="width: 50%; height: 100px; vertical-align: top; border-right: 1px solid black;"
                                                            class="toosmall">
                                                            <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed; font-size: small; height: 100%;">
                                                                <tr>
                                                                    <td align="left" class="toosmall">Buyer,
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="lblPartyName" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="lblPartyAddr" runat="server" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">City:&nbsp;<asp:Label ID="lblPartyCity" runat="server" Font-Bold="true" Text='<%#Eval("party_city") %>'
                                                                        class="toosmall"></asp:Label>
                                                                        &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblPartyState" Text='<%#Eval("party_state") %>'
                                                                            class="toosmall" Font-Bold="true"></asp:Label>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;State
                                                                    Code:&nbsp;<asp:Label Text='<%#Eval("PartyGSTStateCode") %>' runat="server" class="toosmall" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top; height: 40px;" class="toosmall">
                                                                        <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblGSTNo" runat="server" Text='<%#Eval("Party_Gst") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label12" runat="server" Text='<%#Eval("Party_PAN") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label24" runat="server" Text='<%#Eval("Party_FSSAI") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblLICNo" runat="server" Text='<%#Eval("Party_SLN") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EwayBill No:</b>&nbsp;<asp:Label ID="lblEwaybill_NO" runat="server" Text='<%#Eval("Eway_Bill_No") %>'
                                                                                        Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EINVOICE NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label28" Text='<%#Eval("einvoiceno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>ACK NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label36" Text='<%#Eval("ackno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>

                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left" style="width: 50%; height: 100px; vertical-align: top;" class="toosmall">
                                                            <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed; font-size: small; height: 100%;">
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text='<%#Eval("UnitName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("UnitAddress") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("UnitCity") %>' class="toosmall"></asp:Label>&nbsp;
                                                                    <asp:Label ID="Label7" Text='<%#Eval("UnitGSTStateCode") %>' runat="server" class="toosmall" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top; height: 40px;" class="toosmall">
                                                                        <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("UnitGST") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label5" runat="server" Text='<%#Eval("UnitPan") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label27" runat="server" Text='<%#Eval("UnitFSSAI") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label6" runat="server" Text='<%#Eval("UnitLicNo") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EwayBill No:</b>&nbsp;<asp:Label ID="Label21" runat="server" Text='<%#Eval("Eway_Bill_No") %>'
                                                                                        Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EINVOICE NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label32" Text='<%#Eval("einvoiceno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>ACK NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label33" Text='<%#Eval("ackno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
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
                                    <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; font-size: small; border-bottom: 1px solid black;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 60%; padding-top: 8px;" align="left" class="toosmall">Mill Name:&nbsp;<asp:Label runat="server" ID="lblMillName" Text='<%#Eval("Mill_Name") %>'></asp:Label>
                                            </td>
                                            <td style="width: 40%;" align="left" class="toosmall">
                                                <asp:Label runat="server" ID="Label9w" Text='<%#Eval("PODetails") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%;" align="left" class="toosmall">Dispatched From:&nbsp;<asp:Label runat="server" ID="lblDispatchedFrom" Text='<%#Eval("From_Place") %>'
                                                Font-Bold="true"></asp:Label>
                                            </td>
                                            <td style="width: 40%;" align="left" class="toosmall">To:&nbsp;<asp:Label runat="server" ID="Labewl9" Text='<%#Eval("To_Place") %>' Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="toosmall" style="padding-bottom: 10px">Lorry No:&nbsp;<asp:Label runat="server" ID="lbllorry" Text='<%#Eval("lorry") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; FSSAI_NO:<asp:Label
                                                    runat="server" ID="Label35" Text='<%#Eval("millfssai") %>' Font-Bold="true"></asp:Label>
                                            </td>
                                            <td align="left" class="toosmall">
                                                <asp:Label runat="server" ID="Label10" Text='<%#Eval("DriverMobile") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black; font-size: small"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 20%;" align="left">
                                                <b>Particulars</b>
                                            </td>
                                            <td style="width: 10%;" align="center">
                                                <b>HSN/ACS</b>
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
                                    <table width="100%" style="table-layout: fixed; font-size: small" align="center"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 100%; vertical-align: top; height: 40px;" class="toosmall">
                                                <asp:DataList runat="server" ID="dtItemDetails" Width="100%" class="toosmall">
                                                    <ItemTemplate>
                                                        <table width="100%" align="center" cellspacing="0px" style="font-size: medium; table-layout: fixed; border-bottom: 1px dashed black;"
                                                            class="toosmall">
                                                            <tr>
                                                                <td style="width: 20%;" align="left">
                                                                    <asp:Label runat="server" ID="lblGrade" Text='<%#Eval("Item") %>'></asp:Label>
                                                                </td>
                                                                <td style="width: 10%;" align="center">
                                                                    <asp:Label runat="server" ID="Label9" Text='<%#Eval("HSN") %>'></asp:Label>
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
                                    <table width="100%" align="center" cellspacing="2" style="font-size: small; table-layout: fixed; border-bottom: 1px solid black; height: 130px;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Sub Total:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblSubTotal" Text='<%#Eval("Sub_Total") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Frieght:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblLessFrieght" Text='<%#Eval("Less_Frieght").ToString()=="0.00" || Eval("Less_Frieght").ToString()=="0"?"":Eval("Less_Frieght","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Taxable Amount:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label20" Text='<%#Eval("TaxableAmount").ToString()=="0.00" || Eval("TaxableAmount").ToString()=="0"?"":Eval("TaxableAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>CGST %:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label16" Text='<%#Eval("CGSTRate").ToString()=="0.00" || Eval("CGSTRate").ToString()=="0"?"":Eval("CGSTRate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                    runat="server" ID="Label13" Text='<%#Eval("CGSTAmount").ToString()=="0.00" || Eval("CGSTAmount").ToString()=="0"?"":Eval("CGSTAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>SGST %: </b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label17" Text='<%#Eval("SGSTRate").ToString()=="0.00" || Eval("SGSTRate").ToString()=="0"?"":Eval("SGSTRate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="Label14" Text='<%#Eval("SGSTAmount").ToString()=="0.00" || Eval("SGSTAmount").ToString()=="0"?"":Eval("SGSTAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>IGST %:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label18" Text='<%#Eval("IGSTRate").ToString()=="0.00" || Eval("IGSTRate").ToString()=="0"?"":Eval("IGSTRate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="Label15" Text='<%#Eval("IGSTAmount").ToString()=="0.00" || Eval("IGSTAmount").ToString()=="0"?"":Eval("IGSTAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                        <td style="width: 80%;" align="right" class="toosmall">
                                            <b>Cash Advance:</b>
                                        </td>
                                        <td style="width: 20%;" align="right" class="toosmall">
                                            <asp:Label runat="server" ID="lblCashAdvance" Text='<%#Eval("Cash_Advance").ToString()=="0.00" || Eval("Cash_Advance").ToString()=="0"?"":Eval("Cash_Advance","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Rate Diff:</b><asp:Label runat="server" ID="lblBankCommRate" Text='<%#Eval("RateDiff").ToString()=="0.00"?"":Eval("RateDiff","{0}") %>'></asp:Label>/Qntl:
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblBankComm" Text='<%#Eval("Bank_Commission").ToString()=="0.00"?"":Eval("Bank_Commission","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Other Expense:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblOtherExpe" Text='<%#Eval("Other_Expenses").ToString()=="0.00"?"":Eval("Other_Expenses","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <span style="text-align: left; float: left;" class="toosmall">
                                                    <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("ASN_No") %>'
                                                        Visible="true"></asp:Label></span> <b>Total Amount:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblBillAmount" Text='<%#Eval("Bill_Amount") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>TCS %::</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label30" Text='<%#Eval("TCS_Rate").ToString()=="0.000" || Eval("TCS_Rate").ToString()=="0"?"":Eval("TCS_Rate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="Label26" Text='<%#Eval("TCS_Amt").ToString()=="0.00" || Eval("TCS_Amt").ToString()=="0"?"":Eval("TCS_Amt","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%; border-bottom: 1px solid black;" align="right" class="toosmall">
                                                <b>Net Payable With TCS:</b>
                                            </td>
                                            <td style="width: 20%; border-bottom: 1px solid black;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblTCSNet_Payable" Text='<%#Eval("TCS_Net_Payable") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left" class="toosmall">Rs.:<asp:Label runat="server" ID="lblInwords" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" cellspacing="0" style="table-layout: fixed; height: 80px; font-size: small"
                                        class="toosmallforimg">
                                        <tr>
                                            <td align="left" colspan="2" class="toosmallforimg">
                                            Our PAN No.:
                                            <asp:Label runat="server" ID="lblCmptinNo" Text="" Font-Bold="true"></asp:Label>&nbsp;&nbsp;FSSAI
                                            No:<asp:Label runat="server" ID="lblCompnayFSSAI_No" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%;" align="left" class="toosmallforimg">
                                                <b><u>Note:</u></b>&nbsp;After Dispatch of the goods we are not responsible for
                                            non delivery or any kind of damage.
                                            </td>
                                            <td rowspan="4" align="right" style="vertical-align: top;" class="toosmallforimg">
                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="100px" CssClass="toosmall" /><br />
                                                For,<asp:Label runat="server" ID="lblNameCmp" Font-Bold="true" CssClass="toosmall"></asp:Label><br />
                                                <%--<p style="font-size: small; font-style: italic;">--%>
                                            Authorised Signatory<%--</p>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="toosmallforimg">1) Please credit the amount in our account and send the amount by RTGS immediately.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="toosmallforimg">2) If the amount is not sent before the due date payment Interest 24% will be charged.
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
            <asp:Panel runat="server" ID="pnl2" CssClass="toosmall">
                <table width="70%" align="center" cellspacing="4" cellpadding="0" class="print9pt">
                    <tr>
                        <td style="width: 100%;" class="print9pt">
                            <asp:DataList runat="server" ID="dtlist1" Width="100%" OnItemDataBound="dtlist_OnItemDataBound"
                                CssClass="print9pt">
                                <ItemTemplate>
                                    <table width="100%" style="table-layout: fixed; height: 100px;" class="noprinttoosmall">
                                        <tr>
                                            <td style="width: 20%; vertical-align: top;" align="center" class="noprinttoosmall"></td>
                                            <td style="width: 80%; vertical-align: top;" align="left" class="noprinttoosmall">
                                                <table width="100%" style="table-layout: fixed;" class="noprinttoosmall">
                                                    <tr>
                                                        <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprinttoosmall">
                                                            <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" CssClass="noprinttoosmall"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="noprinttoosmall">
                                                            <asp:Label ID="Label8" runat="server" Text="" Font-Bold="true" CssClass="noprinttoosmall"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                            <asp:Label runat="server" ID="lblAl1" ForeColor="Blue" CssClass="noprinttoosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                            <asp:Label runat="server" ID="lblAl2" ForeColor="Blue" CssClass="noprinttoosmall">  </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                            <asp:Label runat="server" ID="lblAl3" ForeColor="Blue" CssClass="noprinttoosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                            <asp:Label runat="server" ID="lblAl4" ForeColor="Blue" CssClass="noprinttoosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%; font-family: Verdana;" class="noprinttoosmall">
                                                            <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue" CssClass="noprinttoosmall"> </asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black; border-top: 1px solid black;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 100%; height: 15px; border-bottom: 1px solid black; text-align: center;">
                                                <b>INVOICE</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="toosmall" style="height: 80px;">
                                                <table cellspacing="0" style="width: 100%; table-layout: fixed; height: 80px;" class="toosmall">
                                                    <tr>
                                                        <td style="width: 50%; border-right: 1px solid black; border-bottom: 1px solid black;"
                                                            align="left" class="toosmall">
                                                            <table style="width: 100%; table-layout: fixed;" class="toosmall">
                                                                <tr>
                                                                    <td style="width: 30%">Reverse Charge:
                                                                    </td>
                                                                    <td align="left">NO
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Invoice No:
                                                                    </td>
                                                                    <td align="left">
                                                                         <b>2023-2024-</b> <asp:Label runat="server" ID="lblNewSB" Text='<%#Eval("newsbno") %>' Font-Bold="true"></asp:Label>
                                                                        <asp:Label runat="server" ID="lblSB_No" Text='<%#Eval("#") %>' Font-Bold="true" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Invoice Date:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label runat="server" ID="lblBillDate" Text='<%#Eval("dt") %>' Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>State:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label11" Text='<%#Eval("CompanyState") %>' Font-Bold="true" runat="server" />
                                                                    </td>
                                                                    <td>State Code:
                                                                    <asp:Label ID="Label21" Text='<%#Eval("CompanyGSTStateCode") %>' runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 50%; border-bottom: 1px solid black;" align="left" class="toosmall">
                                                            <table style="width: 100%;" class="toosmall">
                                                                <tr>
                                                                    <td align="left" style="width: 40%">Our GST Number:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label19" Text='<%#Eval("CompanyGST") %>' runat="server" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">Transportation Mode:
                                                                    </td>
                                                                    <td align="left"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">Date Of Supply:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label22" Text='<%#Eval("dt") %>' runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">Place Of Supply:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label23" Text='<%#Eval("CompanyState") %>' runat="server" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="toosmall" style="height: 100px;">
                                                <table width="100%" align="center" style="height: 100px; table-layout: fixed;" class="toosmall">
                                                    <tr>
                                                        <td align="left" style="width: 50%; height: 100px; vertical-align: top; border-right: 1px solid black;"
                                                            class="toosmall">
                                                            <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed; height: 100%;">
                                                                <tr>
                                                                    <td align="left" class="toosmall">Buyer,
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="lblPartyName" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="lblPartyAddr" runat="server" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">City:&nbsp;<asp:Label ID="lblPartyCity" runat="server" Font-Bold="true" Text='<%#Eval("party_city") %>'
                                                                        class="toosmall"></asp:Label>
                                                                        &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblPartyState" Text='<%#Eval("party_state") %>'
                                                                            class="toosmall" Font-Bold="true"></asp:Label>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;State
                                                                    Code:&nbsp;<asp:Label ID="Label24" Text='<%#Eval("PartyGSTStateCode") %>' runat="server"
                                                                        class="toosmall" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top; height: 40px;" class="toosmall">
                                                                        <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblGSTNo" runat="server" Text='<%#Eval("Party_Gst") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label12" runat="server" Text='<%#Eval("Party_PAN") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label38" runat="server" Text='<%#Eval("Party_FSSAI") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblLICNo" runat="server" Text='<%#Eval("Party_SLN") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EwayBill No:</b>&nbsp;<asp:Label ID="Label25" runat="server" Text='<%#Eval("Eway_Bill_No") %>'
                                                                                        Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EINVOICE NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label28" Text='<%#Eval("einvoiceno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>ACK NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label36" Text='<%#Eval("ackno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="left" style="width: 50%; height: 100px; vertical-align: top;" class="toosmall">
                                                            <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed; height: 100%;">
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Text='<%#Eval("UnitName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="toosmall">
                                                                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("UnitAddress") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="font-size: medium;" class="toosmall">
                                                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("UnitCity") %>' class="toosmall"></asp:Label>&nbsp;
                                                                    <asp:Label ID="Label7" Text='<%#Eval("UnitGSTStateCode") %>' runat="server" class="toosmall" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="vertical-align: top; height: 40px;" class="toosmall">
                                                                        <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("UnitGST") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label5" runat="server" Text='<%#Eval("UnitPan") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label34" runat="server" Text='<%#Eval("UnitFSSAI") %>' Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="Label6" runat="server" Text='<%#Eval("UnitLicNo") %>'></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EwayBill No:</b>&nbsp;<asp:Label ID="Label26" runat="server" Text='<%#Eval("Eway_Bill_No") %>'
                                                                                        Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td align="left"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>EINVOICE NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label29" Text='<%#Eval("einvoiceno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>ACK NO:</b>&nbsp;
                                                                                <asp:Label runat="server" ID="Label31" Text='<%#Eval("ackno") %>' Font-Bold="true"
                                                                                    Font-Size="Small"></asp:Label>
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
                                    <table width="100%" align="center" cellspacing="4" style="table-layout: fixed; border-bottom: 1px solid black;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 60%;" align="left" class="toosmall">Mill Name:&nbsp;<asp:Label runat="server" ID="lblMillName" Text='<%#Eval("Mill_Name") %>'></asp:Label>
                                            </td>
                                            <td style="width: 40%;" align="left" class="toosmall">
                                                <asp:Label runat="server" ID="Label9w" Text='<%#Eval("PODetails") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 60%;" align="left" class="toosmall">Dispatched From:&nbsp;<asp:Label runat="server" ID="lblDispatchedFrom" Text='<%#Eval("From_Place") %>'
                                                Font-Bold="true"></asp:Label>
                                            </td>
                                            <td style="width: 40%;" align="left" class="toosmall">To:&nbsp;<asp:Label runat="server" ID="Labewl9" Text='<%#Eval("To_Place") %>' Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="toosmall">Lorry No:&nbsp;<asp:Label runat="server" ID="lbllorry" Text='<%#Eval("lorry") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; FSSAI_NO:<asp:Label
                                                    runat="server" ID="Label35" Text='<%#Eval("millfssai") %>' Font-Bold="true"></asp:Label>
                                            </td>
                                            <td align="left" class="toosmall">
                                                <asp:Label runat="server" ID="Label10" Text='<%#Eval("DriverMobile") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 20%;" align="left">
                                                <b>Particulars</b>
                                            </td>
                                            <td style="width: 10%;" align="center">
                                                <b>HSN/ACS</b>
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
                                    <table width="100%" style="table-layout: fixed;" align="center" class="toosmall">
                                        <tr>
                                            <td style="width: 100%; vertical-align: top; height: 40px;" class="toosmall">
                                                <asp:DataList runat="server" ID="dtItemDetails" Width="100%" class="toosmall">
                                                    <ItemTemplate>
                                                        <table width="100%" align="center" cellspacing="0px" style="table-layout: fixed; border-bottom: 1px dashed black;"
                                                            class="toosmall">
                                                            <tr>
                                                                <td style="width: 20%;" align="left">
                                                                    <asp:Label runat="server" ID="lblGrade" Text='<%#Eval("Item") %>'></asp:Label>
                                                                </td>
                                                                <td style="width: 10%;" align="center">
                                                                    <asp:Label runat="server" ID="Label9" Text='<%#Eval("HSN") %>'></asp:Label>
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
                                    <table width="100%" align="center" cellspacing="2" style="font-size: medium; table-layout: fixed; border-bottom: 1px solid black; height: 130px;"
                                        class="toosmall">
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Sub Total:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblSubTotal" Text='<%#Eval("Sub_Total") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Frieght:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblLessFrieght" Text='<%#Eval("Less_Frieght").ToString()=="0.00" || Eval("Less_Frieght").ToString()=="0"?"":Eval("Less_Frieght","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Taxable Amount:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label20" Text='<%#Eval("TaxableAmount").ToString()=="0.00" || Eval("TaxableAmount").ToString()=="0"?"":Eval("TaxableAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>CGST %:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label16" Text='<%#Eval("CGSTRate").ToString()=="0.00" || Eval("CGSTRate").ToString()=="0"?"":Eval("CGSTRate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                    runat="server" ID="Label13" Text='<%#Eval("CGSTAmount").ToString()=="0.00" || Eval("CGSTAmount").ToString()=="0"?"":Eval("CGSTAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>SGST %: </b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label17" Text='<%#Eval("SGSTRate").ToString()=="0.00" || Eval("SGSTRate").ToString()=="0"?"":Eval("SGSTRate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="Label14" Text='<%#Eval("SGSTAmount").ToString()=="0.00" || Eval("SGSTAmount").ToString()=="0"?"":Eval("SGSTAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>IGST %:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label18" Text='<%#Eval("IGSTRate").ToString()=="0.00" || Eval("IGSTRate").ToString()=="0"?"":Eval("IGSTRate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="Label15" Text='<%#Eval("IGSTAmount").ToString()=="0.00" || Eval("IGSTAmount").ToString()=="0"?"":Eval("IGSTAmount","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                        <td style="width: 80%;" align="right" class="toosmall">
                                            <b>Cash Advance:</b>
                                        </td>
                                        <td style="width: 20%;" align="right" class="toosmall">
                                            <asp:Label runat="server" ID="lblCashAdvance" Text='<%#Eval("Cash_Advance").ToString()=="0.00" || Eval("Cash_Advance").ToString()=="0"?"":Eval("Cash_Advance","{0}") %>'></asp:Label>
                                        </td>
                                    </tr>--%>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Rate Diff:</b><asp:Label runat="server" ID="lblBankCommRate" Text='<%#Eval("RateDiff").ToString()=="0.00"?"":Eval("RateDiff","{0}") %>'></asp:Label>/Qntl:
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblBankComm" Text='<%#Eval("Bank_Commission").ToString()=="0.00"?"":Eval("Bank_Commission","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>Other Expense:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblOtherExpe" Text='<%#Eval("Other_Expenses").ToString()=="0.00"?"":Eval("Other_Expenses","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <span style="text-align: left; float: left;" class="toosmall">
                                                    <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("ASN_No") %>'
                                                        Visible="true"></asp:Label></span> <b>Total Amount:</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblBillAmount" Text='<%#Eval("Bill_Amount") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%;" align="right" class="toosmall">
                                                <b>TCS %::</b>
                                            </td>
                                            <td style="width: 20%;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="Label30" Text='<%#Eval("TCS_Rate").ToString()=="0.000" || Eval("TCS_Rate").ToString()=="0"?"":Eval("TCS_Rate","{0}") %>'></asp:Label>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="Label27" Text='<%#Eval("TCS_Amt").ToString()=="0.00" || Eval("TCS_Amt").ToString()=="0"?"":Eval("TCS_Amt","{0}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 80%; border-bottom: 1px solid black;" align="right" class="toosmall">
                                                <b>Net Payable With TCS:</b>
                                            </td>
                                            <td style="width: 20%; border-bottom: 1px solid black;" align="right" class="toosmall">
                                                <asp:Label runat="server" ID="lblTCSNet_Payable" Text='<%#Eval("TCS_Net_Payable") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left" class="toosmall">Rs.:<asp:Label runat="server" ID="lblInwords" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" cellspacing="0" style="table-layout: fixed; height: 80px;"
                                        class="toosmallforimg">
                                        <tr>
                                           <td align="left" colspan="2" class="toosmallforimg">
                                            Our PAN No.:
                                            <asp:Label runat="server" ID="lblCmptinNo" Text="" Font-Bold="true"></asp:Label>&nbsp;&nbsp;FSSAI
                                            No:<asp:Label runat="server" ID="lblCompnayFSSAI_No" Text="" Font-Bold="true"></asp:Label>
                                        </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%;" align="left" class="toosmallforimg">
                                                <b><u>Note:</u></b>&nbsp;After Dispatch of the goods we are not responsible for
                                            non delivery or any kind of damage.
                                            </td>
                                            <td rowspan="4" align="right" style="vertical-align: top;" class="toosmallforimg">
                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="100px" CssClass="toosmall" /><br />
                                                For,<asp:Label runat="server" ID="lblNameCmp" Font-Bold="true" CssClass="toosmall"></asp:Label><br />
                                                <%--<p style="font-size: small; font-style: italic;">--%>
                                            Authorised Signatory<%--</p>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="toosmallforimg">1) Please credit the amount in our account and send the amount by RTGS immediately.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="toosmallforimg">2) If the amount is not sent before the due date payment Interest 24% will be charged.
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
