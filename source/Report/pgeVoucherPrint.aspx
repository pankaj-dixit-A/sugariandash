<%@ Page Title="Voucher Print" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeVoucherPrint.aspx.cs" Inherits="Report_pgeVoucherPrint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script type="text/javascript" language="javascript">
    function getSrc() {
        alert('script');
        document.getElementById("<%=ifrVoucher.ClientID %>").src = "http://localhost:2487/AccoWeb/Report/rptVoucher.aspx";        
    }
</script>--%>
    <script type="text/javascript" src="../JS/select all.js"></script>
    <link href="../CSS/Grid.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">

        function sp(VNO, mailID, pageBreak, type) {
            var tn;
            window.open('../Report/rptVouchersNew.aspx?VNO=' + VNO + '&mailID=' + mailID + '&pageBreak=' + pageBreak + '&type=' + type, '_blank');    //R=Redirected  O=Original
        }

    </script>
    <script type="text/javascript">
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


                var pageCount = document.getElementById("<%=hdnfPageCount.ClientID %>").value;
                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }

                if (hdnfClosePopupValue == "txtParty") {
                    document.getElementById("<%=txtPary.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblPartyName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
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
            <asp:Label ID="label1" runat="server" Text="   Voucher Print   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <%--
<asp:UpdatePanel ID="upp" runat="server">
<ContentTemplate>--%>
    <asp:HiddenField ID="hdnfClosePopup" runat="server" />
    <asp:HiddenField ID="hdnfPageCount" runat="server" />
    <asp:TextBox ID="txtFrameSrc" runat="server" Width="300px" Height="24px" Visible="false"></asp:TextBox>
    <%--<asp:Panel ID="pnlFilter" runat="server" align="center"  Font-Bold="true" Font-Size="12px" >--%>
    <table width="80%" align="center" cellpadding="5px" cellspacing="5px">
        <tr>
            <td align="left">
                <asp:DropDownList ID="drpVoucherType" runat="server" Width="150px" CssClass="ddl"
                    Height="24px">
                    <asp:ListItem Text="Local Voucher" Value="LV"></asp:ListItem>
                    <asp:ListItem Text="Loading Voucher" Value="OV"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td valign="middle" align="left">
                <asp:DropDownList ID="drpFilter" runat="server" CssClass="ddl" Width="200px" Height="24px"
                    AutoPostBack="True" OnSelectedIndexChanged="drpFilter_SelectedIndexChanged">
                    <asp:ListItem Text="Date Wise" Value="D"></asp:ListItem>
                    <asp:ListItem Text="Voucher Number Wise" Value="V"></asp:ListItem>
                    <asp:ListItem Text="Party Wise" Value="P"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
            </td>
            <td align="center">
                <asp:Panel ID="pnldate" runat="server" Width="500px">
                    From:
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
                </asp:Panel>
                <asp:Panel ID="pnlVNo" runat="server" Width="500px">
                    From:
                    <asp:TextBox ID="txtVNoFrom" runat="server" Width="80px" CssClass="txt" Height="24px"></asp:TextBox>
                    To:
                    <asp:TextBox ID="txtVNoTo" runat="server" Width="80px" CssClass="txt" Height="24px"></asp:TextBox>
                </asp:Panel>
                <asp:Panel ID="pnlParty" runat="server" Width="500px">
                    <table>
                        <tr align="left">
                            <td colspan="4">
                                Party:
                                <asp:TextBox ID="txtPary" runat="server" Width="80px" CssClass="txt" AutoPostBack="true"
                                    Height="24px" OnTextChanged="txtPary_TextChanged"></asp:TextBox>
                                <asp:Button ID="btnParty" runat="server" Text="..." CssClass="btnHelp" OnClick="btnParty_Click"
                                    Height="24px" Width="20px" />
                                <asp:Label ID="lblPartyName" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                From:
                                <asp:TextBox ID="txtDtFrom1" runat="server" Width="80px" CssClass="txt" AutoPostBack="True"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    OnTextChanged="txtDtFrom1_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Image ID="imgtxtDtFrom1" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" TabIndex="4" />
                                <ajax1:CalendarExtender ID="CalendarExtendertxtDtFrom1" runat="server" TargetControlID="txtDtFrom1"
                                    PopupButtonID="imgtxtDtFrom1" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                                &nbsp;&nbsp; To:&nbsp;&nbsp;
                                <asp:TextBox ID="txtDtTo1" runat="server" Width="80px" CssClass="txt" Height="24px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                                <asp:Image ID="imgtxtDtTo1" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" TabIndex="4" />
                                <ajax1:CalendarExtender ID="CalendarExtendertxtDtTo1" runat="server" TargetControlID="txtDtTo1"
                                    PopupButtonID="imgtxtDtTo1" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
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
                                    <asp:GridView ID="grdPopup" runat="server" AutoGenerateColumns="true" AllowPaging="true"
                                        PageSize="20" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                        HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                                        Style="table-layout: fixed;" OnPageIndexChanging="grdPopup_PageIndexChanging">
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
            </td>
            <td>
                <asp:Button ID="btnGet" runat="server" Text="Get" Width="80px" CssClass="btnHelp"
                    OnClick="btnGet_Click" Height="24px" />
            </td>
        </tr>
    </table>
    <%--
</asp:Panel>--%>
    <asp:Panel ID="pnlGrid" runat="server" Width="1300px" ScrollBars="Both" Height="500px"
        BorderColor="GrayText" BorderWidth="1px" BackColor="White">
        <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#397CBB"
            HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" EmptyDataText="No Records found"
            Width="100%" CellPadding="5" CellSpacing="5" Font-Bold="true" ForeColor="Black"
            Font-Names="Verdana" Font-Size="12px" Style="overflow: hidden; table-layout: fixed;"
            OnRowDataBound="grdDetail_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Doc_No" HeaderText="Voch.No" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Voucher_Date" HeaderText="Voucher Date" ControlStyle-Width="15px"
                    ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Party Name" HeaderText="Party Name" ControlStyle-Width="100px"
                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Mill Name" HeaderText="Mill Name" ControlStyle-Width="200px"
                    ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Quantal" HeaderText="Quantal" ControlStyle-Width="15px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Mill_Rate" HeaderText="Mill_Rate" ControlStyle-Width="15px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Voucher_Amount" HeaderText="Voucher_Amt" ControlStyle-Width="20px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Email_Id" HeaderText="Email_Id" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="PartyMobile" HeaderText="Mobile" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="Is Print">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" Checked="false" AutoPostBack="true"
                            OnCheckedChanged="chkselectAll_checkchanged" OnClick="selectAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkIsPrint" runat="server" Checked="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="" HeaderText="Status" NullDisplayText="N" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Mill_Code" HeaderText="millID" ControlStyle-Width="10px"
                    ItemStyle-HorizontalAlign="Left" />
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
        OnClick="btnMail_Click" Height="24px" Visible="false" />
    <asp:Button runat="server" ID="btnMailToEach" Width="80px" Height="24px" CssClass="btnHelp"
        OnClick="btnMailToEach_Click" Text="Mail" />
    <asp:Button ID="btnPrint" Text="Print" Width="80px" CssClass="btnHelp" runat="server"
        OnClick="btnPrint_Click" Height="24px" />
    <iframe id="ifrVoucher" runat="server" width="50px" height="50px" visible="false">
    </iframe>
    <div style="display: none;">
        <asp:Panel runat="server" ID="pnlMain">
            <asp:DataList runat="server" ID="dtlDetails" Width="100%" OnItemDataBound="dtlDetails_OnItemDataBound">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblDocno" Visible="false" Text='<%#Eval("Doc_No") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:Label runat="server" ID="lbltype" Visible="false" Text='<%#Eval("Tran_Type") %>'
                        CssClass="lbl"></asp:Label>
                    <asp:DataList runat="server" ID="dtl" Width="100%" OnItemDataBound="dtl_OnItemDataBound">
                        <ItemTemplate>
                            <table id="tbMain" runat="server" align="center" style="table-layout: fixed;" width="1000px"
                                class="print">
                                <tr>
                                    <td style="width: 100%;" colspan="2">
                                        <table width="100%" style="table-layout: fixed; height: 125px;" class="print9pt">
                                            <tr>
                                                <td style="width: 20%; vertical-align: top;" align="center">
                                                    <asp:Image runat="server" ID="imgLogo" ImageUrl="~/Images/Logo.jpg" Width="100%"
                                                        Height="30%" />
                                                </td>
                                                <td style="width: 80%; vertical-align: top;" align="left">
                                                    <table width="100%" style="table-layout: fixed;">
                                                        <tr>
                                                            <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                                                <asp:Label ID="lblCompany" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; text-transform: uppercase; color: Red;" class="print9pt">
                                                                <asp:Label ID="Label5" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                                                <asp:Label runat="server" ID="lblAl1" ForeColor="Blue"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                                                <asp:Label runat="server" ID="lblAl2" ForeColor="Blue"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                                                <asp:Label runat="server" ID="lblAl3" ForeColor="Blue"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                                                <asp:Label runat="server" ID="lblAl4" ForeColor="Blue"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100%; font-family: Verdana;" class="toosmall">
                                                                <asp:Label runat="server" ID="lblOtherDetails" ForeColor="Blue"> </asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                        Voucher No:<asp:Label runat="server" ID="lblVoucherNo" Font-Bold="true" Text='<%#Eval("VoucherNo") %>'></asp:Label>
                                        <asp:Label ID="lblDocNo" runat="server" Text='<%#Eval("Doc_No") %>' Font-Bold="true"></asp:Label>
                                    </td>
                                    <td align="right" style="border-top: solid 1px black; border-bottom: solid 1px black;">
                                        Date:
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Doc_Date") %>' Font-Bold="true"></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left" class="toosmall" style="border-bottom: 1px solid black;">
                                        <table width="100%" align="center" style="height: 165px; table-layout: fixed;" class="toosmall">
                                            <tr>
                                                <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                    <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed;
                                                        height: 100%;">
                                                        <tr>
                                                            <td align="left" style="font-size: small;">
                                                                Buyer,
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="lblPartyName" runat="server" Font-Bold="true" Text='<%#Eval("PartyName") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="lblPartyAddr" runat="server" Text='<%#Eval("PartyAddress") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="font-size: small;" class="toosmall">
                                                                City:&nbsp;<asp:Label ID="lblPartyCity" runat="server" Font-Bold="true" Text='<%#Eval("party_city") %>'></asp:Label>
                                                                &nbsp;State:&nbsp;<asp:Label runat="server" ID="lblPartyState" Text='<%#Eval("party_state") %>'
                                                                    Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; height: 50px;" class="toosmall">
                                                                <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblCSTNo" runat="server" Text='<%#Eval("Cst_no") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblGSTNo" runat="server" Text='<%#Eval("Gst_No") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblTinNo" runat="server" Text='<%#Eval("Tin_No") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblLICNo" runat="server" Text='<%#Eval("Local_Lic_No") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblECCNo" runat="server" Text='<%#Eval("ECC_No") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("CompanyPan") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" style="width: 50%; vertical-align: top;" class="toosmall">
                                                    <table width="100%" align="center" class="toosmall" cellspacing="2" style="table-layout: fixed;
                                                        height: 100%;">
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label runat="server" ID="lblConsignedto" Text='<%#Eval("CT") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text='<%#Eval("PartyNameC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="print9pt">
                                                                <asp:Label ID="Label3" runat="server" Text='<%#Eval("PartyAddressC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="font-size: small;" class="toosmall">
                                                                <asp:Label ID="Label4" runat="server" Text='<%#Eval("party_cityC") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align: top; height: 50px;" class="toosmall">
                                                                <table width="100%" align="center" class="toosmall" style="table-layout: fixed;">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label6" runat="server" Text='<%#Eval("Cst_noC") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label7" runat="server" Text='<%#Eval("Gst_NoC") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label8" runat="server" Text='<%#Eval("Tin_NoC") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label9" runat="server" Text='<%#Eval("Local_Lic_NoC") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label10" runat="server" Text='<%#Eval("ECC_NoC") %>'></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label11" runat="server" Text='<%#Eval("CompanyPanC") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <table width="100%" cellspacing="2" cellpadding="0" style="table-layout: fixed; height: 25px;"
                                            class="print9pt">
                                            <tr>
                                                <td style="width: 50%" align="left">
                                                    Dispatched From: &nbsp;<asp:Label ID="lblDispatchFrom" runat="server" Text='<%#Eval("From_Place") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    To:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTo" runat="server"
                                                        Text='<%#Eval("To_Place") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50%" align="left">
                                                    Lorry No:&nbsp;&nbsp;
                                                    <asp:Label ID="lblLorryNo" runat="server" Text='<%#Eval("Lorry_No") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                        ID="lblDriverMobile" runat="server" Text='<%#Eval("driver_no") %>'></asp:Label>
                                                </td>
                                                <td style="width: 50%" align="left">
                                                    <asp:Label runat="server" ID="lblBroker" Text='<%#Eval("BrokerShortNew") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 15px;">
                                        Particulars:-&nbsp;Quantal : &nbsp;&nbsp;<asp:Label ID="lblQuantal" runat="server"
                                            Text='<%#Eval("Quantal") %>'></asp:Label>&nbsp;&nbsp; Bags:&nbsp;&nbsp;<asp:Label
                                                ID="lblBags" runat="server" Text='<%#Eval("BAGS") %>'></asp:Label>&nbsp;&nbsp;
                                        Grade:&nbsp;&nbsp;<asp:Label ID="lblGrade" runat="server" Text='<%#Eval("Grade") %>'></asp:Label>&nbsp;&nbsp;
                                        Mill Rate:&nbsp;&nbsp;<asp:Label ID="lblRate" runat="server" Text='<%#Eval("Mill_Rate") %>'></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="border-bottom: solid 1px black; height: 70px; vertical-align: top;"
                                        class="print9pt">
                                        We have paid on your behalf in account of<br />
                                        <asp:Label ID="lblMillNameFull" runat="server" Text='<%#Eval("MillName") %>' Font-Bold="true"></asp:Label>&nbsp;Credit
                                        the same to our account & debit to mill's account
                                    </td>
                                    <td align="right" style="border-bottom: solid 1px black;" class="print9pt">
                                        <asp:Label ID="lblMillAmount" runat="server" Text='<%#Eval("Mill_Amount") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;" align="right">
                                        <table width="50%" align="right" class="small" style="table-layout: fixed; height: 145px;">
                                            <tr>
                                                <td align="left">
                                                    rate diff debit/credit your a/c:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblRateDiff" runat="server" Text='<%#Eval("Diff_Rate").ToString()=="0.00" || Eval("Diff_Rate").ToString()=="0"?"":Eval("Diff_Rate","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Bank Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBankCommission" runat="server" Text='<%#Eval("BANK_COMMISSION").ToString()=="0.00" || Eval("BANK_COMMISSION").ToString()=="0"?"":Eval("BANK_COMMISSION","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Brokrage:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblBrokrage" runat="server" Text='<%#Eval("Brokrage").ToString()=="0.00" || Eval("Brokrage").ToString()=="0"?"":Eval("Brokrage","{0}")%>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Quality Difference:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblQualityDiff" runat="server" Text='<%#Eval("RATEDIFF").ToString()=="0.00" || Eval("RATEDIFF").ToString()=="0"?"":Eval("RATEDIFF","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Commission:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblCommission" runat="server" Text='<%#Eval("Commission_Amount").ToString()=="0.00" || Eval("Commission_Amount").ToString()=="0"?"":Eval("Commission_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Freight & Other Exp:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblFreight" runat="server" Text='<%#Eval("FREIGHT").ToString()=="0.00" || Eval("FREIGHT").ToString()=="0"?"":Eval("FREIGHT","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Post and Phone:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblPostAmt" runat="server" Text='<%#Eval("Postage").ToString()=="0.00" || Eval("Postage").ToString()=="0"?"":Eval("Postage","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Interest:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest").ToString()=="0.00" || Eval("Interest").ToString()=="0"?"":Eval("Interest","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    Transport:
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblTransport" runat="server" Text='<%#Eval("Cash_Ac_Amount").ToString()=="0.00" || Eval("Cash_Ac_Amount").ToString()=="0"?"":Eval("Cash_Ac_Amount","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Other:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblOther" runat="server" Text='<%#Eval("OTHER_Expenses").ToString()=="0.00" || Eval("OTHER_Expenses").ToString()=="0"?"":Eval("OTHER_Expenses","{0}") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" align="right" class="print" style="table-layout: fixed; height: 20px;">
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Total:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" align="right" style="table-layout: fixed; height: 20px;" class="print">
                                            <tr>
                                                <td align="left" style="width: 40%;">
                                                    Credit account total
                                                </td>
                                                <td align="left" style="width: 40%;">
                                                    RTGS Rs.:
                                                </td>
                                                <td align="right" style="width: 20%;">
                                                    <asp:Label ID="lblTotalAmt" Font-Bold="true" runat="server" Text='<%#Eval("Voucher_Amount") %>'></asp:Label>&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="border-bottom: 1px solid black;">
                                        In Words:<asp:Label runat="server" ID="lblInWords" Font-Bold="true" Text='<%#Eval("InWords") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <asp:Label ID="lblNote" Font-Bold="true" runat="server" Text="Note:"></asp:Label>&nbsp;&nbsp;
                                        After dispatch of the goods we are not responsible for non-delivery or any kind
                                        of damage or demand.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black;">
                                        <table width="100%" class="print" style="table-layout: fixed; height: 50px;">
                                            <tr>
                                                <td style="height: 50px; width: 100%; vertical-align: top;">
                                                    <table width="100%" align="left" style="table-layout: fixed;" class="print">
                                                        <tr>
                                                            <td style="width: 70%;" align="left">
                                                                <asp:Label ID="lblNarration1" Font-Bold="true" runat="server" Text='<%#Eval("Narration1") %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 30%;" rowspan="2" align="right">
                                                                <asp:Image runat="server" ID="imgSign" Height="40px" Width="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%;" align="left">
                                                                <asp:Label ID="lblNarration2" Font-Bold="true" runat="server" Text='<%#Eval("ASN_No") %>'
                                                                    Visible="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%;" align="right">
                                                    For,<asp:Label runat="server" ID="lblSignCmpName"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: solid 1px black; height: 25px;" class="print9pt">
                                        1)Please credit the amount in our account and send the amount by RTGS immediately.
                                        <br />
                                        2)If the amount is not sent before the due date payment charges 24% will be charged.
                                        <br />
                                        3)This is computer generated print No Signature Required.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px; width: 100%;">
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    <%--</ContentTemplate>
</asp:UpdatePanel>--%>
</asp:Content>
