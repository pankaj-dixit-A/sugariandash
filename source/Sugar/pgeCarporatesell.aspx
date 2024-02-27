<%@ Page Title="Carporate Sell" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeCarporatesell.aspx.cs" Inherits="pgeCarporatesell" %>

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


                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;

                    document.getElementById("<%=txtac_code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtac_code") {
                    document.getElementById("<%=txtac_code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblParty_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtunit_code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtunit_code") {
                    document.getElementById("<%=txtunit_code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUnit_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBroker.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBroker") {
                    document.getElementById("<%=txtBroker.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBroker.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtpodetail.ClientID %>").focus();
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
            <asp:Label ID="label1" runat="server" Text="   Carporate Sale   " Font-Names="verdana"
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
                <table width="70%" align="center" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Entry No:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" TabIndex="0" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                    CssClass="btnHelp" Height="24px" />
                            </td>
                            <td align="left" style="width: 10%;">
                                Date:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtdoc_date" runat="Server" CssClass="txt" TabIndex="1" Width="90px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtdoc_date_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="calenderExtendertxtdoc_date" runat="server" TargetControlID="txtdoc_date"
                                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Selling Type:
                            </td>
                            <td align="left">
                                <asp:DropDownList runat="server" ID="drpSellingType" CssClass="ddl" Height="24px"
                                    AutoPostBack="true" TabIndex="2" Width="200px" OnSelectedIndexChanged="drpSellingType_SelectedIndexChanged">
                                    <asp:ListItem Text="Carporate Sell" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="PDS Sell" Value="P"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Party:
                            </td>
                            <td align="left" style="width: 10%;" colspan="3">
                                <asp:TextBox ID="txtac_code" runat="Server" CssClass="txt" TabIndex="3" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtac_code_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtac_code" runat="server" Text="..." OnClick="btntxtac_code_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblParty_name" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Unit Name:
                            </td>
                            <td align="left" style="width: 10%;" colspan="3">
                                <asp:TextBox ID="txtunit_code" runat="Server" CssClass="txt" TabIndex="4" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtunit_code_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtunit_code" runat="server" Text="..." OnClick="btntxtunit_code_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblUnit_name" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Broker:
                            </td>
                            <td align="left" style="width: 10%;" colspan="3">
                                <asp:TextBox ID="txtBroker" runat="Server" CssClass="txt" TabIndex="5" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtBroker_TextChanged"></asp:TextBox>
                                <asp:Button ID="btntxtBroker" runat="server" Text="..." CssClass="btnHelp" Height="24px"
                                    Width="20px" OnClick="btntxtBroker_Click" />
                                <asp:Label ID="lblBroker" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                P.O.Detail:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtpodetail" runat="Server" CssClass="txt" TabIndex="6" Width="200px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtpodetail_TextChanged"
                                    Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Quantal:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtquantal" runat="Server" CssClass="txt" TabIndex="7" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtquantal_TextChanged"
                                    Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Sell Rate:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtsell_rate" runat="Server" CssClass="txt" TabIndex="8" Width="90px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtsell_rate_TextChanged"
                                    Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Remark:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtasn_no" runat="Server" CssClass="txt" TabIndex="9" Width="200px"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtasn_no_TextChanged"
                                    Height="24px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                    Width="80px" Height="24px" OnClick="btnOpenDetailsPopup_Click" TabIndex="10" />
                            </td>
                            <td align="left">
                                Diff:<asp:Label runat="server" ID="lblQntlDiff" ForeColor="Yellow"></asp:Label>
                            </td>
                            <%--<tr>
<td colspan="4">
-------------------------------------------  SCHEDULE --------------------------------------
</td>
</tr>
<tr>
<td align="left">
Date:
</td>
<td align="left" style="width:10%;">
<asp:TextBox ID="txtdt1" runat="Server" CssClass="txt" TabIndex="8" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtdt1_no_TextChanged"></asp:TextBox>
   <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" 
        Width="25px" Height="15px" />
<ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdt1" PopupButtonID="Image1"
 Format="dd/MM/yyyy"></ajax1:CalendarExtender>
</td>
<td align="left">
Quantal:
</td>
<td align="left">
<asp:TextBox ID="txtqtl1" runat="Server" CssClass="txt" TabIndex="9" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtqtl1_TextChanged"></asp:TextBox>
 <ajax1:FilteredTextBoxExtender ID="ft1" runat="server" TargetControlID="txtqtl1" FilterType="Numbers,Custom" ValidChars="."></ajax1:FilteredTextBoxExtender>
</td>
</tr>
<tr>
<td align="left">
Date:
</td>
<td align="left" style="width:10%;">
<asp:TextBox ID="txtdt2" runat="Server" CssClass="txt" TabIndex="10" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtdt2_TextChanged"></asp:TextBox>
    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendar_icon1.png" 
        Width="25px" Height="15px" />
<ajax1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtdt2" PopupButtonID="Image2"
 Format="dd/MM/yyyy"></ajax1:CalendarExtender>
</td>
<td align="left">
Quantal:
</td>
<td align="left">
<asp:TextBox ID="txtqtl2" runat="Server" CssClass="txt" TabIndex="11" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtqtl2_TextChanged"></asp:TextBox>
 <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtqtl2" FilterType="Numbers,Custom" ValidChars="."></ajax1:FilteredTextBoxExtender>
</td>
</tr>
<tr>
<td align="left">
Date:
</td>
<td align="left" style="width:10%;">
<asp:TextBox ID="txtdt3" runat="Server" CssClass="txt" TabIndex="12" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtdt3_TextChanged"></asp:TextBox>
     <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/calendar_icon1.png" 
        Width="25px" Height="15px" />
<ajax1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtdt3" PopupButtonID="Image3"
 Format="dd/MM/yyyy"></ajax1:CalendarExtender>
</td>
<td align="left">
Quantal:
</td>
<td align="left">
<asp:TextBox ID="txtqtl3" runat="Server" CssClass="txt" TabIndex="13" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtqtl3_TextChanged"></asp:TextBox>
 <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtqtl3" FilterType="Numbers,Custom" ValidChars="."></ajax1:FilteredTextBoxExtender>
</td>
</tr>
<tr>
<td align="left">
Date:
</td>
<td align="left" style="width:10%;">
<asp:TextBox ID="txtdt4" runat="Server" CssClass="txt" TabIndex="14" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtdt4_TextChanged"></asp:TextBox>
      <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/calendar_icon1.png" 
        Width="25px" Height="15px" />
<ajax1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtdt4" PopupButtonID="Image4"
 Format="dd/MM/yyyy"></ajax1:CalendarExtender>
</td>
<td align="left">
Quantal:
</td>
<td align="left">
<asp:TextBox ID="txtqtl4" runat="Server" CssClass="txt" TabIndex="15" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtqtl4_TextChanged"></asp:TextBox>
 <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtqtl4" FilterType="Numbers,Custom" ValidChars="."></ajax1:FilteredTextBoxExtender>
</td>
</tr>
<tr>
<td align="left">
Date:
</td>
<td align="left" style="width:10%;">
<asp:TextBox ID="txtdt5" runat="Server" CssClass="txt" TabIndex="16" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtdt5_TextChanged"></asp:TextBox>
       <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/calendar_icon1.png" 
        Width="25px" Height="15px" />
<ajax1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtdt5" PopupButtonID="Image5"
 Format="dd/MM/yyyy"></ajax1:CalendarExtender>
</td>
<td align="left">
Quantal:
</td>
<td align="left">
<asp:TextBox ID="txtqtl5" runat="Server" CssClass="txt" TabIndex="17" Width="80px" style="text-align:left;"
 AutoPostBack="True" ontextchanged="txtqtl5_TextChanged"></asp:TextBox>
 <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtqtl5" FilterType="Numbers,Custom" ValidChars="."></ajax1:FilteredTextBoxExtender>
 <asp:TextBox ID="txtqtlTotal" runat="server" ReadOnly="true" Font-Bold="true" Width="80px" CssClass="txt" ForeColor="DarkBlue"></asp:TextBox>
</td>
</tr>--%>
                </table>
            </asp:Panel>
            <div style="width: 100%; position: relative;" align="center">
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="150px" Width="70%"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: none;">
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
            <table width="80%" align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" TabIndex="17" Text="Save" CssClass="btnHelp"
                            Width="90px" ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnCancel_Click" Height="24px" />
                    </td>
                    <td align="center">
                        &nbsp;
                        <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnFirst_Click" Width="90px" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnPrevious_Click" Width="90px" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnNext_Click" Width="90px" Height="24px" />
                        &nbsp;
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
                                    HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                    OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
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
                <table width="80%" align="center" cellpadding="3" cellspacing="5">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;
                            height: 30px;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Schedule Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ID&nbsp;<asp:Label ID="lblID" runat="server"></asp:Label>
                        </td>
                        <td>
                            NO&nbsp;<asp:Label ID="lblNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Schedule Date:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtSCDate" runat="Server" CssClass="txt" TabIndex="11" Width="90px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Height="24px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtSCDate_TextChanged"></asp:TextBox>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtSCDate"
                                PopupButtonID="Image1" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            <%--<asp:RequiredFieldValidator runat="server" ID="rfv1" ControlToValidate="txtSCDate"
                                    ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Quantal:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtSCQuantal" runat="Server" CssClass="txt" TabIndex="12" Width="90px"
                                Height="24px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtSCQuantal_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilterertxtSCQuantal" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtSCQuantal">
                            </ajax1:FilteredTextBoxExtender>
                            <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtSCQuantal"
                                ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Transit days:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtTransitDays" runat="Server" CssClass="txt" TabIndex="13" Width="90px"
                                Height="24px" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtTransitDays_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilterertxtTransitDays" runat="server" FilterType="Numbers"
                                TargetControlID="txtTransitDays">
                            </ajax1:FilteredTextBoxExtender>
                            <%--   <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtTransitDays"
                                ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Remind Date:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtRemindDate" runat="Server" CssClass="txt" TabIndex="14" Width="90px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Height="24px" ReadOnly="true" Style="text-align: right;" OnTextChanged="txtRemindDate_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnSubmit" Width="80px"
                                Height="24px" OnClick="btnAdddetails_Click" TabIndex="15" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnSubmit"
                                Height="24px" Width="80px" OnClick="btnClosedetails_Click" TabIndex="16" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
