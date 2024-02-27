using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class pgeCommissionvoucher : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string cityMasterTable = string.Empty;
    string qryCommon = string.Empty;
    int defaultAccountCode = 0;
    string searchString = string.Empty;
    static WebControl objAsp = null;
    string user = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string qryAccountList = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "Voucher";
            tblDetails = tblPrefix + "VoucherDetails";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            qryCommon = tblPrefix + "qryVoucherList";
            pnlPopup.Style["display"] = "none";
            qryAccountList = tblPrefix + "qryAccountsList";
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

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;

            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "Doc_No";

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
                                    txtDoc_no.Text = ds.Tables[0].Rows[0][0].ToString();
                                    //setFocusControl(txtDOC_DATE);
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
                btnSave.Text = "Save";
                btnParty.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtGetpass.Enabled = false;
                btnDO.Enabled = false;
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                lblMsg.Text = string.Empty;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
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
                btntxtDOC_NO.Text = "Change No";
                btnParty.Enabled = true;
                btntxtGetpass.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtGRADE.Enabled = true;
                btnDO.Enabled = true;
                btntxtDOC_NO.Enabled = true;
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                #region set Business logic for save

                btnOpenDetailsPopup.Enabled = true;
                txtDoc_no.Enabled = false;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                ViewState["currentTable"] = null;
                lblMill_name.Text = string.Empty;
                lblPartyName.Text = string.Empty;
                setFocusControl(txtDO);
                pnlgrdDetail.Enabled = true;
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
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                btnParty.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGRADE.Enabled = false;
                btnDO.Enabled = false;
                btntxtGetpass.Enabled = false;
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
                #region Logic
                btntxtDOC_NO.Text = "Choose No";
                btnParty.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtGetpass.Enabled = true;
                btntxtGRADE.Enabled = true;
                btnDO.Enabled = true;
                btntxtDOC_NO.Enabled = true;
                lblMsg.Text = string.Empty;
                txtDoc_no.Enabled = false;
                txtSuffix.Enabled = false;
                btnOpenDetailsPopup.Enabled = true;
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
            string qry = string.Empty;
            qry = "select max(doc_no) as doc_no from " + tblHead +
                 " where Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                 " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                        hdnf.Value = dt.Rows[0][0].ToString();
                        qry = "select Suffix from " + tblHead + " where doc_no=" + hdnf.Value + " and Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                            " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        hdnfSuffix.Value = clsCommon.getString(qry);

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

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where  Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                       " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

        string cnt = clsCommon.getString(query);
        if (cnt != string.Empty)
        {
            RecordCount = Convert.ToInt32(cnt);
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
        if (txtDoc_no.Text != string.Empty)
        {
            #region check for next or previous record exist or not
            if (hdnf.Value != string.Empty)
            {

                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>'" + Convert.ToInt32(hdnf.Value) + "' ORDER BY doc_no asc  ";
                string strDoc_No = clsCommon.getString(query);
                if (strDoc_No != string.Empty)
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<'" + Convert.ToInt32(hdnf.Value) + "'  ORDER BY doc_no asc  ";
                strDoc_No = clsCommon.getString(query);
                if (strDoc_No != string.Empty)
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
            #endregion
        }
        #endregion
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        #region [code]
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + ")  " +
                " and  Tran_Type='CV' and Suffix=''  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
        #endregion
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDoc_no.Text != string.Empty)
            {
                string query = "";

                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                    " and  Tran_Type='CV' and Suffix=''  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            if (txtDoc_no.Text != string.Empty)
            {
                string query = "";
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    " and  Tran_Type='CV' and Suffix=''  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + ")  " +
                " and  Tran_Type='CV' and Suffix=''  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [navigateRecord]
    private void navigateRecord()
    {
        if (hdnf.Value != string.Empty)
        {

            ViewState["mode"] = "U";
            txtDoc_no.Text = hdnf.Value;
            hdnfSuffix.Value = "";
            string query = getDisplayQuery(); ;
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
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.getMaxCode();
        txtPACKING.Text = "50";
        pnlPopupDetails.Style["display"] = "none";
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

    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int DONumber = txtDO.Text != string.Empty ? Convert.ToInt32(txtDO.Text) : 0;
            string currentDoc_No = txtDoc_no.Text;
            string currentSuffix = txtSuffix.Text;
            DataSet ds = new DataSet();
            string strrev = "";
            using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
            {
                obj.flag = 3;
                obj.tableName = tblHead;
                obj.columnNm = "  Tran_Type='CV' and Doc_No=" + currentDoc_No + " and Suffix='" + currentSuffix.Trim() + "'" +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                obj.values = "none";
                ds = obj.insertAccountMaster(ref strrev);

                //updating delivery order table 
                string str = "2";
                obj.flag = 2;
                obj.tableName = tblPrefix + "deliveryorder";
                obj.columnNm = "voucher_no='" + 0 + "',voucher_type='" + string.Empty + "' where DOC_NO='" + DONumber + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                               " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.values = "none";
                ds = obj.insertAccountMaster(ref str);

                if (strrev == "-3")
                {
                    obj.flag = 3;
                    obj.tableName = tblDetails;
                    obj.columnNm = "  Tran_Type='CV' and Doc_No=" + currentDoc_No + " and Suffix='" + currentSuffix.Trim() + "'" +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);
                }
            }
            string query = "";

            if (strrev == "-3")
            {
                query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                       " and Tran_Type='CV' and Suffix='' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                        " " +
                        " ORDER BY Doc_No asc  ";


                hdnf.Value = clsCommon.getString(query);

                if (hdnf.Value == string.Empty)
                {
                    query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                         " and Tran_Type='CV' and Suffix='' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                        " " +
                        " ORDER BY Doc_No desc  ";
                    hdnf.Value = clsCommon.getString(query);
                }

                if (hdnf.Value != string.Empty)
                {
                    query = "select Suffix from " + tblHead + " where doc_no=" + hdnf.Value +
                       " and Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                       " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    hdnfSuffix.Value = clsCommon.getString(query);

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
        string str = clsCommon.getString("select count(doc_no) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='CV'");

        if (str != "0")
        {
            clsButtonNavigation.enableDisable("S");
            this.enableDisableNavigateButtons();
            this.makeEmptyForm("S");
            //this.fetchRecord();
        }
        else
        {
            clsButtonNavigation.enableDisable("N");
            this.enableDisableNavigateButtons();
            this.makeEmptyForm("N");

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
        txtDoc_no.Text = "";
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
                        txtDoc_no.Text = dt.Rows[0]["DOC_NO"].ToString();
                        txtSuffix.Text = dt.Rows[0]["SUFFIX"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["DOC_DATE"].ToString();
                        txtParty.Text = dt.Rows[0]["AC_CODE"].ToString();
                        lblPartyName.Text = dt.Rows[0]["PartyName"].ToString();
                        txtGetpass.Text = dt.Rows[0]["Unit_Code"].ToString();
                        lblGetpass.Text = dt.Rows[0]["Unit_Name"].ToString();
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        lblMill_name.Text = dt.Rows[0]["MillName"].ToString();
                        txtDO.Text = dt.Rows[0]["DO_No"].ToString();
                        txtDueDays.Text = dt.Rows[0]["Due_Days"].ToString();
                        txtFrom.Text = dt.Rows[0]["From_Place"].ToString();
                        txtTo.Text = dt.Rows[0]["To_Place"].ToString();
                        txtLorryNo.Text = dt.Rows[0]["Lorry_No"].ToString();
                        txtQNTL.Text = dt.Rows[0]["Quantal"].ToString();
                        txtGRADE.Text = dt.Rows[0]["GRADE"].ToString();
                        txtMILL_RATE.Text = dt.Rows[0]["MILL_RATE"].ToString();
                        txtMILL_AMOUNT.Text = dt.Rows[0]["MILL_AMOUNT"].ToString();
                        txtCOMMISSION_RATE.Text = dt.Rows[0]["COMMISSION_RATE"].ToString();
                        txtCOMMISSION.Text = dt.Rows[0]["Commission_Amount"].ToString();
                        txtLOADING_CHARGE.Text = dt.Rows[0]["LOADING_CHARGE"].ToString();
                        txtSALE_RATE.Text = dt.Rows[0]["SALE_RATE"].ToString();
                        txtPURCHASE_RATE.Text = dt.Rows[0]["PURCHASE_RATE"].ToString();
                        txtDIFF_AMOUNT.Text = dt.Rows[0]["DIFF_AMOUNT"].ToString();
                        txtVOUCHER_AMOUNT.Text = dt.Rows[0]["VOUCHER_AMOUNT"].ToString();
                        txtNARRATION1.Text = dt.Rows[0]["NARRATION1"].ToString();
                        txtNARRATION2.Text = dt.Rows[0]["NARRATION2"].ToString();
                        txtNARRATION3.Text = dt.Rows[0]["NARRATION3"].ToString();
                        txtNARRATION4.Text = dt.Rows[0]["NARRATION4"].ToString();
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
                        txtRAWANGI_DATE.Text = dt.Rows[0]["RAWANGI_DATE"].ToString();
                        recordExist = true;
                        lblMsg.Text = "";

                        #region Voucher Details
                        qry = "select ID,Bank_Code,BankName,Narration,Amount,Convert(varchar(10),re_date,103) as re_date from " + qryCommon +
                              " where Tran_Type='CV' and Suffix='" + hdnfSuffix.Value.Trim() + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                              " and Doc_No=" + hdnf.Value;
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
                                        ViewState["currentTable"] = dt;
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
                        GridDiffAmount();
                        pnlgrdDetail.Enabled = false;
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

    #region [btnOpenDetailsPopup_Click]
    protected void btnOpenDetailsPopup_Click(object sender, EventArgs e)
    {
        btnAdddetails.Text = "ADD";
        pnlPopupDetails.Style["display"] = "block";
        setFocusControl(txtBANK_CODE);
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtReConsilationDt.Text != string.Empty)
            {
                if (clsCommon.isValidDate(txtReConsilationDt.Text))
                {

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:alert('Please Fill Valid Date!');", true);
                    setFocusControl(txtReConsilationDt);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "as", "javascript:alert('InValid Date!');", true);
                setFocusControl(txtReConsilationDt);
                return;
            }


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
                            rowIndex = maxIndex;          //0
                        }
                        #endregion

                        // rowIndex = dt.Rows.Count + 1;
                        dr["ID"] = rowIndex;
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        //update row
                        int n = Convert.ToInt32(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;

                        #region decide whether actual row is updating or virtual [rowAction]

                        string id = clsCommon.getString("select ID from " + tblDetails +
                        " where Doc_No='" + txtDoc_no.Text + "' and ID='" + lblID.Text +
                        "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                        " and Suffix='" + txtSuffix.Text + "'" +
                        " and Tran_Type='CV'" +
                        " and Order_Code=" + n +
                        " and Branch_Code=" + Session["Branch_Code"].ToString());
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
                    dt.Columns.Add((new DataColumn("ID", typeof(int))));

                    dt.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("BankName", typeof(string))));
                    dt.Columns.Add((new DataColumn("Narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("Re_Date", typeof(string))));
                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                    dr = dt.NewRow();
                    dr["ID"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
                //////////
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add((new DataColumn("ID", typeof(int))));

                dt.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("BankName", typeof(string))));
                dt.Columns.Add((new DataColumn("Narration", typeof(string))));
                dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("Re_Date", typeof(string))));
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;

            }
            if (txtBANK_CODE.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Ac_Code=" + txtBANK_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    dr["Bank_Code"] = txtBANK_CODE.Text;
                    dr["BankName"] = lblBank_name.Text;
                }
                else
                {
                    txtBANK_CODE.Text = "";
                    lblBank_name.Text = "";
                    setFocusControl(txtBANK_CODE);
                    return;
                }
            }
            else
            {
                setFocusControl(txtBANK_CODE);
                return;
            }
            dr["Narration"] = txtNARRATION.Text.Trim();
            if (txtBANK_AMOUNT.Text != string.Empty)
            {
                dr["Amount"] = txtBANK_AMOUNT.Text;
            }
            else
            {
                setFocusControl(txtBANK_AMOUNT);
                return;
            }
            dr["Re_Date"] = txtReConsilationDt.Text;

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
            txtBANK_CODE.Text = string.Empty;
            lblBank_name.Text = string.Empty;
            txtBANK_AMOUNT.Text = string.Empty;
            txtNARRATION.Text = string.Empty;
            txtReConsilationDt.Text = string.Empty;
            btnAdddetails.Text = "ADD";
            this.calVoucherAmount();
            #region calculate grid diff
            GridDiffAmount();
            setFocusControl(txtBANK_CODE);
            #endregion
        }
        catch
        {
        }
    }

    private void GridDiffAmount()
    {
        double GriddiffAmt = 0.0;
        double GridAmtTotal = 0.0;
        double millAmt = Convert.ToDouble("0" + txtMILL_AMOUNT.Text);
        for (int i = 0; i < grdDetail.Rows.Count; i++)
        {
            GridAmtTotal = GridAmtTotal + Convert.ToDouble("0" + grdDetail.Rows[i].Cells[6].Text);
        }
        GriddiffAmt = millAmt - GridAmtTotal;
        lblGridDiff.Text = GriddiffAmt.ToString();
    }
    #endregion

    #region [btnClosedetails_Click]
    protected void btnClosedetails_Click(object sender, EventArgs e)
    {
        pnlPopupDetails.Style["display"] = "none";
        setFocusControl(btnSave);
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(6);
            e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(30);
            e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[5].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[8].Style["overflow"] = "hidden";

            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[5].Text = e.Row.Cells[5].Text.Replace("&nbsp;", "");
            // e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(5);
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
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            if (v == "txtParty" || v == "txtMILL_CODE" || v == "txtBANK_CODE")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[1].Width = new Unit("250px");
                //e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                //e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(60);
                //e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(30);

                //e.Row.Cells[0].Style["overflow"] = "hidden";
                //e.Row.Cells[1].Style["overflow"] = "hidden";
                //e.Row.Cells[2].Style["overflow"] = "hidden";
            }
            if (v == "txtDOC_NO")
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(50);
            }
            if (v == "txtDO")
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
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

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gridViewRow)
    {
        try
        {
            lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text);
            lblNo.Text = Server.HtmlDecode(gridViewRow.Cells[9].Text);
            txtBANK_CODE.Text = Server.HtmlDecode(gridViewRow.Cells[3].Text);
            lblBank_name.Text = Server.HtmlDecode(gridViewRow.Cells[4].Text);
            txtNARRATION.Text = Server.HtmlDecode(gridViewRow.Cells[5].Text);
            txtBANK_AMOUNT.Text = Server.HtmlDecode(gridViewRow.Cells[6].Text);
            txtReConsilationDt.Text = Server.HtmlDecode(gridViewRow.Cells[7].Text);
            setFocusControl(txtBANK_CODE);
        }
        catch
        {

        }
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

                string IDExisting = clsCommon.getString("select ID from " + tblDetails +
                    " where Tran_Type='CV' and Doc_No=" + txtDoc_no.Text + " and " +
                    " Suffix='" + txtSuffix.Text.Trim() + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and " +
                    " Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " and ID=" + ID);

                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[8].Text = "D";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[8].Text = "N";

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

                        grdDetail.Rows[rowIndex].Cells[8].Text = "R";       //R=Only remove fro grid        

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[8].Text = "A";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }

                    // dt.Rows[rowIndex]["rowAction"] = "N";   //Do nothing
                }
                ViewState["currentTable"] = dt;
                ///   this.calculateBalanceSelf();
                //ViewState["currentTable"] = dt;
            }

        }
        catch
        {

        }
    }
    #endregion

    #region [txtDoc_no_TextChanged]
    protected void txtDoc_no_TextChanged(object sender, EventArgs e)
    {
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtDoc_no.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtDoc_no.Text != string.Empty)
                {
                    txtValue = txtDoc_no.Text;

                    string qry = "select * from " + tblHead + " where Tran_Type='CV' and  Doc_No='" + txtValue + "' " +
                        " and Suffix='" + txtSuffix.Text.Trim() + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and " +
                        " Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                hdnf.Value = dt.Rows[0]["Doc_No"].ToString();
                                hdnfSuffix.Value = dt.Rows[0]["Suffix"].ToString();
                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        // this.getMaxCode();
                                        //txtDoc_no.Enabled = false;

                                        btnSave.Enabled = true;   //IMP
                                        txtSuffix.Text = string.Empty;
                                        setFocusControl(txtSuffix);
                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        qry = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Suffix='" + hdnfSuffix.Value + "'" +
                                           " and Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                                          " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                                        bool recordExist = this.fetchRecord(qry);
                                        if (recordExist == true)
                                        {
                                            txtDoc_no.Enabled = false;
                                            pnlgrdDetail.Enabled = true;
                                            setFocusControl(txtDO);


                                            hdnf.Value = txtDoc_no.Text;
                                            hdnfSuffix.Value = txtSuffix.Text.Trim();
                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";
                                    setFocusControl(txtDO);
                                    txtDoc_no.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("E");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtDoc_no.Text = string.Empty;
                                    setFocusControl(txtDoc_no);
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    txtDoc_no.Focus();
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Doc No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtDoc_no.Text = string.Empty;
                txtDoc_no.Focus();
            }
        }
        catch
        {

        }
        #endregion
    }
    #endregion

    #region [txtSuffix_TextChanged]
    protected void txtSuffix_TextChanged(object sender, EventArgs e)
    {
        txtDoc_no_TextChanged(sender, e);
    }
    #endregion

    #region [btntxtDOC_NO_Click]
    protected void btntxtDOC_NO_Click(object sender, EventArgs e)
    {
        try
        {
            if (btntxtDOC_NO.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtDoc_no.Text = string.Empty;
                txtDoc_no.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtDoc_no);
            }

            if (btntxtDOC_NO.Text == "Choose No")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtDOC_NO";
                btnSearch_Click(sender, e);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDOC_DATE_TextChanged]
    protected void txtDOC_DATE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDOC_DATE.Text != string.Empty)
            {

                string docDt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                if (clsCommon.isValidDate(docDt) == true)
                {
                    setFocusControl(txtParty);
                }
                else
                {
                    txtDOC_DATE.Text = string.Empty;
                    setFocusControl(txtDOC_DATE);
                }
            }
            else
            {
                setFocusControl(txtDOC_DATE);
            }
        }
        catch
        {
            setFocusControl(txtDOC_DATE);
        }
    }
    #endregion

    #region [txtParty_TextChanged]
    protected void txtParty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string partyName = string.Empty;
            if (txtParty.Text != string.Empty)
            {
                searchString = txtParty.Text;
                bool a = clsCommon.isStringIsNumeric(txtParty.Text);
                if (a == false)
                {
                    btnParty_Click(this, new EventArgs());
                }
                else
                {
                    string qry = "";
                    qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtParty.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    partyName = clsCommon.getString(qry);

                    if (partyName != string.Empty)
                    {
                        lblPartyName.Text = partyName;
                        txtTo.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtParty.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        setFocusControl(txtGetpass);
                    }
                    else
                    {
                        lblPartyName.Text = string.Empty;
                        txtParty.Text = string.Empty;
                        setFocusControl(txtParty);
                    }
                }
            }
            else
            {
                setFocusControl(txtParty);
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [btnParty_Click]
    protected void btnParty_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtParty";
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
        try
        {
            string millName = string.Empty;
            if (txtMILL_CODE.Text != string.Empty)
            {
                searchString = txtMILL_CODE.Text;
                bool a = clsCommon.isStringIsNumeric(txtMILL_CODE.Text);
                if (a == false)
                {
                    btntxtMILL_CODE_Click(this, new EventArgs());
                }
                else
                {
                    string qry = "";
                    qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    millName = clsCommon.getString(qry);

                    if (millName != string.Empty)
                    {
                        lblMill_name.Text = millName;
                        txtFrom.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        setFocusControl(txtFrom);
                        //pnlPopup.Style["display"] = "none";
                    }
                    else
                    {
                        lblMill_name.Text = string.Empty;
                        txtMILL_CODE.Text = string.Empty;
                        setFocusControl(txtMILL_CODE);
                    }
                }
            }
            else
            {
                setFocusControl(txtMILL_CODE);
            }
        }
        catch
        {

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

    #region [txtGetpass]
    protected void txtGetpass_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string getpass = string.Empty;
            if (txtGetpass.Text != string.Empty)
            {
                searchString = txtGetpass.Text;

                bool a = clsCommon.isStringIsNumeric(txtGetpass.Text);
                if (a == false)
                {
                    btntxtGetpass_Click(this, new EventArgs());
                }
                else
                {
                    string qry = "";
                    qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtGetpass.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    getpass = clsCommon.getString(qry);

                    if (getpass != string.Empty)
                    {
                        lblGetpass.Text = getpass;
                        setFocusControl(txtMILL_CODE);
                        //txtFrom.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtGetpass.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    }
                    else
                    {
                        lblGetpass.Text = string.Empty;
                        txtGetpass.Text = string.Empty;
                        setFocusControl(txtGetpass);
                    }
                }
            }
            else
            {
                setFocusControl(txtGetpass);
            }
        }
        catch
        {

        }
    }
    #endregion
    #region [btntxtGetpass_Click]
    protected void btntxtGetpass_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGetpass";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtQNTL_TextChanged]
    protected void txtQNTL_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtQNTL.Text != string.Empty)
            {
                this.BagsCalculation();
                this.calMillAmount();
                this.calDiffAmount();
                this.calVoucherAmount();
                setFocusControl(txtPACKING);
            }
        }
        catch
        {

        }
    }

    private void BagsCalculation()
    {
        double qntl = Convert.ToDouble("0" + txtQNTL.Text);
        Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
        Int32 bags = 0;
        bags = Convert.ToInt32((qntl / packing) * 100);
        txtBAGS.Text = bags.ToString();
    }
    #endregion

    #region [txtGRADE_TextChanged]
    protected void txtGRADE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGRADE.Text;
        setFocusControl(txtMILL_RATE);
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

    #region [txtMILL_RATE_TextChanged]
    protected void txtMILL_RATE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMILL_RATE.Text != string.Empty && txtQNTL.Text != string.Empty)
            {
                this.calMillAmount();
                this.calDiffAmount();
                this.calVoucherAmount();
                setFocusControl(txtSALE_RATE);
            }
            else
            {
                setFocusControl(txtMILL_RATE);
            }
        }
        catch
        {

        }
    }
    #endregion

    private void calMillAmount()
    {
        try
        {
            double millAmt = 0.0;
            double qtl = 0.0;
            double millRate = 0.0;
            if (txtQNTL.Text != string.Empty)
                qtl = Convert.ToDouble(txtQNTL.Text);
            if (txtMILL_RATE.Text != string.Empty)
                millRate = Convert.ToDouble(txtMILL_RATE.Text);
            millAmt = Convert.ToDouble(qtl * millRate);
            txtMILL_AMOUNT.Text = millAmt.ToString();
        }
        catch
        {

        }
    }


    #region [txtPACKING_TextChanged]
    protected void txtPACKING_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPACKING.Text;
        //strTextBox = "txtPACKING";
        this.BagsCalculation();
        setFocusControl(txtBAGS);
        //AmountCalculation();
    }
    #endregion

    #region [txtBAGS_TextChanged]
    protected void txtBAGS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBAGS.Text;
        //strTextBox = "txtBAGS";
        csCalculations();
    }
    #endregion

    #region [txtMILL_AMOUNT_TextChanged]
    protected void txtMILL_AMOUNT_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region [txtCOMMISSION_RATE_TextChanged]
    protected void txtCOMMISSION_RATE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtCOMMISSION_RATE.Text != string.Empty && txtQNTL.Text != string.Empty)
            {
                this.calCommissionAmt();
                this.calVoucherAmount();
                setFocusControl(txtCOMMISSION);
            }
            else
            {
                setFocusControl(txtCOMMISSION_RATE);
            }
        }
        catch
        {

        }
    }


    #endregion


    #region [txtCOMMISSION_TextChanged]
    protected void txtCOMMISSION_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    private void calVoucherAmount()
    {
        try
        {
            double voucherAmt = 0.0;
            double loadingCharge = Convert.ToDouble("0" + txtLOADING_CHARGE.Text);
            double diffAmt = Convert.ToDouble("0" + txtDIFF_AMOUNT.Text);
            double commissionAmt = Convert.ToDouble("0" + txtCOMMISSION.Text);
            double millAmount = Convert.ToDouble("0" + txtMILL_AMOUNT.Text);

            if (grdDetail.Rows.Count > 0)
            {
                voucherAmt = commissionAmt + loadingCharge + diffAmt + millAmount;
            }
            else
            {
                voucherAmt = commissionAmt + loadingCharge + diffAmt;
            }

            txtVOUCHER_AMOUNT.Text = voucherAmt.ToString();
        }
        catch
        {
            setFocusControl(txtDIFF_AMOUNT);
        }
    }

    private void calCommissionAmt()
    {
        try
        {
            double commAmt = 0.0;
            double quantal = Convert.ToDouble("0" + txtQNTL.Text);
            double commissionRate = Convert.ToDouble("0" + txtCOMMISSION_RATE.Text);

            commAmt = commissionRate * quantal;
            txtCOMMISSION.Text = commAmt.ToString();
        }
        catch
        {
            setFocusControl(txtQNTL);
        }
    }

    #region [txtLOADING_CHARGE_TextChanged]
    protected void txtLOADING_CHARGE_TextChanged(object sender, EventArgs e)
    {
        this.calVoucherAmount();
        setFocusControl(txtDIFF_AMOUNT);

    }
    #endregion

    #region [txtSALE_RATE_TextChanged]
    protected void txtSALE_RATE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtPURCHASE_RATE.Text = string.Empty;
            this.calDiffAmount();
            this.calVoucherAmount();
            setFocusControl(txtPURCHASE_RATE);

        }
        catch
        {

        }
    }
    #endregion


    private void calDiffAmount()
    {
        try
        {
            double diffAmt = 0.0;

            double millRate = Convert.ToDouble("0" + txtMILL_RATE.Text);
            double saleRate = Convert.ToDouble("0" + txtSALE_RATE.Text);
            double purcRate = Convert.ToDouble("0" + txtPURCHASE_RATE.Text);
            double qtl = Convert.ToDouble("0" + txtQNTL.Text);
            double diff = 0.0;


            if (saleRate == 0 && purcRate == 0)
            {
                txtDIFF_AMOUNT.Text = "0";
                lblDiff.Text = "0";

            }
            else
            {
                if (saleRate != 0)
                {
                    diff = saleRate - millRate;
                    diffAmt = Math.Round(diff * qtl, 2);
                    txtPURCHASE_RATE.Text = "";
                }
                else
                {
                    diff = millRate - purcRate;
                    diffAmt = Math.Round(diff * qtl, 2);
                    txtSALE_RATE.Text = "";
                }

            }
            lblDiff.Text = diff.ToString();
            txtDIFF_AMOUNT.Text = diffAmt.ToString();

        }
        catch
        {

        }
    }

    #region [txtPURCHASE_RATE_TextChanged]
    protected void txtPURCHASE_RATE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //if (txtPURCHASE_RATE.Text != string.Empty && txtMILL_RATE.Text != string.Empty)
            //{
            txtSALE_RATE.Text = string.Empty;
            this.calDiffAmount();
            this.calVoucherAmount();
            setFocusControl(txtCOMMISSION_RATE);
            //}
        }
        catch
        {

        }
    }
    #endregion

    #region [txtDIFF_AMOUNT_TextChanged]
    protected void txtDIFF_AMOUNT_TextChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region [txtVOUCHER_AMOUNT_TextChanged]

    protected void txtVOUCHER_AMOUNT_TextChanged(object sender, EventArgs e)
    {

    }
    #endregion

    #region [txtNARRATION1_TextChanged]
    protected void txtNARRATION1_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtNARRATION2);
    }
    #endregion

    #region [txtNARRATION2_TextChanged]
    protected void txtNARRATION2_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtNARRATION3);
    }
    #endregion

    #region [txtNARRATION3_TextChanged]
    protected void txtNARRATION3_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtNARRATION4);
    }
    #endregion

    #region [txtNARRATION4_TextChanged]
    protected void txtNARRATION4_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtDueDays);
    }
    #endregion

    #region [txtRAWANGI_DATE_TextChanged]
    protected void txtRAWANGI_DATE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string RwDate = DateTime.Parse(txtRAWANGI_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            if (clsCommon.isValidDate(RwDate) == true)
            {
                this.setFocusControl(txtNARRATION1);
            }
            else
            {
                txtRAWANGI_DATE.Text = string.Empty;
                this.setFocusControl(txtRAWANGI_DATE);
            }
        }
        catch
        {
            this.setFocusControl(txtRAWANGI_DATE);
        }

    }
    #endregion

    #region [txtBANK_CODE_TextChanged]
    protected void txtBANK_CODE_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string bankName = string.Empty;
            if (txtBANK_CODE.Text != string.Empty)
            {
                searchString = txtBANK_CODE.Text;
                bool a = clsCommon.isStringIsNumeric(txtBANK_CODE.Text);
                if (a == false)
                {
                    btntxtBANK_CODE_Click(this, new EventArgs());
                }
                else
                {
                    string qry = "select Ac_Name_E from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtBANK_CODE.Text;
                    bankName = clsCommon.getString(qry);
                    if (bankName != string.Empty)
                    {
                        lblBank_name.Text = bankName;
                        setFocusControl(txtNARRATION);
                    }
                    else
                    {
                        lblBank_name.Text = string.Empty;
                        txtBANK_CODE.Text = string.Empty;
                        setFocusControl(txtBANK_CODE);
                    }
                }
            }
            else
            {
                this.setFocusControl(txtBANK_CODE);
            }

        }
        catch
        {
            this.setFocusControl(txtRAWANGI_DATE);
        }
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
        setFocusControl(txtBANK_AMOUNT);
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
        setFocusControl(txtReConsilationDt);
    }
    #endregion

    #region [txtReConsilationDt_TextChanged]
    protected void txtReConsilationDt_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnAdddetails);
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty || hdnfClosePopup.Value == "txtDO")
            {
                txtSearchText.Text = searchString;
            }

            if (hdnfClosePopup.Value == "txtSuffix")
            {
                lblPopupHead.Text = "--Select--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtParty")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code" +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='B' and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGetpass")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code" +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='B' and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code" +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='M' and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')" +
                " and " + AccountMasterTable + ".Ac_type='M'";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGRADE")
            {
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBANK_CODE")
            {
                lblPopupHead.Text = "--Select Bank--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code" +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION")
            {
                lblPopupHead.Text = "--Select Bank--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtDO")
            {
                lblPopupHead.Text = "--Select DO--";
                //string qry = "select Ac_Code,Ac_Name_E from " + AccountMasterTable +
                //" where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (Ac_Code like '%" + txtSearchText.Text + "%' or" +
                //" Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtDOC_NO")
            {
                lblPopupHead.Text = "-- Select DOC No --";
                string qry = "SELECT     " + tblHead + ".Doc_No, " + tblHead + ".Suffix, Convert(varchar(10)," + tblHead + ".Doc_Date,103) as Doc_Date, " + tblHead + ".DO_No, Party.Ac_Name_E AS PartyName " +
                            " FROM         " + tblHead + " INNER JOIN " +
                            " " + AccountMasterTable + " AS Party ON " + tblHead + ".Ac_Code = Party.Ac_Code AND " + tblHead + ".Company_Code = Party.Company_Code " +
                            " where " + tblHead + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and " + tblHead + ".Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and" +
                            " Branch_Code='" + Session["Branch_Code"].ToString() + "' and Tran_Type='CV' " +
                            " and (Doc_No like '%" + txtSearchText.Text + "%' or Suffix like '%" + txtSearchText.Text + "%' or Doc_Date like '%" + txtSearchText.Text + "%'" +
                            " or DO_No like '%" + txtSearchText.Text + "%'  or Party.Ac_Name_E like '%" + txtSearchText.Text + "%')" +
                            " order by Doc_Date,Doc_No";
                this.showPopup(qry);
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
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                    }
                    //for (int i = 0; i < grdPopup.Rows.Count; i++)
                    //{
                    //    grdPopup.Rows[i].Cells[0].Width = Unit.Percentage(20);
                    //    grdPopup.Rows[i].Cells[1].Width = Unit.Percentage(80);
                    //}
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

            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            if (hdnfClosePopup.Value == "txtParty")
            {
                setFocusControl(txtParty);
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }

            hdnfClosePopup.Value = "Close";
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
        try
        {
            this.saveData();
        }
        catch
        {

        }
    }

    private void saveData()
    {
        try
        {
            bool isValidated = true;
            string retValue = "";
            string strRev = "";
            #region -----  Declared Part Unused-----
            /*string Tran_Type = "CV";
            Int32 Doc_No = txtDoc_no.Text != string.Empty ? Convert.ToInt32(txtDoc_no.Text) : 0;
            string Suffix = "";
            Int32 DO_No = txtDO.Text != string.Empty ? Convert.ToInt32(txtDO.Text) : 0;
            string Doc_Date = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd"); ;
            Int32 Ac_Code = txtParty.Text != string.Empty ? Convert.ToInt32(txtParty.Text) : 0;
            Int32 Mill_Code = txtMILL_CODE.Text != string.Empty ? Convert.ToInt32(txtMILL_CODE.Text) : 0;
            string From_Place = txtFrom.Text;
            string To_Place = txtTo.Text;
            double Quantal = txtQNTL.Text != string.Empty ? Convert.ToDouble(txtQNTL.Text) : 0.00;
            double Packing = txtPACKING.Text != string.Empty ? Convert.ToDouble(txtPACKING.Text) : 0.00;
            double Bags = txtBAGS.Text != string.Empty ? Convert.ToDouble(txtBAGS.Text) : 0.00;
            string Grade = txtGRADE.Text;
            string Lorry_No = txtLorryNo.Text;
            double Mill_Rate = txtMILL_RATE.Text != string.Empty ? Convert.ToDouble(txtMILL_RATE.Text) : 0.00;
            double Mill_Amount = txtMILL_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtMILL_AMOUNT.Text) : 0.00; ;
            double Commission_Rate = txtCOMMISSION_RATE.Text != string.Empty ? Convert.ToDouble(txtCOMMISSION_RATE.Text) : 0.00;
            double Commission_Amount = txtCOMMISSION.Text != string.Empty ? Convert.ToDouble(txtCOMMISSION.Text) : 0.00;
            double Loading_Charge = txtLOADING_CHARGE.Text != string.Empty ? Convert.ToDouble(txtLOADING_CHARGE.Text) : 0.00;
            double Sale_Rate = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0.00;
            double Purchase_Rate = txtPURCHASE_RATE.Text != string.Empty ? Convert.ToDouble(txtPURCHASE_RATE.Text) : 0.00;
            double Diff_Rate = 0.0;// txtPURCHASE_RATE.Text != string.Empty ? Convert.ToDouble(txtd.Text) : 0.00;
            double Diff_Amount = txtDIFF_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtDIFF_AMOUNT.Text) : 0.00;
            double Voucher_Amount = txtVOUCHER_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtVOUCHER_AMOUNT.Text) : 0.00;
            string Narration1 = txtNARRATION1.Text;
            string Narration2 = txtNARRATION2.Text;
            string Narration3 = txtNARRATION3.Text;
            string Narration4 = txtNARRATION4.Text;
            string Rawangi_Date = DateTime.Parse(txtRAWANGI_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");*/
            #endregion

            #region -----  Head Part Declaration  -----
            string Tran_Type = "CV";
            Int32 Doc_No = 0;
            string Suffix = "";
            Int32 DO_No = 0;
            string Doc_Date = "";
            Int32 Ac_Code = 0;
            Int32 Unit_Code = 0;
            string From_Place = "";
            string To_Place = "";
            double Quantal = 0;
            int Packing = 0;
            int Bags = 0;
            string Grade = "";
            string Lorry_No = string.Empty;
            Int32 Mill_Code = 0;
            double Mill_Rate = 0.0;
            double Mill_Amount = 0.0;
            double Commission_Rate = 0.0;
            double Commission_Amount = 0.0;
            double Loading_Charge = 0.0;
            double Sale_Rate = 0.0;
            double Purchase_Rate = 0.0;
            double Diff_Rate = 0.0;
            double Diff_Amount = 0.0;
            double Voucher_Amount = 0.0;
            string Narration1 = string.Empty;
            string Narration2 = string.Empty;
            string Narration3 = string.Empty;
            string Narration4 = string.Empty;
            string Rawangi_Date = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            int DueDays = txtDueDays.Text != string.Empty ? Convert.ToInt32(txtDueDays.Text) : 0;
            #endregion

            #region --- Head Assignment and Validation---
            if (txtDoc_no.Text != string.Empty)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    if (txtSuffix.Text.Trim() == string.Empty)
                    {
                        this.getMaxCode();

                        isValidated = true;
                    }
                    else
                    {
                        string str = clsCommon.getString("select Doc_No from " + tblHead + " where Tran_Type='CV' and Doc_No='" + txtDoc_no.Text + "'" +
                                 " and Suffix='" + txtSuffix.Text.Trim() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (str != string.Empty)
                        {
                            lblMsg.Text = "Doc No " + txtDoc_no.Text + " already exist";
                            isValidated = false;
                            setFocusControl(txtSuffix);
                            Doc_No = Convert.ToInt32(txtDoc_no.Text);
                            return;
                        }
                        else
                        {
                            isValidated = true;
                            Doc_No = Convert.ToInt32(txtDoc_no.Text);
                        }
                    }
                    Doc_No = Convert.ToInt32(txtDoc_no.Text);
                    hdnf.Value = txtDoc_no.Text;
                    hdnfSuffix.Value = txtSuffix.Text;
                }
                else
                {
                    Doc_No = Convert.ToInt32(txtDoc_no.Text);
                    isValidated = true;
                }
            }
            else
            {
                setFocusControl(txtDoc_no);
                isValidated = false;
                return;
            }

            Suffix = txtSuffix.Text;
            if (txtDO.Text != string.Empty)
            {
                DO_No = Convert.ToInt32(txtDO.Text);
            }
            else
            {
                DO_No = 0;
            }
            if (txtDOC_DATE.Text != string.Empty)
            {
                if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
                {
                    Doc_Date = DateTime.Parse(txtDOC_DATE.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    txtDOC_DATE.Text = string.Empty;
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
            if (txtParty.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Ac_Code=" + txtParty.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    Ac_Code = Convert.ToInt32(txtParty.Text);
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    txtParty.Text = string.Empty;
                    lblPartyName.Text = "";
                    setFocusControl(txtParty);
                    return;
                }

            }
            else
            {
                lblPartyName.Text = "";
                isValidated = false;
                setFocusControl(txtParty);
                return;
            }
            if (txtGetpass.Text != string.Empty)
            {
                Unit_Code = Convert.ToInt32(txtGetpass.Text);
            }
            else
            {
                Unit_Code = 0;
            }
            From_Place = txtFrom.Text;
            To_Place = txtTo.Text;
            if (txtQNTL.Text != string.Empty)
            {
                Quantal = Convert.ToDouble(txtQNTL.Text);
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtQNTL);
                return;
            }
            if (txtPACKING.Text != string.Empty)
            {
                Packing = Convert.ToInt32(txtPACKING.Text);
            }
            else
            {
                Packing = 0;
            }
            if (txtBAGS.Text != string.Empty)
            {
                Bags = Convert.ToInt32(txtBAGS.Text);
            }
            else
            {
                Bags = 0;
            }
            Grade = txtGRADE.Text;
            Lorry_No = txtLorryNo.Text;
            if (txtMILL_CODE.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (str != string.Empty)
                {
                    Mill_Code = Convert.ToInt32(txtMILL_CODE.Text);
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    lblMill_name.Text = "";
                    setFocusControl(txtMILL_CODE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                lblMill_name.Text = "";
                setFocusControl(txtMILL_CODE);
                return;
            }
            if (txtMILL_RATE.Text != string.Empty)
            {
                Mill_Rate = Convert.ToDouble(txtMILL_RATE.Text);
                isValidated = true;
            }
            else
            {
                setFocusControl(txtMILL_RATE);
                isValidated = false;
                return;

            }
            if (txtMILL_AMOUNT.Text != string.Empty)
            {
                Mill_Amount = Convert.ToDouble(txtMILL_AMOUNT.Text);
                isValidated = true;
            }

            else
            {
                Mill_Amount = 0.0;
                isValidated = false;
                setFocusControl(txtMILL_RATE);
                return;
            }
            if (txtCOMMISSION_RATE.Text != string.Empty)
            {
                Commission_Rate = Convert.ToDouble(txtCOMMISSION_RATE.Text);
            }
            else
            {
                Commission_Rate = 0.0;
            }

            if (txtCOMMISSION.Text != string.Empty)
            {
                Commission_Amount = Convert.ToDouble(txtCOMMISSION.Text);
            }
            else
            {
                Commission_Amount = 0.0;
            }

            if (txtLOADING_CHARGE.Text != string.Empty)
            {
                Loading_Charge = Convert.ToDouble(txtLOADING_CHARGE.Text);
            }
            else
            {
                Loading_Charge = 0.0;
            }
            if (txtSALE_RATE.Text != string.Empty)
            {
                Sale_Rate = Convert.ToDouble(txtSALE_RATE.Text);
            }
            else
            {
                Sale_Rate = 0.0;
            }
            if (txtPURCHASE_RATE.Text != string.Empty)
            {
                Purchase_Rate = Convert.ToDouble(txtPURCHASE_RATE.Text);
            }
            else
            {
                Purchase_Rate = 0.0;
            }
            //Diff_Rate = Convert.ToDouble(txtdiff.Text);
            if (txtDIFF_AMOUNT.Text != string.Empty)
            {
                Diff_Amount = Convert.ToDouble(txtDIFF_AMOUNT.Text);
            }
            else
            {
                Diff_Amount = 0.0;
            }
            if (txtVOUCHER_AMOUNT.Text != string.Empty)
            {
                Voucher_Amount = Convert.ToDouble(txtVOUCHER_AMOUNT.Text);
            }
            else
            {
                Voucher_Amount = 0.0;
            }
            Narration1 = txtNARRATION1.Text;
            Narration2 = txtNARRATION2.Text;
            Narration3 = txtNARRATION3.Text;
            Narration4 = txtNARRATION4.Text;
            if (txtRAWANGI_DATE.Text != string.Empty)
            {
                if (clsCommon.isValidDate(txtRAWANGI_DATE.Text) == true)
                {
                    Rawangi_Date = DateTime.Parse(txtRAWANGI_DATE.Text, CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                }
                else
                {
                    txtRAWANGI_DATE.Text = string.Empty;
                    setFocusControl(txtRAWANGI_DATE);
                    isValidated = false;
                    return;
                }
            }
            else
            {
                Rawangi_Date = "";
                isValidated = true;
            }
            #region Diff Valid
            if (grdDetail.Rows.Count > 0)
            {
                double diff = Convert.ToDouble("0" + lblGridDiff.Text);
                if (diff != 0)
                {
                    isValidated = false;
                    lblMsg.Text = "Diff Not Valid";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    setFocusControl(btnOpenDetailsPopup);
                    return;
                }
                else
                {
                    isValidated = true;
                }
            }

            int count = 0;
            if (grdDetail.Rows.Count > 1)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[8].Text.ToString() == "D")
                    {
                        count++;
                    }
                }
                if (grdDetail.Rows.Count == count)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Add Details!');", true);
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
                    if (grdDetail.Rows[i].Cells[8].Text.ToString() != "D")
                    {
                        double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                        total += amount;
                    }
                }
                if (total == Convert.ToDouble(txtMILL_AMOUNT.Text))
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

            #endregion
            #endregion

            #region ---------- save ----------
            if (isValidated == true)
            {
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    if (ViewState["mode"] != null)
                    {
                        DataSet ds = new DataSet();
                        if (ViewState["mode"].ToString() == "I")
                        {
                            #region ---- Voucher ----
                            obj.flag = 1;
                            obj.tableName = tblHead;
                            obj.columnNm = "Tran_Type,Doc_No,Suffix,DO_No,Doc_Date,Ac_Code,Unit_Code,From_Place,To_Place,Quantal,Grade,Lorry_No,Mill_Code,Mill_Rate,Mill_Amount,Commission_Rate,Commission_Amount,Loading_Charge,Sale_Rate, " +
                            " Purchase_Rate,Diff_Rate,Diff_Amount,Voucher_Amount,Narration1,Narration2,Narration3,Narration4,Due_Days,Rawangi_Date,Company_Code,Year_Code,Branch_Code,Created_By";
                            obj.values = "'" + Tran_Type + "','" + Doc_No + "','" + Suffix + "','" + DO_No + "','" + Doc_Date + "','" + Ac_Code + "','" + Unit_Code + "','" + From_Place + "','" + To_Place + "','" +
                             Quantal + "','" + Grade + "','" + Lorry_No + "','" + Mill_Code + "','" + Mill_Rate + "','" + Mill_Amount + "','" + Commission_Rate + "','" +
                             Commission_Amount + "','" + Loading_Charge + "','" + Sale_Rate + "', " +
                            " '" + Purchase_Rate + "','" + Diff_Rate + "','" + Diff_Amount + "','" + Voucher_Amount + "','" + Narration1 + "','" +
                            Narration2 + "','" + Narration3 + "','" + Narration4 + "','" + DueDays + "','" + Rawangi_Date + "','" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + user + "'";

                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;
                        }
                        else
                        {
                            //Update Mode
                            obj.flag = 2;
                            obj.tableName = tblHead;
                            obj.columnNm = "DO_No='" + DO_No + "',Doc_Date='" + Doc_Date + "',Ac_Code='" + Ac_Code + "',Unit_Code='" + Unit_Code + "',From_Place='" + From_Place +
                            "',To_Place='" + To_Place + "',Quantal='" + Quantal + "',Grade='" + Grade + "',Lorry_No='" + Lorry_No + "'" +
                            " ,Mill_Code='" + Mill_Code + "',Mill_Rate='" + Mill_Rate + "',Mill_Amount='" + Mill_Amount + "',Commission_Rate='" + Commission_Rate +
                            "',Commission_Amount='" +
                             Commission_Amount + "',Loading_Charge='" + Loading_Charge + "',Sale_Rate='" + Sale_Rate + "', " +
                            " Purchase_Rate='" + Purchase_Rate + "',Diff_Rate='" + Diff_Rate + "',Diff_Amount='" + Diff_Amount + "',Voucher_Amount='" + Voucher_Amount + "',Narration1='" + Narration1 + "'," +
                             " Narration2='" +
                            Narration2 + "',Narration3='" + Narration3 + "',Narration4='" + Narration4 + "',Due_Days='" + DueDays + "',Rawangi_Date='" + Rawangi_Date + "',Modified_By='" + user +
                            "'where Company_Code='" + Company_Code + "' and Year_Code='" + Year_Code + "' and Branch_Code='" + Branch_Code +
                            "' and Tran_Type='" + Tran_Type + "' and Doc_No='" + Doc_No + "' and Suffix='" + Suffix + "'";

                            obj.values = "none";
                            ds = new DataSet();
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;
                        }
                            #endregion

                        #region -------------------- Voucher Details --------------------

                        Int32 Bank_Code = 0;
                        string Narration = "";
                        double Amount = 0.00;
                        Int32 Order_Code = 0;
                        string re_date = "";
                        string i_d = "";

                        if (strRev == "-1" || strRev == "-2")
                        {
                            if (grdDetail.Rows.Count > 0)
                            {
                                for (int i = 0; i < grdDetail.Rows.Count; i++)
                                {
                                    Bank_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text);
                                    Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                                    Order_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[9].Text);
                                    if (grdDetail.Rows[i].Cells[7].Text != string.Empty && grdDetail.Rows[i].Cells[7].Text != "&nbsp;")
                                    {
                                        re_date = grdDetail.Rows[i].Cells[7].Text.Trim();
                                        re_date = DateTime.Parse(re_date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                                    }
                                    i_d = grdDetail.Rows[i].Cells[2].Text;

                                    if (grdDetail.Rows[i].Cells[8].Text != "N" && grdDetail.Rows[i].Cells[8].Text != "R")
                                    {
                                        if (grdDetail.Rows[i].Cells[8].Text == "A")
                                        {
                                            obj.flag = 1;
                                            obj.tableName = tblDetails;
                                            obj.columnNm = "Tran_Type,Doc_No,Suffix,Bank_Code,Narration,Amount,Company_Code,year_Code,Branch_Code,Order_Code,re_date,ID";
                                            obj.values = "'" + Tran_Type + "','" + Doc_No + "','" + Suffix + "','" + Bank_Code + "','" + Narration + "','" + Amount + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + Order_Code + "','" + re_date + "','" + i_d + "'";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);
                                        }
                                        if (grdDetail.Rows[i].Cells[8].Text == "U")
                                        {
                                            obj.flag = 2;
                                            obj.tableName = tblDetails;
                                            obj.columnNm = "Bank_Code='" + Bank_Code + "',Narration='" + Narration + "',Amount='" + Amount + "',Order_Code='" + Order_Code + "',re_date='" + re_date + "'" +
                                                " where Company_Code='" + Company_Code + "' and year_Code='" + year_Code + "' and Branch_Code=" + Branch_Code +
                                                " and Tran_Type='" + Tran_Type + "' and Doc_No='" + Doc_No + "' and Suffix='" + Suffix.Trim() + "' and ID=" + i_d;
                                            obj.values = "none";
                                            ds = new DataSet();
                                            ds = obj.insertAccountMaster(ref strRev);
                                        }
                                        if (grdDetail.Rows[i].Cells[8].Text == "D")
                                        {
                                            obj.flag = 3;
                                            obj.tableName = tblDetails;
                                            obj.columnNm = "Company_Code='" + Company_Code + "' and year_Code='" + year_Code + "' and Branch_Code=" + Branch_Code +
                                                " and Tran_Type='" + Tran_Type + "' and Doc_No='" + Doc_No + "' and Suffix='" + Suffix.Trim() + "' and ID=" + i_d;
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
                            clsGledgerupdations ledger = new clsGledgerupdations();
                            ledger.CommisionVoucherGlederEffect("CV","", Doc_No, Company_Code, year_Code);
                        }
                        #endregion

                        if (retValue == "-1")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                            string qry = getDisplayQuery(); ;
                            this.fetchRecord(qry);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added !');", true);
                        }
                        if (retValue == "-2" || retValue == "-3")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                            string qry = getDisplayQuery(); ;
                            this.fetchRecord(qry);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                        }
                    }
                }
            }
            #endregion
        }
        catch
        {

        }
    }
    #endregion

    protected void txtDO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string doName = string.Empty;
            if (txtDO.Text != string.Empty)
            {
                searchString = txtDO.Text;
                string qry = "";
                //qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtDO.Text;
                doName = clsCommon.getString(qry);

                if (doName != string.Empty)
                {
                    lblDOName.Text = doName;
                    setFocusControl(txtDOC_DATE);
                }
                else
                {
                    lblDOName.Text = string.Empty;
                    txtDO.Text = string.Empty;
                    setFocusControl(txtDO);
                }
            }
            else
            {
                setFocusControl(txtDO);
            }
        }
        catch
        {
        }
    }
    protected void btnDO_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDO";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    //protected void txtFrom_TextChanged(object sender, EventArgs e)
    //{
    //    setFocusControl(txtTo);
    //}
    //protected void txtTo_TextChanged(object sender, EventArgs e)
    //{
    //    setFocusControl(txtQNTL);
    //}
    protected void txtLorryNo_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtQNTL);
    }

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (txtMILL_CODE.Text != string.Empty)
            {
                setFocusControl(txtGRADE);
            }
            else
            {
                setFocusControl(txtMILL_CODE);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Suffix='" + hdnfSuffix.Value + "'" +
                            " and Tran_Type='CV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                           " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    protected void txtMILL_AMOUNT_TextChanged1(object sender, EventArgs e)
    {
        setFocusControl(txtPURCHASE_RATE);
    }
    protected void txtCOMMISSION_TextChanged1(object sender, EventArgs e)
    {
        setFocusControl(txtLOADING_CHARGE);
    }
    protected void txtDueDays_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtRAWANGI_DATE);
    }
}

