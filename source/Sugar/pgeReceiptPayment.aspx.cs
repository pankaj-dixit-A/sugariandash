using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Sugar_pgeReceiptPayment : System.Web.UI.Page
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
    string voucherTable = string.Empty;
    string qryVoucherList = string.Empty;
    int defaultAccountCode = 0;
    string trntype = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
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
            voucherTable = tblPrefix + "Voucher";
            qryVoucherList = tblPrefix + "qryVoucherList";
            trntype = drpTrnType.SelectedValue;
            user = Session["user"].ToString();
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
                //btnDelete.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                drpTrnType.Enabled = true;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                //btnDelete.Enabled = true;
                if (drpTrnType.SelectedValue != "CR")
                {
                    drpFilter.Visible = true;
                    txtVoucherNo.Enabled = false;
                    txtvoucherType.Enabled = false;
                    btntxtVoucherNo.Enabled = false;
                }
                else
                {
                    txtVoucherNo.Enabled = false;
                    txtvoucherType.Enabled = false;
                    btntxtVoucherNo.Enabled = false;
                }

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
                lblMsg.Text = string.Empty;
                #region set Business logic for save
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;

                drpTrnType.Enabled = false;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                trntype = drpTrnType.SelectedValue;
                if (trntype == "BP" || trntype == "BR")
                {
                    txtCashBank.Enabled = true;
                    btntxtCashBank.Enabled = true;
                }
                else
                {
                    txtCashBank.Enabled = false;
                    btntxtCashBank.Enabled = false;
                }
                txtdoc_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

                drpTrnType.Enabled = true;
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
                txtCashBank.Enabled = true;
                btntxtCashBank.Enabled = true;
                drpTrnType.Enabled = false;
                txtvoucherType.Enabled = false;
            }
            #region Always check this
            //if (dAction == "A" || dAction == "E")
            //{
            //    if (txtCashBank.Text != String.Empty)
            //    {
            //        txtCashBank.Text = "";
            //        lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            //    }
            //    //if (drpTrnType.SelectedValue == "CP" || drpTrnType.SelectedValue == "CR")
            //    //{
            //    //    txtCashBank.Text = "1";
            //    //    lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //    //    txtCashBank.Enabled = true;
            //    //    btntxtCashBank.Enabled = true;
            //    //}
            //    //else
            //    //{
            //    //    txtCashBank.Enabled = true;
            //    //    btntxtCashBank.Enabled = true;
            //    //}
            //}


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
                            btnEdit.Enabled = true;
                            btnDelete.Enabled = true;
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
        if (RecordCount > 0)
        {
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
            }
            this.makeEmptyForm("S");
        }
        else
        {
            this.makeEmptyForm("N");
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
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
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
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
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
        this.TranTypeFilter();
        this.getMaxCode();
        pnlPopupDetails.Style["display"] = "none";
    }

    private void TranTypeFilter()
    {
        try
        {
            trntype = drpTrnType.SelectedValue;
            if (trntype == "BP" || trntype == "CP")
            {
                drpFilter.Visible = true;
                drpFilter.Items.Clear();
                drpFilter.Items.Add(new ListItem("--Select--", "A"));
                drpFilter.Items.Add(new ListItem("Against Transport Advance", "T"));
                txtVoucherNo.Enabled = false;
                txtvoucherType.Enabled = false;
                btntxtVoucherNo.Enabled = false;

            }
            else
            {
                if (trntype == "BR")
                {
                    drpFilter.Visible = true;
                    drpFilter.Items.Clear();
                    drpFilter.Items.Add(new ListItem("Against Loading Voucher", "V"));
                    drpFilter.Items.Add(new ListItem("Against Sauda", "S"));
                    txtVoucherNo.Enabled = true;
                    txtvoucherType.Enabled = true;
                    btntxtVoucherNo.Enabled = true;
                }
                else
                {
                    drpFilter.Visible = false;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
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
        trntype = drpTrnType.SelectedValue;
        if (trntype == "BP" || trntype == "CP")
        {
            drpFilter.Visible = true;
            drpFilter.Items.Clear();
            drpFilter.Items.Add(new ListItem("--Select--", "A"));
            drpFilter.Items.Add(new ListItem("Against Transport Advance", "T"));
            txtVoucherNo.Enabled = false;
            txtvoucherType.Enabled = false;
            btntxtVoucherNo.Enabled = false;
        }
        else
        {
            if (trntype == "BR")
            {
                drpFilter.Visible = true;
                drpFilter.Items.Clear();
                drpFilter.Items.Add(new ListItem("Against Loading Voucher", "V"));
                drpFilter.Items.Add(new ListItem("Against Sauda", "S"));
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            else
            {
                drpFilter.Visible = false;
            }
        }
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
                string strrev = "";
                string qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + drpTrnType.SelectedValue + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 3;
                    obj.tableName = tblHead;
                    obj.columnNm = "   Doc_No=" + currentDoc_No +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);

                    obj.flag = 3;
                    obj.tableName = GLedgerTable;
                    obj.columnNm = "   Doc_No=" + currentDoc_No +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);
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
        string str = clsCommon.getString("select count(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + drpTrnType.SelectedValue + "'");
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
                        txtCashBank.Text = dt.Rows[0]["debit_ac"].ToString();
                        lblCashBank.Text = dt.Rows[0]["debitAcName"].ToString();
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

                        trntype = drpTrnType.SelectedValue;

                        if (trntype == "BR")
                        {
                            drpFilter.Visible = true;
                        }
                        else
                        {
                            drpFilter.Visible = false;
                        }


                        #region ---------- Details -------------
                        qry = "select detail_id as ID,credit_ac as AcCode,creditAcName as Name,Unit_Code,Unit_Name,Voucher_No,Voucher_Type,[Tender_No] as TenderNo,[TenderDetail_ID] as DetailID, amount,Adjusted_Amount,narration,drpFilterValue from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
            this.columnTotal();
            //csCalculations();
            //    this.enableDisableNavigateButtons();
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
        txtACCode.Text = string.Empty;
        txtUnit_Code.Text = string.Empty;
        txtVoucherNo.Text = string.Empty;
        txtvoucherType.Text = string.Empty;
        txtamount.Text = string.Empty;
        txtadAmount.Text = string.Empty;
        txtnarration.Text = string.Empty;
        lblACName.Text = "";
        lblUnitName.Text = "";
        trntype = drpTrnType.SelectedValue;
        if (trntype == "BP" || trntype == "CP")
        {
            drpFilter.Visible = true;
            drpFilter.Items.Clear();
            drpFilter.Items.Add(new ListItem("--Select--", "A"));
            drpFilter.Items.Add(new ListItem("Against Transport Advance", "T"));
            txtVoucherNo.Enabled = false;
            txtvoucherType.Enabled = false;
            btntxtVoucherNo.Enabled = false;
        }
        else
        {
            if (trntype == "BR")
            {
                drpFilter.Visible = true;
                drpFilter.Items.Clear();
                drpFilter.Items.Add(new ListItem("Against Loading Voucher", "V"));
                drpFilter.Items.Add(new ListItem("Against Sauda", "S"));
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            else
            {
                drpFilter.Visible = false;
            }
        }
        setFocusControl(txtACCode);
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtACCode.Text != string.Empty)
            {
                string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
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
            if (txtVoucherNo.Text != string.Empty && txtVoucherNo.Text != "0")
            {
                string hdnfVal = hdnfTransportBalance.Value.ToString().TrimStart();
                if (!string.IsNullOrEmpty(hdnfVal))
                {
                    double TransportAdvanceBalance = Convert.ToDouble(hdnfTransportBalance.Value.TrimStart());
                    double amount1 = Convert.ToDouble(txtamount.Text);
                    if (amount1 > TransportAdvanceBalance)
                    {
                        lblErrorAdvance.Text = "Amount Is Greater Than Transport Advance Balance!";
                        isValidated = false;
                        setFocusControl(txtamount);
                        return;
                    }
                    else
                    {
                        lblErrorAdvance.Text = "";
                        isValidated = true;
                    }
                }
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
                        int n = int.Parse(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;

                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select detail_id from " + tblHead + " where doc_no=" + txtdoc_no.Text + " and detail_id=" + rowIndex + " And Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + drpTrnType.SelectedValue + "'");
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
                    dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("AcCode", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Unit_Code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Unit_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Voucher_No", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Voucher_Type", typeof(string))));
                    dt.Columns.Add((new DataColumn("TenderNo", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("DetailID", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("Adjusted_Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("drpFilterValue", typeof(string))));
                    #endregion
                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt.Columns.Add((new DataColumn("ID", typeof(int))));


                    dr = dt.NewRow();
                    dr["ID"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
            }
            else
            {
                dt = new DataTable();

                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                #region [Write here columns]
                dt.Columns.Add((new DataColumn("AcCode", typeof(Int32))));
                dt.Columns.Add((new DataColumn("Name", typeof(string))));
                dt.Columns.Add((new DataColumn("Unit_Code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("Unit_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("Voucher_No", typeof(Int32))));
                dt.Columns.Add((new DataColumn("Voucher_Type", typeof(string))));
                dt.Columns.Add((new DataColumn("TenderNo", typeof(Int32))));
                dt.Columns.Add((new DataColumn("DetailID", typeof(Int32))));
                dt.Columns.Add((new DataColumn("amount", typeof(double))));
                dt.Columns.Add((new DataColumn("Adjusted_Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("narration", typeof(string))));
                dt.Columns.Add((new DataColumn("drpFilterValue", typeof(string))));
                #endregion

                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("ID", typeof(int))));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }

            #region [ Set values to dr]
            if (txtACCode.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    dr["AcCode"] = txtACCode.Text;
                    dr["Name"] = str;
                }
                else
                {
                    lblACName.Text = string.Empty;
                    txtACCode.Text = string.Empty;
                    setFocusControl(txtACCode);
                    return;
                }
            }
            else
            {
                lblACName.Text = string.Empty;
                txtACCode.Text = string.Empty;
                setFocusControl(txtACCode);
                return;
            }
            if (txtACCode.Text != string.Empty)
            {
                string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (iscarporate == "Y")
                {
                    if (txtUnit_Code.Text != string.Empty)
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            dr["Unit_Code"] = txtUnit_Code.Text;
                            dr["Unit_Name"] = str;
                        }
                        else
                        {
                            lblUnitName.Text = string.Empty;
                            txtUnit_Code.Text = string.Empty;
                            setFocusControl(txtUnit_Code);
                            return;
                        }
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                        setFocusControl(txtUnit_Code);
                        return;
                    }
                }
                else
                {
                    if (txtUnit_Code.Text != string.Empty)
                    {
                        dr["Unit_Code"] = txtUnit_Code.Text;
                        dr["Unit_Name"] = lblUnitName.Text;
                    }
                    else
                    {
                        dr["Unit_Code"] = "0";
                        dr["Unit_Name"] = "";
                    }
                }
            }

            if (drpFilter.SelectedValue == "V" || drpFilter.SelectedValue == "T")
            {
                //if (txtVoucherNo.Text != string.Empty)
                //{
                //    string str = clsCommon.getString("select Tran_Type from " + tblPrefix + "qryVoucherSaleUnion where Doc_No=" + txtVoucherNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                //    //string voucherby=clsCommon.getString("
                //    if (str != string.Empty)
                //    {
                //        dr["Voucher_No"] = txtVoucherNo.Text;
                //        dr["Voucher_Type"] = txtvoucherType.Text;

                //        dr["TenderNo"] = 0;
                //        dr["DetailID"] = 0;
                //    }
                //    else
                //    {
                //        txtvoucherType.Text = string.Empty;
                //        txtVoucherNo.Text = string.Empty;
                //        setFocusControl(txtVoucherNo);
                //        return;
                //    }

                //}
                dr["Voucher_No"] = txtVoucherNo.Text != string.Empty ? Convert.ToInt32(txtVoucherNo.Text) : 0;
                dr["Voucher_Type"] = txtvoucherType.Text;

                dr["TenderNo"] = 0;
                dr["DetailID"] = 0;
                dr["drpFilterValue"] = drpFilter.SelectedValue;
            }
            if (drpFilter.SelectedValue == "S")
            {
                dr["Voucher_No"] = 0;
                dr["Voucher_Type"] = "";

                dr["TenderNo"] = txtVoucherNo.Text != string.Empty ? Convert.ToInt32(txtVoucherNo.Text) : 0;
                dr["DetailID"] = txtvoucherType.Text;
                dr["drpFilterValue"] = drpFilter.SelectedValue;
            }
            else
            {
                dr["drpFilterValue"] = drpFilter.SelectedValue;
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
            if (txtadAmount.Text != string.Empty)
            {
                dr["Adjusted_Amount"] = txtadAmount.Text;
            }
            else
            {
                dr["Adjusted_Amount"] = 0;
            }

            if (txtnarration.Text != string.Empty)
            {
                dr["narration"] = Server.HtmlDecode(txtnarration.Text);
            }
            else
            {
                dr["narration"] = Server.HtmlDecode(txtnarration.Text);
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
                setFocusControl(txtACCode);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            csCalculations();
            txtACCode.Text = string.Empty;
            txtVoucherNo.Text = string.Empty;
            txtvoucherType.Text = string.Empty;
            lblACName.Text = string.Empty;
            txtamount.Text = string.Empty;
            txtadAmount.Text = string.Empty;
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
        btnAdd.Focus();
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gr)
    {
        try
        {
            this.TranTypeFilter();
            lblNo.Text = Server.HtmlDecode(gr.Cells[16].Text);
            lblID.Text = Server.HtmlDecode(gr.Cells[2].Text);
            txtACCode.Text = Server.HtmlDecode(gr.Cells[3].Text);
            lblACName.Text = Server.HtmlDecode(gr.Cells[4].Text);
            txtUnit_Code.Text = Server.HtmlDecode(gr.Cells[5].Text);
            lblUnitName.Text = Server.HtmlDecode(gr.Cells[6].Text);
            string selectedValue = Server.HtmlDecode(gr.Cells[14].Text);
            drpFilter.SelectedValue = selectedValue;
            if (Server.HtmlDecode(gr.Cells[7].Text) != "0")
            {
                lblHead.Text = "Voucher No";
                txtVoucherNo.Text = Server.HtmlDecode(gr.Cells[7].Text);
                txtvoucherType.Text = Server.HtmlDecode(gr.Cells[8].Text);
            }
            else
            {
                lblHead.Text = "Sauda No";
                txtVoucherNo.Text = Server.HtmlDecode(gr.Cells[9].Text);
                txtvoucherType.Text = Server.HtmlDecode(gr.Cells[10].Text);
            }
            txtVoucherNo.Enabled = false;
            txtvoucherType.Enabled = false;
            txtamount.Text = Server.HtmlDecode(gr.Cells[11].Text);
            txtadAmount.Text = Server.HtmlDecode(gr.Cells[12].Text);
            txtnarration.Text = Server.HtmlDecode(gr.Cells[13].Text);
        }
        catch (Exception)
        {
            throw;
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
                string IDExisting = clsCommon.getString("select detail_id from " + tblHead + " where detail_id=" + ID + " and Tran_Type='" + trntype + "' and doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[15].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "A";
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
        try
        {
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(4);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(7);
            e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(22);
            e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(22);
            e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(7);
            e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(7);
            e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[11].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[12].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[13].ControlStyle.Width = Unit.Percentage(25);

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[11].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[8].Style["overflow"] = "hidden";
            e.Row.Cells[9].Style["overflow"] = "hidden";
            e.Row.Cells[10].Style["overflow"] = "hidden";
            e.Row.Cells[12].Style["overflow"] = "hidden";

            int i = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Left;

                if (e.Row.Cells[13].Text.Length > 27)
                {
                    e.Row.Cells[13].Style["overflow"] = "hidden";
                    string s = e.Row.Cells[13].Text.ToString();
                    //e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 27) + "..";
                    e.Row.Cells[13].ToolTip = s;
                }
            }
            e.Row.Cells[14].Visible = false;
            e.Row.Cells[15].Visible = false;
            e.Row.Cells[16].Visible = false;

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
        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v == "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

            if (v != "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = new Unit("80px");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            }
            i++;
            foreach (TableCell cell in e.Row.Cells)
            {
                string s = cell.Text.ToString();
                if (cell.Text.Length > 35)
                {
                    cell.Text = cell.Text.Substring(0, 35) + "..";
                    cell.ToolTip = s;
                }
            }
        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtVoucherNo")
            {
                if (drpFilter.SelectedValue == "V")
                {
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(10);

                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Left;
                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text.ToString();
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "..";
                            cell.ToolTip = s;
                        }
                    }
                }
                if (drpFilter.SelectedValue == "T")
                {
                    e.Row.Cells[0].ControlStyle.Width = new Unit("15px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("10px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("10px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("300px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("100px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("300px");
                    e.Row.Cells[6].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[7].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[8].ControlStyle.Width = new Unit("80px");
                }
                if (drpFilter.SelectedValue == "S")
                {
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(10);

                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Left;
                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text.ToString();
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "..";
                            cell.ToolTip = s;
                        }
                    }
                }
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
                        if (grdDetail.Rows[rowindex].Cells[15].Text != "D" && grdDetail.Rows[rowindex].Cells[15].Text != "R")
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

    #region [txtACCode_TextChanged]
    protected void txtACCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtACCode.Text;
        strTextBox = "txtACCode";
        csCalculations();
    }
    #endregion

    #region [btntxtACCode_Click]
    protected void btntxtACCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtACCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtVoucherNo_TextChanged]
    protected void txtVoucherNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVoucherNo.Text;
        strTextBox = "txtVoucherNo";
        csCalculations();
    }
    #endregion

    #region [btntxtVoucherNo_Click]
    protected void btntxtVoucherNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtVoucherNo";
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

    #region [txtadAmount_TextChanged]
    protected void txtadAmount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtadAmount.Text;
        strTextBox = "txtadAmount";
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

    #region [btntxtCashBank_Click]
    protected void btntxtCashBank_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCashBank";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtCashBank_TextChanged]
    protected void txtCashBank_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCashBank.Text;
        strTextBox = "txtCashBank";
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
            if (hdnfClosePopup.Value == "txtACCode")
            {
                setFocusControl(txtACCode);
            }
            if (hdnfClosePopup.Value == "txtVoucherNo")
            {
                setFocusControl(txtVoucherNo);
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
            string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
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
        trntype = drpTrnType.SelectedValue;
        if (trntype == "BP" || trntype == "BR")
        {
            if (txtCashBank.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtCashBank);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtCashBank);
                return;
            }
        }
        #endregion

        #region -Head part declearation
        Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
        string DOC_DATE = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        int cmpcashac = Convert.ToInt32(clsCommon.getString("Select Ac_Code from " + tblPrefix + "AccountMaster where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())));
        Int32 cashBank = txtCashBank.Text != string.Empty ? Convert.ToInt32(txtCashBank.Text) : cmpcashac;
        string retValue = string.Empty;
        string strRev = string.Empty;
        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int year_Code = Convert.ToInt32(Session["year"].ToString());
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");

        string drcr = string.Empty;
        string drcr0 = string.Empty;
        if (trntype == "CP" || trntype == "BP")
        {
            drcr = "D";
            drcr0 = "C";
        }
        else
        {
            drcr = "C";
            drcr0 = "D";
        }

        #endregion-End of Head part declearation

        #region save Head Master
        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
        {
            if (ViewState["mode"] != null)
            {
                DataSet ds = new DataSet();

                Int32 GID = 0;
                string qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + drpTrnType.SelectedValue + "' and DOC_NO=" + DOC_NO + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    Int32 vocherNo = 0;
                    string voucherType = "";
                    Int32 Tender_No = 0;
                    Int32 TenderDetail_ID = 0;
                    string drpFiltervalue = Server.HtmlDecode(grdDetail.Rows[i].Cells[14].Text);
                    Int32 acCode = grdDetail.Rows[i].Cells[3].Text != string.Empty ? Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text) : 0;
                    Int32 Unit_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                    //Int32 acCode = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    Int32 detail_id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);

                    if (drpFiltervalue == "V" || drpFiltervalue == "T")
                    {
                        vocherNo = Convert.ToInt32("0" + Server.HtmlDecode(grdDetail.Rows[i].Cells[7].Text));
                        voucherType = Server.HtmlDecode(grdDetail.Rows[i].Cells[8].Text);
                    }
                    if (drpFiltervalue == "S")
                    {
                        Tender_No = Convert.ToInt32("0" + Server.HtmlEncode(grdDetail.Rows[i].Cells[9].Text));
                        TenderDetail_ID = Convert.ToInt32("0" + Server.HtmlEncode(grdDetail.Rows[i].Cells[10].Text));
                    }
                    double amount = 0.00;
                    try
                    {
                        amount = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                    }
                    catch
                    {
                        amount = 0.00;
                    }
                    double adAmount = 0.00;
                    try
                    {
                        adAmount = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text);
                    }
                    catch
                    {
                        adAmount = 0.00;
                    }
                    string narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[13].Text);
                    if (grdDetail.Rows[i].Cells[15].Text != "N" && grdDetail.Rows[i].Cells[15].Text != "R")
                    {
                        if (grdDetail.Rows[i].Cells[15].Text == "A")
                        {
                            obj.flag = 1;
                            obj.tableName = tblHead;
                            obj.columnNm = "Tran_Type,DOC_NO,DOC_DATE,debit_ac,credit_ac,Unit_Code,amount,narration,Company_Code,Year_Code,Branch_Code,Created_By,Voucher_No,Voucher_Type,Adjusted_Amount,[Tender_No],[TenderDetail_ID],drpFilterValue";
                            obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','" + acCode + "','" + Unit_Code + "','" + amount + "','" + narration + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + vocherNo + "','" + voucherType + "','" + adAmount + "','" + Tender_No + "','" + TenderDetail_ID + "','" + drpFiltervalue + "'";
                            ds = obj.insertAccountMaster(ref strRev);
                            retValue = strRev;

                            #region GLedger Effect
                            if (retValue == "-1")
                            {
                                GID = GID + 1;
                                obj.flag = 1;
                                obj.tableName = GLedgerTable;
                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + acCode + "','" + Unit_Code + "','" + narration + "','" + amount + "',null,null,'" + vocherNo + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + detail_id + "','" + drcr + "','" + cashBank + "','" + adAmount + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + trntype + "','" + DOC_NO + "'";
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                ds = obj.insertAccountMaster(ref strRev);

                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','" + narration + "','" + amount + "',null,null,'" + vocherNo + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + detail_id + "','" + drcr0 + "','" + acCode + "','" + adAmount + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + trntype + "','" + DOC_NO + "'";
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                ds = obj.insertAccountMaster(ref strRev);
                            }
                            #endregion
                        }
                        if (grdDetail.Rows[i].Cells[15].Text == "U")
                        {
                            obj.flag = 2;
                            obj.tableName = tblHead;

                            obj.columnNm = " DOC_DATE='" + DOC_DATE + "',debit_ac='" + cashBank + "',credit_ac='" + acCode + "',Unit_Code='" + Unit_Code + "',amount='" + amount + "',narration='" + narration + "',Modified_By='" + user + "',Voucher_No='" + vocherNo + "',Voucher_Type='" + voucherType + "',Adjusted_Amount='" + adAmount + "',Tender_No='" + Tender_No + "',TenderDetail_ID='" + TenderDetail_ID + "',drpFilterValue='" + drpFiltervalue + "' where Tran_Type='" + trntype + "' and doc_no='" + txtdoc_no.Text + "' and detail_id='" + detail_id + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' ";
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

                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + acCode + "','" + Unit_Code + "','" + narration + "','" + amount + "',null,null,'" + vocherNo + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + detail_id + "','" + drcr + "','" + cashBank + "','" + adAmount + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + trntype + "','" + DOC_NO + "'";
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,UNIT_Code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                ds = obj.insertAccountMaster(ref strRev);

                                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','" + narration + "','" + amount + "',null,null,'" + vocherNo + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + detail_id + "','" + drcr0 + "','" + acCode + "','" + adAmount + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + trntype + "','" + DOC_NO + "'";
                                obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                ds = obj.insertAccountMaster(ref strRev);
                            }
                            #endregion
                        }
                        if (grdDetail.Rows[i].Cells[15].Text == "D")
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

                #region Insert order code 0 entry of gledger
                GID = GID + 1;
                obj.flag = 1;
                //obj.tableName = GLedgerTable;
                //obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code";
                //obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','','" + txtTotal.Text + "',null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "',0,'" + drcr0 + "','" + cashBank + "',0,'" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "'";
                //ds = obj.insertAccountMaster(ref strRev);

                #endregion
                if (retValue == "-1")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");

                    hdnf.Value = txtdoc_no.Text;
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
                                                    pnlgrdDetail.Enabled = true;
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
                                            pnlgrdDetail.Enabled = true;
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
            if (strTextBox == "txtACCode")
            {
                if (txtACCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtACCode.Text);
                    if (a == false)
                    {
                        btntxtACCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblACName.Text = str;
                            string ac_city = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            txtnarration.Text = ac_city;
                            setFocusControl(txtUnit_Code);
                        }
                        else
                        {
                            lblACName.Text = string.Empty;
                            txtACCode.Text = string.Empty;
                            setFocusControl(txtACCode);
                        }
                    }
                }
                else
                {
                    lblACName.Text = string.Empty;
                    txtACCode.Text = string.Empty;
                    setFocusControl(txtACCode);

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
                        string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (iscarporate == "Y")
                        {
                            lblMsg.Text = "";
                            lblPopupHead.Text = "--Select Unit--";
                            string qry = "select UnitName from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtACCode.Text +
                                " and Unit_name=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                            acname = clsCommon.getString(qry);
                            if (acname != string.Empty)
                            {
                                lblUnitName.Text = acname;
                                setFocusControl(txtVoucherNo);
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
                            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (str != string.Empty)
                            {
                                lblACName.Text = str;
                                setFocusControl(txtVoucherNo);
                            }
                            else
                            {
                                lblUnitName.Text = string.Empty;
                                txtUnit_Code.Text = string.Empty;
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
            if (strTextBox == "txtCashBank")
            {
                if (txtCashBank.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtCashBank.Text);
                    if (a == false)
                    {
                        btntxtCashBank_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblCashBank.Text = str;
                            setFocusControl(btnOpenDetailsPopup);
                        }
                        else
                        {
                            lblCashBank.Text = string.Empty;
                            txtCashBank.Text = string.Empty;
                            setFocusControl(txtCashBank);
                        }
                    }
                }
                else
                {
                    lblCashBank.Text = string.Empty;
                    txtCashBank.Text = string.Empty;
                    setFocusControl(txtCashBank);

                }
            }
            //string ac_city = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (strTextBox == "txtVoucherNo")
            {
                setFocusControl(txtamount);
                DataSet ds;
                if (drpFilter.SelectedValue == "V")
                {
                    if (txtVoucherNo.Text != string.Empty)
                    {
                        qry = "";// "select * from " + tblPrefix + "Voucher where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Doc_No=" + txtVoucherNo.Text + " and Tran_Type='" + txtvoucherType.Text + "'";
                        qry = "select Doc_No as doc_no,Tran_Type,Suffix,Convert(varchar(10),Doc_Date,103) as Doc_Date,PartyName,Unit_Name,NETQNTL,BrokerName,Sale_Rate,Bill_Amount,mill_code, " +
                              " (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code" +
                              " ) as Paid_Amount,(Bill_Amount - (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where " +
                              " Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code )) as Balance " +
                              " from " + tblPrefix + "qryVoucherSaleUnion where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "" +
                              "  and doc_no=" + txtVoucherNo.Text + " and Tran_Type='" + txtvoucherType.Text + "'";

                        ds = new DataSet();
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string VocNo = "Vouch No:" + txtVoucherNo.Text + "(" + txtvoucherType.Text + ")";
                            string millcode = ds.Tables[0].Rows[0]["mill_code"].ToString();
                            string millshortname = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + millcode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            double Qntl = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["NETQNTL"].ToString())), 2);
                            double SR = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Sale_Rate"].ToString())), 2);
                            //double frt = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["FreightPerQtl"].ToString())), 2);
                            //string naration = VocNo + " " + millshortname + "-" + Qntl + "-(SR:" + SR + "-frt:" + frt + ")";
                            string naration = VocNo + " " + millshortname + "-" + Qntl + "-(SR:" + SR + ")";
                            txtnarration.Text = naration;
                            hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["Balance"].ToString();
                        }
                        else
                        {
                            txtnarration.Text = "";
                        }
                    }
                }
                if (drpFilter.SelectedValue == "S")
                {
                    string qry = "";  // "Select * from " + tblPrefix + "qryTenderList where Tender_No=" + txtVoucherNo.Text + " and ID=" + txtvoucherType.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                    qry = "select [Tender_No],[ID],Convert(VarChar(10),[Tender_Date],103) as Tender_Date,[millname],[salerate],[salepartyfullname],[Buyer_Quantal],[salevalue],[received],[balance],[Commission_Rate] from " + tblPrefix + "qrySaudaBalance" +
                       " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtVoucherNo.Text + " and ID=" + txtvoucherType.Text + "";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string millshortname = ds.Tables[0].Rows[0]["millname"].ToString();
                        //string millshortname = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + millcode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        double Qntl = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Buyer_Quantal"].ToString())), 2);
                        double SR = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["salerate"].ToString())), 2);
                        double Commission = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Commission_Rate"].ToString())), 2);
                        double Sale_Rate = SR - Commission;
                        string SaudaDate = ds.Tables[0].Rows[0]["Tender_Date"].ToString();
                        txtnarration.Text = millshortname + " " + Qntl + "(SR:" + Sale_Rate + "+Comm:" + Commission + ")dt-" + SaudaDate;
                        hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["balance"].ToString();
                    }
                }
                if (drpFilter.SelectedValue == "T")
                {
                    string qry = ""; // "Select * from " + tblPrefix + "qryDeliveryOrderListReport where doc_no=" + txtVoucherNo.Text.Trim() + " and tran_type='" + txtvoucherType.Text.Trim() + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                    qry = "select do.doc_no,do.voucher_no,do.quantal,do.tran_type,Convert(varchar(10),do.doc_date,103) as date,do.millName as Mill,do.truck_no,do.TransportName,do.Memo_Advance," +
                         " (Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code ) as Paid,((do.Memo_Advance)-" +
                         "(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance" +
                         " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and do.doc_no=" + txtVoucherNo.Text.Trim() + " and do.tran_type='" + txtvoucherType.Text.Trim() + "' and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string vn = "";
                        string VoucherNo = ds.Tables[0].Rows[0]["voucher_no"].ToString();
                        string lorry = ds.Tables[0].Rows[0]["truck_no"].ToString();
                        string qntl = ds.Tables[0].Rows[0]["quantal"].ToString();
                        string Memo_Advance = ds.Tables[0].Rows[0]["Memo_Advance"].ToString();
                        if (VoucherNo != "0")
                        {
                            vn = "Vouc.No.:" + VoucherNo;
                        }
                        string ac_city = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string narration = vn + " Lorry:" + lorry + " Qntl:" + qntl + "-" + Memo_Advance + "(" + ac_city + ")";
                        txtnarration.Text = narration;
                        hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["Balance"].ToString();
                    }
                }
            }
            if (strTextBox == "txtamount")
            {
                if (txtVoucherNo.Text != string.Empty)
                {
                    string hdnfVal = hdnfTransportBalance.Value.ToString().TrimStart();
                    if (!string.IsNullOrEmpty(hdnfVal))
                    {
                        double TransportAdvanceBalance = hdnfVal != string.Empty ? Convert.ToDouble(hdnfVal) : 0.00;
                        double amount1 = Convert.ToDouble(txtamount.Text);
                        if (amount1 > TransportAdvanceBalance)
                        {
                            lblErrorAdvance.Text = "Amount Is Greater Than Transport Advance Balance!";
                            setFocusControl(txtamount);
                        }
                        else
                        {
                            lblErrorAdvance.Text = "";
                        }
                    }
                }
                setFocusControl(txtadAmount);
            }
            if (strTextBox == "txtadAmount")
            {
                setFocusControl(txtnarration);
            }
            if (strTextBox == "txtnarration")
            {
                setFocusControl(btnAdddetails);
            }
            this.columnTotal();
        }
        catch
        {
        }
    }

    private void columnTotal()
    {
        #region Calculation Part
        double total = 0.00;
        if (grdDetail.Rows.Count > 0)
        {

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                double Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                total = total + Amt;
            }
        }
        txtTotal.Text = Math.Round(total, 2).ToString();
        #endregion
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty)// && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
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
                    string qry = "select doc_no,doc_date,debitAcName from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
                    " and  (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%') group by doc_no,doc_date,debitAcName order by doc_no";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtACCode")
            {
                lblPopupHead.Text = "--Select AC Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    " (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtUnit_Code")
            {
                if (txtACCode.Text != string.Empty)
                {
                    string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (iscarporate == "Y")
                    {
                        lblMsg.Text = "";
                        lblPopupHead.Text = "--Select Unit--";
                        string qry = "select Unit_name,UnitName,unitCity from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtACCode.Text +
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
                    setFocusControl(txtACCode);
                }
            }
            if (hdnfClosePopup.Value == "txtCashBank")
            {
                string qry = "";
                lblPopupHead.Text = "--Select Cash/Bank--";
                if (drpTrnType.SelectedValue == "CP")
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    " Ac_type='C' and  (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                }
                else
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                       " Ac_type='B' and  (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                }
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtVoucherNo")
            {
                if (drpFilter.SelectedValue == "V")
                {
                    lblPopupHead.Text = "--Select Voucher No--";
                    string qry = "select Doc_No as doc_no,Tran_Type,Suffix,Convert(varchar(10),Doc_Date,103) as Doc_Date,PartyName,Unit_Name,NETQNTL,BrokerName,Sale_Rate,Bill_Amount, " +
                    " (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code" +
                    " ) as Paid_Amount,(Bill_Amount - (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where " +
                    " Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code )) as Balance " +
                    " from " + tblPrefix + "qryVoucherSaleUnion where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "" +
                    "  and Ac_Code=" + txtACCode.Text + "";
                    this.showPopup(qry);
                }
                if (drpFilter.SelectedValue == "S")
                {
                    lblPopupHead.Text = "--Select Sauda No--";
                    string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Tender_Date],103) as Tender_Date,[millname],[salerate],[salepartyfullname],[Buyer_Quantal],[salevalue],[received],[balance] from " + tblPrefix + "qrySaudaBalance" +
                        " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) + " and [Buyer]=" + txtACCode.Text + "";
                    this.showPopup(qry);
                }
                if (drpFilter.SelectedValue == "T")
                {
                    lblPopupHead.Text = "--Select Transport Account--";
                    qry = "select do.doc_no,do.tran_type,Convert(varchar(10),do.doc_date,103) as date,do.millName as Mill,do.truck_no as Lorry,do.TransportName,do.Memo_Advance as Transport_Advance," +
                          " (Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code ) as Paid,((do.Memo_Advance)-" +
                          "(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance" +
                          " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and do.transport=" + txtACCode.Text + " and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    this.showPopup(qry);
                }
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

    protected void drpTrnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Focus();
        this.showLastRecord();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    protected void drpFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drpFilter.SelectedValue == "V" || drpFilter.SelectedValue == "T")
            {
                lblHead.Text = "Voucher Number";
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            if (drpFilter.SelectedValue == "S")
            {
                lblHead.Text = "Sauda Number";
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            if (drpFilter.SelectedValue == "0")
            {
                txtVoucherNo.Enabled = false;
                txtvoucherType.Enabled = false;
                btntxtVoucherNo.Enabled = false;
            }
            setFocusControl(txtVoucherNo);
        }
        catch
        {

        }
    }
    protected void grdPopup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdPopup.SelectedIndex = -1;
            grdPopup.DataBind();
        }
        catch { }
    }
    protected void txtVoucherNo_TextChanged1(object sender, EventArgs e)
    {
        searchString = txtVoucherNo.Text;
        strTextBox = "txtVoucherNo";
        csCalculations();
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
}
