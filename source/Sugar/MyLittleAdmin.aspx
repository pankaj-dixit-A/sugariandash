<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyLittleAdmin.aspx.cs" Inherits="Sugar_MyLittleAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .serverexplorer
        {
            width: 300px;
            border: 1px solid blue;
            height: 700px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <ajax1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
    </ajax1:ToolkitScriptManager>
    <div class="serverexplorer">
        <p style="background-color: #545FBD; color: White; height: 25px; text-align: left;
            vertical-align: top; margin-top: 0px;">
            Connections
        </p>
        <div id="treeview">
            <asp:UpdatePanel runat="server" ID="updatePnl" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TreeView ID="treeview1" runat="server">
                    </asp:TreeView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
