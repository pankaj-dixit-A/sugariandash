using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeLocalvoucherForGST : System.Web.UI.Page
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
    string millShortName = string.Empty;
    string GLedgerTable = string.Empty;
    string Tran_Type = "LV";             //Local Voucher
    static WebControl objAsp = null;
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
            user = Session["user"].ToString();
            tblHead = tblPrefix + "Voucher";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            qryAccountList = tblPrefix + "qryAccountsList";
            qryCommon = tblPrefix + "qryVoucherList";
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
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
                    if (Session["LV_NO"] != null)
                    {
                        hdnf.Value = Session["LV_NO"].ToString();
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        this.enableDisableNavigateButtons();
                        Session["LV_NO"] = null;
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
            hdnf.Value = txtEditDoc_No.Text;
            string qry = "select *,CONVERT(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no='" + txtEditDoc_No.Text + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and tran_type='" + Tran_Type + "'"; ;
            this.fetchRecord(qry);
            setFocusControl(txtEditDoc_No);
            //pnlgrdDetail.Enabled = true;
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
                obj.tableName = tblHead + " where Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
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
                                    txtdoc_no.Text = ds.Tables[0].Rows[0][0].ToString();
                                    txtdoc_no.Enabled = false;
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

                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                lblMsg.Text = string.Empty;
                txtEditDoc_No.Enabled = true;
                #region set Business logic
                lblAc_name.Text = string.Empty;
                lblUnitName.Text = string.Empty;
                lblMill_name.Text = string.Empty;
                lblBroker_name.Text = string.Empty;
                lblDiff.Text = string.Empty;

                btntxtDONO.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtUnitcode.Enabled = false;
                btntxtNarration1.Enabled = false;
                btntxtNarration2.Enabled = false;
                btntxtNarration3.Enabled = false;
                btntxtNarration4.Enabled = false;
                calenderExtenderDate.Enabled = false;
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
                btntxtDOC_NO.Text = "Change No";
                btntxtDOC_NO.Enabled = true;
                txtdoc_no.Enabled = false;
                lblMsg.Text = "";
                lblTenderNo.Text = "";
                #region set Business logic for save
                lblAc_name.Text = string.Empty;
                lblUnitName.Text = string.Empty;
                lblMill_name.Text = string.Empty;
                lblBroker_name.Text = string.Empty;
                lblDiff.Text = string.Empty;
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                btntxtDONO.Enabled = true;
                btntxtAC_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtGRADE.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtNarration1.Enabled = true;
                btntxtNarration2.Enabled = true;
                btntxtNarration3.Enabled = true;
                btntxtNarration4.Enabled = true;
                btntxtUnitcode.Enabled = true;
                calenderExtenderDate.Enabled = true;
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

                #region set Business logic for save


                btntxtDONO.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtNarration1.Enabled = false;
                btntxtNarration2.Enabled = false;
                btntxtNarration3.Enabled = false;
                btntxtNarration4.Enabled = false;
                btntxtUnitcode.Enabled = false;
                calenderExtenderDate.Enabled = false;
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
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = true;
                lblMsg.Text = string.Empty;
                #region logic
                btntxtDONO.Enabled = true;
                btntxtAC_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtGRADE.Enabled = true;
                btntxtUnitcode.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtNarration1.Enabled = true;
                btntxtNarration2.Enabled = true;
                btntxtNarration3.Enabled = true;
                btntxtNarration4.Enabled = true;
                calenderExtenderDate.Enabled = true;
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
            #region tenderdata
            //Page tenderPage = Page.PreviousPage;
            //if (tenderPage != null)
            //{
            //    string abc=((TextBox)tenderPage.FindControl("txtMillRate")).Text.ToString();
            //    txtMILL_RATE.Text = ((TextBox)tenderPage.FindControl("txtMillRate")).Text;
            //    txtQNTL.Text = ((TextBox)tenderPage.FindControl("txtQuantal")).Text;
            //    txtPACKING.Text = ((TextBox)tenderPage.FindControl("txtPacking")).Text;
            //    txtBAGS.Text = ((TextBox)tenderPage.FindControl("txtBags")).Text;
            //    txtGRADE.Text = ((TextBox)tenderPage.FindControl("txtGrade")).Text;
            //    txtPURCHASE_RATE.Text = ((TextBox)tenderPage.FindControl("txtPurcRate")).Text;
            //}
            #endregion
            string qry = string.Empty;
            qry = "select max(doc_no) as doc_no from " + tblHead +
                 " where Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
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
                        qry = "select Suffix from " + tblHead + " where doc_no=" + hdnf.Value +
                            " and Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                            " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                        hdnfSuffix.Value = clsCommon.getString(qry);
                        qry = getDisplayQuery();
                        bool recordExist = this.fetchRecord(qry);
                        if (recordExist == true)
                        {
                            btnEdit.Focus();
                        }
                        else                            //new code
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
        query = "   select count(*) from " + tblHead + " where  Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
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
        if (txtdoc_no.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not

                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    " and Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " ORDER BY doc_no asc  ";
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


                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                    " and Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " ORDER BY doc_no asc  ";
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") " +
                "  and Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                    " and  Tran_Type='" + Tran_Type + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
                    " and  Tran_Type='" + Tran_Type + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")  " +
                " and  Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
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

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.getMaxCode();
        setFocusControl(txtDONO);
        txtGSTRateCode.Text = "2";
        string gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=1  and Company_Code="
            + Convert.ToInt32(Session["Company_Code"].ToString()));
        lblGSTRateName.Text = gstname;
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
        if (lblTenderNo.Text != string.Empty)
        {
            txtDONO.Enabled = false;
            btntxtDONO.Enabled = false;
        }
        else
        {
            txtDONO.Enabled = true;
            btntxtDONO.Enabled = true;
        }
        txtdoc_no.Enabled = false;
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string currentDoc_No = txtdoc_no.Text;
                string currentSuffix = txtSUFFIX.Text;
                DataSet ds = new DataSet();
                string query = "";
                query = "";
                query = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(query);

                string strrev = "";
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 3;
                    obj.tableName = tblHead;
                    obj.columnNm = "  Tran_Type='" + Tran_Type + "' and Doc_No=" + currentDoc_No + " and Suffix='" + currentSuffix.Trim() + "'" +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);

                    if (!string.IsNullOrEmpty(lblTenderNo.Text))
                    {
                        //updating Tender Table
                        int TenderNo = Convert.ToString(lblTenderNo.Text.Trim()) != string.Empty ? Convert.ToInt32(lblTenderNo.Text.Trim()) : 0;
                        //string TenderNo = lblTenderNo.Text.Trim().ToString(); 
                        string str = "2";
                        obj.flag = 2;
                        obj.tableName = tblPrefix + "Tender";
                        obj.columnNm = "Voucher_No='" + 0 + "' where Tender_No='" + TenderNo + "'and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                        obj.values = "none";
                        ds = obj.insertAccountMaster(ref str);
                    }
                    if (!string.IsNullOrEmpty(txtDONO.Text))
                    {
                        //update DO to set memo no=0
                        string qry = "";
                        qry = "update " + tblPrefix + "deliveryorder set voucher_no=0,voucher_type=''  where tran_type='DO' and doc_no=" + txtDONO.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);
                    }
                }
                if (strrev == "-3")
                {
                    query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                           " and Tran_Type='" + Tran_Type + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                            " ORDER BY Doc_No asc  ";


                    hdnf.Value = clsCommon.getString(query);

                    if (hdnf.Value == string.Empty)
                    {
                        query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                             " and Tran_Type='" + Tran_Type + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                            " ORDER BY Doc_No desc  ";
                        hdnf.Value = clsCommon.getString(query);
                    }

                    if (hdnf.Value != string.Empty)
                    {
                        query = "select Suffix from " + tblHead + " where doc_no=" + hdnf.Value +
                           " and Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                           " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                        hdnfSuffix.Value = clsCommon.getString(query);

                        query = getDisplayQuery(); ;
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
        string str = clsCommon.getString("select count(Doc_No) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + Tran_Type + "'");

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
                        txtdoc_no.Text = dt.Rows[0]["DOC_NO"].ToString();
                        txtSUFFIX.Text = dt.Rows[0]["SUFFIX"].ToString();
                        txtDONO.Text = dt.Rows[0]["DO_No"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["DOC_DATE"].ToString();
                        txtAC_CODE.Text = dt.Rows[0]["AC_CODE"].ToString();
                        lblAc_name.Text = dt.Rows[0]["PartyName"].ToString();
                        txtUnit_Code.Text = dt.Rows[0]["Unit_Code"].ToString();
                        lblUnitName.Text = dt.Rows[0]["Unit_Name"].ToString();
                        txtBroker_CODE.Text = dt.Rows[0]["BROKER_CODE"].ToString();
                        lblBroker_name.Text = dt.Rows[0]["BrokerName"].ToString();
                        string transportcode = dt.Rows[0]["TRANSPORT_CODE"].ToString();
                        txtTRANSPORT_CODE.Text = transportcode;
                        LBLTRANSPORT_NAME.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + transportcode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        txtQNTL.Text = dt.Rows[0]["Quantal"].ToString();
                        txtPACKING.Text = dt.Rows[0]["PACKING"].ToString();
                        txtBAGS.Text = dt.Rows[0]["BAGS"].ToString();
                        txtGRADE.Text = dt.Rows[0]["GRADE"].ToString();
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        lblMill_name.Text = dt.Rows[0]["MillName"].ToString();
                        txtMILL_RATE.Text = dt.Rows[0]["MILL_RATE"].ToString();
                        txtSALE_RATE.Text = dt.Rows[0]["SALE_RATE"].ToString();
                        lblDiff.Text = dt.Rows[0]["Diff_Amount"].ToString();
                        txtPURCHASE_RATE.Text = dt.Rows[0]["PURCHASE_RATE"].ToString();
                        txtRDiffTender.Text = dt.Rows[0]["RDIFFTENDER"].ToString();
                        txtNarration1.Text = dt.Rows[0]["NARRATION1"].ToString();
                        txtPostage.Text = dt.Rows[0]["POSTAGE"].ToString();
                        txtNarration2.Text = dt.Rows[0]["NARRATION2"].ToString();
                        txtCommissionPerQntl.Text = dt.Rows[0]["Commission_Rate"].ToString();
                        txtResale_Commisson.Text = dt.Rows[0]["RESALE_COMMISSON"].ToString();
                        txtNarration3.Text = dt.Rows[0]["NARRATION3"].ToString();
                        txtBANK_COMMISSION.Text = dt.Rows[0]["BANK_COMMISSION"].ToString();
                        txtNarration4.Text = dt.Rows[0]["NARRATION4"].ToString();
                        txtFREIGHT.Text = dt.Rows[0]["FREIGHT"].ToString();
                        txtOTHER_Expenses.Text = dt.Rows[0]["OTHER_EXPENSES"].ToString();
                        txtVoucher_Amount.Text = dt.Rows[0]["VOUCHER_AMOUNT"].ToString();

                        txtTaxableAmount.Text = dt.Rows[0]["TaxableAmount"].ToString();
                        txtCGSTRate.Text = dt.Rows[0]["CGSTRate"].ToString();
                        txtCGSTAmount.Text = dt.Rows[0]["CGSTAmount"].ToString();
                        txtSGSTRate.Text = dt.Rows[0]["SGSTRate"].ToString();
                        txtSGSTAmount.Text = dt.Rows[0]["SGSTAmount"].ToString();
                        txtIGSTRate.Text = dt.Rows[0]["IGSTRate"].ToString();
                        txtIGSTAmount.Text = dt.Rows[0]["IGSTAmount"].ToString();
                        txtNarration5.Text = dt.Rows[0]["Narration5"].ToString();

                        millShortName = dt.Rows[0]["millshortname"].ToString();
                        txtVoucher_Amount.Text = dt.Rows[0]["Voucher_Amount"].ToString();
                        lblDiff.Text = dt.Rows[0]["Diff_Amount"].ToString();
                        txtNarration1.Text = dt.Rows[0]["Narration1"].ToString();
                        lblTenderNo.Text = dt.Rows[0]["Tender_No"].ToString();
                        txtDueDays.Text = dt.Rows[0]["Due_Days"].ToString();
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
                        //lblVoucherBy.Text = dt.Rows[0]["Ac_Name_E"].ToString();
                        hdnf.Value = txtdoc_no.Text;
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtParty" || v == "txtMILL_CODE" || v == "txtBANK_CODE")
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(60);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(30);
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[0].Style["overflow"] = "hidden";
                e.Row.Cells[1].Style["overflow"] = "hidden";
                e.Row.Cells[2].Style["overflow"] = "hidden";
            }
            if (v == "txtdoc_no")
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(50);
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v == "txtDONO")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("250px");
                e.Row.Cells[2].Width = new Unit("50px");
                e.Row.Cells[3].Width = new Unit("50px");
                e.Row.Cells[4].Width = new Unit("50px");
                e.Row.Cells[7].Width = new Unit("40px");
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


    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon +
                " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                " and Tran_Type='" + Tran_Type + "' and Doc_No=" + hdnf.Value + " and Suffix='" + hdnfSuffix.Value.Trim() + "'";
            return qryDisplay;
        }
        catch
        {
            return "";
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

    #region [txtSUFFIX_TextChanged]
    protected void txtSUFFIX_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSUFFIX.Text;
        strTextBox = "txtSUFFIX";
        csCalculations();
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
                txtdoc_no.Text = string.Empty;
                txtdoc_no.Enabled = true;
                btnSave.Enabled = false;
                setFocusControl(txtdoc_no);
            }
            if (btntxtDOC_NO.Text == "Choose No")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtdoc_no";
                btnSearch_Click(sender, e);

            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtSUFFIX_Click]
    protected void btntxtSUFFIX_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSUFFIX";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDONO_TextChanged]
    protected void txtDONO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDONO.Text;
        strTextBox = "txtDONO";
        csCalculations();
    }
    #endregion

    #region [btntxtDONO_Click]
    protected void btntxtDONO_Click(object sender, EventArgs e)
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
        strTextBox = "txtAC_CODE";
        searchString = txtAC_CODE.Text;
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

    #region [txtQNTL_TextChanged]
    protected void txtQNTL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtQNTL.Text;
        strTextBox = "txtQNTL";
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

        csCalculations();
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

    #region [txtMILL_RATE_TextChanged]
    protected void txtMILL_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMILL_RATE.Text;
        strTextBox = "txtMILL_RATE";
        csCalculations();
    }
    #endregion

    #region [txtSALE_RATE_TextChanged]
    protected void txtSALE_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSALE_RATE.Text;
        strTextBox = "txtSALE_RATE";
        csCalculations();
    }
    #endregion

    #region [txtPURCHASE_RATE_TextChanged]
    protected void txtPURCHASE_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPURCHASE_RATE.Text;
        strTextBox = "txtPURCHASE_RATE";
        txtSALE_RATE.Text = string.Empty;
        csCalculations();
    }
    #endregion

    #region [txtRDiffTender_TextChanged]
    protected void txtRDiffTender_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRDiffTender.Text;
        strTextBox = "txtRDiffTender";
        csCalculations();
    }
    #endregion

    #region [txtNarration1_TextChanged]
    protected void txtNarration1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNarration1.Text;
        strTextBox = "txtNarration1";
        csCalculations();
    }
    #endregion

    #region [btntxtNarration1_Click]
    protected void btntxtNarration1_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNarration1";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPostage_TextChanged]
    protected void txtPostage_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPostage.Text;
        strTextBox = "txtPostage";
        csCalculations();
    }
    #endregion

    #region [txtNarration2_TextChanged]
    protected void txtNarration2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNarration2.Text;
        strTextBox = "txtNarration2";
        csCalculations();
    }
    #endregion

    #region [btntxtNarration2_Click]
    protected void btntxtNarration2_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNarration2";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtResale_Commisson_TextChanged]
    protected void txtResale_Commisson_TextChanged(object sender, EventArgs e)
    {
        searchString = txtResale_Commisson.Text;
        strTextBox = "txtResale_Commisson";
        csCalculations();
    }
    #endregion

    #region [txtNarration3_TextChanged]
    protected void txtNarration3_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNarration3.Text;
        strTextBox = "txtNarration3";
        csCalculations();
    }
    #endregion

    #region [btntxtNarration3_Click]
    protected void btntxtNarration3_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNarration3";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
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

    #region [txtNarration4_TextChanged]
    protected void txtNarration4_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNarration4.Text;
        strTextBox = "txtNarration3";
        csCalculations();
    }
    #endregion

    #region [btntxtNarration4_Click]
    protected void btntxtNarration4_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNarration4";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
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

    #region [txtOTHER_Expenses_TextChanged]
    protected void txtOTHER_Expenses_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOTHER_Expenses.Text;
        strTextBox = "txtOTHER_Expenses";
        csCalculations();
    }
    #endregion

    #region [txtVoucher_Amount_TextChanged]
    protected void txtVoucher_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVoucher_Amount.Text;
        csCalculations();
    }
    #endregion

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
                searchString = txtSearchText.Text;
            }

            if (hdnfClosePopup.Value == "txtdoc_no")
            {
                lblPopupHead.Text = "--Select Voucher--";
                string qry = "SELECT  " + tblHead + ".Doc_No, " + tblHead + ".Suffix, " + tblHead + ".DO_No, Convert(varchar(10)," + tblHead + ".Doc_Date,103) as Doc_Date, " +
                " Party.Ac_Name_E AS PartyName, " + tblHead + ".Quantal FROM  " + AccountMasterTable + " AS Party left outer JOIN " +
                " " + tblHead + " ON Party.Ac_Code = " + tblHead + ".Ac_Code and Party.Company_Code = " + tblHead + ".Company_Code where " + tblHead + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    "  and " + tblHead + ".Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " and " + tblHead + ".Tran_Type='" + Tran_Type + "' and (Party.Ac_Name_E like '%" + searchString + "%' or " + tblHead + ".Doc_No like '%" + searchString + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtDONO")
            {
                lblPopupHead.Text = "--Select DO--";
                string qry = "select doc_no,VoucherByname as Party,quantal,mill_rate,sale_rate,millShortName as mill,truck_no,BrokerName from " + tblPrefix + "qryDeliveryOrderList where voucher_no=0 and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO'" +
                    " and (doc_no like '%" + txtSearchText.Text + "%' or VoucherByname like '%" + txtSearchText.Text + "%' or quantal like '%" + txtSearchText.Text + "%' or millShortName like '%" + txtSearchText.Text + "%' or truck_no like '%" + txtSearchText.Text + "%' or BrokerName like '%" + txtSearchText.Text + "%') " +
                    " group by doc_no,VoucherByname,quantal,mill_rate,sale_rate,millShortName,truck_no,BrokerName";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {

                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code " +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                lblPopupHead.Text = "--Select Transport--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code " +
                " where " + AccountMasterTable + ".Ac_type='T' and " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
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
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {

                lblPopupHead.Text = "--Select Broker--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code " +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGRADE")
            {
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select Ac_Code,Ac_Name_E,city_name_e from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code " +
                " where " + AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%')" +
                " and " + AccountMasterTable + ".Ac_type='M'";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNarration1")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E from SystemMaster where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNarration2")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E from SystemMaster where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNarration3")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E from SystemMaster where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNarration4")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E from SystemMaster where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
            #region validation
            bool isValidated = true;

            if (txtdoc_no.Text != string.Empty)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    if (txtSUFFIX.Text.Trim() == string.Empty)
                    {
                        this.getMaxCode();
                        isValidated = true;
                    }
                    else
                    {
                        string str = clsCommon.getString("select Doc_No from " + tblHead + " where Tran_Type='" + Tran_Type + "' and Doc_No='" + txtdoc_no.Text + "'" +
                                 " and Suffix='" + txtSUFFIX.Text.Trim() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                        if (str != string.Empty)
                        {
                            lblMsg.Text = "Doc No " + txtdoc_no.Text + " already exist";
                            isValidated = false;
                            setFocusControl(txtSUFFIX);
                            return;
                        }
                        else
                        {
                            isValidated = true;
                        }
                    }
                }
                else
                {
                    isValidated = true;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtdoc_no);
                hdnf.Value = txtdoc_no.Text;
                return;
            }
            if (txtDOC_DATE.Text != string.Empty)
            {
                try
                {
                    string strDt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    isValidated = true;
                }
                catch
                {
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
            if (txtQNTL.Text != string.Empty)
            {
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
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPACKING);
                return;
            }
            if (txtBAGS.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtBAGS);
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

            double frieght = txtFREIGHT.Text != string.Empty ? Convert.ToDouble(txtFREIGHT.Text) : 0.0;
            if (frieght != 0)
            {
                if (txtTRANSPORT_CODE.Text != string.Empty || txtTRANSPORT_CODE.Text.Trim() != "0")
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtTRANSPORT_CODE);
                    return;
                }
            }

            #endregion

            #region -Head part declearation
            string T_Type = Tran_Type;
            Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
            string SUFFIX = txtSUFFIX.Text;
            Int32 DO_No = txtDONO.Text != string.Empty ? Convert.ToInt32(txtDONO.Text) : 0;
            string DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            Int32 AC_CODE = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
            Int32 Unit_Code = txtUnit_Code.Text != string.Empty ? Convert.ToInt32(txtUnit_Code.Text) : 0;
            Int32 BROKER_CODE = txtBroker_CODE.Text != string.Empty ? Convert.ToInt32(txtBroker_CODE.Text) : 0;
            double Quantal = txtQNTL.Text != string.Empty ? Convert.ToDouble(txtQNTL.Text) : 0.00;
            Int32 PACKING = txtPACKING.Text != string.Empty ? Convert.ToInt32(txtPACKING.Text) : 0;
            double BAGS = txtBAGS.Text != string.Empty ? Convert.ToDouble(txtBAGS.Text) : 0.00;
            string GRADE = txtGRADE.Text;
            Int32 MILL_CODE = txtMILL_CODE.Text != string.Empty ? Convert.ToInt32(txtMILL_CODE.Text) : 0;
            double MILL_RATE = txtMILL_RATE.Text != string.Empty ? Convert.ToDouble(txtMILL_RATE.Text) : 0.00;
            double SALE_RATE = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0.00;
            double PURCHASE_RATE = txtPURCHASE_RATE.Text != string.Empty ? Convert.ToDouble(txtPURCHASE_RATE.Text) : 0.00;
            double RDIFFTENDER = txtRDiffTender.Text != string.Empty ? Convert.ToDouble(txtRDiffTender.Text) : 0.00;
            string NARRATION1 = txtNarration1.Text;
            double POSTAGE = txtPostage.Text != string.Empty ? Convert.ToDouble(txtPostage.Text) : 0.00;
            string NARRATION2 = txtNarration2.Text;
            double RESALE_COMMISSON = txtResale_Commisson.Text != string.Empty ? Convert.ToDouble(txtResale_Commisson.Text) : 0.00;
            string NARRATION3 = txtNarration3.Text;
            double BANK_COMMISSION = txtBANK_COMMISSION.Text != string.Empty ? Convert.ToDouble(txtBANK_COMMISSION.Text) : 0.00;
            string NARRATION4 = txtNarration4.Text;
            double FREIGHT = txtFREIGHT.Text != string.Empty ? Convert.ToDouble(txtFREIGHT.Text) : 0.00;
            Int32 TRANSPORT_CODE = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
            double OTHER_EXPENSES = txtOTHER_Expenses.Text != string.Empty ? Convert.ToDouble(txtOTHER_Expenses.Text) : 0.00;

            double VOUCHER_AMOUNT = txtVoucher_Amount.Text != string.Empty ? Convert.ToDouble(txtVoucher_Amount.Text) : 0.00;
            // double VOUCHER_AMOUNT = txtTaxableAmount.Text != string.Empty ? Convert.ToDouble(txtTaxableAmount.Text) : 0.00;


            double Diff_Amount = lblDiff.Text != string.Empty ? Convert.ToDouble(lblDiff.Text) : 0.00;
            double Commission_Rate = txtCommissionPerQntl.Text != string.Empty ? Convert.ToDouble(txtCommissionPerQntl.Text) : 0.00;
            Int32 Due_Days = txtDueDays.Text != string.Empty ? Convert.ToInt32(txtDueDays.Text) : 0;
            Int32 DONO = txtDONO.Text != string.Empty ? Convert.ToInt32(txtDONO.Text) : 0;
            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            string myNarration = string.Empty;

            millShortName = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + MILL_CODE + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string partyShortName = string.Empty;
            string brokerShortName = string.Empty;
            if (BROKER_CODE != 0 || BROKER_CODE != 2)
            {
                brokerShortName = clsCommon.getString("select Short_Name from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + BROKER_CODE + "");

            }
            partyShortName = clsCommon.getString("select Short_Name from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + AC_CODE + "");

            string partyNarration = string.Empty;
            if (PURCHASE_RATE > 0)
            {
                //if (ViewState["mode"].ToString() == "I")
                //{
                myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + MILL_RATE + " P.R." + PURCHASE_RATE + ") " + partyShortName + " " + brokerShortName;
                partyNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + MILL_RATE + " P.R." + PURCHASE_RATE + ") " + brokerShortName;
                //}
            }
            else
            {
                //if (ViewState["mode"].ToString() == "I")
                //{
                myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + MILL_RATE + " S.R." + SALE_RATE + ") " + partyShortName + " " + brokerShortName;
                partyNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + MILL_RATE + " S.R." + SALE_RATE + ")  " + brokerShortName;
                //}
            }

            double CGSTRate = txtCGSTRate.Text != string.Empty ? Convert.ToDouble(txtCGSTRate.Text) : 0;
            double CGSTAmount = txtCGSTAmount.Text != string.Empty ? Convert.ToDouble(txtCGSTAmount.Text) : 0;
            double IGSTRate = txtIGSTRate.Text != string.Empty ? Convert.ToDouble(txtIGSTRate.Text) : 0;
            double IGSTAmount = txtIGSTAmount.Text != string.Empty ? Convert.ToDouble(txtIGSTAmount.Text) : 0;
            double SGSTRate = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0;
            double SGSTAmount = txtSGSTAmount.Text != string.Empty ? Convert.ToDouble(txtSGSTAmount.Text) : 0;
            Int32 GstRateCode = txtGSTRateCode.Text != string.Empty ? Convert.ToInt32(txtGSTRateCode.Text) : 0;
            double TaxableAmount = txtTaxableAmount.Text != string.Empty ? Convert.ToDouble(txtTaxableAmount.Text) : 0;
            string NARRATION5 = txtNarration5.Text;
            hdnf.Value = txtdoc_no.Text;
            hdnfSuffix.Value = txtSUFFIX.Text;
            #endregion-End of Head part declearation

            #region ---------- save ----------
            if (isValidated == true)
            {
                csCalculations();

                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    if (ViewState["mode"] != null)
                    {
                        DataSet ds = new DataSet();

                        if (ViewState["mode"].ToString() == "I")
                        {
                            obj.flag = 1;
                            obj.tableName = tblHead;
                            obj.columnNm = " Tran_Type, DOC_NO , SUFFIX , DO_No , DOC_DATE , AC_CODE,Unit_Code, BROKER_CODE ," +
                            " Quantal,PACKING , BAGS ,GRADE , MILL_CODE, MILL_RATE ,Sale_Rate,Purchase_Rate, RESALE_COMMISSON ," +
                            " RDIFFTENDER , POSTAGE , BANK_COMMISSION, FREIGHT , NARRATION1 ,NARRATION2 , NARRATION3 , NARRATION4 ," +
                            " OTHER_EXPENSES, VOUCHER_AMOUNT ,Diff_Amount,Due_Days," +
                            " Company_Code, Year_Code , Branch_Code,Created_By,Commission_Rate,TRANSPORT_CODE," +
                            "CGSTRate,CGSTAmount,SGSTRate,SGSTAmount,IGSTRate,IGSTAmount,GstRateCode,TaxableAmount,Narration5";

                            obj.values = "'" + T_Type + "','" + DOC_NO + "','" + SUFFIX + "','" + DO_No + "','" + DOC_DATE + "','" + AC_CODE + "','" + Unit_Code + "','" + BROKER_CODE + "'," +
                            "'" + Quantal + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_CODE + "','" + MILL_RATE + "','" + SALE_RATE + "','" + PURCHASE_RATE + "','" + RESALE_COMMISSON + "'," +
                            " '" + RDIFFTENDER + "','" + POSTAGE + "','" + BANK_COMMISSION + "','" + FREIGHT + "','" + NARRATION1 + myNarration + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "'," +
                            " '" + OTHER_EXPENSES + "','" + VOUCHER_AMOUNT + "','" + Diff_Amount + "','" + Due_Days + "'," +
                            " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + user + "','"
                            + Commission_Rate + "','" + TRANSPORT_CODE + "','" + CGSTRate + "','" + CGSTAmount + "','" + SGSTRate
                            + "','" + SGSTAmount + "','" + IGSTRate + "','" + IGSTAmount + "','" + GstRateCode + "','" + TaxableAmount + "','"+NARRATION5+"'";
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;

                            //update voucher no to DO
                            //qry = "";
                            //qry = "update " + tblPrefix + "deliveryorder set voucher_no=" + txtdoc_no.Text + " , Tran_Type='" + Tran_Type + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no='" + DO_No + "'";
                            //ds = clsDAL.SimpleQuery(qry);
                        }
                        if (ViewState["mode"].ToString() == "U")
                        {

                            obj.flag = 2;
                            obj.tableName = tblHead;

                            obj.columnNm = "  DO_No='" + DO_No + "' , DOC_DATE='" + DOC_DATE + "' , AC_CODE='" + AC_CODE + "',Unit_Code='" + Unit_Code + "', BROKER_CODE='" + BROKER_CODE + "' ," +
                            " Quantal='" + Quantal + "',PACKING='" + PACKING + "' , BAGS='" + BAGS + "' ,GRADE='" + GRADE + "' , MILL_CODE='" + MILL_CODE + "', MILL_RATE='" + MILL_RATE + "' ,SALE_RATE='" + SALE_RATE + "'," +
                            " PURCHASE_RATE='" + PURCHASE_RATE + "',RESALE_COMMISSON='" + RESALE_COMMISSON + "' ," +
                            " RDIFFTENDER='" + RDIFFTENDER + "' , POSTAGE='" + POSTAGE + "' , BANK_COMMISSION='" + BANK_COMMISSION + "', FREIGHT='" + FREIGHT + "' , NARRATION1='" + NARRATION1 + "'" +
                            " ,NARRATION2='" + NARRATION2 + "' , NARRATION3='" + NARRATION3 + "' , NARRATION4='" + NARRATION4 + "' ," +
                            " OTHER_EXPENSES='" + OTHER_EXPENSES + "', VOUCHER_AMOUNT='" + VOUCHER_AMOUNT + "',Diff_Amount='" + Diff_Amount
                            + "',Due_Days='" + Due_Days + "',Modified_By='" + user + "',Commission_Rate='" + Commission_Rate + "',TRANSPORT_CODE='"
                            + TRANSPORT_CODE + "',CGSTRate='" + CGSTRate + "',CGSTAmount='" + CGSTAmount + "',SGSTRate='" + SGSTRate + "',SGSTAmount='"
                            + SGSTAmount + "',IGSTRate='" + IGSTRate + "',IGSTAmount='" + IGSTAmount + "',GstRateCode='" + GstRateCode + "',TaxableAmount='"
                            + TaxableAmount + "',Narration5='"+NARRATION5+"' where " +
                            " Company_Code='" + Company_Code + "' and  Year_Code='" + Year_Code + "'  and DOC_NO='" + DOC_NO + "'" +
                            " and SUFFIX='" + txtSUFFIX.Text.Trim() + "' and Tran_Type='" + T_Type + "'";

                            obj.values = "none";
                            ds = obj.insertAccountMaster(ref strRev);

                            retValue = strRev;

                            //update voucher no to DO
                            //qry = "";
                            //qry = "update " + tblPrefix + "deliveryorder set voucher_no=" + txtdoc_no.Text + " , Tran_Type='" + Tran_Type + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no='" + DONO + "'";
                            //ds = clsDAL.SimpleQuery(qry);
                        }

                        #region ---------------  GLedger Effect  --------------------

                        qry = "";
                        qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + DOC_NO + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);
                        Int32 GID = 0;

                        if (VOUCHER_AMOUNT > 0)
                        {
                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + AC_CODE + "','" + partyNarration + "','" + Math.Abs(VOUCHER_AMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                        }
                        else
                        {
                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + AC_CODE + "','" + partyNarration + "','" + Math.Abs(VOUCHER_AMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','C','" + 0 + "','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                        }
                        // diffrance amount effect
                        if (VOUCHER_AMOUNT > 0)
                        {
                            //------------Credit effect
                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "','" + Math.Abs(VOUCHER_AMOUNT - RESALE_COMMISSON) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                        }
                        else
                        {
                            //------------Debit effect
                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "','" + Math.Abs(VOUCHER_AMOUNT - RESALE_COMMISSON) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);

                        }

                        //other accounting effects.............
                        string drcr = "";
                        double amt = 0.00;

                        //if (RDIFFTENDER != 0)
                        //{
                        //    drcr = "";
                        //    amt = RDIFFTENDER;
                        //    if (RDIFFTENDER > 0)
                        //    {
                        //        drcr = "C";
                        //    }
                        //    else
                        //    {
                        //        drcr = "D";
                        //        amt = 0 - amt;
                        //    }
                        //    GID = GID + 1;
                        //    obj.flag = 1;
                        //    obj.tableName = GLedgerTable;
                        //    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                        //    obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + Session["QUALITY_DIFF_AC"].ToString() + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + AC_CODE + "','0','" + Branch_Code + "'";
                        //    ds = obj.insertAccountMaster(ref strRev);


                        //    if (RDIFFTENDER > 0)
                        //    {
                        //        drcr = "D";
                        //    }
                        //    else
                        //    {
                        //        drcr = "C";
                        //        amt = 0 - amt;
                        //    }
                        //    GID = GID + 1;
                        //    obj.flag = 1;
                        //    obj.tableName = GLedgerTable;
                        //    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                        //    obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + AC_CODE + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + Session["QUALITY_DIFF_AC"].ToString() + "','0','" + Branch_Code + "'";
                        //    ds = obj.insertAccountMaster(ref strRev);
                        //}

                        if (POSTAGE != 0)
                        {
                            drcr = "";
                            amt = POSTAGE;
                            if (POSTAGE > 0)
                            {
                                drcr = "C";
                            }
                            else
                            {
                                drcr = "D";
                                amt = 0 - amt;
                            }

                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + Session["POSTAGE_AC"].ToString() + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + AC_CODE + "','0','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                        }

                        if (RESALE_COMMISSON != 0)
                        {
                            drcr = "";
                            amt = RESALE_COMMISSON;
                            if (RESALE_COMMISSON > 0)
                            {
                                drcr = "C";
                            }
                            else
                            {
                                drcr = "D";
                                amt = 0 - amt;
                            }

                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + Session["COMMISSION_AC"].ToString() + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + AC_CODE + "','0','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);

                        }
                        if (BANK_COMMISSION != 0)
                        {
                            drcr = "";
                            amt = BANK_COMMISSION;
                            if (BANK_COMMISSION > 0)
                            {
                                drcr = "C";
                            }
                            else
                            {
                                drcr = "D";
                                amt = 0 - amt;
                            }

                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + Session["BANK_COMMISSION_AC"].ToString() + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + AC_CODE + "','0','" + Branch_Code + "'";

                            ds = obj.insertAccountMaster(ref strRev);

                        }
                        if (FREIGHT != 0)
                        {
                            drcr = "";
                            amt = FREIGHT;
                            if (FREIGHT > 0)
                            {
                                drcr = "C";
                            }
                            else
                            {
                                drcr = "D";
                                amt = 0 - amt;
                            }

                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + TRANSPORT_CODE + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + AC_CODE + "','0','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);

                            if (FREIGHT > 0)
                            {
                                drcr = "D";
                            }
                            else
                            {
                                drcr = "C";
                                amt = 0 - amt;
                            }
                            //to account legder
                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + AC_CODE + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + TRANSPORT_CODE + "','0','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                        }

                        if (OTHER_EXPENSES != 0)
                        {
                            drcr = "";
                            amt = OTHER_EXPENSES;
                            if (OTHER_EXPENSES > 0)
                            {
                                drcr = "C";
                            }
                            else
                            {
                                drcr = "D";
                                amt = 0 - amt;
                            }

                            GID = GID + 1;
                            obj.flag = 1;
                            obj.tableName = GLedgerTable;
                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                            obj.values = "'" + Tran_Type + "','" + DOC_NO + "','" + DOC_DATE + "','" + Session["OTHER_AMOUNT_AC"].ToString() + "','" + NARRATION1 + myNarration + "','" + amt + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + drcr + "','" + AC_CODE + "','0','" + Branch_Code + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                        }


                        #endregion

                        if (retValue == "-1")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                            hdnf.Value = txtdoc_no.Text;
                            hdnfSuffix.Value = txtSUFFIX.Text.Trim();
                            qry = getDisplayQuery();
                            this.fetchRecord(qry);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added !');", true);
                        }
                        if (retValue == "-2" || retValue == "-3")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
                            hdnf.Value = txtdoc_no.Text;
                            hdnfSuffix.Value = txtSUFFIX.Text.Trim();
                            qry = getDisplayQuery();
                            this.fetchRecord(qry);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                        }
                    }
                }
            }
            txtEditDoc_No.Text = string.Empty;
            #endregion
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
            if (strTextBox == "txtAC_CODE")
            {
                string partyName = string.Empty;
                if (txtAC_CODE.Text != string.Empty)
                {
                    searchString = txtAC_CODE.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btntxtAC_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            lblAc_name.Text = partyName;
                            setFocusControl(txtUnit_Code);
                        }
                        else
                        {
                            lblAc_name.Text = string.Empty;
                            txtAC_CODE.Text = string.Empty;
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
                    if (!clsCommon.isStringIsNumeric(txtUnit_Code.Text))
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
                                setFocusControl(txtBroker_CODE);
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
                                setFocusControl(txtBroker_CODE);
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
            if (strTextBox == "txtTRANSPORT_CODE")
            {
                string qry = string.Empty;
                string partyName = string.Empty;
                if (txtTRANSPORT_CODE.Text != string.Empty)
                {
                    searchString = txtTRANSPORT_CODE.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btntxtTRANSPORT_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        partyName = clsCommon.getString(qry);

                        if (partyName != string.Empty)
                        {
                            LBLTRANSPORT_NAME.Text = partyName;
                            setFocusControl(txtOTHER_Expenses);
                        }
                        else
                        {
                            LBLTRANSPORT_NAME.Text = string.Empty;
                            txtTRANSPORT_CODE.Text = string.Empty;
                            setFocusControl(txtTRANSPORT_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtTRANSPORT_CODE);
                }
            }
            if (strTextBox == "txtBroker_CODE")
            {
                string brokername = string.Empty;
                if (txtBroker_CODE.Text != string.Empty)
                {
                    searchString = txtBroker_CODE.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btntxtBroker_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBroker_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        brokername = clsCommon.getString(qry);

                        if (brokername != string.Empty)
                        {
                            lblBroker_name.Text = brokername;
                            setFocusControl(txtQNTL);
                        }
                        else
                        {
                            lblBroker_name.Text = string.Empty;
                            txtBroker_CODE.Text = string.Empty;
                            setFocusControl(txtBroker_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBroker_CODE);
                }
            }

            if (strTextBox == "txtMILL_CODE")
            {
                string millName = string.Empty;

                if (txtMILL_CODE.Text != string.Empty)
                {
                    searchString = txtMILL_CODE.Text;
                    if (!clsCommon.isStringIsNumeric(searchString))
                    {
                        btntxtMILL_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        string qry = "";
                        qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='M'";
                        millName = clsCommon.getString(qry);
                        //get shortname
                        qry = "select Short_Name from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='M'";
                        millShortName = clsCommon.getString(qry);

                        if (millName != string.Empty)
                        {
                            lblMill_name.Text = millName;

                            setFocusControl(txtMILL_RATE);
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

            if (strTextBox == "txtdoc_no" || strTextBox == "txtSUFFIX")
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

                            string qry = "select * from " + tblHead + " where Tran_Type='" + Tran_Type + "' and  Doc_No='" + txtValue + "' " +
                                " and Suffix='" + txtSUFFIX.Text.Trim() + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and " +
                                " Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' ";
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
                                                //this.getMaxCode();
                                                txtdoc_no.Enabled = false;
                                                btnSave.Enabled = true;   //IMP
                                                txtSUFFIX.Text = string.Empty;
                                                setFocusControl(txtSUFFIX);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Suffix='" + hdnfSuffix.Value + "'" +
                                                   " and Tran_Type='" + Tran_Type + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                                                  " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtdoc_no.Enabled = false;
                                                    setFocusControl(txtDONO);
                                                    hdnf.Value = txtdoc_no.Text;
                                                    hdnfSuffix.Value = txtSUFFIX.Text.Trim();
                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtDONO);
                                            txtdoc_no.Enabled = false;
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
            }
            if (strTextBox == "txtDONO")
            {
                if (txtDONO.Text != string.Empty)
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    string qry = "";
                    qry = "select * from " + tblPrefix + "qryDeliveryOrderList where doc_no=" + txtDONO.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and voucher_no=0 and tran_type='DO'";
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                txtAC_CODE.Text = dt.Rows[0]["Ac_Code"].ToString();
                                lblAc_name.Text = dt.Rows[0]["PartyName"].ToString();
                                txtBroker_CODE.Text = dt.Rows[0]["broker"].ToString();
                                lblBroker_name.Text = dt.Rows[0]["BrokerName"].ToString();
                                txtQNTL.Text = dt.Rows[0]["quantal"].ToString();
                                txtPACKING.Text = dt.Rows[0]["packing"].ToString();
                                txtBAGS.Text = dt.Rows[0]["bags"].ToString();
                                txtGRADE.Text = dt.Rows[0]["grade"].ToString();
                                txtMILL_CODE.Text = dt.Rows[0]["mill_code"].ToString();
                                lblMill_name.Text = dt.Rows[0]["millName"].ToString();
                                txtMILL_RATE.Text = dt.Rows[0]["mill_rate"].ToString();
                                txtSALE_RATE.Text = dt.Rows[0]["sale_rate"].ToString();

                                setFocusControl(txtDOC_DATE);
                            }
                            else
                            {
                                setFocusControl(txtDONO);
                            }
                        }
                        else
                        {
                            setFocusControl(txtDONO);
                        }
                    }
                    else
                    {
                        setFocusControl(txtDONO);
                    }
                }
                else
                {
                    setFocusControl(txtDONO);
                }
            }

            if (strTextBox == "txtDOC_DATE")
            {
                setFocusControl(txtAC_CODE);
            }
            if (strTextBox == "txtQNTL")
            {
                txtPACKING.Text = "50";
                setFocusControl(txtPACKING);
            }
            if (strTextBox == "txtPACKING")
            {
                setFocusControl(txtGRADE);
            }
            if (strTextBox == "txtGRADE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (strTextBox == "txtMILL_RATE")
            {
                setFocusControl(txtSALE_RATE);
            }
            if (strTextBox == "txtSALE_RATE")
            {
                setFocusControl(txtPURCHASE_RATE);
            }
            if (strTextBox == "txtPURCHASE_RATE")
            {
                setFocusControl(txtRDiffTender);
            }

            if (strTextBox == "txtRDiffTender")
            {
                setFocusControl(txtPostage);
            }
            if (strTextBox == "txtPostage")
            {
                setFocusControl(txtCommissionPerQntl);
            }
            if (strTextBox == "txtCommissionPerQntl")
            {
                setFocusControl(txtResale_Commisson);
            }
            if (strTextBox == "txtResale_Commisson")
            {
                setFocusControl(txtBANK_COMMISSION);
            }
            if (strTextBox == "txtBANK_COMMISSION")
            {
                setFocusControl(txtFREIGHT);
            }
            if (strTextBox == "txtFREIGHT")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (strTextBox == "txtTRANSPORT_CODE")
            {
                setFocusControl(txtOTHER_Expenses);
            }
            if (strTextBox == "txtNarration1")
            {
                setFocusControl(txtNarration2);
            }
            if (strTextBox == "txtNarration2")
            {
                setFocusControl(txtNarration3);
            }
            if (strTextBox == "txtNarration3")
            {
                setFocusControl(txtNarration4);
            }
            if (strTextBox == "txtNarration4")
            {
                setFocusControl(btnSave);
            }
            if (strTextBox == "txtDueDays")
            {
                setFocusControl(txtNarration1);
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
                            setFocusControl(txtRDiffTender);
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
                    setFocusControl(txtGSTRateCode);
                }
            }

            #region [Calculation Part]

            double qtl = Convert.ToDouble("0" + txtQNTL.Text);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
            double bags = 0.00;
            double saleRate = Convert.ToDouble("0" + txtSALE_RATE.Text);
            double millRate = Convert.ToDouble("0" + txtMILL_RATE.Text);
            double purcRate = Convert.ToDouble("0" + txtPURCHASE_RATE.Text);
            double rDiffTender = Convert.ToDouble("0" + txtRDiffTender.Text);
            double postage = Convert.ToDouble("0" + txtPostage.Text);
            double resale_comm = Convert.ToDouble("0" + txtResale_Commisson.Text);
            double bank_comm = Convert.ToDouble("0" + txtBANK_COMMISSION.Text);
            double freight = Convert.ToDouble("0" + txtFREIGHT.Text);
            double other_expense = Convert.ToDouble("0" + txtOTHER_Expenses.Text);
            double voucher_Amt = 0.00;
            double diffAmt = 0.00;
            double diff = 0.00;

            if (qtl != 0 && packing != 0)
            {
                bags = (100 / packing) * qtl;
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }

            if (saleRate == 0 && purcRate == 0)
            {
                lblDiff.Text = "0";
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
            lblDiff.Text = diffAmt.ToString();

            voucher_Amt = Math.Round(diffAmt + rDiffTender + postage + resale_comm + bank_comm + freight + other_expense, 2);
            txtVoucher_Amount.Text = voucher_Amt.ToString();

            string aaa = "";
            if (txtAC_CODE.Text.Trim() != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
                if (a == true)
                {
                    aaa = clsCommon.getString("select ISNULL(GSTStateCode,0) from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAC_CODE.Text + "");
                }
            }
            int partygstStateCode = 0;
            if (aaa.Trim().ToString() != "")
            {
                partygstStateCode = Convert.ToInt32(aaa);
            }

            int companyGstStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
            string GSTRateCode = txtGSTRateCode.Text;
            double GSTRate = Convert.ToDouble(clsCommon.getString("select Rate from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            double cgstrate = Convert.ToDouble(clsCommon.getString("select CGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            double sgstrate = Convert.ToDouble(clsCommon.getString("select SGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            double igstrate = Convert.ToDouble(clsCommon.getString("select IGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));

            double CGSTAmountForPS = 0.0;
            double SGSTAmountForPS = 0.0;
            double IGSTAmountForPS = 0.0;

            double CGSTRateForPS = 0.00;
            double SGSTRateForPS = 0.00;
            double IGSTRateForPS = 0.00;

            if (companyGstStateCode == partygstStateCode)
            {
                CGSTRateForPS = cgstrate;
                double cgsttaxAmountOnMR = Math.Round((diffAmt * cgstrate / 100), 2);
                //double cgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + cgsttaxAmountOnMR) * mill_rate)), 2);
                //double cgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - cgstExMillRate), 2);
                CGSTAmountForPS = Math.Round(cgsttaxAmountOnMR, 2);

                SGSTRateForPS = sgstrate;
                double sgsttaxAmountOnMR = Math.Round((diffAmt * sgstrate / 100), 2);
                //double sgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + sgsttaxAmountOnMR) * mill_rate)), 2);
                //double sgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - sgstExMillRate), 2);
                SGSTAmountForPS = Math.Round(sgsttaxAmountOnMR, 2);
            }
            else
            {
                IGSTRateForPS = igstrate;
                double igsttaxAmountOnMR = ((diffAmt) * igstrate / 100);
                //double igstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + igsttaxAmountOnMR) * mill_rate)), 2);
                //double igstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - igstExMillRate), 2);
                IGSTAmountForPS = Math.Round(igsttaxAmountOnMR, 2);
            }

            txtCGSTRate.Text = CGSTRateForPS.ToString();
            txtCGSTAmount.Text = CGSTAmountForPS.ToString();
            txtSGSTRate.Text = SGSTRateForPS.ToString();
            txtSGSTAmount.Text = SGSTAmountForPS.ToString();
            txtIGSTRate.Text = IGSTRateForPS.ToString();
            txtIGSTAmount.Text = IGSTAmountForPS.ToString();

            txtTaxableAmount.Text = diffAmt.ToString();
            voucher_Amt = Math.Round(diffAmt + rDiffTender + postage + resale_comm + bank_comm + freight + other_expense + CGSTAmountForPS + SGSTAmountForPS + IGSTAmountForPS, 2);
            txtVoucher_Amount.Text = voucher_Amt.ToString();
            #endregion
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ks", "javascript:p('" + txtdoc_no.Text + "','" + "LV" + "')", true);
    }
    protected void txtDueDays_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtDueDays";
        csCalculations();
    }
    protected void txtCommissionPerQntl_TextChanged(object sender, EventArgs e)
    {
        double commision = txtCommissionPerQntl.Text != string.Empty ? Convert.ToDouble(txtCommissionPerQntl.Text) : 0;
        double commamt = commision * Convert.ToDouble(txtQNTL.Text);
        txtResale_Commisson.Text = Convert.ToString(commamt);
        strTextBox = "txtCommissionPerQntl";
        csCalculations();
    }


    protected void txtCGSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGSTRate.Text;
        strTextBox = "txtCGSTRate";
        csCalculations();

    }


    protected void txtSGSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGSTRate.Text;
        strTextBox = "txtSGSTRate";
        csCalculations();

    }
    protected void txtIGSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGSTRate.Text;
        strTextBox = "txtIGSTRate";
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

    #region [txtNarration1_TextChanged]
    protected void txtNarration5_TextChanged(object sender, EventArgs e)
    {
        
    }
    #endregion

}
