<%@ Page Title="Fill Opening Balance" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeFillOpeningBalance.aspx.cs" Inherits="Sugar_pgeFillOpeningBalance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure want to Update Data?")) {
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
                    document.getElementById("<%=lblAc_Name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtAC_CODE.ClientID %>").focus();
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
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); width: 90%; margin-left: 30px;
                float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 5px; border-left: 0px;
                border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="   Fill Opening Balance   " Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfconfirm" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <br />
            <table width="700px" align="center" cellspacing="15">
                <tr>
                    <td align="left" style="width: 80%;">
                        &nbsp;&nbsp;&nbsp;&nbsp;<b>Ac_Code:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox runat="server" ID="txtAC_CODE" CssClass="txt" Width="80px" Style="text-align: right;"
                            TabIndex="1" AutoPostBack="True" Height="22px" OnTextChanged="txtAC_CODE_TextChanged"></asp:TextBox>
                        <asp:Button ID="btntxtAC_CODE" runat="server" Text="..." Width="25px" OnClick="btntxtAC_CODE_Click"
                            CssClass="btnHelp" Height="22px" />
                        <asp:Label runat="server" ID="lblAc_Name" CssClass="lblName"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Opening Balance:</b><asp:TextBox runat="server" ID="txtOpeningBalance" CssClass="txt"
                            TabIndex="2" Width="100px" Height="24px"></asp:TextBox><ajax1:FilteredTextBoxExtender
                                runat="server" ID="rdf2" FilterType="Numbers,Custom" TargetControlID="txtOpeningBalance"
                                ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        &nbsp;<b>DrCr:</b>
                        <asp:DropDownList ID="drpDrCr" runat="server" Width="80px" TabIndex="3" OnSelectedIndexChanged="drpDrCr_SelectedIndexChanged">
                            <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                            <asp:ListItem Text="Credit" Value="C"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button runat="server" ID="btnUpdate" Text="UPDATE" CssClass="btnHelp" Width="80px"
                            TabIndex="4" Height="24px" OnClick="btnUpdate_Click" OnClientClick="Confirm()" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);"  runat="server" Width="70%" align="center" ScrollBars="None"
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
                            <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);"  runat="server" Width="250px" Height="20px" AutoPostBack="true"
                                OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);"  runat="server" Text="Search" CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select"  runat="server" AutoGenerateColumns="true" AllowPaging="true"
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
