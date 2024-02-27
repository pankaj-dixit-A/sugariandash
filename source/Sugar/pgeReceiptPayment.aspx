<%@ Page Title="Reciept Payment" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeReceiptPayment.aspx.cs" Inherits="Sugar_pgeReceiptPayment" %>

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

                if (hdnfClosePopupValue == "txtVoucherNo") {
                    document.getElementById("<%=txtVoucherNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtvoucherType.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    var drp = document.getElementById('<%=drpFilter.ClientID %>');
                    var val = drp.options[drp.selectedIndex].value;
                    document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    if (val == "T") {
                    //                        document.getElementById("<%= hdnfTransportBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    //                        document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    }
                    //                    if (val == "S" || val == "V") {
                    //                        document.getElementById("<%= hdnfTransportBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[11].innerText;
                    //                        document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                    //                    }

                }
                if (hdnfClosePopupValue == "txtCashBank") {
                    document.getElementById("<%=txtCashBank.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCashBank.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    //document.getElementById("<%=btnOpenDetailsPopup.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtACCode") {
                    document.getElementById("<%=txtACCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblACName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtUnit_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUnit_Code") {
                    document.getElementById("<%=txtUnit_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUnitName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtnarration") {
                    document.getElementById("<%=txtnarration.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;

                    //document.getElementById("<%=btnAdddetails.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    //document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_date.ClientID %>").focus();
                }

                //            if (hdnfClosePopupValue == "txtVoucherNo") {

                //             //   if (drpFilter.value == "S") {

                //                    document.getElementById("<%=txtVoucherNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                //                    document.getElementById("<%=txtvoucherType.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                //                    document.getElementById("<%=txtVoucherNo.ClientID %>").focus();
                //              //  }
                //            } 
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
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Receipt/Payment   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfTransportBalance" runat="server" />
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
                        <td align="left">
                            Transaction Type:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpTrnType" runat="server" CssClass="ddl" Width="200px" Height="24px"
                                TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="drpTrnType_SelectedIndexChanged">
                                <asp:ListItem Text="Cash Payment" Value="CP"></asp:ListItem>
                                <asp:ListItem Text="Cash Receipt" Value="CR"></asp:ListItem>
                                <asp:ListItem Text="Bank Payment" Value="BP"></asp:ListItem>
                                <asp:ListItem Text="Bank Receipt" Value="BR"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left" style="width: 10%;">
                            Entry No:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                CssClass="btnHelp" Height="24px" />
                            <ajax1:FilteredTextBoxExtender ID="Filteretxtdoc_no" runat="server" TargetControlID="txtdoc_no"
                                FilterType="Numbers">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="right" style="width: 10%;">
                            Date:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtdoc_date" runat="Server" CssClass="txt" TabIndex="2" Width="90px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtdoc_date_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtdoc_date"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Cash/Bank:
                        </td>
                        <td align="left" colspan="5">
                            <asp:TextBox ID="txtCashBank" runat="server" CssClass="txt" Style="text-align: right;"
                                AutoPostBack="True" TabIndex="3" OnTextChanged="txtCashBank_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtCashBank" runat="server" Text="..." OnClick="btntxtCashBank_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblCashBank" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                Width="80px" Height="24px" OnClick="btnOpenDetailsPopup_Click" TabIndex="4" />
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
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="100%"
                                OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound"
                                Style="table-layout: fixed;">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRecord" Text="Edit"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRecord" Text="Delete"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
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
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="6" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                    </td>
                    <td align="center">
                        <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                            Height="24px" OnClick="btnFirst_Click" Width="90px" />
                        <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                            Height="24px" OnClick="btnPrevious_Click" Width="90px" />
                        <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                            Height="24px" OnClick="btnNext_Click" Width="90px" />
                        <asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="First" CssClass="btnHelp"
                            Height="24px" OnClick="btnLast_Click" Width="90px" />
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
            <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="955px"
                BorderColor="Teal" BorderWidth="1px" Height="330px" BorderStyle="Solid" Style="z-index: 4999;
                left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                <table width="90%" align="center" cellspacing="5">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;
                            height: 25px; vertical-align: middle;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Voucher Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNo" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblID" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            A/C Code:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtACCode" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                TabIndex="5" AutoPostBack="True" OnTextChanged="txtACCode_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtACCode" runat="server" Text="..." OnClick="btntxtACCode_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblACName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Unit:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtUnit_Code" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtUnit_Code_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtUnitcode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                Width="20px" OnClick="btntxtUnitcode_Click" />
                            <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Select:
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpFilter" runat="server" CssClass="ddl" Width="280px" Height="25px"
                                Visible="true" AutoPostBack="true" TabIndex="7" OnSelectedIndexChanged="drpFilter_SelectedIndexChanged"
                                OnTextChanged="drpFilter_SelectedIndexChanged">
                                <asp:ListItem Text="Againt Loading Voucher" Value="V"></asp:ListItem>
                                <asp:ListItem Text="Againt Sauda" Value="S"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            <asp:Label Text="Voucher Number:" runat="server" ID="lblHead"></asp:Label>
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtVoucherNo" Height="24px" runat="Server" CssClass="txt" Width="80px"
                                Style="text-align: right;" TabIndex="8" AutoPostBack="true" OnTextChanged="txtVoucherNo_TextChanged1"></asp:TextBox>
                            <asp:Button ID="btntxtVoucherNo" runat="server" Text="..." OnClick="btntxtVoucherNo_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:TextBox ID="txtvoucherType" Enabled="false" runat="Server" CssClass="txt" Width="20px"
                                Style="text-align: right;" AutoPostBack="False" Height="24px"></asp:TextBox>
                            <asp:Label ID="lblVoucherBy" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtamount" runat="Server" TabIndex="9" Height="24px" CssClass="txt"
                                Width="80px" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtamount_TextChanged"></asp:TextBox><asp:Label
                                    runat="server" ID="lblErrorAdvance" Text="" ForeColor="Red"></asp:Label>
                            <ajax1:FilteredTextBoxExtender ID="filterAmount" runat="server" TargetControlID="txtamount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Adjusted Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtadAmount" runat="Server" CssClass="txt" Height="24px" Width="80px"
                                Style="text-align: right;" AutoPostBack="false" TabIndex="10" OnTextChanged="txtadAmount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FiltertxtadAmount" runat="server" TargetControlID="txtamount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Narration:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtnarration" runat="Server" CssClass="txt" Width="480px" Height="50px"
                                TextMode="MultiLine" Style="text-align: left;" AutoPostBack="True" TabIndex="11"
                                OnTextChanged="txtnarration_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtnarration" runat="server" Text="..." OnClick="btntxtnarration_Click"
                                CssClass="btnHelp" Width="20px" Height="24px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnSubmit" Width="80px"
                                Height="24px" OnClick="btnAdddetails_Click" TabIndex="12" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnSubmit"
                                Height="24px" Width="80px" OnClick="btnClosedetails_Click" TabIndex="13" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%--<ajax1:ModalPopupExtender runat="server" ID="popup1" CancelControlID="btnClosedetails"
                PopupControlID="pnlPopupDetails" TargetControlID="btnOpenDetailsPopup">
            </ajax1:ModalPopupExtender>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
