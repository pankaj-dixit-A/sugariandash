using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Sugar_pgeJournalvoucher : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string systemMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string trntype = "JV";
    string qryAccountList = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string GLedgerTable = string.Empty;
    string user = string.Empty;
    static WebControl objAsp = null;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "Transact";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = tblPrefix + "qryTransactList";
            qryAccountList = tblPrefix + "qryAccountsList";
            cityMasterTable = tblPrefix + "CityMaster";
            systemMasterTable = tblPrefix + "SystemMaster";
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
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "and Tran_Type='" + trntype + "'";
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
                btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                calenderExtenderDate.Enabled = false;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
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
                txtdoc_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                #region set Business logic for save
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                calenderExtenderDate.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                pnlgrdDetail.Enabled = false;
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
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;
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
            qry = "select max(doc_no) as doc_no from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "' ORDER BY doc_no asc  ";
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "' ORDER BY doc_no asc  ";
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
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "')  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
            if (txtdoc_no.Text != string.Empty)
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "')  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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

                DataSet ds = new DataSet();
                string qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + trntype + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                string strrev = "";
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 3;
                    obj.tableName = tblHead;
                    obj.columnNm = "   Doc_No=" + currentDoc_No +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);
                    if (strrev == "-3")
                    {
                        obj.flag = 3;
                        obj.tableName = GLedgerTable;
                        obj.columnNm = "   Doc_No=" + currentDoc_No +
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";

                        obj.values = "none";
                        ds = obj.insertAccountMaster(ref strrev);
                    }
                }
                string query = "";

                if (strrev == "-3")
                {
                    query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                           "   and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='" + trntype + "'" +
                            " ORDER BY Doc_No asc  ";


                    hdnf.Value = clsCommon.getString(query);

                    if (hdnf.Value == string.Empty)
                    {
                        query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                             "  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='" + trntype + "'" +
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
        string str = clsCommon.getString("select count(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'");
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
                        txtdoc_date.Text = dt.Rows[0]["DOC_DATE"].ToString();
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
                        #region ---------- Details -------------
                        qry = "select detail_id as ID,debit_ac,debitAcName,credit_ac,creditAcName,amount,narration from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
                                        dt.Rows[i]["rowAction"] = "U";
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
                }
            }
            //this.enableDisableNavigateButtons();
            hdnf.Value = txtdoc_no.Text;
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
            string qryDisplay = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
        lblNo.Text = string.Empty;
        lblID.Text = string.Empty;
        setFocusControl(txtdebit_ac);
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

                        // rowIndex = dt.Rows.Count + 1;
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
                        string id = clsCommon.getString("select detail_id from " + tblHead + " where doc_no=" + txtdoc_no.Text + " and detail_id=" + rowIndex + " And Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
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

                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("debit_ac", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("debitAcName", typeof(string))));
                    dt.Columns.Add((new DataColumn("credit_ac", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("creditAcName", typeof(string))));
                    dt.Columns.Add((new DataColumn("amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("narration", typeof(string))));
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
                dt.Columns.Add((new DataColumn("debit_ac", typeof(Int32))));
                dt.Columns.Add((new DataColumn("debitAcName", typeof(string))));
                dt.Columns.Add((new DataColumn("credit_ac", typeof(Int32))));
                dt.Columns.Add((new DataColumn("creditAcName", typeof(string))));
                dt.Columns.Add((new DataColumn("amount", typeof(double))));
                dt.Columns.Add((new DataColumn("narration", typeof(string))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            if (txtdebit_ac.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtdebit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    dr["debit_ac"] = txtdebit_ac.Text;
                    dr["debitAcName"] = str;
                }
                else
                {
                    lblDebit_name.Text = string.Empty;
                    txtdebit_ac.Text = string.Empty;
                    setFocusControl(txtdebit_ac);
                    return;
                }
            }
            else
            {
                lblDebit_name.Text = string.Empty;
                txtdebit_ac.Text = string.Empty;
                setFocusControl(txtdebit_ac);
                return;
            }

            if (txtcredit_ac.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtcredit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    dr["credit_ac"] = txtcredit_ac.Text;
                    dr["creditAcName"] = str;
                }
                else
                {
                    lblCredit_name.Text = string.Empty;
                    txtcredit_ac.Text = string.Empty;
                    setFocusControl(txtcredit_ac);
                    return;
                }
            }
            else
            {
                lblCredit_name.Text = string.Empty;
                txtcredit_ac.Text = string.Empty;
                setFocusControl(txtcredit_ac);
                return;
            }

            if (txtamount.Text != string.Empty)
            {
                dr["amount"] = txtamount.Text;
            }
            else
            {
                setFocusControl(txtamount);
                return;
            }

            if (txtnarration.Text != string.Empty)
            {
                dr["narration"] = txtnarration.Text;

            }
            else
            {
                dr["narration"] = txtnarration.Text;
                //setFocusControl(txtnarration);
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
                setFocusControl(txtdebit_ac);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            txtdebit_ac.Text = string.Empty;
            lblDebit_name.Text = string.Empty;
            txtcredit_ac.Text = string.Empty;
            lblCredit_name.Text = string.Empty;
            txtamount.Text = string.Empty;
            txtnarration.Text = string.Empty;
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
        btnSave.Focus();
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gr)
    {
        lblNo.Text = Server.HtmlDecode(gr.Cells[10].Text);
        lblID.Text = Server.HtmlDecode(gr.Cells[2].Text);
        txtdebit_ac.Text = Server.HtmlDecode(gr.Cells[3].Text);
        lblDebit_name.Text = Server.HtmlDecode(gr.Cells[4].Text);
        txtcredit_ac.Text = Server.HtmlDecode(gr.Cells[5].Text);
        lblCredit_name.Text = Server.HtmlDecode(gr.Cells[6].Text);
        txtamount.Text = Server.HtmlDecode(gr.Cells[7].Text);
        txtnarration.Text = Server.HtmlDecode(gr.Cells[8].Text);
        setFocusControl(txtdebit_ac);
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
                string IDExisting = clsCommon.getString("select detail_id from " + tblHead + " where detail_id=" + ID + " and Tran_Type='" + trntype + "' and doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
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

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(4);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(7);
            e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(22);
            e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(22);
            e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(20);

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[5].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[8].Style["overflow"] = "hidden";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            }

            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;

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
            if (v == "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("400px");
            }


            if (v != "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = new Unit("80px");
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
                        if (grdDetail.Rows[rowindex].Cells[9].Text != "D" && grdDetail.Rows[rowindex].Cells[9].Text != "R")
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

    #region [txtdoc_date_TextChanged]
    protected void txtdoc_date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_date.Text;
        strTextBox = "txtdoc_date";
        csCalculations();
    }
    #endregion

    #region [txtdebit_ac_TextChanged]
    protected void txtdebit_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdebit_ac.Text;
        strTextBox = "txtdebit_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtdebit_ac_Click]
    protected void btntxtdebit_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtdebit_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtcredit_ac_TextChanged]
    protected void txtcredit_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcredit_ac.Text;
        strTextBox = "txtcredit_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtcredit_ac_Click]
    protected void btntxtcredit_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtcredit_ac";
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

    #region [txtnarration_TextChanged]
    protected void txtnarration_TextChanged(object sender, EventArgs e)
    {
        searchString = txtnarration.Text;
        strTextBox = "txtnarration";
        csCalculations();
    }
    #endregion

    #region [btntxtnarration_Click]
    protected void btntxtnarration_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtnarration";
            btnSearch_Click(sender, e);
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
            if (hdnfClosePopup.Value == "txtdebit_ac")
            {
                setFocusControl(txtdebit_ac);
            }
            if (hdnfClosePopup.Value == "txtcredit_ac")
            {
                setFocusControl(txtcredit_ac);
            }
            if (hdnfClosePopup.Value == "txtnarration")
            {
                setFocusControl(txtnarration);
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
        if (txtdoc_no.Text != string.Empty)
        {

            if (ViewState["mode"] != null)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'");
                    if (str != string.Empty)
                    {
                        lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                        this.getMaxCode();
                        // hdnf.Value = txtdoc_no.Text;
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
        if (txtdoc_date.Text != string.Empty)
        {
            string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (clsCommon.isValidDate(dt) == true)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
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
        #endregion

        #region -Head part declearation
        Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
        string DOC_DATE = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        string retValue = string.Empty;
        string strRev = string.Empty;
        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int year_Code = Convert.ToInt32(Session["year"].ToString());
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");

        //grid part variables


        #endregion-End of Head part declearation

        #region save Head Master
        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
        {
            if (ViewState["mode"] != null)
            {
                DataSet ds = new DataSet();

                Int32 GID = 0;

                string qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + trntype + "' and DOC_NO=" + DOC_NO + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    Int32 detail_id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    Int32 debit_ac = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    Int32 credit_ac = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                    double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    string narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[8].Text);

                    if (grdDetail.Rows[i].Cells[9].Text != "N" && grdDetail.Rows[i].Cells[9].Text != "R")
                    {
                        if (grdDetail.Rows[i].Cells[9].Text == "A")
                        {
                            obj.flag = 1;
                            obj.tableName = tblHead;
                            obj.columnNm = "Tran_Type,DOC_NO,DOC_DATE,debit_ac,credit_ac,amount,narration,Company_Code,Year_Code,Branch_Code,Created_By";
                            obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + debit_ac + "','" + credit_ac + "','" + amount + "','" + narration + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;

                            #region GLedger Effect
                            if (retValue == "-1")
                            {
                                GID = GID + 1;

                                obj.flag = 1;
                                obj.tableName = GLedgerTable;
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + debit_ac + "','" + narration + "','" + amount + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','D','" + credit_ac + "','0','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                                ds = obj.insertAccountMaster(ref strRev);

                                GID = GID + 1;
                                obj.flag = 1;
                                obj.tableName = GLedgerTable;
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + credit_ac + "','" + narration + "','" + amount + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','C','" + debit_ac + "','0','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                                ds = obj.insertAccountMaster(ref strRev);

                            }
                            #endregion

                        }
                        if (grdDetail.Rows[i].Cells[9].Text == "U")
                        {
                            //Update Mode

                            obj.flag = 2;
                            obj.tableName = tblHead;
                            obj.columnNm = " DOC_DATE='" + DOC_DATE + "',debit_ac='" + debit_ac + "',credit_ac='" + credit_ac + "',amount='" + amount + "',narration='" + narration + "',Modified_By='" + user + "' where Tran_Type='" + trntype + "' and doc_no='" + txtdoc_no.Text + "' and detail_id='" + detail_id + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' ";
                            obj.values = "none";
                            ds = new DataSet();
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;

                            #region GLedger Effect
                            if (retValue == "-2")
                            {
                                GID = GID + 1;

                                obj.flag = 1;
                                obj.tableName = GLedgerTable;
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + debit_ac + "','" + narration + "','" + amount + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','D','" + credit_ac + "','0','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                                ds = obj.insertAccountMaster(ref strRev);

                                GID = GID + 1;
                                obj.flag = 1;
                                obj.tableName = GLedgerTable;
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + credit_ac + "','" + narration + "','" + amount + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','C','" + debit_ac + "','0','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";

                                ds = obj.insertAccountMaster(ref strRev);
                            }
                            #endregion
                        }
                        if (grdDetail.Rows[i].Cells[9].Text == "D")
                        {
                            obj.flag = 3;
                            obj.tableName = tblHead;
                            obj.columnNm = " Tran_Type='" + trntype + "' and doc_no='" + txtdoc_no.Text + "' and detail_id='" + detail_id + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' ";
                            obj.values = "none";
                            ds = new DataSet();
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;
                        }
                    }
                }

                hdnf.Value = txtdoc_no.Text;
                if (retValue == "-1")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
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
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'";

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
                                                txtdoc_no.Enabled = false;
                                                // hdnf.Value = txtdoc_no.Text;
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

                                                    txtdoc_no.Enabled = false;

                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtdoc_date);
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
            if (strTextBox == "txtdoc_date")
            {
                if (txtdoc_date.Text != string.Empty)
                {
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(btnOpenDetailsPopup);
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
            if (strTextBox == "txtdebit_ac")
            {
                if (txtdebit_ac.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtdebit_ac.Text);
                    if (a == false)
                    {
                        btntxtdebit_ac_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtdebit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblDebit_name.Text = str;
                            setFocusControl(txtcredit_ac);
                        }
                        else
                        {
                            lblDebit_name.Text = string.Empty;
                            txtdebit_ac.Text = string.Empty;
                            setFocusControl(txtdebit_ac);

                        }
                    }
                }
                else
                {
                    lblDebit_name.Text = string.Empty;
                    txtdebit_ac.Text = string.Empty;
                    setFocusControl(txtdebit_ac);

                }
            }
            if (strTextBox == "txtcredit_ac")
            {
                if (txtcredit_ac.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtcredit_ac.Text);
                    if (a == false)
                    {
                        btntxtcredit_ac_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtcredit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblCredit_name.Text = str;
                            setFocusControl(txtamount);
                        }
                        else
                        {
                            lblCredit_name.Text = string.Empty;
                            txtcredit_ac.Text = string.Empty;
                            setFocusControl(txtcredit_ac);

                        }
                    }
                }
                else
                {
                    lblCredit_name.Text = string.Empty;
                    txtcredit_ac.Text = string.Empty;
                    setFocusControl(txtcredit_ac);

                }
            }
            if (strTextBox == "txtamount")
            {
                setFocusControl(txtnarration);
            }
            if (strTextBox == "txtnarration")
            {
                setFocusControl(btnAdddetails);
            }

        }
        catch
        {
        }
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
                if (btntxtdoc_no.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtdoc_no.Text = string.Empty;
                    txtdoc_no.Enabled = true;

                    btnSave.Enabled = false;
                    setFocusControl(txtdoc_no);
                    hdnfClosePopup.Value = "Close";
                }
                else
                {
                    lblPopupHead.Text = "--Select DOC No--";
                    string qry = "select doc_no,doc_date from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
                    " and  (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%')  group by doc_no,doc_date order by doc_no";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtdebit_ac")
            {
                lblPopupHead.Text = "--Select debit AC--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtcredit_ac")
            {
                lblPopupHead.Text = "--Select credit AC--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    " (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtnarration")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + systemMasterTable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
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
}
