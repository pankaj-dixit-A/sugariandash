<%@ Page Title="Vouchers" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeVouchers.aspx.cs" Inherits="Report_pgeLocalvoucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
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

                var pagecount = document.getElementById("<%=hdnpagecount.ClientID %>").value;
                pagecount = parseInt(pagecount);
                if (pagecount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }


                if (hdnfClosePopupValue == "txtParty") {
                    document.getElementById("<%=txtParty.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblPartyName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtParty.ClientID %>").focus();

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
    <script type="text/javascript">
        function sp(ac_code, Fromdt, Todt, c_item) {
            var tn;
            window.open('rptVouchers.aspx?ac_code=' + ac_code + '&Fromdt=' + Fromdt + '&Todt=' + Todt + '&Category=' + c_item);

        }
    </script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
        float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
        border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Local Voucher's   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:HiddenField ID="hdnfClosePopup" runat="server" />
    <asp:HiddenField ID="hdnpagecount" runat="server" />
    <br />
    <br />
    <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
        Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
        <table align="left" cellspacing="5">
            <tr>
                <td align="left">
                    Select Option:
                </td>
                <td align="left">
                    <asp:DropDownList runat="server" ID="drpSelectOption" CssClass="ddl" Height="24px"
                        Width="120px" AutoPostBack="true" OnSelectedIndexChanged="drpSelectOption_SelectedIndexChanged">
                        <asp:ListItem Text="Date Wise" Value="D"></asp:ListItem>
                        <asp:ListItem Text="Party Wise" Value="P"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="left" style="width: 75%;">
                    <asp:Panel ID="pnlParty" runat="server">
                        Party :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtParty"
                            runat="server" Width="90px" CssClass="txt" AutoPostBack="True" OnTextChanged="txtParty_TextChanged"
                            Height="24px"></asp:TextBox>
                        <asp:Button ID="btnparty" runat="server" Text="..." CssClass="btnHelp" Width="20px"
                            OnClick="btnParty_Click" Height="24px" />
                        <asp:Label ID="lblPartyName" runat="server" CssClass="lblName"></asp:Label>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlDate" runat="server">
                        From Date :&nbsp;<asp:TextBox ID="txtfromDt1" runat="server" Width="90px" CssClass="txt"
                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                            Height="24px"></asp:TextBox>
                        <asp:Image ID="imgcalender2" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="CalendarExtendertxtfromDt1" runat="server" TargetControlID="txtfromDt1"
                            PopupButtonID="imgcalender2" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                        &nbsp;&nbsp; To Date :&nbsp;<asp:TextBox ID="txttoDt1" runat="server" Width="90px"
                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                            CssClass="txt" Height="24px"></asp:TextBox>
                        <asp:Image ID="imgcalender3" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="CalendarExtendertxttoDt1" runat="server" TargetControlID="txttoDt1"
                            PopupButtonID="imgcalender3" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                    </asp:Panel>
                    <%--From Date :&nbsp;<asp:TextBox ID="txtfromDt" runat="server" Width="90px" CssClass="txt"
                            Height="24px"></asp:TextBox>&nbsp;&nbsp;
                        <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="calenderExtendertxtfromDt" runat="server" TargetControlID="txtfromDt"
                            PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>
                        To Date :&nbsp;<asp:TextBox ID="txttoDt" runat="server" Width="90px" CssClass="txt"
                            Height="24px"></asp:TextBox>
                        <asp:Image ID="imgcalender1" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                            Width="25px" Height="15px" />
                        <ajax1:CalendarExtender ID="CalendarExtendertxttoDt" runat="server" TargetControlID="txttoDt"
                            PopupButtonID="imgcalender1" Format="dd/MM/yyyy">
                        </ajax1:CalendarExtender>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    Category:
                </td>
                <td align="left">
                    <asp:DropDownList runat="server" ID="drpCategory" CssClass="ddl" Width="330px" Height="24px"
                        OnSelectedIndexChanged="drpCategory_SelectedIndexChanged">
                        <asp:ListItem Text="Local Voucher Generated From Purchase" Value="LP"></asp:ListItem>
                        <asp:ListItem Text="Local Voucher Generated From Delivery Order" Value="LDO"></asp:ListItem>
                        <asp:ListItem Text="Commission Voucher Generated From Dispatch" Value="CDI"></asp:ListItem>
                        <asp:ListItem Text="Direct Local Voucher" Value="DLV"></asp:ListItem>
                        <asp:ListItem Text="Direct Commission Voucher" Value="DCV"></asp:ListItem>
                        <asp:ListItem Text="All Local Voucher" Value="ALV"></asp:ListItem>
                        <asp:ListItem Text="All Commission Voucher" Value="ACV"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 5%;">
                    <asp:Button ID="btnGetVoucher" Text="Get" runat="server" CssClass="btnHelp" Width="80px"
                        Height="24px" OnClick="btnGetVoucher_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
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
                        <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" PageSize="20"
                            AllowPaging="true" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                            HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                            Style="table-layout: fixed;" OnPageIndexChanging="grdPopup_PageIndexChanging">
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
</asp:Content>
