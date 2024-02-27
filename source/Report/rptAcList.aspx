<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rptAcList.aspx.cs" Inherits="Report_rptAcList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <table width="1000px" align="center">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="City :" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblcityName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div align="center">
        <asp:DataList ID="dtlAcList" runat="server">
            <ItemTemplate>
                <table width="1000px" align="center">
                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="lblAcCode" runat="server" Text='<%#Eval("Ac_Code") %>'></asp:Label>
                        </td>
                        <td style="width: 200px;">
                            <asp:Label ID="lblAcName" runat="server" Text='<%#Eval("Ac_Name_E") %>'></asp:Label>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
    </form>
</body>
</html>
