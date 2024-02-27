<%@ Page Title="Loading Voucher" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeLoadingvoucher.aspx.cs" Inherits="pgeLoadingvoucher" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function p(VNO, type) {
            window.open('../Report/rptVouchersNew.aspx?VNO=' + VNO + '&type=' + type);
        }
        function CV(Doc_No) {
            var docno = document.getElementById('<%=txtDoc_no.ClientID %>').value;
            window.open('../Report/rptITCVouc.aspx?Doc_No=' + docno);
        }

        function FE(Doc_No) {
            var docno = document.getElementById('<%=txtDoc_no.ClientID %>').value;
            window.open('../Report/rptFormE.aspx?Doc_No=' + docno);
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

                if (hdnfClosePopupValue == "txtDoc_no") {
                    document.getElementById("<%=txtDoc_no.ClientID %>").value = "";
                    document.getElementById("<%=txtDoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtDoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtDoc_no.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtAC_CODE") {
                    document.getElementById("<%=txtAC_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblPartyname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtUnit_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUnit_Code") {
                    document.getElementById("<%=txtUnit_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUnitName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMill_Code.ClientID %>").focus(); txtUnit_Code
                }
                if (hdnfClosePopupValue == "txtMill_Code") {
                    document.getElementById("<%=txtMill_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMillname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtLorry_No.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtTRANSPORT_CODE") {
                    document.getElementById("<%=txtTRANSPORT_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblTransport_Name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBroker_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBroker_CODE") {
                    document.getElementById("<%=txtBroker_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBroker_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtFrom_Place.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtCash_Account") {
                    document.getElementById("<%=txtCash_Account.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLCash_Account.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtCash_Amount_RATE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDO_No") {
                    document.getElementById("<%=txtDO_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtAC_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGrade") {
                    document.getElementById("<%=txtGrade.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtMill_Rate.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGrade1") {
                    document.getElementById("<%=txtGrade1.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtMill_Rate1.ClientID %>").focus();
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
    <style type="text/css">
        .style1
        {
            width: 66%;
        }
        .style2
        {
            width: 362px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Loading Voucher   " Font-Names="verdana"
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
                <table align="left" style="width: 95%">
                    <tr>
                        <td align="center" class="style1">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                OnClick="btnAdd_Click" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                                TabIndex="45" OnClick="btnSave_Click" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                                OnClick="btnEdit_Click" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                                OnClick="btnDelete_Click" Height="24px" OnClientClick="Confirm()" />
                            &nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                                OnClick="btnCancel_Click" Height="24px" />
                            &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btnHelp" Width="90px"
                                Height="24px" OnClick="btnPrint_Click" />
                            &nbsp;<asp:Button ID="btnITCVouc" runat="server" Text="ITC Voucher" CssClass="btnHelp"
                                OnClientClick="return CV();" Width="90px" Height="24px" />
                            &nbsp;<asp:Button ID="btnFormE" runat="server" Text="Form E" CssClass="btnHelp" Width="90px"
                                OnClientClick="return FE();" Height="24px" />
                        </td>
                        <td align="center" colspan="4" class="style2">
                            <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnFirst_Click" Width="90px" Height="24px" />
                            &nbsp;<asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnPrevious_Click" Width="90px" Height="24px" />
                            &nbsp;
                            <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnNext_Click" Width="90px" Height="24px" />
                            &nbsp;<asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnLast_Click" Width="90px" Height="24px" />
                        </td>
                    </tr>
                </table>
                <table cellspacing="5" width="100%">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Voucher No.
                        </td>
                        <td align="left" colspan="10" style="width: 10%;">
                            <asp:TextBox ID="txtDoc_no" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtDoc_no_Click"
                                CssClass="btnHelp" Height="24px" />
                            Suffix
                            <asp:TextBox ID="txtSuffix" runat="Server" CssClass="txt" TabIndex="1" Width="30px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtSuffix_TextChanged"
                                Height="24px"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp; Date
                            <asp:TextBox ID="txtDoc_date" runat="Server" AutoPostBack="True" CssClass="txt" OnTextChanged="txtDoc_date_TextChanged"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" TabIndex="2" Width="80px" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            &nbsp;D.O.No
                            <asp:TextBox ID="txtDO_No" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDO_No_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtDO_No" runat="server" Text="..." OnClick="btntxtDO_No_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            &nbsp;&nbsp;Delivery Type :
                            <asp:Label runat="server" ID="lblDeliveryType" Font-Bold="true" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Party:
                        </td>
                        <td align="left" colspan="9" style="width: 10%;">
                            <asp:TextBox ID="txtAC_CODE" runat="Server" CssClass="txt" TabIndex="4" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtAC_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtAC_CODE" runat="server" Text="..." OnClick="btntxtAC_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblPartyname" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Unit:
                        </td>
                        <td align="left" colspan="9" style="width: 10%;">
                            <asp:TextBox ID="txtUnit_Code" runat="Server" CssClass="txt" TabIndex="5" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtUnit_Code_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtUnitcode" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                Width="20px" OnClick="btntxtUnitcode_Click" />
                            <asp:Label ID="lblUnitName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%">
                            Mill:
                        </td>
                        <td align="left" colspan="9" style="width: 10%;">
                            <asp:TextBox ID="txtMill_Code" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMill_Code_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtMill_Code" runat="server" Text="..." OnClick="btntxtMill_Code_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblMillname" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Lorry No:
                        </td>
                        <td align="left" colspan="5" style="width: 10%;">
                            <asp:TextBox ID="txtLorry_No" runat="Server" CssClass="txt" TabIndex="7" Width="150px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtLorry_No_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                        <td align="right" style="width: 10%;">
                            Transport:
                        </td>
                        <td align="left" colspan="6">
                            <asp:TextBox ID="txtTRANSPORT_CODE" runat="Server" CssClass="txt" TabIndex="8" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtTRANSPORT_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtTRANSPORT_CODE" runat="server" Text="..." OnClick="btntxtTRANSPORT_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblTransport_Name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Broker:
                        </td>
                        <td align="left" colspan="10">
                            <asp:TextBox ID="txtBroker_CODE" runat="Server" CssClass="txt" TabIndex="9" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBroker_CODE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtBroker_CODE" runat="server" Text="..." OnClick="btntxtBroker_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblBroker_NAME" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            From:
                        </td>
                        <td align="left" colspan="7" style="width: 10%;">
                            <asp:TextBox ID="txtFrom_Place" runat="Server" CssClass="txt" TabIndex="10" Width="150px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtFrom_Place_TextChanged"
                                Height="24px"></asp:TextBox>
                            &nbsp; To:
                            <asp:TextBox ID="txtTo_Place" runat="Server" CssClass="txt" TabIndex="11" Width="150px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtTo_Place_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Qntl:
                        </td>
                        <td align="left" colspan="9" style="width: 10%;">
                            <asp:TextBox ID="txtQuantal" runat="Server" CssClass="txt" TabIndex="12" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtQuantal_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtQuantal" TargetControlID="txtQuantal"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Packing:
                            <asp:TextBox ID="txtPACKING" runat="Server" CssClass="txt" TabIndex="13" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPACKING_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtPACKING" TargetControlID="txtPACKING"
                                FilterType="Numbers" runat="server">
                            </ajax1:FilteredTextBoxExtender>
                            Bags:
                            <asp:TextBox ID="txtBAGS" runat="Server" CssClass="txt" TabIndex="14" Width="80px"
                                ReadOnly="true" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBAGS_TextChanged"
                                Height="24px"></asp:TextBox>
                            Grade:
                            <asp:TextBox ID="txtGrade" runat="Server" CssClass="txt" TabIndex="15" Width="120px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtGrade_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGrade" runat="server" Text="..." OnClick="btntxtGrade_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            Mill Rate:
                            <asp:TextBox ID="txtMill_Rate" runat="Server" CssClass="txt" TabIndex="16" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMill_Rate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtMill_Rate" TargetControlID="txtMill_Rate"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtMill_Amount" runat="Server" CssClass="txt" TabIndex="17" ReadOnly="true"
                                Width="120px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMill_Amount_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtMill_Amount" TargetControlID="txtMill_Amount"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Qntl:
                        </td>
                        <td align="left" colspan="9" style="width: 10%;">
                            <asp:TextBox ID="txtQuantal1" runat="Server" CssClass="txt" TabIndex="18" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtQuantal1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtQuantal1" TargetControlID="txtQuantal1"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Packing:
                            <asp:TextBox ID="txtPACKING1" runat="Server" CssClass="txt" TabIndex="19" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPACKING1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtPACKING1" TargetControlID="txtPACKING1"
                                FilterType="Numbers" runat="server">
                            </ajax1:FilteredTextBoxExtender>
                            Bags:
                            <asp:TextBox ID="txtBAGS1" runat="Server" CssClass="txt" TabIndex="20" Width="80px"
                                ReadOnly="true" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBAGS1_TextChanged"
                                Height="24px"></asp:TextBox>
                            Grade:
                            <asp:TextBox ID="txtGrade1" runat="Server" CssClass="txt" TabIndex="21" Width="120px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtGrade1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGrade1" runat="server" Text="..." OnClick="btntxtGrade1_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            Mill Rate:
                            <asp:TextBox ID="txtMill_Rate1" runat="Server" CssClass="txt" TabIndex="22" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMill_Rate1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtMill_Rate1"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtMill_Amount1" runat="Server" CssClass="txt" TabIndex="23" Width="120px"
                                ReadOnly="true" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtMill_Amount1_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtMill_Amount1" TargetControlID="txtMill_Amount1"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Sale Rate:
                        </td>
                        <td align="left" colspan="9" style="width: 10%;">
                            <asp:TextBox ID="txtSale_Rate" runat="Server" CssClass="txt" TabIndex="24" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtSale_Rate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtSale_Rate" TargetControlID="txtSale_Rate"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;Less Freight:
                            <asp:TextBox ID="txtFreightPerQtl" runat="Server" CssClass="txt" TabIndex="25" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtFreightPerQtl_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtFreightPerQtl" TargetControlID="txtFreightPerQtl"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            Difference:
                            <asp:TextBox ID="txtLESSDIFF" runat="Server" CssClass="txt" TabIndex="26" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtLESSDIFF_TextChanged"
                                ReadOnly="true" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtLESSDIFF" TargetControlID="txtLESSDIFF"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left">
                            Rate Diff:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFreightDiff" runat="Server" CssClass="txt" TabIndex="27" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtFreightDiff_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtFreightDiff"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".,-">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Remarks:
                        </td>
                        <td align="left" colspan="7" style="width: 10%;">
                            <asp:TextBox ID="txtNarration1" runat="Server" CssClass="txt" TabIndex="28" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration1_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 10%;">
                            Brokrage:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBrokrage" runat="Server" CssClass="txt" TabIndex="32" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBrokrage_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtBrokrage" TargetControlID="txtBrokrage"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Remarks:
                        </td>
                        <td align="left" colspan="7" style="width: 10%;">
                            <asp:TextBox ID="txtNarration2" runat="Server" CssClass="txt" TabIndex="29" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration2_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 10%;">
                            Service Charge:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtService_Charge" runat="Server" CssClass="txt" TabIndex="33" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtService_Charge_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtService_Charge" TargetControlID="txtService_Charge"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Remarks:
                        </td>
                        <td align="left" colspan="5" style="width: 10%;">
                            <asp:TextBox ID="txtNarration3" runat="Server" CssClass="txt" TabIndex="30" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration3_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="right" style="width: 10%;">
                            Rate Diff:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtL_Rate_Diff" runat="Server" CssClass="txt" TabIndex="34" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtL_Rate_Diff_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtL_Rate_Diff" TargetControlID="txtL_Rate_Diff"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtRATEDIFF" runat="Server" CssClass="txt" Width="120px" Style="text-align: right;"
                                AutoPostBack="True" ReadOnly="true" OnTextChanged="txtRATEDIFF_TextChanged" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtRATEDIFF" TargetControlID="txtRATEDIFF"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Due Days:
                        </td>
                        <td align="left" colspan="4" style="width: 10%;">
                            <asp:TextBox ID="txtDue_Days" runat="Server" CssClass="txt" TabIndex="31" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtDue_Days_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtDue_Days" TargetControlID="txtDue_Days"
                                FilterType="Numbers" runat="server">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="right" colspan="2" style="width: 10%;">
                            Bank Comm:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtCommission_Rate" runat="Server" CssClass="txt" TabIndex="35"
                                Width="80px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCommission_Rate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtCommission_Rate" TargetControlID="txtCommission_Rate"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBANK_COMMISSION" runat="Server" CssClass="txt" Width="120px"
                                ReadOnly="true" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBANK_COMMISSION_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtBANK_COMMISSION" TargetControlID="txtBANK_COMMISSION"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 10%;">
                            Interest:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtInterest" runat="Server" CssClass="txt" TabIndex="36" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtInterest_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtInterest" TargetControlID="txtInterest"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Narration:
                        </td>
                        <td align="left" colspan="7" style="width: 10%;">
                            <asp:TextBox ID="txtNarration4" runat="Server" CssClass="txt" TabIndex="40" Width="200px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNarration4_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 10%;">
                            Transport:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtTransport_Amount" runat="Server" CssClass="txt" TabIndex="37"
                                Width="120px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtTransport_Amount_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtTransport_Amount" TargetControlID="txtTransport_Amount"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 10%;">
                            Other Exps:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtOTHER_Expenses" runat="Server" CssClass="txt" TabIndex="38" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtOTHER_Expenses_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtOTHER_Expenses" TargetControlID="txtOTHER_Expenses"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Other A/C:
                        </td>
                        <td colspan="6" align="left" style="width: 10%;">
                            <asp:TextBox ID="txtCash_Account" runat="Server" CssClass="txt" TabIndex="41" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCash_Account_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtCash_Account" runat="server" Text="..." OnClick="btntxtCash_Account_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LBLCash_Account" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtCash_Amount_RATE" runat="Server" CssClass="txt" Visible="false"
                                Width="80px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCash_Amount_RATE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtCash_Amount_RATE"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                        <td colspan="2" align="left">
                            <asp:DropDownList runat="server" ID="drpRateType" CssClass="ddl" AutoPostBack="true"
                                Visible="false" OnSelectedIndexChanged="drpRateType_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value="NULL"></asp:ListItem>
                                <asp:ListItem Text="Add" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Less" Value="L"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left" style="width: 10%;">
                            Transport Amt:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtCash_Ac_Amount" runat="Server" CssClass="txt" TabIndex="39" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCash_Ac_Amount_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtCash_Ac_Amount" TargetControlID="txtCash_Ac_Amount"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            ASN/GRN No.:
                        </td>
                        <td colspan="9">
                            <asp:TextBox runat="server" ID="txtASNGRNNo" CssClass="txt" Width="120px" Height="24px"
                                TabIndex="43"></asp:TextBox>
                        </td>
                        <td align="left" style="width: 10%;">
                            Voucher Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtVoucher_Amount" runat="Server" CssClass="txt" TabIndex="42" Width="120px"
                                ReadOnly="true" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtVoucher_Amount_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtVoucher_Amount" TargetControlID="txtVoucher_Amount"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
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
