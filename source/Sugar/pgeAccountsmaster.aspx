<%@ Page Title="Account Master" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeAccountsmaster.aspx.cs" Inherits="pgeAccountsmaster"
    ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript" src="/Script/jquery-1.4.2.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtAC_NAME_E").focus(function () {
                $("#txtAC_NAME_E").animate({ width: "400px" });
            });
            $("#txtAC_NAME_E").blur(function () {
                $("#txtAC_NAME_E").css("width", "50px");
            });

        });
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
                    document.getElementById("<%=txtAC_CODE.ClientID %>").disabled = false;
                    document.getElementById("<%=txtAC_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtAC_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").disabled = false;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtSendingAcCode") {
                    document.getElementById("<%=txtSendingAcCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtSendingAcCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtCITY_CODE") {
                    document.getElementById("<%=txtCITY_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCITYNAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtCITY_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGROUP_CODE") {

                    document.getElementById("<%=txtGROUP_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblGROUPNAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGROUP_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGstStateCode") {
                    document.getElementById("<%=txtGstStateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtGstStateName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGstStateCode.ClientID %>").focus();
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
    <style type="text/css">
        .bagroundPopup
        {
            opacity: 0.6;
            background-color: Black;
        }
        
        #clientsDropDown
        {
            position: absolute;
            bottom: 0;
            width: 400px;
            background-color: Black;
            padding-bottom: 2%;
            z-index: 100;
        }
        #clientsOpen
        {
            background: url("images/open.png") no-repeat scroll 68px 10px #414142;
            color: #ececec;
            cursor: pointer;
            float: right;
            font-size: 26px;
            margin: -2px 0 0 10%;
            padding: 0 15px 2px;
            text-decoration: none;
            width: 63px;
        }
        #clientsCTA
        {
            background: #414142;
            width: 100%;
            color: #CCCCCC;
            text-align: center;
            font-size: 46px;
            margin: 0;
            padding: 30px 0;
            text-decoration: none;
        }
        #clientsDropDown .clientsClose
        {
            background-image: url(images/close.png);
        }
        #clientsDropDown #clientsDashboard
        {
            display: block;
        }
    </style>
    <script type="text/javascript">
        function validate() {
            var cityname = document.getElementById('<%=txtCityName.ClientID %>').value.trim();
            if (cityname == "") {
                alert('City Name Is Required!');
                document.getElementById('<%=txtCityName.ClientID %>').focus();
                $find("mpe").show();
            }
        }
    </script>
    <script type="text/javascript">
        function valid() {
            var Group = document.getElementById('<%=txtGroupName.ClientID %>').value.trim();
            if (Group == "") {
                alert('Group Name Is Required!');
                document.getElementById('<%=txtGroupName.ClientID %>').focus();
                $find("mpes").show();
            }
        }</script>
    <script type="text/javascript">
        function validationAll() {
            var drp = document.getElementById('<%=drpType.ClientID %>');
            var val = drp.options[drp.selectedIndex].value;
            if (val == "BR") {
                var shortname = document.getElementById('<%=txtSHORT_NAME.ClientID %>').value;
                if (shortname == "") {
                    alert('Short Name is Compulsory');
                    document.getElementById('<%=txtSHORT_NAME.ClientID %>').focus();
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    </script>
    <script type="text/javascript">
        function EnableDisable(val) {
            if (val == "T") {
                document.getElementById('<%=txtCOMMISSION.ClientID %>').disabled = true;
                document.getElementById('<%=txtADDRESS_E.ClientID %>').disabled = true;
                document.getElementById('<%=txtADDRESS_R.ClientID %>').disabled = true;
                //document.getElementById('<%=txtOPENING_BALANCE.ClientID %>').disabled = true;
                document.getElementById('<%=drpDrCr.ClientID %>').disabled = true;
                document.getElementById('<%=txtLOCAL_LIC_NO.ClientID %>').disabled = true;
                document.getElementById('<%=txtTIN_NO.ClientID %>').disabled = true;
                document.getElementById('<%=txtCST_NO.ClientID %>').disabled = true;
                document.getElementById('<%=txtGST_NO.ClientID %>').disabled = true;
                document.getElementById('<%=txtADDRESS_R.ClientID %>').disabled = true;
                document.getElementById('<%=txtBANK_OPENING.ClientID %>').disabled = true;
                document.getElementById('<%=drpBankDrCr.ClientID %>').disabled = true;
            }
            if (val == "O") {
                document.getElementById('<%=txtBANK_NAME.ClientID %>').disabled = true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
                float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
                border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Account Master   " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfCity" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table cellspacing="3">
                    <tr>
                        <td align="left">
                            Change No:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                TabIndex="0" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                            <%-- <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                    TargetControlID="txtEditDoc_No">
                                </ajax1:FilteredTextBoxExtender>--%>
                            &nbsp;
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Account Code:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtAC_CODE" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtAC_CODE_TextChanged"
                                    Height="22px"></asp:TextBox>
                                <asp:Button ID="btntxtAC_CODE" runat="server" Text="..." Width="80px" OnClick="btntxtAC_CODE_Click"
                                    CssClass="btnHelp" Height="22px" />
                            </td>
                            <td align="left" style="width: 10%;">
                                Type:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:DropDownList ID="drpType" runat="server" Width="200px" TabIndex="1" Height="25px"
                                    CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="drpType_SelectedIndexChanged">
                                    <asp:ListItem Text="Party" Value="P" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Supplier" Value="S"></asp:ListItem>
                                    <asp:ListItem Text="Bank" Value="B"></asp:ListItem>
                                    <asp:ListItem Text="Cash" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Relative" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Fixed Assets" Value="F"></asp:ListItem>
                                    <asp:ListItem Text="Interest Party" Value="I"></asp:ListItem>
                                    <asp:ListItem Text="Income/Expenses" Value="E"></asp:ListItem>
                                    <asp:ListItem Text="Trading" Value="O"></asp:ListItem>
                                    <asp:ListItem Text="Mill" Value="M"></asp:ListItem>
                                    <asp:ListItem Text="Transport" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="Broker" Value="BR"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" style="width: 10%;">
                                Interest Rate:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtAC_RATE" runat="Server" CssClass="txt" TabIndex="2" Width="80px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtAC_RATE_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Name of account:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtAC_NAME_E" runat="Server" CssClass="txt" TabIndex="3" Width="250px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtAC_NAME_E_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Regional Name:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtAC_NAME_R" runat="Server" CssClass="txt" TabIndex="4" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtAC_NAME_R_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td rowspan="6" colspan="2" align="center" valign="top">
                                <asp:Panel ID="pnlGroup" runat="server" ScrollBars="Auto" BorderColor="Navy" BorderWidth="1px"
                                    Width="350px" Height="200px">
                                    <asp:GridView ID="grdGroup" EmptyDataText="No Records Found" Width="90%" AllowPaging="false"
                                        runat="server" AutoGenerateColumns="false" Height="112px">
                                        <Columns>
                                            <asp:BoundField DataField="System_Code" HeaderText="code" ControlStyle-Width="5px"
                                                ItemStyle-Width="5px" HeaderStyle-Width="5px" ControlStyle-CssClass="invisible"
                                                ItemStyle-CssClass="invisible" HeaderStyle-CssClass="invisible" />
                                            <asp:BoundField DataField="System_Name_E" HeaderText="Group Name" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk" runat="server" Checked="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="MediumOrchid" ForeColor="White" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Commission Rate:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtCOMMISSION" runat="Server" CssClass="txt" TabIndex="5" Width="103px"
                                    Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtCOMMISSION_TextChanged"
                                    Height="22px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="filtertxtCOMMISSION" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtCOMMISSION">
                                </ajax1:FilteredTextBoxExtender>
                            </td>
                            <td align="left" style="width: 10%;">
                                Short Name:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtSHORT_NAME" runat="Server" CssClass="txtUpper" TabIndex="6" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtSHORT_NAME_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Address:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtADDRESS_E" runat="Server" CssClass="txt" TabIndex="7" Width="250px"
                                    TextMode="MultiLine" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtADDRESS_E_TextChanged"
                                    Height="50px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Address 2:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtADDRESS_R" runat="Server" CssClass="txt" TabIndex="8" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtADDRESS_R_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                City Code:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtCITY_CODE" runat="Server" CssClass="txt" TabIndex="9" Width="80px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCITY_CODE_TextChanged"
                                    Height="22px"></asp:TextBox>
                                <asp:Button ID="btntxtCITY_CODE" runat="server" Text="..." OnClick="btntxtCITY_CODE_Click"
                                    CssClass="btnHelp" Height="22px" Width="20px" />
                                <asp:Button runat="server" ID="btnAddCity" Text="Add New City" Width="100px" CssClass="btnHelp"
                                    OnClick="btnAddCity_Click" />
                                <asp:Label ID="lblCITYNAME" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                            <td align="left" style="width: 10%;">
                                Pin Code:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtPINCODE" runat="Server" CssClass="txt" TabIndex="10" Width="80px"
                                    Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtPINCODE_TextChanged"
                                    Height="22px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="rgdt" FilterType="Numbers" TargetControlID="txtPINCODE">
                                </ajax1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                GST State Code:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtGstStateCode" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                                    Height="24px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtGstStateCode_TextChanged"></asp:TextBox>
                                <asp:Button ID="btntxtGstStateCode" runat="server" Text="..." OnClick="btntxtGstStateCode_Click"
                                    Height="24px" Width="20px" CssClass="btnHelp" />
                                <asp:Label ID="lbltxtGstStateName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                            <td align="left" style="width: 10%;">
                                Distance:
                            </td>
                            <td>
                                <asp:TextBox ID="txtdistance" runat="Server" AutoPostBack="false" CssClass="txt"
                                    Height="22px" OnTextChanged="txtPINCODE_TextChanged" Style="text-align: right;"
                                    TabIndex="10" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Opening Balance:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtOPENING_BALANCE" runat="Server" CssClass="txt" TabIndex="11"
                                    Width="103px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtOPENING_BALANCE_TextChanged"
                                    Height="22px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender2" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtOPENING_BALANCE">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp; DRCR:
                                <asp:DropDownList ID="drpDrCr" runat="server" Width="80px" TabIndex="12" OnSelectedIndexChanged="drpDrCr_SelectedIndexChanged">
                                    <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="Credit" Value="C"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="left" colspan="2">
                                <table width="100%" align="center" cellspacing="4" style="table-layout: fixed;">
                                    <tr>
                                        <td align="left" style="width: 40%;">
                                            <asp:Label ID="lblBranch1" runat="server" Text="Branch1" CssClass="lblName"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 60%;">
                                            <asp:TextBox runat="server" ID="txtBranch1OB" CssClass="txt" Width="103px" TabIndex="13"
                                                Style="text-align: right;" AutoPostBack="true" Height="22px" OnTextChanged="txtBranch1OB_TextChanged"></asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender3" FilterType="Numbers,Custom"
                                                ValidChars="." TargetControlID="txtBranch1OB">
                                            </ajax1:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="drpBranch1Drcr" runat="server" Width="80px" AutoPostBack="true"
                                                TabIndex="14" OnSelectedIndexChanged="drpBranch1Drcr_SelectedIndexChanged">
                                                <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                                                <asp:ListItem Text="Credit" Value="C"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" style="width: 40%;">
                                            <asp:Label ID="lblBranch2" runat="server" Text="Branch2" CssClass="lblName"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 60%;">
                                            <asp:TextBox runat="server" ID="txtBranch2OB" CssClass="txt" Width="103px" TabIndex="15"
                                                AutoPostBack="true" Style="text-align: right;" Height="22px" OnTextChanged="txtBranch2OB_TextChanged"></asp:TextBox>
                                            <ajax1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender4" FilterType="Numbers,Custom"
                                                ValidChars="." TargetControlID="txtBranch2OB">
                                            </ajax1:FilteredTextBoxExtender>
                                            <asp:DropDownList ID="drpBranch2Drcr" runat="server" Width="80px" TabIndex="16" AutoPostBack="true"
                                                OnSelectedIndexChanged="drpBranch2Drcr_SelectedIndexChanged">
                                                <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                                                <asp:ListItem Text="Credit" Value="C"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Group Code:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtGROUP_CODE" runat="Server" CssClass="txt" TabIndex="17" Width="80px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtGROUP_CODE_TextChanged"
                                    Height="22px"></asp:TextBox>
                                <asp:Button ID="btntxtGROUP_CODE" runat="server" Text="..." OnClick="btntxtGROUP_CODE_Click"
                                    CssClass="btnHelp" Height="22px" Width="20px" />
                                <asp:Button runat="server" ID="btnAddGroup" Text="Add Group" Width="100px" CssClass="btnHelp"
                                    OnClick="btnAddGroup_Click1" />
                                <asp:Label ID="lblGROUPNAME" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                            <td align="left">
                                Is Carporate Party:
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="chkCarporate" runat="server" Checked="false" Text="Yes" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                Sugar Lic No:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtLOCAL_LIC_NO" runat="Server" CssClass="txt" TabIndex="18" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtLOCAL_LIC_NO_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Bank Name:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtBANK_NAME" runat="Server" CssClass="txt" TabIndex="24" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtBANK_NAME_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Ref By:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtRefBy" runat="server" Width="200px" CssClass="txt" TabIndex="30"
                                    Height="22px" OnTextChanged="txtRefBy_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                VAT/Tin No:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtTIN_NO" runat="Server" CssClass="txt" TabIndex="19" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtTIN_NO_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Bank IFSC Code:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtIfsc" runat="Server" CssClass="txt" TabIndex="25" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" Height="22px" OnTextChanged="txtIfsc_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Off. Phone:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtOffPhone" runat="server" Width="200px" CssClass="txt" TabIndex="31"
                                    Height="22px" OnTextChanged="txtOffPhone_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                CST No:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtCST_NO" runat="Server" CssClass="txt" TabIndex="20" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtCST_NO_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Bank Ac No:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtBANK_AC_NO" runat="Server" CssClass="txt" TabIndex="26" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtBANK_AC_NO_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Company Pan:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtcompanyPan" runat="server" Width="200px" CssClass="txt" TabIndex="32"
                                    OnTextChanged="txtcompanyPan_TextChanged" Height="22px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                GST:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtGST_NO" runat="Server" CssClass="txt" TabIndex="21" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtGST_NO_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Email:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtEMAIL_ID" runat="Server" CssClass="txt" TabIndex="27" Width="200px"
                                    ToolTip="You can add multiple email using comma after one emailid" Style="text-align: left;"
                                    AutoPostBack="false" OnTextChanged="txtEMAIL_ID_TextChanged" Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Fax:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtfax" runat="server" Width="200px" CssClass="txt" TabIndex="33"
                                    Height="22px" OnTextChanged="txtfax_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                ECC No:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtECC_NO" runat="Server" CssClass="txt" TabIndex="22" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtECC_NO_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                CC Email:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtEMAIL_ID_CC" runat="Server" CssClass="txt" TabIndex="28" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtEMAIL_ID_CC_TextChanged"
                                    Height="22px"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%;">
                                Mobile No.:
                            </td>
                            <td>
                                <asp:TextBox ID="txtMOBILE" runat="server" CssClass="txt" Width="200px" Style="text-align: left;"
                                    MaxLength="10" AutoPostBack="false" Height="22px" TabIndex="34" OnTextChanged="txtMOBILE_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                    TargetControlID="txtMOBILE">
                                </ajax1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%; vertical-align: top;">
                                FSSAI Lic No:
                            </td>
                            <td align="left" style="width: 10%; vertical-align: top;">
                                <asp:TextBox ID="txtFssaiNo" runat="Server" CssClass="txt" TabIndex="23" Width="200px"
                                    Style="text-align: left;" AutoPostBack="false" Height="22px" OnTextChanged="txtFssaiNo_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 10%; vertical-align: top;">
                                Other Narration:
                            </td>
                            <td align="left" style="width: 10%;">
                                <asp:TextBox ID="txtOTHER_NARRATION" runat="Server" CssClass="txt" TabIndex="29"
                                    TextMode="MultiLine" Width="200px" Style="text-align: left;" AutoPostBack="false"
                                    OnTextChanged="txtOTHER_NARRATION_TextChanged" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%; vertical-align: top;">
                                Unregister For GST:
                            </td>
                            <td align="left" style="width: 10%; vertical-align: top;">
                                <asp:CheckBox Text="" runat="server" ID="chkUnregisterGST" />
                            </td>
                             <td align="left" style="width: 10%; vertical-align: top;">
                            Is TDS:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:CheckBox Text="" runat="server" ID="chkIsTDS" />
                        </td>
                        </tr>
                        <asp:TextBox ID="txtBANK_OPENING" runat="Server" CssClass="txt" TabIndex="25" Width="100px"
                            Visible="false" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtBANK_OPENING_TextChanged"></asp:TextBox>
                        <ajax1:FilteredTextBoxExtender ID="filtertxtBANK_OPENING" runat="server" FilterType="Numbers"
                            TargetControlID="txtBANK_OPENING">
                        </ajax1:FilteredTextBoxExtender>
                        <asp:DropDownList Visible="false" ID="drpBankDrCr" runat="server" Width="100px" TabIndex="26"
                            AutoPostBack="True" OnSelectedIndexChanged="drpBankDrCr_SelectedIndexChanged">
                            <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                            <asp:ListItem Text="Credit" Value="C"></asp:ListItem>
                        </asp:DropDownList>
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="Add Contacts" CssClass="btnHelp"
                                    Width="90px" Height="25px" OnClick="btnOpenDetailsPopup_Click" TabIndex="35" />
                            </td>
                            <td>
                                DRCR DIFF:<asp:Label runat="server" ID="lblDRCRDiff"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td align="right">
                                <asp:TextBox runat="server" ID="txtSendingAcCode" AutoPostBack="true" CssClass="txt"
                                    Width="60px" Height="24px" OnTextChanged="txtSendingAcCode_TextChanged"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button ID="btntxtSendingAcCode" runat="server" Text="..." CssClass="btnHelp"
                                    Height="24px" Width="20px" OnClick="btntxtSendingAcCode_Click" />&nbsp;<asp:Label
                                        runat="server" ID="lblSendingAcCode" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 100%; position: relative; top: 0px; left: 0px;">
                <%-- <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>--%>
                <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="120px" Width="1000px"
                    BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                    Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                    <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                        HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="100%"
                        OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound"
                        Style="table-layout: fixed;" OnRowCreated="grdDetail_RowCreated">
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
                <%--    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <%-- <asp:TextBox ID="TextBox1" runat="Server" CssClass="txt" TabIndex="0" Width="80px"
                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtAC_CODE_TextChanged"
                    Height="22px"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="..." Width="80px" OnClick="btntxtAC_CODE_Click"
                    CssClass="btnHelp" Height="22px" />--%>
                <!-- //clientsDropDown -->
                <%--<div style="width: 200px;height:100px; float: right; margin-bottom: 0px;background-color:White;"></div>--%>
                <br />
                <asp:TextBox Style="margin-left: -40px" ReadOnly="false" ToolTip="Mobile" runat="server"
                    ID="txtSendingMobile" Width="120px" CssClass="txt" Height="24px"></asp:TextBox>&nbsp;
                <asp:TextBox ReadOnly="false" ToolTip="Email" runat="server" ID="txtSendingEmail"
                    Width="120px" CssClass="txt" Height="24px"></asp:TextBox><br />
                <br />
                <asp:CheckBox runat="server" ID="chkAddressDetails" Text="Address" AutoPostBack="true"
                    TextAlign="Left" Font-Bold="true" OnCheckedChanged="chkAddressDetails_CheckedChanged" />&nbsp;<asp:CheckBox
                        AutoPostBack="true" runat="server" ID="chkBankDetails" Text="Bank Details" TextAlign="Left"
                        Font-Bold="true" OnCheckedChanged="chkBankDetails_CheckedChanged" />
                <br />
                <asp:Button Text="SMS" ID="btnSendSMS" CommandName="sms" CssClass="btnHelp" Height="24px"
                    Width="80px" runat="server" OnCommand="btnSendSMS_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Button Text="E-Mail" ID="btnEmail" CssClass="btnHelp" Height="24px" Width="80px"
                    CommandName="email" runat="server" OnCommand="btnSendSMS_Click" />
            </div>
            <table width="100%" align="left">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="22px" />
                        &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            OnClientClick="return validationAll();" ValidationGroup="add" OnClick="btnSave_Click"
                            Height="22px" TabIndex="43" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnEdit_Click" Height="22px" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="22px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnCancel_Click" Height="22px" />
                    </td>
                    <td align="center">
                        <asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnFirst_Click" Width="90px" Height="22px" />
                        <asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnPrevious_Click" Width="90px" Height="22px" />
                        <asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnNext_Click" Width="90px" Height="22px" />
                        <asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnLast_Click" Width="90px" Height="22px" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="70%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px;
                min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center;
                left: 10%; top: 10%;">
                <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                    OnClick="imgBtnClose_Click" Width="20px" Height="20px" Style="float: right; vertical-align: top;"
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
                                Width="250px" Height="20px" AutoPostBack="false" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search"
                                CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="30" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                    OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
                                    <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                                    <RowStyle Height="25px" Width="100px" ForeColor="Black" Wrap="true" />
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
                <table width="80%" align="center" cellspacing="4">
                    <tr>
                        <td colspan="2" align="center" style="background-color: Silver; height: 15px; color: White;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Contact Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblID" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblNo" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Person Name:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtPERSON_NAME" runat="Server" CssClass="txt" TabIndex="36" Width="250px"
                                Height="22px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtPERSON_NAME_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Mobile:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtPERSON_MOBILE" runat="Server" CssClass="txt" TabIndex="37" Width="200px"
                                Height="22px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtPERSON_MOBILE_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Email:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtPERSON_EMAIL" runat="Server" CssClass="txt" TabIndex="38" Width="200px"
                                Height="22px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtPERSON_EMAIL_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            Pan:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtPerson_PAN" runat="Server" CssClass="txt" TabIndex="39" Width="200px"
                                Height="22px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtPerson_PAN_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">
                            OTHER:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtPERSON_OTHER" runat="Server" CssClass="txt" TabIndex="40" Width="200px"
                                Height="60px" TextMode="MultiLine" Style="text-align: left;" AutoPostBack="false"
                                OnTextChanged="txtPERSON_OTHER_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnSubmit" Width="80px"
                                Height="25px" OnClick="btnAdddetails_Click" TabIndex="41" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnSubmit"
                                Width="80px" Height="25px" OnClick="btnClosedetails_Click" TabIndex="42" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div id="pnlCity" class="city" style="display: none;">
                <asp:ImageButton runat="server" ID="imgClose" ImageUrl="~/Images/closebtn.jpg" Width="20px"
                    Height="20px" Style="float: right; vertical-align: top;" ToolTip="Close" OnClick="imgClose_Click" />
                <table cellspacing="7">
                    <tr>
                        <td colspan="2" align="center">
                            <h3 style="color: White; margin-top: 2px;">
                                CITY MASTER
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblCityCode" ForeColor="White" Text="City Code:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtCityCode" Height="24px" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label2" ForeColor="White" Text="City Name:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtCityName" CssClass="txtUpper" Height="24px" Width="200px"
                                onkeyup="javascript:onfocus();"></asp:TextBox>
                            <asp:Label runat="server" ID="lblErr" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label3" ForeColor="White" Text="Regional Name:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtRegionalName" CssClass="txt" Height="24px" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label4" ForeColor="White" Text="State:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtState" CssClass="txtUpper" Height="24px" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button runat="server" ID="btnSaveCity" Text="SAVE" CssClass="button" OnClick="btnSaveCity_Click" />
                        </td>
                        <td align="center">
                            <asp:Button runat="server" ID="btnCancelCity" Text="Cancel" CssClass="button" />
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btn12" Style="display: none;" />
                <ajax1:ModalPopupExtender ID="modalCity" BackgroundCssClass="bagroundPopup" TargetControlID="btn12"
                    BehaviorID="mpe" PopupControlID="pnlCity" runat="server">
                </ajax1:ModalPopupExtender>
            </div>
            <div id="BSGroup" class="city" style="display: none;">
                <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/Images/closebtn.jpg"
                    Width="20px" Height="20px" Style="float: right; vertical-align: top;" ToolTip="Close"
                    OnClick="ImageButton1_Click" />
                <table cellspacing="7">
                    <tr>
                        <td colspan="2" align="center">
                            <h3 style="color: White; margin-top: 2px;">
                                Balance Sheet Group Master
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label5" ForeColor="White" Text="Group Code:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtGroupCode" Height="24px" Width="80px" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label6" ForeColor="White" Text="Group Name:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtGroupName" Height="24px" Width="200px"></asp:TextBox><asp:Label
                                runat="server" ID="lblGrr" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label8" ForeColor="White" Text="Group Summary:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpGroupSummary" runat="server" CssClass="ddl" Width="100px"
                                TabIndex="3" Height="24px" OnSelectedIndexChanged="drpGroupSummary_SelectedIndexChanged">
                                <asp:ListItem Text="Yes" Value="Y" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label9" ForeColor="White" Text="Group Section:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpgroupSection" runat="server" CssClass="ddl" Width="200px"
                                TabIndex="4" Height="24px" OnSelectedIndexChanged="drpgroupSection_SelectedIndexChanged">
                                <asp:ListItem Text="Trading" Value="T" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Profit & Loss" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Balance Sheet" Value="B"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label7" ForeColor="White" Text="Group Order:" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtGroupOrder" Height="24px" Width="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button runat="server" ID="btnSaveGroup" Text="SAVE" CssClass="button" OnClick="btnSaveGroup_Click" />
                        </td>
                        <td align="center">
                            <asp:Button runat="server" ID="Button2" Text="Cancel" CssClass="button" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label runat="server" ID="lblGropCodeexist" ForeColor="Yellow"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btn13" Style="display: none;" />
                <ajax1:ModalPopupExtender ID="ModalGroupMaster" BackgroundCssClass="bagroundPopup"
                    TargetControlID="btn13" PopupControlID="BSGroup" runat="server" BehaviorID="mpes">
                </ajax1:ModalPopupExtender>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
