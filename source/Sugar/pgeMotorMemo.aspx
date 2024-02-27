<%@ Page Title="Motor Memo" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeMotorMemo.aspx.cs" Inherits="Sugar_pgeMotorMemo" %>

<%--<%@ PreviousPageType VirtualPath="~/Sugar/pgeDeliveryorder.aspx" %>--%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">

        function sp(do_no, email) {
            var tn;
            window.open('../Report/rptMotorMemoBlank.aspx?do_no=' + do_no + '&email=' + email, '_blank');    //R=Redirected  O=Original
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

                if (hdnfClosePopupValue == "txtMILL_CODE") {
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLMILL_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtPurcNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGETPASS_CODE") {
                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLGETPASS_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGRADE.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtPartyCode") {
                    document.getElementById("<%=txtPartyCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLPARTY_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGRADE") {
                    document.getElementById("<%=txtGRADE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;

                    document.getElementById("<%=txtquantal.ClientID %>").focus();
                }


                if (hdnfClosePopupValue == "txtTRANSPORT_CODE") {
                    document.getElementById("<%=txtTRANSPORT_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLTRANSPORT_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtNARRATION1.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtDOC_DATE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION1") {
                    document.getElementById("<%=txtNARRATION1.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION2.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION2") {
                    document.getElementById("<%=txtNARRATION2.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION3.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION3") {
                    document.getElementById("<%=txtNARRATION3.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION4.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION4") {
                    document.getElementById("<%=txtNARRATION4.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=btnOpenDetailsPopup.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtPurcNo") {

                    document.getElementById("<%=txtPurcNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;

                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").focus();
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
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Motor Memo   " Font-Names="verdana"
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
                <table width="80%" align="left" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Do No.
                        </td>
                        <td align="left" style="width: 15%;">
                            <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtDOC_NO" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                CssClass="btnHelp" Height="24px" />
                        </td>
                        <td align="left" style="width: 10%;">
                            Date:
                        </td>
                        <td align="left" style="width: 10%;" colspan="2">
                            <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="1" Width="120px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtDOC_DATE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Purc. No:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPurcNo" runat="Server" CssClass="txt" TabIndex="2" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPurcNo_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtPurcNo" runat="server" Text="..." OnClick="btntxtPurcNo_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            &nbsp;
                            <asp:Label ID="lblPurcOrder" runat="server" Text="" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Mill Code:
                        </td>
                        <td align="left" colspan="2" style="width: 10%;">
                            <asp:TextBox ID="txtMILL_CODE" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMILL_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtMILL_CODE" runat="server" Text="..." OnClick="btntxtMILL_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LBLMILL_NAME" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Voucher By:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtPartyCode" runat="Server" CssClass="txt" TabIndex="4" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPartyCode_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtPartyCode" runat="server" Text="..." OnClick="btntxtPartyCode_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LBLPARTY_NAME" runat="server" CssClass="lblName"></asp:Label>
                            EMail:<asp:TextBox ID="txtPartyEmail" CssClass="txt" runat="server" Width="250px"
                                TabIndex="5" AutoPostBack="True" OnTextChanged="txtPartyEmail_TextChanged" Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Get Pass:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtGETPASS_CODE" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtGETPASS_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGETPASS_CODE" runat="server" Text="..." OnClick="btntxtGETPASS_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LBLGETPASS_NAME" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Grade:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtGRADE" runat="Server" CssClass="txt" TabIndex="7" Width="150px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtGRADE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGRADE" runat="server" Text="..." OnClick="btntxtGRADE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            Quantal:
                            <asp:TextBox ID="txtquantal" runat="Server" CssClass="txt" TabIndex="8" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtquantal_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtquantal" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtquantal">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp; Packing: &nbsp;
                            <asp:TextBox ID="txtPACKING" runat="Server" CssClass="txt" TabIndex="9" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPACKING_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteretxtPACKING" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtPACKING">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp; Bags: &nbsp;
                            <asp:TextBox ID="txtBAGS" runat="Server" CssClass="txt" Width="88px" ReadOnly="true"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBAGS_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteretxtBAGS" runat="server" FilterType="Numbers"
                                TargetControlID="txtPACKING">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;
                        </td>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Freight/Qtl:
                            </td>
                            <td align="left" colspan="2" style="width: 10%;">
                                <asp:TextBox ID="txtfreightperQtl" runat="Server" CssClass="txt" TabIndex="10" Width="103px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtfreightperQtl_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtfreightperQtl" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtfreightperQtl">
                                </ajax1:FilteredTextBoxExtender>
                            </td>
                            <td align="left">
                                Amount:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtFreightperQtlAmount" runat="Server" CssClass="txt" ReadOnly="true"
                                    Width="120px" Style="text-align: right;" AutoPostBack="True" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtFreightAmount" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtFreightAmount">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;
                            </td>
                        </tr>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Transport:
                        </td>
                        <td align="left" colspan="2" style="width: 10%;">
                            <asp:TextBox ID="txtTRANSPORT_CODE" runat="Server" CssClass="txt" TabIndex="11" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtTRANSPORT_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtTRANSPORT_CODE" runat="server" Text="..." OnClick="btntxtTRANSPORT_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LBLTRANSPORT_NAME" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                        <td align="left">
                            Less:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLess" runat="Server" CssClass="txt" TabIndex="12" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtLess_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FiltertxtLess" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtLess">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Vasuli:
                        </td>
                        <td align="left" colspan="2" style="width: 10%;">
                            <asp:TextBox ID="txtVasuliRate" runat="Server" CssClass="txt" TabIndex="13" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtVasuliRate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtVasuliRate">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:TextBox ID="txtVasuliAmount" runat="Server" CssClass="txt" Width="113px" Style="text-align: right;"
                                ReadOnly="true" Height="24px"></asp:TextBox>
                        </td>
                        <td align="left">
                            Final Amount:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFinalAmount" runat="Server" CssClass="txt" Width="120px" Style="text-align: right;"
                                ReadOnly="true" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FiltertxtFinalAmount" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtFinalAmount">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Truck No:
                        </td>
                        <td align="left" style="width: 10%;" colspan="2">
                            <asp:TextBox ID="txtTruck_NO" runat="Server" CssClass="txt" TabIndex="14" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtTruck_NO_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                        <td align="left">
                            Mobile No:
                        </td>
                        <td align="left" colspan="2">
                            <asp:TextBox ID="txtMobile" runat="Server" CssClass="txt" TabIndex="15" Width="200px"
                                MaxLength="10" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtMobile_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                TargetControlID="txtMobile">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Narration1:
                        </td>
                        <td align="left" style="width: 10%;" colspan="2">
                            <asp:TextBox ID="txtNARRATION1" runat="Server" CssClass="txt" TabIndex="16" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNARRATION1" runat="server" Text="..." OnClick="btntxtNARRATION1_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration3:
                        </td>
                        <td align="left" style="width: 15%;" colspan="2">
                            <asp:TextBox ID="txtNARRATION3" runat="Server" CssClass="txt" TabIndex="18" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION3_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNARRATION3" runat="server" Text="..." OnClick="btntxtNARRATION3_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Narration2:
                        </td>
                        <td align="left" style="width: 10%;" colspan="2">
                            <asp:TextBox ID="txtNARRATION2" runat="Server" CssClass="txt" TabIndex="17" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION2_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNARRATION2" runat="server" Text="..." OnClick="btntxtNARRATION2_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <td align="left" style="width: 10%;">
                            Narration4:
                        </td>
                        <td align="left" style="width: 10%;" colspan="2">
                            <asp:TextBox ID="txtNARRATION4" runat="Server" CssClass="txt" TabIndex="19" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION4_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtNARRATION4" runat="server" Text="..." OnClick="btntxtNARRATION4_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                        </td>
                        <tr>
                            <td align="left" valign="top">
                                <br />
                                Freight Rate:
                                <br></br>
                                <br></br>
                                <br></br>
                            </td>
                            <td align="left" colspan="5" style="width: 100%;">
                                <table align="left" width="100%" cellspacing="5">
                                    <tr>
                                        <td align="left">
                                            <asp:TextBox ID="txtfreightRate" runat="Server" AutoPostBack="True" CssClass="txt"
                                                OnTextChanged="txtfreightRate_TextChanged" Style="text-align: left;" TabIndex="20"
                                                Width="80px" Height="24px"></asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers,Custom"
                                                ValidChars="." TargetControlID="txtfreightRate">
                                            </ajax1:FilteredTextBoxExtender>
                                            &nbsp;
                                            <asp:TextBox ID="txtfreightAmount" runat="Server" CssClass="txt" ReadOnly="true"
                                                Style="text-align: left;" Width="120px" Height="24px"></asp:TextBox>
                                        </td>
                                        <td align="left">
                                            Paid: &nbsp;
                                            <asp:TextBox ID="txtpaid1" runat="Server" AutoPostBack="True" CssClass="txt" OnTextChanged="txtpaid1_TextChanged"
                                                Style="text-align: left;" TabIndex="21" Width="80px" Height="24px"></asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers,Custom"
                                                ValidChars="." TargetControlID="txtpaid1">
                                            </ajax1:FilteredTextBoxExtender>
                                            &nbsp;
                                            <asp:TextBox ID="txtpaidAmount1" runat="Server" CssClass="txt" ReadOnly="true" Style="text-align: left;"
                                                Width="120px" Height="24px"></asp:TextBox>
                                            &nbsp; Narration:
                                            <asp:TextBox ID="txtpaidNarration1" runat="Server" AutoPostBack="True" CssClass="txt"
                                                OnTextChanged="txtpaidNarration1_TextChanged" Style="text-align: left;" TabIndex="22"
                                                Width="199px" Height="24px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            Paid: &nbsp;
                                            <asp:TextBox ID="txtpaid2" runat="Server" AutoPostBack="True" CssClass="txt" OnTextChanged="txtpaid2_TextChanged"
                                                Style="text-align: left;" TabIndex="23" Width="80px" Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                                    TargetControlID="txtpaid2">
                                                </ajax1:FilteredTextBoxExtender>
                                            &nbsp;
                                            <asp:TextBox ID="txtpaidAmount2" runat="Server" CssClass="txt" ReadOnly="true" Style="text-align: left;"
                                                Width="120px" Height="24px"></asp:TextBox>
                                            &nbsp; Narration:
                                            <asp:TextBox ID="txtpaidNarration2" runat="Server" AutoPostBack="True" CssClass="txt"
                                                OnTextChanged="txtpaidNarration2_TextChanged" Style="text-align: left;" TabIndex="24"
                                                Width="199px" Height="24px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            Paid: &nbsp;
                                            <asp:TextBox ID="txtpaid3" runat="Server" AutoPostBack="True" CssClass="txt" OnTextChanged="txtpaid3_TextChanged"
                                                Style="text-align: left;" TabIndex="25" Width="80px" Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender
                                                    ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers,Custom" ValidChars="."
                                                    TargetControlID="txtpaid3">
                                                </ajax1:FilteredTextBoxExtender>
                                            &nbsp;
                                            <asp:TextBox ID="txtpaidAmount3" runat="Server" CssClass="txt" ReadOnly="true" Style="text-align: left;"
                                                Width="120px" Height="24px"></asp:TextBox>
                                            &nbsp; Narration:
                                            <asp:TextBox ID="txtpaidNarration3" runat="Server" AutoPostBack="True" CssClass="txt"
                                                OnTextChanged="txtpaidNarration3_TextChanged" Style="text-align: left;" TabIndex="26"
                                                Width="199px" Height="24px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                    Width="80px" Height="24px" Visible="false" />
                            </td>
                        </tr>
                    </tr>
                </table>
            </asp:Panel>
            <table width="80%" align="left">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="save" OnClick="btnAdd_Click" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            TabIndex="27" Height="24px" ValidationGroup="add" OnClick="btnSave_Click" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="save" OnClick="btnEdit_Click" TabIndex="28" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            TabIndex="29" Height="24px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="30" Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />
                        &nbsp;
                        <asp:Button ID="btnMail" runat="server" Text="Email MM" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="save" OnClick="btnMail_Click" TabIndex="30" />
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
                BackColor="#FFFFE4" Direction="LeftToRight" Style="z-index: 5000; overflow: auto;
                position: absolute; display: none; float: right; max-height: 500px; min-height: 500px;
                box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
                <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                    Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
                    ToolTip="Close" />
                <table width="95%" cellspacing="5">
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
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" ScrollBars="Both" Direction="LeftToRight"
                                BackColor="#FFFFE4" Style="z-index: 5000; float: right; overflow: auto; height: 400px">
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
