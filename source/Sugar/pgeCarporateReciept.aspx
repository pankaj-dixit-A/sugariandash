<%@ Page Title="Carporate Reciept" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeCarporateReciept.aspx.cs" Inherits="Sugar_pgeCarporateReciept" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
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
                if (hdnfClosePopupValue == "txtCashBank") {
                    document.getElementById("<%=txtCashBank.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCashBank.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtACCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtACCode") {
                    document.getElementById("<%=txtACCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblACName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtACCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_date.ClientID %>").focus();
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
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfTransportBalance" runat="server" />
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
                float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
                border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Multiple Reciept   " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table align="left" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Payment For:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpPaymentFor" runat="server" CssClass="ddl" Width="200px"
                                Height="24px" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpPaymentFor_SelectedIndexChanged">
                                <asp:ListItem Text="Carporate / Party" Value="C"></asp:ListItem>
                                <asp:ListItem Text="Transport" Value="T"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Tran Type:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpTrnType" runat="server" CssClass="ddl" Width="200px" Height="24px"
                                TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="drpTrnType_SelectedIndexChanged">
                                <asp:ListItem Text="Bank Receipt" Value="BR"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right" style="width: 10%;">
                            Entry No:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                CssClass="btnHelp" Height="24px" Visible="false" />
                            <ajax1:FilteredTextBoxExtender ID="Filteretxtdoc_no" runat="server" TargetControlID="txtdoc_no"
                                FilterType="Numbers">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp; Date:<asp:TextBox ID="txtdoc_date" runat="Server" CssClass="txt"
                                TabIndex="4" Width="90px" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                                onkeydown="return DateFormat(this,event.keyCode)" Style="text-align: left;" AutoPostBack="True"
                                OnTextChanged="txtdoc_date_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtdoc_date"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                        <td align="right" style="width: 10%;">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Cash/Bank:
                        </td>
                        <td align="left" colspan="5">
                            <asp:TextBox ID="txtCashBank" runat="server" CssClass="txt" Style="text-align: right;"
                                AutoPostBack="True" Width="80px" TabIndex="5" OnTextChanged="txtCashBank_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtCashBank" runat="server" Text="..." OnClick="btntxtCashBank_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblCashBank" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">
                            A/C Code:
                        </td>
                        <td align="left" style="width: 10%;" colspan="5">
                            <asp:TextBox ID="txtACCode" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                TabIndex="6" AutoPostBack="True" OnTextChanged="txtACCode_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtACCode" runat="server" Text="..." OnClick="btntxtACCode_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblACName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td align="right" style="width: 10%;">
                            Unit:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;" colspan="5">
                            <asp:TextBox ID="txtUnit_Code" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtUnit_Code_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtUnitcode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                Width="20px" OnClick="btntxtUnitcode_Click" />
                            <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right">
                            Amount:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtAmount" Height="24px" CssClass="txt" Width="150px"
                                Style="text-align: right;" TabIndex="7" AutoPostBack="true" OnTextChanged="txtAmount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filterAmount" runat="server" TargetControlID="txtAmount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left">
                            <asp:Button runat="server" ID="btnGetvouchers" Text="Get Vouchers" Width="100px"
                                CssClass="btnHelp" Height="24px" OnClick="btnGetvouchers_Click" TabIndex="8" />
                        </td>
                        <td align="center">
                            Balance:&nbsp;<asp:TextBox runat="server" ID="txtBalance" Width="100px" Height="24px"
                                Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 100%; position: relative;">
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="300px" Width="1300px"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" OnRowCommand="grdDetail_RowCommand"
                                CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound" Style="table-layout: fixed;">
                                <Columns>
                                    <asp:BoundField DataField="doc_no" HeaderText="Doc_No" />
                                    <asp:BoundField DataField="Tran_Type" HeaderText="Tran_Type" />
                                    <asp:BoundField DataField="Doc_Date" HeaderText="Date" />
                                    <asp:BoundField DataField="Party_Code" HeaderText="Party_Code" />
                                    <asp:BoundField DataField="PartyName" HeaderText="Party" />
                                    <asp:BoundField DataField="Unit_Code" HeaderText="Unit_Code" />
                                    <asp:BoundField DataField="Unit_Name" HeaderText="Unit_Name" />
                                    <asp:BoundField DataField="Mill_Short" HeaderText="Mill" />
                                    <asp:BoundField DataField="NETQNTL" HeaderText="Quintal" />
                                    <asp:BoundField DataField="Bill_Amount" HeaderText="Vouc.Amount" />
                                    <asp:BoundField DataField="Paid_Amount" HeaderText="Paid_Amount" />
                                    <asp:BoundField DataField="Balance" HeaderText="Balance" />
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtgrdAmount" CssClass="txt" AutoPostBack="true"
                                                Width="80px" Height="24px" Style="text-align: right;" OnTextChanged="txtgrdAmount_TextChanged"></asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender ID="filterAmount1" runat="server" TargetControlID="txtgrdAmount"
                                                FilterType="Custom,Numbers" ValidChars=".">
                                            </ajax1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Adjusted_Amount">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtgrdAdjustedAmount" CssClass="txt" AutoPostBack="true"
                                                Width="80px" Height="24px" Style="text-align: right;" OnTextChanged="txtgrdAdjustedAmount_TextChanged"> </asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender ID="filterAmount2" runat="server" TargetControlID="txtgrdAdjustedAmount"
                                                FilterType="Custom,Numbers" ValidChars=".">
                                            </ajax1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Narration">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtNarration" CssClass="txt" AutoPostBack="true"
                                                Width="80px" Height="24px" Style="text-align: right;" OnTextChanged="txtNarration_TextChanged"> </asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="30px" Wrap="false" ForeColor="Black" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <table width="70%" align="left">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTotal" runat="server" CssClass="lblName" Font-Bold="true" Text="Total:"></asp:Label>&nbsp;<asp:TextBox
                            ID="txtTotal" runat="server" ReadOnly="true" CssClass="txt" Width="100px" Style="text-align: right;"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table width="80%" align="left">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            TabIndex="5" ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="6" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                    </td>
                    <td align="center">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlPopup" runat="server" Width="80%" align="center" ScrollBars="None"
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
                                <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" EmptyDataText="No Records Found"
                                    HeaderStyle-BackColor="#6D8980" HeaderStyle-ForeColor="White" AllowPaging="true"
                                    PageSize="20" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                                    OnPageIndexChanging="grdPopup_PageIndexChanging" Style="table-layout: fixed;"
                                    OnSelectedIndexChanged="grdPopup_SelectedIndexChanged">
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
