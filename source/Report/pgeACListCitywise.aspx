<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeACListCitywise.aspx.cs" Inherits="Report_pgeACListCitywise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<fieldset style="border-top:1px dotted rgb(131, 127, 130); border-radius:3px; width:90%; margin-left:30px; float:left;  border-bottom:0px; padding-top:0px; padding-bottom:10px; border-left:0px; border-right:0px; height:7px; ">
<legend style="text-align:center;"><asp:Label ID="label1" runat="server" Text="   Citywise Account List   " Font-Names="verdana" ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
</fieldset>
<asp:HiddenField ID="hdnfClosePopup" runat="server"/>
<asp:HiddenField ID="hdHelpPageCount" runat="server" />
<table width="60%" align="center">
<tr>
<td align="left">
City Code:
</td>
<td align="left">
<asp:TextBox ID="txtcityCode" runat="server" CssClass="txt" Width="90px"></asp:TextBox>
<asp:Label ID="lblcityName" runat="server"></asp:Label>
</td>
</tr>
</table>



</asp:Content>

