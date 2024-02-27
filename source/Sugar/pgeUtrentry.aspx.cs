using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
public partial class Sugar_pgeUtrentry : System.Web.UI.Page
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
    string GLedgerTable = string.Empty;
    string trnType = "UT";
    int defaultAccountCode = 0;
    string qryAccountList = string.Empty;
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

            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "UTR";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = tblPrefix + "qryUTRList";
            qryAccountList = tblPrefix + "qryAccountsList";
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
                obj.tableName = tblHead + "  where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "Doc_no";
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
                                    txtDoc_no.Enabled = false;
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                chkIsSave.Enabled = false;
                #region set Business logic for save
                calenderExtenderDate.Enabled = false;
                btntxtbank_ac.Enabled = false;
                btntxtmill_code.Enabled = false;
                lblBank_name.Text = string.Empty;
                lblMill_name.Text = string.Empty;

                btnSendSMS.Enabled = true;
                btnUtrReport.Enabled = true;
                rbEditMobile.Enabled = true;
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
                chkIsSave.Enabled = true;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Change No";
                btntxtdoc_no.Enabled = true;
                #region set Business logic for save
                calenderExtenderDate.Enabled = true;
                btntxtbank_ac.Enabled = true;
                btntxtmill_code.Enabled = true;
                lblBank_name.Text = string.Empty;
                lblMill_name.Text = string.Empty;
                txtdoc_date.Text = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                txtMillMobile.Enabled = false;
                btnSendSMS.Enabled = false;
                btnUtrReport.Enabled = false;
                rbEditMobile.Enabled = false;
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
                chkIsSave.Enabled = false;
                #region set Business logic for save
                calenderExtenderDate.Enabled = false;
                btntxtbank_ac.Enabled = false;
                btntxtmill_code.Enabled = false;
                txtMillMobile.Enabled = false;
                btnSendSMS.Enabled = true;
                btnUtrReport.Enabled = true;
                rbEditMobile.Enabled = true;
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
                chkIsSave.Enabled = true;
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                #region set Business logic for save
                calenderExtenderDate.Enabled = true;
                btntxtbank_ac.Enabled = true;
                btntxtmill_code.Enabled = true;
                txtMillMobile.Enabled = false;
                btnSendSMS.Enabled = false;
                btnUtrReport.Enabled = false;
                rbEditMobile.Enabled = false;
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
        query = "select count(*) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
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
        if (txtDoc_no.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY doc_no asc  ";
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  ORDER BY doc_no asc  ";
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
            if (txtDoc_no.Text != string.Empty)
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
            if (txtDoc_no.Text != string.Empty)
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
        setFocusControl(txtbank_ac);
        txtDoc_no.Enabled = false;
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        this.makeEmptyForm("E");
        txtDoc_no.Enabled = true;
        setFocusControl(txtdoc_date);
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
                string currentDoc_No = txtDoc_no.Text;
                string isPresent = clsCommon.getString("Select UTR_NO from " + tblPrefix + "qryDeliveryOrderList where UTR_NO=" + currentDoc_No + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (string.IsNullOrWhiteSpace(isPresent))
                {

                    string qry = "";
                    qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + trnType + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(qry);


                    string strrev = "";
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        obj.flag = 3;
                        obj.tableName = tblHead;
                        obj.columnNm = "   Doc_No=" + currentDoc_No +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                        obj.values = "none";
                        ds = obj.insertAccountMaster(ref strrev);
                        if (strrev == "-3")
                        {
                            obj.flag = 3;
                            obj.tableName = tblDetails;
                            obj.columnNm = "   Doc_No=" + currentDoc_No + " '" +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                            obj.values = "none";
                            ds = obj.insertAccountMaster(ref strrev);
                        }
                    }
                    string query = "";

                    if (strrev == "-3")
                    {
                        query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                               "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                                " ORDER BY Doc_No asc  ";


                        hdnf.Value = clsCommon.getString(query);

                        if (hdnf.Value == string.Empty)
                        {
                            query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                                 "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
                }
                else
                {
                    lblMsg.Text = "You Cannot delete this Utr.Used in DO";
                }

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
        this.enableDisableNavigateButtons();
        this.makeEmptyForm("S");
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
                        txtdoc_date.Text = dt.Rows[0]["DOC_DATE"].ToString();
                        txtbank_ac.Text = dt.Rows[0]["BANK_AC"].ToString();
                        lblBank_name.Text = dt.Rows[0]["bankname"].ToString();
                        txtmill_code.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        txtMillMobile.Text = clsCommon.getString("select Mobile_No from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtmill_code.Text + "");
                        lblMill_name.Text = dt.Rows[0]["millname"].ToString();
                        txtamount.Text = dt.Rows[0]["AMOUNT"].ToString();
                        txtutr_no.Text = dt.Rows[0]["UTR_NO"].ToString();
                        txtnarration_header.Text = dt.Rows[0]["NARRATION_HEADER"].ToString();
                        txtnarration_footer.Text = dt.Rows[0]["NARRATION_FOOTER"].ToString();
                        string IsSave = dt.Rows[0]["IsSave"].ToString();
                        if (IsSave == "1")
                        {
                            chkIsSave.Checked = true;
                        }
                        else
                        {
                            chkIsSave.Checked = false;
                        }
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

                    }
                }
            }
            hdnf.Value = txtDoc_no.Text;
            //  this.enableDisableNavigateButtons();
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
            string qryDisplay = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                txtDoc_no.Text = hdnf.Value;
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
            e.Row.Cells[0].Width = new Unit("80px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
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

    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDoc_no.Text;
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

    #region [txtdoc_date_TextChanged]
    protected void txtdoc_date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_date.Text;
        strTextBox = "txtdoc_date";
        csCalculations();
    }
    #endregion

    #region [btntxtdoc_date_Click]
    protected void btntxtdoc_date_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtdoc_date";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtbank_ac_TextChanged]
    protected void txtbank_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtbank_ac.Text;
        strTextBox = "txtbank_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtbank_ac_Click]
    protected void btntxtbank_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtbank_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtmill_code_TextChanged]
    protected void txtmill_code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmill_code.Text;
        strTextBox = "txtmill_code";
        csCalculations();
    }
    #endregion

    #region [btntxtmill_code_Click]
    protected void btntxtmill_code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmill_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtamount_TextChanged]
    protected void txtamount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtamount.Text;
        strTextBox = "txtamount";
        csCalculations();
    }
    #endregion

    #region [txtutr_no_TextChanged]
    protected void txtutr_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtutr_no.Text;
        strTextBox = "txtutr_no";
        csCalculations();
    }
    #endregion

    #region [txtnarration_header_TextChanged]
    protected void txtnarration_header_TextChanged(object sender, EventArgs e)
    {
        searchString = txtnarration_header.Text;
        strTextBox = "txtnarration_header";
        csCalculations();
    }
    #endregion

    #region [txtnarration_footer_TextChanged]
    protected void txtnarration_footer_TextChanged(object sender, EventArgs e)
    {
        searchString = txtnarration_footer.Text;
        strTextBox = "txtnarration_footer";
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
            //else
            //{
            //    txtSearchText.Text = string.Empty;
            //}

            if (hdnfClosePopup.Value == "txtdoc_no")
            {
                lblPopupHead.Text = "--Select DOC NO--";
                string qry = "select doc_no,doc_date,utr_no,amount,millnameshort,banknameshort from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or utr_no like '%" + txtSearchText.Text + "%' or millnameshort like '%" + txtSearchText.Text + "%' or banknameshort like '%" + txtSearchText.Text + "%' or amount like  '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtbank_ac")
            {
                lblPopupHead.Text = "--Select Bank--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Ac_type='B' and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%')  order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtmill_code")
            {
                lblPopupHead.Text = "--Select--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                   " and Ac_type IN ('M','P','T') and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
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
            if (hdnfClosePopup.Value == "txtbank_ac")
            {
                setFocusControl(txtbank_ac);
            }
            if (hdnfClosePopup.Value == "txtmill_code")
            {
                setFocusControl(txtmill_code);
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
        #region [Validation Part]
        bool isValidated = true;
        if (txtDoc_no.Text != string.Empty)
        {

            if (ViewState["mode"] != null)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtDoc_no.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                    if (str != string.Empty)
                    {
                        lblMsg.Text = "Code " + txtDoc_no.Text + " already exist";
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
            setFocusControl(txtDoc_no);
            return;
        }
        if (txtdoc_date.Text != string.Empty)
        {
            try
            {
                string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                if (clsCommon.isValidDate(dt) == true)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    txtdoc_date.Text = string.Empty;
                    setFocusControl(txtdoc_date);
                    return;
                }
            }
            catch
            {
                isValidated = false;
                txtdoc_date.Text = string.Empty;
                setFocusControl(txtdoc_date);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtdoc_date);
            return;
        }
        if (txtbank_ac.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbank_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtbank_ac);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtbank_ac);
            return;
        }
        if (txtmill_code.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmill_code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type IN ('M','P','T')");
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtmill_code);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtmill_code);
            return;
        }
        if (txtamount.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtamount);
            return;
        }
        if (txtutr_no.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtutr_no);
            return;
        }

        #endregion

        #region -Head part declearation
        Int32 DOC_NO = txtDoc_no.Text != string.Empty ? Convert.ToInt32(txtDoc_no.Text) : 0;
        string DOC_DATE = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        Int32 BANK_AC = txtbank_ac.Text != string.Empty ? Convert.ToInt32(txtbank_ac.Text) : 0;
        Int32 MILL_CODE = txtmill_code.Text != string.Empty ? Convert.ToInt32(txtmill_code.Text) : 0;
        double AMOUNT = txtamount.Text != string.Empty ? Convert.ToDouble(txtamount.Text) : 0.00;
        string UTR_NO = txtutr_no.Text;
        string NARRATION_HEADER = txtnarration_header.Text;
        string NARRATION_FOOTER = txtnarration_footer.Text;
        int IsSave = 0;
        if (chkIsSave.Checked == true)
        {
            IsSave = 1;
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
                if (ViewState["mode"].ToString() == "I")
                {
                    obj.flag = 1;
                    obj.tableName = tblHead;
                    obj.columnNm = "DOC_NO,DOC_DATE,BANK_AC,MILL_CODE,AMOUNT,UTR_NO,NARRATION_HEADER,NARRATION_FOOTER,Company_Code,Year_Code,Branch_Code,Created_By,IsSave";
                    obj.values = "'" + DOC_NO + "','" + DOC_DATE + "','" + BANK_AC + "','" + MILL_CODE + "','" + AMOUNT + "','" + UTR_NO + "','" + NARRATION_HEADER + "','" + NARRATION_FOOTER + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'," + IsSave + "";
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }
                else
                {
                    //Update Mode
                    obj.flag = 2;
                    obj.tableName = tblHead;
                    obj.columnNm = "DOC_DATE='" + DOC_DATE + "',BANK_AC='" + BANK_AC + "',MILL_CODE='" + MILL_CODE + "',AMOUNT='" + AMOUNT + "',UTR_NO='" + UTR_NO + "',NARRATION_HEADER='" + NARRATION_HEADER + "',NARRATION_FOOTER='" + NARRATION_FOOTER + "',Modified_By='" + user + "',IsSave=" + IsSave + " where doc_no='" + txtDoc_no.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                    obj.values = "none";
                    ds = new DataSet();
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }
                string millShortName = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + MILL_CODE + "");
                #region Gledger Effect
                Int32 GID = 0;
                string qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + trnType + "' and DOC_NO=" + DOC_NO + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                GID = GID + 1;
                obj.flag = 1;
                obj.tableName = GLedgerTable;
                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                obj.values = "'" + trnType + "','" + DOC_NO + "','" + DOC_DATE + "','" + MILL_CODE + "','" + "UTR No:" + txtutr_no.Text.Trim() + " " + NARRATION_HEADER + " " + millShortName + "','" + AMOUNT + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','D','" + BANK_AC + "','0','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                ds = obj.insertAccountMaster(ref strRev);

                GID = GID + 1;
                obj.flag = 1;
                obj.tableName = GLedgerTable;
                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                obj.values = "'" + trnType + "','" + DOC_NO + "','" + DOC_DATE + "','" + BANK_AC + "','" + "UTR No:" + txtutr_no.Text.Trim() + " " + NARRATION_HEADER + " " + millShortName + "','" + AMOUNT + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','C','" + MILL_CODE + "','0','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                ds = obj.insertAccountMaster(ref strRev);
                #endregion

                if (retValue == "-1")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
                    hdnf.Value = txtDoc_no.Text;
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);
                }
                if (retValue == "-2" || retValue == "-3")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
                    hdnf.Value = txtDoc_no.Text;
                    qry = getDisplayQuery();
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
            if (strTextBox == "txtdoc_no")
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

                            string qry = "select * from " + tblHead + " where   Doc_No='" + txtValue + "' " +
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
                                                txtDoc_no.Enabled = false;
                                                hdnf.Value = txtDoc_no.Text;
                                                btnSave.Enabled = true;   //IMP
                                                setFocusControl(txtdoc_date);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtDoc_no.Enabled = false;
                                                }
                                                setFocusControl(txtdoc_date);
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtdoc_date);
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
                            setFocusControl(txtDoc_no);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Doc No is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtDoc_no.Text = string.Empty;
                        setFocusControl(txtDoc_no);
                    }
                }
                catch
                {

                }
                #endregion
            }
            if (strTextBox == "txtdoc_date")
            {
                if (txtdoc_date.Text != string.Empty)
                {
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtbank_ac);
                    }
                    else
                    {
                        txtdoc_date.Text = string.Empty;
                        setFocusControl(txtdoc_date);
                    }
                }
                else
                {
                    setFocusControl(txtdoc_date);
                }
            }
            if (strTextBox == "txtbank_ac")
            {
                if (txtbank_ac.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtbank_ac.Text);
                    if (a == false)
                    {
                        btntxtbank_ac_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_type='B' and Ac_Code=" + txtbank_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblBank_name.Text = str;
                            setFocusControl(txtmill_code);
                        }
                        else
                        {
                            lblBank_name.Text = str;
                            txtbank_ac.Text = string.Empty;
                            setFocusControl(txtbank_ac);

                        }
                    }
                }
                else
                {
                    lblBank_name.Text = string.Empty;
                    txtbank_ac.Text = string.Empty;
                    setFocusControl(txtbank_ac);
                }
            }
            if (strTextBox == "txtmill_code")
            {
                if (txtmill_code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtmill_code.Text);
                    if (a == false)
                    {
                        btntxtmill_code_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmill_code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type IN ('M','P','T')");
                        if (str != string.Empty)
                        {
                            lblMill_name.Text = str;
                            setFocusControl(txtamount);
                        }
                        else
                        {
                            lblMill_name.Text = str;
                            txtmill_code.Text = string.Empty;
                            setFocusControl(txtmill_code);

                        }
                    }
                }
                else
                {
                    lblMill_name.Text = string.Empty;
                    txtmill_code.Text = string.Empty;
                    setFocusControl(txtmill_code);
                }
            }
            if (strTextBox == "txtamount")
            {
                setFocusControl(txtutr_no);
            }
            if (strTextBox == "txtutr_no")
            {
                setFocusControl(txtnarration_header);
            }
            if (strTextBox == "txtnarration_header")
            {
                setFocusControl(txtnarration_footer);
            }
            if (strTextBox == "txtnarration_footer")
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
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            string API = clsGV.msgAPI;
            string mobile = txtMillMobile.Text.Trim();
            string companyCity = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            string msg = "From," + Session["Company_Name"].ToString() + "," + companyCity + " Rs." + txtamount.Text + " And Ref.No/UTR No.:" + txtutr_no.Text;
            string URL = API + "mobile=" + mobile + "&message=" + msg;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            StreamReader reader = new StreamReader(myResp.GetResponseStream());
            string str = reader.ReadToEnd();
            reader.Close();
            myResp.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
