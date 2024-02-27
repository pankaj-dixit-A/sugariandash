using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
public partial class Sugar_pgeSugarsaleReturnForGST : System.Web.UI.Page
{

    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string SystemMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string trntype = "RS";
    string user = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    DataTable dt;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "SugarSaleReturn";
            tblDetails = tblPrefix + "sugarsaleDetailsReturn";
            AccountMasterTable = tblPrefix + "AccountMaster";
            SystemMasterTable = tblPrefix + "SystemMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            qryCommon = tblPrefix + "qrySugarSaleListReturn";
            qryAccountList = tblPrefix + "qryAccountsList";
            GLedgerTable = tblPrefix + "GLEDGER";
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
                    this.makeEmptyForm("N");
                    ViewState["mode"] = "I";
                    if (Session["RS_NO"] != null)
                    {
                        hdnf.Value = Session["RS_NO"].ToString();
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        this.enableDisableNavigateButtons();
                        Session["RS_NO"] = null;
                    }
                    else
                    {
                        this.showLastRecord();
                    }
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

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
            if (a == false)
            {
                hdnf.Value = txtEditDoc_No.Text;
                pnlPopup.Style["display"] = "block";
                txtSearchText.Text = txtEditDoc_No.Text;
                hdnfClosePopup.Value = "txtEditDoc_No";
                btnSearch_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                string qry = "select *,CONVERT(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no='" + txtEditDoc_No.Text 
                    + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and tran_type='" + trntype + "'"; ;
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);
                //pnlgrdDetail.Enabled = true;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                                    txtDOC_NO.Text = ds.Tables[0].Rows[0][0].ToString();
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
                pnlPopup.Style["display"] = "none";
                btnOpenDetailsPopup.Enabled = false;
                txtEditDoc_No.Enabled = true;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                #region logic
                LblPartyname.Text = "";
                lblUnitName.Text = "";
                LBLMILLNAME.Text = "";
                LBLBROKERNAME.Text = "";
                lblGSTRateName.Text = "";
                lblTransportName.Text = "";
                btntxtGSTRateCode.Enabled = false;
                btntxtPURCNO.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtBROKER.Enabled = false;
                btntxtdoc_no.Enabled = false;
                btnTransport.Enabled = false;
                calenderExtenderDate.Enabled = false;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                btnPrintSaleBill.Enabled = true;
                btntxtUnitcode.Enabled = false;
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
                btntxtdoc_no.Text = "Change No";
                btntxtdoc_no.Enabled = true;
                txtEditDoc_No.Enabled = false;
                #region set Business logic for save
                LblPartyname.Text = "";
                lblUnitName.Text = "";
                LBLMILLNAME.Text = "";
                LBLBROKERNAME.Text = "";
                lblGSTRateName.Text = "";
                lblTransportName.Text = "";
                btntxtAC_CODE.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtBROKER.Enabled = true;
                btntxtGSTRateCode.Enabled = true;
                btntxtPURCNO.Enabled = true;
                calenderExtenderDate.Enabled = true;
                btnTransport.Enabled = true;
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                btnOpenDetailsPopup.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = true;
                btnPrintSaleBill.Enabled = false;
                btntxtUnitcode.Enabled = true;
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                btntxtUnitcode.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                pnlgrdDetail.Enabled = false;
                btnPrintSaleBill.Enabled = true;
                txtEditDoc_No.Enabled = true;
                #region logic
                btntxtAC_CODE.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtBROKER.Enabled = false;
                calenderExtenderDate.Enabled = false;
                btntxtGSTRateCode.Enabled = false;
                btntxtPURCNO.Enabled = false;
                btnTransport.Enabled = false;
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                btnOpenDetailsPopup.Enabled = true;
                btntxtUnitcode.Enabled = true;
                pnlgrdDetail.Enabled = true;
                btnPrintSaleBill.Enabled = false;
                #region logic
                btntxtAC_CODE.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtBROKER.Enabled = true;
                calenderExtenderDate.Enabled = true;
                btntxtPURCNO.Enabled = true;
                btntxtGSTRateCode.Enabled = true;
                btnTransport.Enabled = true;
                txtEditDoc_No.Enabled = false;
                #endregion
            }

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
            qry = "select max(doc_no) as doc_no from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                        qry = getDisplayQuery();
                        bool recordExist = this.fetchRecord(qry);
                        if (recordExist == true)
                        {
                            btnEdit.Focus();
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

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
        if (txtDOC_NO.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " ORDER BY doc_no asc  ";
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " ORDER BY doc_no asc  ";
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") " +
                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
            if (txtDOC_NO.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            if (txtDOC_NO.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            query = "select DOC_NO from " + tblHead + " where DOC_NO=(select MAX(DOC_NO) from " + tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") " +
                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
        txtDOC_NO.Enabled = false;
        pnlPopupDetails.Style["display"] = "none";
        setFocusControl(txtPURCNO);
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");
        txtDOC_NO.Enabled = false;
        setFocusControl(txtITEM_CODE);
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string currentDoc_No = txtDOC_NO.Text;
                string qry = "";
                DataSet ds = new DataSet();
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + trntype + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString()) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                qry = "delete from " + tblHead + " where doc_no=" + currentDoc_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                qry = "delete from " + tblDetails + " where doc_no=" + currentDoc_No + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                ds = clsDAL.SimpleQuery(qry);

                int inmultiple = Convert.ToInt16(clsCommon.getString("Select ISNULL(MAX(Doc_No),0) from tblMultipleReturnPurchase where RS_No=" + currentDoc_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                if (inmultiple > 0)
                {
                    string qryDeleteMultiple = "delete from tblMultipleReturnPurchase where RS_No=" + currentDoc_No + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                    clsDAL.SimpleQuery(qryDeleteMultiple);
                }

                //string DONumber = Convert.ToString(lblDONo.Text);
                //if (!string.IsNullOrWhiteSpace(DONumber))
                //{
                //    if (DONumber == "0")
                //    { }
                //    else
                //    {
                //        ds = new DataSet();
                //        qry = "update " + tblPrefix + "deliveryorder SET SB_No=NULL where doc_no='" + DONumber + "' and tran_type='DO'";
                //        ds = clsDAL.SimpleQuery(qry);
                //    }
                //}

                string query = "SELECT top 1 [doc_no] from " + tblHead + "  where doc_no>" + Convert.ToInt32(currentDoc_No) +
                        "   and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' " +
                         " ORDER BY doc_no asc";

                hdnf.Value = clsCommon.getString(query);

                if (hdnf.Value == string.Empty)
                {
                    query = "SELECT top 1 [doc_no] from " + tblHead + "  where doc_no<" + Convert.ToInt32(currentDoc_No) +
                         "  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' " +
                        " ORDER BY doc_no desc  ";
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
                this.enableDisableNavigateButtons();
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
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        this.enableDisableNavigateButtons();

        txtEditDoc_No.Text = string.Empty;
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
                        txtDOC_NO.Text = dt.Rows[0]["DOC_NO"].ToString();
                        txtPURCNO.Text = dt.Rows[0]["PURCNO"].ToString();
                        lblDONo.Text = dt.Rows[0]["DO_No"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["DOC_DATE"].ToString();
                        txtAC_CODE.Text = dt.Rows[0]["AC_CODE"].ToString();
                        LblPartyname.Text = dt.Rows[0]["PartyName"].ToString();
                        txtUnit_Code.Text = dt.Rows[0]["Unit_Code"].ToString();
                        lblUnitName.Text = dt.Rows[0]["Unit_Name"].ToString();
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        LBLMILLNAME.Text = dt.Rows[0]["millname"].ToString();
                        txtFROM_STATION.Text = dt.Rows[0]["FROM_STATION"].ToString();
                        txtTO_STATION.Text = dt.Rows[0]["TO_STATION"].ToString();
                        txtLORRYNO.Text = dt.Rows[0]["LORRYNO"].ToString();
                        txtBROKER.Text = dt.Rows[0]["BROKER"].ToString();
                        LBLBROKERNAME.Text = dt.Rows[0]["brokername"].ToString();
                        txtWEARHOUSE.Text = dt.Rows[0]["WEARHOUSE"].ToString();
                        txtSUBTOTAL.Text = dt.Rows[0]["SUBTOTAL"].ToString();
                        txtLESS_FRT_RATE.Text = dt.Rows[0]["LESS_FRT_RATE"].ToString();
                        txtFREIGHT.Text = dt.Rows[0]["FREIGHT"].ToString();
                        txtCASH_ADVANCE.Text = dt.Rows[0]["CASH_ADVANCE"].ToString();
                        txtBANK_COMMISSION.Text = dt.Rows[0]["BANK_COMMISSION"].ToString();
                        txtOTHER_AMT.Text = dt.Rows[0]["OTHER_AMT"].ToString();
                        txtBILL_AMOUNT.Text = dt.Rows[0]["BILL_AMOUNT"].ToString();
                        txtDUE_DAYS.Text = dt.Rows[0]["DUE_DAYS"].ToString();
                        txtNETQNTL.Text = dt.Rows[0]["NETQNTL"].ToString();
                        txtTransportCode.Text = dt.Rows[0]["Transport_Code"].ToString();
                        lblTransportName.Text = dt.Rows[0]["Transport_Name"].ToString();
                        txtLCST.Text = dt.Rows[0]["LCST"].ToString();
                        txtRCST.Text = dt.Rows[0]["RCST"].ToString();
                        txtLSGST.Text = dt.Rows[0]["LSGST"].ToString();
                        txtRSGST.Text = dt.Rows[0]["RSGST"].ToString();
                        txtLIGST.Text = dt.Rows[0]["LIGST"].ToString();
                        txtIGST.Text = dt.Rows[0]["RIGST"].ToString();
                        txtGSTRateCode.Text = dt.Rows[0]["GST_Code"].ToString();
                        lblGSTRateName.Text = dt.Rows[0]["GST_Name"].ToString();
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
                        recordExist = true;
                        lblMsg.Text = "";

                        #region  Details
                        qry = "select detail_id as ID,item_code,narration,Quantal,packing,bags,rate,item_Amount from " + qryCommon + " where doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
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
                    else
                    {
                        grdDetail.DataSource = null;
                        grdDetail.DataBind();
                        ViewState["currentTable"] = null;
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

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Tran_Type='" + trntype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                txtDOC_NO.Text = hdnf.Value;
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
        txtPACKING.Text = "50";
        txtITEM_CODE.Text = "";
        txtQUANTAL.Text = "";
        txtBAGS.Text = "";
        txtRATE.Text = "";
        txtITEMAMOUNT.Text = "";
        txtITEM_NARRATION.Text = "";
        LBLITEMNAME.Text = "";
        setFocusControl(txtITEM_CODE);
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
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
                        // update row
                        int n = Convert.ToInt32(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;
                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select detail_id from " + tblDetails + " where doc_no=" + txtDOC_NO.Text + "  And Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (id != string.Empty)
                        {
                            dr["rowAction"] = "U";   //actual row
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }
                        #endregion
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add((new DataColumn("ID", typeof(Int32))));
                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("Quantal", typeof(double))));
                    dt.Columns.Add((new DataColumn("packing", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("bags", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("rate", typeof(double))));
                    dt.Columns.Add((new DataColumn("item_Amount", typeof(double))));
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
                dt.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("narration", typeof(string))));
                dt.Columns.Add((new DataColumn("Quantal", typeof(double))));
                dt.Columns.Add((new DataColumn("packing", typeof(Int32))));
                dt.Columns.Add((new DataColumn("bags", typeof(Int32))));
                dt.Columns.Add((new DataColumn("rate", typeof(double))));
                dt.Columns.Add((new DataColumn("item_Amount", typeof(double))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            dr["item_code"] = txtITEM_CODE.Text;

            dr["narration"] = Server.HtmlDecode(txtITEM_NARRATION.Text);
            if (txtQUANTAL.Text != string.Empty)
            {
                dr["Quantal"] = txtQUANTAL.Text;
            }
            else
            {
                setFocusControl(txtQUANTAL);
            }
            if (txtPACKING.Text != string.Empty)
            {
                dr["packing"] = txtPACKING.Text;
            }
            else
            {
                setFocusControl(txtPACKING);
            }

            dr["bags"] = txtBAGS.Text;
            if (txtRATE.Text != string.Empty)
            {
                dr["rate"] = txtRATE.Text;
            }
            else
            {
                setFocusControl(txtRATE);
            }
            if (txtITEMAMOUNT.Text != string.Empty)
            {
                dr["item_Amount"] = txtITEMAMOUNT.Text;
            }
            else
            {
                setFocusControl(txtITEMAMOUNT);
            }
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
                setFocusControl(txtITEM_CODE);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            txtITEM_CODE.Text = "";
            txtITEM_NARRATION.Text = "";
            txtQUANTAL.Text = "";
            txtRATE.Text = "";
            txtITEMAMOUNT.Text = "";
            txtPACKING.Text = "50";
            txtBAGS.Text = "";
            csCalculations();
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
        setFocusControl(txtLCST);
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gvrow)
    {
        lblNo.Text = Server.HtmlDecode(gvrow.Cells[11].Text);
        lblID.Text = Server.HtmlDecode(gvrow.Cells[2].Text);
        txtITEM_CODE.Text = Server.HtmlDecode(gvrow.Cells[3].Text);
        LBLITEMNAME.Text = clsCommon.getString("Select System_Name_E from " + tblPrefix + "SystemMaster where System_Code=" + Server.HtmlDecode(gvrow.Cells[3].Text.ToString()) + " and System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        txtITEM_NARRATION.Text = Server.HtmlDecode(gvrow.Cells[4].Text);
        txtQUANTAL.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
        txtPACKING.Text = Server.HtmlDecode(gvrow.Cells[6].Text);
        txtBAGS.Text = Server.HtmlDecode(gvrow.Cells[7].Text);
        txtRATE.Text = Server.HtmlDecode(gvrow.Cells[8].Text);
        txtITEMAMOUNT.Text = Server.HtmlDecode(gvrow.Cells[9].Text);
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
                string IDExisting = clsCommon.getString("select detail_id from " + tblDetails + " where doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[10].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[10].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[10].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[10].Text = "A";
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (hdnfClosePopup.Value == "txtPURCNO")
        {
            this.grdPopup.Columns[0].Visible = true;
        }
        else
        {
            this.grdPopup.Columns[0].Visible = false;
        }

        //if (v == "txtGSTRateCode")
        //{
        //    e.Row.Cells[0].Width = new Unit("50px");
        //    e.Row.Cells[1].Width = new Unit("100px");
        //    e.Row.Cells[2].Width = new Unit("300px");

        //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        //    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        //}
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
                        if (grdDetail.Rows[rowindex].Cells[10].Text != "D" && grdDetail.Rows[rowindex].Cells[10].Text != "R")
                        {
                            pnlPopupDetails.Style["display"] = "block";
                            this.showDetailsRow(grdDetail.Rows[rowindex]);
                            btnAdddetails.Text = "Update";
                            setFocusControl(txtITEM_CODE);
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

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            // {
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[2].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[3].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[4].ControlStyle.Width = new Unit("250px");
            e.Row.Cells[5].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[6].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[7].ControlStyle.Width = new Unit("60px");

            int i = 1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 20)
                    {
                        cell.Text = cell.Text.Substring(0, 31) + "..";
                        cell.ToolTip = s;
                    }
                }

            }
            //}
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDOC_NO_TextChanged]
    protected void txtDOC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDOC_NO.Text;
        strTextBox = "txtDOC_NO";
        csCalculations();
    }
    #endregion

    #region [btntxtDOC_NO_Click]
    protected void btntxtDOC_NO_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDOC_NO";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPURCNO_TextChanged]
    protected void txtPURCNO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPURCNO.Text;
        strTextBox = "txtPURCNO";
        csCalculations();
    }
    #endregion

    #region [btntxtPURCNO_Click]
    protected void btntxtPURCNO_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPURCNO";
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

    #region [txtAC_CODE_TextChanged]
    protected void txtAC_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_CODE.Text;
        strTextBox = "txtAC_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtAC_CODE_Click]
    protected void btntxtAC_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAC_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtMILL_CODE_TextChanged]
    protected void txtMILL_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMILL_CODE.Text;
        strTextBox = "txtMILL_CODE";
        csCalculations();
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

    #region [txtFROM_STATION_TextChanged]
    protected void txtFROM_STATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFROM_STATION.Text;
        strTextBox = "txtFROM_STATION";
        csCalculations();
    }
    #endregion

    #region [txtTO_STATION_TextChanged]
    protected void txtTO_STATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTO_STATION.Text;
        strTextBox = "txtTO_STATION";
        csCalculations();
    }
    #endregion

    #region [txtLORRYNO_TextChanged]
    protected void txtLORRYNO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLORRYNO.Text;
        strTextBox = "txtLORRYNO";
        csCalculations();
    }
    #endregion

    #region [txtBROKER_TextChanged]
    protected void txtBROKER_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBROKER.Text;
        strTextBox = "txtBROKER";
        csCalculations();
    }
    #endregion

    #region [btntxtBROKER_Click]
    protected void btntxtBROKER_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBROKER";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtWEARHOUSE_TextChanged]
    protected void txtWEARHOUSE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtWEARHOUSE.Text;
        strTextBox = "txtWEARHOUSE";
        csCalculations();
    }
    #endregion

    #region [txtSUBTOTAL_TextChanged]
    protected void txtSUBTOTAL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSUBTOTAL.Text;
        strTextBox = "txtSUBTOTAL";
        csCalculations();
    }
    #endregion

    #region [txtLESS_FRT_RATE_TextChanged]
    protected void txtLESS_FRT_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLESS_FRT_RATE.Text;
        strTextBox = "txtLESS_FRT_RATE";
        csCalculations();
    }
    #endregion

    #region [txtFREIGHT_TextChanged]
    protected void txtFREIGHT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFREIGHT.Text;
        strTextBox = "txtFREIGHT";
        csCalculations();
    }
    #endregion

    #region [txtCASH_ADVANCE_TextChanged]
    protected void txtCASH_ADVANCE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCASH_ADVANCE.Text;
        strTextBox = "txtCASH_ADVANCE";
        csCalculations();
    }
    #endregion

    #region [txtBANK_COMMISSION_TextChanged]
    protected void txtBANK_COMMISSION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_COMMISSION.Text;
        strTextBox = "txtBANK_COMMISSION";
        csCalculations();
    }
    #endregion

    #region [txtOTHER_AMT_TextChanged]
    protected void txtOTHER_AMT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOTHER_AMT.Text;
        strTextBox = "txtOTHER_AMT";
        csCalculations();
    }
    #endregion

    #region [txtBILL_AMOUNT_TextChanged]
    protected void txtBILL_AMOUNT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBILL_AMOUNT.Text;
        strTextBox = "txtBILL_AMOUNT";
        csCalculations();
    }
    #endregion

    #region [txtDUE_DAYS_TextChanged]
    protected void txtDUE_DAYS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDUE_DAYS.Text;
        strTextBox = "txtDUE_DAYS";
        csCalculations();
    }
    #endregion

    #region [txtNETQNTL_TextChanged]
    protected void txtNETQNTL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNETQNTL.Text;
        strTextBox = "txtNETQNTL";
        csCalculations();
    }
    #endregion

    #region [txtITEM_CODE_TextChanged]
    protected void txtITEM_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtITEM_CODE.Text;
        strTextBox = "txtITEM_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtITEM_CODE_Click]
    protected void btntxtITEM_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtITEM_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtITEM_NARRATION_TextChanged]
    protected void txtITEM_NARRATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtITEM_NARRATION.Text;
        strTextBox = "txtITEM_NARRATION";
        csCalculations();
    }
    #endregion


    #region [txtQUANTAL_TextChanged]
    protected void txtQUANTAL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtQUANTAL.Text;
        strTextBox = "txtQUANTAL";
        csCalculations();
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


    #region [txtRATE_TextChanged]
    protected void txtRATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRATE.Text;
        strTextBox = "txtRATE";
        csCalculations();
    }
    #endregion


    #region [txtITEMAMOUNT_TextChanged]
    protected void txtITEMAMOUNT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtITEMAMOUNT.Text;
        strTextBox = "txtITEMAMOUNT";
        csCalculations();
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
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                setFocusControl(txtAC_CODE);
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (hdnfClosePopup.Value == "txtBROKER")
            {
                setFocusControl(txtBROKER);
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
        string qry = "";
        if (txtDOC_NO.Text != string.Empty)
        {
            if (ViewState["mode"] != null)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no=" + txtDOC_NO.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    if (str != string.Empty)
                    {
                        lblMsg.Text = "Code " + txtDOC_NO.Text + " already exist";

                        this.getMaxCode();
                        hdnf.Value = txtDOC_NO.Text;
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
            setFocusControl(txtDOC_NO);
            return;
        }
        if (txtAC_CODE.Text != string.Empty)
        {
            string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (iscarporate == "Y")
            {
                if (txtUnit_Code.Text != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtUnit_Code);
                    return;
                }
            }
        }
        if (txtDOC_DATE.Text != string.Empty)
        {

            if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
            {
                isValidated = true;
            }
            else
            {
                txtDOC_DATE.Text = "";
                isValidated = false;
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtDOC_DATE);
            return;
        }

        if (txtAC_CODE.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtAC_CODE);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtAC_CODE);
            return;
        }

        if (txtMILL_CODE.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='M'");
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
        int count = 0;
        if (grdDetail.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Purchase Details!');", true);
            isValidated = false;
            setFocusControl(btnOpenDetailsPopup);
            return;
        }
        if (grdDetail.Rows.Count >= 1)
        {
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[10].Text == "D")
                {
                    count++;
                }
            }
            if (grdDetail.Rows.Count == count)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Minimum One Purchase Details is compulsory!');", true);
                isValidated = false;
                setFocusControl(btnOpenDetailsPopup);
                return;
            }
        }
        double CashAdvacne = txtCASH_ADVANCE.Text != string.Empty ? Convert.ToDouble(txtCASH_ADVANCE.Text) : 0.00;
        string Transport = txtTransportCode.Text != string.Empty ? txtTransportCode.Text : "0";
        if (CashAdvacne != 0)
        {
            if (Transport == "0" || string.IsNullOrEmpty(Transport))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Transport Is Compulsory!');", true);
                isValidated = false;
                setFocusControl(txtTransportCode);
                return;
            }
        }
        #endregion

        #region -Head part declearation
        Int32 DOC_NO = txtDOC_NO.Text != string.Empty ? Convert.ToInt32(txtDOC_NO.Text) : 0;
        Int32 PURCNO = txtPURCNO.Text != string.Empty ? Convert.ToInt32(txtPURCNO.Text) : 0;
        string DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        Int32 AC_CODE = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
        Int32 Unit_Code = txtUnit_Code.Text != string.Empty ? Convert.ToInt32(txtUnit_Code.Text) : 0;
        Int32 MILL_CODE = txtMILL_CODE.Text != string.Empty ? Convert.ToInt32(txtMILL_CODE.Text) : 0;
        Int32 TRANSPORT_CODE = txtTransportCode.Text != string.Empty ? Convert.ToInt32(txtTransportCode.Text) : 0;
        string FROM_STATION = txtFROM_STATION.Text;
        string TO_STATION = txtTO_STATION.Text;
        string LORRYNO = txtLORRYNO.Text;
        Int32 BROKER = txtBROKER.Text != string.Empty ? Convert.ToInt32(txtBROKER.Text) : 2;
        string WEARHOUSE = txtWEARHOUSE.Text;
        double SUBTOTAL = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0.00;
        double LESS_FRT_RATE = txtLESS_FRT_RATE.Text != string.Empty ? Convert.ToDouble(txtLESS_FRT_RATE.Text) : 0.00;
        double FREIGHT = txtFREIGHT.Text != string.Empty ? Convert.ToDouble(txtFREIGHT.Text) : 0.00;
        double CASH_ADVANCE = txtCASH_ADVANCE.Text != string.Empty ? Convert.ToDouble(txtCASH_ADVANCE.Text) : 0.00;
        double BANK_COMMISSION = txtBANK_COMMISSION.Text != string.Empty ? Convert.ToDouble(txtBANK_COMMISSION.Text) : 0.00;
        double OTHER_AMT = txtOTHER_AMT.Text != string.Empty ? Convert.ToDouble(txtOTHER_AMT.Text) : 0.00;
        double BILL_AMOUNT = txtBILL_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtBILL_AMOUNT.Text) : 0.00;
        Int32 DUE_DAYS = txtDUE_DAYS.Text != string.Empty ? Convert.ToInt32(txtDUE_DAYS.Text) : 0;
        double NETQNTL = txtNETQNTL.Text != string.Empty ? Convert.ToDouble(txtNETQNTL.Text) : 0.00;
        double LCST = txtLCST.Text != string.Empty ? Convert.ToDouble(txtLCST.Text) : 0;
        double RCST = txtRCST.Text != string.Empty ? Convert.ToDouble(txtRCST.Text) : 0;
        double IGST = txtIGST.Text != string.Empty ? Convert.ToDouble(txtIGST.Text) : 0;
        double LIGST = txtLIGST.Text != string.Empty ? Convert.ToDouble(txtLIGST.Text) : 0;
        double LSGST = txtLSGST.Text != string.Empty ? Convert.ToDouble(txtLSGST.Text) : 0;
        double RSGST = txtRSGST.Text != string.Empty ? Convert.ToDouble(txtRSGST.Text) : 0;
        Int32 GST_Code = txtGSTRateCode.Text != string.Empty ? Convert.ToInt32(txtGSTRateCode.Text) : 0;
        string retValue = string.Empty;
        string strRev = string.Empty;
        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int year_Code = Convert.ToInt32(Session["year"].ToString());
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());

        string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");

        #endregion-End of Head part declearation
        clsGledgerupdations gleder = new clsGledgerupdations();
        #region save Head Master
        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
        {

            if (ViewState["mode"] != null)
            {
                DataSet ds = new DataSet();
                if (ViewState["mode"].ToString() == "I")
                {
                    obj.flag = 1;
                    obj.tableName = tblHead;
                    obj.columnNm = "DOC_NO,Tran_Type,PURCNO,DOC_DATE,AC_CODE,Unit_Code,MILL_CODE,FROM_STATION,TO_STATION,LORRYNO,BROKER,WEARHOUSE,SUBTOTAL,LESS_FRT_RATE,FREIGHT,CASH_ADVANCE,BANK_COMMISSION,OTHER_AMT,BILL_AMOUNT,DUE_DAYS,NETQNTL,LCST,RCST,LSGST,RSGST,LIGST,RIGST,GST_Code,Company_Code,Year_Code,Branch_Code,Created_By,Transport_Code";
                    obj.values = "'" + DOC_NO + "','" + trntype + "','" + PURCNO + "','" + DOC_DATE + "','" + AC_CODE + "','" + Unit_Code + "','" + MILL_CODE + "','" + FROM_STATION + "','" + TO_STATION + "','" + LORRYNO + "','" + BROKER + "','" + WEARHOUSE + "','" + SUBTOTAL + "','" + LESS_FRT_RATE + "','" + FREIGHT + "','" + CASH_ADVANCE + "','" + BANK_COMMISSION + "','" + OTHER_AMT + "','" + BILL_AMOUNT + "','" + DUE_DAYS + "','" + NETQNTL + "','" + LCST + "','" + RCST + "','" + LSGST + "','" + RSGST + "','" + LIGST + "','" + IGST + "','" + GST_Code + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + TRANSPORT_CODE + "'";
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }
                else
                {
                    //Update Mode
                    obj.flag = 2;
                    obj.tableName = tblHead;
                    obj.columnNm = "DOC_NO='" + DOC_NO + "',Tran_Type='" + trntype + "',PURCNO='" + PURCNO + "',DOC_DATE='" + DOC_DATE + "',AC_CODE='" + AC_CODE + "',Unit_Code='" + Unit_Code + "',MILL_CODE='" + MILL_CODE + "',FROM_STATION='" + FROM_STATION + "',TO_STATION='" + TO_STATION + "',LORRYNO='" + LORRYNO + "',BROKER='" + BROKER + "',WEARHOUSE='" + WEARHOUSE + "',SUBTOTAL='" + SUBTOTAL + "',LESS_FRT_RATE='" + LESS_FRT_RATE + "',FREIGHT='" + FREIGHT + "',CASH_ADVANCE='" + CASH_ADVANCE + "',BANK_COMMISSION='" + BANK_COMMISSION + "',OTHER_AMT='" + OTHER_AMT + "',BILL_AMOUNT='" + BILL_AMOUNT + "',DUE_DAYS='" + DUE_DAYS + "',NETQNTL='" + NETQNTL + "',LCST='" + LCST + "',RCST='" + RCST + "',LSGST='" + LSGST + "',RSGST='" + RSGST + "',LIGST='" + LIGST + "',RIGST='" + IGST + "',GST_Code='" + GST_Code + "',Modified_By='" + user + "',Transport_Code='" + TRANSPORT_CODE + "'  where doc_no=" + DOC_NO + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                    obj.values = "none";
                    ds = new DataSet();
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }

                #region --------------------  Details --------------------

                Int32 item_code = 0;
                string narration = "";
                double Quantal = 0.00;
                int packing = 0;
                int bags = 0;
                double rate = 0.00;
                double item_Amount = 0.00;
                string i_d = "";
                if (strRev == "-1" || strRev == "-2")
                {
                    if (grdDetail.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdDetail.Rows.Count; i++)
                        {
                            item_code = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                            narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[4].Text);
                            Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[5].Text);
                            packing = Convert.ToInt32(grdDetail.Rows[i].Cells[6].Text);
                            bags = Convert.ToInt32(grdDetail.Rows[i].Cells[7].Text);
                            rate = Convert.ToDouble(grdDetail.Rows[i].Cells[8].Text);
                            item_Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                            i_d = grdDetail.Rows[i].Cells[2].Text;

                            if (grdDetail.Rows[i].Cells[10].Text != "N" && grdDetail.Rows[i].Cells[10].Text != "R")
                            {
                                if (grdDetail.Rows[i].Cells[10].Text == "A")
                                {
                                    obj.flag = 1;
                                    obj.tableName = tblDetails;
                                    obj.columnNm = "doc_no,Tran_Type,item_code,narration,Quantal,packing,bags,rate,item_Amount,Company_Code,Year_Code,Branch_Code,Created_By";
                                    obj.values = "'" + DOC_NO + "','" + trntype + "','" + item_code + "','" + narration + "','" + Quantal + "','" + packing + "','" + bags + "','" + rate + "','" + item_Amount + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                                    ds = new DataSet();
                                    ds = obj.insertAccountMaster(ref strRev);
                                }
                                if (grdDetail.Rows[i].Cells[10].Text == "U")
                                {

                                    obj.flag = 2;
                                    obj.tableName = tblDetails;
                                    obj.columnNm = "Tran_Type='" + trntype + "',item_code='" + item_code + "',narration='" + narration + "',Quantal='" + Quantal + "',packing='" + packing + "',bags='" + bags + "'" +
                                        " ,rate='" + rate + "',item_Amount='" + item_Amount + "',Modified_By='" + user + "'" +
                                        " where Company_Code='" + Company_Code + "' and year_Code='" + year_Code + "'" +
                                        "  and doc_no='" + txtDOC_NO.Text + "' and detail_id=" + i_d;
                                    obj.values = "none";
                                    ds = new DataSet();
                                    ds = obj.insertAccountMaster(ref strRev);
                                }
                                if (grdDetail.Rows[i].Cells[10].Text == "D")
                                {
                                    obj.flag = 3;
                                    obj.tableName = tblDetails;
                                    obj.columnNm = " Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                                        " and Doc_No='" + txtDOC_NO.Text + "' and detail_id=" + i_d;
                                    obj.values = "none";
                                    ds = new DataSet();
                                    ds = obj.insertAccountMaster(ref strRev);
                                }

                            }
                        }
                    }
                    else
                    {
                        grdDetail.DataSource = null;
                        grdDetail.DataBind();
                    }
                }
                gleder.SugarSaleReturnGledgerEffect(trntype, DOC_NO, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                if (retValue == "-1")
                {
                    if (ViewState["lstVouchers"] != null)
                    {
                        List<int> lstVouchers = (List<int>)ViewState["lstVouchers"];
                        Int32[] arr = lstVouchers.ToArray();
                        for (int l = 0; l < arr.Length; l++)
                        {
                            DataSet dsInsert = new DataSet();
                            dsInsert = clsDAL.SimpleQuery("Insert into tblMultipleReturnPurchase (RS_No,Purc_No,Company_Code,Year_Code,Branch_Code,Created_By) Values(" + DOC_NO + "," + arr[l].ToString() + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + user + "')");
                        }
                    }
                }
                #endregion

                hdnf.Value = txtDOC_NO.Text;
                if (retValue == "-1")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);
                }
                if (retValue == "-2" || retValue == "-3")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                }
            }
        }
        #endregion

        txtEditDoc_No.Text = string.Empty;
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtPURCNO")
            {
                grdPopup.AllowPaging = false;
                btnSelectVoucers.Enabled = true;
            }
            else
            {
                grdPopup.AllowPaging = true;
                btnSelectVoucers.Enabled = false;
            }

            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            if (hdnfClosePopup.Value == "txtDOC_NO" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtdoc_no.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtDOC_NO.Text = string.Empty;
                    txtDOC_NO.Enabled = true;

                    btnSave.Enabled = false;
                    setFocusControl(txtDOC_NO);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtdoc_no.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select DOC No--";
                    string qry = "select doc_no,doc_date,PartyShortname,PartyCity from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                        " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or PartyShortname like '%" + txtSearchText.Text + "%' or PartyCity like '%" + txtSearchText.Text + "%')";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtPURCNO")
            {
                lblPopupHead.Text = "--Select Purchase No--";
                //string qry = "Select s.doc_no as PurcNo,a.Ac_Name_E as mill,b.Ac_Name_E as broker,Convert(varchar(10),s.doc_date,103) as doc_date,s.NETQNTL from " + tblPrefix + "SugarPurchaseReturn s" +
                //            " left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=s.mill_code and a.Company_Code=s.Company_Code left outer join " + tblPrefix + "AccountMaster b on b.Ac_Code=s.BROKER and b.Company_Code=s.Company_Code" +
                //            " where s.doc_no NOT IN (Select k.PURCNO from " + tblPrefix + "SugarSale k where k.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and k.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and ( PURCNO like '%" + txtSearchText.Text + "%')";

                string qry = "select doc_no as PurcNo,doc_date,MillName,PartyName,NETQNTL,Balance from " + tblPrefix + "qrySugarPurchaseReturnBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Balance!=0 and doc_no Not in(select Purc_No from tblMultipleReturnPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and (doc_no like '%" + txtSearchText.Text + "%' or MillName like '%" + txtSearchText.Text + "%' or PartyName like '%" + txtSearchText.Text + "%')";

                this.showPopup(qry);

            }
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtUnit_Code")
            {
                if (txtAC_CODE.Text != string.Empty)
                {
                    string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (iscarporate == "Y")
                    {
                        lblMsg.Text = "";
                        lblPopupHead.Text = "--Select Unit--";
                        string qry = "select Unit_name,UnitName,unitCity from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtAC_CODE.Text +
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (Unit_name like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or unitCity like '%" + txtSearchText.Text + "%') order by UnitName";
                        this.showPopup(qry);
                    }
                    else
                    {
                        lblPopupHead.Text = "--Select Unit Code--";
                        string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                        this.showPopup(qry);
                    }
                }
                else
                {
                    lblMsg.Text = "Please Enter Ac_Code First!";
                    setFocusControl(txtAC_CODE);
                }
            }
            if (hdnfClosePopup.Value == "txtTransportCode")
            {
                lblPopupHead.Text = "--Select Transport Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Type='T' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                lblPopupHead.Text = "--Select Mill--";
                //string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                //    " inner join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code where " + AccountMasterTable + ".Company_Code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='M' " +
                //    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";

                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Ac_type='M' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBROKER")
            {
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " Left Outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtITEM_CODE")
            {
                lblPopupHead.Text = "--Select Item--";
                string qry = "select System_Code,System_Name_E as Item_Name from " + SystemMasterTable + " where System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGSTRateCode")
            {
                lblPopupHead.Text = "--Select Item--";
                string qry = "select Doc_no,GST_Name,Rate from " + tblPrefix + "GSTRateMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and ( Doc_no like '%" + txtSearchText.Text + "%' or GST_Name like '%" + txtSearchText.Text + "%' or Rate like '%" + txtSearchText.Text + "%') order by GST_Name"; ;
                this.showPopup(qry);
            }


        }
        catch
        {
        }
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtDOC_NO")
            {
                #region code
                try
                {
                    int n;
                    bool isNumeric = int.TryParse(txtDOC_NO.Text, out n);

                    if (isNumeric == true)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string txtValue = "";
                        if (txtDOC_NO.Text != string.Empty)
                        {
                            txtValue = txtDOC_NO.Text;

                            string qry = "select * from " + tblHead + " where  doc_no='" + txtValue + "' " +
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

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
                                                txtDOC_NO.Enabled = false;
                                                hdnf.Value = txtDOC_NO.Text;
                                                btnSave.Enabled = true;   //IMP                                       
                                                setFocusControl(txtPURCNO);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtDOC_NO.Enabled = false;
                                                    setFocusControl(txtPURCNO);
                                                    pnlgrdDetail.Enabled = true;
                                                    hdnf.Value = txtDOC_NO.Text;

                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtPURCNO);
                                            txtDOC_NO.Enabled = false;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtDOC_NO.Text = string.Empty;
                                            setFocusControl(txtDOC_NO);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                            setFocusControl(txtDOC_NO);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Doc No is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtDOC_NO.Text = string.Empty;
                        setFocusControl(txtDOC_NO);
                    }
                }
                catch
                {

                }
                #endregion
            }
            //if (strTextBox == "txtPURCNO")
            //{
            //    setFocusControl(txtDOC_DATE);
            //}
            if (strTextBox == "txtDOC_DATE")
            {
                try
                {
                    string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtAC_CODE);
                    }
                    else
                    {
                        txtDOC_DATE.Text = "";
                        setFocusControl(txtDOC_DATE);
                    }
                }
                catch
                {
                    txtDOC_DATE.Text = "";
                    setFocusControl(txtDOC_DATE);
                }
            }
            if (strTextBox == "txtAC_CODE")
            {
                string acname = "";
                if (txtAC_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
                    if (a == false)
                    {
                        btntxtAC_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty)
                        {
                            LblPartyname.Text = acname;
                            setFocusControl(txtUnit_Code);
                            txtTO_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            txtAC_CODE.Text = string.Empty;
                            LblPartyname.Text = acname;
                            setFocusControl(txtAC_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtAC_CODE);
                }
            }
            if (strTextBox == "txtUnit_Code")
            {
                string acname = "";
                if (txtUnit_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtUnit_Code.Text);
                    if (a == false)
                    {
                        btntxtUnitcode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (iscarporate == "Y")
                        {
                            lblMsg.Text = "";
                            lblPopupHead.Text = "--Select Unit--";
                            string qry = "select UnitName from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtAC_CODE.Text +
                                " and Unit_name=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                            acname = clsCommon.getString(qry);
                            if (acname != string.Empty)
                            {
                                lblUnitName.Text = acname;
                                setFocusControl(txtMILL_CODE);
                                //txtTo_Place.Text = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtAC_CODE.Text);
                            }
                            else
                            {
                                txtUnit_Code.Text = string.Empty;
                                lblUnitName.Text = acname;
                                setFocusControl(txtUnit_Code);
                            }
                        }
                        else
                        {
                            acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtUnit_Code.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (acname != string.Empty)
                            {

                                lblUnitName.Text = acname;
                                setFocusControl(txtMILL_CODE);
                                // txtTO_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            }
                            else
                            {
                                txtUnit_Code.Text = string.Empty;
                                lblUnitName.Text = acname;
                                setFocusControl(txtUnit_Code);
                            }
                        }
                    }
                }
                else
                {
                    setFocusControl(txtUnit_Code);
                }
            }
            if (strTextBox == "txtTransportCode")
            {
                string acname = "";
                if (txtTransportCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtTransportCode.Text);
                    if (a == false)
                    {
                        btnTransport_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Type='T' and Ac_Code=" + txtTransportCode.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty)
                        {
                            lblTransportName.Text = acname;
                            setFocusControl(txtCASH_ADVANCE);
                        }
                        else
                        {
                            txtTransportCode.Text = string.Empty;
                            lblTransportName.Text = acname;
                            setFocusControl(txtTransportCode);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtTransportCode);
                }
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

                            LBLMILLNAME.Text = millName;
                            setFocusControl(txtFROM_STATION);
                            txtFROM_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            txtMILL_CODE.Text = string.Empty;
                            LBLMILLNAME.Text = millName;
                            setFocusControl(txtMILL_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtMILL_CODE);
                }
            }
            if (strTextBox == "txtPURCNO")
            {
                if (txtPURCNO.Text != string.Empty)
                {
                    qry = "select doc_no,PURCNO,Convert(varchar(10),doc_date,103) as doc_date,Ac_Code,PartyName as Party,Unit_Code,Unit_Name,mill_code,MillName,FROM_STATION,TO_STATION,LORRYNO,BROKER,brokerName as broker_name,wearhouse,subTotal,LESS_FRT_RATE,freight,cash_advance,bank_commission,OTHER_AMT" +
                            ",Bill_Amount,Due_Days,NETQNTL,Balance from " + tblPrefix + "qrySugarPurchaseReturnBalance  where doc_no=" + txtPURCNO.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string doc_no = ds.Tables[0].Rows[0]["doc_no"].ToString();
                            txtAC_CODE.Text = ds.Tables[0].Rows[0]["Ac_Code"].ToString();
                            LblPartyname.Text = ds.Tables[0].Rows[0]["Party"].ToString();
                            txtUnit_Code.Text = ds.Tables[0].Rows[0]["Unit_Code"].ToString();
                            lblUnitName.Text = ds.Tables[0].Rows[0]["Unit_Name"].ToString();
                            txtMILL_CODE.Text = ds.Tables[0].Rows[0]["mill_code"].ToString();
                            LBLMILLNAME.Text = ds.Tables[0].Rows[0]["MillName"].ToString();
                            txtFROM_STATION.Text = ds.Tables[0].Rows[0]["FROM_STATION"].ToString();
                            txtTO_STATION.Text = ds.Tables[0].Rows[0]["TO_STATION"].ToString();
                            txtDOC_DATE.Text = ds.Tables[0].Rows[0]["doc_date"].ToString();
                            txtLORRYNO.Text = ds.Tables[0].Rows[0]["LORRYNO"].ToString();
                            txtWEARHOUSE.Text = ds.Tables[0].Rows[0]["wearhouse"].ToString();
                            txtBROKER.Text = ds.Tables[0].Rows[0]["BROKER"].ToString();
                            LBLBROKERNAME.Text = ds.Tables[0].Rows[0]["broker_name"].ToString();
                            txtNETQNTL.Text = ds.Tables[0].Rows[0]["Balance"].ToString();
                            txtSUBTOTAL.Text = ds.Tables[0].Rows[0]["subTotal"].ToString();
                            txtFREIGHT.Text = ds.Tables[0].Rows[0]["freight"].ToString();
                            txtLESS_FRT_RATE.Text = ds.Tables[0].Rows[0]["LESS_FRT_RATE"].ToString();
                            txtDUE_DAYS.Text = ds.Tables[0].Rows[0]["Due_Days"].ToString();
                            txtCASH_ADVANCE.Text = ds.Tables[0].Rows[0]["cash_advance"].ToString();
                            txtBANK_COMMISSION.Text = ds.Tables[0].Rows[0]["bank_commission"].ToString();
                            txtOTHER_AMT.Text = ds.Tables[0].Rows[0]["OTHER_AMT"].ToString();
                            txtBILL_AMOUNT.Text = ds.Tables[0].Rows[0]["Bill_Amount"].ToString();

                            qry = "select detail_id as ID,item_code,narration,Quantal,packing,bags,rate,item_Amount,Balance from " + tblPrefix + "qrySugarPurchaseReturnBalance where doc_no=" + doc_no + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by detail_id";
                            ds = new DataSet();
                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                dt = new DataTable();
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    dt.Columns.Add(new DataColumn("ID", typeof(string)));
                                    dt.Columns.Add(new DataColumn("item_code", typeof(string)));
                                    dt.Columns.Add(new DataColumn("narration", typeof(string)));
                                    dt.Columns.Add(new DataColumn("Quantal", typeof(string)));
                                    dt.Columns.Add(new DataColumn("packing", typeof(string)));
                                    dt.Columns.Add(new DataColumn("bags", typeof(string)));
                                    dt.Columns.Add(new DataColumn("rate", typeof(string)));
                                    dt.Columns.Add(new DataColumn("item_Amount", typeof(string)));
                                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                    dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                                    int srno = 1;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["ID"] = ds.Tables[0].Rows[i]["ID"].ToString();
                                        dr["item_code"] = ds.Tables[0].Rows[i]["item_code"].ToString();
                                        dr["narration"] = ds.Tables[0].Rows[i]["narration"].ToString();
                                        dr["Quantal"] = ds.Tables[0].Rows[i]["Balance"].ToString();
                                        dr["packing"] = ds.Tables[0].Rows[i]["packing"].ToString();
                                        dr["bags"] = ds.Tables[0].Rows[i]["bags"].ToString();
                                        dr["rate"] = ds.Tables[0].Rows[i]["rate"].ToString();
                                        dr["item_Amount"] = ds.Tables[0].Rows[i]["item_Amount"].ToString();
                                        dr["rowAction"] = "A";
                                        dr["SrNo"] = srno++;
                                        dt.Rows.Add(dr);
                                    }
                                    if (dt.Rows.Count > 0)
                                    {
                                        ViewState["currentTable"] = dt;
                                        grdDetail.DataSource = dt;
                                        grdDetail.DataBind();
                                    }
                                    else
                                    {
                                        grdDetail.DataSource = null;
                                        grdDetail.DataBind();
                                    }
                                }
                            }
                        }
                    }
                }
            }


            if (strTextBox == "txtGSTRateCode")
            {
                string gstname = "";
                if (txtGSTRateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGSTRateCode.Text);
                    if (a == false)
                    {
                        btntxtGSTRateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (gstname != string.Empty)
                        {
                            lblGSTRateName.Text = gstname;
                            setFocusControl(txtGSTRateCode);
                        }
                        else
                        {
                            txtGSTRateCode.Text = string.Empty;
                            lblGSTRateName.Text = gstname;
                            setFocusControl(txtGSTRateCode);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBROKER);
                }
            }
            if (strTextBox == "txtFROM_STATION")
            {
                setFocusControl(txtTO_STATION);
            }
            if (strTextBox == "txtTO_STATION")
            {
                setFocusControl(txtLORRYNO);
            }
            if (strTextBox == "txtLORRYNO")
            {
                setFocusControl(txtWEARHOUSE);
            }
            if (strTextBox == "txtWEARHOUSE")
            {
                setFocusControl(txtBROKER);
            }
            if (strTextBox == "txtBROKER")
            {
                string brokername = "";
                if (txtBROKER.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBROKER.Text);
                    if (a == false)
                    {
                        btntxtBROKER_Click(this, new EventArgs());
                    }
                    else
                    {
                        brokername = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBROKER.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (brokername != string.Empty)
                        {

                            LBLBROKERNAME.Text = brokername;
                            setFocusControl(txtGSTRateCode);
                        }
                        else
                        {
                            txtBROKER.Text = string.Empty;
                            LBLBROKERNAME.Text = brokername;
                            setFocusControl(txtBROKER);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBROKER);
                }
            }
            if (strTextBox == "txtDUE_DAYS")
            {
                setFocusControl(txtLESS_FRT_RATE);
            }
            if (strTextBox == "txtCASH_ADVANCE")
            {
                setFocusControl(txtBILL_AMOUNT);
            }
            if (strTextBox == "txtBANK_COMMISSION")
            {
                setFocusControl(txtOTHER_AMT);
            }
            if (strTextBox == "txtLESS_FRT_RATE")
            {
                setFocusControl(txtBANK_COMMISSION);
            }
            if (strTextBox == "txtOTHER_AMT")
            {
                setFocusControl(txtTransportCode);
            }
            //if (strTextBox == "txtTransportCode")
            //{
            //    setFocusControl(txtCASH_ADVANCE);
            //}
            if (strTextBox == "txtITEM_CODE")
            {
                string itemname = "";
                if (txtITEM_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtITEM_CODE.Text);
                    if (a == false)
                    {
                        btntxtITEM_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        itemname = clsCommon.getString("select System_Name_E from " + SystemMasterTable + " where System_Code=" + txtITEM_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='I'");
                        if (itemname != string.Empty)
                        {
                            LBLITEMNAME.Text = itemname;
                            setFocusControl(txtQUANTAL);
                        }
                        else
                        {
                            txtITEM_CODE.Text = string.Empty;
                            LBLITEMNAME.Text = itemname;
                            setFocusControl(txtITEM_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtITEM_CODE);
                }
            }
            if (strTextBox == "txtQUANTAL")
            {
                setFocusControl(txtPACKING);
            }
            if (strTextBox == "txtPACKING")
            {
                setFocusControl(txtRATE);
            }
            if (strTextBox == "txtRATE")
            {
                setFocusControl(txtITEMAMOUNT);
            }
            if (strTextBox == "txtITEMAMOUNT")
            {
                setFocusControl(txtITEM_NARRATION);
            }
            if (strTextBox == "txtITEM_NARRATION")
            {
                setFocusControl(btnAdddetails);
            }

            if (strTextBox == "txtLCST")
            {
                this.CSTCalculation();
                setFocusControl(txtLSGST);
            }

            if (strTextBox == "txtLIGST")
            {
                this.LIGSTCalculation();
                setFocusControl(txtLESS_FRT_RATE);
            }

            if (strTextBox == "txtLSGST")
            {
                this.LSGSTCalculation();
                setFocusControl(txtLIGST);
            }

            #region calculation part
            double qtl = Convert.ToDouble("0" + txtQUANTAL.Text);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
            Int32 bags = 0;

            double lessfreight = Convert.ToDouble("0" + txtLESS_FRT_RATE.Text);
            double freight = 0.00;

            double netQntl = 0.00;
            double subtotal = 0.00;
            double cashAdv = Convert.ToDouble("0" + txtCASH_ADVANCE.Text);
            double bankComm = Convert.ToDouble("0" + txtBANK_COMMISSION.Text);
            double other = Convert.ToDouble("0" + txtOTHER_AMT.Text);
            double billAmt = 0.00;

            double item_Amount = 0.00;
            double rate = Convert.ToDouble("0" + txtRATE.Text);

            double RCST = Convert.ToDouble("0" + txtRCST.Text);
            double RIGST = Convert.ToDouble("0" + txtIGST.Text);
            double RSGST = Convert.ToDouble("0" + txtRSGST.Text);

            if (qtl != 0 && packing != 0)
            {
                bags = Convert.ToInt32((qtl / packing) * 100);
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }



            item_Amount = Math.Round((qtl * rate), 2);
            txtITEMAMOUNT.Text = item_Amount.ToString();

            #region calculate subtotal
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    double item_Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text.Trim());
                    subtotal = subtotal + item_Amt;
                }
                txtSUBTOTAL.Text = subtotal.ToString();
            }
            #endregion

            #region calculate net Quantal
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    double qntl = Convert.ToDouble(grdDetail.Rows[i].Cells[5].Text.Trim());
                    netQntl = netQntl + qntl;
                }
                txtNETQNTL.Text = netQntl.ToString();
            }
            #endregion

            freight = Math.Round((lessfreight * netQntl), 2);
            txtFREIGHT.Text = freight.ToString();



            billAmt = (subtotal + bankComm + cashAdv + other + RCST + RIGST + RSGST) - freight;
            txtBILL_AMOUNT.Text = billAmt.ToString();

            #endregion

        }
        catch
        {
        }
    }
    #endregion


    #region[CSTCalculation]
    private void CSTCalculation()
    {
        double Totalamt = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0.00;
        double CSTtax = txtLCST.Text != string.Empty ? Convert.ToDouble(txtLCST.Text) : 0.00;
        double result = 0.00;

        CSTtax = CSTtax / 100;
        result = Totalamt * CSTtax;
        txtRCST.Text = result.ToString();

    }

    #endregion

    #region[LSGSTCalculation]
    private void LSGSTCalculation()
    {
        double Totalamt = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0;
        double LSTtax = txtLSGST.Text != string.Empty ? Convert.ToDouble(txtLSGST.Text) : 0;
        double result = 0;

        LSTtax = LSTtax / 100;
        result = Totalamt * LSTtax;
        txtRSGST.Text = result.ToString();

    }

    #endregion

    #region[LIGSTCalculation]
    private void LIGSTCalculation()
    {
        double Totalamt = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0;
        double LITtax = txtLIGST.Text != string.Empty ? Convert.ToDouble(txtLIGST.Text) : 0;
        double result = 0;

        LITtax = LITtax / 100;
        result = Totalamt * LITtax;
        txtIGST.Text = result.ToString();

    }

    #endregion


    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //savechkdvls(); 
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

        //if (ViewState["pages"] != null)
        //{
        //    if (dt.Rows.Count>0)
        //    {
        //        dt = (DataTable)ViewState["pages"];    
        //    }
        //}
        //else
        //{
        //    dt = new DataTable();
        //    dt.Columns.Add(new DataColumn("pageIndex", typeof(int)));
        //    dt.Columns.Add(new DataColumn("rowIndex", typeof(string)));
        //    DataColumn[] keyColumn = new DataColumn[1];
        //    keyColumn[0] = dt.Columns["pageIndex"];
        //    dt.PrimaryKey = keyColumn;
        //}
        //int previousPageIndex = grdPopup.PageIndex;
        //int rowCount = grdPopup.Rows.Count;
        //if (dt.Rows.Contains(previousPageIndex))
        //{
        //    foreach (DataRow pageindex in dt.Rows)
        //    {
        //        if (Convert.ToInt32(pageindex["pageIndex"].ToString()) == previousPageIndex)
        //        {
        //            string[] rowIndexesArr = pageindex["rowIndex"].ToString().Split(',').ToArray();

        //            foreach (var item in rowIndexesArr)
        //            {
        //                CheckBox chkSelect = (CheckBox)grdPopup.Rows[Convert.ToInt32(item)].FindControl("chkSelect");
        //                chkSelect.Checked = true;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    if (dt.Rows.Contains(e.NewPageIndex))
        //    {
        //        foreach (DataRow pageindex in dt.Rows)
        //        {
        //            if (Convert.ToInt32(pageindex["pageIndex"].ToString()) == e.NewPageIndex)
        //            {
        //                string[] rowIndexesArr = pageindex["rowIndex"].ToString().Split(',').ToArray();

        //                foreach (var item in rowIndexesArr)
        //                {
        //                    CheckBox chkSelect = (CheckBox)grdPopup.Rows[Convert.ToInt32(item)].FindControl("chkSelect");
        //                    chkSelect.Checked = true;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        List<string> rowsList = new List<string>();
        //        for (int i = 0; i < rowCount; i++)
        //        {
        //            CheckBox chkSelect = (CheckBox)grdPopup.Rows[i].FindControl("chkSelect");

        //            if (chkSelect.Checked)
        //            {
        //                rowsList.Add(i.ToString());
        //            }
        //        }
        //        if (rowsList.Count > 0)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["pageIndex"] = previousPageIndex;
        //            dr["rowIndex"] = String.Join(",", rowsList.ToArray());
        //            dt.Rows.Add(dr);
        //        }
        //    }
        //}



        //ViewState["pages"] = dt;
        //chkdvaluesp();
    }
    protected void txtTransportCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTransportCode.Text;
        strTextBox = "txtTransportCode";
        csCalculations();
    }
    protected void btnTransport_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtTransportCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void txtUnit_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtUnit_Code.Text;
        strTextBox = "txtUnit_Code";
        csCalculations();
    }
    protected void btntxtUnitcode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUnit_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btnSelectVoucers_Click(object sender, EventArgs e)
    {
        try
        {
            List<int> lstvouchers = new List<int>();
            List<double> lstQuintal = new List<double>();
            double totalQuintal = 0.0;
            DataTable dtVouchers;
            if (grdPopup.Rows.Count > 0)
            {
                for (int i = 0; i < grdPopup.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdPopup.Rows[i].Cells[0].FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        int voucherNo = Convert.ToInt32(grdPopup.Rows[i].Cells[1].Text.ToString());
                        double qntl = Convert.ToDouble(grdPopup.Rows[i].Cells[6].Text.ToString());
                        lstvouchers.Add(voucherNo);
                        lstQuintal.Add(qntl);
                        totalQuintal += qntl;

                    }
                }

                dtVouchers = new DataTable();

                dtVouchers.Columns.Add(new DataColumn("ID", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("item_code", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("narration", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("Quantal", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("packing", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("bags", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("rate", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("item_Amount", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dtVouchers.Columns.Add(new DataColumn("SrNo", typeof(string)));

                DataRow dr = dtVouchers.NewRow();
                dr["ID"] = 1;
                dr["item_code"] = 1;
                dr["narration"] = "From Vouchers:" + String.Join(",", lstvouchers.ToArray());
                dr["Quantal"] = totalQuintal;
                dr["packing"] = 50;
                dr["bags"] = totalQuintal * 50;
                dr["rate"] = 0.0;
                dr["item_Amount"] = 0.0;
                dr["rowAction"] = "A";
                dr["SrNo"] = 1;
                dtVouchers.Rows.Add(dr);

                if (dtVouchers.Rows.Count > 0)
                {
                    ViewState["currentTable"] = dtVouchers;
                    grdDetail.DataSource = dtVouchers;
                    grdDetail.DataBind();
                    ViewState["lstVouchers"] = lstvouchers;
                    ViewState["lstQuintal"] = lstQuintal;
                    txtNETQNTL.Text = totalQuintal.ToString();
                    txtSUBTOTAL.Text = "0.0";
                    txtFREIGHT.Text = "0.0";
                    txtLESS_FRT_RATE.Text = "0.0";
                    txtDUE_DAYS.Text = "0";
                    txtCASH_ADVANCE.Text = "0.0";
                    txtBANK_COMMISSION.Text = "0.0";
                    txtOTHER_AMT.Text = "0.0";
                    txtBILL_AMOUNT.Text = "0.0";
                    pnlPopup.Style["display"] = "none";
                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                }

            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void chkdvaluesp()
    {

        ArrayList usercontent = (ArrayList)Session["chkditems"];

        if (usercontent != null && usercontent.Count > 0)
        {

            foreach (GridViewRow gvrow in grdPopup.Rows)
            {

                int index = Convert.ToInt32(gvrow.RowIndex.ToString());

                if (usercontent.Contains(index))
                {

                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");

                    myCheckBox.Checked = true;

                }

            }

        }

    }
    private void savechkdvls()
    {

        ArrayList usercontent = new ArrayList();

        int index = -1;

        foreach (GridViewRow gvrow in grdPopup.Rows)
        {
            //grdPopup.DataKeys[gvrow.RowIndex].Value.ToString();
            index = Convert.ToInt32(gvrow.RowIndex.ToString());

            bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;



            // Check in the Session

            if (Session["chkditems"] != null)

                usercontent = (ArrayList)Session["chkditems"];

            if (result)
            {
                if (!usercontent.Contains(index))
                    usercontent.Add(index);
            }
            else
                usercontent.Remove(index);

        }

        if (usercontent != null && usercontent.Count > 0)
            Session["chkditems"] = usercontent;

    }

    protected void txtLCST_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLCST.Text;
        strTextBox = "txtLCST";
        csCalculations();

    }


    protected void txtLSGST_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLSGST.Text;
        strTextBox = "txtLSGST";
        csCalculations();

    }
    protected void txtLIGST_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLIGST.Text;
        strTextBox = "txtLIGST";
        csCalculations();

    }
    protected void txtGSTRateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGSTRateCode.Text;
        strTextBox = "txtGSTRateCode";
        csCalculations();

    }

    protected void btntxtGSTRateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGSTRateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
}
