﻿<%@ Page Title="Group Ledger" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeGroupLedger.aspx.cs" Inherits="Report_pgeGroupLedger" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">

        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;


        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40) {
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            }
            else if (KeyCode == 38) {
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            }

            else if (KeyCode == 13) {

                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;

                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";

                document.getElementById("<%=txtSearchText.ClientID %>").value = "";

                var grid = document.getElementById("<%= grdPopup.ClientID %>");

                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
                var pageCount = document.getElementById("<%= hdHelpPageCount.ClientID %>").value;


                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }
                if (hdnfClosePopupValue == "txtAcCode") {
                    document.getElementById("<%= txtAcCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblAcCodeName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                }
                if (hdnfClosePopupValue == "txtUnitCode") {
                    document.getElementById("<%= txtUnitCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblUnitName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                }

                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }
        }
        function SelectRow(CurrentRow, RowIndex) {
            UpperBound = parseInt('<%= this.grdPopup.Rows.Count %>') - 1;
            LowerBound = 0;

            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)

                if (SelectedRow != null) {
                    SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                    SelectedRow.style.color = SelectedRow.originalForeColor;
                }
            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }
            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }
    </script>
    <script type="text/javascript" language="javascript">
        function sp(accode, fromdt, todt) {
            var tn;
            window.open('rptGroupLedger.aspx?accode=' + accode + '&fromdt=' + fromdt + '&todt=' + todt, '_blank');    //R=Redirected  O=Original
        }
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="  Group GLedger Report   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:HiddenField ID="hdnfClosePopup" runat="server" />
    <asp:HiddenField ID="hdHelpPageCount" runat="server" />
    <table width="60%" align="center" cellspacing="10">
        <tr>
            <td align="left">
                <b>Account Code 1:</b> &nbsp;
                <asp:TextBox ID="txtAcCode" runat="server" Width="80px" CssClass="txt" AutoPostBack="True"
                    OnTextChanged="txtAcCode_TextChanged" Height="24px"></asp:TextBox>
                <asp:Button ID="btnAcCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnAcCode_Click"
                    Height="24px" Width="20px" />
                <asp:Label ID="lblAcCodeName" runat="server" CssClass="lblName"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <b>Accont Code 2:</b> &nbsp;&nbsp;
                <asp:TextBox ID="txtUnitCode" runat="server" Width="80px" CssClass="txt" AutoPostBack="True"
                    Height="24px" OnTextChanged="txtUnitCode_TextChanged"></asp:TextBox>
                <asp:Button ID="btnUnitCode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                    Width="20px" OnClick="btnUnitCode_Click" />
                <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>From Date:</b>
                <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                    Width="25px" Height="15px" />
                <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                </ajax1:CalendarExtender>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <b>To Date:</b>
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
        <tr>
            <td align="center">
                <asp:Button ID="btnGetData" runat="server" Text="Get" CssClass="btnHelp" Width="90px"
                    Height="24px" OnClick="btnGetData_Click" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlPopup" runat="server" Width="70%" align="center" ScrollBars="None"
        BackColor="#FFFFE4" Direction="LeftToRight" Style="z-index: 5000; position: absolute;
        display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px;
        background-position: center; left: 10%; top: 10%;">
        <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
            Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
            ToolTip="Close" />
        <table width="95%">
            <tr>
                <td align="center" style="background-color: #F5B540; width: 100%;">
                    <asp:Label ID="lblPopupHead" runat="server" Font-Size="Medium" Font-Names="verdana"
                        Font-Bold="true" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Search Text:
                    <asp:TextBox ID="txtSearchText" runat="server" Width="250px" Height="20px" AutoPostBack="false"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btnSubmit" OnClick="btnSearch_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <%--<asp:Panel ID="pnlScroll" runat="server" Width="680px" ScrollBars="Both" Direction="LeftToRight" BackColor="#FFFFE4" style="z-index:5000 ;  float:right; max-height:380px; height:380px;">--%>
                    <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                        Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                        <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" EmptyDataText="No Records Found"
                            ViewStateMode="Disabled" PageSize="20" AllowPaging="true" HeaderStyle-BackColor="#6D8980"
                            HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnPageIndexChanging="grdPopup_PageIndexChanging"
                            Style="table-layout: fixed;" OnRowDataBound="grdPopup_RowDataBound">
                            <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                            <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                            <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                            <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                            <PagerSettings Position="TopAndBottom" />
                        </asp:GridView>
                    </asp:Panel>
                    <%--</asp:Panel>--%>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
