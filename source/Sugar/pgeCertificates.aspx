<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeCertificates.aspx.cs" Inherits="Sugar_pgeCertificates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/Grid.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="  Security Certificates  " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnlMain" Width="600px" Height="400px" BorderColor="White"
        BorderWidth="2px">
        <asp:GridView ID="datagrid" runat="server" AutoGenerateColumns="false" RowStyle-CssClass="rows"
            PagerStyle-CssClass="Grid th" HeaderStyle-CssClass="Grid th" PageSize="10" AllowPaging="True"
            OnPageIndexChanging="datagrid_PageIndexChanging" OnRowDataBound="datagrid_RowDataBound"
            GridLines="Both" OnSelectedIndexChanging="datagrid_SelectedIndexChanging">
            <Columns>
                <asp:BoundField DataField="Computer_User" HeaderText="Computer Name" ItemStyle-Width="200px" />
                <asp:BoundField DataField="IPAddress" HeaderText="Unique ID" ItemStyle-Width="300px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Created_Date" HeaderText="Created Date" ItemStyle-Width="100px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="grdCB" Height="5px" Width="5px" />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel runat="server" ID="Panel1">
        <table>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnDelete" Text="Delete Certificate" Width="200px"
                        CssClass="button" OnClick="btnDelete_Click" />
                </td>
            </tr>
            <%--<tr>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" Text="Upload" OnClick="Upload" runat="server" />
                </td>
            </tr>--%>
        </table>
    </asp:Panel>
</asp:Content>
