using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Sugar_pgeCompanyparameter : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string qryAccountList = string.Empty;
    static WebControl objAsp = null;
    string autovoucher = string.Empty;
    string user = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "CompanyParameters";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = tblPrefix + "";
            qryAccountList = tblPrefix + "qryAccountsList";
            pnlPopup.Style["display"] = "none";
            user = Session["user"].ToString();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    pnlPopup.Style["display"] = "none";
                    ViewState["currentTable"] = null;
                    clsButtonNavigation.enableDisable("N");
                    //this.makeEmptyForm("N");
                    ViewState["mode"] = "I";
                    this.showLastRecord();
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
                pnlPopup.Style["display"] = "none";
                btnSave.Text = "Save";
                lblMsg.Text = string.Empty;
                #region set Business logic for save
                btntxtbrokrage_ac.Enabled = false;
                btntxtservice_charge_ac.Enabled = false;
                btntxtcommission_ac.Enabled = false;
                btntxtquality_diff_ac.Enabled = false;
                btntxtbank_commission_ac.Enabled = false;
                btntxtinterest_ac.Enabled = false;
                btntxttransport_ac.Enabled = false;
                btntxtsale_dalali_ac.Enabled = false;
                btntxtloading_charge_ac.Enabled = false;
                btntxtmotor_freight_ac.Enabled = false;
                btntxtpostage_ac.Enabled = false;
                btntxtother_amount_ac.Enabled = false;
                btntxtself_ac.Enabled = false;


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
                btnSave.Text = "Save";

                #region set Business logic for save
                btntxtbrokrage_ac.Enabled = true;
                btntxtservice_charge_ac.Enabled = true;
                btntxtcommission_ac.Enabled = true;
                btntxtquality_diff_ac.Enabled = true;
                btntxtbank_commission_ac.Enabled = true;
                btntxtinterest_ac.Enabled = true;
                btntxttransport_ac.Enabled = true;
                btntxtsale_dalali_ac.Enabled = true;
                btntxtloading_charge_ac.Enabled = true;
                btntxtmotor_freight_ac.Enabled = true;
                btntxtpostage_ac.Enabled = true;
                btntxtother_amount_ac.Enabled = true;
                btntxtself_ac.Enabled = true;
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
                #region set Business logic for save
                btntxtbrokrage_ac.Enabled = false;
                btntxtservice_charge_ac.Enabled = false;
                btntxtcommission_ac.Enabled = false;
                btntxtquality_diff_ac.Enabled = false;
                btntxtbank_commission_ac.Enabled = false;
                btntxtinterest_ac.Enabled = false;
                btntxttransport_ac.Enabled = false;
                btntxtsale_dalali_ac.Enabled = false;
                btntxtloading_charge_ac.Enabled = false;
                btntxtmotor_freight_ac.Enabled = false;
                btntxtpostage_ac.Enabled = false;
                btntxtother_amount_ac.Enabled = false;
                btntxtself_ac.Enabled = false;
                #endregion
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }

                lblMsg.Text = string.Empty;
                #region set Business logic for save
                btntxtbrokrage_ac.Enabled = true;
                btntxtservice_charge_ac.Enabled = true;
                btntxtcommission_ac.Enabled = true;
                btntxtquality_diff_ac.Enabled = true;
                btntxtbank_commission_ac.Enabled = true;
                btntxtinterest_ac.Enabled = true;
                btntxttransport_ac.Enabled = true;
                btntxtsale_dalali_ac.Enabled = true;
                btntxtloading_charge_ac.Enabled = true;
                btntxtmotor_freight_ac.Enabled = true;
                btntxtpostage_ac.Enabled = true;
                btntxtother_amount_ac.Enabled = true;
                btntxtself_ac.Enabled = true;
                #endregion
            }
            #region Always check this
            #endregion
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
            string qry = this.getDisplayQuery();
            this.fetchRecord(qry);
            btnSave.Enabled = true;
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
        query = "select count(*) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
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
        if (txtbrokrage_ac.Text != string.Empty)
        {
            #region check for next or previous record exist or not
            ds = new DataSet();
            dt = new DataTable();
            query = "SELECT top 1 [brokrage_ac] from " + tblHead + " where brokrage_ac>" + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' ORDER BY brokrage_ac asc  ";
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
            query = "SELECT top 1 [brokrage_ac] from " + tblHead + " where brokrage_ac<" + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' ORDER BY brokrage_ac asc  ";
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

        #endregion
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select brokrage_ac from " + tblHead + " where brokrage_ac=(select MIN(brokrage_ac) from " + tblHead + ") and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' ";
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
            if (txtbrokrage_ac.Text != string.Empty)
            {
                string query = "";
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
            if (txtbrokrage_ac.Text != string.Empty)
            {
                string query = "";
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
            query = "select brokrage_ac from " + tblHead + " where brokrage_ac=(select MAX(brokrage_ac) from " + tblHead + ") and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' ";
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

        pnlPopupDetails.Style["display"] = "none";
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");

        this.makeEmptyForm("E");

    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("N");
        this.makeEmptyForm("N");
        this.showLastRecord();
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
                        txtbrokrage_ac.Text = dt.Rows[0]["BROKRAGE_AC"].ToString();
                        lblBrokrage_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbrokrage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtservice_charge_ac.Text = dt.Rows[0]["SERVICE_CHARGE_AC"].ToString();
                        lblService_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtservice_charge_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtcommission_ac.Text = dt.Rows[0]["COMMISSION_AC"].ToString();
                        lblCommission_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtcommission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtquality_diff_ac.Text = dt.Rows[0]["QUALITY_DIFF_AC"].ToString();
                        lblQuality_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtquality_diff_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtbank_commission_ac.Text = dt.Rows[0]["BANK_COMMISSION_AC"].ToString();
                        lblBankcommission_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbank_commission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtinterest_ac.Text = dt.Rows[0]["INTEREST_AC"].ToString();
                        lblInterest_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtinterest_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txttransport_ac.Text = dt.Rows[0]["TRANSPORT_AC"].ToString();
                        lblTransport_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txttransport_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtsale_dalali_ac.Text = dt.Rows[0]["SALE_DALALI_AC"].ToString();
                        lblSale_dalali_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtsale_dalali_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtloading_charge_ac.Text = dt.Rows[0]["LOADING_CHARGE_AC"].ToString();
                        lblLoading_charge_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtloading_charge_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtmotor_freight_ac.Text = dt.Rows[0]["MOTOR_FREIGHT_AC"].ToString();
                        lblMotor_freight_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmotor_freight_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtpostage_ac.Text = dt.Rows[0]["POSTAGE_AC"].ToString();
                        lblPostage_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtpostage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtother_amount_ac.Text = dt.Rows[0]["OTHER_AMOUNT_AC"].ToString();
                        lblOther_name.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtother_amount_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtSaleReturn.Text = dt.Rows[0]["SaleReturnAc"].ToString();
                        lblSaleReturnAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleReturn.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtReturnSale.Text = dt.Rows[0]["ReturnSaleAc"].ToString();
                        lblReturnSaleAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtReturnSale.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtself_ac.Text = dt.Rows[0]["SELF_AC"].ToString();
                        lblSelf_ac.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtself_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        string branch1code = dt.Rows[0]["Branch1"].ToString();
                        lblBranch1Code.Text = branch1code;
                        txtBranch1.Text = clsCommon.getString("select Branch from BranchMaster where Branch_Id=" + branch1code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        string branch2code = dt.Rows[0]["Branch2"].ToString();
                        lblBranch2Code.Text = branch2code;
                        txtBranch2.Text = clsCommon.getString("select Branch from BranchMaster where Branch_Id=" + branch2code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


                        txtGstStateCode.Text = dt.Rows[0]["GSTStateCode"].ToString();
                        lbltxtGstStateName.Text = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");

                        txtCGSTAc.Text = dt.Rows[0]["CGSTAc"].ToString();
                        lbltxtCGSTAcName.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtSGSTAc.Text = dt.Rows[0]["SGSTAc"].ToString();
                        lbltxtSGSTAcName.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtIGSTAc.Text = dt.Rows[0]["IGSTAc"].ToString();
                        lbltxtIGSTAcName.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtPurchaseCGSTAc.Text = dt.Rows[0]["PurchaseCGSTAc"].ToString();
                        lbltxtPurchaseCGSTAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtPurchaseSGSTAc.Text = dt.Rows[0]["PurchaseSGSTAc"].ToString();
                        lbltxtPurchaseSGSTAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtPurchaseIGSTAc.Text = dt.Rows[0]["PurchaseIGSTAc"].ToString();
                        lbltxtPurchaseIGSTAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtRoundOff.Text = dt.Rows[0]["RoundOff"].ToString();
                        lblRoundOff.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtRoundOff.Text
                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


                        txtTCS.Text = dt.Rows[0]["TCS"].ToString();
                        txtPurchaseTCSAc.Text = dt.Rows[0]["PurchaseTCSAc"].ToString();
                        lbltxtPurchaseTCSAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code="
                            + txtPurchaseTCSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtSaleTCSAc.Text = dt.Rows[0]["SaleTCSAc"].ToString();
                        lbltxtSaleTCSAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code="
                            + txtSaleTCSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtSaleTDSAc.Text = dt.Rows[0]["SaleTDSAc"].ToString();
                        lbltxtSaleTDSAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code="
                            + txtSaleTDSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtPurchaseTDSAc.Text = dt.Rows[0]["PurchaseTDSAc"].ToString();
                        lbltxtPurchaseTDSAc.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code="
                            + txtPurchaseTDSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtSaleTDS.Text = dt.Rows[0]["SaleTDSRate"].ToString();
                        txtPurchaseTDS.Text = dt.Rows[0]["PurchaseTDSRate"].ToString();
                        txtTCSLimit.Text = dt.Rows[0]["TCSLimit"].ToString();

                        txtexcise_rate.Text = dt.Rows[0]["EXCISE_RATE"].ToString();
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
                        string chk = dt.Rows[0]["AutoVoucher"].ToString();
                        if (chk == "YES")
                        {
                            chkAutoVoucher.Checked = true;
                        }
                        else
                        {
                            chkAutoVoucher.Checked = false;
                        }
                        recordExist = true;
                        lblMsg.Text = "";
                    }
                }
            }
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    protected void txtRoundOff_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRoundOff.Text;
        strTextBox = "txtRoundOff";
        csCalculations();
    }
    protected void txtbtnRoundOff_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtRoundOff";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region gerDisplayQuery
    private string gerDisplayQuery()
    {
        try
        {
            string qryDisplay = "";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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


    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].Width = new Unit("80px");
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        }

    }
    #endregion

    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
           e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
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

    #region [txtbrokrage_ac_TextChanged]
    protected void txtbrokrage_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtbrokrage_ac.Text;
        strTextBox = "txtbrokrage_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtbrokrage_ac_Click]
    protected void btntxtbrokrage_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtbrokrage_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtservice_charge_ac_TextChanged]
    protected void txtservice_charge_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtservice_charge_ac.Text;
        strTextBox = "txtservice_charge_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtservice_charge_ac_Click]
    protected void btntxtservice_charge_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtservice_charge_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtcommission_ac_TextChanged]
    protected void txtcommission_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcommission_ac.Text;
        strTextBox = "txtcommission_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtcommission_ac_Click]
    protected void btntxtcommission_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtcommission_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtquality_diff_ac_TextChanged]
    protected void txtquality_diff_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtquality_diff_ac.Text;
        strTextBox = "txtquality_diff_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtquality_diff_ac_Click]
    protected void btntxtquality_diff_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtquality_diff_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtbank_commission_ac_TextChanged]
    protected void txtbank_commission_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtbank_commission_ac.Text;
        strTextBox = "txtbank_commission_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtbank_commission_ac_Click]
    protected void btntxtbank_commission_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtbank_commission_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtinterest_ac_TextChanged]
    protected void txtinterest_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtinterest_ac.Text;
        strTextBox = "txtinterest_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtinterest_ac_Click]
    protected void btntxtinterest_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtinterest_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txttransport_ac_TextChanged]
    protected void txttransport_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txttransport_ac.Text;
        strTextBox = "txttransport_ac";
        csCalculations();
    }
    #endregion

    #region [btntxttransport_ac_Click]
    protected void btntxttransport_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txttransport_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtsale_dalali_ac_TextChanged]
    protected void txtsale_dalali_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtsale_dalali_ac.Text;
        strTextBox = "txtsale_dalali_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtsale_dalali_ac_Click]
    protected void btntxtsale_dalali_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtsale_dalali_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtloading_charge_ac_TextChanged]
    protected void txtloading_charge_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtloading_charge_ac.Text;
        strTextBox = "txtloading_charge_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtloading_charge_ac_Click]
    protected void btntxtloading_charge_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtloading_charge_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtmotor_freight_ac_TextChanged]
    protected void txtmotor_freight_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmotor_freight_ac.Text;
        strTextBox = "txtmotor_freight_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtmotor_freight_ac_Click]
    protected void btntxtmotor_freight_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmotor_freight_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtpostage_ac_TextChanged]
    protected void txtpostage_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpostage_ac.Text;
        strTextBox = "txtpostage_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtpostage_ac_Click]
    protected void btntxtpostage_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtpostage_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtother_amount_ac_TextChanged]
    protected void txtother_amount_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtother_amount_ac.Text;
        strTextBox = "txtother_amount_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtother_amount_ac_Click]
    protected void btntxtother_amount_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtother_amount_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtself_ac_TextChanged]
    protected void txtself_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtself_ac.Text;
        strTextBox = "txtself_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtself_ac_Click]
    protected void btntxtself_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtself_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
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

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            if (hdnfClosePopup.Value == "txtBranch1")
            {
                lblPopupHead.Text = "--Select Branch--";
                string qry = "select Branch_Id,Branch from BranchMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                branchgrid(qry);
            }
            if (hdnfClosePopup.Value == "txtBranch2")
            {
                lblPopupHead.Text = "--Select Branch--";
                string qry = "select Branch_Id,Branch from BranchMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                branchgrid(qry);
            }
            if (hdnfClosePopup.Value == "txtbrokrage_ac")
            {
                lblPopupHead.Text = "--Select Brokrage Account--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtservice_charge_ac")
            {
                lblPopupHead.Text = "--Select Service Charge Ac--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtcommission_ac")
            {
                lblPopupHead.Text = "--Select Commission AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtquality_diff_ac")
            {
                lblPopupHead.Text = "--Select Quality DIff AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtbank_commission_ac")
            {
                lblPopupHead.Text = "--Select Bank Commssion AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtinterest_ac")
            {
                lblPopupHead.Text = "--Select Interest AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txttransport_ac")
            {
                lblPopupHead.Text = "--Select Transport AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtsale_dalali_ac")
            {
                lblPopupHead.Text = "--Select Sale Dalali AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtloading_charge_ac")
            {
                lblPopupHead.Text = "--Select Loading Charge AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtmotor_freight_ac")
            {
                lblPopupHead.Text = "--Select Motor Freight AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtpostage_ac")
            {
                lblPopupHead.Text = "--Select Postage AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtother_amount_ac")
            {
                lblPopupHead.Text = "--Select Other Amount AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtSaleReturn")
            {
                lblPopupHead.Text = "--Select Other Amount AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtReturnSale")
            {
                lblPopupHead.Text = "--Select Other Amount AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtself_ac")
            {
                lblPopupHead.Text = "--Select Self AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtCGSTAc" || hdnfClosePopup.Value == "txtSGSTAc" || hdnfClosePopup.Value == "txtIGSTAc" || hdnfClosePopup.Value == "txtPurchaseCGSTAc"
                || hdnfClosePopup.Value == "txtPurchaseSGSTAc" || hdnfClosePopup.Value == "txtPurchaseIGSTAc")
            {
                lblPopupHead.Text = "--Select GST Account--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGstStateCode")
            {
                string qry = "Select State_Code,State_Name from GSTStateMaster";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPurchaseTCSAc" || hdnfClosePopup.Value == "txtSaleTCSAc")
            {
                lblPopupHead.Text = "--Select TCS AC--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPurchaseTDSAc" || hdnfClosePopup.Value == "txtSaleTDSAc")
            {
                lblPopupHead.Text = "--Select TDS AC--";
                string qry = "";
                this.showPopup(qry);
            }
        }
        catch
        {
        }
    }

    private void branchgrid(string qry)
    {
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
                }
                else
                {
                    grdPopup.DataSource = null;
                    grdPopup.DataBind();
                }
            }
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
            if (qry.Trim() == string.Empty)
            {
                qry = "select a.Ac_Code,a.Ac_Name_E,c.city_name_e as city from " + tblPrefix + "AccountMaster a Left outer join " + tblPrefix + "CityMaster c on a.City_Code=c.city_code and a.Company_Code=c.company_code where a.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                   " (a.Ac_Code like '%" + txtSearchText.Text + "%' or a.Ac_Name_E like '%" + txtSearchText.Text + "%' or c.city_name_e  like '%" + txtSearchText.Text + "%' ) order by a.Ac_Code";
            }

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
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
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
            if (hdnfClosePopup.Value == "txtbrokrage_ac")
            {
                setFocusControl(txtbrokrage_ac);
            }
            if (hdnfClosePopup.Value == "txtservice_charge_ac")
            {
                setFocusControl(txtservice_charge_ac);
            }
            if (hdnfClosePopup.Value == "txtcommission_ac")
            {
                setFocusControl(txtcommission_ac);
            }
            if (hdnfClosePopup.Value == "txtquality_diff_ac")
            {
                setFocusControl(txtquality_diff_ac);
            }
            if (hdnfClosePopup.Value == "txtbank_commission_ac")
            {
                setFocusControl(txtbank_commission_ac);
            }
            if (hdnfClosePopup.Value == "txtinterest_ac")
            {
                setFocusControl(txtinterest_ac);
            }
            if (hdnfClosePopup.Value == "txttransport_ac")
            {
                setFocusControl(txttransport_ac);
            }
            if (hdnfClosePopup.Value == "txtsale_dalali_ac")
            {
                setFocusControl(txtsale_dalali_ac);
            }
            if (hdnfClosePopup.Value == "txtloading_charge_ac")
            {
                setFocusControl(txtloading_charge_ac);
            }
            if (hdnfClosePopup.Value == "txtmotor_freight_ac")
            {
                setFocusControl(txtmotor_freight_ac);
            }
            if (hdnfClosePopup.Value == "txtpostage_ac")
            {
                setFocusControl(txtpostage_ac);
            }
            if (hdnfClosePopup.Value == "txtother_amount_ac")
            {
                setFocusControl(txtother_amount_ac);
            }
            if (hdnfClosePopup.Value == "txtself_ac")
            {
                setFocusControl(txtself_ac);
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
        #region [Validation Part]
        bool isValidated = true;
        if (txtbrokrage_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbrokrage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtbrokrage_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtbrokrage_ac);
            return;
        }
        if (txtservice_charge_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtservice_charge_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtservice_charge_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtservice_charge_ac);
            return;
        }
        if (txtcommission_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtcommission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtcommission_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtcommission_ac);
            return;
        }
        if (txtquality_diff_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtquality_diff_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtquality_diff_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtquality_diff_ac);
            return;
        }
        if (txtbank_commission_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbank_commission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtbank_commission_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtbank_commission_ac);
            return;
        }
        if (txtinterest_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtinterest_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtinterest_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtinterest_ac);
            return;
        }
        if (txttransport_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txttransport_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txttransport_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txttransport_ac);
            return;
        }
        if (txtsale_dalali_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtsale_dalali_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtsale_dalali_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtsale_dalali_ac);
            return;
        }
        if (txtloading_charge_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtloading_charge_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtloading_charge_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtloading_charge_ac);
            return;
        }
        if (txtmotor_freight_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmotor_freight_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtmotor_freight_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtmotor_freight_ac);
            return;
        }
        if (txtpostage_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtpostage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtpostage_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtpostage_ac);
            return;
        }
        if (txtother_amount_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtother_amount_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtother_amount_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtother_amount_ac);
            return;
        }
        if (txtReturnSale.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtReturnSale.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtReturnSale);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtReturnSale);
            return;
        }
        if (txtSaleReturn.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleReturn.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtSaleReturn);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtSaleReturn);
            return;
        }
        if (txtself_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtself_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtself_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtself_ac);
            return;
        }

        if (txtGstStateCode.Text != string.Empty)
        {
            string str = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtGstStateCode);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtGstStateCode);
            return;
        }

        if (txtCGSTAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtCGSTAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtCGSTAc);
            return;
        }
        if (txtSGSTAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtSGSTAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtSGSTAc);
            return;
        }
        if (txtIGSTAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtIGSTAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtIGSTAc);
            return;
        }

        if (txtPurchaseCGSTAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPurchaseCGSTAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPurchaseCGSTAc);
            return;
        }

        if (txtPurchaseSGSTAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPurchaseSGSTAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPurchaseSGSTAc);
            return;
        }
        if (txtPurchaseIGSTAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPurchaseIGSTAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPurchaseIGSTAc);
            return;
        }


        if (txtPurchaseTCSAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseTCSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPurchaseTCSAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPurchaseTCSAc);
            return;
        }
        if (txtSaleTCSAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleTCSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtSaleTCSAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtSaleTCSAc);
            return;
        }
        if (txtTCS.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtTCS);
            return;
        }
        if (txtSaleTDSAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleTDSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtSaleTDSAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtSaleTDSAc);
            return;
        }
        if (txtPurchaseTDSAc.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseTDSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPurchaseTDSAc);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPurchaseTDSAc);
            return;
        }

        if (txtSaleTDS.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtSaleTDS);
            return;
        }
        if (txtPurchaseTDS.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPurchaseTDS);
            return;
        }
        if (txtTCSLimit.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtTCSLimit);
            return;
        }

        //if (txtexcise_rate.Text != string.Empty)
        //{
        //    isValidated = true;
        //}
        //else
        //{
        //    isValidated = false;
        //    setFocusControl(txtexcise_rate);
        //    return;
        //}
        #endregion

        #region -Head part declearation
        Int32 BROKRAGE_AC = txtbrokrage_ac.Text != string.Empty ? Convert.ToInt32(txtbrokrage_ac.Text) : 0;
        Int32 SERVICE_CHARGE_AC = txtservice_charge_ac.Text != string.Empty ? Convert.ToInt32(txtservice_charge_ac.Text) : 0;
        Int32 COMMISSION_AC = txtcommission_ac.Text != string.Empty ? Convert.ToInt32(txtcommission_ac.Text) : 0;
        Int32 QUALITY_DIFF_AC = txtquality_diff_ac.Text != string.Empty ? Convert.ToInt32(txtquality_diff_ac.Text) : 0;
        Int32 BANK_COMMISSION_AC = txtbank_commission_ac.Text != string.Empty ? Convert.ToInt32(txtbank_commission_ac.Text) : 0;
        Int32 INTEREST_AC = txtinterest_ac.Text != string.Empty ? Convert.ToInt32(txtinterest_ac.Text) : 0;
        Int32 TRANSPORT_AC = txttransport_ac.Text != string.Empty ? Convert.ToInt32(txttransport_ac.Text) : 0;
        Int32 SALE_DALALI_AC = txtsale_dalali_ac.Text != string.Empty ? Convert.ToInt32(txtsale_dalali_ac.Text) : 0;
        Int32 LOADING_CHARGE_AC = txtloading_charge_ac.Text != string.Empty ? Convert.ToInt32(txtloading_charge_ac.Text) : 0;
        Int32 MOTOR_FREIGHT_AC = txtmotor_freight_ac.Text != string.Empty ? Convert.ToInt32(txtmotor_freight_ac.Text) : 0;
        Int32 POSTAGE_AC = txtpostage_ac.Text != string.Empty ? Convert.ToInt32(txtpostage_ac.Text) : 0;
        Int32 OTHER_AMOUNT_AC = txtother_amount_ac.Text != string.Empty ? Convert.ToInt32(txtother_amount_ac.Text) : 0;
        Int32 SaleReturnAc = txtSaleReturn.Text != string.Empty ? Convert.ToInt32(txtSaleReturn.Text) : 0;
        Int32 ReturnSaleAc = txtReturnSale.Text != string.Empty ? Convert.ToInt32(txtReturnSale.Text) : 0;
        Int32 SELF_AC = txtself_ac.Text != string.Empty ? Convert.ToInt32(txtself_ac.Text) : 0;
        double EXCISE_RATE = txtexcise_rate.Text != string.Empty ? Convert.ToDouble(txtexcise_rate.Text) : 0;
        Int32 Branch1 = lblBranch1Code.Text != string.Empty ? Convert.ToInt32(lblBranch1Code.Text) : 0;
        Int32 Branch2 = lblBranch2Code.Text != string.Empty ? Convert.ToInt32(lblBranch2Code.Text) : 0;
        Int32 GSTStateCode = txtGstStateCode.Text != string.Empty ? Convert.ToInt32(txtGstStateCode.Text) : 0;
        Int32 CGSTAc = txtCGSTAc.Text != string.Empty ? Convert.ToInt32(txtCGSTAc.Text) : 0;
        Int32 SGSTAc = txtSGSTAc.Text != string.Empty ? Convert.ToInt32(txtSGSTAc.Text) : 0;
        Int32 IGSTAc = txtIGSTAc.Text != string.Empty ? Convert.ToInt32(txtIGSTAc.Text) : 0;
        Int32 PurchaseCGSTAc = txtPurchaseCGSTAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseCGSTAc.Text) : 0;
        Int32 PurchaseSGSTAc = txtPurchaseSGSTAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseSGSTAc.Text) : 0;
        Int32 PurchaseIGSTAc = txtPurchaseIGSTAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseIGSTAc.Text) : 0;
        Int32 roundoff = txtRoundOff.Text != string.Empty ? Convert.ToInt32(txtRoundOff.Text) : 0;


        double TCS = txtTCS.Text != string.Empty ? Convert.ToDouble(txtTCS.Text) : 0.000;
        Int32 PurchaseTCSAc = txtPurchaseTCSAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseTCSAc.Text) : 0;
        Int32 SaleTCSAc = txtSaleTCSAc.Text != string.Empty ? Convert.ToInt32(txtSaleTCSAc.Text) : 0;

        Int32 PurchaseTDSAc = txtPurchaseTDSAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseTDSAc.Text) : 0;
        Int32 SaleTDSAc = txtSaleTDSAc.Text != string.Empty ? Convert.ToInt32(txtSaleTDSAc.Text) : 0;
        double SaleTDS = txtSaleTDS.Text != string.Empty ? Convert.ToDouble(txtSaleTDS.Text) : 0.00;
        double PurchaseTDS = txtPurchaseTDS.Text != string.Empty ? Convert.ToDouble(txtPurchaseTDS.Text) : 0.00;
        double TCSLimit = txtTCSLimit.Text != string.Empty ? Convert.ToDouble(txtTCSLimit.Text) : 0.00;

        if (chkAutoVoucher.Checked == true)
        {
            autovoucher = "YES";
        }
        else
        {
            autovoucher = "NO";
        }
        string retValue = string.Empty;
        string strRev = string.Empty;
        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int year_Code = Convert.ToInt32(Session["year"].ToString());
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
        #endregion-End of Head part declearation

        #region save Head Master
        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
        {
            if (ViewState["mode"] != null)
            {
                DataSet ds = new DataSet();
                string s = clsCommon.getString("select Year_Code from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (s == string.Empty)
                {
                    obj.flag = 1;
                    obj.tableName = tblHead;
                    obj.columnNm = "BROKRAGE_AC,SERVICE_CHARGE_AC,COMMISSION_AC,QUALITY_DIFF_AC,BANK_COMMISSION_AC,INTEREST_AC,TRANSPORT_AC,SALE_DALALI_AC,LOADING_CHARGE_AC," +
                        "MOTOR_FREIGHT_AC,POSTAGE_AC,OTHER_AMOUNT_AC,SaleReturnAc,ReturnSaleAc,SELF_AC,EXCISE_RATE,Company_Code,Year_Code,Branch_Code,Created_By,AutoVoucher," +
                        "Branch1,Branch2,GSTStateCode,CGSTAc,SGSTAc,IGSTAc,PurchaseCGSTAc,PurchaseSGSTAc,PurchaseIGSTAc,RoundOff,TCS,PurchaseTCSAc,SaleTCSAc,PurchaseTDSAc,SaleTDSAc,SaleTDSRate,PurchaseTDSRate,TCSLimit";
                    obj.values = "'" + BROKRAGE_AC + "','" + SERVICE_CHARGE_AC + "','" + COMMISSION_AC + "','" + QUALITY_DIFF_AC + "','" + BANK_COMMISSION_AC + "','" + INTEREST_AC
                        + "','" + TRANSPORT_AC + "','" + SALE_DALALI_AC + "','" + LOADING_CHARGE_AC + "','" + MOTOR_FREIGHT_AC + "','" + POSTAGE_AC + "','" + OTHER_AMOUNT_AC +
                        "','" + SaleReturnAc + "','" + ReturnSaleAc + "','" + SELF_AC + "','" + EXCISE_RATE + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + autovoucher
                        + "','" + Branch1 + "','" + Branch2 + "','" + GSTStateCode + "','" + CGSTAc + "','" + SGSTAc + "','" + IGSTAc + "','" + PurchaseCGSTAc + "','"
                        + PurchaseSGSTAc + "','" + PurchaseIGSTAc + "','" + roundoff + "','" + TCS + "','" + PurchaseTCSAc + "','" + SaleTCSAc + "','" + PurchaseTDSAc + "','" + SaleTDSAc + "','" + SaleTDS + "','" + PurchaseTDS + "','" + TCSLimit + "'";
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }
                else
                {
                    //Update Mode
                    obj.flag = 2;
                    obj.tableName = tblHead;
                    obj.columnNm = "BROKRAGE_AC='" + BROKRAGE_AC + "',SERVICE_CHARGE_AC='" + SERVICE_CHARGE_AC + "',COMMISSION_AC='" + COMMISSION_AC +
                        "',QUALITY_DIFF_AC='" + QUALITY_DIFF_AC + "',BANK_COMMISSION_AC='" + BANK_COMMISSION_AC + "',INTEREST_AC='" + INTEREST_AC + "',TRANSPORT_AC='"
                        + TRANSPORT_AC + "',SALE_DALALI_AC='" + SALE_DALALI_AC + "',LOADING_CHARGE_AC='" + LOADING_CHARGE_AC + "',MOTOR_FREIGHT_AC='" + MOTOR_FREIGHT_AC
                        + "',POSTAGE_AC='" + POSTAGE_AC + "',OTHER_AMOUNT_AC='" + OTHER_AMOUNT_AC + "',SaleReturnAc='" + SaleReturnAc + "',ReturnSaleAc='" + ReturnSaleAc
                        + "',SELF_AC='" + SELF_AC + "',EXCISE_RATE='" + EXCISE_RATE + "',Modified_By='" + user + "',AutoVoucher='" + autovoucher + "', Branch1='" + Branch1
                        + "',Branch2='" + Branch2 + "',GSTStateCode='" + GSTStateCode + "',CGSTAc='" + CGSTAc + "',SGSTAc='" + SGSTAc + "',IGSTAc='" + IGSTAc + "',PurchaseCGSTAc='"
                        + PurchaseCGSTAc + "',PurchaseSGSTAc='" + PurchaseSGSTAc + "',PurchaseIGSTAc='" + PurchaseIGSTAc
                        + "',RoundOff='" + roundoff + "',TCS='" + TCS + "',PurchaseTCSAc='" + PurchaseTCSAc + "',SaleTCSAc='" + SaleTCSAc + "',PurchaseTDSAc='" + PurchaseTDSAc + "',SaleTDSAc='" + SaleTDSAc + "',SaleTDSRate='" + SaleTDS
                        + "',PurchaseTDSRate='" + PurchaseTDS + "',TCSLimit='" + TCSLimit + "' where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    obj.values = "none";
                    ds = new DataSet();
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }

                Session["TCS_Rate"] = TCS;
                Session["SaleTCSAc"] = SaleTCSAc;
                Session["PurchaseTCSAc"] = PurchaseTCSAc;

                Session["SaleTDS_Rate"] = SaleTDS;
                Session["PurchaseTDS_Rate"] = PurchaseTDS;
                Session["SaleTDSAc"] = SaleTDSAc;
                Session["PurchaseTDSAc"] = PurchaseTDSAc;
                Session["TCSLimit"] = TCSLimit;

                if (retValue == "-1")
                {
                    string qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);
                }
                if (retValue == "-2" || retValue == "-3")
                {
                    string qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                }
            }
        }
        #endregion
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtbrokrage_ac")
            {
                if (txtbrokrage_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbrokrage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblBrokrage_name.Text = str;
                        setFocusControl(txtservice_charge_ac);
                    }
                    else
                    {
                        lblBrokrage_name.Text = string.Empty;
                        txtbrokrage_ac.Text = string.Empty;
                        setFocusControl(txtbrokrage_ac);
                    }
                }
                else
                {
                    lblBrokrage_name.Text = string.Empty;
                    txtbrokrage_ac.Text = string.Empty;
                    setFocusControl(txtbrokrage_ac);
                }
            }

            if (strTextBox == "txtRoundOff")
            {
                if (txtRoundOff.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable
                        + " where Ac_Code=" + txtRoundOff.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblRoundOff.Text = str;
                        setFocusControl(txtRoundOff);
                    }
                    else
                    {
                        lblRoundOff.Text = string.Empty;
                        txtRoundOff.Text = string.Empty;
                        setFocusControl(txtRoundOff);
                    }
                }
                else
                {
                    lblRoundOff.Text = string.Empty;
                    txtRoundOff.Text = string.Empty;
                    setFocusControl(txtRoundOff);
                }
            }

            if (strTextBox == "txtservice_charge_ac")
            {
                if (txtservice_charge_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtservice_charge_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblService_name.Text = str;
                        setFocusControl(txtcommission_ac);
                    }
                    else
                    {
                        lblService_name.Text = string.Empty;
                        txtservice_charge_ac.Text = string.Empty;
                        setFocusControl(txtservice_charge_ac);
                    }
                }
                else
                {
                    lblService_name.Text = string.Empty;
                    txtservice_charge_ac.Text = string.Empty;
                    setFocusControl(txtservice_charge_ac);
                }
            }
            if (strTextBox == "txtcommission_ac")
            {
                if (txtcommission_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtcommission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblCommission_name.Text = str;
                        setFocusControl(txtquality_diff_ac);
                    }
                    else
                    {
                        lblCommission_name.Text = string.Empty;
                        txtcommission_ac.Text = string.Empty;
                        setFocusControl(txtcommission_ac);
                    }
                }
                else
                {
                    lblCommission_name.Text = string.Empty;
                    txtcommission_ac.Text = string.Empty;
                    setFocusControl(txtcommission_ac);
                }
            }
            if (strTextBox == "txtquality_diff_ac")
            {
                if (txtquality_diff_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtquality_diff_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblQuality_name.Text = str;
                        setFocusControl(txtbank_commission_ac);
                    }
                    else
                    {
                        lblQuality_name.Text = string.Empty;
                        txtquality_diff_ac.Text = string.Empty;
                        setFocusControl(txtquality_diff_ac);
                    }
                }
                else
                {
                    lblQuality_name.Text = string.Empty;
                    txtquality_diff_ac.Text = string.Empty;
                    setFocusControl(txtquality_diff_ac);
                }
            }
            if (strTextBox == "txtbank_commission_ac")
            {
                if (txtbank_commission_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbank_commission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblBankcommission_name.Text = str;
                        setFocusControl(txtinterest_ac);
                    }
                    else
                    {
                        lblBankcommission_name.Text = string.Empty;
                        txtbank_commission_ac.Text = string.Empty;
                        setFocusControl(txtbank_commission_ac);
                    }
                }
                else
                {
                    lblBankcommission_name.Text = string.Empty;
                    txtbank_commission_ac.Text = string.Empty;
                    setFocusControl(txtbank_commission_ac);
                }
            }
            if (strTextBox == "txtinterest_ac")
            {
                if (txtinterest_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtinterest_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblInterest_name.Text = str;
                        setFocusControl(txttransport_ac);
                    }
                    else
                    {
                        lblInterest_name.Text = string.Empty;
                        txtinterest_ac.Text = string.Empty;
                        setFocusControl(txtinterest_ac);
                    }
                }
                else
                {
                    lblInterest_name.Text = string.Empty;
                    txtinterest_ac.Text = string.Empty;
                    setFocusControl(txtinterest_ac);
                }
            }
            if (strTextBox == "txttransport_ac")
            {
                if (txttransport_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txttransport_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblTransport_name.Text = str;
                        setFocusControl(txtsale_dalali_ac);
                    }
                    else
                    {
                        lblTransport_name.Text = string.Empty;
                        txttransport_ac.Text = string.Empty;
                        setFocusControl(txttransport_ac);
                    }
                }
                else
                {
                    lblTransport_name.Text = string.Empty;
                    txttransport_ac.Text = string.Empty;
                    setFocusControl(txttransport_ac);
                }
            }
            if (strTextBox == "txtsale_dalali_ac")
            {
                if (txtsale_dalali_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtsale_dalali_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblSale_dalali_name.Text = str;
                        setFocusControl(txtloading_charge_ac);
                    }
                    else
                    {
                        lblSale_dalali_name.Text = string.Empty;
                        txtsale_dalali_ac.Text = string.Empty;
                        setFocusControl(txtsale_dalali_ac);
                    }
                }
                else
                {
                    lblSale_dalali_name.Text = string.Empty;
                    txtsale_dalali_ac.Text = string.Empty;
                    setFocusControl(txtsale_dalali_ac);
                }
            }
            if (strTextBox == "txtloading_charge_ac")
            {
                if (txtloading_charge_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtloading_charge_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblLoading_charge_name.Text = str;
                        setFocusControl(txtmotor_freight_ac);
                    }
                    else
                    {
                        lblLoading_charge_name.Text = string.Empty;
                        txtloading_charge_ac.Text = string.Empty;
                        setFocusControl(txtloading_charge_ac);
                    }
                }
                else
                {
                    lblLoading_charge_name.Text = string.Empty;
                    txtloading_charge_ac.Text = string.Empty;
                    setFocusControl(txtloading_charge_ac);
                }
            }
            if (strTextBox == "txtmotor_freight_ac")
            {
                if (txtmotor_freight_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmotor_freight_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblMotor_freight_name.Text = str;
                        setFocusControl(txtpostage_ac);
                    }
                    else
                    {
                        lblMotor_freight_name.Text = string.Empty;
                        txtmotor_freight_ac.Text = string.Empty;
                        setFocusControl(txtmotor_freight_ac);
                    }
                }
                else
                {
                    lblMotor_freight_name.Text = string.Empty;
                    txtmotor_freight_ac.Text = string.Empty;
                    setFocusControl(txtmotor_freight_ac);
                }
            }
            if (strTextBox == "txtpostage_ac")
            {
                if (txtpostage_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtpostage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblPostage_name.Text = str;
                        setFocusControl(txtother_amount_ac);
                    }
                    else
                    {
                        lblPostage_name.Text = string.Empty;
                        txtpostage_ac.Text = string.Empty;
                        setFocusControl(txtpostage_ac);
                    }
                }
                else
                {
                    lblPostage_name.Text = string.Empty;
                    txtpostage_ac.Text = string.Empty;
                    setFocusControl(txtpostage_ac);
                }
            }
            if (strTextBox == "txtother_amount_ac")
            {
                if (txtother_amount_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtother_amount_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblOther_name.Text = str;
                        setFocusControl(txtSaleReturn);
                    }
                    else
                    {
                        lblOther_name.Text = string.Empty;
                        txtother_amount_ac.Text = string.Empty;
                        setFocusControl(txtother_amount_ac);
                    }
                }
                else
                {
                    lblOther_name.Text = string.Empty;
                    txtother_amount_ac.Text = string.Empty;
                    setFocusControl(txtother_amount_ac);
                }
            }
            if (strTextBox == "txtSaleReturn")
            {
                if (txtSaleReturn.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleReturn.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblSaleReturnAc.Text = str;
                        setFocusControl(txtReturnSale);
                    }
                    else
                    {
                        lblSaleReturnAc.Text = string.Empty;
                        txtSaleReturn.Text = string.Empty;
                        setFocusControl(txtSaleReturn);
                    }
                }
                else
                {
                    lblSaleReturnAc.Text = string.Empty;
                    txtSaleReturn.Text = string.Empty;
                    setFocusControl(txtSaleReturn);
                }
            }
            if (strTextBox == "txtReturnSale")
            {
                if (txtReturnSale.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtReturnSale.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblReturnSaleAc.Text = str;
                        setFocusControl(txtself_ac);
                    }
                    else
                    {
                        lblReturnSaleAc.Text = string.Empty;
                        txtReturnSale.Text = string.Empty;
                        setFocusControl(txtReturnSale);
                    }
                }
                else
                {
                    lblReturnSaleAc.Text = string.Empty;
                    txtReturnSale.Text = string.Empty;
                    setFocusControl(txtReturnSale);
                }
            }
            if (strTextBox == "txtself_ac")
            {
                if (txtself_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtself_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblSelf_ac.Text = str;
                        setFocusControl(txtexcise_rate);
                    }
                    else
                    {
                        lblSelf_ac.Text = string.Empty;
                        txtself_ac.Text = string.Empty;
                        setFocusControl(txtself_ac);
                    }
                }
                else
                {
                    lblSelf_ac.Text = string.Empty;
                    txtself_ac.Text = string.Empty;
                    setFocusControl(txtself_ac);
                }
            }

            if (strTextBox == "txtGstStateCode")
            {
                if (txtGstStateCode.Text != string.Empty)
                {
                    string str = clsCommon.getString("Select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");
                    if (str != string.Empty)
                    {
                        lbltxtGstStateName.Text = str;
                        setFocusControl(txtCGSTAc);
                    }
                    else
                    {
                        lbltxtGstStateName.Text = string.Empty;
                        txtGstStateCode.Text = string.Empty;
                        setFocusControl(txtGstStateCode);
                    }
                }
                else
                {
                    lbltxtGstStateName.Text = string.Empty;
                    txtGstStateCode.Text = string.Empty;
                    setFocusControl(txtGstStateCode);
                }
            }
            if (strTextBox == "txtCGSTAc")
            {
                if (txtCGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtCGSTAcName.Text = str;
                        setFocusControl(txtSGSTAc);
                    }
                    else
                    {
                        lbltxtCGSTAcName.Text = string.Empty;
                        txtCGSTAc.Text = string.Empty;
                        setFocusControl(txtCGSTAc);
                    }
                }
                else
                {
                    lbltxtCGSTAcName.Text = string.Empty;
                    txtCGSTAc.Text = string.Empty;
                    setFocusControl(txtCGSTAc);
                }
            }
            if (strTextBox == "txtSGSTAc")
            {
                if (txtSGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtSGSTAcName.Text = str;
                        setFocusControl(txtIGSTAc);
                    }
                    else
                    {
                        lbltxtSGSTAcName.Text = string.Empty;
                        txtSGSTAc.Text = string.Empty;
                        setFocusControl(txtSGSTAc);
                    }
                }
                else
                {
                    lbltxtSGSTAcName.Text = string.Empty;
                    txtSGSTAc.Text = string.Empty;
                    setFocusControl(txtSGSTAc);
                }
            }
            if (strTextBox == "txtIGSTAc")
            {
                if (txtIGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtIGSTAcName.Text = str;
                        setFocusControl(txtexcise_rate);
                    }
                    else
                    {
                        lbltxtIGSTAcName.Text = string.Empty;
                        txtIGSTAc.Text = string.Empty;
                        setFocusControl(txtIGSTAc);
                    }
                }
                else
                {
                    lbltxtIGSTAcName.Text = string.Empty;
                    txtIGSTAc.Text = string.Empty;
                    setFocusControl(txtIGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseCGSTAc")
            {
                if (txtPurchaseCGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtPurchaseCGSTAc.Text = str;
                        setFocusControl(txtPurchaseSGSTAc);
                    }
                    else
                    {
                        lbltxtPurchaseCGSTAc.Text = string.Empty;
                        txtPurchaseCGSTAc.Text = string.Empty;
                        setFocusControl(txtPurchaseCGSTAc);
                    }
                }
                else
                {
                    lbltxtPurchaseCGSTAc.Text = string.Empty;
                    txtPurchaseCGSTAc.Text = string.Empty;
                    setFocusControl(txtPurchaseCGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseSGSTAc")
            {
                if (txtPurchaseSGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtPurchaseSGSTAc.Text = str;
                        setFocusControl(txtPurchaseIGSTAc);
                    }
                    else
                    {
                        lbltxtPurchaseSGSTAc.Text = string.Empty;
                        txtPurchaseSGSTAc.Text = string.Empty;
                        setFocusControl(txtPurchaseSGSTAc);
                    }
                }
                else
                {
                    lbltxtPurchaseSGSTAc.Text = string.Empty;
                    txtPurchaseSGSTAc.Text = string.Empty;
                    setFocusControl(txtPurchaseSGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseIGSTAc")
            {
                if (txtPurchaseIGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtPurchaseIGSTAc.Text = str;
                        setFocusControl(btnSave);
                    }
                    else
                    {
                        lbltxtPurchaseIGSTAc.Text = string.Empty;
                        txtPurchaseIGSTAc.Text = string.Empty;
                        setFocusControl(txtPurchaseIGSTAc);
                    }
                }
                else
                {
                    lbltxtPurchaseIGSTAc.Text = string.Empty;
                    txtPurchaseIGSTAc.Text = string.Empty;
                    setFocusControl(txtPurchaseIGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseTCSAc")
            {
                if (txtPurchaseTCSAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseTCSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtPurchaseTCSAc.Text = str;
                        setFocusControl(txtSaleTCSAc);
                    }
                    else
                    {
                        lbltxtPurchaseTCSAc.Text = string.Empty;
                        txtPurchaseTCSAc.Text = string.Empty;
                        setFocusControl(txtPurchaseTCSAc);
                    }
                }
                else
                {
                    lbltxtPurchaseTCSAc.Text = string.Empty;
                    txtPurchaseTCSAc.Text = string.Empty;
                    setFocusControl(txtPurchaseTCSAc);
                }
            }
            if (strTextBox == "txtSaleTCSAc")
            {
                if (txtSaleTCSAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleTCSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtSaleTCSAc.Text = str;
                        setFocusControl(btnSave);
                    }
                    else
                    {
                        lbltxtSaleTCSAc.Text = string.Empty;
                        txtSaleTCSAc.Text = string.Empty;
                        setFocusControl(txtSaleTCSAc);
                    }
                }
                else
                {
                    lbltxtSaleTCSAc.Text = string.Empty;
                    txtSaleTCSAc.Text = string.Empty;
                    setFocusControl(txtSaleTCSAc);
                }
            }

            if (strTextBox == "txtPurchaseTDSAc")
            {
                if (txtPurchaseTDSAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseTDSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtPurchaseTDSAc.Text = str;
                        setFocusControl(txtSaleTDSAc);
                    }
                    else
                    {
                        lbltxtPurchaseTDSAc.Text = string.Empty;
                        txtPurchaseTDSAc.Text = string.Empty;
                        setFocusControl(txtPurchaseTDSAc);
                    }
                }
                else
                {
                    lbltxtPurchaseTDSAc.Text = string.Empty;
                    txtPurchaseTDSAc.Text = string.Empty;
                    setFocusControl(txtPurchaseTDSAc);
                }
            }
            if (strTextBox == "txtSaleTDSAc")
            {
                if (txtSaleTDSAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleTDSAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lbltxtSaleTDSAc.Text = str;
                        setFocusControl(txtSaleTDSAc);
                    }
                    else
                    {
                        lbltxtSaleTDSAc.Text = string.Empty;
                        txtSaleTDSAc.Text = string.Empty;
                        setFocusControl(txtSaleTDSAc);
                    }
                }
                else
                {
                    lbltxtSaleTDSAc.Text = string.Empty;
                    txtSaleTDSAc.Text = string.Empty;
                    setFocusControl(txtSaleTDSAc);
                }
            }

            if (strTextBox == "txtexcise_rate")
            {
                setFocusControl(btnSave);
            }

        }
        catch
        {
        }
    }
    #endregion

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    protected void btnBranch1_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBranch1";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btnBranch2_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBranch2";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void txtSaleReturn_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSaleReturn.Text;
        strTextBox = "txtSaleReturn";
        csCalculations();
    }
    protected void btnSaleReturn_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSaleReturn";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void txtReturnSale_TextChanged(object sender, EventArgs e)
    {
        searchString = txtReturnSale.Text;
        strTextBox = "txtReturnSale";
        csCalculations();
    }
    protected void btnReturnSale_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtReturnSale";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region [txtGstStateCode_TextChanged]
    protected void txtGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGstStateCode.Text;
        strTextBox = "txtGstStateCode";
        csCalculations();
    }
    #endregion

    #region [btntxtGstStateCode_Click]
    protected void btntxtGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtCGSTAc_TextChanged]
    protected void txtCGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGSTAc.Text;
        strTextBox = "txtCGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtCGSTAc_Click]
    protected void btntxtCGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSGSTAc_TextChanged]
    protected void txtSGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGSTAc.Text;
        strTextBox = "txtSGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtSGSTAc_Click]
    protected void btntxtSGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtIGSTAc_TextChanged]
    protected void txtIGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGSTAc.Text;
        strTextBox = "txtIGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtIGSTAc_Click]
    protected void btntxtIGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtIGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtPurchaseCGSTAc_TextChanged]
    protected void txtPurchaseCGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseCGSTAc.Text;
        strTextBox = "txtPurchaseCGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseCGSTAc_Click]
    protected void btntxtPurchaseCGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseCGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtPurchaseSGSTAc_TextChanged]
    protected void txtPurchaseSGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseSGSTAc.Text;
        strTextBox = "txtPurchaseSGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseSGSTAc_Click]
    protected void btntxtPurchaseSGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseSGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPurchaseIGSTAc_TextChanged]
    protected void txtPurchaseIGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseIGSTAc.Text;
        strTextBox = "txtPurchaseIGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseIGSTAc_Click]
    protected void btntxtPurchaseIGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseIGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtexcise_rate_TextChanged]
    protected void txtTCS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTCS.Text;
        strTextBox = "txtTCS";
        csCalculations();
    }
    #endregion

    #region [txtPurchaseTCSAc_TextChanged]
    protected void txtPurchaseTCSAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseTCSAc.Text;
        strTextBox = "txtPurchaseTCSAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseTCSAc_Click]
    protected void btntxtPurchaseTCSAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseTCSAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSaleTCSAc_TextChanged]
    protected void txtSaleTCSAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSaleTCSAc.Text;
        strTextBox = "txtSaleTCSAc";
        csCalculations();
    }
    #endregion

    #region [btntxtSaleTCSAc_Click]
    protected void btntxtSaleTCSAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSaleTCSAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSaleTDSAc_TextChanged]
    protected void txtSaleTDSAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSaleTDSAc.Text;
        strTextBox = "txtSaleTDSAc";
        csCalculations();
    }
    #endregion

    #region [btntxtSaleTDSAc_Click]
    protected void btntxtSaleTDSAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSaleTDSAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    #region [txtPurchaseTDSAc_TextChanged]
    protected void txtPurchaseTDSAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseTDSAc.Text;
        strTextBox = "txtPurchaseTDSAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseTDSAc_Click]
    protected void btntxtPurchaseTDSAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseTDSAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    #region [txtSaleTDS_TextChanged]
    protected void txtSaleTDS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSaleTDS.Text;
        strTextBox = "txtSaleTDS";
        csCalculations();
    }
    #endregion
    #region [txtPurchaseTDS_TextChanged]
    protected void txtPurchaseTDS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseTDS.Text;
        strTextBox = "txtPurchaseTDS";
        csCalculations();
    }
    #endregion
    #region [txtTCSLimit_TextChanged]
    protected void txtTCSLimit_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTCSLimit.Text;
        strTextBox = "txtTCSLimit";
        csCalculations();
    }
    #endregion
}
