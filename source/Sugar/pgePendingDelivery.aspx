<%@ Page Title="Pending Delivery" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgePendingDelivery.aspx.cs" Inherits="Sugar_pgePendingDelivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
                float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
                border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Pending Delivery   " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:Panel Width="100%" runat="server" ID="pnlMain" Font-Names="verdana" Font-Bold="true"
                ForeColor="Black" Font-Size="Small" Style="margin-left: 30px; margin-top: 0px;
                z-index: 100;">
                <table runat="server" align="left" cellspacing="5">
                    <tr>
                        <td style="width: 50%;" align="right">
                            <asp:Label ID="lblCategory" runat="server" Text="Select Category:" Font-Bold="true"
                                ForeColor="Yellow"></asp:Label>
                        </td>
                        <td style="width: 50%;" align="left">
                            <asp:DropDownList runat="server" ID="drpCategory" Width="300px">
                                <asp:ListItem Text="Pending Delivery of Commission" Value="C"></asp:ListItem>
                                <asp:ListItem Text="Pending Delivery of Naka Delivery" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btnGetReport" Text="Get Data" CssClass="btnHelp" Width="80px"
                                Height="24px" OnClick="btnGetReport_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlGrid" runat="server" align="left" BorderColor="Blue" BorderWidth="1px"
                BackColor="White" Height="300px" ScrollBars="Both" Style="text-align: left" Width="1100px"
                DefaultButton="btnUpdate">
                <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" CellPadding="6"
                    Font-Bold="true" ForeColor="Black" GridLines="Both" HeaderStyle-BackColor="#397CBB"
                    HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" RowStyle-Height="30px"
                    EmptyDataText="No Records Found" AlternatingRowStyle-BackColor="#F8F8F8" Style="table-layout: fixed;"
                    OnRowDataBound="grdDetail_RowDataBound">
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="Mill" HeaderText="Mill" />
                        <asp:BoundField DataField="L_Date" HeaderText="Lifting Date" />
                        <asp:BoundField DataField="Party" HeaderText="Buyer Party" />
                        <asp:BoundField DataField="Qntl" HeaderText="Qntl" />
                        <asp:BoundField DataField="Sale_Rate" HeaderText="Sale Rate" />
                        <asp:BoundField DataField="balance" HeaderText="Balance" />
                        <asp:TemplateField HeaderText="Mobile">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtMobile" Style="border: none;" Text='<%#Eval("mobile") %>'
                                    MaxLength="10" Width="90px" Height="25px">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SMS">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="grdCB" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button runat="server" ID="btnUpdate" Style="display: none;" OnClick="btnUpdate_Click" />
            </asp:Panel>
            <br />
            <asp:Panel ID="pnlMsg" runat="server" align="center" Width="500px">
                <asp:Button ID="btnSendSms" Text="SEND" runat="server" CssClass="btnHelp" Width="100px"
                    Height="24px" OnClick="btnSendSms_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
