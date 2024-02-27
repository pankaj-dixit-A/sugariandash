<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckingUnitility.aspx.cs"
    Inherits="Sugar_CheckingUnitility" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button Text="Check" ID="btnCheck" runat="server" OnClick="btnCheck_Click" Width="83px" /><br />
        <center>
            <asp:Panel runat="server" ID="pnl" Height="400px" Width="600px" ScrollBars="Both">
                <asp:GridView runat="server" ID="grdRecords" AutoGenerateColumns="true" />
            </asp:Panel>
        </center>
    </div>
    </form>
</body>
</html>
