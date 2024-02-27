<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeLocalvoucherForGST.aspx.cs" Inherits="Sugar_pgeLocalvoucherForGST" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
    <script type="text/javascript">
        function p(VNO, type) {
            window.open('../Report/rptVoucherForGST.aspx?VNO=' + VNO + '&type=' + type);
        }
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

                if (hdnfClosePopupValue == "txtAC_CODE") {
                    document.getElementById("<%=txtAC_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblAc_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtUnit_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUnit_Code") {
                    document.getElementById("<%=txtUnit_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUnitName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBroker_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBroker_CODE") {
                    document.getElementById("<%=txtBroker_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBroker_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtQNTL.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtMILL_CODE") {
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMill_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMILL_RATE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtSUFFIX.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtGRADE") {
                    document.getElementById("<%=txtGRADE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNarration1") {
                    document.getElementById("<%=txtNarration1.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNarration2.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNarration2") {
                    document.getElementById("<%=txtNarration2.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNarration3.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNarration3") {
                    document.getElementById("<%=txtNarration3.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNarration4.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNarration2") {
                    document.getElementById("<%=txtNarration4.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=btnSave.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDONO") {
                    document.getElementById("<%=txtDONO.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
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
            <asp:Label ID="label1" runat="server" Text="   Local Voucher   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table style="width: 80%;" align="left" cellpadding="2" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Change No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                TabIndex="0" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                TargetControlID="txtEditDoc_No">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Note No.
                        </td>
                        <td colspan="6" align="left" style="width: 80%;">
                            <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtDOC_NO" runat="server" Text="..." Width="80px" OnClick="btntxtDOC_NO_Click"
                                CssClass="btnHelp" Height="24px" />
                            &nbsp; Suffix: &nbsp;
                            <asp:TextBox ID="txtSUFFIX" runat="Server" CssClass="txt" TabIndex="2" Width="20px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtSUFFIX_TextChanged"
                                Height="24px"></asp:TextBox>
                            &nbsp; D.O No.:
                            <asp:TextBox ID="txtDONO" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDONO_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtDONO" runat="server" Text="..." OnClick="btntxtDONO_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />&nbsp; Tender No(R):<asp:Label runat="server"
                                    ID="lblTenderNo" ForeColor="Black"></asp:Label>
                        </td>
                        <%--<td align="left">
                        </td>--%>
                    </tr>
                    <tr>
                        <td align="left">
                            Date:
                        </td>
                        <td align="left" colspan="2">
                            <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="4" Width="90px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDOC_DATE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Party:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtAC_CODE" runat="Server" CssClass="txt" TabIndex="5" Width="90px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtAC_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtAC_CODE" runat="server" Text="..." OnClick="btntxtAC_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblAc_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Unit:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtUnit_Code" runat="Server" CssClass="txt" TabIndex="6" Width="90px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtUnit_Code_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtUnitcode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                Width="20px" OnClick="btntxtUnitcode_Click" />
                            <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Broker:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtBroker_CODE" runat="Server" CssClass="txt" TabIndex="7" Width="90px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBroker_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtBroker_CODE" runat="server" Text="..." OnClick="btntxtBroker_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblBroker_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Quantal:
                        </td>
                        <td align="left" colspan="6" style="width: 10%;">
                            <asp:TextBox ID="txtQNTL" runat="Server" CssClass="txt" TabIndex="8" Width="115px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtQNTL_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBags"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtQNTL">
                                </ajax1:FilteredTextBoxExtender>
                            &nbsp; Packing: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPACKING" runat="Server" CssClass="txt" TabIndex="9" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPACKING_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender1"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtPACKING">
                                </ajax1:FilteredTextBoxExtender>
                            &nbsp; Bags: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtBAGS" runat="Server" CssClass="txt" TabIndex="10" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBAGS_TextChanged"
                                ReadOnly="true" Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server"
                                    ID="FilteredTextBoxExtender2" FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtBAGS">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Grade:
                        </td>
                        <td align="left" colspan="2">
                            <asp:TextBox ID="txtGRADE" runat="Server" CssClass="txt" TabIndex="11" Width="90px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtGRADE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGRADE" runat="server" Text="..." OnClick="btntxtGRADE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <td colspan="3">
                            Transport:
                            <asp:TextBox ID="txtTRANSPORT_CODE" runat="Server" CssClass="txt" TabIndex="22" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtTRANSPORT_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtTRANSPORT_CODE" runat="server" Text="..." OnClick="btntxtTRANSPORT_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LBLTRANSPORT_NAME" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Mill:
                        </td>
                        <td align="left" colspan="3" style="width: 10%;">
                            <asp:TextBox ID="txtMILL_CODE" runat="Server" CssClass="txt" TabIndex="12" Width="90px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMILL_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtMILL_CODE" runat="server" Text="..." OnClick="btntxtMILL_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblMill_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Mill Rate:
                        </td>
                        <td align="left" colspan="6" style="width: 10%;">
                            <asp:TextBox ID="txtMILL_RATE" runat="Server" CssClass="txt" TabIndex="13" Width="110px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMILL_RATE_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender3"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtMILL_RATE">
                                </ajax1:FilteredTextBoxExtender>
                            &nbsp; Sale Rate:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtSALE_RATE" runat="Server" CssClass="txt" TabIndex="14" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtSALE_RATE_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender11"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtSALE_RATE">
                                </ajax1:FilteredTextBoxExtender>
                            &nbsp; Purc Rate: &nbsp;
                            <asp:TextBox ID="txtPURCHASE_RATE" runat="Server" CssClass="txt" TabIndex="15" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPURCHASE_RATE_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender4"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtPURCHASE_RATE">
                                </ajax1:FilteredTextBoxExtender>
                            &nbsp; Diff.: &nbsp;
                            <asp:Label ID="lblDiff" runat="server" CssClass="lblName"></asp:Label>
                            &nbsp; GST Rate Code
                            <asp:TextBox ID="txtGSTRateCode" runat="Server" CssClass="txt" TabIndex="11" Width="80px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtGSTRateCode_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGSTRateCode" runat="server" Text="..." OnClick="btntxtGSTRateCode_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblGSTRateName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            R.DIFF.Tender:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtRDiffTender" runat="Server" CssClass="txt" TabIndex="16" Width="110px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtRDiffTender_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender5"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtRDiffTender">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration1:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNarration1" runat="Server" CssClass="txt" TabIndex="24" Width="320px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNarration1" runat="server" Text="..." OnClick="btntxtNarration1_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration2:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNarration2" runat="Server" CssClass="txt" TabIndex="25" Width="300px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration2_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNarration2" runat="server" Text="..." OnClick="btntxtNarration2_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Postage:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtPostage" runat="Server" CssClass="txt" TabIndex="17" Width="110px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPostage_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender6"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtPostage">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration3:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNarration3" runat="Server" CssClass="txt" TabIndex="27" Width="300px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration3_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNarration3" runat="server" Text="..." OnClick="btntxtNarration3_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration4:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNarration4" runat="Server" CssClass="txt" TabIndex="28" Width="300px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration4_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNarration4" runat="server" Text="..." OnClick="btntxtNarration4_Click"
                                Height="24px" Width="20px" CssClass="btnHelp" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 10%;">
                            Resell Comm'n:<asp:TextBox runat="server" ID="txtCommissionPerQntl" Width="40px"
                                TabIndex="18" AutoPostBack="true" Height="20px" OnTextChanged="txtCommissionPerQntl_TextChanged"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 10%; vertical-align: bottom;">
                            <asp:TextBox ID="txtResale_Commisson" runat="Server" CssClass="txt" TabIndex="19"
                                Width="110px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtResale_Commisson_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender7"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtResale_Commisson">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Taxable Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtTaxableAmount" runat="Server" CssClass="txt" Width="140px" Style="text-align: right;"
                                ReadOnly="True" Height="24px"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration 5:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNarration5" runat="Server" CssClass="txt" TabIndex="28" Width="300px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtNarration5_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Bank Commission:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBANK_COMMISSION" runat="Server" CssClass="txt" TabIndex="20"
                                Width="110px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBANK_COMMISSION_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender8"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtBANK_COMMISSION">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td style="width: 10%;">
                            CGST%
                        </td>
                        <td style="">
                            <asp:TextBox ID="txtCGSTRate" runat="Server" CssClass="txt" Width="52px" TabIndex="22"
                                Style="text-align: right;" Height="24px" AutoPostBack="true" OnTextChanged="txtCGSTRate_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredtxtCGSTRate" runat="server" FilterType="Numbers,Custom"
                                TargetControlID="txtCGSTRate" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:TextBox ID="txtCGSTAmount" runat="Server" CssClass="txt" Width="80px" ReadOnly="true"
                                Style="text-align: right;" Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Freight:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtFREIGHT" runat="Server" CssClass="txt" TabIndex="21" Width="110px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtFREIGHT_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender9"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtFREIGHT">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td style="">
                            SGST%
                        </td>
                        <td style="">
                            <asp:TextBox ID="txtSGSTRate" runat="Server" CssClass="txt" Width="50px" TabIndex="23"
                                AutoPostBack="true" OnTextChanged="txtSGSTRate_TextChanged" Style="text-align: right;"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredtxtSGSTRate" runat="server" FilterType="Numbers,Custom"
                                TargetControlID="txtSGSTRate" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:TextBox ID="txtSGSTAmount" runat="Server" CssClass="txt" Width="82px" ReadOnly="true"
                                Style="text-align: right;" Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Misc Exps:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtOTHER_Expenses" runat="Server" CssClass="txt" TabIndex="23" Width="110px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtOTHER_Expenses_TextChanged"
                                Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender10"
                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtOTHER_Expenses">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td style="">
                            IGST%
                        </td>
                        <td style="">
                            <asp:TextBox ID="txtIGSTRate" runat="Server" CssClass="txt" Width="52px" AutoPostBack="true"
                                OnTextChanged="txtIGSTRate_TextChanged" TabIndex="24" Style="text-align: right;"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredtxtIGSTRate" runat="server" FilterType="Numbers,Custom"
                                TargetControlID="txtIGSTRate" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:TextBox ID="txtIGSTAmount" runat="Server" CssClass="txt" Width="80px" ReadOnly="true"
                                Style="text-align: right;" Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Due Days:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtDueDays" runat="Server" CssClass="txt" TabIndex="24" Width="110px"
                                Style="text-align: right;" AutoPostBack="false" Height="24px" OnTextChanged="txtDueDays_TextChanged"></asp:TextBox><ajax1:FilteredTextBoxExtender
                                    runat="server" ID="FilteredTextBoxExtender12" FilterType="Numbers" TargetControlID="txtDueDays">
                                </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Voucher Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtVoucher_Amount" runat="Server" CssClass="txt" Width="140px" Style="text-align: right;"
                                AutoPostBack="True" OnTextChanged="txtVoucher_Amount_TextChanged" ReadOnly="True"
                                Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <table width="80%" align="left">
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            TabIndex="28" Height="24px" ValidationGroup="add" OnClick="btnSave_Click" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            TabIndex="29" Height="24px" ValidationGroup="save" OnClick="btnEdit_Click" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()"
                            TabIndex="30" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="31" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                        &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" Width="90px"
                            TabIndex="32" Height="24px" OnClick="btnPrint_Click" />
                    </td>
                    <td align="center" colspan="4">
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
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" autosize="true"
                Width="80%" align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px;
                min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center;
                left: 10%; top: 10%;">
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
                            <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server"
                                Width="250px" Height="20px" AutoPostBack="true" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search"
                                CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" ScrollBars="Auto" autosize="true" Width="100%"
                                Direction="LeftToRight" BackColor="#FFFFE4" Style="z-index: 5000; float: right;
                                overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="25" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
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
            <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="955px"
                BorderColor="Teal" BorderWidth="1px" Height="300px" BorderStyle="Solid" Style="z-index: 4999;
                left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                <table width="80%" align="center">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Tender Details"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
