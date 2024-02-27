using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class pgeLoadingvoucher : System.Web.UI.Page
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
    string TranTyp = string.Empty;
    int defaultAccountCode = 0;
    string GLedgerTable = string.Empty;
    string qryAccountsList = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string Delivery_Type = string.Empty;
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
            TranTyp = "OV";
            GLedgerTable = tblPrefix + "GLEDGER";
            qryCommon = tblPrefix + "qryVoucherList";
            qryAccountsList = tblPrefix + "qryAccountsList";
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
                    if (Session["VOUC_NO"] != null)
                    {
                        hdnf.Value = Session["VOUC_NO"].ToString();
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        this.enableDisableNavigateButtons();
                        Session["VOUC_NO"] = null;
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

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                //  obj.tableName = tblHead + " where Tran_Type='" + TranTyp + "' and Company_Code="+Convert.ToInt32(Session["Company_Code"].ToString())+" and Year_Code="+Convert.ToInt32(Session["year"].ToString())+" and Branch_Code="+Convert.ToInt32(Session["Branch_Code"].ToString());
                obj.tableName = tblHead + " where Tran_Type='" + TranTyp + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                lblUnitName.Text = "";
                lblMillname.Text = string.Empty;
                lblBroker_NAME.Text = string.Empty;
                lblPartyname.Text = string.Empty;
                lblTransport_Name.Text = string.Empty;
                LBLCash_Account.Text = string.Empty;
                btnPrint.Enabled = true;
                btnITCVouc.Enabled = true;
                btnFormE.Enabled = true;
                btntxtDO_No.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtCash_Account.Enabled = false;
                btntxtGrade.Enabled = false;
                calenderExtenderDate.Enabled = false;
                btntxtUnitcode.Enabled = false;
                btntxtMill_Code.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtGrade1.Enabled = false;
                drpRateType.Enabled = false;
                //btnDelete.Enabled = true;
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
                lblUnitName.Text = "";
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Change No";
                btntxtdoc_no.Enabled = true;
                drpRateType.Enabled = true;
                #region set Business logic for save
                setFocusControl(txtDoc_date);
                lblMillname.Text = string.Empty;
                lblBroker_NAME.Text = string.Empty;
                lblPartyname.Text = string.Empty;
                lblTransport_Name.Text = string.Empty;
                LBLCash_Account.Text = string.Empty;
                lblDeliveryType.Text = "";
                btntxtDO_No.Enabled = true;
                btntxtAC_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtCash_Account.Enabled = true;
                btntxtGrade.Enabled = true;
                calenderExtenderDate.Enabled = true;
                btntxtMill_Code.Enabled = true;
                btntxtUnitcode.Enabled = true;
                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtGrade1.Enabled = true;
                btnPrint.Enabled = false;
                btnITCVouc.Enabled = false;
                btnFormE.Enabled = false;
                txtDoc_date.Text = DateTime.Now.ToString("dd/MM/yyyy"); //clsCommon.getString("select Convert(varchar(10),getDate(),103) as docDate");
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
                btntxtDO_No.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtCash_Account.Enabled = false;
                btntxtGrade.Enabled = false;
                calenderExtenderDate.Enabled = false;

                btntxtMill_Code.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtGrade1.Enabled = false;
                drpRateType.Enabled = false;
                btnPrint.Enabled = true;
                btnITCVouc.Enabled = true;
                btnFormE.Enabled = true;
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
                txtDoc_no.Enabled = true;
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                #region logic
                setFocusControl(txtDoc_date);
                btntxtDO_No.Enabled = true;
                btntxtAC_CODE.Enabled = true;
                btntxtUnitcode.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtCash_Account.Enabled = true;
                btntxtGrade.Enabled = true;
                calenderExtenderDate.Enabled = true;
                btntxtMill_Code.Enabled = true;
                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtGrade1.Enabled = true;
                drpRateType.Enabled = true;
                btnPrint.Enabled = false;
                btnITCVouc.Enabled = false;
                btnFormE.Enabled = false;
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
            //   qry = "select max(doc_no) as doc_no from " + tblHead+" where Year_Code="+Convert.ToInt32(Session["year"].ToString())+" and Company_Code="+Convert.ToInt32(Session["Company_Code"].ToString())+" and Branch_Code="+Convert.ToInt32(Session["Branch_Code"].ToString())+" and Tran_Type='"+TranTyp+"'";
            qry = "select max(doc_no) as doc_no from " + tblHead + " where Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Tran_Type='" + TranTyp + "'";
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
                        qry = "select Suffix from " + tblHead + " where doc_no=" + hdnf.Value +
                            " and Tran_Type='" + TranTyp + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
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
                    else
                    {
                        hdnf.Value = "0";
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
        // query = "select count(*) from " + tblHead + " where Tran_Type='"+TranTyp+"' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +" and Tran_Type='"+TranTyp+"'";
        query = "select count(*) from " + tblHead + " where Tran_Type='" + TranTyp + "' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and Tran_Type='" + TranTyp + "'";
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
            if (txtDoc_no.Text != string.Empty)
            {
                if (hdnf.Value != string.Empty)
                {
                    #region check for next or previous record exist or not
                    ds = new DataSet();
                    dt = new DataTable();
                    //   query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())  + " and Tran_Type='" + TranTyp + "' ORDER BY doc_no asc  ";
                    query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and Tran_Type='" + TranTyp + "' ORDER BY doc_no asc  ";
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
                    //  query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())  + " and Tran_Type='" + TranTyp + "' ORDER BY doc_no desc  ";
                    query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "' ORDER BY doc_no desc  ";
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
            // query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no)  from " + tblHead + ") and  Company_Code="+Convert.ToInt32(Session["Company_Code"].ToString())+" and Year_Code="+Convert.ToInt32(Session["year"].ToString())+" and Branch_Code="+Convert.ToInt32(Session["Branch_Code"].ToString());
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no)  from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "') and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "'";
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
                //string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                //    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +" and Branch_Code="+Convert.ToInt32(Session["Branch_Code"].ToString())+
                //    " ORDER BY doc_no DESC  ";
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "'" +
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
                //string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                //    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())  +
                //    " ORDER BY doc_no asc  ";
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "'" +
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
            // query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + ") and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) ;
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "') and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "'";
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
        txtDoc_no.Enabled = false;
        //pnlPopupDetails.Style["display"] = "none";
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        this.makeEmptyForm("E");
        //txtDoc_no.Enabled = false;
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string currentDoc_No = txtDoc_no.Text;
                string qry = "";
                DataSet ds = new DataSet();


                //update DO to set voucher no to 0
                qry = "update " + tblPrefix + "deliveryorder set voucher_no=0 ,voucher_type='' where tran_type='DO' and doc_no=" + txtDO_No.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + TranTyp + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);

                qry = "delete from " + tblHead + " where Doc_No=" + currentDoc_No + " and Tran_Type='" + TranTyp + "' and Suffix='" + hdnfSuffix.Value.Trim() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);



                qry = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                       " and Tran_Type='" + TranTyp + "'  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                        " ORDER BY Doc_No asc  ";


                hdnf.Value = clsCommon.getString(qry);

                if (hdnf.Value == string.Empty)
                {
                    qry = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                         " and Tran_Type='" + TranTyp + "'  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                        " ORDER BY Doc_No desc  ";
                    hdnf.Value = clsCommon.getString(qry);
                    hdnfSuffix.Value = clsCommon.getString("select Suffix from " + tblHead + " where Doc_No='" + hdnf.Value + "' and Tran_Type='" + TranTyp + "'  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                }

                if (hdnf.Value != string.Empty)
                {
                    qry = getDisplayQuery(); ;
                    bool recordExist = this.fetchRecord(qry);

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

        string str = clsCommon.getString("select count(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + TranTyp + "'");
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
                        txtDoc_no.Text = dt.Rows[0]["DOC_NO"].ToString();
                        txtSuffix.Text = dt.Rows[0]["SUFFIX"].ToString();
                        txtDoc_date.Text = dt.Rows[0]["DOC_DATE"].ToString();
                        txtDO_No.Text = dt.Rows[0]["DO_NO"].ToString();
                        txtAC_CODE.Text = dt.Rows[0]["AC_CODE"].ToString();
                        txtUnit_Code.Text = dt.Rows[0]["Unit_Code"].ToString();
                        lblUnitName.Text = dt.Rows[0]["Unit_Name"].ToString();
                        lblPartyname.Text = dt.Rows[0]["PartyName"].ToString();
                        txtMill_Code.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        lblMillname.Text = dt.Rows[0]["MillName"].ToString();
                        txtLorry_No.Text = dt.Rows[0]["LORRY_NO"].ToString();
                        string transportcode = dt.Rows[0]["TRANSPORT_CODE"].ToString();
                        txtTRANSPORT_CODE.Text = transportcode;
                        if (transportcode != string.Empty)
                        {
                            lblTransport_Name.Text = clsCommon.getString("SELECT Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + dt.Rows[0]["TRANSPORT_CODE"].ToString() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        }
                        txtBroker_CODE.Text = dt.Rows[0]["BROKER_CODE"].ToString();
                        lblBroker_NAME.Text = dt.Rows[0]["BrokerName"].ToString();
                        txtFrom_Place.Text = dt.Rows[0]["FROM_PLACE"].ToString();
                        txtTo_Place.Text = dt.Rows[0]["TO_PLACE"].ToString();
                        txtQuantal.Text = dt.Rows[0]["QUANTAL"].ToString();
                        txtPACKING.Text = dt.Rows[0]["PACKING"].ToString();
                        txtBAGS.Text = dt.Rows[0]["BAGS"].ToString();
                        txtGrade.Text = dt.Rows[0]["GRADE"].ToString();
                        txtMill_Rate.Text = dt.Rows[0]["MILL_RATE"].ToString();
                        txtMill_Amount.Text = dt.Rows[0]["MILL_AMOUNT"].ToString();
                        txtQuantal1.Text = dt.Rows[0]["QUANTAL1"].ToString();
                        txtPACKING1.Text = dt.Rows[0]["PACKING1"].ToString();
                        txtBAGS1.Text = dt.Rows[0]["BAGS1"].ToString();
                        txtGrade1.Text = dt.Rows[0]["GRADE1"].ToString();
                        txtMill_Rate1.Text = dt.Rows[0]["MILL_RATE1"].ToString();
                        txtMill_Amount1.Text = dt.Rows[0]["MILL_AMOUNT1"].ToString();
                        txtSale_Rate.Text = dt.Rows[0]["SALE_RATE"].ToString();
                        txtFreightPerQtl.Text = dt.Rows[0]["FREIGHTPERQTL"].ToString();
                        txtLESSDIFF.Text = dt.Rows[0]["LESSDIFF"].ToString();
                        txtNarration1.Text = dt.Rows[0]["NARRATION1"].ToString();
                        txtBrokrage.Text = dt.Rows[0]["BROKRAGE"].ToString();
                        txtNarration2.Text = dt.Rows[0]["NARRATION2"].ToString();
                        txtService_Charge.Text = dt.Rows[0]["SERVICE_CHARGE"].ToString();
                        txtNarration3.Text = dt.Rows[0]["NARRATION3"].ToString();
                        txtL_Rate_Diff.Text = dt.Rows[0]["L_RATE_DIFF"].ToString();
                        txtRATEDIFF.Text = dt.Rows[0]["RATEDIFF"].ToString();
                        txtInterest.Text = dt.Rows[0]["INTEREST"].ToString();
                        txtCommission_Rate.Text = dt.Rows[0]["Commission_Rate"].ToString();
                        txtBANK_COMMISSION.Text = dt.Rows[0]["Commission_Amount"].ToString();
                        txtNarration4.Text = dt.Rows[0]["NARRATION4"].ToString();
                        txtTransport_Amount.Text = dt.Rows[0]["TRANSPORT_AMOUNT"].ToString();
                        txtOTHER_Expenses.Text = dt.Rows[0]["OTHER_EXPENSES"].ToString();
                        string cashaccount = dt.Rows[0]["CASH_ACCOUNT"].ToString();
                        txtCash_Account.Text = cashaccount;
                        LBLCash_Account.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + cashaccount + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtCash_Amount_RATE.Text = dt.Rows[0]["CASH_AMOUNT_RATE"].ToString();
                        txtCash_Ac_Amount.Text = dt.Rows[0]["CASH_AC_AMOUNT"].ToString();
                        txtVoucher_Amount.Text = dt.Rows[0]["VOUCHER_AMOUNT"].ToString();
                        txtASNGRNNo.Text = dt.Rows[0]["ASN_No"].ToString();
                        txtDue_Days.Text = dt.Rows[0]["Due_Days"].ToString();
                        txtFreightDiff.Text = dt.Rows[0]["Diff_Rate"].ToString();
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
                        string rate_type = dt.Rows[0]["Rate_Type"].ToString();
                        if (rate_type == "L")
                        {
                            drpRateType.SelectedValue = "L";
                        }
                        if (rate_type == "A")
                        {
                            drpRateType.SelectedValue = "A";
                        }
                        //else
                        //{
                        //    drpRateType.SelectedIndex = 0;
                        //}
                        Delivery_Type = dt.Rows[0]["Delivery_Type"].ToString();
                        if (Delivery_Type == "N")
                        {
                            lblDeliveryType.Text = "Naka Delivery";
                        }
                        else
                        {
                            lblDeliveryType.Text = "Commission";
                        }

                        hdnf.Value = txtDoc_no.Text;
                        hdnfSuffix.Value = txtSuffix.Text.Trim();
                        recordExist = true;
                        lblMsg.Text = "";
                        //pnlgrdDetail.Enabled = false;
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
            string qryDisplay = "select * from " + qryCommon + " where Tran_Type='" + TranTyp + "' and Doc_No='" + hdnf.Value + "' and Suffix='" + hdnfSuffix.Value + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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

        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtMill_Code" || v == "txtAC_CODE" || v == "txtTRANSPORT_CODE" || v == "txtBroker_CODE" || v == "txtCash_Account")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = new Unit("80px");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            }
            if (v == "txtDO_No")
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(5);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(7);
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(7);
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(7);
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(20);
                e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);


            }

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

    #region [txtDoc_no_TextChanged]
    protected void txtDoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDoc_no.Text;
        strTextBox = "txtDoc_no";
        csCalculations();
    }
    #endregion

    #region [btntxtDoc_no_Click]
    protected void btntxtDoc_no_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDoc_no";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSuffix_TextChanged]
    protected void txtSuffix_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSuffix.Text;
        strTextBox = "txtDoc_no";
        csCalculations();
    }
    #endregion

    #region [txtDoc_date_TextChanged]
    protected void txtDoc_date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDoc_date.Text;
        strTextBox = "txtDoc_date";
        csCalculations();
    }
    #endregion

    #region [txtDO_No_TextChanged]
    protected void txtDO_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDO_No.Text;
        strTextBox = "txtDO_No";
        csCalculations();
    }
    #endregion

    #region [btntxtDO_No_Click]
    protected void btntxtDO_No_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDO_No";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
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

    #region [txtMill_Code_TextChanged]
    protected void txtMill_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMill_Code.Text;
        strTextBox = "txtMill_Code";
        csCalculations();
    }
    #endregion

    #region [btntxtMill_Code_Click]
    protected void btntxtMill_Code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMill_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtLorry_No_TextChanged]
    protected void txtLorry_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLorry_No.Text;
        strTextBox = "txtLorry_No";
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

    #region [txtFrom_Place_TextChanged]
    protected void txtFrom_Place_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFrom_Place.Text;
        strTextBox = "txtFrom_Place";
        csCalculations();
    }
    #endregion

    #region [txtTo_Place_TextChanged]
    protected void txtTo_Place_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTo_Place.Text;
        strTextBox = "txtTo_Place";
        csCalculations();
    }
    #endregion

    #region [txtQuantal_TextChanged]
    protected void txtQuantal_TextChanged(object sender, EventArgs e)
    {
        searchString = txtQuantal.Text;
        strTextBox = "txtQuantal";
        txtPACKING.Text = "50";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtPACKING_TextChanged]
    protected void txtPACKING_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPACKING.Text;
        strTextBox = "txtPACKING";
        csCalculations();
        AmountCalculation();
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

    #region [txtGrade_TextChanged]
    protected void txtGrade_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtGrade.Text;
        //strTextBox = "txtGrade";
        setFocusControl(txtMill_Rate);
    }
    #endregion

    #region [btntxtGrade_Click]
    protected void btntxtGrade_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGrade";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtMill_Rate_TextChanged]
    protected void txtMill_Rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMill_Rate.Text;
        strTextBox = "txtMill_Rate";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtMill_Amount_TextChanged]
    protected void txtMill_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMill_Amount.Text;
        strTextBox = "txtMill_Amount";
        csCalculations();
    }
    #endregion

    #region [txtQuantal1_TextChanged]
    protected void txtQuantal1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtQuantal1.Text;
        strTextBox = "txtQuantal1";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtPACKING1_TextChanged]
    protected void txtPACKING1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPACKING1.Text;
        strTextBox = "txtPACKING1";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtBAGS1_TextChanged]
    protected void txtBAGS1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBAGS1.Text;
        strTextBox = "txtBAGS1";
        csCalculations();
    }
    #endregion

    #region [txtGrade1_TextChanged]
    protected void txtGrade1_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtMill_Rate1);
    }
    #endregion

    #region [btntxtGrade1_Click]
    protected void btntxtGrade1_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGrade1";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtMill_Rate1_TextChanged]
    protected void txtMill_Rate1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMill_Rate1.Text;
        strTextBox = "txtMill_Rate1";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtMill_Amount1_TextChanged]
    protected void txtMill_Amount1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMill_Amount1.Text;
        strTextBox = "txtMill_Amount1";
        csCalculations();
    }
    #endregion

    #region [txtSale_Rate_TextChanged]
    protected void txtSale_Rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSale_Rate.Text;
        strTextBox = "txtSale_Rate";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtFreightPerQtl_TextChanged]
    protected void txtFreightPerQtl_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFreightPerQtl.Text;
        strTextBox = "txtFreightPerQtl";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtLESSDIFF_TextChanged]
    protected void txtLESSDIFF_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLESSDIFF.Text;
        strTextBox = "txtLESSDIFF";
        csCalculations();
        AmountCalculation();
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

    #region [txtBrokrage_TextChanged]
    protected void txtBrokrage_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBrokrage.Text;
        strTextBox = "txtBrokrage";
        csCalculations();
        AmountCalculation();
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

    #region [txtService_Charge_TextChanged]
    protected void txtService_Charge_TextChanged(object sender, EventArgs e)
    {
        searchString = txtService_Charge.Text;
        strTextBox = "txtService_Charge";
        csCalculations();
        AmountCalculation();
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

    #region [txtL_Rate_Diff_TextChanged]
    protected void txtL_Rate_Diff_TextChanged(object sender, EventArgs e)
    {
        searchString = txtL_Rate_Diff.Text;
        strTextBox = "txtL_Rate_Diff";
        AmountCalculation();
        csCalculations();

    }
    #endregion

    #region [txtRATEDIFF_TextChanged]
    protected void txtRATEDIFF_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRATEDIFF.Text;
        strTextBox = "txtRATEDIFF";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtInterest_TextChanged]
    protected void txtInterest_TextChanged(object sender, EventArgs e)
    {
        searchString = txtInterest.Text;
        strTextBox = "txtInterest";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtNarration4_TextChanged]
    protected void txtNarration4_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNarration4.Text;
        strTextBox = "txtNarration4";
        csCalculations();
    }
    #endregion

    #region [txtTransport_Amount_TextChanged]
    protected void txtTransport_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTransport_Amount.Text;
        strTextBox = "txtTransport_Amount";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtOTHER_Expenses_TextChanged]
    protected void txtOTHER_Expenses_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOTHER_Expenses.Text;
        strTextBox = "txtOTHER_Expenses";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtCash_Account_TextChanged]
    protected void txtCash_Account_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCash_Account.Text;
        strTextBox = "txtCash_Account";

        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [btntxtCash_Account_Click]
    protected void btntxtCash_Account_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCash_Account";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtCash_Amount_RATE_TextChanged]
    protected void txtCash_Amount_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCash_Amount_RATE.Text;
        strTextBox = "txtCash_Amount_RATE";
        setFocusControl(drpRateType);
        double cashAmtRate = Convert.ToDouble("0" + txtCash_Amount_RATE.Text);
        double qtl = Convert.ToDouble("0" + txtQuantal.Text);
        double qtl1 = Convert.ToDouble("0" + txtQuantal1.Text);
        double cashAmount = 0.00;
        cashAmount = Math.Round(((qtl + qtl1) * cashAmtRate), 2);
        txtCash_Ac_Amount.Text = cashAmount.ToString();

        //AmountCalculation();
    }
    #endregion

    #region [txtCash_Ac_Amount_TextChanged]
    protected void txtCash_Ac_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCash_Ac_Amount.Text;
        strTextBox = "txtCash_Ac_Amount";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtVoucher_Amount_TextChanged]
    protected void txtVoucher_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVoucher_Amount.Text;
        strTextBox = "txtVoucher_Amount";
        csCalculations();
    }
    #endregion

    #region [txtCommission_Rate_TextChanged]
    protected void txtCommission_Rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCommission_Rate.Text;
        strTextBox = "txtCommission_Rate";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtDue_Days_TextChanged]
    protected void txtDue_Days_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDue_Days.Text;
        strTextBox = "txtDue_Days";
        csCalculations();
    }
    #endregion

    #region [txtBANK_COMMISSION_TextChanged]
    protected void txtBANK_COMMISSION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_COMMISSION.Text;
        strTextBox = "txtBANK_COMMISSION";
        csCalculations();
        AmountCalculation();
    }
    #endregion

    #region [txtFreightDiff_TextChanged]
    protected void txtFreightDiff_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtFreightDiff.Text;
        //strTextBox = "txtFreightDiff";
        //csCalculations();
        //AmountCalculation();
        //if (txtFreightDiff.Text != string.Empty)
        //{
        //    double lessDiff = Convert.ToDouble(txtLESSDIFF.Text);
        //    double freightDiff = Convert.ToDouble(txtFreightDiff.Text);
        //    double cashAmount = lessDiff - freightDiff;
        //    txtCash_Ac_Amount.Text = cashAmount.ToString();
        //}
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
            if (hdnfClosePopup.Value == "txtDoc_no")
            {
                if (btntxtdoc_no.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtDoc_no.Text = string.Empty;
                    txtDoc_no.Enabled = true;

                    btnSave.Enabled = false;
                    setFocusControl(txtDoc_no);
                    hdnfClosePopup.Value = "Close";
                }

                if (btntxtdoc_no.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select Voucher No--";
                    string qry = "select doc_no,doc_date,quantal,PartyName,MillName from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + TranTyp + "' " +
                        " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millName like '%" + txtSearchText.Text + "%' or PartyName like '%" + txtSearchText.Text + "%')  order by doc_date";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtDO_No")
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
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='B' " + " and " + AccountMasterTable + ".Ac_type!='C' " +
                    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
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
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and (Unit_name like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or unitCity like '%" + txtSearchText.Text + "%') order by UnitName";
                        this.showPopup(qry);
                    }
                    else
                    {
                        lblPopupHead.Text = "--Select Unit--";
                        string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                            " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='B' " + " and " + AccountMasterTable + ".Ac_type!='C' " +
                            " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                        this.showPopup(qry);
                    }
                }
            }
            if (hdnfClosePopup.Value == "txtMill_Code")
            {
                lblPopupHead.Text = "--Select Mill--";
                //string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                //    " inner join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code where " + AccountMasterTable + ".Company_Code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='M' " +
                //    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                   " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                   + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='M' " +
                   " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                lblPopupHead.Text = "--Select Transport--";
                //string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                //    " inner join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code where " + AccountMasterTable + ".Company_Code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='T' " +
                //    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                  " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                  AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='T' " +
                   " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " LEFT OUTER join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' " +
                    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGrade")
            {
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGrade1")
            {
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtCash_Account")
            {
                lblPopupHead.Text = "--Select cash Account--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='T' and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
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
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                setFocusControl(txtAC_CODE);
            }
            if (hdnfClosePopup.Value == "txtMill_Code")
            {
                setFocusControl(txtMill_Code);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {
                setFocusControl(txtBroker_CODE);
            }
            if (hdnfClosePopup.Value == "txtCash_Account")
            {
                setFocusControl(txtCash_Account);
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
        try
        {
            #region [Validation Part]
            string myNarration = string.Empty;
            bool isValidated = true;
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
                        string str = clsCommon.getString("select Doc_No from " + tblHead + " where Tran_Type='" + TranTyp + "' and Doc_No='" + txtDoc_no.Text + "'" +
                                 " and Suffix='" + txtSuffix.Text.Trim() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));

                        if (str != string.Empty)
                        {
                            lblMsg.Text = "Doc No " + txtDoc_no.Text + " already exist";
                            isValidated = false;
                            setFocusControl(txtSuffix);
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
                setFocusControl(txtDoc_no);
                return;
            }
            if (txtDoc_date.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtDoc_date);
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
            if (txtMill_Code.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMill_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='M'");
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtMill_Code);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtMill_Code);
                return;
            }
            if (txtDO_No.Text != string.Empty)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    string str = clsCommon.getString("select doc_no  from " + tblPrefix + "deliveryorder  where doc_no=" + txtDO_No.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and voucher_no=0");
                    if (str != string.Empty)
                    {
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                        txtDO_No.Text = "";
                        setFocusControl(txtDO_No);
                        return;
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
                setFocusControl(txtDO_No);
                return;
            }
            if (txtQuantal.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtQuantal);
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
            if (txtGrade.Text != string.Empty)
            {

                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtGrade);
                return;
            }
            if (txtMill_Rate.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtMill_Rate);
                return;
            }
            if (txtSale_Rate.Text != string.Empty || txtSale_Rate.Text != "0")
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtSale_Rate);
                return;
            }
            if (txtCash_Ac_Amount.Text != string.Empty)
            {
                if (Convert.ToDouble(txtCash_Ac_Amount.Text) != 0)
                {
                    if (txtCash_Account.Text != string.Empty)
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCash_Account.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            isValidated = true;
                        }
                        else
                        {
                            LBLCash_Account.Text = string.Empty;
                            isValidated = false;
                            setFocusControl(txtCash_Account);
                            return;
                        }
                    }
                    else
                    {
                        LBLCash_Account.Text = string.Empty;
                        isValidated = false;
                        setFocusControl(txtCash_Account);
                        return;
                    }
                }
                else
                {
                    if (txtCash_Account.Text == string.Empty)
                    {
                        isValidated = false;
                        setFocusControl(txtCash_Ac_Amount);
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
                if (txtCash_Account.Text != string.Empty)
                {
                    isValidated = false;
                    setFocusControl(txtCash_Ac_Amount);
                    return;
                }
                else
                {
                    isValidated = true;
                }
            }
            #endregion

            #region -Head part declearation
            Int32 DOC_NO = txtDoc_no.Text != string.Empty ? Convert.ToInt32(txtDoc_no.Text) : 0;
            string SUFFIX = txtSuffix.Text;
            string DOC_DATE = DateTime.Parse(txtDoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            Int32 DO_NO = txtDO_No.Text != string.Empty ? Convert.ToInt32(txtDO_No.Text) : 0;
            Int32 AC_CODE = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
            Int32 Unit_Code = txtUnit_Code.Text != string.Empty ? Convert.ToInt32(txtUnit_Code.Text) : 0;
            Int32 MILL_CODE = txtMill_Code.Text != string.Empty ? Convert.ToInt32(txtMill_Code.Text) : 0;
            string LORRY_NO = txtLorry_No.Text;
            Int32 TRANSPORT_CODE = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
            Int32 BROKER_CODE = txtBroker_CODE.Text != string.Empty ? Convert.ToInt32(txtBroker_CODE.Text) : 2;
            string FROM_PLACE = txtFrom_Place.Text;
            string TO_PLACE = txtTo_Place.Text;
            double QUANTAL = txtQuantal.Text != string.Empty ? Convert.ToDouble(txtQuantal.Text) : 0.00;
            Int32 PACKING = txtPACKING.Text != string.Empty ? Convert.ToInt32(txtPACKING.Text) : 0;
            Int32 BAGS = txtBAGS.Text != string.Empty ? Convert.ToInt32(txtBAGS.Text) : 0;
            string GRADE = txtGrade.Text;
            double MILL_RATE = txtMill_Rate.Text != string.Empty ? Convert.ToDouble(txtMill_Rate.Text) : 0.00;
            double MILL_AMOUNT = txtMill_Amount.Text != string.Empty ? Convert.ToDouble(txtMill_Amount.Text) : 0.00;
            double Commission_Rate = txtCommission_Rate.Text != string.Empty ? Convert.ToDouble(txtCommission_Rate.Text) : 0.00;
            double Commission_Amount = txtBANK_COMMISSION.Text != string.Empty ? Convert.ToDouble(txtBANK_COMMISSION.Text) : 0.00;
            double bank_Commission_Amount = txtBANK_COMMISSION.Text != string.Empty ? Convert.ToDouble(txtBANK_COMMISSION.Text) : 0.00;
            double QUANTAL1 = txtQuantal1.Text != string.Empty ? Convert.ToDouble(txtQuantal1.Text) : 0.00;
            Int32 PACKING1 = txtPACKING1.Text != string.Empty ? Convert.ToInt32(txtPACKING1.Text) : 0;
            Int32 BAGS1 = txtBAGS1.Text != string.Empty ? Convert.ToInt32(txtBAGS1.Text) : 0;
            string GRADE1 = txtGrade1.Text;
            string Rate_Type = drpRateType.SelectedValue;
            string ASN_No = txtASNGRNNo.Text;
            double MILL_RATE1 = txtMill_Rate1.Text != string.Empty ? Convert.ToDouble(txtMill_Rate1.Text) : 0.00;
            double MILL_AMOUNT1 = txtMill_Amount1.Text != string.Empty ? Convert.ToDouble(txtMill_Amount1.Text) : 0.00;
            double SALE_RATE = txtSale_Rate.Text != string.Empty ? Convert.ToDouble(txtSale_Rate.Text) : 0.00;
            double FREIGHTPERQTL = txtFreightPerQtl.Text != string.Empty ? Convert.ToDouble(txtFreightPerQtl.Text) : 0.00;
            double LESSDIFF = txtLESSDIFF.Text != string.Empty ? Convert.ToDouble(txtLESSDIFF.Text) : 0.00;
            string NARRATION1 = txtNarration1.Text;
            double BROKRAGE = txtBrokrage.Text != string.Empty ? Convert.ToDouble(txtBrokrage.Text) : 0.00;
            string NARRATION2 = txtNarration2.Text;
            double SERVICE_CHARGE = txtService_Charge.Text != string.Empty ? Convert.ToDouble(txtService_Charge.Text) : 0.00;
            string NARRATION3 = txtNarration3.Text;
            double L_RATE_DIFF = txtL_Rate_Diff.Text != string.Empty ? Convert.ToDouble(txtL_Rate_Diff.Text) : 0.00;
            double RATEDIFF = txtRATEDIFF.Text != string.Empty ? Convert.ToDouble(txtRATEDIFF.Text) : 0.00;
            //  double BankCommission = txtBANK_COMMISSION.Text != string.Empty ? Convert.ToDouble(txtBANK_COMMISSION.Text) : 0.00;
            double INTEREST = txtInterest.Text != string.Empty ? Convert.ToDouble(txtInterest.Text) : 0.00;
            string NARRATION4 = txtNarration4.Text;
            double TRANSPORT_AMOUNT = txtTransport_Amount.Text != string.Empty ? Convert.ToDouble(txtTransport_Amount.Text) : 0.00;
            double OTHER_EXPENSES = txtOTHER_Expenses.Text != string.Empty ? Convert.ToDouble(txtOTHER_Expenses.Text) : 0.00;
            Int32 CASH_ACCOUNT = txtCash_Account.Text != string.Empty ? Convert.ToInt32(txtCash_Account.Text) : 0;
            double CASH_AMOUNT_RATE = txtCash_Amount_RATE.Text != string.Empty ? Convert.ToDouble(txtCash_Amount_RATE.Text) : 0.00;
            double CASH_AC_AMOUNT = txtCash_Ac_Amount.Text != string.Empty ? Convert.ToDouble(txtCash_Ac_Amount.Text) : 0.00;
            double VOUCHER_AMOUNT = txtVoucher_Amount.Text != string.Empty ? Convert.ToDouble(txtVoucher_Amount.Text) : 0.00;
            Int32 Due_Days = txtDue_Days.Text != string.Empty ? Convert.ToInt32(txtDue_Days.Text) : 0;

            myNarration = "Qntl " + QUANTAL + QUANTAL1 + "  " + lblMillname.Text + " M.R." + MILL_RATE + " S.R." + SALE_RATE + " Less Frt " + FREIGHTPERQTL;

            double Diff_Rate = txtFreightDiff.Text != string.Empty ? Convert.ToDouble(txtFreightDiff.Text) : 0.00;
            double rate_diffference = txtFreightDiff.Text != string.Empty ? Convert.ToDouble(txtFreightDiff.Text) : 0.00;
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            string qry = "";
            double NetRateDiff = (VOUCHER_AMOUNT - CASH_AC_AMOUNT) - (MILL_AMOUNT + MILL_AMOUNT1);
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
                        obj.columnNm = "Tran_Type,DOC_NO,SUFFIX,DOC_DATE,DO_NO,AC_CODE,MILL_CODE," +
                        " LORRY_NO,TRANSPORT_CODE,BROKER_CODE,FROM_PLACE,TO_PLACE,QUANTAL,PACKING," +
                        " BAGS,GRADE,MILL_RATE,MILL_AMOUNT,QUANTAL1,PACKING1,BAGS1,GRADE1,MILL_RATE1," +
                        " MILL_AMOUNT1,SALE_RATE,FREIGHTPERQTL,LESSDIFF,NARRATION1,BROKRAGE,NARRATION2," +
                        " SERVICE_CHARGE,NARRATION3,L_RATE_DIFF,RATEDIFF,INTEREST,Commission_Rate," +
                        " Commission_Amount,NARRATION4,TRANSPORT_AMOUNT,OTHER_EXPENSES,CASH_ACCOUNT," +
                        " CASH_AMOUNT_RATE,CASH_AC_AMOUNT,VOUCHER_AMOUNT,Due_Days,Company_Code,Year_Code,Branch_Code,Created_By,Diff_Rate,Rate_Type,Unit_Code,ASN_No";

                        obj.values = "'" + TranTyp + "','" + DOC_NO + "','" + SUFFIX + "','" + DOC_DATE + "','" + DO_NO + "','" + AC_CODE + "','" + MILL_CODE +
                            "','" + LORRY_NO + "','" + TRANSPORT_CODE + "','" + BROKER_CODE + "','" + FROM_PLACE + "','" + TO_PLACE
                            + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + GRADE + "','" + MILL_RATE + "','" +
                            MILL_AMOUNT + "','" + QUANTAL1 + "','" + PACKING1 + "','" + BAGS1 + "','" + GRADE1 + "','" +
                            MILL_RATE1 + "','" + MILL_AMOUNT1 + "','" + SALE_RATE + "','" + FREIGHTPERQTL + "','" + LESSDIFF +
                            "','" + NARRATION1 + "','" + BROKRAGE + "','" + NARRATION2 + "','" + SERVICE_CHARGE + "','" +
                            NARRATION3 + "','" + L_RATE_DIFF + "','" + RATEDIFF + "','" + INTEREST + "','" + Commission_Rate + "','" +
                            Commission_Amount + "','" + NARRATION4 + "','" + TRANSPORT_AMOUNT + "','" + OTHER_EXPENSES +
                            "','" + CASH_ACCOUNT + "','" + CASH_AMOUNT_RATE + "','" + CASH_AC_AMOUNT + "','" + VOUCHER_AMOUNT +
                            "','" + Due_Days + "','" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + user + "','" + Diff_Rate + "','" + Rate_Type + "','" + Unit_Code + "','" + ASN_No + "'";
                        string nn = obj.insertDO(ref strRev);
                        txtNarration4.Text = nn;
                        retValue = strRev;

                        //update voucher no to DO
                        qry = "";
                        qry = "update " + tblPrefix + "deliveryorder set voucher_no=" + txtDoc_no.Text + " , voucher_type='" + TranTyp + "',Modified_By='" + userinfo + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + txtDO_No.Text;
                        ds = clsDAL.SimpleQuery(qry);

                    }
                    else
                    {
                        //Update Mode
                        obj.flag = 2;
                        obj.tableName = tblHead;
                        obj.columnNm = "DOC_NO='" + DOC_NO + "',SUFFIX='" + SUFFIX + "',DOC_DATE='" + DOC_DATE +
                                                 "',DO_NO='" + DO_NO + "',AC_CODE='" + AC_CODE + "',MILL_CODE='" + MILL_CODE +
                                                 "',LORRY_NO='" + LORRY_NO + "',TRANSPORT_CODE='" + TRANSPORT_CODE +
                                                 "',BROKER_CODE='" + BROKER_CODE + "',FROM_PLACE='" + FROM_PLACE + "',TO_PLACE='" +
                                                 TO_PLACE + "',QUANTAL='" + QUANTAL + "',PACKING='" + PACKING +
                                                 "',BAGS='" + BAGS + "',GRADE='" + GRADE + "',MILL_RATE='" + MILL_RATE +
                                                 "',MILL_AMOUNT='" + MILL_AMOUNT + "',QUANTAL1='" + QUANTAL1 +
                                                 "',PACKING1='" + PACKING1 + "',BAGS1='" + BAGS1 + "',GRADE1='" + GRADE1 +
                                                 "',MILL_RATE1='" + MILL_RATE1 + "',MILL_AMOUNT1='" + MILL_AMOUNT1 +
                                                 "',SALE_RATE='" + SALE_RATE + "',FREIGHTPERQTL='" + FREIGHTPERQTL +
                                                 "',LESSDIFF='" + LESSDIFF + "',NARRATION1='" + NARRATION1 + "',BROKRAGE='" +
                                                 BROKRAGE + "',NARRATION2='" + NARRATION2 + "',SERVICE_CHARGE='" +
                                                 SERVICE_CHARGE + "',NARRATION3='" + NARRATION3 + "',L_RATE_DIFF='" +
                                                 L_RATE_DIFF + "',RATEDIFF='" + RATEDIFF + "',INTEREST='" + INTEREST +
                                                 "', Commission_Rate='" + Commission_Rate + "' ,Commission_Amount='" +
                                                 Commission_Amount + "' ,NARRATION4='" + NARRATION4 + "',TRANSPORT_AMOUNT='" +
                                                 TRANSPORT_AMOUNT + "',OTHER_EXPENSES='" + OTHER_EXPENSES + "',CASH_ACCOUNT='" +
                                                 CASH_ACCOUNT + "',CASH_AMOUNT_RATE='" + CASH_AMOUNT_RATE + "',CASH_AC_AMOUNT='" +
                                                 CASH_AC_AMOUNT + "',VOUCHER_AMOUNT='" + VOUCHER_AMOUNT + "',Due_Days='" + Due_Days + "',ASN_No='" + ASN_No + "',Modified_By='" + user + "',Diff_Rate='" + Diff_Rate + "',Rate_Type='" + Rate_Type + "',Unit_Code='" + Unit_Code + "'  where Tran_Type='" + TranTyp + "' and " +
                                                 " Doc_No='" + txtDoc_no.Text + "' and Suffix='" + txtSuffix.Text.Trim() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        obj.values = "none";
                        ds = new DataSet();
                        ds = obj.insertAccountMaster(ref strRev);
                        retValue = strRev;

                        //update voucher no to DO
                        qry = "";
                        qry = "update " + tblPrefix + "deliveryorder set voucher_no=" + txtDoc_no.Text + " , voucher_type='" + TranTyp + "' where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_no=" + txtDO_No.Text;
                        ds = clsDAL.SimpleQuery(qry);
                    }

                    gleder.LoadingVoucherGlederEffect(TranTyp, DOC_NO, Convert.ToInt32(Session["Company_Code"].ToString()), Convert.ToInt32(Session["year"].ToString()));

                    if (retValue == "-1")
                    {
                        hdnf.Value = txtDoc_no.Text;
                        clsButtonNavigation.enableDisable("S");
                        this.enableDisableNavigateButtons();

                        hdnfSuffix.Value = txtSuffix.Text.Trim();
                        this.makeEmptyForm("S");
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        AmountCalculation();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);
                    }
                    if (retValue == "-2" || retValue == "-3")
                    {
                        hdnf.Value = txtDoc_no.Text;
                        hdnfSuffix.Value = txtSuffix.Text.Trim();
                        clsButtonNavigation.enableDisable("S");
                        this.enableDisableNavigateButtons();
                        this.makeEmptyForm("S");
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        AmountCalculation();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                    }
                }
            }
            #endregion
        }

        catch (Exception exxx)
        {
            txtNarration3.Text = exxx.Message;
        }

    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (IsAsync == false)
            {
                if (strTextBox == "txtDoc_no")
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

                                string qry = "select * from " + tblHead + " where Tran_Type='" + TranTyp + "' and  Doc_No='" + txtValue + "' " +
                                    " and Suffix='" + txtSuffix.Text.Trim() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " +
                                    " Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                                                       " and Tran_Type='" + TranTyp + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                                                      " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                                                    bool recordExist = this.fetchRecord(qry);
                                                    if (recordExist == true)
                                                    {
                                                        txtDoc_no.Enabled = true;
                                                        setFocusControl(txtDoc_date);
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
                                                setFocusControl(txtDoc_date);
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
                if (strTextBox == "txtSuffix")
                {
                    setFocusControl(txtSuffix);
                }
                if (strTextBox == "txtDoc_date")
                {
                    if (txtDoc_date.Text != string.Empty)
                    {
                        try
                        {
                            string dodate = DateTime.Parse(txtDoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                            if (clsCommon.isValidDate(dodate) == true)
                            {
                                setFocusControl(txtDO_No);
                            }
                            else
                            {
                                txtDoc_date.Text = "";
                                setFocusControl(txtDoc_date);

                            }
                        }
                        catch
                        {
                            txtDoc_date.Text = "";
                            setFocusControl(txtDoc_date);
                        }
                    }
                }
                if (strTextBox == "txtDO_No")
                {
                    if (txtDO_No.Text != string.Empty)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string qry = "";
                        qry = "select * from " + tblPrefix + "qryDeliveryOrderList where doc_no=" + txtDO_No.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and voucher_no=0 and tran_type='DO'";
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    txtMill_Code.Text = dt.Rows[0]["mill_code"].ToString();
                                    lblMillname.Text = dt.Rows[0]["millName"].ToString();
                                    txtAC_CODE.Text = dt.Rows[0]["voucher_by"].ToString();
                                    lblPartyname.Text = dt.Rows[0]["VoucherByname"].ToString();
                                    txtLorry_No.Text = dt.Rows[0]["truck_no"].ToString();
                                    txtTRANSPORT_CODE.Text = dt.Rows[0]["transport"].ToString();
                                    lblTransport_Name.Text = dt.Rows[0]["TransportName"].ToString();
                                    txtBroker_CODE.Text = dt.Rows[0]["broker"].ToString();
                                    lblBroker_NAME.Text = dt.Rows[0]["BrokerName"].ToString();
                                    txtSale_Rate.Text = dt.Rows[0]["sale_rate"].ToString();
                                    txtQuantal.Text = dt.Rows[0]["quantal"].ToString();
                                    txtPACKING.Text = dt.Rows[0]["packing"].ToString();
                                    txtBAGS.Text = dt.Rows[0]["bags"].ToString();
                                    txtGrade.Text = dt.Rows[0]["grade"].ToString();
                                    txtMill_Rate.Text = dt.Rows[0]["mill_rate"].ToString();
                                    txtMill_Amount.Text = dt.Rows[0]["Amount"].ToString();
                                    txtFrom_Place.Text = clsCommon.getString("select CityName from " + qryAccountsList + " where Ac_Code=" + txtMill_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                    txtTo_Place.Text = clsCommon.getString("select CityName from " + qryAccountsList + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                    txtFreightPerQtl.Text = dt.Rows[0]["FreightPerQtl"].ToString();
                                    setFocusControl(txtAC_CODE);
                                }
                                else
                                {
                                    setFocusControl(txtDO_No);
                                }
                            }
                            else
                            {
                                setFocusControl(txtDO_No);
                            }

                        }
                        else
                        {
                            setFocusControl(txtDO_No);
                        }
                    }
                    else
                    {
                        setFocusControl(txtDO_No);
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
                            acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (acname != string.Empty)
                            {

                                lblPartyname.Text = acname;
                                setFocusControl(txtUnit_Code);
                                txtTo_Place.Text = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtAC_CODE.Text);
                            }
                            else
                            {
                                txtAC_CODE.Text = string.Empty;
                                lblPartyname.Text = acname;
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
                                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  order by UnitName";
                                acname = clsCommon.getString(qry);
                                if (acname != string.Empty)
                                {
                                    lblUnitName.Text = acname;
                                    setFocusControl(txtMill_Code);
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
                                acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtUnit_Code.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                if (acname != string.Empty)
                                {
                                    lblUnitName.Text = acname;
                                    setFocusControl(txtMill_Code);
                                    //txtTo_Place.Text = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtAC_CODE.Text);
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
                if (strTextBox == "txtCash_Account")
                {
                    string acname = "";
                    if (txtCash_Account.Text != string.Empty)
                    {
                        bool a = clsCommon.isStringIsNumeric(txtCash_Account.Text);
                        if (a == false)
                        {
                            btntxtCash_Account_Click(this, new EventArgs());
                        }
                        else
                        {
                            acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCash_Account.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (acname != string.Empty)
                            {
                                LBLCash_Account.Text = acname;
                                setFocusControl(txtCash_Ac_Amount);
                                txtTo_Place.Text = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtCash_Account.Text);
                            }
                            else
                            {
                                txtCash_Account.Text = string.Empty;
                                LBLCash_Account.Text = acname;
                                setFocusControl(txtCash_Account);
                            }
                        }
                    }
                    else
                    {
                        setFocusControl(txtAC_CODE);
                    }
                }
                if (strTextBox == "txtMill_Code")
                {
                    string millName = "";
                    if (txtMill_Code.Text != string.Empty)
                    {
                        bool a = clsCommon.isStringIsNumeric(txtMill_Code.Text);
                        if (a == false)
                        {
                            btntxtMill_Code_Click(this, new EventArgs());
                        }
                        else
                        {
                            millName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMill_Code.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (millName != string.Empty)
                            {
                                lblMillname.Text = millName;
                                setFocusControl(txtLorry_No);
                                txtFrom_Place.Text = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtMill_Code.Text);
                            }
                            else
                            {
                                txtMill_Code.Text = string.Empty;
                                lblMillname.Text = millName;
                                setFocusControl(txtMill_Code);
                            }
                        }
                    }
                    else
                    {
                        setFocusControl(txtMill_Code);
                    }
                }
                if (strTextBox == "txtLorry_No")
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
                            transportName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (transportName != string.Empty)
                            {

                                lblTransport_Name.Text = transportName;
                                setFocusControl(txtBroker_CODE);
                            }
                            else
                            {
                                txtTRANSPORT_CODE.Text = string.Empty;
                                lblTransport_Name.Text = transportName;
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

                                lblBroker_NAME.Text = brokerName;
                                setFocusControl(txtFrom_Place);
                            }
                            else
                            {
                                txtBroker_CODE.Text = string.Empty;
                                lblBroker_NAME.Text = brokerName;
                                setFocusControl(txtBroker_CODE);
                            }
                        }
                    }
                    else
                    {
                        setFocusControl(txtBroker_CODE);
                    }
                }
                if (strTextBox == "txtFrom_Place")
                {
                    setFocusControl(txtTo_Place);
                }
                if (strTextBox == "txtTo_Place")
                {
                    setFocusControl(txtQuantal);
                }
                if (strTextBox == "txtQuantal")
                {
                    txtPACKING.Text = "50";
                    setFocusControl(txtPACKING);
                }
                if (strTextBox == "txtPACKING")
                {
                    setFocusControl(txtBAGS);
                }
                if (strTextBox == "txtBAGS")
                {
                    setFocusControl(txtGrade);
                }
                //if (strTextBox == "txtGrade")
                //{
                //    setFocusControl(txtMill_Rate);
                //}
                if (strTextBox == "txtMill_Rate")
                {
                    setFocusControl(txtMill_Amount);
                }
                if (strTextBox == "txtMill_Amount")
                {
                    setFocusControl(txtQuantal1);
                }
                if (strTextBox == "txtQuantal1")
                {
                    txtPACKING1.Text = "50";
                    setFocusControl(txtPACKING1);
                }
                if (strTextBox == "txtPACKING1")
                {
                    setFocusControl(txtBAGS1);
                }
                if (strTextBox == "txtBAGS1")
                {
                    setFocusControl(txtGrade1);
                }
                if (strTextBox == "txtCommission_Rate")
                {
                    setFocusControl(txtInterest);
                }
                if (strTextBox == "txtMill_Rate1")
                {
                    setFocusControl(txtMill_Amount1);
                }
                if (strTextBox == "txtMill_Amount1")
                {
                    setFocusControl(txtSale_Rate);
                }
                if (strTextBox == "txtSale_Rate")
                {
                    setFocusControl(txtFreightPerQtl);
                }
                if (strTextBox == "txtFreightPerQtl")
                {
                    setFocusControl(txtFreightDiff);
                }
                if (strTextBox == "txtFreightDiff")
                {
                    setFocusControl(txtNarration1);
                }
                if (strTextBox == "txtNarration1")
                {
                    setFocusControl(txtNarration2);
                }
                if (strTextBox == "txtBrokrage")
                {
                    setFocusControl(txtService_Charge);
                }
                if (strTextBox == "txtNarration2")
                {
                    setFocusControl(txtNarration3);
                }
                if (strTextBox == "txtService_Charge")
                {
                    setFocusControl(txtL_Rate_Diff);
                }
                if (strTextBox == "txtNarration3")
                {
                    setFocusControl(txtDue_Days);
                }
                if (strTextBox == "txtL_Rate_Diff")
                {
                    setFocusControl(txtCommission_Rate);
                }
                if (strTextBox == "txtDue_Days")
                {
                    setFocusControl(txtBrokrage);
                }
                if (strTextBox == "txtInterest")
                {
                    setFocusControl(txtTransport_Amount);
                }
                if (strTextBox == "txtNarration4")
                {
                    setFocusControl(txtCash_Account);
                }
                if (strTextBox == "txtTransport_Amount")
                {
                    setFocusControl(txtOTHER_Expenses);
                }
                if (strTextBox == "txtOTHER_Expenses")
                {
                    setFocusControl(txtCash_Ac_Amount);
                }
                if (strTextBox == "txtCash_Account")
                {
                    setFocusControl(txtCash_Amount_RATE);
                }
                if (strTextBox == "txtCash_Amount_RATE")
                {
                    setFocusControl(drpRateType);
                }
            }
        }
        catch
        {
        }
    }

    private void AmountCalculation()
    {
        #region [Calculation Part]
        try
        {
            double qtl = Convert.ToDouble("0" + txtQuantal.Text);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
            Int32 bags = 0;
            double saleRate = Convert.ToDouble("0" + txtSale_Rate.Text);
            double millRate = Convert.ToDouble("0" + txtMill_Rate.Text);
            double diffAmt = 0.00;
            double diff = 0.00;

            double qtl1 = Convert.ToDouble("0" + txtQuantal1.Text);
            Int32 packing1 = Convert.ToInt32("0" + txtPACKING.Text);
            Int32 bags1 = 0;
            double millRate1 = Convert.ToDouble("0" + txtMill_Rate1.Text);
            double freightPerQtl = Convert.ToDouble("0" + txtFreightPerQtl.Text);
            double millAmt = 0;
            double millAmt1 = 0;

            double lrateDiff = Convert.ToDouble("0" + txtL_Rate_Diff.Text);
            double rateDiffAmt = txtRATEDIFF.Text != string.Empty ? Convert.ToDouble(txtRATEDIFF.Text) : 0.00;
            rateDiffAmt = Math.Round(((qtl + qtl1) * lrateDiff), 2);
            txtRATEDIFF.Text = rateDiffAmt.ToString();
            double CommissionRate = Convert.ToDouble("0" + txtCommission_Rate.Text);
            double BankCommission = Convert.ToDouble("0" + txtBANK_COMMISSION.Text);
            BankCommission = Math.Round(((qtl + qtl1) * CommissionRate), 2);
            txtBANK_COMMISSION.Text = BankCommission.ToString();
            double cashAmtRate = Convert.ToDouble("0" + txtCash_Amount_RATE.Text);
            double cashAmount = 0.00;

            double brokrage = Convert.ToDouble("0" + txtBrokrage.Text);
            double serviceCharge = Convert.ToDouble("0" + txtService_Charge.Text);
            double interest = Convert.ToDouble("0" + txtInterest.Text);
            double transport = Convert.ToDouble("0" + txtTransport_Amount.Text);
            double other_Expense = Convert.ToDouble("0" + txtOTHER_Expenses.Text);
            double voucherAmount = 0.00;
            //cashAmount = Math.Round(((qtl + qtl1) * cashAmtRate), 2);
            //txtCash_Ac_Amount.Text = cashAmount.ToString();
            if (qtl != 0 && packing != 0)
            {
                bags = Convert.ToInt32((qtl / packing) * 100);
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }

            if (qtl1 != 0 && packing1 != 0)
            {
                bags1 = Convert.ToInt32((qtl1 / packing1) * 100);
                txtBAGS1.Text = bags1.ToString();
            }
            else
            {
                txtBAGS1.Text = bags1.ToString();
            }
            millAmt = (millRate * qtl);
            millAmt1 = (millRate1 * qtl1);
            txtMill_Amount.Text = millAmt.ToString();
            txtMill_Amount1.Text = millAmt1.ToString();

            // double lessDiff = Math.Round((((millAmt + millAmt1)) - ((saleRate * qtl) + (saleRate * qtl1))) - ((freightPerQtl * qtl) + freightPerQtl * qtl1));
            double lessDiff = Math.Round(((saleRate - freightPerQtl) - (millRate + millRate1)) * (qtl + qtl1));
            txtLESSDIFF.Text = lessDiff.ToString();
            txtFreightDiff.Text = txtLESSDIFF.Text;
            double freightDiff = 0.00;
            freightDiff = txtFreightDiff.Text != string.Empty ? Convert.ToDouble(txtFreightDiff.Text) : 0;
            double transportAmount = txtCash_Ac_Amount.Text != string.Empty ? Convert.ToDouble(txtCash_Ac_Amount.Text) : 0.00;
            voucherAmount = Math.Round(millAmt + millAmt1 + brokrage + serviceCharge + interest + transport + rateDiffAmt + BankCommission + other_Expense + cashAmount + freightDiff + transportAmount, 2);
            //if (drpRateType.SelectedValue == "A")
            //{
            //    cashAmount = Math.Round(((qtl + qtl1) * cashAmtRate), 2);
            //    voucherAmount = Math.Round(millAmt + millAmt1 + brokrage + serviceCharge + interest + transport + rateDiffAmt + BankCommission + other_Expense + cashAmount + freightDiff + transportAmount, 2);
            //}
            //if (drpRateType.SelectedValue == "L")
            //{
            //    //double lesdif = txtLESSDIFF.Text != string.Empty ? Convert.ToDouble(txtLESSDIFF.Text) : 0;
            //    //double transamt = txtCash_Ac_Amount.Text != string.Empty ? Convert.ToDouble(txtLESSDIFF.Text) : 0;
            //    txtFreightDiff.Text = Convert.ToString(double.Parse(txtLESSDIFF.Text) - double.Parse(txtCash_Ac_Amount.Text));

            //    freightDiff = txtFreightDiff.Text != string.Empty ? Convert.ToDouble(txtFreightDiff.Text) : 0;

            //    //cashAmount = Math.Round(((qtl + qtl1) * cashAmtRate), 2);
            //    voucherAmount = Math.Round(millAmt + millAmt1 + brokrage + serviceCharge + interest + transport + rateDiffAmt + BankCommission + other_Expense + cashAmount + freightDiff + transportAmount, 2);
            //}
            //   txtLESSDIFF.Text = Math.Round((((qtl1 + qtl) * saleRate) - ((qtl1 + qtl) * freightPerQtl)) - (millAmt + millAmt1), 2).ToString();
            if (saleRate != 0 && millRate != 0)
            {
                diff = saleRate - millRate;
                diffAmt = Math.Round(diff * qtl, 2);
            }
            txtVoucher_Amount.Text = voucherAmount.ToString();
        }
        catch (Exception e)
        {
            throw e;
        }
        #endregion
    }
    #endregion

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }
    protected void drpRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        csCalculations();
        AmountCalculation();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ks", "javascript:p('" + txtDoc_no.Text + "','" + "OV" + "')", true);
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
