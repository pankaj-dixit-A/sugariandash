<%@ Page Title="DO Prints" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeDOPrint.aspx.cs" Inherits="Report_pgeDOPrint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../JS/select all.js"></script>
    <script type="text/javascript">
        function doprint(do_no, email) {
            window.open('rptDeliveryOrder.aspx?do_no=' + do_no + '&email=' + email);
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

                if (hdnfClosePopupValue == "txtmillCode") {
                    document.getElementById("<%=txtmillCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblmillname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
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
            <asp:Label ID="label1" runat="server" Text="   Delivery Order Print   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:HiddenField ID="hdnfClosePopup" runat="server" />
    <asp:HiddenField ID="hdHelpPageCount" runat="server" />
    <asp:Panel ID="pnlMain" runat="server">
        <table width="70%">
            <tr>
                <td align="left">
                    From Date:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                        MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                    <asp:Image ID="imgtxtFromDt" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                        Width="25px" Height="15px" TabIndex="4" />
                    <ajax1:CalendarExtender ID="CalendarExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                        PopupButtonID="imgtxtFromDt" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                    To:
                    <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                        MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                    <asp:Image ID="imgtxtToDt" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                        Width="25px" Height="15px" TabIndex="4" />
                    <ajax1:CalendarExtender ID="CalendarExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                        PopupButtonID="imgtxtToDt" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td align="left">
                    Mill :
                </td>
                <td align="left">
                    <asp:TextBox ID="txtmillCode" runat="server" Width="100px" CssClass="txt" Height="24px"
                        AutoPostBack="true" OnTextChanged="txtmillCode_TextChanged"></asp:TextBox>
                    <asp:Button ID="btnmillcode" runat="server" Text="..." Width="20px" CssClass="btnHelp"
                        Height="24px" OnClick="btnmillcode_Click" />
                    <asp:Label ID="lblmillname" runat="server" CssClass="lblName"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnGet" runat="server" Text="Get Data" CssClass="btnHelp" Width="80px"
                        Height="24px" OnClick="btnGet_Click" />
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
                    <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                        Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                        <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" EmptyDataText="No Records Found"
                            HeaderStyle-BackColor="#6D8980" HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated"
                            OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
                            <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                            <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlGrid" runat="server" Width="1200px" Height="400px" BorderColor="GrayText"
        BorderWidth="1px" ScrollBars="Both" BackColor="White">
        <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#397CBB"
            HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" EmptyDataText="No Records found"
            CellPadding="5" CellSpacing="5" Font-Bold="true" ForeColor="Black" Font-Names="Verdana"
            Font-Size="12px" Style="overflow: hidden; table-layout: fixed;" OnRowDataBound="grdDetail_RowDataBound">
            <Columns>
                <asp:BoundField DataField="No" HeaderText="No" ControlStyle-Width="10px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Type" HeaderText="Type" ControlStyle-Width="10px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="DO_Date" HeaderText="Voucher_Date" ControlStyle-Width="15px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Party Name" HeaderText="Party Name" ControlStyle-Width="100px"
                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="Lorry" HeaderText="Lorry No" ControlStyle-Width="400px"
                    ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="quantal" HeaderText="Quantal" ControlStyle-Width="15px"
                    ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="mill_rate" HeaderText="Mill_Rate" ControlStyle-Width="15px"
                    ItemStyle-HorizontalAlign="Right" />
                <%--<asp:BoundField DataField="Email" HeaderText="Email" ControlStyle-Width="10px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="" HeaderText="Mobile" ControlStyle-Width="10px" ItemStyle-HorizontalAlign="Left" />--%>
                <asp:TemplateField HeaderText="Is Print">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" Checked="false" AutoPostBack="true"
                            OnCheckedChanged="chkselectAll_checkchanged" OnClick="selectAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkIsPrint" runat="server" Checked="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <%--  <asp:BoundField DataField="" HeaderText="Status" NullDisplayText="N" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Mill_Code" HeaderText="millID" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Left" />--%>
            </Columns>
            <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
        </asp:GridView>
    </asp:Panel>
    <div align="center" style="font-family: Calibri; font-weight: bold; color: Black;
        width: 1300px; font-size: 14px;">
        Allow Page Break:&nbsp;<asp:CheckBox ID="chkPageBreak" runat="server" Checked="false"
            Text="Yes/No" />
    </div>
    <asp:Button ID="btnMail" Text="Mail" Width="80px" CssClass="btnHelp" runat="server"
        Visible="false" OnClick="btnMail_Click" Height="24px" />
    <asp:Button ID="btnPrint" Text="Print" Width="80px" CssClass="btnHelp" runat="server"
        OnClick="btnPrint_Click" Height="24px" />
    <iframe id="ifrDO" runat="server" width="50px" height="50px" visible="false"></iframe>
</asp:Content>
