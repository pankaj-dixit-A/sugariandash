<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeSelectYear.aspx.cs" Inherits="Sugar_pgeSelectYear" %>

    <%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" RowStyle-Height="30px" HeaderStyle-Height="40px">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="yearCode" HeaderText="ID" SortExpression="yearCode" />
            <asp:TemplateField HeaderText="Year">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkYear" Text='<%#Eval("year") %>' OnClick="lnkYear_Click"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        <SortedAscendingCellStyle BackColor="#FDF5AC" />
        <SortedAscendingHeaderStyle BackColor="#4D0000" />
        <SortedDescendingCellStyle BackColor="#FCF6C0" />
        <SortedDescendingHeaderStyle BackColor="#820000" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [yearCode], [year] FROM [AccountingYear] WHERE ([Company_Code] = @Company_Code)">
        <SelectParameters>
            <asp:SessionParameter Name="Company_Code" SessionField="Company_Code" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
