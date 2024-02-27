<%@ Page Title="UTR Entry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeUtrentry.aspx.cs" Inherits="Sugar_pgeUtrentry" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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


        function UP(DocNo) {
            var No = document.getElementById('<%=txtDoc_no.ClientID %>').value;
            window.open('../Report/rptUtrPrint.aspx?Doc_No=' + No);
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

                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtDoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtDoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtDoc_no.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtbank_ac") {

                    document.getElementById("<%=txtbank_ac.ClientID%>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBank_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtmill_code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtmill_code") {

                    document.getElementById("<%=txtmill_code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMill_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtamount.ClientID %>").focus();
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
        function enable(invoker) {
            var textbox = document.getElementById("txtMillMobile");
            document.getElementById('<%= txtMillMobile.ClientID%>').disabled = invoker.checked ? false : true;
            if (!document.getElementById('<%= txtMillMobile.ClientID%>').disabled) {
                textbox.focus();
            }

        }
    </script>
    <script type="text/javascript">
        function text() {
            var txtmobile = document.getElementById('<%= txtMillMobile.ClientID%>').value;

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
            <asp:Label ID="label1" runat="server" Text="   UTR Entry   " Font-Names="verdana"
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
                <table width="100%" align="center">
                    <tr>
                        <td align="right">
                            Mill Mobile:<asp:TextBox runat="server" ID="txtMillMobile" Enabled="true" CssClass="txt"
                                MaxLength="10" Width="200px" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="ajx1" TargetControlID="txtMillMobile"
                                FilterMode="ValidChars" FilterType="Numbers">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;<asp:CheckBox runat="server" ID="rbEditMobile" Text="Edit" OnClick="enable(this);" />&nbsp;<asp:Button
                                runat="server" ID="btnSendSMS" Text="Send Sms" CssClass="btnHelp" Height="24px"
                                OnClick="btnSendSMS_Click" Width="100px" />
                        </td>
                    </tr>
                </table>
                <table width="60%" align="center" cellpadding="3" cellspacing="5">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Entry No:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtDoc_no" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                CssClass="btnHelp" Height="24px" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Entry Date:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtdoc_date" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
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
                        <td align="left" style="width: 10%;">
                            Bank Code:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtbank_ac" runat="Server" CssClass="txt" TabIndex="2" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtbank_ac_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtbank_ac" runat="server" Text="..." OnClick="btntxtbank_ac_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblBank_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Mill Code:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtmill_code" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtmill_code_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtmill_code" runat="server" Text="..." OnClick="btntxtmill_code_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblMill_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtamount" runat="Server" CssClass="txt" TabIndex="4" Width="100px"
                                Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtamount_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filterAmount" runat="server" TargetControlID="txtamount"
                                FilterType="Custom,Numbers" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Utr No:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtutr_no" runat="Server" CssClass="txt" TabIndex="5" Width="300px"
                                Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtutr_no_TextChanged"
                                Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="width: 10%;">
                            Narration Header
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtnarration_header" runat="Server" CssClass="txt" TabIndex="6"
                                Width="300px" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtnarration_header_TextChanged"
                                Height="24px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="width: 10%;">
                            Narration Footer
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtnarration_footer" runat="Server" CssClass="txt" TabIndex="7"
                                Width="300px" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtnarration_footer_TextChanged"
                                Height="24px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="width: 10%;">
                            Is Save
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:CheckBox runat="server" ID="chkIsSave" Width="25px" Height="20px" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <table width="80%" align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            TabIndex="8" ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            TabIndex="9" ValidationGroup="save" OnClick="btnCancel_Click" Height="24px" />
                        &nbsp;<asp:Button runat="server" ID="btnUtrReport" Text="Print" Width="80px" Height="24px"
                            CssClass="btnHelp" OnClientClick="return UP();" />
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
