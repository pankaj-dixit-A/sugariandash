<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="pgeHome.aspx.cs" Inherits="pgeHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script type="text/javascript">
        //Total out of range dialog
        function ShowRangeDialog() {
            $(function () {
                $('#dialog').dialog({
            })
        }).dialog("open");
    }
    </script>
    <style type="text/css">
        .bagroundPopup
        {
            opacity: 0.6;
            background-color: Black;
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "http://jsonip.appspot.com/?callback=DisplayIP";
            document.getElementsByTagName("head")[0].appendChild(script);
        };
        function DisplayIP(response) {
            document.getElementById("ipaddress").innerHTML = "Your IP Address is " + response.ip;
        }
    </script>
    <script type="text/javascript">
        function Validate() {
            var RB1 = document.getElementById("<%=rblist.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var isChecked = false;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    isChecked = true;
                    break;
                }
            }
            if (!isChecked) {
                alert("Please select atleast one option");
            }
            return isChecked;
        }
    </script>
    <script type="text/javascript">
        function CheckBoxCheck(rb) {
            debugger;
            var gv = document.getElementById("<%=AdminGrid.ClientID%>");
            var chk = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;
            for (var i = 0; i < chk.length; i++) {
                if (chk[i].type == "checkbox") {
                    if (chk[i].checked && chk[i] != rb) {
                        chk[i].checked = false;
                        break;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function validateCheckBoxes() {
            var isValid = false;
            var gridView = document.getElementById('<%= AdminGrid.ClientID %>');
            for (var i = 1; i < gridView.rows.length; i++) {
                var inputs = gridView.rows[i].getElementsByTagName('input');
                if (inputs != null) {
                    if (inputs[0].type == "checkbox") {
                        if (inputs[0].checked) {
                            isValid = true;
                            return true;
                        }
                    }
                }
            }
            //document.getElementById('<%=lblValidateCheckbox.ClientID %>').innerText = "Please select atleast one checkbox";
            alert("Please select atleast one Admin!");
            return false;
        }

        
    </script>
    <script type="text/javascript">
        function dispSummary() {
            var d = new Date().toISOString().slice(0, 10);
            window.open('../Report/rptDispSummarySmall.aspx?fromDT=' + d + '&toDT=' + d + '&Branch_Code=');
        }
        function bwlpa(FromDT, ToDt, Broker_Code) {
            window.open('../Report/rptBrokerWiseLatePayAll.aspx?FromDT=' + FromDT + '&ToDt=' + ToDt + '&Broker_Code=' + Broker_Code);
        }
    
    </script>
    <script type="text/javascript">
        window.onload = function () {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = "http://www.telize.com/jsonip?callback=DisplayIP";
            document.getElementsByTagName("head")[0].appendChild(script);
        };
        function DisplayIP(response) {

        }
    </script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>
    <script type="text/javascript">
        var checkStatus;

        var element = new Image();
        element.__defineGetter__('id', function () {
            checkStatus = 'on';
        });

        setInterval(function () {
            checkStatus = 'off';
            console.log(element);
            console.clear();
            document.querySelector('#status').innerHTML = checkStatus;
        }, 1000)
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="leftPane" style="width: 1300px; height: 30px; margin: 20px 0 0 0px;">
        <table width="80%" cellpadding="2" align="left" cellspacing="10">
            <tr>
                <td align="left">
                    <a href="../Report/pgeRegisters.aspx" target="_blank" style="font-size: large; width: 170px;
                        text-decoration: none; color: White; text-align: center;" class="button">
                        <p style="margin: 20px auto;">
                            Register</p>
                    </a>
                </td>
                <td>
                    <a href="../Sugar/pgeDeliveryOrderForGST.aspx" target="_blank" style="font-size: large;
                        width: 170px; color: White;" class="button">
                        <p style="margin-top: 20px; margin-left: 20px;">
                            Delivery Order</p>
                    </a>
                </td>
                <td>
                    <a href="../Sugar/pgeUtrentry.aspx" target="_blank" style="font-size: large; color: White;
                        width: 170px;" class="button">
                        <p style="margin-top: 20px; margin-left: 30px;">
                            UTR Entry</p>
                    </a>
                </td>
                <td>
                    <a href="../Sugar/pgeTenderPurchase.aspx" target="_blank" style="font-size: large;
                        width: 170px; color: White;" class="button">
                        <p style="margin: 20px auto; margin-left: 10px;">
                            Tender Purchase<p>
                            </p>
                    </a>
                </td>
                <td>
                    <asp:Button runat="server" ID="btndatabasebackup" Text="Database Backup" Style="font-size: large;
                        text-decoration: none; color: White; width: 190px; height: 50px;" class="button"
                        OnClick="btndatabasebackup_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <a href="../Sugar/pgeCarporatesell.aspx" target="_blank" style="font-size: large;
                        color: White; width: 170px;" class="button">
                        <p style="margin: 20px auto;">
                            Carporate Sale</p>
                    </a>
                </td>
                <td>
                    <a href="../Sugar/pgeCarporateReciept.aspx" target="_blank" style="font-size: large;
                        width: 170px; color: White;" class="button">
                        <p style="margin: 20px auto; margin-left: 10px;">
                            Multiple Reciept</p>
                    </a>
                </td>
                <td>
                    <a href="../Sugar/pgeReceiptPayment.aspx" target="_blank" style="font-size: large;
                        text-decoration: none; color: White; width: 170px;" class="button">
                        <p style="margin: 20px auto; margin-left: 10px;">
                            Reciept Payment</p>
                    </a>
                </td>
                <td>
                    <a href="../Report/pgeTrialBalanceScreen.aspx" target="_blank" style="font-size: large;
                        text-decoration: none; color: White; width: 170px;" class="button">
                        <p style="margin: 20px auto; margin-left: 10px;">
                            Trial Balance Screen</p>
                    </a>
                </td>
            </tr>
            <tr>
                <td>
                    <a onclick="javascript:dispSummary();" style="font-size: large; text-decoration: none;
                        color: White;" class="button">
                        <p style="margin: 20px auto;">
                            Dispatch Summary
                        </p>
                    </a>
                </td>
                <td>
                    <a href="../Sugar/pgeTransportSMS.aspx" target="_blank" style="font-size: large;
                        text-decoration: none; color: White; width: 170px;" class="button">
                        <p style="margin: 20px auto; margin-left: 10px;">
                            Transport SMS</p>
                    </a>
                </td>
                <td>
                    <a href="../Report/rptSugarBalanceStocks.aspx" target="_blank" style="font-size: large;
                        text-decoration: none; color: White; width: 170px;" class="button">
                        <p style="margin: 20px auto; margin-left: 10px;">
                            Stock Book</p>
                    </a>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnBWSPA" Text="Broker Report" Style="font-size: large;
                        text-decoration: none; color: White; width: 190px; height: 80px;" class="button"
                        OnClick="btnBWSPA_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog" style="display: none;">
        <table cellspacing="10">
            <tr>
                <td colspan="2" align="center">
                    <asp:Label runat="server" ID="lblMsg" Text="This Computer Dont have Security Certificate to Use This Site"
                        Font-Size="Large" ForeColor="Red" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Label runat="server" ID="lblGenerate" Text="You Want To Generate Certificate On this Computer?"
                        Font-Size="Large" ForeColor="Blue"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnGenerate" Text="Generate" CssClass="button2" OnClick="btnGenerate_Click" />
                </td>
                <td align="left">
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="button2" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <ajax1:ModalPopupExtender ID="ModalPopupMsg" PopupControlID="dialog" BackgroundCssClass="bagroundPopup"
        TargetControlID="btn" runat="server">
    </ajax1:ModalPopupExtender>
    <asp:Button ID="btn" runat="server" Style="display: none;" />
    <asp:Button ID="btn2" runat="server" Style="display: none;" />
    <asp:Button ID="btn3" runat="server" Style="display: none;" />
    <asp:Button ID="btn4" runat="server" Style="display: none;" />
    <div class="otp-popup" id="divOtp" style="display: none;">
        <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlOTP">
                    <table>
                        <tr>
                            <td align="center">
                                <h3 style="color: White;">
                                    User Verification</h3>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblSendOtp" runat="server" Text="One Time Password" Font-Bold="true"
                                    Font-Italic="true" ForeColor="BurlyWood"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <p style="font-size: large; outline-color: Red; color: White;">
                                    Please Select Way To recieve OTP...</p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rblist" OnSelectedIndexChanged="rblist_SelectedIndexChanged"
                                    ForeColor="#FFFFCC">
                                    <asp:ListItem Text="SMS" Value="S"></asp:ListItem>
                                    <asp:ListItem Text="Email" Value="E"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button runat="server" ID="btnSend" Text="SEND" CssClass="button2" OnClientClick="return Validate();"
                                    OnClick="btnSend_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" ID="btnCancelOtp" Text="CANCEL"
                                    CssClass="button2" OnClick="btnCancelOtp_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <ajax1:ModalPopupExtender ID="ModalPopupOTP" PopupControlID="divOtp" BackgroundCssClass="bagroundPopup"
        TargetControlID="btn2" runat="server">
    </ajax1:ModalPopupExtender>
    <div id="otpVerification" style="display: none;" class="otp-verification">
        <asp:UpdatePanel runat="server" ID="upl4">
            <ContentTemplate>
                <table>
                    <tr>
                        <td colspan="2" align="center">
                            <h3 style="color: Olive; font-weight: bold;">
                                OTP Verification</h3>
                            <asp:Label runat="server" ID="lblWrongOtp" ForeColor="Red"></asp:Label>
                            <asp:Label runat="server" ID="lblResendOtp" ForeColor="BlueViolet"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            Please Enter One Time Password(OTP) You have Recieved:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtOtpVerification" CssClass="textbox" Style="focus: true;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnConfirm" CssClass="button2" Text="VERIFY" OnClick="btnConfirm_Click" />
                        </td>
                        <td align="left">
                            <asp:LinkButton runat="server" Text="Resend OTP" OnClick="resendlnk_Click" ID="resendlnk"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <ajax1:ModalPopupExtender ID="ModalPopupVerification" PopupControlID="otpVerification"
        BackgroundCssClass="bagroundPopup" TargetControlID="btn3" runat="server">
    </ajax1:ModalPopupExtender>
    <div id="user" style="width: 600px; height: 300px; background-color: #FFFFFF; display: none;">
        <div id="userhead" style="background-color: Red; height: 30px; text-align: center;
            top: 3px; border: 1px solid white;">
            <h3 style="color: White; margin-top: 3px;">
                Access Denied!</h3>
            <div id="usercontent">
                <br />
                <table width="100%">
                    <tr>
                        <td align="left">
                            <p style="font-weight: bold; background-color: Black; color: White;">
                                Your Dont Have Access To Generate Certificate...If You Want To Generate Please Contact
                                Your Admin..Below Is The List Of Site Admin..Please Select Only One Admin Who will
                                Give you The OTP Sent On His Mobile...</p>
                        </td>
                    </tr>
                </table>
                <div id="gridview">
                    <asp:GridView runat="server" ID="AdminGrid" PageSize="5" AutoGenerateColumns="false"
                        Width="100%" HeaderStyle-BackColor="ButtonShadow" HeaderStyle-ForeColor="White"
                        AlternatingRowStyle-BackColor="BlueViolet" RowStyle-Height="25px" RowStyle-ForeColor="Black"
                        AlternatingRowStyle-ForeColor="White" PagerStyle-BackColor="Black" PagerStyle-ForeColor="White">
                        <Columns>
                            <asp:BoundField DataField="User_Name" HeaderText="Admin Name" ItemStyle-Width="300px" />
                            <asp:BoundField DataField="mobile" HeaderText="Mobile" ItemStyle-Width="100px" />
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="grdCB" onclick="CheckBoxCheck(this);" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <asp:Label runat="server" ID="lblValidateCheckbox" Text="" ForeColor="Red"></asp:Label>
                <table width="100%" align="center">
                    <tr>
                        <td align="right">
                            <asp:Button runat="server" ID="btnSendOtptoAdmin" Text="SEND" CssClass="button2"
                                OnClick="btnSendOtptoAdmin_Click" OnClientClick="javascript:validateCheckBoxes()" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td align="left">
                            <asp:Button runat="server" ID="Button1" Text="Cancel" CssClass="button2" OnClick="Button1_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <ajax1:ModalPopupExtender ID="usermodalpopup" PopupControlID="user" BackgroundCssClass="bagroundPopup"
        TargetControlID="btn4" runat="server">
    </ajax1:ModalPopupExtender>
</asp:Content>
