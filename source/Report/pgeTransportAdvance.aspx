<%@ Page Title="Transport Advance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeTransportAdvance.aspx.cs" Inherits="Report_pgeTransportAdvance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
    <script type="text/javascript">
        function br(Ac_Code, Branch_Code, Fromdt, Todt) {
            window.open('../Report/rptTransportAdvanceOnlyBalNew.aspx?Ac_Code=' + Ac_Code + '&Branch_Code=' + Branch_Code + '&Fromdt=' + Fromdt + '&Todt=' + Todt);
        }

        function dr(Ac_Code, Branch_Code, Fromdt, Todt) {
            window.open('../Report/rptTransportAdvanceBalDetailsNew.aspx?Ac_Code=' + Ac_Code + '&Branch_Code=' + Branch_Code + '&Fromdt=' + Fromdt + '&Todt=' + Todt);
        }
    </script>
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
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                var grid = document.getElementById("<%= grdPopup.ClientID %>");
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";

                var pageCount = document.getElementById("<%= hdHelpPageCount.ClientID %>").value;
                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }

                if (hdnfClosePopupValue == "txtAC_CODE") {
                    document.getElementById("<%=txtAC_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblAc_Name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                }
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
                float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
                border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Transport Advance   " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfconfirm" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <br />
            <table width="700px" align="center" cellspacing="10">
                <tr>
                    <td align="right" style="width: 20%;">
                        <asp:Label runat="server" ID="lblBranch" Text="Select Branch" Font-Bold="true" ForeColor="White"></asp:Label>
                    </td>
                    <td align="left" style="width: 80%;">
                        <asp:DropDownList runat="server" ID="drpBranch" Width="200px" CssClass="ddl" Height="22px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%;">
                        <asp:Label runat="server" ID="lblTrnasport" Text="Transport Account" Font-Bold="true"
                            ForeColor="White"></asp:Label>
                    </td>
                    <td align="left" style="width: 80%;">
                        <asp:TextBox runat="server" ID="txtAC_CODE" CssClass="txt" Width="80px" Style="text-align: right;"
                            AutoPostBack="True" Height="22px" OnTextChanged="txtAC_CODE_TextChanged"></asp:TextBox>
                        <asp:Button ID="btntxtAC_CODE" runat="server" Text="..." Width="25px" OnClick="btntxtAC_CODE_Click"
                            CssClass="btnHelp" Height="22px" />
                        <asp:Label runat="server" ID="lblAc_Name" CssClass="lblName"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 20%;" colspan="2">
                        &nbsp;&nbsp;&nbsp;&nbsp; <b style="color: White;">From:</b>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                        <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="calendertxtFromDate" runat="server" TargetControlID="txtFromDate"
                            PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                        <b style="color: White;">To:</b><asp:TextBox ID="txtToDate" runat="server" CssClass="txt"
                            Width="80px" Height="24px" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                            onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                            Height="15px" />
                        <ajax1:CalendarExtender ID="CalendarExtendertxtToDate" runat="server" TargetControlID="txtToDate"
                            PopupButtonID="Image1" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnOnlyBalance" Text="Only Balance" CssClass="btnHelp"
                            Width="100px" Height="22px" OnClick="btnOnlyBalance_Click" />
                    </td>
                    <td align="left">
                        <asp:Button runat="server" ID="btnDetail" Text="Detail Report" CssClass="btnHelp"
                            Width="100px" Height="22px" OnClick="btnDetail_Click" />
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
                            <asp:TextBox ID="txtSearchText" runat="server" Width="250px" Height="20px" AutoPostBack="true"
                                OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" AllowPaging="true"
                                    PageSize="20" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                    OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
                                    <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                                    <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                                    <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
