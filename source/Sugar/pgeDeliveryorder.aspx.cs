using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
public partial class pgeDeliveryorder : System.Web.UI.Page
{
    #region data section
    string temp = "0";

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string SystemMastertable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string qrycarporateSalebalance = string.Empty;
    string qryUTRBalance = string.Empty;
    string qrypurc_No = string.Empty;
    string qryAccountList = string.Empty;
    string millShortName = string.Empty;
    int defaultAccountCode = 0;
    string trnType = "DO";
    string AUTO_VOUCHER = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    public static int an = 0;

    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "deliveryorder";
            tblDetails = tblPrefix + "DODetails";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMastertable = tblPrefix + "SystemMaster";
            qryCommon = tblPrefix + "qryDeliveryOrderList";
            pnlPopup.Style["display"] = "none";
            GLedgerTable = tblPrefix + "GLEDGER";
            qrycarporateSalebalance = tblPrefix + "qryCarporatesellbalance";
            qryUTRBalance = tblPrefix + "qryUTRBalance";
            qryAccountList = tblPrefix + "qryAccountsList";
            qrypurc_No = "qrysugarBalancestock";
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    //hdnvouchernumber.Value = "0";
                    pnlPopup.Style["display"] = "none";
                    ViewState["currentTable"] = null;
                    ViewState["ankush"] = "B";
                    clsButtonNavigation.enableDisable("N");
                    this.makeEmptyForm("N");
                    ViewState["mode"] = "I";
                    this.showLastRecord();
                    txtFromDate.Text = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
            if (objAsp != null)
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where  tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "doc_no";

                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    txtdoc_no.Text = ds.Tables[0].Rows[0][0].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [makeEmptyForm]
    private void makeEmptyForm(string dAction)
    {
        try
        {
            if (dAction == "N")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                    if (c is System.Web.UI.WebControls.Label)
                    {
                        ((System.Web.UI.WebControls.Label)c).Text = "";
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }

                }
                txtcarporateSale.Enabled = false;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                pnlVoucherEntries.Style["display"] = "none";
                pnlPopup.Style["display"] = "none";
                btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                ddlFrieghtType.Enabled = false;
                lblMsg.Text = string.Empty;
                //txtPartyCommission.Enabled = false;
                drpCC.Enabled = false;
                btnVoucherOtherAmounts.Enabled = false;
                #region Logic
                calenderExtenderDate.Enabled = false;
                drpDOType.Enabled = false;
                drpDeliveryType.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtvoucher_by.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtDO_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtNARRATION1.Enabled = false;
                btntxtNARRATION2.Enabled = false;
                btntxtNARRATION3.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btntxtPurcNo.Enabled = false;
                drpDeliveryType.Enabled = false;
                lblUTRYearCode.Text = string.Empty;
                lblCSYearCode.Text = string.Empty;
                btntxtUTRNo.Enabled = false;
                btntxtcarporateSale.Enabled = false;
                btnTransLetter.Enabled = true;
                btnWayBill.Enabled = true;
                btnMail.Enabled = true;
                btnOurDO.Enabled = true;
                btnPrintSaleBill.Enabled = true;
                btnPrintCarpVoucher.Enabled = true;
                btnPrintMotorMemo.Enabled = true;
                btnPrintITCVoc.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btnSendSms.Enabled = true;
                #endregion
            }
            if (dAction == "A")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                txtcarporateSale.Enabled = false;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                drpCC.Enabled = true;
                btnSendSms.Enabled = false;
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Change No";
                btntxtDOC_NO.Enabled = true;
                //drpDeliveryType.Enabled = true;
                #region set Business logic for save
                calenderExtenderDate.Enabled = true;
                btnVoucherOtherAmounts.Enabled = true;
                drpDOType.Enabled = true;
                btnTransLetter.Enabled = false;
                btnWayBill.Enabled = false;
                drpDeliveryType.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtvoucher_by.Enabled = true;
                btntxtGRADE.Enabled = true;
                btntxtDO_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtNARRATION1.Enabled = true;
                btntxtNARRATION2.Enabled = true;
                btntxtNARRATION3.Enabled = true;
                btntxtNARRATION4.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                txtdoc_no.Enabled = false;
                btntxtPurcNo.Enabled = true;
                lblMillAmount.Text = string.Empty;
                LBLMILL_NAME.Text = string.Empty;
                LBLGETPASS_NAME.Text = string.Empty;
                lblvoucherbyname.Text = string.Empty;
                LBLBROKER_NAME.Text = string.Empty;
                LBLDO_NAME.Text = string.Empty;
                LBLTRANSPORT_NAME.Text = string.Empty;
                lblDiffrate.Text = string.Empty;
                lblMemoNo.Text = "";
                lblVoucherNo.Text = "";
                lblVoucherType.Text = "";
                //lblFreight.Text = "";
                ddlFrieghtType.Enabled = true;
                ddlFrieghtType.SelectedIndex = 0;
                lblMsg.Text = "";
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                ViewState["currentTable"] = null;
                txtUTRNo.Text = string.Empty;
                lblUTRYearCode.Text = string.Empty;
                lblCSYearCode.Text = string.Empty;
                btntxtUTRNo.Enabled = true;
                btntxtcarporateSale.Enabled = true;
                btnMail.Enabled = false;
                btnPrintSaleBill.Enabled = false;
                btnOurDO.Enabled = false;
                btnPrintCarpVoucher.Enabled = false;
                btnPrintMotorMemo.Enabled = false;
                btnPrintITCVoc.Enabled = false;
                txtDOC_DATE.Text = DateTime.Now.ToString("dd/MM/yyyy");
                setFocusControl(txtDOC_DATE);
                #endregion
            }
            if (dAction == "S")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                txtcarporateSale.Enabled = false;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                drpCC.Enabled = false;
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                #region Logic
                btnVoucherOtherAmounts.Enabled = false;
                calenderExtenderDate.Enabled = false;
                drpDOType.Enabled = false;
                drpDeliveryType.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtvoucher_by.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtDO_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtNARRATION1.Enabled = false;
                btntxtNARRATION2.Enabled = false;
                btntxtNARRATION3.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                btntxtPurcNo.Enabled = false;
                btnMail.Enabled = true;
                drpDeliveryType.Enabled = false;
                btntxtUTRNo.Enabled = false;
                btntxtcarporateSale.Enabled = false;
                btnTransLetter.Enabled = true;
                btnWayBill.Enabled = true;
                ddlFrieghtType.Enabled = false;
                btnOurDO.Enabled = true;
                btnPrintSaleBill.Enabled = true;
                btnPrintCarpVoucher.Enabled = true;
                btnPrintMotorMemo.Enabled = true;
                btnPrintITCVoc.Enabled = true;
                btnSendSms.Enabled = true;
                #endregion
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;

                        if (((System.Web.UI.WebControls.TextBox)c).Text == "0.00")
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Text = string.Empty;
                        }
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;

                        if (((System.Web.UI.WebControls.TextBox)c).Text == "0.00")
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Text = string.Empty;
                        }
                    }
                }
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                txtcarporateSale.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                btnVoucherOtherAmounts.Enabled = true;
                drpCC.Enabled = true;
                btnSendSms.Enabled = true;
                hdnfpacking.Value = "1";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = true;
                lblMsg.Text = string.Empty;
                drpDeliveryType.Enabled = true;
                #region set Business logic for edit
                calenderExtenderDate.Enabled = true;
                drpDOType.Enabled = false;
                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtvoucher_by.Enabled = true;
                btntxtGRADE.Enabled = true;
                btntxtDO_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtNARRATION1.Enabled = true;
                btntxtNARRATION2.Enabled = true;
                btntxtNARRATION3.Enabled = true;
                btntxtNARRATION4.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                btntxtPurcNo.Enabled = true;
                txtUTRNo.Text = string.Empty;
                lblUTRYearCode.Text = string.Empty;
                btntxtUTRNo.Enabled = true;
                btnPrintSaleBill.Enabled = false;
                btntxtcarporateSale.Enabled = false;
                btnMail.Enabled = false;
                btnTransLetter.Enabled = false;
                btnWayBill.Enabled = false;
                btnOurDO.Enabled = false;
                ddlFrieghtType.Enabled = true;
                btnPrintCarpVoucher.Enabled = false;
                btnPrintMotorMemo.Enabled = false;
                btnPrintITCVoc.Enabled = false;
                #endregion
            }
            #region Always check this
            string s_item = "";
            s_item = drpDOType.SelectedValue;
            if (dAction == "E" || dAction == "A")
            {
                if (s_item == "DI")
                {
                    pnlgrdDetail.Enabled = true;
                    btnOpenDetailsPopup.Enabled = true;
                    btntxtUTRNo.Enabled = true;
                    txtUTRNo.Enabled = true;
                    //grdDetail.DataSource = null;
                    //grdDetail.DataBind();
                }
                else
                {
                    pnlgrdDetail.Enabled = false;
                    btnOpenDetailsPopup.Enabled = false;
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();

                    txtUTRNo.Text = "";
                    lblUTRYearCode.Text = "";
                    //btntxtUTRNo.Enabled = false;
                    //txtUTRNo.Enabled = false;
                }
            }
            #endregion
            txtPurcNo.Enabled = false;
            txtPurcOrder.Enabled = false;
        }
        catch
        {
        }
    }
    #endregion

    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = "select MAX(doc_no) as doc_no from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        hdnf.Value = dt.Rows[0]["doc_no"].ToString();
                        if (hdnf.Value == string.Empty)
                        {
                            hdnf.Value = "1";
                        }
                        qry = getDisplayQuery();
                        bool recordExist = this.fetchRecord(qry);
                        if (recordExist == true)
                        {
                            btnAdd.Focus();
                        }
                        else                     //new code
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                }
            }
            this.enableDisableNavigateButtons();
        }
        catch
        {
        }
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 0;
            // if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(6);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(7);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(6);
                e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(6);
                e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(8);
                e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(35);
                e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(30);
                e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(7);
                e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(5);
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;

                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 33)
                    {
                        cell.Text = cell.Text.Substring(0, 33) + "...";
                        cell.ToolTip = s;
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 0;
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (v == "txtMILL_CODE" || v == "txtGETPASS_CODE" || v == "txtvoucher_by" || v == "txtBroker_CODE" || v == "txtDO_CODE" || v == "txtTRANSPORT_CODE")
                {

                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                }
                if (v == "txtdoc_no")
                {

                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    //e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(60);
                    //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }
                if (v == "txtdoc_no")
                {

                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(5);

                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text;
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "(..)";
                            cell.ToolTip = s;
                        }

                    }
                }
                if (v == "txtcarporateSale")
                {

                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(8);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(15);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text;
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "(..)";
                            cell.ToolTip = s;
                        }

                    }
                }

                if (v == "txtPurcNo")
                {

                    grdPopup.Style["table-layout"] = "auto";
                    grdPopup.CellSpacing = 10;

                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text;
                        if (cell.Text.Length > 25)
                        {
                            cell.Text = cell.Text.Substring(0, 25) + "(..)";
                            cell.ToolTip = s;
                        }

                    }
                }
                if (v == "txtUTRNo")
                {

                    e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("240px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("440px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("150px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("150px");
                    e.Row.Cells[6].ControlStyle.Width = new Unit("40px");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;

                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text;
                        if (cell.Text.Length > 50)
                        {
                            cell.Text = cell.Text.Substring(0, 50) + "(..)";
                            cell.ToolTip = s;
                        }

                    }
                }

            }
            //e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
        }
        catch
        {

        }
    }
    #endregion

    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int pgCount = 0;
            pgCount = grdPopup.PageCount;
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex, pgCount);
                e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                e.Row.Attributes["onselectstart"] = "javascript:return false;";
                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [RowCommand]
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            if (e.CommandArgument == "lnk")
            {
                switch (e.CommandName)
                {
                    case "EditRecord":
                        if (grdDetail.Rows[rowindex].Cells[8].Text != "D" && grdDetail.Rows[rowindex].Cells[8].Text != "R")
                        {
                            pnlPopupDetails.Style["display"] = "block";
                            this.showDetailsRow(grdDetail.Rows[rowindex]);
                            btnAdddetails.Text = "Update";
                        }
                        break;
                    case "DeleteRecord":
                        string action = "";
                        LinkButton lnkDelete = (LinkButton)e.CommandSource;
                        if (lnkDelete.Text == "Delete")
                        {
                            action = "Delete";
                            lnkDelete.Text = "Open";
                        }
                        else
                        {
                            action = "Open";
                            lnkDelete.Text = "Delete";
                        }
                        this.DeleteDetailsRow(grdDetail.Rows[rowindex], action);
                        break;
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(query);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
        }
        if (RecordCount != 0 && RecordCount == 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
        else if (RecordCount != 0 && RecordCount > 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = true;
        }
        if (txtdoc_no.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "' ORDER BY doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //next record exist
                            btnNext.Enabled = true;
                            btnLast.Enabled = true;
                        }
                        else
                        {
                            //next record does not exist
                            btnNext.Enabled = false;
                            btnLast.Enabled = false;
                        }
                    }
                }
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "' ORDER BY doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //previous record exist
                            btnPrevious.Enabled = true;
                            btnFirst.Enabled = true;
                        }
                        else
                        {
                            btnPrevious.Enabled = false;
                            btnFirst.Enabled = false;
                        }
                    }
                }
                #endregion
            }
        }

        #endregion
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "') and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                  "  and tran_type='" + trnType + "'";
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtdoc_no.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                    "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'" +
                    " ORDER BY doc_no DESC  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Next]
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtdoc_no.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'" +
                    " ORDER BY doc_no asc  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Last]
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "') and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'";

            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.getMaxCode();
        pnlPopupDetails.Style["display"] = "none";
        lblPDSParty.Text = "";
        lblSB_No.Text = "";
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //clsIsEdit.Tender_No = Convert.ToInt32(txtPurcNo.Text);
        //clsIsEdit.Tender.Add(Convert.ToInt32(txtPurcNo.Text));

        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");
        txtdoc_no.Enabled = true;
        hdnfpacking.Value = "2";
        if (grdDetail.Rows.Count > 0)
        {
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                grdDetail.Rows[i].Cells[9].Text = "U";
            }
        }
        if (txtcarporateSale.Text != "0" || !string.IsNullOrWhiteSpace(txtcarporateSale.Text))
        {
            ViewState["PreQntl"] = txtquantal.Text.ToString();
        }
        carporatesale();
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                DataSet ds = new DataSet();
                string vno = clsCommon.getString("select voucher_no from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                string vtype = clsCommon.getString("select voucher_type from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (vno != "0")
                {
                    if (vtype == "PS")
                    {
                        DataSet dsdel = new DataSet();

                        string delqry = "delete from " + tblPrefix + "SugarPurchase where doc_no=" + vno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        dsdel = clsDAL.SimpleQuery(delqry);

                        delqry = "";
                        delqry = "delete from " + tblPrefix + "SugarPurchaseDetails where doc_no=" + vno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        dsdel = clsDAL.SimpleQuery(delqry);

                        string qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + vtype + "' and DOC_NO=" + vno + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                        dsdel = clsDAL.SimpleQuery(qry);
                    }
                    else
                    {
                        string qry = "";
                        qry = "";
                        qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + vtype + "' and DOC_NO=" + vno + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);

                        qry = "delete from " + tblPrefix + "Voucher where Doc_No=" + vno + " and Tran_Type='" + vtype + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);
                    }
                }
                string memo_no = clsCommon.getString("select memo_no from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (memo_no != "0")
                {
                    qry = "delete from " + tblPrefix + "deliveryorder where Tran_Type='MM' and Doc_No=" + memo_no + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(qry);
                }
                string SB_No = clsCommon.getString("select SB_No from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (SB_No != "0" && !string.IsNullOrEmpty(SB_No))
                {
                    qry = "delete from " + GLedgerTable + " where TRAN_TYPE='SB' and DOC_NO=" + SB_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(qry);

                    qry = "delete from " + tblPrefix + "SugarSale where doc_no=" + SB_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(qry);

                    qry = "delete from " + tblPrefix + "sugarsaleDetails where doc_no=" + SB_No + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(qry);
                }

                //if (vno == "0" && memo_no == "0")
                //{
                //    if (string.IsNullOrEmpty(SB_No) || SB_No == "0")
                //    {
                string currentDoc_No = txtdoc_no.Text;
                qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + trnType + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);
                string strrev = "";
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 3;
                    obj.tableName = tblHead;
                    obj.columnNm = "  Tran_Type='" + trnType + "' and Doc_No=" + currentDoc_No +
                    " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);
                    if (strrev == "-3")
                    {
                        obj.flag = 3;
                        obj.tableName = tblDetails;
                        obj.columnNm = " Doc_No=" + currentDoc_No + " " +
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                        obj.values = "none";
                        ds = obj.insertAccountMaster(ref strrev);
                    }
                }
                if (txtcarporateSale.Text != "0" && !string.IsNullOrWhiteSpace(txtcarporateSale.Text))
                {
                    double BalanceSelf = Convert.ToDouble(clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "qryTenderList where Buyer=2 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtPurcNo.Text.Trim() + ""));
                    double Quintal = Convert.ToDouble(txtquantal.Text.Trim());
                    BalanceSelf = BalanceSelf + Quintal;

                    qry = "";
                    qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal=" + BalanceSelf + " where Tender_No=" + txtPurcNo.Text.Trim() + " and ID=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and Buyer_Party=2";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);

                    qry = "";
                    qry = "Delete from " + tblPrefix + "Tenderdetails where Tender_No=" + txtPurcNo.Text.Trim() + " and ID=" + txtPurcOrder.Text.Trim() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                }
                string query = "";

                if (strrev == "-3")
                {
                    query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                           " and Tran_Type='" + trnType + "'  and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                            " ORDER BY Doc_No asc  ";


                    hdnf.Value = clsCommon.getString(query);

                    if (hdnf.Value == string.Empty)
                    {
                        query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                             " and Tran_Type='" + trnType + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' " +
                            " ORDER BY Doc_No desc  ";
                        hdnf.Value = clsCommon.getString(query);
                    }

                    if (hdnf.Value != string.Empty)
                    {

                        query = getDisplayQuery();
                        bool recordExist = this.fetchRecord(query);


                        this.makeEmptyForm("S");
                        clsButtonNavigation.enableDisable("S");
                    }

                    else
                    {
                        this.makeEmptyForm("N");
                        //new code

                        clsButtonNavigation.enableDisable("N");         //No record exist  Last record deleted.
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                }
                this.enableDisableNavigateButtons();
                //    }
                //    else
                //    {
                //        lblMsg.Text = "Cannot delete this entry !";
                //        lblMsg.ForeColor = System.Drawing.Color.Red;
                //    }
                //}
                //else
                //{
                //    lblMsg.Text = "Cannot delete this entry !";
                //    lblMsg.ForeColor = System.Drawing.Color.Red;
                //}
            }
            else
            {
                lblMsg.Text = "Cannot delete this entry !";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        catch
        {
        }
    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (hdnf.Value != string.Empty)
        {
            string query = getDisplayQuery(); ;
            bool recordExist = this.fetchRecord(query);
        }
        else
        {
            this.showLastRecord();
        }
        string str = clsCommon.getString("select count(doc_no) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'");

        if (str != "0")
        {
            clsButtonNavigation.enableDisable("S");
            this.enableDisableNavigateButtons();
            this.makeEmptyForm("S");
        }
        else
        {
            clsButtonNavigation.enableDisable("N");
            this.enableDisableNavigateButtons();
            this.makeEmptyForm("N");

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
    #endregion

    #region [fetchrecord]
    private bool fetchRecord(string qry)
    {
        try
        {
            bool recordExist = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtdoc_no.Text = dt.Rows[0]["DOC_NO"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["doc_date1"].ToString();
                        drpDOType.SelectedValue = dt.Rows[0]["DESP_TYPE"].ToString();
                        if (drpDOType.SelectedValue == "DI")
                        {
                            drpDeliveryType.Visible = true;
                        }
                        else
                        {
                            drpDeliveryType.Visible = false;
                        }
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        LBLMILL_NAME.Text = dt.Rows[0]["millName"].ToString();
                        txtMillEmailID.Text = dt.Rows[0]["millEmail"].ToString();
                        txtMillMobile.Text = dt.Rows[0]["MillMobile"].ToString();
                        txtGETPASS_CODE.Text = dt.Rows[0]["GETPASSCODE"].ToString();
                        LBLGETPASS_NAME.Text = dt.Rows[0]["GetPassName"].ToString();
                        txtvoucher_by.Text = dt.Rows[0]["VOUCHER_BY"].ToString();
                        lblvoucherbyname.Text = dt.Rows[0]["VoucherByname"].ToString();
                        txtGRADE.Text = dt.Rows[0]["GRADE"].ToString();
                        txtquantal.Text = dt.Rows[0]["QUANTAL"].ToString();
                        txtPACKING.Text = dt.Rows[0]["PACKING"].ToString();
                        txtBAGS.Text = dt.Rows[0]["BAGS"].ToString();
                        txtexcise_rate.Text = dt.Rows[0]["EXCISE_RATE"].ToString();
                        txtmillRate.Text = dt.Rows[0]["mill_rate"].ToString();
                        txtSALE_RATE.Text = dt.Rows[0]["SALE_RATE"].ToString();
                        lblDiffrate.Text = dt.Rows[0]["DIFF_RATE"].ToString();
                        txtDIFF_AMOUNT.Text = dt.Rows[0]["DIFF_AMOUNT"].ToString();
                        txtDO_CODE.Text = dt.Rows[0]["DO"].ToString();
                        LBLDO_NAME.Text = dt.Rows[0]["DOName"].ToString();
                        txtBroker_CODE.Text = dt.Rows[0]["BROKER"].ToString();
                        LBLBROKER_NAME.Text = dt.Rows[0]["BrokerName"].ToString();
                        txtTruck_NO.Text = dt.Rows[0]["TRUCK_NO"].ToString();
                        txtTRANSPORT_CODE.Text = dt.Rows[0]["TRANSPORT"].ToString();
                        LBLTRANSPORT_NAME.Text = dt.Rows[0]["TransportName"].ToString();
                        txtNARRATION1.Text = dt.Rows[0]["NARRATION1"].ToString();
                        txtNARRATION2.Text = dt.Rows[0]["NARRATION2"].ToString();
                        txtNARRATION3.Text = dt.Rows[0]["NARRATION3"].ToString();
                        txtNARRATION4.Text = dt.Rows[0]["NARRATION4"].ToString();
                        txtPurcNo.Text = dt.Rows[0]["purc_no"].ToString();
                        txtPurcOrder.Text = dt.Rows[0]["purc_order"].ToString();
                        txtDriverMobile.Text = dt.Rows[0]["driver_no"].ToString();
                        txtINVOICE_NO.Text = dt.Rows[0]["Invoice_No"].ToString();
                        txtVasuliRate1.Text = dt.Rows[0]["vasuli_rate1"].ToString();
                        txtVasuliAmount1.Text = dt.Rows[0]["vasuli_amount1"].ToString();
                        //txtPartyCommission.Text = dt.Rows[0]["Party_Commission_Rate"].ToString();
                        txtMemoAdvanceRate.Text = dt.Rows[0]["MM_Rate"].ToString();
                        drpCC.SelectedValue = dt.Rows[0]["MM_CC"].ToString();
                        txtVoucherBrokrage.Text = dt.Rows[0]["Voucher_Brokrage"].ToString();
                        txtVoucherServiceCharge.Text = dt.Rows[0]["Voucher_Service_Charge"].ToString();
                        txtVoucherL_Rate_Diff.Text = dt.Rows[0]["Voucher_RateDiffRate"].ToString();
                        txtVoucherRATEDIFFAmt.Text = dt.Rows[0]["Voucher_RateDiffAmt"].ToString();
                        txtVoucherCommission_Rate.Text = dt.Rows[0]["Voucher_BankCommRate"].ToString();
                        txtVoucherBANK_COMMISSIONAmt.Text = dt.Rows[0]["Voucher_BankCommAmt"].ToString();
                        txtVoucherInterest.Text = dt.Rows[0]["Voucher_Interest"].ToString();
                        txtVoucherTransport_Amount.Text = dt.Rows[0]["Voucher_TransportAmt"].ToString();
                        txtVoucherOTHER_Expenses.Text = dt.Rows[0]["Voucher_OtherExpenses"].ToString();
                        txtCheckPostName.Text = dt.Rows[0]["CheckPost"].ToString();
                        ddlFrieghtType.SelectedValue = dt.Rows[0]["WhoseFrieght"].ToString();
                        Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        if (lblCreated != null)
                        {
                            lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        }
                        if (lblModified != null)
                        {
                            lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        }
                        txtFrieght.Text = dt.Rows[0]["FreightPerQtl"].ToString();
                        txtFrieghtAmount.Text = dt.Rows[0]["Freight_Amount"].ToString();
                        txtVasuliRate.Text = dt.Rows[0]["vasuli_rate"].ToString();
                        txtVasuliAmount.Text = dt.Rows[0]["vasuli_amount"].ToString();
                        txtMemoAdvance.Text = dt.Rows[0]["memo_advance"].ToString();
                        string CS_No = dt.Rows[0]["Carporate_Sale_No"].ToString();
                        txtcarporateSale.Text = CS_No;
                        string PDS = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + CS_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                        if (PDS == "P")
                        {
                            string prtyCode = clsCommon.getString("Select Ac_Code from " + tblPrefix + "CarporateSale where Doc_No=" + CS_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                            hdnfPDSPartyCode.Value = prtyCode;
                            string unitCode = clsCommon.getString("Select Unit_Code from " + tblPrefix + "CarporateSale where Doc_No=" + CS_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                            hdnfPDSUnitCode.Value = unitCode;
                            string nm = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + prtyCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            lblPDSParty.Text = "<b Style=" + "color:black;" + ">Party:</b> " + nm;
                            btnPrintSaleBill.Visible = true;
                        }
                        else
                        {
                            lblPDSParty.Text = "";
                            btnPrintSaleBill.Enabled = true;
                        }
                        lblCSYearCode.Text = dt.Rows[0]["Carporate_Sale_Year_Code"].ToString();
                        lblMillAmount.Text = dt.Rows[0]["final_amount"].ToString();
                        if (txtcarporateSale.Text == "0")
                        {
                            txtcarporateSale.Text = "";
                        }
                        txtUTRNo.Text = dt.Rows[0]["Utr_No"].ToString();
                        lblUTRYearCode.Text = dt.Rows[0]["UTR_Year_Code"].ToString();
                        if (txtUTRNo.Text == "0")
                        {
                            txtUTRNo.Text = "";
                        }
                        hdnvouchernumber.Value = dt.Rows[0]["voucher_no"].ToString();
                        lblVoucherNo.Text = hdnvouchernumber.Value.TrimStart();
                        lblVoucherType.Text = dt.Rows[0]["voucher_type"].ToString();
                        hdnmemonumber.Value = dt.Rows[0]["memo_no"].ToString();
                        lblMemoNo.Text = hdnmemonumber.Value.TrimStart();
                        txtSaleBillTo.Text = dt.Rows[0]["SaleBillTo"].ToString();
                        string SB_No = dt.Rows[0]["SB_No"].ToString();
                        if (!string.IsNullOrEmpty(SB_No))
                        {
                            lblsbnol.Text = "Sale Bill No:";
                            lblSB_No.Text = SB_No;
                            hdnfSB_No.Value = SB_No;//"<b Style=" + "color:Black;" + ">Sale Bill No:</b> " +
                        }
                        else
                        {
                            lblsbnol.Text = "";
                            lblSB_No.Text = "";
                            btnPrintSaleBill.Enabled = false;
                        }
                        string DT = dt.Rows[0]["DT"].ToString();
                        if (DT == "C")
                        {
                            drpDeliveryType.SelectedValue = "C";
                        }
                        else
                        {
                            drpDeliveryType.SelectedValue = "N";
                        }
                        //lblFreight.Text = dt.Rows[0]["Freight_Amount"].ToString();
                        recordExist = true;
                        lblMsg.Text = "";
                        #region Deliverty order Details
                        qry = "select detail_Id as ID ,ddType as Type,Bank_Code,BankName, Narration,Amount,UTR_NO from " + qryCommon + " where doc_no=" + hdnf.Value + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    string idValue = dt.Rows[0]["ID"].ToString();
                                    if (idValue == "")  //blank Row
                                    {
                                        grdDetail.DataSource = null;
                                        grdDetail.DataBind();
                                        ViewState["currentTable"] = null;
                                    }
                                    else
                                    {
                                        dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                        dt.Columns.Add(new DataColumn("SrNo", typeof(int)));
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            dt.Rows[i]["rowAction"] = "N";
                                            dt.Rows[i]["SrNo"] = i + 1;
                                        }
                                        grdDetail.DataSource = dt;
                                        grdDetail.DataBind();
                                        ViewState["currentTable"] = dt;
                                    }
                                }
                                else
                                {
                                    grdDetail.DataSource = null;
                                    grdDetail.DataBind();
                                    ViewState["currentTable"] = null;
                                }
                            }
                            else
                            {
                                grdDetail.DataSource = null;
                                grdDetail.DataBind();
                                ViewState["currentTable"] = null;
                            }
                        }
                        else
                        {
                            grdDetail.DataSource = null;
                            grdDetail.DataBind();
                            ViewState["currentTable"] = null;
                        }
                        #endregion
                        pnlgrdDetail.Enabled = false;
                    }
                }
            }
            this.enableDisableNavigateButtons();
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion


    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select *,CONVERT(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no='" + hdnf.Value + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and tran_type='" + trnType + "'";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region navigateRecord
    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U";
                txtdoc_no.Text = hdnf.Value;
                hdnfSuffix.Value = "";
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }
                this.enableDisableNavigateButtons();
                this.makeEmptyForm("S");
            }
            else
            {
                showLastRecord();
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [btnOpenDetailsPopup_Click]
    protected void btnOpenDetailsPopup_Click(object sender, EventArgs e)
    {
        btnAdddetails.Text = "ADD";
        pnlPopupDetails.Style["display"] = "block";
        txtBANK_CODE.Text = txtMILL_CODE.Text;
        lblBank_name.Text = LBLMILL_NAME.Text;
        double qntl = double.Parse(txtquantal.Text);
        double millrate = double.Parse(txtmillRate.Text);
        double Mill_Amount = qntl * millrate;
        if (grdDetail.Rows.Count > 0)
        {
            double total = 0.00;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[9].Text.ToString() != "D")
                {
                    double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    total += amount;
                }
            }
            if (Mill_Amount != total)
            {
                string BankAmt = Convert.ToString(Mill_Amount - total);
                hdnfMainBankAmount.Value = BankAmt;
                txtBANK_AMOUNT.Text = BankAmt;
            }
            else
            {
                txtBANK_AMOUNT.Text = Convert.ToString(Mill_Amount);
            }
        }
        setFocusControl(drpddType);
        lblUtrBalnceError.Text = "";
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        Int32 UTR_No = txtUTRNo.Text != string.Empty ? Convert.ToInt32(txtUTRNo.Text) : 0;
        try
        {
            int rowIndex = 1;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt = new DataTable();
            if (ViewState["currentTable"] != null)
            {
                dt = (DataTable)ViewState["currentTable"];
                if (dt.Rows[0]["ID"].ToString().Trim() != "")
                {
                    if (btnAdddetails.Text == "ADD")
                    {
                        dr = dt.NewRow();
                        #region calculate rowindex
                        int maxIndex = 0;
                        int[] index = new int[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            index[i] = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        }
                        if (index.Length > 0)
                        {
                            for (int i = 0; i < index.Length; i++)
                            {
                                if (index[i] > maxIndex)
                                {
                                    maxIndex = index[i];
                                }
                            }
                            rowIndex = maxIndex + 1;
                        }
                        else
                        {
                            rowIndex = maxIndex;          //1
                        }
                        #endregion
                        //     rowIndex = dt.Rows.Count + 1;
                        dr["ID"] = rowIndex;     //auto
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        //update row
                        int n = Convert.ToInt32(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;

                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select detail_Id from " + tblDetails + " where detail_Id=" + rowIndex + " And doc_no=" + hdnf.Value + " and  Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (id != string.Empty)
                        {
                            dr["rowAction"] = "U";   //actual row
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }

                        if (id == "1" && ViewState["mode"].ToString() == "I")
                        {
                            temp = "1";
                        }
                        #endregion
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add((new DataColumn("ID", typeof(Int32))));
                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("Type", typeof(string))));   //ddType
                    dt.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("BankName", typeof(string))));
                    dt.Columns.Add((new DataColumn("Narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("UTR_NO", typeof(Int32))));
                    #endregion
                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                    dr = dt.NewRow();
                    dr["ID"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add((new DataColumn("ID", typeof(int))));
                #region [Write here columns]
                dt.Columns.Add((new DataColumn("Type", typeof(string))));   //ddType
                dt.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("BankName", typeof(string))));
                dt.Columns.Add((new DataColumn("Narration", typeof(string))));
                dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("UTR_NO", typeof(Int32))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            dr["Type"] = drpddType.SelectedValue;
            dr["Bank_Code"] = txtBANK_CODE.Text;
            dr["Narration"] = txtNARRATION.Text;
            dr["BankName"] = lblBank_name.Text;
            dr["Amount"] = txtBANK_AMOUNT.Text;
            dr["UTR_NO"] = UTR_No;
            #endregion
            if (btnAdddetails.Text == "ADD")
            {
                dt.Rows.Add(dr);
            }
            #region set sr no
            DataRow drr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drr = (DataRow)dt.Rows[i];
                    drr["SrNo"] = i + 1;
                }
            }
            #endregion
            grdDetail.DataSource = dt;
            grdDetail.DataBind();
            ViewState["currentTable"] = dt;
            if (btnAdddetails.Text == "ADD")
            {
                pnlPopupDetails.Style["display"] = "block";
                setFocusControl(txtBANK_CODE);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            //txtBANK_CODE.Text = string.Empty;
            txtNARRATION.Text = string.Empty;
            //lblBank_name.Text = string.Empty;
            txtBANK_AMOUNT.Text = string.Empty;
            txtUTRNo.Text = string.Empty;
            double qntl = double.Parse(txtquantal.Text);
            double millrate = double.Parse(txtmillRate.Text);
            double Mill_Amount = qntl * millrate;
            if (grdDetail.Rows.Count > 0)
            {
                double total = 0.00;
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[9].Text.ToString() != "D")
                    {
                        double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                        total += amount;
                    }
                }
                if (Mill_Amount != total)
                {
                    string BankAmt = Convert.ToString(Mill_Amount - total);
                    hdnfMainBankAmount.Value = BankAmt;
                    txtBANK_AMOUNT.Text = BankAmt;
                }
                else
                {
                    txtBANK_AMOUNT.Text = Convert.ToString(Mill_Amount);
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnClosedetails_Click]
    protected void btnClosedetails_Click(object sender, EventArgs e)
    {
        pnlPopupDetails.Style["display"] = "none";
        setFocusControl(btnSave);
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gridViewRow)
    {

        drpddType.SelectedValue = Server.HtmlDecode(gridViewRow.Cells[3].Text);
        txtBANK_CODE.Text = Server.HtmlDecode(gridViewRow.Cells[4].Text);
        lblBank_name.Text = Server.HtmlDecode(gridViewRow.Cells[5].Text);
        txtNARRATION.Text = Server.HtmlDecode(gridViewRow.Cells[6].Text);
        string MainBankAmount = Server.HtmlDecode(gridViewRow.Cells[7].Text);
        txtBANK_AMOUNT.Text = MainBankAmount;
        hdnfMainBankAmount.Value = MainBankAmount;
        txtUTRNo.Text = Server.HtmlDecode(gridViewRow.Cells[8].Text);
        lblNo.Text = Server.HtmlDecode(gridViewRow.Cells[10].Text);
        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text);
        setFocusControl(drpddType);
        lblUtrBalnceError.Text = "";
    }
    #endregion

    #region [DeleteDetailsRow]
    private void DeleteDetailsRow(GridViewRow gridViewRow, string action)
    {
        try
        {
            int rowIndex = gridViewRow.RowIndex;
            if (ViewState["currentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["currentTable"];
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["ID"].ToString());
                string IDExisting = clsCommon.getString("select detail_Id from " + tblDetails + " where detail_Id=" + ID + " and doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "N";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "N";
                    }
                }
                else
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "A";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }
                }
                ViewState["currentTable"] = dt;
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_no.Text;
        strTextBox = "txtdoc_no";
        csCalculations();
    }
    #endregion

    #region [btntxtdoc_no_Click]
    protected void btntxtdoc_no_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtdoc_no";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDOC_DATE_TextChanged]
    protected void txtDOC_DATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDOC_DATE.Text;
        strTextBox = "txtDOC_DATE";
        csCalculations();
    }
    #endregion

    #region [drpDOType_SelectedIndexChanged]
    protected void drpDOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        searchString = drpDOType.SelectedValue;
        strTextBox = "drpDOType";
        csCalculations();
    }
    #endregion

    #region [txtMILL_CODE_TextChanged]
    protected void txtMILL_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMILL_CODE.Text;
        strTextBox = "txtMILL_CODE";
        csCalculations();
        if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
        {
            if (ViewState["mode"].ToString() == "I")
            {
                calculation();
            }
        }
    }
    #endregion

    #region [btntxtMILL_CODE_Click]
    protected void btntxtMILL_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMILL_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtGETPASS_CODE_TextChanged]
    protected void txtGETPASS_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGETPASS_CODE.Text;
        strTextBox = "txtGETPASS_CODE";
        string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
        if (txtGETPASS_CODE.Text == selfac)
        {
            txtNARRATION4.Enabled = false;
            txtSaleBillTo.Enabled = false;
        }
        else
        {
            txtNARRATION4.Enabled = true;
            txtSaleBillTo.Enabled = false;
        }
        csCalculations();
    }
    #endregion

    #region [btntxtGETPASS_CODE_Click]
    protected void btntxtGETPASS_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGETPASS_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtvoucher_by_TextChanged]
    protected void txtvoucher_by_TextChanged(object sender, EventArgs e)
    {
        searchString = txtvoucher_by.Text;
        strTextBox = "txtvoucher_by";
        csCalculations();
    }
    #endregion

    #region [btntxtvoucher_by_Click]
    protected void btntxtvoucher_by_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtvoucher_by";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtGRADE_TextChanged]
    protected void txtGRADE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGRADE.Text;
        strTextBox = "txtGRADE";
        csCalculations();
    }
    #endregion

    #region [btntxtGRADE_Click]
    protected void btntxtGRADE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGRADE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtquantal_TextChanged]
    protected void txtquantal_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtquantal.Text != string.Empty && txtPACKING.Text != string.Empty && txtquantal.Text != "0" && txtPACKING.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtquantal.Text) * (100 / float.Parse(txtPACKING.Text))));
                txtBAGS.Text = bags.ToString();
                if (hdnfpacking.Value != "1")
                {
                    setFocusControl(txtPACKING);
                }
                else
                {
                    setFocusControl(txtGETPASS_CODE);
                    hdnfpacking.Value = "2";
                }
            }
            else if ((txtPACKING.Text == string.Empty || txtPACKING.Text == "0") && txtquantal.Text != string.Empty && txtquantal.Text != "0")
            {
                txtPACKING.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtquantal.Text) * (100 / float.Parse(txtPACKING.Text))));
                txtBAGS.Text = bags.ToString();
                setFocusControl(txtPACKING);
            }
            else
            {
                txtquantal.Text = string.Empty;
                setFocusControl(txtquantal);
                txtBAGS.Text = "0";
            }
            searchString = txtquantal.Text;
            strTextBox = "txtquantal";
            calculation();
        }
        catch { }
    }
    #endregion

    #region [txtPACKING_TextChanged]
    protected void txtPACKING_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPACKING.Text;
        strTextBox = "txtPACKING";
        csCalculations();
    }
    #endregion

    #region [txtBAGS_TextChanged]
    protected void txtBAGS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBAGS.Text;
        strTextBox = "txtBAGS";
        csCalculations();
    }
    #endregion

    #region [txtexcise_rate_TextChanged]
    protected void txtexcise_rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtexcise_rate.Text;
        strTextBox = "txtexcise_rate";
        csCalculations();
    }
    #endregion

    #region [txtSALE_RATE_TextChanged]
    protected void txtSALE_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSALE_RATE.Text;
        strTextBox = "txtSALE_RATE";
        csCalculations();
        calculation();
    }
    #endregion

    #region [txtDIFF_AMOUNT_TextChanged]
    protected void txtDIFF_AMOUNT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDIFF_AMOUNT.Text;
        strTextBox = "txtDIFF_AMOUNT";
        csCalculations();
    }
    #endregion

    #region [txtDO_CODE_TextChanged]
    protected void txtDO_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDO_CODE.Text;
        strTextBox = "txtDO_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtDO_CODE_Click]
    protected void btntxtDO_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDO_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtBroker_CODE_TextChanged]
    protected void txtBroker_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBroker_CODE.Text;
        strTextBox = "txtBroker_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtBroker_CODE_Click]
    protected void btntxtBroker_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBroker_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtTruck_NO_TextChanged]
    protected void txtTruck_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTruck_NO.Text;
        strTextBox = "txtTruck_NO";
        csCalculations();
    }
    #endregion

    #region [txtTRANSPORT_CODE_TextChanged]
    protected void txtTRANSPORT_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTRANSPORT_CODE.Text;
        strTextBox = "txtTRANSPORT_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtTRANSPORT_CODE_Click]
    protected void btntxtTRANSPORT_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtTRANSPORT_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION1_TextChanged]
    protected void txtNARRATION1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION1.Text;
        strTextBox = "txtNARRATION1";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION1_Click]
    protected void btntxtNARRATION1_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION1";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION2_TextChanged]
    protected void txtNARRATION2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION2.Text;
        strTextBox = "txtNARRATION2";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION2_Click]
    protected void btntxtNARRATION2_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION2";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION3_TextChanged]
    protected void txtNARRATION3_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION3.Text;
        strTextBox = "txtNARRATION3";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION3_Click]
    protected void btntxtNARRATION3_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION3";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION4_TextChanged]
    protected void txtNARRATION4_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION4.Text;
        strTextBox = "txtNARRATION4";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION4_Click]
    protected void btntxtNARRATION4_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION4";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtBANK_CODE_TextChanged]
    protected void txtBANK_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_CODE.Text;
        strTextBox = "txtBANK_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtBANK_CODE_Click]
    protected void btntxtBANK_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBANK_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION_TextChanged]
    protected void txtNARRATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION.Text;
        strTextBox = "txtNARRATION";
        setFocusControl(txtBANK_AMOUNT);
        //csCalculations();
    }
    #endregion

    #region [btntxtNARRATION_Click]
    protected void btntxtNARRATION_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtBANK_AMOUNT_TextChanged]
    protected void txtBANK_AMOUNT_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtBANK_AMOUNT.Text;
        //strTextBox = "txtBANK_AMOUNT";
        setFocusControl(btnAdddetails);
        //csCalculations();
    }
    #endregion
    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    #region [txtSearchText_TextChanged]
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "Close")
            {
                txtSearchText.Text = string.Empty;
                pnlPopup.Style["display"] = "none";
                grdPopup.DataSource = null;
                grdPopup.DataBind();
                if (objAsp != null)
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                searchString = txtSearchText.Text;
                strTextBox = hdnfClosePopup.Value;
                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string qry = "";
            #region [Validation Part]
            bool isValidated = true;
            if (txtdoc_no.Text != string.Empty)
            {
                if (ViewState["mode"] != null)
                {
                    if (ViewState["mode"].ToString() == "I")
                    {
                        string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'");
                        if (str != string.Empty)
                        {
                            lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                            this.getMaxCode();
                            isValidated = true;
                        }
                        else
                        {
                            isValidated = true;
                        }
                    }
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtdoc_no);
                return;
            }
            if (txtDOC_DATE.Text != string.Empty)
            {
                if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtDOC_DATE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtDOC_DATE);
                return;
            }
            if (drpDOType.SelectedValue != "0")
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(drpDOType);
                return;
            }
            if (txtMILL_CODE.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtMILL_CODE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtMILL_CODE);
                return;
            }
            if (txtGETPASS_CODE.Text != string.Empty && txtGETPASS_CODE.Text != "2")
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtGETPASS_CODE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                txtGETPASS_CODE.Text = string.Empty;
                LBLGETPASS_NAME.Text = string.Empty;
                setFocusControl(txtGETPASS_CODE);
                return;
            }
            if (txtvoucher_by.Text != string.Empty && txtvoucher_by.Text != "2")
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtvoucher_by);
                    return;
                }
            }
            else
            {
                isValidated = false;
                txtvoucher_by.Text = string.Empty;
                lblvoucherbyname.Text = string.Empty;
                setFocusControl(txtvoucher_by);
                return;
            }
            if (txtGRADE.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtGRADE);
                return;
            }
            if (txtquantal.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtquantal);
                return;
            }
            if (txtPACKING.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPACKING);
                return;
            }
            if (txtexcise_rate.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtexcise_rate);
                return;
            }
            if (txtPurcNo.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPurcNo);
                return;
            }
            if (txtcarporateSale.Text != string.Empty)
            {
                qry = "select Year_Code from " + qrycarporateSalebalance + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Doc_No=" + txtcarporateSale.Text;
                string s = clsCommon.getString(qry);
                if (s != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    txtcarporateSale.Text = "";
                    lblCSYearCode.Text = "";
                    setFocusControl(txtcarporateSale);
                    return;
                }
            }
            int count = 0;
            if (grdDetail.Rows.Count > 1)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[9].Text.ToString() == "D")
                    {
                        count++;
                    }
                }
                if (grdDetail.Rows.Count == count)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Add Dispatch Details!');", true);
                    isValidated = false;
                    setFocusControl(btnOpenDetailsPopup);
                    return;
                }
            }
            if (grdDetail.Rows.Count > 0)
            {
                double total = 0.00;
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[9].Text.ToString() != "D")
                    {
                        double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                        total += amount;
                    }
                }

                if (total == Convert.ToDouble(lblMillAmount.Text))
                {
                    isValidated = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Mill Amount Does Not match with detail amount!');", true);
                    isValidated = false;
                    setFocusControl(btnOpenDetailsPopup);
                    return;
                }
            }
            if (drpDOType.SelectedValue == "DI")
            {
                if (grdDetail.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Dispatch Details!');", true);
                    isValidated = false;
                    setFocusControl(btnOpenDetailsPopup);
                    return;
                }
            }
            if (txtSALE_RATE.Text != "0")
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtSALE_RATE);
                return;
            }
            #endregion

            #region -Head part declearation
            Int32 DONumber = 0;
            Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
            string DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string DESP_TYPE = drpDOType.SelectedValue;
            string Delivery_Type = string.Empty;
            if (DESP_TYPE == "DI")
            {
                if (drpDOType.SelectedValue == "DI")
                {
                    Delivery_Type = drpDeliveryType.SelectedValue;
                }
            }
            string MILL_CODE = txtMILL_CODE.Text;
            string GETPASS_CODE = txtGETPASS_CODE.Text;
            string VOUCHER_BY = txtvoucher_by.Text;
            double FRIEGHT_RATE = txtFrieght.Text != string.Empty ? Convert.ToDouble(txtFrieght.Text) : 0;
            double FRIEGHT_AMOUNT = txtFrieghtAmount.Text != string.Empty ? Convert.ToDouble(txtFrieghtAmount.Text) : 0;
            double VASULI_AMOUNT = txtVasuliAmount.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount.Text) : 0;
            double VASULI_RATE = txtVasuliRate.Text != string.Empty ? Convert.ToDouble(txtVasuliRate.Text) : 0;
            double MEMO_ADVANCE = txtMemoAdvance.Text != string.Empty ? Convert.ToDouble(txtMemoAdvance.Text) : 0;
            string Ac_Code = txtvoucher_by.Text;
            string GRADE = txtGRADE.Text;
            double QUANTAL = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.00;
            Int32 PACKING = txtPACKING.Text != string.Empty ? Convert.ToInt32(txtPACKING.Text) : 0;
            Int32 BAGS = txtBAGS.Text != string.Empty ? Convert.ToInt32(txtBAGS.Text) : 0;
            double mill_rate = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.00;
            double EXCISE_RATE = txtexcise_rate.Text != string.Empty ? Convert.ToDouble(txtexcise_rate.Text) : 0.00;
            double SALE_RATE = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0.00;
            double MILL_AMOUNT = 0.00;// double.Parse(lblMillAmount.Text.ToString());
            MILL_AMOUNT = QUANTAL * mill_rate;
            lblMillAmount.Text = MILL_AMOUNT.ToString();
            double DIFF_RATE = lblDiffrate.Text != string.Empty ? Convert.ToDouble(lblDiffrate.Text) : 0.00;
            double DIFF_AMOUNT = txtDIFF_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtDIFF_AMOUNT.Text) : 0.00;
            double VASULI_RATE_1 = txtVasuliRate1.Text != string.Empty ? Convert.ToDouble(txtVasuliRate1.Text) : 0.00;
            double VASULI_AMOUNT_1 = txtVasuliAmount1.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount1.Text) : 0.00;
            string SaleBillTo = txtSaleBillTo.Text;
            string MM_CC = drpCC.SelectedValue.ToString();
            //double Party_Commission_Rate = txtPartyCommission.Text != string.Empty ? Convert.ToDouble(txtPartyCommission.Text) : 0.00;
            double MM_Rate = txtMemoAdvanceRate.Text != string.Empty ? Convert.ToDouble(txtMemoAdvanceRate.Text) : 0.00;

            Int32 DO_CODE = txtDO_CODE.Text != string.Empty ? Convert.ToInt32(txtDO_CODE.Text) : 2;
            Int32 BROKER_CODE = txtBroker_CODE.Text != string.Empty ? Convert.ToInt32(txtBroker_CODE.Text) : 2;
            string TRUCK_NO = txtTruck_NO.Text;
            Int32 TRANSPORT_CODE = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
            Int32 OVTransportCode = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0; ;
            if (drpCC.SelectedValue == "Cash")
            {
                OVTransportCode = 1;
            }

            string NARRATION1 = txtNARRATION1.Text;
            string NARRATION2 = txtNARRATION2.Text;
            string NARRATION3 = txtNARRATION3.Text;
            string NARRATION4 = txtNARRATION4.Text;
            string INVOICE_NO = txtINVOICE_NO.Text;
            string CheckPost = txtCheckPostName.Text;
            int purc_no = txtPurcNo.Text != string.Empty ? Convert.ToInt32(txtPurcNo.Text) : 0;
            int purc_order = txtPurcOrder.Text != string.Empty ? Convert.ToInt32(txtPurcOrder.Text) : 0;
            //double final_amout = mill_rate * QUANTAL;
            #region other voucher amount
            double VoucherBrokrage = txtVoucherBrokrage.Text != string.Empty ? Convert.ToDouble(txtVoucherBrokrage.Text) : 0.00;
            double VoucherServiceCharge = txtVoucherServiceCharge.Text != string.Empty ? Convert.ToDouble(txtVoucherServiceCharge.Text) : 0.00;
            double VoucherRateDiffRate = txtVoucherL_Rate_Diff.Text != string.Empty ? Convert.ToDouble(txtVoucherL_Rate_Diff.Text) : 0.00;
            double VoucherRateDiffAmt = txtVoucherRATEDIFFAmt.Text != string.Empty ? Convert.ToDouble(txtVoucherRATEDIFFAmt.Text) : 0.00;
            double VoucherBankCommRate = txtVoucherCommission_Rate.Text != string.Empty ? Convert.ToDouble(txtVoucherCommission_Rate.Text) : 0.00;
            double VoucherBankCommAmt = txtVoucherBANK_COMMISSIONAmt.Text != string.Empty ? Convert.ToDouble(txtVoucherBANK_COMMISSIONAmt.Text) : 0.00;
            double VoucherInterest = txtVoucherInterest.Text != string.Empty ? Convert.ToDouble(txtVoucherInterest.Text) : 0.00;
            double VoucherTransport = txtVoucherTransport_Amount.Text != string.Empty ? Convert.ToDouble(txtVoucherTransport_Amount.Text) : 0.00;
            double VoucherOtherExpenses = txtVoucherOTHER_Expenses.Text != string.Empty ? Convert.ToDouble(txtVoucherOTHER_Expenses.Text) : 0.00;

            #endregion
            double FINAL_AMOUNT = FRIEGHT_AMOUNT - MEMO_ADVANCE;
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            float DIFF = float.Parse(lblDiffrate.Text);
            double LESS_DIFF = Math.Round(((SALE_RATE - FRIEGHT_RATE) - (mill_rate)) * QUANTAL);
            double LESSDIFFOV = DIFF_RATE * QUANTAL;
            string Driver_Mobile = txtDriverMobile.Text;
            double Diff_Rate = 0.00;
            double VOUCHER_AMOUNT = 0.00;
            string Rate_Type = string.Empty;
            if (drpDeliveryType.SelectedValue == "N")
            {
                Diff_Rate = ((SALE_RATE - FRIEGHT_RATE) - mill_rate) * QUANTAL;
                VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                Rate_Type = "A";
            }
            else
            {
                Diff_Rate = ((SALE_RATE) - mill_rate) * QUANTAL;
                VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                Rate_Type = "L";
            }
            //Int32 UTR_No = txtUTRNo.Text != string.Empty ? Convert.ToInt32(txtUTRNo.Text) : 0;
            Int32 Carporate_Sale_No = txtcarporateSale.Text != string.Empty ? Convert.ToInt32(txtcarporateSale.Text) : 0;
            string WhoseFrieght = ddlFrieghtType.SelectedValue.ToString();
            int UTR_Year_Code = lblUTRYearCode.Text != string.Empty ? Convert.ToInt32(lblUTRYearCode.Text) : 0;
            int Carporate_Sale_Year_Code = lblCSYearCode.Text != string.Empty ? Convert.ToInt32(lblCSYearCode.Text) : 0;
            Int32 voucher_no = 0;
            string PDS = clsCommon.getString("Select SellingType from " + tblPrefix + "CarporateSale where Doc_No=" + Carporate_Sale_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
            AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            int memo_no = 0;
            if (ViewState["mode"].ToString() == "I")
            {
                if (AUTO_VOUCHER == "YES")
                {
                    if (DESP_TYPE == "DI")
                    {
                        memo_no = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblHead + " Where Tran_Type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    }
                    if (DESP_TYPE == "DO")
                    {
                        if (DIFF_AMOUNT != 0)
                        {
                            MaxVoucher();
                            if (hdnvouchernumber.Value != string.Empty)
                            {
                                voucher_no = Convert.ToInt32(int.Parse(hdnvouchernumber.Value.TrimStart()));
                            }
                        }
                    }
                    else
                    {
                        MaxVoucher();
                        if (hdnvouchernumber.Value != string.Empty)
                        {
                            voucher_no = Convert.ToInt32(int.Parse(hdnvouchernumber.Value.TrimStart()));
                        }
                    }
                }
            }
            string voucher_type = lblVoucherType.Text;
            //Int32 memo_no = lblMemoNo.Text != string.Empty ? Convert.ToInt32(lblMemoNo.Text) : 0;
            Int32 voucherlbl = lblVoucherNo.Text != string.Empty ? Convert.ToInt32(lblVoucherNo.Text) : 0;
            //double Freight_Amount = lblFreight.Text != string.Empty ? Convert.ToDouble(lblFreight.Text) : 0.00;
            string myNarration = string.Empty;
            string myNarration2 = string.Empty;
            string myNarration3 = string.Empty;
            string myNarration4 = string.Empty;
            string vouchnarration = string.Empty;
            millShortName = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + MILL_CODE + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (ddlFrieghtType.SelectedValue == "O")
            {
                vouchnarration = millShortName + " (" + "S.R." + SALE_RATE + "-" + FRIEGHT_RATE + "- M.R." + mill_rate + ")*" + QUANTAL;
            }
            else
            {
                vouchnarration = "Qntl " + QUANTAL + "  " + millShortName + "(M.R." + mill_rate + " P.R." + SALE_RATE + ")";
            }
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    string utrno = Server.HtmlEncode(grdDetail.Rows[i].Cells[6].Text.ToString());
                    string Utr_No = Server.HtmlEncode(grdDetail.Rows[i].Cells[8].Text.ToString());
                    string nar = clsCommon.getString("select 'dt:'+Convert(varchar(10),doc_date,103)+'  amt:'+CONVERT(nvarchar(255),amount) from " + tblPrefix + "UTR where doc_no=" + Utr_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                    if (i == 0)
                    {
                        myNarration = utrno + " " + nar;
                    }
                    if (i == 1)
                    {
                        myNarration2 = utrno;
                    }
                    if (i == 2)
                    {
                        myNarration3 = utrno;
                    }
                    if (i == 3)
                    {
                        myNarration4 = utrno;
                    }
                }
            }

            int VOUCHERAMOUNT = Convert.ToInt32(Math.Ceiling(DIFF_AMOUNT));
            //double MILL_AMOUNT = mill_rate * QUANTAL;
            string city_code = clsCommon.getString("select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string From_Place = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code=" + city_code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string city_code2 = clsCommon.getString("select City_Code from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string To_Place = clsCommon.getString("select city_name_e from " + tblPrefix + "CityMaster where city_code=" + city_code2 + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            double BILL_AMOUNT = 0.00;
            double SUBTOTAL = 0.00;
            SUBTOTAL = QUANTAL * mill_rate;
            BILL_AMOUNT = SUBTOTAL;
            int ID1 = 0;
            #endregion-End of Head part declearation
            Int32 pdsparty = hdnfPDSPartyCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSPartyCode.Value) : 0; ;
            Int32 pdsunit = hdnfPDSUnitCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSUnitCode.Value) : 0;
            #region save Head Master
            if (isValidated == true)
            {
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    clsGledgerupdations gleder = new clsGledgerupdations();
                    if (ViewState["mode"] != null)
                    {
                        DataSet ds = new DataSet();
                        if (ViewState["mode"].ToString() == "I")
                        {
                            obj.flag = 1;
                            obj.tableName = tblHead;
                            obj.columnNm = "tran_type,DOC_NO,DOC_DATE,DESP_TYPE,MILL_CODE,GETPASSCODE,VOUCHER_BY,GRADE,QUANTAL,PACKING,BAGS,mill_rate,amount,EXCISE_RATE,SALE_RATE,DIFF_RATE,DIFF_AMOUNT,DO,broker,TRUCK_NO,transport,NARRATION1,NARRATION2,NARRATION3,NARRATION4,company_code,Year_Code,Branch_Code,purc_no,purc_order,Created_By,UTR_Year_Code,Carporate_Sale_No,Carporate_Sale_Year_Code,final_amout,memo_no,Ac_Code,FreightPerQtl,Freight_Amount,vasuli_rate,vasuli_amount,Memo_Advance,Delivery_Type,driver_no,WhoseFrieght,Invoice_No,vasuli_rate1,vasuli_amount1," +
                                           " MM_CC,MM_Rate,Voucher_Brokrage,Voucher_Service_Charge,Voucher_RateDiffRate,Voucher_RateDiffAmt,Voucher_BankCommRate,Voucher_BankCommAmt,Voucher_Interest,Voucher_TransportAmt,Voucher_OtherExpenses,CheckPost,SaleBillTo";
                            obj.values = "'" + trnType + "','" + DOC_NO + "','" + DOC_DATE + "','" + DESP_TYPE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + VOUCHER_BY + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + mill_rate + "','" + MILL_AMOUNT + "','" + EXCISE_RATE + "','" + SALE_RATE + "','" + DIFF_RATE + "','" + DIFF_AMOUNT + "','" + DO_CODE + "','" + BROKER_CODE + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + myNarration + "','" + NARRATION2 + myNarration2 + "','" + NARRATION3 + myNarration3 + "','" + NARRATION4 + myNarration4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + purc_order + "','" + user + "','" + UTR_Year_Code + "','" + Carporate_Sale_No + "','" + Carporate_Sale_Year_Code + "','" + MILL_AMOUNT + "','" + memo_no + "','" + Ac_Code + "'," +
                                "'" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + Delivery_Type + "','" + Driver_Mobile + "','" + WhoseFrieght + "','" + INVOICE_NO + "','" + VASULI_RATE_1 + "','" + VASULI_AMOUNT_1 + "', " +
                                "'" + MM_CC + "','" + MM_Rate + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "','" + CheckPost + "','" + SaleBillTo + "'";
                            string nn = obj.insertDO(ref strRev);
                            retValue = strRev;
                            txtNARRATION4.Text = nn;

                            #region entry in tender details
                            if (purc_order == 1)
                            {
                                string id = clsCommon.getString("select AutoID from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + purc_order + "'");
                                if (id != string.Empty)
                                {
                                    //this id is already inserted Get max id
                                    string newId = clsCommon.getString("select max(ID) from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                    ID1 = Convert.ToInt32(newId) + 1;
                                }
                                if (drpDOType.SelectedValue != "DI")
                                {
                                    Delivery_Type = "C";
                                }
                                obj.flag = 1;
                                obj.tableName = tblPrefix + "Tenderdetails";
                                obj.columnNm = "Tender_No,Company_Code,Buyer,Buyer_Quantal,Sale_Rate,Commission_Rate,ID,Buyer_Party,IsActive,year_code,Branch_Id,Delivery_Type";
                                if (PDS == "P")
                                {
                                    obj.values = "'" + purc_no + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + pdsunit + "','" + QUANTAL + "','" + SALE_RATE + "','0.00','" + ID1 + "','" + pdsparty + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                                }
                                else
                                {
                                    obj.values = "'" + purc_no + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + GETPASS_CODE + "','" + QUANTAL + "','" + SALE_RATE + "','0.00','" + ID1 + "','" + VOUCHER_BY + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                                }
                                ds = obj.insertAccountMaster(ref strRev);
                                retValue = strRev;

                                string buyerQntl = clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                double buyerTotalQntl = Convert.ToDouble(buyerQntl) - QUANTAL;

                                qry = "";
                                qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + buyerTotalQntl + "' where Tender_No='" + purc_no + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                ds = clsDAL.SimpleQuery(qry);

                                qry = "";
                                qry = "update " + tblHead + " set purc_order='" + ID1 + "' where DOC_NO='" + DOC_NO + "' and tran_type='" + trnType + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                ds = clsDAL.SimpleQuery(qry);
                            }
                            #endregion

                            #region VoucherEntries
                            if (AUTO_VOUCHER == "YES")
                            {
                                #region Code to use later if Customer wants
                                if (drpDOType.SelectedValue == "DI")
                                {
                                    if (txtGETPASS_CODE.Text == selfac || PDS == "P")
                                    {
                                        string purchaseNo = Convert.ToString(voucher_no);
                                        if (purchaseNo != string.Empty)
                                        {
                                            string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarPurchase where doc_no=" + purchaseNo + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                            if (str != string.Empty)
                                            {
                                                getvoucherscode(tblPrefix + "SugarPurchase", "doc_no", "NULL", "Tran_Type");
                                                purchaseNo = ViewState["maxval"].ToString();
                                            }
                                        }
                                        Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                                        obj.flag = 1;
                                        obj.tableName = tblPrefix + "SugarPurchase";
                                        obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By";
                                        obj.values = "'" + purchaseNo + "','PS','" + DOC_NO + "','" + DOC_DATE + "','" + Payment_To + "','" + MILL_CODE + "','" + From_Place + "','" + To_Place + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + SUBTOTAL + "','" + (QUANTAL * mill_rate) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                        ds = obj.insertAccountMaster(ref strRev);
                                        retValue = strRev;

                                        obj.flag = 1;
                                        obj.tableName = tblPrefix + "SugarPurchaseDetails";
                                        obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                        obj.values = "'" + purchaseNo + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + mill_rate + "','" + (QUANTAL * mill_rate) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                        ds = new DataSet();
                                        ds = obj.insertAccountMaster(ref strRev);

                                        gleder.SugarPurchaseGledgerEffect("PS", Convert.ToInt32(purchaseNo), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                                        qry = "";
                                        qry = "update " + tblHead + " set voucher_no='" + purchaseNo + "' , voucher_type='" + "PS" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                        ds = clsDAL.SimpleQuery(qry);

                                        if (PDS == "P")
                                        {
                                            #region Entry In Sugar Sale
                                            string unitcity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + pdsunit + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                                            int saleno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "SugarSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));

                                            string saleNumber = Convert.ToString(saleno);
                                            if (saleNumber != string.Empty)
                                            {
                                                string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarSale where doc_no=" + saleNumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                                if (str != string.Empty)
                                                {
                                                    getvoucherscode(tblPrefix + "SugarSale", "doc_no", "NULL", "Tran_Type");
                                                    saleNumber = ViewState["maxval"].ToString();
                                                }
                                            }
                                            hdnfSB_No.Value = saleNumber;
                                            lblSB_No.Text = "Sale Bill No: " + hdnfSB_No.Value;
                                            string saleParty = hdnfPDSPartyCode.Value;
                                            //entry in main table
                                            obj.flag = 1;
                                            obj.tableName = tblPrefix + "SugarSale";
                                            obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,LESS_FRT_RATE,FREIGHT,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By,DO_No,Transport_Code,CASH_ADVANCE";
                                            obj.values = "'" + saleNumber + "','SB','" + purchaseNo + "','" + DOC_DATE + "','" + pdsunit + "','" + pdsunit + "','" + MILL_CODE + "','" + From_Place + "','" + unitcity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + TRANSPORT_CODE + "','" + MEMO_ADVANCE + "'";
                                            ds = obj.insertAccountMaster(ref strRev);

                                            //entry in detail table
                                            obj.flag = 1;
                                            obj.tableName = tblPrefix + "sugarsaleDetails";
                                            obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                            obj.values = "'" + saleNumber + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + SALE_RATE + "','" + (QUANTAL * SALE_RATE) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);

                                            qry = "";
                                            qry = "update " + tblHead + " set SB_No='" + saleNumber + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                            ds = clsDAL.SimpleQuery(qry);
                                            gleder.SugarSaleGledgerEffect("SB", Convert.ToInt32(saleNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                            #endregion
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(txtSaleBillTo.Text))
                                            {
                                                string salebillto = txtSaleBillTo.Text;
                                                string salebilltocity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + salebillto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                                                int saleno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(DOC_NO),0)+1 from " + tblPrefix + "SugarSale where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                                string saleNumber = Convert.ToString(saleno);
                                                if (saleNumber != string.Empty)
                                                {
                                                    string str = clsCommon.getString("select doc_no from " + tblPrefix + "SugarSale where doc_no=" + saleNumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                                    if (str != string.Empty)
                                                    {
                                                        getvoucherscode(tblPrefix + "SugarSale", "doc_no", "NULL", "Tran_Type");
                                                        saleNumber = ViewState["maxval"].ToString();
                                                    }
                                                }
                                                hdnfSB_No.Value = saleNumber;
                                                lblSB_No.Text = "Sale Bill No: " + hdnfSB_No.Value;
                                                //entry in main table
                                                obj.flag = 1;
                                                obj.tableName = tblPrefix + "SugarSale";
                                                obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,SUBTOTAL,LESS_FRT_RATE,FREIGHT,BILL_AMOUNT,NETQNTL,Company_Code,Year_Code,Branch_Code,Created_By,DO_No,Transport_Code,CASH_ADVANCE";
                                                obj.values = "'" + saleNumber + "','SB','" + purchaseNo + "','" + DOC_DATE + "','" + salebillto + "','" + salebillto + "','" + MILL_CODE + "','" + From_Place + "','" + salebilltocity + "','" + TRUCK_NO + "','" + BROKER_CODE + "','" + (QUANTAL * SALE_RATE) + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "','" + QUANTAL + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + DOC_NO + "','" + TRANSPORT_CODE + "','" + MEMO_ADVANCE + "'";
                                                ds = obj.insertAccountMaster(ref strRev);

                                                //entry in detail table
                                                obj.flag = 1;
                                                obj.tableName = tblPrefix + "sugarsaleDetails";
                                                obj.columnNm = "doc_no,item_code,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                                obj.values = "'" + saleNumber + "','1','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + SALE_RATE + "','" + (QUANTAL * SALE_RATE) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                                ds = new DataSet();
                                                ds = obj.insertAccountMaster(ref strRev);

                                                qry = "";
                                                qry = "update " + tblHead + " set SB_No='" + saleNumber + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                                ds = clsDAL.SimpleQuery(qry);
                                                gleder.SugarSaleGledgerEffect("SB", Convert.ToInt32(saleNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        #region Entry in Loading Voucher
                                        int voucherno = voucher_no;
                                        obj.flag = 1;
                                        obj.tableName = "" + tblPrefix + "Voucher";
                                        obj.columnNm = "Tran_Type, DOC_NO , SUFFIX , DO_No ,Lorry_No, DOC_DATE , AC_CODE,Unit_Code, BROKER_CODE ," +
                                        " Quantal,PACKING , BAGS ,GRADE , MILL_CODE, MILL_RATE ,Sale_Rate," +
                                        " FreightPerQtl, NARRATION1 ,NARRATION2 , NARRATION3 , NARRATION4 ,From_Place,To_Place," +
                                        " Mill_Amount,TRANSPORT_CODE,LESSDIFF,Diff_Rate,VOUCHER_AMOUNT,CASH_ACCOUNT,CASH_AMOUNT_RATE,CASH_AC_AMOUNT," +
                                        " Company_Code, Year_Code , Branch_Code,Delivery_Type,Created_By,Rate_Type," +
                                        " BROKRAGE,SERVICE_CHARGE,L_RATE_DIFF,RATEDIFF,Commission_Rate,Commission_Amount,INTEREST,TRANSPORT_AMOUNT,OTHER_EXPENSES";

                                        string voucherNumber = Convert.ToString(voucherno);
                                        if (voucherNumber != string.Empty)
                                        {
                                            string str = clsCommon.getString("select Doc_No from " + tblPrefix + "Voucher where Doc_No=" + voucherNumber + " and Tran_Type='OV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                            if (str != string.Empty)
                                            {
                                                getvoucherscode(tblPrefix + "Voucher", "Doc_No", "OV", "Tran_Type");
                                                voucherNumber = ViewState["maxval"].ToString();
                                            }
                                        }
                                        if (drpDeliveryType.SelectedValue == "N")
                                        {
                                            obj.values = "'" + "OV" + "','" + voucherNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + TRUCK_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASS_CODE + "','" + BROKER_CODE + "'," +
                                                         "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "'," +
                                                         " '" + FRIEGHT_RATE + "','" + vouchnarration + "','" + myNarration + "','" + NARRATION2 + " " + myNarration2 + "','" + NARRATION3 + " " + myNarration3 + " " + NARRATION4 + " " + myNarration4 + "','" + From_Place + "','" + To_Place + "'," +
                                                         " '" + MILL_AMOUNT + "','" + TRANSPORT_CODE + "','" + LESSDIFFOV + "','" + Diff_Rate + "','" + VOUCHER_AMOUNT + "','" + OVTransportCode + "','" + FRIEGHT_RATE + "','" + MEMO_ADVANCE + "'," +
                                                         " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Delivery_Type + "','" + user + "','" + Rate_Type + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "'";
                                        }
                                        else
                                        {
                                            obj.values = "'" + "OV" + "','" + voucherNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + TRUCK_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASS_CODE + "','" + BROKER_CODE + "'," +
                                                         "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "'," +
                                                         " '0.00','" + vouchnarration + "','" + myNarration + "','" + NARRATION2 + " " + myNarration2 + "','" + NARRATION3 + " " + myNarration3 + " " + NARRATION4 + " " + myNarration4 + "','" + From_Place + "','" + To_Place + "'," +
                                                         " '" + MILL_AMOUNT + "','" + TRANSPORT_CODE + "','" + LESSDIFFOV + "','" + Diff_Rate + "','" + VOUCHER_AMOUNT + "','" + OVTransportCode + "','" + FRIEGHT_RATE + "','" + MEMO_ADVANCE + "'," +
                                                         " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Delivery_Type + "','" + user + "','" + Rate_Type + "','" + VoucherBrokrage + "','" + VoucherServiceCharge + "','" + VoucherRateDiffRate + "','" + VoucherRateDiffAmt + "','" + VoucherBankCommRate + "','" + VoucherBankCommAmt + "','" + VoucherInterest + "','" + VoucherTransport + "','" + VoucherOtherExpenses + "'";
                                        }
                                        ds = obj.insertAccountMaster(ref strRev);
                                        retValue = strRev;

                                        qry = "";
                                        qry = "update " + tblHead + " set voucher_no='" + voucherNumber + "' , voucher_type='" + "OV" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                        ds = clsDAL.SimpleQuery(qry);
                                        gleder.LoadingVoucherGlederEffect("OV", Convert.ToInt32(voucherNumber), Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                                    }
                                    #region Entry in Motor Memo

                                    //if (AUTO_VOUCHER == "YES")
                                    //{
                                    //    if (DESP_TYPE == "DI")
                                    //    {
                                    //        memo_no = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblHead + " Where Tran_Type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                                    //    }
                                    //}
                                    obj.flag = 1;
                                    obj.tableName = tblHead;
                                    obj.columnNm = "tran_type,DESP_TYPE,DOC_NO,DOC_DATE,MILL_CODE,GETPASSCODE,GRADE,QUANTAL,PACKING,BAGS,TRUCK_NO,transport,NARRATION1,NARRATION2,NARRATION3,NARRATION4,company_code,Year_Code,Branch_Code,purc_no,purc_order,Ac_Code,FreightPerQtl,Freight_Amount,vasuli_rate,vasuli_amount,less,Created_By,final_amout,driver_no";
                                    string MemoNumber = Convert.ToString(memo_no);
                                    if (MemoNumber != string.Empty)
                                    {
                                        string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no=" + MemoNumber + " and tran_type='MM' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                        if (str != string.Empty)
                                        {
                                            getvoucherscode(tblHead, "doc_no", "MM", "tran_type");
                                            MemoNumber = ViewState["maxval"].ToString();
                                        }
                                    }
                                    if (purc_order == 1)
                                    {
                                        if (PDS == "P")
                                        {
                                            obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + pdsunit + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + ID1 + "','" + pdsparty + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                        }
                                        else
                                        {
                                            obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + ID1 + "','" + VOUCHER_BY + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                        }
                                    }
                                    else
                                    {
                                        if (PDS == "P")
                                        {
                                            obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + pdsunit + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + ID1 + "','" + pdsparty + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                        }
                                        else
                                        {
                                            obj.values = "'MM','" + DESP_TYPE + "','" + MemoNumber + "','" + DOC_DATE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + purc_order + "','" + VOUCHER_BY + "','" + FRIEGHT_RATE + "','" + FRIEGHT_AMOUNT + "','" + VASULI_RATE + "','" + VASULI_AMOUNT + "','" + MEMO_ADVANCE + "','" + user + "','" + FINAL_AMOUNT + "','" + Driver_Mobile + "'";
                                        }
                                    }
                                    string mm = obj.insertDO(ref strRev);
                                    retValue = strRev;
                                    #endregion
                                }
                                        #endregion
                                #region Creating Local Voucher
                                if (drpDOType.SelectedValue == "DO")
                                {
                                    if (DIFF_AMOUNT != 0)
                                    {
                                        int voucherno = voucher_no;
                                        obj.flag = 1;
                                        obj.tableName = "" + tblPrefix + "Voucher";
                                        obj.columnNm = " Tran_Type, DOC_NO , SUFFIX , DO_No , DOC_DATE , AC_CODE,Unit_Code, BROKER_CODE ," +
                                        " Quantal,PACKING , BAGS ,GRADE , MILL_CODE, MILL_RATE ,Sale_Rate,Mill_Amount," +
                                        " FREIGHT, NARRATION1 ,NARRATION2 , NARRATION3 , NARRATION4 ," +
                                        " VOUCHER_AMOUNT ,Diff_Amount," +
                                        " Company_Code, Year_Code , Branch_Code,Created_By";

                                        string LVNumber = Convert.ToString(voucherno);
                                        if (LVNumber != string.Empty)
                                        {
                                            string str = clsCommon.getString("select Doc_No from " + tblPrefix + "Voucher where Doc_No=" + LVNumber + " and Tran_Type='LV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                            if (str != string.Empty)
                                            {
                                                getvoucherscode(tblPrefix + "Voucher", "Doc_No", "LV", "Tran_Type");
                                                LVNumber = ViewState["maxval"].ToString();
                                            }
                                        }

                                        obj.values = "'" + "LV" + "','" + LVNumber + "','" + string.Empty.Trim() + "','" + DOC_NO + "','" + DOC_DATE + "','" + Ac_Code + "','" + GETPASS_CODE + "','" + BROKER_CODE + "'," +
                                        "'" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + mill_rate + "','" + SALE_RATE + "','" + MILL_AMOUNT + "'," +
                                        " '" + FRIEGHT_AMOUNT + "','" + vouchnarration + "','" + NARRATION2 + " Lorry No:" + TRUCK_NO + "','" + NARRATION3 + "','" + NARRATION4 + TRUCK_NO + "'," +
                                        " '" + DIFF_AMOUNT + "','" + DIFF_RATE + "'," +
                                           "'" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + user + "'";
                                        ds = obj.insertAccountMaster(ref strRev);
                                        retValue = strRev;

                                        //Gledger Effect for Local Voucher
                                        qry = "";
                                        qry = "delete from " + GLedgerTable + " where TRAN_TYPE='LV' and DOC_NO=" + LVNumber + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                        ds = clsDAL.SimpleQuery(qry);
                                        Int32 GID = 0;

                                        double LVVoucherAmount = ((SALE_RATE - mill_rate) * QUANTAL);
                                        if (LVVoucherAmount > 0)
                                        {
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + VOUCHER_BY + "','" + GETPASS_CODE + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                            ds = obj.insertAccountMaster(ref strRev);
                                        }
                                        else
                                        {
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + VOUCHER_BY + "','" + GETPASS_CODE + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                            ds = obj.insertAccountMaster(ref strRev);
                                        }
                                        // diffrance amount effect
                                        if (LVVoucherAmount > 0)
                                        {
                                            //------------Credit effect
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + int.Parse(clsGV.QUALITY_DIFF_AC) + "','" + 0 + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                            ds = obj.insertAccountMaster(ref strRev);
                                        }
                                        else
                                        {
                                            //------------Credit effect
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'LV','" + LVNumber + "','" + DOC_DATE + "','" + int.Parse(clsGV.QUALITY_DIFF_AC) + "','" + 0 + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "','LV','" + LVNumber + "'";
                                            ds = obj.insertAccountMaster(ref strRev);
                                        }
                                    }
                                #endregion
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            //Update Mode
                            obj.flag = 2;
                            obj.tableName = tblHead;
                            obj.columnNm = "DOC_DATE='" + DOC_DATE + "',DESP_TYPE='" + DESP_TYPE +
                                "',MILL_CODE='" + MILL_CODE + "',GETPASSCODE='" + GETPASS_CODE +
                                "',VOUCHER_BY='" + VOUCHER_BY + "',GRADE='" + GRADE +
                                "',QUANTAL='" + QUANTAL + "',PACKING='" + PACKING + "',BAGS='" + BAGS +
                                "',mill_rate='" + mill_rate + "',EXCISE_RATE='" + EXCISE_RATE + "',SALE_RATE='" + SALE_RATE +
                                "',DIFF_RATE='" + DIFF_RATE + "',DIFF_AMOUNT='" + DIFF_AMOUNT + "',DO='" + DO_CODE +
                                "',BROKER='" + BROKER_CODE + "',TRUCK_NO='" + TRUCK_NO + "',TRANSPORT='" +
                                TRANSPORT_CODE + "',NARRATION1='" + myNarration + "',NARRATION2='" + NARRATION2 +
                                "',NARRATION3='" + NARRATION3 + "',NARRATION4='" + NARRATION4 + "',purc_no='" + purc_no +
                                "',purc_order='" + purc_order + "',Modified_By='" + user +
                                "',UTR_Year_Code='" + UTR_Year_Code + "',Carporate_Sale_No='" + Carporate_Sale_No + "'," +
                                " Carporate_Sale_Year_Code='" + Carporate_Sale_Year_Code + "' ,final_amout='" + MILL_AMOUNT + "', " +
                                "  Ac_Code='" + Ac_Code + "'," +
                                "  FreightPerQtl='" + FRIEGHT_RATE + "',Freight_Amount='" + FRIEGHT_AMOUNT + "',vasuli_rate='" + VASULI_RATE + "',vasuli_amount='" + VASULI_AMOUNT + "',Memo_Advance='" + MEMO_ADVANCE + "',Delivery_Type='" + Delivery_Type + "',driver_no='" + Driver_Mobile + "',WhoseFrieght='" + WhoseFrieght + "',Invoice_No='" + INVOICE_NO + "',vasuli_rate1='" + VASULI_RATE_1 + "',vasuli_amount1='" + VASULI_AMOUNT_1 + "',SaleBillTo='" + SaleBillTo + "'," +
                                " MM_CC='" + MM_CC + "',MM_Rate='" + MM_Rate + "',Voucher_Brokrage='" + VoucherBrokrage + "',Voucher_Service_Charge='" + VoucherServiceCharge + "',Voucher_RateDiffRate='" + VoucherRateDiffRate + "',Voucher_RateDiffAmt='" + VoucherRateDiffAmt + "',Voucher_BankCommRate='" + VoucherBankCommRate + "',Voucher_BankCommAmt='" + VoucherBankCommAmt + "',Voucher_Interest='" + VoucherInterest + "',Voucher_TransportAmt='" + VoucherTransport + "',Voucher_OtherExpenses='" + VoucherOtherExpenses + "',CheckPost='" + CheckPost + "' " +
                                "  where DOC_NO='" + DOC_NO + "' and company_code='" + Company_Code + "' and Year_Code='" + year_Code + "'  and tran_type='" + trnType + "' ";
                            obj.values = "none";
                            ds = new DataSet();
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;

                            #region entry in tender details
                            if (purc_order == 1)
                            {
                                string id = clsCommon.getString("select AutoID from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + purc_order + "'");
                                if (id != string.Empty)
                                {
                                    //this id is already inserted Get max id
                                    string newId = clsCommon.getString("select max(ID) from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                    ID1 = Convert.ToInt32(newId) + 1;
                                }
                                if (drpDOType.SelectedValue != "DI")
                                {
                                    Delivery_Type = "C";
                                }
                                obj.flag = 1;
                                obj.tableName = tblPrefix + "Tenderdetails";
                                obj.columnNm = "Tender_No,Company_Code,Buyer,Buyer_Quantal,Sale_Rate,Commission_Rate,ID,Buyer_Party,IsActive,year_code,Branch_Id,Delivery_Type";
                                if (PDS == "P")
                                {
                                    obj.values = "'" + purc_no + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + pdsunit + "','" + QUANTAL + "','" + SALE_RATE + "','0.00','" + ID1 + "','" + pdsparty + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                                }
                                else
                                {
                                    obj.values = "'" + purc_no + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + GETPASS_CODE + "','" + QUANTAL + "','" + SALE_RATE + "','0.00','" + ID1 + "','" + VOUCHER_BY + "','True','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                                }
                                ds = obj.insertAccountMaster(ref strRev);
                                retValue = strRev;

                                string buyerQntl = clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + purc_no + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                double buyerTotalQntl = Convert.ToDouble(buyerQntl) - QUANTAL;

                                qry = "";
                                qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + buyerTotalQntl + "' where Tender_No='" + purc_no + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                ds = clsDAL.SimpleQuery(qry);

                                qry = "";
                                qry = "update " + tblHead + " set purc_order='" + ID1 + "' where DOC_NO='" + DOC_NO + "' and tran_type='" + trnType + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                ds = clsDAL.SimpleQuery(qry);
                            }
                            #endregion

                            if (txtcarporateSale.Text != "0" && !string.IsNullOrWhiteSpace(txtcarporateSale.Text))
                            {
                                double PreQntl = Convert.ToDouble(ViewState["PreQntl"].ToString());
                                double BalanceSelf = Convert.ToDouble(clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "qryTenderList where Buyer=2 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtPurcNo.Text.Trim() + ""));
                                double RemainingQntl = Math.Abs(QUANTAL - PreQntl);
                                if (QUANTAL > PreQntl)
                                {
                                    BalanceSelf = BalanceSelf - RemainingQntl;
                                }
                                if (QUANTAL < PreQntl)
                                {
                                    BalanceSelf = BalanceSelf + RemainingQntl;
                                }

                                if (QUANTAL != PreQntl)
                                {
                                    qry = "";
                                    qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal=" + BalanceSelf + " where Tender_No=" + txtPurcNo.Text.Trim() + " and ID=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and Buyer_Party=2";
                                    ds = new DataSet();
                                    ds = clsDAL.SimpleQuery(qry);

                                    qry = "";
                                    qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal=" + QUANTAL + " where Tender_No=" + txtPurcNo.Text.Trim() + " and ID=" + txtPurcOrder.Text.Trim() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                                    ds = new DataSet();
                                    ds = clsDAL.SimpleQuery(qry);
                                }
                            }
                            if (AUTO_VOUCHER == "YES")
                            {
                                int Zero = Convert.ToInt32(lblVoucherNo.Text.ToString());
                                if (Zero != 0)
                                {
                                    if (drpDOType.SelectedValue == "DI")
                                    {
                                        if (txtGETPASS_CODE.Text == selfac || PDS == "P")
                                        {
                                            Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                                            DONumber = int.Parse(hdnvouchernumber.Value.TrimStart());
                                            obj.flag = 2;
                                            obj.tableName = tblPrefix + "SugarPurchase";
                                            obj.columnNm = "PURCNO='" + DOC_NO + "',DOC_DATE='" + DOC_DATE + "',AC_CODE='" + Payment_To + "',MILL_CODE='" + MILL_CODE + "',FROM_STATION='" + From_Place + "',TO_STATION='" + To_Place + "',LORRYNO='" + TRUCK_NO + "',BROKER='" + BROKER_CODE + "',SUBTOTAL='" + SUBTOTAL + "',BILL_AMOUNT='" + (QUANTAL * mill_rate) + "',NETQNTL='" + QUANTAL + "',Modified_By='" + user + "' where Tran_Type='PS' and doc_no=" + DONumber + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                                            obj.values = "none";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);
                                            retValue = strRev;
                                            string detailsId = clsCommon.getString("select top 1 detail_id from " + tblPrefix + "SugarPurchaseDetails where doc_no=" + DONumber + " and Company_Code=" + Company_Code + " and Year_Code=" + year_Code + "");
                                            obj.flag = 2;
                                            obj.tableName = tblPrefix + "SugarPurchaseDetails";
                                            obj.columnNm = "item_code='1',Quantal='" + QUANTAL + "',packing='" + PACKING + "',bags='" + BAGS + "'," +
                                                " rate='" + SALE_RATE + "',item_Amount='" + (QUANTAL * mill_rate) + "',Modified_By='" + user + "'" +
                                                " where Company_Code='" + Company_Code + "' and year_Code='" + year_Code + "' " +
                                                "  and doc_no='" + DONumber + "' and detail_id='" + detailsId + "'";
                                            obj.values = "none";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);

                                            string drcr_head = clsCommon.getString("select Purchase_AC from " + tblPrefix + "SystemMaster where System_Type='I' and System_Code='1' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                            gleder.SugarPurchaseGledgerEffect("PS", DONumber, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                            if (PDS == "P")
                                            {
                                                #region Entry In Sugar Sale
                                                DONumber = int.Parse(hdnfSB_No.Value.TrimStart());
                                                string saleParty = hdnfPDSPartyCode.Value;
                                                string PN = hdnvouchernumber.Value.TrimStart();
                                                string unitcity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + pdsunit + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                                                //entry in main table
                                                obj.flag = 2;
                                                obj.tableName = tblPrefix + "SugarSale";
                                                obj.columnNm = "PURCNO='" + PN + "',DOC_DATE='" + DOC_DATE + "',AC_CODE='" + pdsunit + "',Unit_Code='" + pdsunit + "',MILL_CODE='" + MILL_CODE + "',FROM_STATION='" + From_Place + "',TO_STATION='" + unitcity + "',LORRYNO='" + TRUCK_NO + "',BROKER='" + BROKER_CODE + "',SUBTOTAL='" + (QUANTAL * SALE_RATE) + "',LESS_FRT_RATE='" + FRIEGHT_RATE + "',FREIGHT='" + FRIEGHT_AMOUNT + "',BILL_AMOUNT='" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "',NETQNTL='" + QUANTAL + "',Modified_By='" + user + "',DO_No='" + DOC_NO + "',Transport_Code='" + TRANSPORT_CODE + "',CASH_ADVANCE='" + MEMO_ADVANCE + "' where Tran_Type='SB' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and DOC_NO=" + DONumber + "";
                                                obj.values = "none";
                                                ds = obj.insertAccountMaster(ref strRev);

                                                //entry in detail table
                                                obj.flag = 2;
                                                obj.tableName = tblPrefix + "sugarsaleDetails";
                                                obj.columnNm = "item_code='1',Quantal='" + QUANTAL + "',packing='" + PACKING + "',bags='" + BAGS + "',rate='" + SALE_RATE + "',item_Amount='" + (QUANTAL * SALE_RATE) + "',Modified_By='" + user + "' where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'  and doc_no=" + DONumber + "";
                                                obj.values = "none";
                                                ds = new DataSet();
                                                ds = obj.insertAccountMaster(ref strRev);
                                                gleder.SugarSaleGledgerEffect("SB", DONumber, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                                #endregion
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(txtSaleBillTo.Text))
                                                {
                                                    DONumber = int.Parse(hdnfSB_No.Value.TrimStart());
                                                    if (!string.IsNullOrEmpty(Convert.ToString(DONumber)))
                                                    {
                                                        string salebillto = txtSaleBillTo.Text;
                                                        string PN = hdnvouchernumber.Value.TrimStart();
                                                        string salebilltocity = clsCommon.getString("Select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + salebillto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                                                        //entry in main table
                                                        obj.flag = 2;
                                                        obj.tableName = tblPrefix + "SugarSale";
                                                        obj.columnNm = "PURCNO='" + PN + "',DOC_DATE='" + DOC_DATE + "',AC_CODE='" + salebillto + "',Unit_Code='" + salebillto + "',MILL_CODE='" + MILL_CODE + "',FROM_STATION='" + From_Place + "',TO_STATION='" + salebilltocity + "',LORRYNO='" + TRUCK_NO + "',BROKER='" + BROKER_CODE + "',SUBTOTAL='" + (QUANTAL * SALE_RATE) + "',LESS_FRT_RATE='" + FRIEGHT_RATE + "',FREIGHT='" + FRIEGHT_AMOUNT + "',BILL_AMOUNT='" + ((QUANTAL * SALE_RATE) + MEMO_ADVANCE - FRIEGHT_AMOUNT) + "',NETQNTL='" + QUANTAL + "',Modified_By='" + user + "',DO_No='" + DOC_NO + "',Transport_Code='" + TRANSPORT_CODE + "',CASH_ADVANCE='" + MEMO_ADVANCE + "' where Tran_Type='SB' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and DOC_NO=" + DONumber + "";
                                                        obj.values = "none";
                                                        ds = obj.insertAccountMaster(ref strRev);

                                                        //entry in detail table
                                                        obj.flag = 2;
                                                        obj.tableName = tblPrefix + "sugarsaleDetails";
                                                        obj.columnNm = "item_code='1',Quantal='" + QUANTAL + "',packing='" + PACKING + "',bags='" + BAGS + "',rate='" + SALE_RATE + "',item_Amount='" + (QUANTAL * SALE_RATE) + "',Modified_By='" + user + "' where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'  and doc_no=" + DONumber + "";
                                                        obj.values = "none";
                                                        ds = new DataSet();
                                                        ds = obj.insertAccountMaster(ref strRev);
                                                        gleder.SugarSaleGledgerEffect("SB", DONumber, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            #region Updating Loading Voucher
                                            //int voucherno = voucher_no;
                                            DONumber = int.Parse(hdnvouchernumber.Value.TrimStart());
                                            obj.flag = 2;
                                            obj.tableName = "" + tblPrefix + "Voucher";
                                            if (drpDeliveryType.SelectedValue == "N")
                                            {
                                                obj.columnNm = "SUFFIX='" + string.Empty + "' , DO_No='" + DOC_NO + "' ,Lorry_No='" + TRUCK_NO + "', DOC_DATE='" + DOC_DATE + "' , AC_CODE='" + VOUCHER_BY + "',Unit_Code='" + GETPASS_CODE + "', BROKER_CODE='" + BROKER_CODE + "', " +
                                                " Quantal='" + QUANTAL + "',PACKING='" + PACKING + "' , BAGS='" + BAGS + "' ,GRADE='" + GRADE + "' , MILL_CODE='" + MILL_CODE + "', MILL_RATE='" + mill_rate + "' ,Sale_Rate='" + SALE_RATE + "', " +
                                                " FreightPerQtl='" + FRIEGHT_RATE + "', NARRATION1='" + vouchnarration + "' ,NARRATION2='" + NARRATION1 + "' , NARRATION3='" + NARRATION2 + "' , NARRATION4='" + NARRATION3 + " " + NARRATION4 + "',From_Place='" + From_Place + "',To_Place='" + To_Place + "'," +
                                                " Mill_Amount='" + MILL_AMOUNT + "',Delivery_Type='" + Delivery_Type + "',TRANSPORT_CODE='" + TRANSPORT_CODE + "',LESSDIFF='" + LESSDIFFOV + "',Diff_Rate='" + Diff_Rate + "',VOUCHER_AMOUNT='" + VOUCHER_AMOUNT + "',CASH_ACCOUNT='" + OVTransportCode + "',CASH_AMOUNT_RATE='" + FRIEGHT_RATE + "',CASH_AC_AMOUNT='" + MEMO_ADVANCE + "',Modified_By='" + user + "', " +
                                                " BROKRAGE='" + VoucherBrokrage + "',SERVICE_CHARGE='" + VoucherServiceCharge + "',L_RATE_DIFF='" + VoucherRateDiffRate + "',RATEDIFF='" + VoucherRateDiffAmt + "',Commission_Rate='" + VoucherBankCommRate + "',Commission_Amount='" + VoucherBankCommAmt + "',INTEREST='" + VoucherInterest + "',TRANSPORT_AMOUNT='" + VoucherTransport + "',OTHER_EXPENSES='" + VoucherOtherExpenses + "' " +
                                                " WHERE  Company_Code='" + Company_Code + "' AND Year_Code='" + Year_Code + "'  AND Tran_Type='" + "OV" + "' AND DOC_NO='" + DONumber + "'";
                                            }
                                            else
                                            {
                                                obj.columnNm = "SUFFIX='" + string.Empty + "' , DO_No='" + DOC_NO + "' ,Lorry_No='" + TRUCK_NO + "', DOC_DATE='" + DOC_DATE + "' , AC_CODE='" + VOUCHER_BY + "',Unit_Code='" + GETPASS_CODE + "', BROKER_CODE='" + BROKER_CODE + "', " +
                                               " Quantal='" + QUANTAL + "',PACKING='" + PACKING + "' , BAGS='" + BAGS + "' ,GRADE='" + GRADE + "' , MILL_CODE='" + MILL_CODE + "', MILL_RATE='" + mill_rate + "' ,Sale_Rate='" + SALE_RATE + "', " +
                                               " FreightPerQtl='0.00', NARRATION1='" + vouchnarration + "' ,NARRATION2='" + NARRATION1 + "' , NARRATION3='" + NARRATION2 + "' , NARRATION4='" + NARRATION3 + " " + NARRATION4 + "',From_Place='" + From_Place + "',To_Place='" + To_Place + "'," +
                                               " Mill_Amount='" + MILL_AMOUNT + "',Delivery_Type='" + Delivery_Type + "',TRANSPORT_CODE='" + TRANSPORT_CODE + "',LESSDIFF='" + LESSDIFFOV + "',Diff_Rate='" + Diff_Rate + "',VOUCHER_AMOUNT='" + VOUCHER_AMOUNT + "',CASH_ACCOUNT='" + OVTransportCode + "',CASH_AMOUNT_RATE='" + FRIEGHT_RATE + "',CASH_AC_AMOUNT='" + MEMO_ADVANCE + "',Modified_By='" + user + "', " +
                                               " BROKRAGE='" + VoucherBrokrage + "',SERVICE_CHARGE='" + VoucherServiceCharge + "',L_RATE_DIFF='" + VoucherRateDiffRate + "',RATEDIFF='" + VoucherRateDiffAmt + "',Commission_Rate='" + VoucherBankCommRate + "',Commission_Amount='" + VoucherBankCommAmt + "',INTEREST='" + VoucherInterest + "',TRANSPORT_AMOUNT='" + VoucherTransport + "',OTHER_EXPENSES='" + VoucherOtherExpenses + "' " +
                                               " WHERE  Company_Code='" + Company_Code + "' AND Year_Code='" + Year_Code + "'  AND Tran_Type='" + "OV" + "' AND DOC_NO='" + DONumber + "'";
                                            }
                                            obj.values = "none";
                                            ds = obj.insertAccountMaster(ref strRev);
                                            retValue = strRev;

                                            qry = "";
                                            qry = "update " + tblHead + " set voucher_no='" + DONumber + "' , voucher_type='" + "OV" + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + DOC_NO;
                                            ds = clsDAL.SimpleQuery(qry);
                                            gleder.LoadingVoucherGlederEffect("OV", DONumber, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                                            #endregion
                                        }

                                        #region Updating Memo
                                        int memono = int.Parse(hdnmemonumber.Value.TrimStart());
                                        obj.flag = 2;
                                        obj.tableName = tblHead;
                                        if (PDS == "P")
                                        {
                                            obj.columnNm = "DESP_TYPE='" + DESP_TYPE + "',DOC_DATE='" + DOC_DATE + "',MILL_CODE='" + MILL_CODE + "',GETPASSCODE='" + pdsunit + "',GRADE='" + GRADE + "',QUANTAL='" + QUANTAL + "',PACKING='" + PACKING + "', " +
                                               " BAGS='" + BAGS + "',TRUCK_NO='" + TRUCK_NO + "',transport='" + TRANSPORT_CODE + "',NARRATION1='" + myNarration + "',NARRATION2='" + NARRATION2 + "',NARRATION3='" + NARRATION3 + "',NARRATION4='" + NARRATION4 + "', " +
                                               " purc_no='" + purc_no + "',purc_order='" + purc_order + "',Ac_Code='" + pdsparty + "',FreightPerQtl='" + FRIEGHT_RATE + "',Freight_Amount='" + FRIEGHT_AMOUNT + "',vasuli_rate='" + VASULI_RATE + "',vasuli_amount='" + VASULI_AMOUNT + "',less='" + MEMO_ADVANCE + "',Modified_By='" + user + "',final_amout='" + FINAL_AMOUNT + "',driver_no='" + Driver_Mobile + "'" +
                                               " WHERE company_code='" + Company_Code + "' AND Year_Code='" + year_Code + "' AND tran_type='MM' AND DOC_NO='" + memono + "'";

                                        }
                                        else
                                        {
                                            obj.columnNm = "DESP_TYPE='" + DESP_TYPE + "',DOC_DATE='" + DOC_DATE + "',MILL_CODE='" + MILL_CODE + "',GETPASSCODE='" + GETPASS_CODE + "',GRADE='" + GRADE + "',QUANTAL='" + QUANTAL + "',PACKING='" + PACKING + "', " +
                                                " BAGS='" + BAGS + "',TRUCK_NO='" + TRUCK_NO + "',transport='" + TRANSPORT_CODE + "',NARRATION1='" + myNarration + "',NARRATION2='" + NARRATION2 + "',NARRATION3='" + NARRATION3 + "',NARRATION4='" + NARRATION4 + "', " +
                                                " purc_no='" + purc_no + "',purc_order='" + purc_order + "',Ac_Code='" + VOUCHER_BY + "',FreightPerQtl='" + FRIEGHT_RATE + "',Freight_Amount='" + FRIEGHT_AMOUNT + "',vasuli_rate='" + VASULI_RATE + "',vasuli_amount='" + VASULI_AMOUNT + "',less='" + MEMO_ADVANCE + "',Modified_By='" + user + "',final_amout='" + FINAL_AMOUNT + "',driver_no='" + Driver_Mobile + "'" +
                                                " WHERE company_code='" + Company_Code + "' AND Year_Code='" + year_Code + "' AND tran_type='MM' AND DOC_NO='" + memono + "'";
                                        }
                                        obj.values = "none";
                                        ds = obj.insertAccountMaster(ref strRev);
                                        retValue = strRev;
                                        #endregion
                                    }
                                    if (drpDOType.SelectedValue == "DO")
                                    {
                                        #region Updating Local Voucher
                                        if (drpDOType.SelectedValue == "DO")
                                        {
                                            DONumber = int.Parse(hdnvouchernumber.Value.TrimStart());
                                            if (DONumber != 0)
                                            {
                                                obj.flag = 2;
                                                obj.tableName = "" + tblPrefix + "Voucher";
                                                obj.columnNm = "SUFFIX='" + string.Empty.Trim() + "', DO_No='" + DOC_NO + "' , DOC_DATE='" + DOC_DATE + "' , AC_CODE='" + Ac_Code + "',Unit_Code='" + GETPASS_CODE + "', BROKER_CODE='" + BROKER_CODE + "' ," +
                                                " Quantal='" + QUANTAL + "',PACKING='" + PACKING + "' , BAGS='" + BAGS + "' ,GRADE='" + GRADE + "' , MILL_CODE='" + MILL_CODE + "', MILL_RATE='" + mill_rate + "',Sale_Rate='" + SALE_RATE + "',Mill_Amount='" + MILL_AMOUNT + "'," +
                                                " FREIGHT='" + FRIEGHT_AMOUNT + "', NARRATION1='" + NARRATION1 + vouchnarration + "' ,NARRATION2='" + NARRATION2 + " Lorry No:" + TRUCK_NO + "' , NARRATION3='" + NARRATION3 + "' , NARRATION4='" + NARRATION4 + TRUCK_NO + "' ," +
                                                " VOUCHER_AMOUNT='" + DIFF_AMOUNT + "' ,Diff_Amount='" + DIFF_RATE + "',Modified_By='" + user + "' where  DOC_NO=" + DONumber + " and Tran_Type='LV' and Company_Code='" + Company_Code + "' and Year_Code='" + Year_Code + "'";
                                                obj.values = "none";
                                                ds = obj.insertAccountMaster(ref strRev);
                                                retValue = strRev;

                                                //Gledger Effect for Local Voucher
                                                qry = "";
                                                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='LV' and DOC_NO=" + DONumber + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                                ds = clsDAL.SimpleQuery(qry);
                                                Int32 GID = 0;

                                                double LVVoucherAmount = ((SALE_RATE - mill_rate) * QUANTAL);
                                                if (LVVoucherAmount > 0)
                                                {
                                                    GID = GID + 1;
                                                    obj.flag = 1;
                                                    obj.tableName = GLedgerTable;
                                                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                    obj.values = "'LV','" + DONumber + "','" + DOC_DATE + "','" + VOUCHER_BY + "','" + GETPASS_CODE + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "','LV','" + voucher_no + "'";
                                                    ds = obj.insertAccountMaster(ref strRev);
                                                }
                                                else
                                                {
                                                    GID = GID + 1;
                                                    obj.flag = 1;
                                                    obj.tableName = GLedgerTable;
                                                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                    obj.values = "'LV','" + DONumber + "','" + DOC_DATE + "','" + VOUCHER_BY + "','" + GETPASS_CODE + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "','LV','" + voucher_no + "'";
                                                    ds = obj.insertAccountMaster(ref strRev);
                                                }
                                                // diffrance amount effect
                                                if (LVVoucherAmount > 0)
                                                {
                                                    //------------Credit effect
                                                    GID = GID + 1;
                                                    obj.flag = 1;
                                                    obj.tableName = GLedgerTable;
                                                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                    obj.values = "'LV','" + DONumber + "','" + DOC_DATE + "','" + int.Parse(clsGV.QUALITY_DIFF_AC) + "','" + 0 + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "','LV','" + voucher_no + "'";
                                                    ds = obj.insertAccountMaster(ref strRev);
                                                }
                                                else
                                                {
                                                    //------------Credit effect
                                                    GID = GID + 1;
                                                    obj.flag = 1;
                                                    obj.tableName = GLedgerTable;
                                                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                    obj.values = "'LV','" + DONumber + "','" + DOC_DATE + "','" + int.Parse(clsGV.QUALITY_DIFF_AC) + "','" + 0 + "','" + vouchnarration + "','" + LVVoucherAmount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "','LV','" + voucher_no + "'";
                                                    ds = obj.insertAccountMaster(ref strRev);
                                                }
                                        #endregion
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            #endregion
                        #region --------------------  Details --------------------

                        Int32 detail_Id = 0;
                        string ddType = "";
                        Int32 Bank_Code = 0;
                        string Narration = "";
                        double Amount = 0.0;
                        int Utr_no = 0;

                        if (strRev == "-1" || strRev == "-2")
                        {
                            Int32 GID = 0;
                            if (grdDetail.Rows.Count > 0)
                            {
                                for (int i = 0; i < grdDetail.Rows.Count; i++)
                                {
                                    detail_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                                    ddType = grdDetail.Rows[i].Cells[3].Text;
                                    Bank_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[4].Text);
                                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[6].Text);
                                    Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                                    Utr_no = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                                    GID = GID + 1;
                                    if (grdDetail.Rows[i].Cells[9].Text != "N" && grdDetail.Rows[i].Cells[9].Text != "R")
                                    {
                                        if (grdDetail.Rows[i].Cells[9].Text == "A")
                                        {
                                            obj.flag = 1;
                                            obj.tableName = tblDetails;
                                            obj.columnNm = "doc_no,ddType,Bank_Code,Narration,Amount,UTR_NO,Company_Code,Year_Code,Branch_Code,DO_No";
                                            obj.values = "'" + DOC_NO + "','" + ddType + "','" + Bank_Code + "','" + Narration + "','" + Amount + "','" + Utr_no + "','" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + DOC_NO + "' ";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);
                                            retValue = strRev;
                                        }
                                        if (grdDetail.Rows[i].Cells[9].Text == "U")
                                        {
                                            obj.flag = 2;
                                            obj.tableName = tblDetails;
                                            obj.columnNm = " ddType= '" + ddType + "',Bank_Code='" + Bank_Code + "',Narration='" + Narration + "',Amount='" + Amount + "',UTR_NO='" + Utr_no + "'" +
                                                " where doc_no='" + hdnf.Value + "' and detail_Id='" + detail_Id + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + year_Code;
                                            obj.values = "none";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);
                                            retValue = strRev;

                                        }
                                        if (grdDetail.Rows[i].Cells[9].Text == "D")
                                        {
                                            obj.flag = 3;
                                            obj.tableName = tblDetails;
                                            obj.columnNm = " doc_no='" + hdnf.Value + "' and detail_Id=" + detail_Id + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                                            obj.values = "none";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);
                                            retValue = strRev;
                                        }
                                    }
                                }
                            }
                            if (GETPASS_CODE != selfac)
                            {
                                gleder.DOUpdation(trnType, DOC_NO, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));
                            }
                        }
                        #endregion
                        hdnf.Value = txtdoc_no.Text;
                        if (retValue == "-1")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            hdnf.Value = txtdoc_no.Text;
                            this.makeEmptyForm("S");
                            qry = getDisplayQuery();
                            this.fetchRecord(qry);
                            hdnfpacking.Value = "0";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);
                        }
                        if (retValue == "-2" || retValue == "-3")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                            qry = getDisplayQuery();
                            this.fetchRecord(qry);
                            hdnfpacking.Value = "0";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                        }
                    }
                }
            }
            #endregion
        }
        catch (Exception exxx)
        {
            txtNARRATION4.Text = exxx.Message;
        }
    }
    #endregion
    private void MaxVoucher()
    {
        string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
        if (AUTO_VOUCHER == "YES")
        {
            if (drpDOType.SelectedValue == "DO")
            {
                int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher Where Tran_Type='LV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                hdnvouchernumber.Value = voucherno.ToString();
                lblVoucherType.Text = "LV";
            }
            if (drpDOType.SelectedValue == "DI")
            {
                if (txtGETPASS_CODE.Text == selfac)
                {
                    int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(doc_no),0)+1 from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    hdnvouchernumber.Value = voucherno.ToString();
                    lblVoucherType.Text = "PS";
                }
                else
                {
                    int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher Where Tran_Type='OV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    hdnvouchernumber.Value = voucherno.ToString();
                    lblVoucherType.Text = "OV";
                }
            }
        }
        else
        {
            hdnvouchernumber.Value = string.Empty;
        }
    }
    protected void txtmillRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmillRate.Text;
        strTextBox = "txtmillRate";
        csCalculations();
        calculation();
    }
    protected void txtPurcNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurcNo.Text;
        strTextBox = "txtPurcNo";
        if (strTextBox == "txtPurcNo")
        {
            //txtPurcOrder.Text = "1";
            setFocusControl(txtGETPASS_CODE);
            int i = 0;
            i++;
            if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
            {
                hdnfpacking.Value = Convert.ToString(i);
            }
            if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
            {
                string qry = "select Buyer,buyerbrokerfullname,Buyer_Party,salepartyfullname,Voucher_By,voucherbyfullname,Grade,Quantal,Packing,Bags," +
                    " Excise_Rate,Mill_Rate,Sale_Rate,Tender_DO,dofullname,Broker,brokerfullname,Commission_Rate as CR,Delivery_Type as DT,Payment_To,paymenttofullname from " + tblPrefix + "qryTenderList" +
                    " where Tender_No=" + txtPurcNo.Text + " and ID=" + txtPurcOrder.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                            {
                                string Buyer = dt.Rows[0]["Buyer"].ToString();
                                string broker = dt.Rows[0]["Buyer_Party"].ToString();
                                txtGETPASS_CODE.Text = dt.Rows[0]["Buyer"].ToString();
                                LBLGETPASS_NAME.Text = dt.Rows[0]["buyerbrokerfullname"].ToString();
                                txtvoucher_by.Text = dt.Rows[0]["Buyer"].ToString();
                                lblvoucherbyname.Text = dt.Rows[0]["buyerbrokerfullname"].ToString();
                                txtBroker_CODE.Text = broker;
                                LBLBROKER_NAME.Text = dt.Rows[0]["salepartyfullname"].ToString();
                                //if (Buyer != broker)
                                //{
                                //    if (broker != "0")
                                //    {
                                //        txtBroker_CODE.Text = dt.Rows[0]["Buyer_Party"].ToString();
                                //        LBLBROKER_NAME.Text = dt.Rows[0]["salepartyfullname"].ToString();
                                //    }
                                //    else
                                //    {
                                //        txtBroker_CODE.Text = "2";
                                //    }
                                //}
                                //else
                                //{
                                //    txtBroker_CODE.Text = "0";
                                //}
                            }
                            txtGRADE.Text = dt.Rows[0]["Grade"].ToString();
                            txtPACKING.Text = dt.Rows[0]["Packing"].ToString();
                            txtBAGS.Text = dt.Rows[0]["Bags"].ToString();
                            txtexcise_rate.Text = dt.Rows[0]["Excise_Rate"].ToString();
                            txtmillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
                            double Comm_rate = Convert.ToDouble(dt.Rows[0]["CR"].ToString());
                            //txtPartyCommission.Text = Convert.ToString(Comm_rate);
                            double SR = Convert.ToDouble(dt.Rows[0]["Sale_Rate"].ToString());
                            hdnfSaleRate.Value = Convert.ToString(SR);
                            string DT = dt.Rows[0]["DT"].ToString();
                            if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                            {
                                drpDeliveryType.SelectedValue = DT;
                            }
                            if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                            {
                                txtSALE_RATE.Text = (SR + Comm_rate).ToString();
                            }
                            txtDO_CODE.Text = dt.Rows[0]["Tender_DO"].ToString();
                            LBLDO_NAME.Text = dt.Rows[0]["dofullname"].ToString();
                            txtPurcNo.Enabled = false;

                            //ViewState["Payment_To"] = dt.Rows[0]["Payment_To"].ToString();
                            //ViewState["Payment_To_Name"] = dt.Rows[0]["paymenttofullname"].ToString();
                            //hdnfpacking.Value = "no";
                        }
                    }
                }
            }
        }
        calculation();
    }

    protected void btntxtPurcNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurcNo";
            pnlPopup.ScrollBars = ScrollBars.Both;
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    protected void btntxtcarporateSale_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtcarporateSale";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btntxtUTRNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUTRNo";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtcarporateSale_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcarporateSale.Text;
        strTextBox = "txtcarporateSale";
        hdnfpacking.Value = "2";
        csCalculations();
    }

    protected void txtUTRNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtUTRNo.Text;
        strTextBox = "txtUTRNo";
        double Utr_Balance = Convert.ToDouble(hdnfUtrBalance.Value.TrimStart());
        double Bank_Amount = Convert.ToDouble(hdnfMainBankAmount.Value.TrimStart());
        bool isValidated = true;
        if (txtUTRNo.Text != string.Empty)
        {
            string qry = "";
            qry = "select Year_Code from " + tblPrefix + "qryUTRBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_no=" + txtUTRNo.Text;
            string s = clsCommon.getString(qry);
            if (s != string.Empty)
            {
                isValidated = true;
                lblUTRYearCode.Text = s.ToString();
            }
            else
            {
                isValidated = false;
                txtUTRNo.Text = "";
                lblUTRYearCode.Text = "";
                setFocusControl(txtUTRNo);
                return;
            }
            if (Bank_Amount > Utr_Balance)
            {
                txtBANK_AMOUNT.Text = Utr_Balance.ToString();
                lblUtrBalnceError.Text = "Mill Amount Is Greater Than Utr Balance.Remaining UTR Balance Reflect to Amount.Please Select Another UTR";
                ViewState["ankush"] = "A";
            }
            else
            {
                double millamount = Convert.ToDouble(txtBANK_AMOUNT.Text);
                if (millamount < Bank_Amount)
                {
                    txtBANK_AMOUNT.Text = millamount.ToString();
                }
                else
                {
                    txtBANK_AMOUNT.Text = Bank_Amount.ToString();
                }
                lblUtrBalnceError.Text = "";
            }

        }
        if (strTextBox == "txtUTRNo")
        {
            setFocusControl(txtNARRATION);
        }
        //csCalculations();
    }

    protected void txtMillEmailID_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMillEmailID.Text;
        strTextBox = "txtMillEmailID";
        csCalculations();
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtdoc_no")
            {
                if (btntxtDOC_NO.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtdoc_no.Text = string.Empty;
                    txtdoc_no.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtdoc_no);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtDOC_NO.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select DO--";
                    tdDate.Visible = true;
                    string fromdt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    string todt = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    string qry = "select distinct(doc_no) as No,ISNULL(LEFT(millName,15),millShortName) as Mill,VoucherByname As Voucher_By,GetPassName as Getpass,quantal as Qntl,convert(varchar(10),doc_date,103) as Date" +
                        " ,voucher_no,TransportName from " + qryCommon + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'" +
                        " and doc_date between '" + fromdt + "' and '" + todt + "' and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millName like '%" + txtSearchText.Text + "%') order by doc_no desc";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                //txtSearchText.Text = searchString;
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='M' " +
                    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                //txtSearchText.Text = txtGETPASS_CODE.Text;
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select GetpassCode--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' and " +
                    AccountMasterTable + ".Ac_type!='B'" +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtvoucher_by")
            {

                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Voucher--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' and " + AccountMasterTable + ".Ac_type!='B' " +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGRADE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtDO_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Do--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' and " + AccountMasterTable + ".Ac_type!='B' " +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' and " + AccountMasterTable + ".Ac_type!='B' " +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select transport Code--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='T' " +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION1")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration --";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION2")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION3")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION4" || hdnfClosePopup.Value == "txtparty")
            {
                tdDate.Visible = false;
                string self_ac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (self_ac == txtGETPASS_CODE.Text)
                {
                    hdnfClosePopup.Value = "txtparty";
                    lblPopupHead.Text = "--Select Party--";
                    string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                  " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                  AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' and " + AccountMasterTable + ".Ac_type!='B' " +
                   " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                    this.showPopup(qry);
                }
                else
                {
                    hdnfClosePopup.Value = "txtNARRATION4";
                    lblPopupHead.Text = "--Select Narration--";
                    string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtBANK_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Bank--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPurcNo")
            {
                tdDate.Visible = false;
                if (txtMILL_CODE.Text != string.Empty)
                {
                    lblPopupHead.Text = "--Select No--";
                    string qry = "select Tender_No,Convert(varchar(10),Tender_Date,103) as Tender_Date,salepartyfullname as Party,buyerbrokerfullname as Party2,Mill_Rate,Sale_Rate,Buyer_Quantal,despatchqty,balance,doname,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,ID,Delivery_Type as DT from "
                        + qrypurc_No + " where  Mill_Code=" + txtMILL_CODE.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                        " and (Tender_No like '%" + txtSearchText.Text + "%' or  Tender_Date like '%" + txtSearchText.Text + "%' or salepartyfullname like  '%" + txtSearchText.Text + "%' or buyerbrokerfullname like '%" + txtSearchText.Text + "%' or doname like '%" + txtSearchText.Text + "%') and balance!=0 and Mill_Code=" + txtMILL_CODE.Text + "  order by Tender_No ,Tender_Date ";
                    this.showPopup(qry);
                }
                else
                {
                    setFocusControl(txtMILL_CODE);
                    pnlPopup.Style["display"] = "none";
                }
            }

            if (hdnfClosePopup.Value == "txtcarporateSale")
            {
                tdDate.Visible = false;
                string qry = "";
                lblPopupHead.Text = "--Select Carporate Sale No--";
                if (ViewState["mode"].ToString() == "I")
                {
                    qry = "select distinct(Doc_No),Doc_Date,partyName,UnitName,Sale_Rate,Po_Details,Quantal,Despatched,balance,SellingType  from " + qrycarporateSalebalance + " where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and (partyName like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or Doc_Date like '%" + txtSearchText.Text + "%')";
                }
                else
                {
                    qry = "select distinct(Doc_No),Doc_Date,partyName,UnitName,Sale_Rate,Po_Details,Quantal,Despatched,balance,SellingType  from " + qrycarporateSalebalance + " where   Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and (partyName like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or Doc_Date like '%" + txtSearchText.Text + "%')";
                }

                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtUTRNo")
            {
                tdDate.Visible = false;
                if (txtBANK_CODE.Text != string.Empty)
                {
                    lblPopupHead.Text = "--Select UTR No--";
                    string qry = "select doc_no,utr_no,bankname,UTRAmount,UsedAmt,balance,Year_Code  from " + tblPrefix + "qryUTRBalanceForDO where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code=" + txtBANK_CODE.Text;
                    DataSet ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string utrno = "";
                            string dtUtrNo = dt.Rows[i]["doc_no"].ToString();
                            double AmtTotal = 0.00;
                            for (int j = 0; j < grdDetail.Rows.Count; j++)
                            {
                                string grdUtrNo = grdDetail.Rows[j].Cells[8].Text.ToString();
                                string rowAction = grdDetail.Rows[j].Cells[9].Text.ToString();
                                if (dtUtrNo == grdUtrNo && rowAction == "A")
                                {
                                    double Amt = Convert.ToDouble(grdDetail.Rows[j].Cells[7].Text.ToString());
                                    AmtTotal += Amt;
                                    utrno = dtUtrNo;
                                }
                            }
                            if (dtUtrNo == utrno)
                            {
                                double balance = Convert.ToDouble(dt.Rows[i]["balance"].ToString());
                                double totalBal = balance - AmtTotal;
                                dt.Rows[i]["balance"] = totalBal;
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                if (dt.Rows[k]["balance"].ToString() == "0")
                                {
                                    dt.Rows[k].Delete();
                                }
                            }
                            grdPopup.DataSource = dt;
                            grdPopup.DataBind();
                            hdHelpPageCount.Value = grdPopup.PageCount.ToString();
                        }
                        else
                        {
                            grdPopup.DataSource = null;
                            grdPopup.DataBind();
                            hdHelpPageCount.Value = "0";
                        }
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                    }
                }
                else
                {
                    setFocusControl(txtBANK_CODE);
                    pnlPopup.Style["display"] = "none";
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Popup Button Code]
    protected void showPopup(string qry)
    {
        try
        {
            setFocusControl(txtSearchText);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdPopup.DataSource = dt;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = grdPopup.PageCount.ToString();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "kj", "javascript:SelectRow(0, {0});", true);
                        //grdPopup.Rows[0].Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});",grdPopup.Rows[0].RowIndex);
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                setFocusControl(txtGETPASS_CODE);
            }
            if (hdnfClosePopup.Value == "txtvoucher_by")
            {
                setFocusControl(txtvoucher_by);
            }
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {
                setFocusControl(txtBroker_CODE);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (hdnfClosePopup.Value == "txtDO_CODE")
            {
                setFocusControl(txtDO_CODE);
            }
            if (hdnfClosePopup.Value == "txtBANK_CODE")
            {
                setFocusControl(txtBANK_CODE);
            }

            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {

        bool isValidated = true;
        try
        {
            if (strTextBox == "txtdoc_no")
            {
                #region code
                try
                {
                    int n;
                    bool isNumeric = int.TryParse(txtdoc_no.Text, out n);
                    if (isNumeric == true)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string txtValue = "";
                        if (txtdoc_no.Text != string.Empty)
                        {
                            txtValue = txtdoc_no.Text;
                            string qry = "select * from " + tblHead + " where   Doc_No='" + txtValue + "' " +
                                "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";

                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        //Record Found
                                        hdnf.Value = dt.Rows[0]["doc_no"].ToString();

                                        if (ViewState["mode"] != null)
                                        {
                                            if (ViewState["mode"].ToString() == "I")
                                            {
                                                lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                                this.getMaxCode();
                                                txtdoc_no.Enabled = true;
                                                //hdnf.Value = txtdoc_no.Text;
                                                btnSave.Enabled = true;   //IMP
                                                setFocusControl(txtDOC_DATE);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();

                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtdoc_no.Enabled = true;
                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtDOC_DATE);
                                            txtdoc_no.Enabled = true;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtdoc_no.Text = string.Empty;
                                            setFocusControl(txtdoc_no);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                            setFocusControl(txtdoc_no);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Doc No is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtdoc_no.Text = string.Empty;
                        setFocusControl(txtdoc_no);
                    }
                }
                catch
                {

                }
                #endregion

                return;
            }
            if (strTextBox == "txtDOC_DATE")
            {
                if (txtDOC_DATE.Text != string.Empty)
                {
                    try
                    {
                        //string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                        //DateTime Start_Date = DateTime.Parse(HttpContext.Current.Session["Start_Date"].ToString());
                        //DateTime End_Date = DateTime.Parse(HttpContext.Current.Session["End_Date"].ToString());
                        //txtNARRATION1.Text = Start_Date.ToString();
                        //txtNARRATION2.Text = DateTime.Parse(HttpContext.Current.Session["Start_Date"].ToString()).ToString();
                        //txtNARRATION3.Text = End_Date.ToString();
                        //txtNARRATION4.Text = DateTime.Parse(HttpContext.Current.Session["End_Date"].ToString()).ToString();
                        if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
                        {
                            setFocusControl(txtcarporateSale);
                        }
                        else
                        {
                            txtDOC_DATE.Text = string.Empty;
                            setFocusControl(txtDOC_DATE);
                        }
                    }
                    catch (Exception exx)
                    {
                        txtNARRATION1.Text = exx.Message;
                        txtDOC_DATE.Text = string.Empty;
                        setFocusControl(txtDOC_DATE);
                    }
                }
                else
                {
                    setFocusControl(txtDOC_DATE);
                }

                return;
            }
            if (strTextBox == "drpDOType")
            {
                string s_item = "";
                s_item = drpDOType.SelectedValue;
                if (s_item == "DI")
                {
                    pnlgrdDetail.Enabled = true;
                    btnOpenDetailsPopup.Enabled = true;
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                    drpDeliveryType.Visible = true;

                    // txtUTRNo.Enabled = true;
                    //btntxtUTRNo.Enabled = true;

                }
                else
                {
                    drpDeliveryType.Visible = false;
                    pnlgrdDetail.Enabled = false;
                    btnOpenDetailsPopup.Enabled = false;
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();

                    //txtUTRNo.Text = "";
                    //txtUTRNo.Enabled = false;
                    //btntxtUTRNo.Enabled = false;
                }
                setFocusControl(txtMILL_CODE);
            }
            if (strTextBox == "txtMILL_CODE")
            {
                string millName = "";
                if (txtMILL_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtMILL_CODE.Text);
                    if (a == false)
                    {
                        btntxtMILL_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        millName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (millName != string.Empty)
                        {

                            txtMillEmailID.Text = clsCommon.getString("select Email_Id from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            txtMillMobile.Text = clsCommon.getString("select Mobile_No from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            LBLMILL_NAME.Text = millName;
                            setFocusControl(txtMillEmailID);
                        }
                        else
                        {
                            txtMILL_CODE.Text = string.Empty;
                            LBLMILL_NAME.Text = millName;
                            setFocusControl(txtMILL_CODE);
                        }
                    }
                }
                else
                {
                    LBLMILL_NAME.Text = "";
                    setFocusControl(txtMILL_CODE);
                }
                return;
            }

            if (strTextBox == "txtMillEmailID")
            {
                setFocusControl(txtPurcNo);
            }

            #region trash
            //if (strTextBox == "txtPurcNo")
            //{
            //    //  txtPurcOrder.Text = "1";
            //    setFocusControl(txtGETPASS_CODE);

            //    if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
            //    {
            //        string qry = "select Buyer,buyerbrokerfullname,Buyer_Party,salepartyfullname,Voucher_By,voucherbyfullname,Grade,Quantal,Packing,Bags," +
            //            " Excise_Rate,Mill_Rate,Sale_Rate,Tender_DO,dofullname,Broker,brokerfullname from " + tblPrefix + "qryTenderList" +
            //            " where Tender_No=" + txtPurcNo.Text + " and ID=" + txtPurcOrder.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            //        DataSet ds = new DataSet();
            //        DataTable dt = new DataTable();
            //        ds = clsDAL.SimpleQuery(qry);
            //        if (ds != null)
            //        {
            //            if (ds.Tables.Count > 0)
            //            {
            //                dt = ds.Tables[0];
            //                if (dt.Rows.Count > 0)
            //                {
            //                    if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
            //                    {
            //                        txtGETPASS_CODE.Text = dt.Rows[0]["Buyer"].ToString();
            //                        LBLGETPASS_NAME.Text = dt.Rows[0]["buyerbrokerfullname"].ToString();
            //                        txtvoucher_by.Text = dt.Rows[0]["Buyer_Party"].ToString();
            //                        lblvoucherbyname.Text = dt.Rows[0]["salepartyfullname"].ToString();
            //                    }
            //                    txtGRADE.Text = dt.Rows[0]["Grade"].ToString();
            //                    txtPACKING.Text = dt.Rows[0]["Packing"].ToString();
            //                    txtBAGS.Text = dt.Rows[0]["Bags"].ToString();
            //                    txtexcise_rate.Text = dt.Rows[0]["Excise_Rate"].ToString();
            //                    txtmillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
            //                    txtSALE_RATE.Text = dt.Rows[0]["Sale_Rate"].ToString();
            //                    txtDO_CODE.Text = dt.Rows[0]["Tender_DO"].ToString();
            //                    LBLDO_NAME.Text = dt.Rows[0]["dofullname"].ToString();
            //                    txtBroker_CODE.Text = dt.Rows[0]["Broker"].ToString();
            //                    LBLBROKER_NAME.Text = dt.Rows[0]["brokerfullname"].ToString();
            //                    txtPurcNo.Enabled = false;
            //                    //hdnfpacking.Value = "no";
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            if (strTextBox == "txtGETPASS_CODE")
            {
                string getPassName = "";
                if (txtGETPASS_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGETPASS_CODE.Text);
                    if (a == false)
                    {
                        btntxtGETPASS_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        getPassName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (getPassName != string.Empty)
                        {
                            LBLGETPASS_NAME.Text = getPassName;
                            if (txtvoucher_by.Text == "2")
                            {
                                txtvoucher_by.Text = txtGETPASS_CODE.Text;
                                lblvoucherbyname.Text = LBLGETPASS_NAME.Text;
                            }
                            setFocusControl(txtvoucher_by);
                        }
                        else
                        {
                            txtGETPASS_CODE.Text = string.Empty;
                            LBLGETPASS_NAME.Text = getPassName;
                            setFocusControl(txtGETPASS_CODE);
                        }
                    }
                }
                else
                {
                    LBLGETPASS_NAME.Text = "";
                    setFocusControl(txtGETPASS_CODE);
                }
            }
            if (strTextBox == "txtvoucher_by")
            {
                string vByName = "";
                if (txtvoucher_by.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtvoucher_by.Text);
                    if (a == false)
                    {
                        btntxtvoucher_by_Click(this, new EventArgs());
                    }
                    else
                    {
                        vByName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (vByName != string.Empty)
                        {

                            lblvoucherbyname.Text = vByName;
                            setFocusControl(txtGRADE);
                        }
                        else
                        {
                            txtvoucher_by.Text = string.Empty;
                            lblvoucherbyname.Text = vByName;
                            setFocusControl(txtvoucher_by);
                        }
                    }
                }
                else
                {
                    lblvoucherbyname.Text = "";
                    setFocusControl(txtGETPASS_CODE);
                }
            }
            if (strTextBox == "txtGRADE")
            {
                setFocusControl(txtquantal);
            }
            if (strTextBox == "txtmillRate")
            {
                //txtPACKING.Text = "50";
                setFocusControl(txtSALE_RATE);
            }
            if (strTextBox == "txtSALE_RATE")
            {
                setFocusControl(txtDIFF_AMOUNT);
            }
            if (strTextBox == "txtDIFF_AMOUNT")
            {
                setFocusControl(txtFrieght);
            }
            if (strTextBox == "txtquantal")
            {
                setFocusControl(txtPACKING);
            }
            if (strTextBox == "txtPACKING")
            {
                setFocusControl(txtBAGS);
            }
            if (strTextBox == "txtexcise_rate")
            {
                setFocusControl(txtFrieght);
            }
            if (strTextBox == "txtFrieght")
            {
                setFocusControl(drpCC);
            }
            if (strTextBox == "txtVasuliRate")
            {
                setFocusControl(txtVasuliRate1);
            }
            if (strTextBox == "txtVasuliRate1")
            {
                setFocusControl(txtDO_CODE);
            }
            if (strTextBox == "txtMemoAdvance")
            {
                setFocusControl(txtVasuliRate);
            }
            if (strTextBox == "txtexcise_rate")
            {
                setFocusControl(txtmillRate);
            }
            if (strTextBox == "txtDO_CODE")
            {
                string doname = "";
                if (txtDO_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtDO_CODE.Text);
                    if (a == false)
                    {
                        btntxtDO_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        doname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtDO_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (doname != string.Empty)
                        {
                            LBLDO_NAME.Text = doname;
                            setFocusControl(txtBroker_CODE);
                        }
                        else
                        {
                            txtDO_CODE.Text = string.Empty;
                            LBLDO_NAME.Text = doname;
                            setFocusControl(txtDO_CODE);
                        }
                    }
                }
                else
                {
                    LBLDO_NAME.Text = "";
                    setFocusControl(txtDO_CODE);
                }
            }
            if (strTextBox == "txtBroker_CODE")
            {
                string brokerName = "";
                if (txtBroker_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBroker_CODE.Text);
                    if (a == false)
                    {
                        btntxtBroker_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        brokerName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBroker_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (brokerName != string.Empty)
                        {

                            LBLBROKER_NAME.Text = brokerName;
                            setFocusControl(txtTruck_NO);
                        }
                        else
                        {
                            txtBroker_CODE.Text = string.Empty;
                            LBLBROKER_NAME.Text = brokerName;
                            setFocusControl(txtBroker_CODE);
                        }
                    }
                }
                else
                {
                    LBLBROKER_NAME.Text = "";
                    setFocusControl(txtBroker_CODE);
                }
            }
            if (strTextBox == "txtTruck_NO")
            {
                setFocusControl(txtDriverMobile);
            }
            if (strTextBox == "txtDriverMobile")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (strTextBox == "txtTRANSPORT_CODE")
            {
                string transportName = "";
                if (txtTRANSPORT_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtTRANSPORT_CODE.Text);
                    if (a == false)
                    {
                        btntxtTRANSPORT_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        transportName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (transportName != string.Empty)
                        {
                            LBLTRANSPORT_NAME.Text = transportName;
                            setFocusControl(txtNARRATION1);
                        }
                        else
                        {
                            txtTRANSPORT_CODE.Text = string.Empty;
                            LBLTRANSPORT_NAME.Text = transportName;
                            setFocusControl(txtTRANSPORT_CODE);
                        }
                    }
                }
                else
                {
                    LBLTRANSPORT_NAME.Text = "";
                    setFocusControl(txtTRANSPORT_CODE);
                }
            }
            if (strTextBox == "txtNARRATION1")
            {
                setFocusControl(txtNARRATION2);
            }
            if (strTextBox == "txtNARRATION2")
            {
                setFocusControl(txtNARRATION3);
            }
            if (strTextBox == "txtNARRATION3")
            {
                setFocusControl(txtNARRATION4);
            }
            if (strTextBox == "txtNARRATION4")
            {
                setFocusControl(btnOpenDetailsPopup);
            }
            if (strTextBox == "txtBANK_CODE")
            {
                string bankName = "";
                if (txtBANK_CODE.Text != string.Empty)
                {
                    bankName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBANK_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (bankName != string.Empty)
                    {

                        lblBank_name.Text = bankName;
                        setFocusControl(txtUTRNo);
                    }
                    else
                    {
                        txtBANK_CODE.Text = string.Empty;
                        lblBank_name.Text = bankName;
                        setFocusControl(txtBANK_CODE);
                    }
                }
                else
                {
                    lblBank_name.Text = "";
                    setFocusControl(txtBANK_CODE);
                }
            }
            if (strTextBox == "txtNARRATION")
            {
                setFocusControl(txtBANK_AMOUNT);
            }
            if (strTextBox == "txtBANK_AMOUNT")
            {
                setFocusControl(btnAdddetails);
            }

            if (strTextBox == "txtcarporateSale")
            {
                carporatesale();
            }
            //calculation();
        }
        catch
        {
        }
    }

    private void carporatesale()
    {
        if (txtcarporateSale.Text != string.Empty)
        {
            string qry = "select Year_Code from " + qrycarporateSalebalance + " where Doc_No=" + txtcarporateSale.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            string s = clsCommon.getString(qry);
            if (s != string.Empty)
            {
                lblCSYearCode.Text = s;
                qry = "select Ac_Code,partyName,Unit_name,Unit_Code,UnitName,BrokerCode,BrokerName,Sale_Rate,Po_Details,balance,SellingType from " + qrycarporateSalebalance + " where Doc_No=" + txtcarporateSale.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                DataSet ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                string sellingType = dt.Rows[0]["SellingType"].ToString();
                string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (sellingType == "C")
                {
                    txtGETPASS_CODE.Text = dt.Rows[0]["Unit_name"].ToString();
                    LBLGETPASS_NAME.Text = dt.Rows[0]["UnitName"].ToString();
                    txtvoucher_by.Text = dt.Rows[0]["Ac_Code"].ToString();
                    lblvoucherbyname.Text = dt.Rows[0]["partyName"].ToString();
                }
                else
                {
                    string getvocname = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + selfac + "");
                    txtGETPASS_CODE.Text = selfac;
                    LBLGETPASS_NAME.Text = getvocname;
                    txtvoucher_by.Text = selfac;
                    lblvoucherbyname.Text = getvocname;
                    hdnfPDSPartyCode.Value = dt.Rows[0]["Ac_Code"].ToString();
                    hdnfPDSUnitCode.Value = dt.Rows[0]["Unit_Code"].ToString();
                    lblPDSParty.Text = "Party: " + dt.Rows[0]["partyName"].ToString();
                }

                txtSALE_RATE.Text = dt.Rows[0]["Sale_Rate"].ToString();
                lblPoDetails.Text = "PO Details:- " + dt.Rows[0]["Po_Details"].ToString();
                txtBroker_CODE.Text = dt.Rows[0]["BrokerCode"].ToString();
                LBLBROKER_NAME.Text = dt.Rows[0]["BrokerName"].ToString();
                if (ViewState["mode"].ToString() == "I")
                {
                    txtquantal.Text = dt.Rows[0]["balance"].ToString();
                }
                drpDeliveryType.SelectedValue = "N";
                drpDeliveryType.Enabled = false;
                ddlFrieghtType.SelectedValue = "O";
                ddlFrieghtType.Enabled = false;
                drpDOType.SelectedValue = "DI";
                drpDOType.Enabled = false;
                setFocusControl(txtMILL_CODE);
            }
            else
            {
                txtcarporateSale.Text = "";
                lblCSYearCode.Text = "";
                setFocusControl(txtcarporateSale);
            }
        }
        else
        {
            txtcarporateSale.Text = "";
            lblCSYearCode.Text = "";
            setFocusControl(txtcarporateSale);
        }
    }

    private void calculation()
    {
        try
        {
            double qt = Convert.ToString(Math.Abs(Convert.ToDouble(txtquantal.Text))) != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.00;
            double qtl = Math.Abs(qt);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
            Int32 bags = 0;
            double saleRate = Convert.ToDouble("0" + txtSALE_RATE.Text);
            double millRate = Convert.ToDouble("0" + txtmillRate.Text);
            double mill_amount = Convert.ToDouble("0" + lblMillAmount.Text);
            double diffAmt = 0.00;
            double diff = 0.00;
            double frieght = Convert.ToDouble("0" + txtFrieght.Text);
            double vasuli_rate = Convert.ToDouble("0" + txtVasuliRate.Text);
            double frieght_amount = Convert.ToDouble("0" + txtFrieghtAmount.Text);
            double vasuli_amount = Convert.ToDouble("0" + txtVasuliAmount.Text);
            double vasuli_rate1 = Convert.ToDouble("0" + txtVasuliRate1.Text);
            double vasuli_amount1 = Convert.ToDouble("0" + txtVasuliAmount1.Text);
            if (qtl != 0 && packing != 0)
            {
                bags = Convert.ToInt32((qtl / packing) * 100);
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }

            if (saleRate != 0 && millRate != 0)
            {
                hdnfSaleRate.Value = Convert.ToString(saleRate);
                diff = saleRate - millRate;
                diffAmt = Math.Round(diff * qtl, 2);
                mill_amount = qtl * millRate;
            }
            lblDiffrate.Text = diff.ToString();
            txtDIFF_AMOUNT.Text = diffAmt.ToString();
            lblMillAmount.Text = Math.Round(mill_amount, 2).ToString();
            if (qtl != 0 && frieght != 0)
            {
                frieght_amount = qtl * frieght;
                txtFrieghtAmount.Text = frieght_amount.ToString();
                //txtMemoAdvance.Text = frieght_amount.ToString();
            }
            else
            {
                frieght_amount = 0.00;
                txtFrieghtAmount.Text = frieght_amount.ToString();
                //txtMemoAdvance.Text = frieght_amount.ToString();
            }

            if (qtl != 0 && vasuli_rate != 0)
            {
                vasuli_amount = qtl * vasuli_rate;
                txtVasuliAmount.Text = vasuli_amount.ToString();
            }
            else
            {
                vasuli_amount = 0.00;
                txtVasuliAmount.Text = vasuli_amount.ToString();
            }

            if (qtl != 0 && vasuli_rate1 != 0)
            {
                vasuli_amount1 = qtl * vasuli_rate1;
                txtVasuliAmount1.Text = vasuli_amount1.ToString();
            }
            else
            {
                vasuli_amount1 = 0.00;
                txtVasuliAmount1.Text = vasuli_amount1.ToString();
            }

            #region ---Add default row in grid---
            if (ViewState["currentTable"] == null)
            {
                if (drpDOType.SelectedValue == "DI")
                {
                    DataTable dt1 = new DataTable();
                    dt1.Columns.Add((new DataColumn("ID", typeof(Int32))));
                    #region [Write here columns]
                    dt1.Columns.Add((new DataColumn("Type", typeof(string))));
                    dt1.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                    dt1.Columns.Add((new DataColumn("BankName", typeof(string))));
                    dt1.Columns.Add((new DataColumn("Narration", typeof(string))));
                    dt1.Columns.Add((new DataColumn("Amount", typeof(double))));
                    dt1.Columns.Add((new DataColumn("UTR_NO", typeof(string))));
                    #endregion
                    dt1.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt1.Columns.Add((new DataColumn("SrNo", typeof(int))));
                    DataRow dr = dt1.NewRow();
                    dr["ID"] = 1;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 1;

                    dr["Type"] = "T";
                    if (txtMILL_CODE.Text != string.Empty)
                    {
                        Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                        dr["Bank_Code"] = Payment_To;
                    }
                    else
                    {
                        dr["Bank_Code"] = "0";
                    }
                    if (LBLMILL_NAME.Text != string.Empty)
                    {
                        string Payment_To_Name = clsCommon.getString("Select paymenttofullname from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        dr["BankName"] = Payment_To_Name;
                    }
                    else
                    {
                        dr["BankName"] = "";
                    }
                    dr["Narration"] = "Transfer Letter";
                    dr["Amount"] = Math.Round(qtl * Convert.ToDouble("0" + txtmillRate.Text), 2);

                    dr["UTR_NO"] = txtUTRNo.Text != string.Empty ? int.Parse(txtUTRNo.Text) : 0;
                    dt1.Rows.Add(dr);
                    ViewState["currentTable"] = dt1;
                    grdDetail.DataSource = dt1;
                    grdDetail.DataBind();
                }
            }
            else
            {
                DataTable dt1 = (DataTable)ViewState["currentTable"];
                DataRow dr = dt1.Rows[0];

                if (temp == "0" && drpDOType.SelectedValue == "DI")
                {
                    dr["ID"] = dt1.Rows[0]["ID"].ToString();
                    //dr["rowAction"] = "U";
                    dr["SrNo"] = 1;

                    dr["Type"] = "T";
                    if (txtMILL_CODE.Text != string.Empty)
                    {
                        Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                        dr["Bank_Code"] = Payment_To;
                    }
                    else
                    {
                        dr["Bank_Code"] = "0";
                    }
                    if (LBLMILL_NAME.Text != string.Empty)
                    {
                        string Payment_To_Name = clsCommon.getString("Select paymenttofullname from " + tblPrefix + "qryTenderList where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        dr["BankName"] = Payment_To_Name;
                    }
                    else
                    {
                        dr["BankName"] = "";
                    }
                    dr["Narration"] = dt1.Rows[0]["Narration"].ToString();
                    double millAmount = Math.Round(qtl * Convert.ToDouble("0" + txtmillRate.Text), 2);

                    if (dt1.Rows.Count == 1)
                    {
                        if (ViewState["mode"].ToString() == "I" || dt1.Rows[0]["UTR_NO"].ToString() == "0")
                        {
                            dt1.Rows[0]["Amount"] = millAmount;
                            if (ViewState["mode"].ToString() == "I")
                            {
                                dt1.Rows[0]["rowAction"] = "A";
                            }
                            else
                            {
                                dt1.Rows[0]["rowAction"] = "U";
                            }

                        }
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["UTR_NO"].ToString() != "0")
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if (dt1.Rows[i]["rowAction"].ToString() != "D")
                                {
                                    if (millAmount > Convert.ToDouble(dt1.Rows[i]["Amount"].ToString()))
                                    {
                                        millAmount = millAmount - Convert.ToDouble(dt1.Rows[i]["Amount"].ToString());
                                    }
                                    else
                                    {
                                        if (i < dt1.Rows.Count)
                                        {
                                            dt1.Rows[i]["Amount"] = millAmount;
                                            dt1.Rows[i]["rowAction"] = "U";

                                            for (int k = i; k < dt1.Rows.Count - 1; k++)
                                            {
                                                dt1.Rows[k + 1]["rowAction"] = "D";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #region comment
                    //dr["Amount"] = millAmount;
                    //string UTR_NO = dt1.Rows[0]["UTR_NO"].ToString();
                    //hdnfMainBankAmount.Value = millAmount.ToString();
                    //dr["UTR_NO"] = UTR_NO;
                    //if (dt1.Rows.Count > 1)
                    //{
                    //    dt1.Rows[0]["rowAction"] = "U";

                    //    for (int i = 0; i < dt1.Rows.Count; i++)
                    //    {
                    //        //if (i == 0)
                    //        //{
                    //        //    dt1.Rows[i + 1]["rowAction"] = "D";
                    //        //    //dt1.Rows[i + 1].Delete();
                    //        //    //dt1.AcceptChanges();
                    //        //}
                    //    }
                    //}
                    //if (ViewState["mode"].ToString() == "U")
                    //{
                    //    double thisSum = Convert.ToDouble(clsCommon.getString("select ISNULL(SUM(UsedAmt),0) from " + tblPrefix + "qryUTRBalance where doc_no=" + UTR_NO + " and mill_code=" + txtMILL_CODE.Text + " and DO_No=" + txtdoc_no.Text.Trim() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    //    double utrSum = Convert.ToDouble(clsCommon.getString("select ISNULL(SUM(balance),0) from " + tblPrefix + "qryUTRBalance where doc_no=" + UTR_NO + " and mill_code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    //    double utrBal = thisSum + utrSum;
                    //    if (millAmount > utrBal)
                    //    {
                    //        dt1.Rows[0]["Amount"] = utrBal;
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Mill Amount Is Greater You Need To Add Another Utr!');", true);
                    //    }
                    //}
                    #endregion

                    ViewState["currentTable"] = dt1;
                    grdDetail.DataSource = dt1;
                    grdDetail.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
            #endregion
    }
    #endregion

    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            string ccMail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Ac_Code='" + txtMILL_CODE.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string millEmail = ccMail + "," + txtMillEmailID.Text;
            string do_no = txtdoc_no.Text;
            if (do_no != string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + do_no + "','" + millEmail + "')", true);
                //Response.Redirect("../Report/rptDO.aspx?do_no=" + do_no + "&email=" + millEmail, true);
            }
        }
        catch
        {

        }
    }
    protected void txtFrieght_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFrieght.Text;
        strTextBox = "txtFrieght";
        csCalculations();
        calculation();
        if (ViewState["mode"].ToString() == "I")
        {
            txtMemoAdvance.Text = "0.00";
            txtMemoAdvanceRate.Text = "0.00";
        }
        //double frieght_amount = txtFrieghtAmount.Text != string.Empty ? Convert.ToDouble(txtFrieghtAmount.Text) : 0.00;

    }
    protected void txtVasuliRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVasuliRate.Text;
        strTextBox = "txtVasuliRate";
        csCalculations();
        calculation();
    }
    protected void txtMemoAdvance_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMemoAdvance.Text;
        strTextBox = "txtMemoAdvance";
        double qntl = Convert.ToDouble(txtquantal.Text);
        double memoadvane = Convert.ToDouble(txtMemoAdvance.Text);
        double rate = Math.Round((memoadvane / qntl), 2);
        txtMemoAdvanceRate.Text = rate.ToString();
        setFocusControl(txtVasuliRate);
    }
    protected void drpDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(txtMILL_CODE);
        if (drpDeliveryType.SelectedValue == "C")
        {
            ddlFrieghtType.SelectedValue = "P";
        }
        else
        {
            ddlFrieghtType.SelectedValue = "O";
        }
    }
    //protected void btnAddNewAccount_Click(object sender, EventArgs e)
    //{

    //}
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void txtDriverMobile_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDriverMobile.Text;
        strTextBox = "txtDriverMobile";
        csCalculations();
    }
    protected void btnOurDO_Click(object sender, EventArgs e)
    {
        try
        {
            string ccMail = clsCommon.getString("Select Email_Id_cc from " + tblPrefix + "AccountMaster where Ac_Code='" + txtMILL_CODE.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string millEmail = ccMail + "," + txtMillEmailID.Text;
            string do_no = txtdoc_no.Text;
            string a = "0";
            if (chkNoprintondo.Checked == true)
            {
                a = "1";
            }
            if (do_no != string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:od('" + do_no + "','" + millEmail + "','O','" + a + "')", true);
                //Response.Redirect("../Report/rptDO.aspx?do_no=" + do_no + "&email=" + millEmail, true);
            }
        }
        catch
        {

        }
    }
    protected void txtFrieghtAmount_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtINVOICE_NO_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnOpenDetailsPopup);
    }
    protected void txtVasuliRate1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVasuliRate1.Text;
        strTextBox = "txtVasuliRate1";
        csCalculations();
        calculation();
    }

    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        try
        {
            string driverMobile = txtDriverMobile.Text;
            string mobile = txtMillMobile.Text;
            string Cst_noC = clsCommon.getString("Select Cst_no from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string Tin_NoC = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string ECC_NoC = clsCommon.getString("Select ECC_No from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string millshort = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            string msg = millshort + " DO.No:" + txtdoc_no.Text + " " + LBLGETPASS_NAME.Text + " TIN:" + Tin_NoC + " ECC No:" + ECC_NoC + " CST:" + Cst_noC + " Qntl:" + txtquantal.Text + " Mill Rate:" + txtmillRate.Text + " Lorry:" + txtTruck_NO.Text + " " + txtNARRATION1.Text;
            string API = clsGV.msgAPI;
            string Url = API + "mobile=" + mobile + "&message=" + msg;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
            StreamReader reder = new StreamReader(resp.GetResponseStream());
            string respString = reder.ReadToEnd();
            reder.Close();
            resp.Close();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Successfully Sent!');", true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtPartyCommission_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //double SR = Convert.ToDouble(hdnfSaleRate.Value.TrimStart());
            //double MR = Convert.ToDouble(txtmillRate.Text);
            //double CR = Convert.ToDouble(txtPartyCommission.Text);
            //txtSALE_RATE.Text = Convert.ToString(MR + CR);
            //double qtl = Convert.ToDouble(txtquantal.Text);
            //double diff = 0.00;
            //double diffAmt = 0.00;
            //diff = SR - MR;
            //diffAmt = Math.Round(diff * qtl, 2);
            //calculation();
            setFocusControl(txtSALE_RATE);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtMemoAdvanceRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double rate = Convert.ToDouble(txtMemoAdvanceRate.Text);
            double qntl = Convert.ToDouble(txtquantal.Text);
            txtMemoAdvance.Text = Convert.ToString(rate * qntl);
            setFocusControl(txtMemoAdvance);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnVoucherOtherAmounts_Click(object sender, EventArgs e)
    {
        pnlVoucherEntries.Style["display"] = "block";
        setFocusControl(txtVoucherBrokrage);
    }
    protected void txtVoucherBrokrage_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherServiceCharge);
    }
    protected void txtVoucherServiceCharge_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherL_Rate_Diff);
    }
    protected void txtVoucherL_Rate_Diff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double voucratediff = Convert.ToDouble(txtVoucherL_Rate_Diff.Text);
            double quintal = Convert.ToDouble(txtquantal.Text);
            double ratediffamt = voucratediff * quintal;
            txtVoucherRATEDIFFAmt.Text = ratediffamt.ToString();
            setFocusControl(txtVoucherCommission_Rate);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtVoucherCommission_Rate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double bankcommrate = Convert.ToDouble(txtVoucherCommission_Rate.Text);
            double quintal = Convert.ToDouble(txtquantal.Text);
            double commamt = bankcommrate * quintal;
            txtVoucherBANK_COMMISSIONAmt.Text = commamt.ToString();
            setFocusControl(txtVoucherInterest);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtVoucherInterest_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherTransport_Amount);
    }
    protected void txtVoucherTransport_Amount_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherOTHER_Expenses);
    }
    protected void txtVoucherOTHER_Expenses_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnOk);
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        pnlVoucherEntries.Style["display"] = "none";
        setFocusControl(txtDO_CODE);
    }

    #region [getMaxCodeofvouchers]
    private void getvoucherscode(string tblName, string objCode, string trantype, string tblColumnType)
    {
        try
        {
            DataSet ds = null;
            string docno = "0";
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                if (trantype == "NULL")
                {
                    obj.tableName = tblName + " where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                else
                {
                    obj.tableName = tblName + " where  " + tblColumnType + "='" + trantype + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                obj.code = objCode;

                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    docno = ds.Tables[0].Rows[0][0].ToString();
                                    ViewState["maxval"] = docno;
                                }
                            }
                        }
                    }
                }
            }
            //return docno;
        }
        catch
        {
        }
    }
    #endregion

    static string returnNumber(string docno)
    {
        return docno;
    }
    //protected void btnPrintITCVoc_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string vtype = clsCommon.getString("select voucher_type from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
    //        if (vtype != "PS")
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kysdsd", "javascript:ITCV('" + lblVoucherNo.Text + "');", true);
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    protected void lnkMemo_Click(object sender, EventArgs e)
    {
        Session["MEMO_NO"] = lblMemoNo.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjs", "javascript:memo();", true);
    }
    protected void lnkVoucOrPurchase_Click(object sender, EventArgs e)
    {
        string vocno = lblVoucherNo.Text;
        string vocType = lblVoucherType.Text;
        if (vocType == "PS")
        {
            Session["PURC_NO"] = vocno;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:sugarpurchase();", true);
        }
        else
        {
            Session["VOUC_NO"] = vocno;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:loadingvoucher();", true);
        }
    }
    protected void lblsbnol_Click(object sender, EventArgs e)
    {
        Int32 sbno = lblSB_No.Text != string.Empty ? Convert.ToInt32(lblSB_No.Text) : 0;
        if (sbno != 0)
        {
            Session["SB_NO"] = sbno;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsdsd", "javascript:salebill();", true);
        }
    }
}
