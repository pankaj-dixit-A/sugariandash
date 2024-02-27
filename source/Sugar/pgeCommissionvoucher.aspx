<%@ Page Title="Commission Voucher" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeCommissionvoucher.aspx.cs" Inherits="pgeCommissionvoucher" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/DateValidation.js">
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

                if (hdnfClosePopupValue == "txtParty") {
                    document.getElementById("<%=txtTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[2].innerText;
                    document.getElementById("<%=txtParty.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblPartyName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtParty.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGetpass") {
                    document.getElementById("<%=txtGetpass.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblGetpass.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtMILL_CODE") {
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMill_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtFrom.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[2].innerText;
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGRADE") {
                    document.getElementById("<%=txtGRADE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtMILL_RATE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDO") {
                    document.getElementById("<%=txtDO.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblDOName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtDOC_DATE.ClientID %>").focus();
                    //document.getElementById("<%=txtFrom.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBANK_CODE") {
                    document.getElementById("<%=txtBANK_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBank_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtNARRATION.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDOC_NO") {
                    document.getElementById("<%=txtDoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtSuffix.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtDOC_DATE.ClientID %>").focus();
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
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Commission Voucher   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfconfirm" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="100%" cellspacing="8">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Voucher No:
                            </td>
                            <td align="left" style="width: 10%;" colspan="7">
                                <asp:TextBox ID="txtDoc_no" runat="Server" CssClass="txt" TabIndex="1" Width="60px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDoc_no_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtDOC_NO" runat="server" Text="..." Width="80px" OnClick="btntxtDOC_NO_Click"
                                    CssClass="btnHelp" Height="24px" />
                                &nbsp; Suffix
                                <asp:TextBox ID="txtSuffix" runat="Server" CssClass="txt" TabIndex="2" Width="20px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtSuffix_TextChanged"
                                    Height="24px"></asp:TextBox>
                                &nbsp;&nbsp; D.O.
                                <asp:TextBox ID="txtDO" runat="Server" CssClass="txt" TabIndex="3" Width="60px" Style="text-align: right;"
                                    AutoPostBack="True" OnTextChanged="txtDO_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btnDO" runat="server" Text="..." OnClick="btnDO_Click" CssClass="btnHelp"
                                    Height="24px" Width="20px" />
                                <asp:Label ID="lblDOName" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp;&nbsp; Date:
                                <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="4" Width="100px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtDOC_DATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                                <%--</td>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Party:
                            </td>
                            <td align="left" style="width: 10%;" colspan="7">
                                <asp:TextBox ID="txtParty" runat="Server" CssClass="txt" TabIndex="5" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtParty_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btnParty" runat="server" Text="..." OnClick="btnParty_Click" CssClass="btnHelp"
                                    Height="24px" Width="20px" />
                                <asp:Label ID="lblPartyName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Getpass:
                            </td>
                            <td align="left" style="width: 10%;" colspan="6">
                                <asp:TextBox ID="txtGetpass" runat="Server" CssClass="txt" TabIndex="6" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtGetpass_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGetpass" runat="server" Text="..." OnClick="btntxtGetpass_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblGetpass" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Mill Name:
                            </td>
                            <td align="left" style="width: 10%;" colspan="7">
                                <asp:TextBox ID="txtMILL_CODE" runat="Server" CssClass="txt" TabIndex="7" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMILL_CODE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtMILL_CODE" runat="server" Text="..." OnClick="btntxtMILL_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblMill_name" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                From:
                            </td>
                            <td align="left" colspan="7">
                                <asp:TextBox ID="txtFrom" runat="Server" CssClass="txt" Width="200px" TabIndex="8"
                                    Height="24px"></asp:TextBox>
                                &nbsp;&nbsp; To:
                                <asp:TextBox ID="txtTo" runat="Server" CssClass="txt" Width="200px" AutoPostBack="false"
                                    TabIndex="9" Height="24px"></asp:TextBox>
                                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;Lorry
                                No.:
                                <asp:TextBox ID="txtLorryNo" runat="Server" CssClass="txt" Width="120px" AutoPostBack="True"
                                    OnTextChanged="txtLorryNo_TextChanged" TabIndex="10" Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Quintal:
                            </td>
                            <td align="left" colspan="7">
                                <asp:TextBox ID="txtQNTL" runat="Server" CssClass="txt" TabIndex="11" Width="120px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtQNTL_TextChanged"
                                    Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                        runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtQNTL">
                                    </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp; &nbsp;&nbsp; Packing:
                                <asp:TextBox ID="txtPACKING" runat="Server" CssClass="txt" TabIndex="12" Width="120px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPACKING_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="filtertxtPACKING" TargetControlID="txtPACKING"
                                    FilterType="Numbers" runat="server">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;Bags:
                                <asp:TextBox ID="txtBAGS" runat="Server" CssClass="txt" TabIndex="13" Width="120px"
                                    ReadOnly="true" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBAGS_TextChanged"
                                    Height="24px"></asp:TextBox>
                                &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Grade:&nbsp;
                                <asp:TextBox ID="txtGRADE" runat="Server" CssClass="txt" TabIndex="14" Width="175px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtGRADE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGRADE" runat="server" Text="..." OnClick="btntxtGRADE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                Mill Rate:
                            </td>
                            <td align="left" colspan="7">
                                <asp:TextBox ID="txtMILL_RATE" runat="Server" CssClass="txt" TabIndex="15" Width="120px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMILL_RATE_TextChanged"
                                    Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                                        runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtMILL_RATE">
                                    </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp; Sale Rate:
                                <asp:TextBox ID="txtSALE_RATE" runat="Server" CssClass="txt" TabIndex="16" Width="120px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtSALE_RATE_TextChanged"
                                    Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                                        runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtSALE_RATE">
                                    </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;Purchase Rate:
                                <asp:TextBox ID="txtPURCHASE_RATE" runat="Server" CssClass="txt" TabIndex="17" Width="120px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPURCHASE_RATE_TextChanged"
                                    Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtPURCHASE_RATE">
                                    </ajax1:FilteredTextBoxExtender>
                                Mill Amount:&nbsp;<asp:TextBox ID="txtMILL_AMOUNT" runat="Server" CssClass="txt"
                                    TabIndex="18" Width="120px" ReadOnly="true" Style="text-align: right;" AutoPostBack="True"
                                    Height="24px" OnTextChanged="txtMILL_AMOUNT_TextChanged1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Comm'n Rate:
                            </td>
                            <td align="left" style="width: 10%;" colspan="7">
                                <asp:TextBox ID="txtCOMMISSION_RATE" runat="Server" CssClass="txt" TabIndex="19"
                                    Width="120px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCOMMISSION_RATE_TextChanged"
                                    Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                                        runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtCOMMISSION_RATE">
                                    </ajax1:FilteredTextBoxExtender>
                                Commission:
                                <asp:TextBox ID="txtCOMMISSION" runat="Server" CssClass="txt" Width="120px" Height="24px"
                                    TabIndex="20" ReadOnly="true" Style="text-align: right;" AutoPostBack="True"
                                    OnTextChanged="txtCOMMISSION_TextChanged1"></asp:TextBox>Loading Charges:
                                <asp:TextBox ID="txtLOADING_CHARGE" runat="Server" CssClass="txt" TabIndex="21" Width="120px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtLOADING_CHARGE_TextChanged"
                                    Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                        runat="server" FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtLOADING_CHARGE">
                                    </ajax1:FilteredTextBoxExtender>
                                Diff Amount:
                                <asp:TextBox ID="txtDIFF_AMOUNT" runat="Server" CssClass="txt" Width="120px" Style="text-align: right;"
                                    AutoPostBack="True" OnTextChanged="txtDIFF_AMOUNT_TextChanged" ReadOnly="True"
                                    TabIndex="22" Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Voucher Amount:
                            </td>
                            <td align="left" style="width: 10%;" colspan="7">
                                <asp:TextBox ID="txtVOUCHER_AMOUNT" runat="Server" CssClass="txt" TabIndex="23" ForeColor="Navy"
                                    Width="120px" Style="text-align: right; font-weight: bolder" AutoPostBack="True"
                                    ReadOnly="true" OnTextChanged="txtVOUCHER_AMOUNT_TextChanged" Height="24px"></asp:TextBox>&nbsp;&nbsp;Diff:<asp:Label
                                        ID="lblDiff" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Due Days:
                            </td>
                            <td align="left" style="width: 60%;">
                                <asp:TextBox ID="txtDueDays" runat="Server" CssClass="txt" TabIndex="24" Width="120px"
                                    Style="text-align: right;" AutoPostBack="false" Height="24px" OnTextChanged="txtDueDays_TextChanged"></asp:TextBox><ajax1:FilteredTextBoxExtender
                                        runat="server" ID="FilteredTextBoxExtender12" FilterType="Numbers" TargetControlID="txtDueDays">
                                    </ajax1:FilteredTextBoxExtender>
                                Rawangi Date:
                                <asp:TextBox ID="txtRAWANGI_DATE" runat="Server" CssClass="txt" TabIndex="25" Width="80px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtRAWANGI_DATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgRAWANGIDt" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="CalendarExtendertxtRAWANGI_DATE" runat="server" TargetControlID="txtRAWANGI_DATE"
                                    PopupButtonID="imgRAWANGIDt" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                            </td>
                            <td align="left" style="width: 10%;">
                            </td>
                            <td align="left" style="width: 10%;">
                            </td>
                            <td colspan="2" align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                NARRATION 1:
                            </td>
                            <td align="left" style="width: 10%;" colspan="8">
                                <asp:TextBox ID="txtNARRATION1" runat="Server" CssClass="txt" TabIndex="26" Width="200px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION1_TextChanged"
                                    Height="24px"></asp:TextBox>&nbsp; 2:<asp:TextBox ID="txtNARRATION2" runat="Server"
                                        CssClass="txt" TabIndex="27" Width="200px" Style="text-align: left;" AutoPostBack="True"
                                        OnTextChanged="txtNARRATION2_TextChanged" Height="24px"></asp:TextBox>&nbsp;
                                3:<asp:TextBox ID="txtNARRATION3" runat="Server" CssClass="txt" TabIndex="28" Width="200px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION3_TextChanged"
                                    Height="24px"></asp:TextBox>&nbsp; 4:<asp:TextBox ID="txtNARRATION4" runat="Server"
                                        CssClass="txt" TabIndex="29" Width="200px" Style="text-align: left;" AutoPostBack="True"
                                        OnTextChanged="txtNARRATION4_TextChanged" Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                    Width="80px" Height="24px" OnClick="btnOpenDetailsPopup_Click" TabIndex="30" />
                            </td>
                        </tr>
                </table>
            </asp:Panel>
            <div style="width: 100%; position: relative;">
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="150px" Width="1000px"
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
                        <asp:Label ID="lblGridTotalN" runat="server" Text="Difference:" CssClass="lblName"></asp:Label>&nbsp;
                        <asp:Label ID="lblGridDiff" runat="server" CssClass="lblName"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <center>
                <table width="80%" align="left">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                                TabIndex="38" ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                                TabIndex="39" ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                                ValidationGroup="add" OnClick="btnDelete_Click" Height="24px" TabIndex="40" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                                ValidationGroup="save" OnClick="btnCancel_Click" Height="24px" TabIndex="41" />
                        </td>
                        <td align="center">
                            <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnFirst_Click" Width="90px" Height="24px" />
                            <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnPrevious_Click" Width="90px" Height="24px" />
                            <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnNext_Click" Width="90px" Height="24px" />
                            <asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnLast_Click" Width="90px" Height="24px" />
                        </td>
                    </tr>
                </table>
            </center>
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
            <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="800px"
                BorderColor="Teal" BorderWidth="1px" Height="300px" BorderStyle="Solid" Style="z-index: 4999;
                left: 15%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                <table width="80%" align="center" cellpadding="4px" cellspacing="4px">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;
                            height: 25px;">
                            <asp:Label ID="lblVoucherDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Voucher Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ID:&nbsp;<asp:Label ID="lblID" runat="server"></asp:Label>
                        </td>
                        <td>
                            NO:&nbsp;<asp:Label ID="lblNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            BANK CODE:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBANK_CODE" runat="Server" CssClass="txt" TabIndex="31" Width="60px"
                                Style="text-align: right;" Height="24px" AutoPostBack="True" OnTextChanged="txtBANK_CODE_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtBANK_CODE" runat="server" Text="..." OnClick="btntxtBANK_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblBank_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            DD/CHQ/RTGS No:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtNARRATION" Height="24px" runat="Server" CssClass="txt" TabIndex="32"
                                Width="200px" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBANK_AMOUNT" Height="24px" runat="Server" CssClass="txt" TabIndex="33"
                                Width="200px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBANK_AMOUNT_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Reconsilation Date:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtReConsilationDt" Height="24px" runat="Server" CssClass="txt"
                                TabIndex="34" Width="80px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtReConsilationDt_TextChanged"></asp:TextBox>
                            <asp:Image ID="imgReConDt" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" TabIndex="4" />
                            <ajax1:CalendarExtender ID="CalendarExtendertxtReConsilationDt" runat="server" TargetControlID="txtReConsilationDt"
                                PopupButtonID="imgReConDt" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnSubmit" Width="80px"
                                Height="25px" OnClick="btnAdddetails_Click" TabIndex="35" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnSubmit"
                                Width="80px" Height="25px" OnClick="btnClosedetails_Click" TabIndex="36" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
