<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptpanrlToPdf.aspx.cs" Inherits="Reports_rptpanrlToPdf" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btnMail" runat="server" Text="Mail" CssClass="btnHelp" Width="80px"
        OnClick="btnMail_Click" />
    <div>
        <asp:Panel ID="Panel1" runat="server">
            <asp:Label ID="lblCompanyName" runat="server" Text="" CssClass="lblName"
                Font-Size="12px"></asp:Label>
            <br />
            <asp:Label ID="Label15" runat="server" Text="Sugar Balance Stock" CssClass="lblName"
                Font-Size="12px"></asp:Label>
            <table style="width: 80%">
                <tr>
                    <td align="left" style="width: 5%;">
                        <asp:Label ID="Label1" runat="server" Text="T.No." Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 8%;">
                        <asp:Label ID="Label2" runat="server" Text="Date" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%;">
                        <asp:Label ID="Label3" runat="server" Text="Mill" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="left" style="width: 15%;">
                        <asp:Label ID="Label4" runat="server" Text="Grade" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:Label ID="Label5" runat="server" Text="Lot" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:Label ID="Label6" runat="server" Text="M.R." Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 5%;">
                        <asp:Label ID="Label8" runat="server" Text="P.R." Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="Label9" runat="server" Text="Qntl" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="Label10" runat="server" Text="Desp" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 7%;">
                        <asp:Label ID="Label11" runat="server" Text="Bal" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 6%;">
                        <asp:Label ID="Label12" runat="server" Text="Lift" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right" style="width: 8%;">
                        <asp:Label ID="Label13" runat="server" Text="DO" Font-Size="8px" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:DataList ID="DataList2" runat="server" Width="80%">
                <ItemTemplate>
                    <table align="center" cellpadding="1px" cellspacing="0" style="width: 100%">
                        <tr>
                            <td align="left" style="width: 5%;">
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Tender_No") %>' Font-Size="8px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server" Visible="false">
            <asp:DataList ID="DataList1" runat="server" Width="80%" OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table style="width: 100%" align="center" cellpadding="1px" cellspacing="0px">
                        <tr>
                            <td align="left" style="width: 5%;">
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("Tender_No") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="left" style="width: 8%;">
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("Tender_Date") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("millname") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="left" style="width: 15%;">
                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("Grade") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 5%;">
                                <asp:Label ID="Label5" runat="server" Text='<%#Eval("Quantal") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 5%;">
                                <asp:Label ID="Label6" runat="server" Text='<%#Eval("Mill_Rate") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 5%;">
                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("Purc_Rate") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 7%;">
                                Quantal
                            </td>
                            <td align="right" style="width: 7%;">
                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("desp") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 7%;">
                                <asp:Label ID="Label10" runat="server" Text='<%#Eval("bal") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 6%;">
                                <asp:Label ID="Label12" runat="server" Text='<%#Eval("Lifting_Date") %>' Font-Size="8px"></asp:Label>
                            </td>
                            <td align="right" style="width: 8%;">
                                <asp:Label ID="Label13" runat="server" Text='<%#Eval("doname") %>' Font-Size="8px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="12" align="left">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:DataList ID="dtl" runat="server" Width="100%">
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" style="width: 5%;">
                                                                <asp:Label ID="Label7" runat="server" Font-Size="8px" Font-Bold="true" Text='<%#Eval("ID") %>'></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 48%;" colspan="6">
                                                                <asp:Label ID="lblBuyer" runat="server" Font-Size="8px" Font-Bold="true" Text='<%#Eval("buyerbrokerfullname") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 7%;">
                                                                <asp:Label ID="Label14" runat="server" Font-Size="8px" Font-Bold="true" Text='<%#Eval("Buyer_Quantal") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 7%;">
                                                                <asp:Label ID="lbldespatchqty" runat="server" Font-Size="8px" Font-Bold="true" Text='<%#Eval("despatchqty") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 7%;">
                                                                <asp:Label ID="lblbalance" runat="server" Font-Size="8px" Font-Bold="true" Text='<%#Eval("balance") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 6%;">
                                                            </td>
                                                            <td style="width: 8%;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <%--<td colspan="12" style="  border-bottom:dashed 1px gray;">
</td>--%>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        <asp:Panel ID="pnl" runat="server">
            test
        </asp:Panel>
    </div>
    </form>
</body>
</html>
