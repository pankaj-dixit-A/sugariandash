<%@ Page Title="Pending Sauda Amount" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgePendingSaudaAmount.aspx.cs" Inherits="Report_pgePendingSaudaAmount" %>

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
                if (hdnfClosePopupValue == "txtPartyCode") {
                    document.getElementById("<%= txtPartyCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblPartyCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
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
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Pending Sauda Amount   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:HiddenField ID="hdnfClosePopup" runat="server" />
    <asp:HiddenField ID="hdHelpPageCount" runat="server" />
    <table width="60%" align="center" cellpadding="5" cellspacing="5">
        <tr>
            <td align="left" style="width: 40%;">
                Mill Code: &nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtAcCode" runat="server" Width="80px" CssClass="txt" AutoPostBack="True"
                    OnTextChanged="txtAcCode_TextChanged" Height="24px"></asp:TextBox>
                <asp:Button ID="btnAcCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnAcCode_Click"
                    Height="24px" Width="20px" />
                <asp:Label ID="lblAcCodeName" runat="server" CssClass="lblName"></asp:Label>
                <asp:RequiredFieldValidator ID="rfvtxtAcCode" runat="server" ControlToValidate="txtAcCode"
                    CssClass="validator" Display="Dynamic" Enabled="false" ErrorMessage="Required"
                    SetFocusOnError="true" Text="Required" ValidationGroup="add"> </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 80%;">
                Party Code: &nbsp;&nbsp;<asp:TextBox ID="txtPartyCode" runat="server" Width="80px"
                    CssClass="txt" AutoPostBack="True" OnTextChanged="txtPartyCode_TextChanged" Height="24px"></asp:TextBox>
                <asp:Button ID="btnPartyCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnPartyCode_Click"
                    Height="24px" Width="20px" />
                <asp:Label ID="lblPartyCode" runat="server" CssClass="lblName"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAcCode"
                    CssClass="validator" Display="Dynamic" Enabled="false" ErrorMessage="Required"
                    SetFocusOnError="true" Text="Required" ValidationGroup="add"> </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="5">
                From Date: &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtFromDt" runat="server" Width="80px"
                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                    CssClass="txt" Height="24px"></asp:TextBox>
                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                    Width="25px" Height="15px" />
                <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                </ajax1:CalendarExtender>
                To Date:
                <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                    Height="15px" />
                <ajax1:CalendarExtender ID="CalendarExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                    PopupButtonID="Image1" Format="dd/MM/yyyy">
                </ajax1:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <asp:Button runat="server" ID="btnPendingReport" Text="Pending Report" CssClass="btnHelp"
                    OnClick="btnPendingReport_Click" Height="25px" Width="164px" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnDetailReport" Text="Detail Report" CssClass="btnHelp"
                    OnClick="btnDetailReport_Click" Height="25px" Width="149px" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Label runat="server" ID="lblPartyName" Font-Bold="true" Height="12px"></asp:Label>
    <br />
    <asp:Panel runat="server" ID="pnlReport" BorderColor="Black" ScrollBars="Both" Height="400px"
        BackColor="White" BorderStyle="Ridge" Width="90%">
        <table width="100%" align="center">
            <tr>
                <td>
                    <asp:GridView runat="server" ID="grdReport" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                        Font-Bold="true" ShowFooter="true" PageSize="12" AllowPaging="false" HeaderStyle-BackColor="#6D8980"
                        Width="100%">
                        <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                        <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                        <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                        <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                        <FooterStyle HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                        <Columns>
                            <asp:BoundField HeaderText="Tender_No" DataField="Tender_No" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="30px" />
                            <asp:BoundField HeaderText="Lifting Date" DataField="Lifting_Date" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="80px" />
                            <asp:BoundField HeaderText="Mill" DataField="millname" ItemStyle-Width="140px" />
                            <asp:BoundField HeaderText="Qntl" DataField="Buyer_Quantal" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField HeaderText="Sale Rate" DataField="salerate" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="80px" />
                            <asp:BoundField HeaderText="Commission" DataField="Commission_Rate" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="80px" />
                            <asp:BoundField HeaderText="Amount" DataField="salevalue" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="120px" />
                            <asp:BoundField HeaderText="Recieved" DataField="received" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="120px" />
                            <asp:BoundField HeaderText="Balance" DataField="balance" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="120px" />
                            <asp:BoundField HeaderText="Mobile" DataField="Party_Mobile" />
                            <asp:TemplateField HeaderText="Select">
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="chkAll" Text="Select" OnCheckedChanged="selectAllCheckBoxes"
                                        AutoPostBack="true" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="grdCB" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
                    <asp:TextBox ID="txtSearchText" runat="server" Width="250px" Height="20px" AutoPostBack="true"></asp:TextBox>
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
