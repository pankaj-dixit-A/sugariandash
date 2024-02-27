<%@ Page Title="Self Stock Calculation" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeCalculateSelfStock.aspx.cs" Inherits="Sugar_pgeCalculateSelfStock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function DOPSSB() {
            window.open('../Report/rptDOPSSB.aspx');
        }
    </script>
    <title>Other Utilities</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Other Utility   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="upPnlPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="True" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="100%" align="left" cellspacing="4px" cellpadding="1px">
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCalculateSelfbal" Text="Balance Calculate" CssClass="btnHelp"
                                Width="130px" Height="24px" OnClick="btnCalculateSelfbal_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnPostingPurchaseEntries" Text="Posting Purchase"
                                CssClass="btnHelp" Width="130px" Height="24px" OnClick="btnPostingPurchaseEntries_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnDOPSSB" Text="Check PS & SB" CssClass="btnHelp"
                                Width="130px" Height="24px" OnClientClick="return DOPSSB();" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCreatePSandSB" Text="Generate PS & SB" CssClass="btnHelp"
                                Width="130px" Height="24px" OnClick="btnCreatePSandSB_Click" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="Button1" Text="Backup" CssClass="btnHelp" Width="130px"
                                Visible="false" Height="24px" OnClick="Button1_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnPdsPartyCodePosting" Text="PDS Party POSTING" CssClass="btnHelp"
                                Width="130px" Visible="true" Height="24px" OnClick="btnPdsPartyCodePosting_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnDoCorrection" Text="DO Correction" Visible="false"
                                OnClick="btnDoCorrection_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnGstPurchasePosting" Text="GST PS POSTING" Visible="true"
                                OnClick="btnGstPurchasePosting_Click" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnRoundOFFSB" Text="Round OF SB" Visible="true" OnClick="btnRoundOFFSB_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            From Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                        <td align="left">
                            To Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                Height="15px" />
                            <ajax1:CalendarExtender ID="CalendarExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                                PopupButtonID="Image1" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
