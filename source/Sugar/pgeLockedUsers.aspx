<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeLockedUsers.aspx.cs" Inherits="Sugar_pgeLockedUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/Grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Validate() {
            var gridview = document.getElementById("<%=datagrid.ClientID %>");
            var Checkbox = gridview.getElementsByTagName("input");
            for (var i = 0; i < Checkbox.length; i++) {
                if (Checkbox[i].type == "checkbox" && che[i].checked) {
                    IsValid = true;
                }
            }
            
            alert('Please Select Atleast One User');
            return;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="  Locked Users  " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <br />
    <br />
    <div>
        <asp:Panel runat="server" ID="pnlMain" Width="600px" Height="400px" BorderColor="White"
            BorderWidth="2px">
            <asp:GridView ID="datagrid" runat="server" AutoGenerateColumns="false" RowStyle-CssClass="rows"
                PagerStyle-CssClass="Grid th" HeaderStyle-CssClass="Grid th" PageSize="10" AllowPaging="True"
                OnPageIndexChanging="datagrid_PageIndexChanging" OnRowDataBound="datagrid_RowDataBound"
                GridLines="Both" OnSelectedIndexChanging="datagrid_SelectedIndexChanging">
                <Columns>
                    <asp:BoundField DataField="User_Name" HeaderText="User Name" />
                    <asp:TemplateField HeaderText="Unlock">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="grdChk" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <table width="50%" align="center">
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnUnlock" CssClass="button" Text="Unlock User" OnClientClick="return Validate();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
