<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptDOPSSB.aspx.cs" Inherits="Report_rptDOPSSB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="80%" align="center" style="table-layout: fixed; border-bottom: 1px solid black;
            border-top: 1px solid black;">
            <tr>
                <td align="center" style="width: 2%;">
                    Do#
                </td>
                <td align="center" style="width: 4%;">
                    Do_Date
                </td>
                <td align="center" style="width: 3%;">
                    Qntl
                </td>
                <td align="center" style="width: 4%;">
                    Sale Rate
                </td>
                <td align="center" style="width: 2%;">
                    PS No.
                </td>
                <td align="center" style="width: 2%;">
                    PS_Qntl
                </td>
                <td align="center" style="width: 4%;">
                    Rate
                </td>
                <td align="center" style="width: 2%;">
                    Bill No.
                </td>
                <td align="center" style="width: 3%;">
                    SB_Qntl
                </td>
                <td align="center" style="width: 2%;">
                    Rate
                </td>
            </tr>
        </table>
        <table width="80%" align="center" style="table-layout: fixed;">
            <tr>
                <td style="width: 100%;">
                    <asp:DataList runat="server" ID="dtl" Width="100%">
                        <ItemTemplate>
                            <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px dashed black;">
                                <tr>
                                    <td align="center" style="width: 2%;">
                                        <asp:Label runat="server" ID="lbl1" Text='<%#Eval("DO_No") %>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 4%;">
                                        <asp:Label runat="server" ID="Label1" Text='<%#Eval("Date") %>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 3%;">
                                        <asp:Label runat="server" ID="Label2" Text='<%#Eval("DO_Qntl") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="center" style="width: 4%;">
                                        <asp:Label runat="server" ID="Label3" Text='<%#Eval("Sale_Rate") %>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 2%;">
                                        <asp:Label runat="server" ID="Label4" Text='<%#Eval("PS_No") %>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 2%;">
                                        <asp:Label runat="server" ID="Label5" Text='<%#Eval("PS_Qntl") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="center" style="width: 4%;">
                                        <asp:Label runat="server" ID="Label6" Text='<%#Eval("PS_Rate") %>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 2%;">
                                        <asp:Label runat="server" ID="Label7" Text='<%#Eval("Bill_No") %>'></asp:Label>
                                    </td>
                                    <td align="center" style="width: 3%;">
                                        <asp:Label runat="server" ID="Label8" Text='<%#Eval("SB_Qntl") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="center" style="width: 2%;">
                                        <asp:Label runat="server" ID="Label9" Text='<%#Eval("SB_Rate") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td style="width: 100%;">
                    <table width="100%" align="center" style="table-layout: fixed; border-bottom: 1px Solid black;
                        border-top: 1px Solid black;">
                        <tr>
                            <td align="center" style="width: 2%;">
                                <asp:Label runat="server" ID="lbl1" Text=""></asp:Label>
                            </td>
                            <td align="center" style="width: 4%;">
                                <asp:Label runat="server" ID="Label1" Text=""></asp:Label>
                            </td>
                            <td align="center" style="width: 3%;">
                                <asp:Label runat="server" ID="lblDOQntlTotal" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="center" style="width: 4%;">
                                <asp:Label runat="server" ID="Label3" Text=""></asp:Label>
                            </td>
                            <td align="center" style="width: 2%;">
                                <asp:Label runat="server" ID="Label4" Text=""></asp:Label>
                            </td>
                            <td align="center" style="width: 2%;">
                                <asp:Label runat="server" ID="lblPSQntlTotal" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="center" style="width: 4%;">
                                <asp:Label runat="server" ID="Label6" Text=""></asp:Label>
                            </td>
                            <td align="center" style="width: 2%;">
                                <asp:Label runat="server" ID="Label7" Text=""></asp:Label>
                            </td>
                            <td align="center" style="width: 3%;">
                                <asp:Label runat="server" ID="lblSBQntlTotal" Text="" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="center" style="width: 2%;">
                                <asp:Label runat="server" ID="Label9" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
