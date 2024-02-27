<%@ Page Title="Tender Purchase" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="pgeTenderPurchase.aspx.cs"
    ValidateRequest="false" Inherits="Sugar_pgeTenderPurchase" %>

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

                if (hdnfClosePopupValue == "GR") {
                    document.getElementById("<%=txtGrade.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtQuantal.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "PT") {
                    document.getElementById("<%=txtPaymentTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblPaymentTo.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtTenderFrom.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "TF") {
                    document.getElementById("<%=txtTenderFrom.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblTenderFrom.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtDO.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "DO") {
                    document.getElementById("<%=txtDO.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblDO.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtVoucherBy.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "VB") {
                    document.getElementById("<%=txtVoucherBy.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblVoucherBy.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBroker.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "BR") {
                    document.getElementById("<%=txtBroker.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBroker.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtExciseRate.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "BU") {
                    document.getElementById("<%=pnlPopupTenderDetails.ClientID %>").style.display = "block";
                    document.getElementById("<%=txtBuyer.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBuyerName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=drpDeliveryType.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "BP") {
                    document.getElementById("<%=pnlPopupTenderDetails.ClientID %>").style.display = "block";
                    document.getElementById("<%=txtBuyerParty.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBuyerPartyName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBuyerQuantal.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "TN") {
                    document.getElementById("<%=txtTenderNo.ClientID %>").value = "";
                    document.getElementById("<%=txtTenderNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtTenderNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtTenderNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "MM") {
                    document.getElementById("<%=txtMillCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMillName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;

                    document.getElementById("<%=txtGrade.ClientID %>").focus();
                }
            } //13
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
        function refreshparent(source) {
            if (source == 'R') {
                window.close();
                window.opener.location = "";
                window.opener.location.reload();
            }
        }

    </script>
    <script type="text/javascript">
        function searchKeyPress(e) {
            // look for window.event in case event isn't passed in
            e = e || window.event;
            if (e.keyCode == 13) {
                document.getElementById('btnMillCode').click();
                return true;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Tender Purchase   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="upPnlPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfIsClick" runat="server" Value="0" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfNextFocus" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="vouchernumber" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="True" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="100%" align="left" cellspacing="4px" cellpadding="1px">
                    <tr>
                        <td colspan="4">
                            <table width="80%" align="left">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="80px"
                                            Height="25px" OnClick="btnAdd_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="add" OnClick="btnSave_Click" TabIndex="30" />
                                        &nbsp;
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="save" OnClick="btnEdit_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="80px"
                                            Height="25px" OnClick="btnCancel_Click" TabIndex="30" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" Width="80px" Height="25px"
                                            CssClass="btnHelp" OnClick="btnFirst_Click" />
                                        <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="Previous" Width="80px"
                                            Height="25px" CssClass="btnHelp" OnClick="btnPrevious_Click" />
                                        <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="Next" Width="80px" Height="25px"
                                            CssClass="btnHelp" OnClick="btnNext_Click" />
                                        <asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="Last" Width="80px" Height="25px"
                                            CssClass="btnHelp" OnClick="btnLast_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Change No:
                        </td>
                        <td colspan="4" align="left">
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                TargetControlID="txtEditDoc_No">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Names="verdana" Font-Italic="true"
                                Font-Size="Small" ForeColor="Yellow"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">
                            Tender No:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtTenderNo" runat="server" CssClass="txt" Width="100px" TabIndex="0"
                                AutoPostBack="True" OnTextChanged="txtTenderNo_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnChangeNo" runat="server" Text="Change No" CssClass="btnHelp" Width="69px"
                                TabIndex="1" OnClick="changeNo_click" Height="24px" />
                            &nbsp;Resale/Mill:&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="drpResale" runat="server" AutoPostBack="True" Width="100px"
                                CssClass="ddl" OnSelectedIndexChanged="drpResale_SelectedIndexChanged" TabIndex="2">
                                <asp:ListItem Text="Resale" Value="R"></asp:ListItem>
                                <asp:ListItem Text="Mill" Value="M" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="With Payment" Value="W"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp; Voucher No:<asp:Label runat="server" ID="lblVoucherNo" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Date:
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtDate" runat="server" CssClass="txt" Width="100px" AutoPostBack="True"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                OnTextChanged="txtDate_TextChanged" TabIndex="3" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDate"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lifting Date:
                            &nbsp;&nbsp;<asp:TextBox ID="txtLiftingDate" runat="server" CssClass="txt" Width="100px"
                                AutoPostBack="true" OnTextChanged="txtLiftingDate_TextChanged" TabIndex="4" onkeyup="ValidateDate(this,event.keyCode)"
                                onkeydown="return DateFormat(this,event.keyCode)" MaxLength="10" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender1" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderLiftingdate" runat="server" TargetControlID="txtLiftingDate"
                                PopupButtonID="imgcalender1" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            <asp:Label runat="server" ID="lblMesg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Mill Code:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtMillCode" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                onkeypress="return searchKeyPress(event);" AutoPostBack="True" OnTextChanged="txtMillCode_TextChanged"
                                TabIndex="5" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnMillCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnMillCode_Click"
                                Height="24px" Width="20px" />
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillCode" FilterType="Numbers" TargetControlID="txtMillCode"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Label ID="lblMillName" runat="server" CssClass="lblName"></asp:Label>
                            <%-- <asp:RequiredFieldValidator ID="rfvtxtMillCode0" runat="server" ControlToValidate="txtMillCode"
                                CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                Text="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Grade:
                        </td>
                        <td align="left" colspan="6">
                            <asp:TextBox ID="txtGrade" runat="server" CssClass="txt" Width="100px" TabIndex="6"
                                AutoPostBack="True" OnTextChanged="txtGrade_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnGrade" runat="server" Text="..." CssClass="btnHelp" OnClick="btnGrade_Click"
                                Height="24px" Width="20px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quintal:
                            <asp:TextBox ID="txtQuantal" runat="server" CssClass="txt" Width="100px" Style="text-align: left;"
                                AutoPostBack="True" OnTextChanged="txtQuantal_TextChanged" TabIndex="7" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtQuantal" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtQuantal">
                            </ajax1:FilteredTextBoxExtender>
                            Packing:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPacking" runat="server" CssClass="txt" AutoPostBack="True" Width="100px"
                                Style="text-align: left;" OnTextChanged="txtPacking_TextChanged" TabIndex="8"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtPacking" FilterType="Numbers"
                                TargetControlID="txtPacking">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;Bags:
                            <asp:TextBox ID="txtBags" runat="server" CssClass="txt" ReadOnly="true" Width="100px"
                                TabIndex="9" Style="text-align: left;" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBags" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtBags">
                            </ajax1:FilteredTextBoxExtender>
                            Balance Self:
                            <asp:Label ID="lblBalanceSelf" runat="server" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Mill Rate:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtMillRate" runat="server" CssClass="txt" AutoPostBack="True" Width="100px"
                                Style="text-align: right;" OnTextChanged="txtMillRate_TextChanged" TabIndex="10"
                                Height="24px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtMillRate" SetFocusOnError="true" runat="server"
                                ControlToValidate="txtMillRate" CssClass="validator" Display="Dynamic" Text="Required"
                                ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtMillRate">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Purch Rate:
                            <asp:TextBox ID="txtPurcRate" runat="server" AutoPostBack="True" CssClass="txt" Width="100px"
                                OnTextChanged="txtPurcRate_TextChanged" TabIndex="11" Height="24px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtPurcRate" Enabled="false" SetFocusOnError="true"
                                runat="server" ControlToValidate="txtPurcRate" CssClass="validator" Display="Dynamic"
                                Text="Required" ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtPurcRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtPurcRate">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;Diff:
                            <asp:Label ID="lbldiff" runat="server" Text="Diff"></asp:Label>
                            Amount:
                            <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Payment To:
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtPaymentTo" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="true" OnTextChanged="txtPaymentTo_TextChanged" TabIndex="12" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnPaymentTo" runat="server" Text="..." CssClass="btnHelp" OnClick="btnPaymentTo_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblPaymentTo" runat="server" CssClass="lblName"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvtxtPaymentTo" runat="server" ControlToValidate="txtPaymentTo"
                                CssClass="validator" Display="Dynamic" Enabled="false" ErrorMessage="Required"
                                SetFocusOnError="true" Text="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            Tender From:
                            <asp:TextBox ID="txtTenderFrom" runat="server" CssClass="txt" Width="100px" TabIndex="15"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtTenderFrom_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btnTenderFrom" runat="server" Text="..." CssClass="btnHelp" OnClick="btnTenderFrom_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblTenderFrom" runat="server" CssClass="lblName"></asp:Label>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtTenderFrom" FilterType="Numbers"  TargetControlID="txtTenderFrom"></ajax1:FilteredTextBoxExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Tender D.O.:
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtDO" runat="server" CssClass="txt" Width="100px" TabIndex="13"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDO_TextChanged"
                                Height="24px"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtDO" FilterType="Numbers"  TargetControlID="txtDO"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnTenderDO" runat="server" Text="..." OnClick="btnTenderDO_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblDO" runat="server" CssClass="lblName"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvtxtDO" Enabled="false" SetFocusOnError="true"
                                runat="server" ControlToValidate="txtDO" CssClass="validator" Display="Dynamic"
                                Text="Required" ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            Voucher By: &nbsp;
                            <asp:TextBox ID="txtVoucherBy" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="True" OnTextChanged="txtVoucherBy_TextChanged" TabIndex="16" Height="24px"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtVoucherBy" FilterType="Numbers"  TargetControlID="txtVoucherBy"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnVoucherBy" runat="server" Text="..." CssClass="btnHelp" OnClick="btnVoucherBy_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblVoucherBy" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Broker:
                        </td>
                        <td colspan="4" align="left">
                            <asp:TextBox ID="txtBroker" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="True" OnTextChanged="txtBroker_TextChanged" TabIndex="14" Height="24px"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBroker" FilterType="Numbers"  TargetControlID="txtBroker"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnBroker" runat="server" Text="..." OnClick="btnBroker_Click" CssClass="btnHelp"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblBroker" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Excise / GST Rate:
                        </td>
                        <td colspan="5" align="left">
                            <asp:TextBox ID="txtExciseRate" runat="server" CssClass="txt" TabIndex="17" Width="100px"
                                Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtExciseRate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtExciseRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtExciseRate">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;GST Rate:<asp:Label Text="" runat="server" ID="lblMillRateGst" ForeColor="Yellow" />&nbsp;
                            Sell Note No:<asp:TextBox runat="server" ID="txtSellNoteNo" Width="150px" Height="24px"
                                TabIndex="18" Style="text-align: right;" CssClass="txt" OnTextChanged="txtSellNoteNo_TextChanged"></asp:TextBox>
                            Narration:
                            <asp:TextBox ID="txtNarration" runat="server" CssClass="txt" TabIndex="19" AutoPostBack="True"
                                Width="250px" OnTextChanged="txtNarration_TextChanged" Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Button ID="Button1" runat="server" Text="ADD" CssClass="btnHelp" Width="72px"
                                Height="24px" OnClick="Button1_Click" TabIndex="25" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <asp:UpdatePanel ID="upGrid" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="220px" Width="1200px"
                                        BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                                        Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                                        <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                            HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="112%"
                                            Height="65%" OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5"
                                            OnRowDataBound="grdDetail_RowDataBound" Style="table-layout: fixed;">
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
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <%-- <div style="width: 100%; position: fixed;">
            </div>--%>
            <asp:Panel ID="pnlPopupTenderDetails" runat="server" BackColor="GhostWhite" Width="800px"
                BorderColor="Teal" BorderWidth="1px" Height="330px" BorderStyle="Solid" Style="z-index: 4999;
                left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                <table width="100%" cellpadding="4px" cellspacing="4px">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Tender Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            ID:
                        </td>
                        <td>
                            <asp:Label ID="lblID" runat="server"></asp:Label>
                            <asp:Label ID="lblno" runat="server" ForeColor="Azure"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Party:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBuyer" runat="server" Width="80px" Height="24px" AutoPostBack="true"
                                CssClass="txt" OnTextChanged="txtBuyer_TextChanged" TabIndex="32"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyer" FilterType="Numbers" TargetControlID="txtBuyer"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnBuyer" runat="server" Text="..." Height="24px" Width="20px" CssClass="btnHelp"
                                OnClick="btnBuyer_Click" />
                            <asp:Label ID="lblBuyerName" runat="server"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvtxtBuyer" runat="server" ControlToValidate="txtBuyer"
                                CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                Text="Required" ValidationGroup="addBuyerDetails">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Delivery Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpDeliveryType" runat="server" CssClass="ddl" Width="140px"
                                TabIndex="33" AutoPostBack="true" Height="26px" OnSelectedIndexChanged="drpDeliveryType_SelectedIndexChanged">
                                <asp:ListItem Text="Naka Delivery" Value="N"></asp:ListItem>
                                <asp:ListItem Text="Commission" Value="C"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Broker:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBuyerParty" runat="server" Width="80px" CssClass="txt" Height="24px"
                                OnTextChanged="txtBuyerParty_TextChanged" AutoPostBack="true" TabIndex="34"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerParty" FilterType="Numbers" TargetControlID="txtBuyerParty"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnBuyerParty" Height="24px" Width="20px" runat="server" Text="..."
                                CssClass="btnHelp" OnClick="btnBuyerParty_Click" />
                            <asp:Label ID="lblBuyerPartyName" runat="server"></asp:Label>
                            <%--<asp:RequiredFieldValidator ID="rfvtxtBuyerParty" runat="server" 
ControlToValidate="txtBuyerParty" CssClass="validator" Display="Dynamic" 
ErrorMessage="Required" SetFocusOnError="true" Text="Required" ValidationGroup="addBuyerDetails">
</asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Buyer Quantal:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBuyerQuantal" runat="server" Width="80px" Height="24px" CssClass="txt"
                                AutoPostBack="true" OnTextChanged="txtBuyerQuantal_TextChanged" TabIndex="35"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerQuantal" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtBuyerQuantal">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="rfvtxtBuyerQuantal" runat="server" ControlToValidate="txtBuyerQuantal"
                                CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                Text="Required" ValidationGroup="addBuyerDetails">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Sale Rate:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBuyerSaleRate" runat="server" Width="80px" Height="24px" CssClass="txt"
                                AutoPostBack="true" OnTextChanged="txtBuyerSaleRate_TextChanged" TabIndex="36"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerSaleRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtBuyerSaleRate">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:RequiredFieldValidator ID="rfvtxtBuyerSaleRate" runat="server" ControlToValidate="txtBuyerSaleRate"
                                CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                Text="Required" ValidationGroup="addBuyerDetails">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Commission:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBuyerCommission" runat="server" CssClass="txt" Height="24px"
                                TabIndex="37" Width="80px" AutoPostBack="true" OnTextChanged="txtBuyerCommission_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerCommission" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtBuyerCommission">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Narration:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBuyerNarration" runat="server" Width="360px" CssClass="txt" Height="50px"
                                TextMode="MultiLine" TabIndex="38" AutoPostBack="true" OnTextChanged="txtBuyerNarration_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnADDBuyerDetails" runat="server" Text="ADD" CssClass="btnSubmit"
                                Font-Bold="false" OnClick="btnADDBuyerDetails_Click" ValidationGroup="addBuyerDetails"
                                TabIndex="39" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btnSubmit"
                                TabIndex="40" Font-Bold="false" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%--<asp:Button ID="btnPop" runat="server" style="display:none;" />
<ajax1:ModalPopupExtender ID="PopupTenderDetails" runat="server" PopupControlID="pnlPopupTenderDetails" X="60" Y="100"
TargetControlID="btnPop"    >
</ajax1:ModalPopupExtender>--%>
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
                            <%--<asp:Panel ID="pnlScroll" runat="server" Width="680px" ScrollBars="Both" Direction="LeftToRight" BackColor="#FFFFE4" style="z-index:5000 ;  float:right; max-height:380px; height:380px;">--%>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" EmptyDataText="No Records Found"
                                    HeaderStyle-BackColor="#6D8980" HeaderStyle-ForeColor="White" PageSize="20" AllowPaging="true"
                                    OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;"
                                    OnPageIndexChanging="grdPopup_PageIndexChanging">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
