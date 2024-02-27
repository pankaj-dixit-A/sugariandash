using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeMotorMemo : System.Web.UI.Page
{
    #region data section
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
    string qryAccountList = string.Empty;
    int defaultAccountCode = 0;
    string trnType = "MM";
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string prememono = "0";
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
            qryCommon = tblPrefix + "qryDeliveryOrderListMM";
            qryAccountList = tblPrefix + "qryAccountsList";
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

                    if (Session["MEMO_NO"] != null)
                    {
                        hdnf.Value = Session["MEMO_NO"].ToString();
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        this.enableDisableNavigateButtons();
                        Session["MEMO_NO"] = null;
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
                obj.tableName = tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
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
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                lblMsg.Text = string.Empty;

                #region Logic
                calenderExtenderDate.Enabled = false;

                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtPartyCode.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtNARRATION1.Enabled = false;
                btntxtNARRATION2.Enabled = false;
                btntxtNARRATION3.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btntxtPurcNo.Enabled = false;
                btnMail.Enabled = true;
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
                lblMsg.Text = "";
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

                #region set Business logic for save
                calenderExtenderDate.Enabled = true;

                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtPartyCode.Enabled = true;
                btntxtGRADE.Enabled = true;

                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtNARRATION1.Enabled = true;
                btntxtNARRATION2.Enabled = true;
                btntxtNARRATION3.Enabled = true;
                btntxtNARRATION4.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                txtdoc_no.Enabled = false;
                btntxtPurcNo.Enabled = true;

                LBLMILL_NAME.Text = string.Empty;
                LBLGETPASS_NAME.Text = string.Empty;
                LBLPARTY_NAME.Text = string.Empty;
                LBLTRANSPORT_NAME.Text = string.Empty;

                lblPurcOrder.Text = "0";
                setFocusControl(txtDOC_DATE);
                btnMail.Enabled = false;
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
                #region Logic
                calenderExtenderDate.Enabled = false;

                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtPartyCode.Enabled = false;
                btntxtGRADE.Enabled = false;

                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtNARRATION1.Enabled = false;
                btntxtNARRATION2.Enabled = false;
                btntxtNARRATION3.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                btntxtPurcNo.Enabled = false;
                btnMail.Enabled = true;
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
                txtdoc_no.Enabled = true;
                #region set Business logic for edit
                calenderExtenderDate.Enabled = true;

                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtPartyCode.Enabled = true;
                btntxtGRADE.Enabled = true;

                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtNARRATION1.Enabled = true;
                btntxtNARRATION2.Enabled = true;
                btntxtNARRATION3.Enabled = true;
                btntxtNARRATION4.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                btntxtPurcNo.Enabled = true;
                btnMail.Enabled = false;

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
            qry = "select max(doc_no) as doc_no from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
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
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "' ORDER BY doc_no asc  ";
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
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "' ORDER BY doc_no asc  ";
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                  " and tran_type='" + trnType + "') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                  " and tran_type='" + trnType + "'";
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
                    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " and tran_type='" + trnType + "'" +
                    " ORDER BY doc_no desc  ";
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
                    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " and tran_type='" + trnType + "'" +
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                  " and tran_type='" + trnType + "') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                " and tran_type='" + trnType + "'";

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

    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        this.makeEmptyForm("E");
        //txtdoc_no.Enabled = false;
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

                //update DO to set memo no=0
                string qry = "";
                qry = "update " + tblPrefix + "deliveryorder set memo_no=0  where tran_type='DO' and purc_no=" + txtPurcNo.Text + " and purc_order=" + int.Parse(lblPurcOrder.Text) + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                ds = clsDAL.SimpleQuery(qry);
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 3;
                    obj.tableName = tblHead;
                    obj.columnNm = "  Tran_Type='" + trnType + "' and Doc_No=" + currentDoc_No +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);

                    //if (strrev == "-3")
                    //{
                    //    //update DO to set memo no=0
                    //    Int32 po = Convert.ToInt32(lblPurcOrder.Text.Trim());
                    //    obj.flag = 2;
                    //    obj.tableName = tblPrefix + "deliveryorder";
                    //    obj.columnNm = "memo_no=0 where tran_type='DO' and purc_no=" + txtPurcNo.Text + " and purc_order=" + po + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    //    obj.values = "none";
                    //    ds = obj.insertAccountMaster(ref strrev);
                    //}

                }
                string query = "";

                if (strrev == "-3")
                {
                    query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No>" + Convert.ToInt32(currentDoc_No) +
                           " and Tran_Type='" + trnType + "'  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
                            " ORDER BY Doc_No asc  ";


                    hdnf.Value = clsCommon.getString(query);

                    if (hdnf.Value == string.Empty)
                    {
                        query = "SELECT top 1 [Doc_No] from " + tblHead + "  where Doc_No<" + Convert.ToInt32(currentDoc_No) +
                             " and Tran_Type='" + trnType + "'  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'" +
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
        string str = clsCommon.getString("select count(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'");

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

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select *,Convert(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int i = 0;
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtMILL_CODE" || v == "txtGETPASS_CODE" || v == "txtvoucher_by" || v == "txtBroker_CODE" || v == "txtDO_CODE" || v == "txtTRANSPORT_CODE" || v == "txtPartyCode")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[2].Width = new Unit("80px");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;

            }
            if (v == "txtdoc_no")
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(15);
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(60);
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            }
            if (v == "txtPurcNo")
            {

                grdPopup.CellSpacing = 10;
                //e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(5);
                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(7);
                //e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                //e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(10);
                //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(5);
                //e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(5);
                //e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
                //e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(10);
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

    #region [txtGETPASS_CODE_TextChanged]
    protected void txtGETPASS_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGETPASS_CODE.Text;
        strTextBox = "txtGETPASS_CODE";
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
        searchString = txtquantal.Text;
        strTextBox = "txtquantal";
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

    protected void txtPartyCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPartyCode.Text;
        strTextBox = "txtPartyCode";
        csCalculations();
    }

    protected void btntxtPartyCode_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPartyCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

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
                    lblPopupHead.Text = "--Select Entry no--";
                    string qry = "select doc_no,Convert(varchar(10),doc_date,103) as Date,quantal,millName from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'" +
                        " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millName like '%" + txtSearchText.Text + "%')" +
                        " group by doc_no,doc_date,quantal,millName";
                    this.showPopup(qry);
                }
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
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                lblPopupHead.Text = "--Select GetpassCode--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " inner join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type!='C' and " +
                    AccountMasterTable + ".Ac_type!='B'" +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPartyCode")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " inner join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + //" and " + AccountMasterTable + ".Ac_type!='C' and " + AccountMasterTable + ".Ac_type!='B' " +
                     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGRADE")
            {
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                lblPopupHead.Text = "--Select transport Code--";
                //string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                //    " inner join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code where " +
                //    AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='T' " +
                //     " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                  " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " +
                  AccountMasterTable + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='T' " +
                   " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION1")
            {
                lblPopupHead.Text = "--Select Narration --";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION2")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION3")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION4")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPurcNo")
            {
                lblPopupHead.Text = "--Select DO No--";
                string qry = "select doc_no,Convert(varchar(10),doc_date,103) as doc_date,millShortName,GetPassName,grade,quantal,bags,DOName,TransportName from " + qryCommon + " where   Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and memo_no=0" +
                     " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millShortName like '%" + txtSearchText.Text + "%' or grade like '%" + txtSearchText.Text + "%')";
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
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                setFocusControl(txtGETPASS_CODE);
            }
            if (hdnfClosePopup.Value == "txtPartyCode")
            {
                setFocusControl(txtPartyCode);
            }

            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                setFocusControl(txtTRANSPORT_CODE);
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
        string qry = "";
        #region [Validation Part]
        bool isValidated = true;
        if (txtdoc_no.Text != string.Empty)
        {
            if (ViewState["mode"] != null)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'");
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
        if (txtDOC_DATE.Text != string.Empty)
        {
            string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (clsCommon.isValidDate(dt) == true)
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
        if (txtGETPASS_CODE.Text != string.Empty)
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
            setFocusControl(txtGETPASS_CODE);
            return;
        }
        if (txtPartyCode.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPartyCode.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPartyCode);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtPartyCode);
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

        if (txtfreightperQtl.Text != string.Empty)
        {
            isValidated = true;

        }
        else
        {
            isValidated = false;
            setFocusControl(txtfreightperQtl);
            return;
        }
        #endregion

        #region -Head part declearation
        Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
        string DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        double MILL_CODE = txtMILL_CODE.Text != string.Empty ? Convert.ToDouble(txtMILL_CODE.Text) : 0;
        double GETPASS_CODE = txtGETPASS_CODE.Text != string.Empty ? Convert.ToDouble(txtGETPASS_CODE.Text) : 2;
        Int32 Ac_Code = txtPartyCode.Text != string.Empty ? Convert.ToInt32(txtPartyCode.Text) : 0;

        string GRADE = txtGRADE.Text;
        double QUANTAL = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.00;
        Int32 PACKING = txtPACKING.Text != string.Empty ? Convert.ToInt32(txtPACKING.Text) : 0;
        Int32 BAGS = txtBAGS.Text != string.Empty ? Convert.ToInt32(txtBAGS.Text) : 0;
        double FreightPerQtl = txtfreightperQtl.Text != string.Empty ? Convert.ToDouble(txtfreightperQtl.Text) : 0.00;
        double Freight_Amount = txtFreightperQtlAmount.Text != string.Empty ? Convert.ToDouble(txtFreightperQtlAmount.Text) : 0.00;
        double Freight_RateMM = txtfreightRate.Text != string.Empty ? Convert.ToDouble(txtfreightRate.Text) : 0.00;
        double Freight_AmountMM = txtfreightAmount.Text != string.Empty ? Convert.ToDouble(txtfreightAmount.Text) : 0.00;
        double final_amout = txtFinalAmount.Text != string.Empty ? Convert.ToDouble(txtFinalAmount.Text) : 0.00;
        double Paid_Rate1 = txtpaid1.Text != string.Empty ? Convert.ToDouble(txtpaid1.Text) : 0.00;
        double Paid_Rate2 = txtpaid2.Text != string.Empty ? Convert.ToDouble(txtpaid2.Text) : 0.00;
        double Paid_Rate3 = txtpaid3.Text != string.Empty ? Convert.ToDouble(txtpaid3.Text) : 0.00;
        double Paid_Amount1 = txtpaidAmount1.Text != string.Empty ? Convert.ToDouble(txtpaidAmount1.Text) : 0.00;
        double Paid_Amount2 = txtpaidAmount2.Text != string.Empty ? Convert.ToDouble(txtpaidAmount2.Text) : 0.00;
        double Paid_Amount3 = txtpaidAmount3.Text != string.Empty ? Convert.ToDouble(txtpaidAmount3.Text) : 0.00;
        string Paid_Narration1 = txtpaidNarration1.Text;
        string Paid_Narration2 = txtpaidNarration2.Text;
        string Paid_Narration3 = txtpaidNarration3.Text;

        double vasuli_rate = txtVasuliRate.Text != string.Empty ? Convert.ToDouble(txtVasuliRate.Text) : 0.00;
        double vasuli_amount = txtVasuliAmount.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount.Text) : 0.00;
        double less_amount = txtLess.Text != string.Empty ? Convert.ToDouble(txtLess.Text) : 0.00;

        string TRUCK_NO = txtTruck_NO.Text;
        int TRANSPORT_CODE = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
        string NARRATION1 = txtNARRATION1.Text;
        string NARRATION2 = txtNARRATION2.Text;
        string NARRATION3 = txtNARRATION3.Text;
        string NARRATION4 = txtNARRATION4.Text;
        int purc_no = txtPurcNo.Text != string.Empty ? Convert.ToInt32(txtPurcNo.Text) : 0;
        int purc_order = lblPurcOrder.Text != string.Empty ? Convert.ToInt32(lblPurcOrder.Text) : 0;
        string MobileNo = txtMobile.Text;
        string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");

        string retValue = string.Empty;
        string strRev = string.Empty;
        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int year_Code = Convert.ToInt32(Session["year"].ToString());
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        #endregion-End of Head part declearation

        #region save Head Master
        if (isValidated == true)
        {
            using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
            {
                if (ViewState["mode"] != null)
                {
                    DataSet ds = new DataSet();
                    if (ViewState["mode"].ToString() == "I")
                    {
                        obj.flag = 1;
                        obj.tableName = tblHead;
                        obj.columnNm = "tran_type,DOC_NO,DOC_DATE,MILL_CODE,GETPASSCODE,GRADE,QUANTAL,PACKING,BAGS,TRUCK_NO,transport,NARRATION1,NARRATION2,NARRATION3,NARRATION4,company_code,Year_Code,Branch_Code,purc_no,purc_order,Ac_Code,FreightPerQtl,Freight_Amount,Freight_RateMM,Freight_AmountMM,Paid_Rate1,Paid_Amount1,Paid_Narration1,Paid_Rate2,Paid_Amount2,Paid_Narration2,Paid_Rate3,Paid_Amount3,Paid_Narration3,vasuli_rate,vasuli_amount,less,driver_no,Created_By,final_amout";
                        obj.values = "'" + trnType + "','" + DOC_NO + "','" + DOC_DATE + "','" + MILL_CODE + "','" + GETPASS_CODE + "','" + GRADE + "','" + QUANTAL + "','" + PACKING + "','" + BAGS + "','" + TRUCK_NO + "','" + TRANSPORT_CODE + "','" + NARRATION1 + "','" + NARRATION2 + "','" + NARRATION3 + "','" + NARRATION4 + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + purc_no + "','" + purc_order + "','" + Ac_Code + "','" + FreightPerQtl + "','" + Freight_Amount + "','" + Freight_RateMM + "','" + Freight_AmountMM + "','" + Paid_Rate1 + "','" + Paid_Amount1 + "','" + Paid_Narration1 + "','" + Paid_Rate2 + "','" + Paid_Amount2 + "','" + Paid_Narration2 + "','" + Paid_Rate3 + "','" + Paid_Amount3 + "','" + Paid_Narration3 + "','" + vasuli_rate + "','" + vasuli_amount + "','" + less_amount + "','" + MobileNo + "','" + user + "','" + final_amout + "'";
                        ds = obj.insertAccountMaster(ref strRev);
                        retValue = strRev;

                        //update entry of delivery order with memo number
                        qry = "update " + tblHead + " set memo_no=" + txtdoc_no.Text + " where tran_type='DO' and doc_no=" + txtPurcNo.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);

                        //update entry of delivery order with freight

                        qry = "update " + tblHead + " set Freight_Amount=" + txtFreightperQtlAmount.Text + " where tran_type='DO' and doc_no=" + txtPurcNo.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);

                        qry = "update " + tblHead + " set FreightPerQtl=" + txtfreightperQtl.Text + " where tran_type='DO' and doc_no=" + txtPurcNo.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);
                    }
                    else
                    {
                        //Update Mode
                        obj.flag = 2;
                        obj.tableName = tblHead;
                        obj.columnNm = "DOC_DATE='" + DOC_DATE + "',MILL_CODE='" + MILL_CODE + "',GETPASSCODE='" + GETPASS_CODE +
                            "',Ac_Code='" + Ac_Code + "',GRADE='" + GRADE + "',QUANTAL='" + QUANTAL +
                            "',PACKING='" + PACKING + "',BAGS='" + BAGS + "',TRUCK_NO='" +
                            TRUCK_NO + "',TRANSPORT='" + TRANSPORT_CODE + "',NARRATION1='" + NARRATION1 +
                            "',NARRATION2='" + NARRATION2 + "',NARRATION3='" + NARRATION3 + "',NARRATION4='" +
                            NARRATION4 + "',purc_no='" + purc_no + "',purc_order='" + purc_order + "'" +
                            ",FreightPerQtl='" + FreightPerQtl + "',Freight_Amount='" + Freight_Amount + "',Freight_RateMM='" + Freight_RateMM + "',Freight_AmountMM='" + Freight_AmountMM + "',Paid_Rate1='" + Paid_Rate1 + "',Paid_Amount1='" + Paid_Amount1 + "',Paid_Narration1='" + Paid_Narration1 + "',Paid_Rate2='" + Paid_Rate2 +
                            "',Paid_Amount2='" + Paid_Amount2 + "',Paid_Narration2='" + Paid_Narration2 + "',Paid_Rate3='" + Paid_Rate3 + "',Paid_Amount3='" + Paid_Amount3 + "',Paid_Narration3='" + Paid_Narration3 + "',vasuli_rate='" + vasuli_rate + "',vasuli_amount='" + vasuli_amount + "',less='" + less_amount + "',driver_no='" + MobileNo + "',Modified_By='" + user + "',final_amout='" + final_amout + "'" +
                            " where DOC_NO='" + DOC_NO + "' and company_code='" + Company_Code + "' and Year_Code='" + year_Code + "'  and tran_type='" + trnType + "'";
                        obj.values = "none";
                        ds = new DataSet();
                        ds = obj.insertAccountMaster(ref strRev);

                        retValue = strRev;

                        //update entry of delivert order with memo number
                        qry = "update " + tblHead + " set memo_no=" + txtdoc_no.Text + " where tran_type='DO' and doc_no=" + txtPurcNo.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);

                        //update entry of delivery order with freight

                        qry = "update " + tblHead + " set Freight_Amount=" + txtFreightperQtlAmount.Text + " where tran_type='DO' and doc_no=" + txtPurcNo.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);

                        qry = "update " + tblHead + " set FreightPerQtl=" + txtfreightperQtl.Text + " where tran_type='DO' and doc_no=" + txtPurcNo.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                        ds = clsDAL.SimpleQuery(qry);
                    }

                    hdnf.Value = txtdoc_no.Text;
                    if (retValue == "-1")
                    {
                        clsButtonNavigation.enableDisable("S");
                        this.enableDisableNavigateButtons();
                        this.makeEmptyForm("S");
                        hdnf.Value = txtdoc_no.Text;
                        qry = getDisplayQuery();
                        this.fetchRecord(qry);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);
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
        }
        #endregion
    }
    #endregion

    protected void txtPurcNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurcNo.Text;
        strTextBox = "txtPurcNo";
        csCalculations();
    }
    protected void btntxtPurcNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurcNo";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtfreightRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtfreightRate.Text;
        strTextBox = "txtfreightRate";
        csCalculations();
    }

    protected void txtpaid1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpaid1.Text;
        strTextBox = "txtpaid1";
        csCalculations();
    }

    protected void txtpaid2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpaid2.Text;
        strTextBox = "txtpaid2";
        csCalculations();
    }

    protected void txtpaid3_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpaid3.Text;
        strTextBox = "txtpaid3";
        csCalculations();
    }

    protected void txtVasuliRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVasuliRate.Text;
        strTextBox = "txtVasuliRate";
        csCalculations();
    }

    protected void txtLess_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLess.Text;
        strTextBox = "txtLess";
        csCalculations();
    }

    protected void txtMobile_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMobile.Text;
        strTextBox = "txtMobile";
        csCalculations();
    }

    protected void txtpaidNarration1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpaidNarration1.Text;
        strTextBox = "txtpaidNarration1";
        csCalculations();
    }

    protected void txtpaidNarration2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpaidNarration2.Text;
        strTextBox = "txtpaidNarration2";
        csCalculations();
    }

    protected void txtpaidNarration3_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpaidNarration3.Text;
        strTextBox = "txtpaidNarration3";
        csCalculations();
    }

    protected void txtfreightperQtl_TextChanged(object sender, EventArgs e)
    {
        searchString = txtfreightperQtl.Text;
        strTextBox = "txtfreightperQtl";
        csCalculations();
    }

    protected void txtPartyEmail_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPartyEmail.Text;
        strTextBox = "txtPartyEmail";
        csCalculations();
    }

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
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        LBLMILL_NAME.Text = dt.Rows[0]["millName"].ToString();
                        txtGETPASS_CODE.Text = dt.Rows[0]["GETPASSCODE"].ToString();
                        LBLGETPASS_NAME.Text = dt.Rows[0]["GetPassName"].ToString();
                        txtPartyCode.Text = dt.Rows[0]["Ac_Code"].ToString();
                        LBLPARTY_NAME.Text = dt.Rows[0]["PartyName"].ToString();
                        txtGRADE.Text = dt.Rows[0]["GRADE"].ToString();
                        txtquantal.Text = dt.Rows[0]["QUANTAL"].ToString();
                        txtPACKING.Text = dt.Rows[0]["PACKING"].ToString();
                        txtBAGS.Text = dt.Rows[0]["BAGS"].ToString();
                        txtTruck_NO.Text = dt.Rows[0]["TRUCK_NO"].ToString();
                        txtTRANSPORT_CODE.Text = dt.Rows[0]["TRANSPORT"].ToString();
                        LBLTRANSPORT_NAME.Text = dt.Rows[0]["TransportName"].ToString();
                        txtNARRATION1.Text = dt.Rows[0]["NARRATION1"].ToString();
                        txtNARRATION2.Text = dt.Rows[0]["NARRATION2"].ToString();
                        txtNARRATION3.Text = dt.Rows[0]["NARRATION3"].ToString();
                        txtNARRATION4.Text = dt.Rows[0]["NARRATION4"].ToString();
                        txtPurcNo.Text = dt.Rows[0]["purc_no"].ToString();
                        lblPurcOrder.Text = dt.Rows[0]["purc_order"].ToString();
                        txtVasuliRate.Text = dt.Rows[0]["vasuli_rate"].ToString();
                        txtVasuliAmount.Text = dt.Rows[0]["vasuli_amount"].ToString();
                        txtLess.Text = dt.Rows[0]["less"].ToString();
                        txtMobile.Text = dt.Rows[0]["driver_no"].ToString();
                        txtfreightperQtl.Text = dt.Rows[0]["FreightPerQtl"].ToString();
                        txtFreightperQtlAmount.Text = dt.Rows[0]["Freight_Amount"].ToString();
                        txtfreightRate.Text = dt.Rows[0]["Freight_RateMM"].ToString();
                        txtfreightAmount.Text = dt.Rows[0]["Freight_AmountMM"].ToString();
                        txtpaid1.Text = dt.Rows[0]["Paid_Rate1"].ToString();
                        txtpaid2.Text = dt.Rows[0]["Paid_Rate2"].ToString();
                        txtpaid3.Text = dt.Rows[0]["Paid_Rate3"].ToString();
                        txtpaidAmount1.Text = dt.Rows[0]["Paid_Amount1"].ToString();
                        txtpaidAmount2.Text = dt.Rows[0]["Paid_Amount2"].ToString();
                        txtpaidAmount3.Text = dt.Rows[0]["Paid_Amount3"].ToString();
                        txtpaidNarration1.Text = dt.Rows[0]["Paid_Narration1"].ToString();
                        txtpaidNarration2.Text = dt.Rows[0]["Paid_Narration2"].ToString();
                        txtpaidNarration3.Text = dt.Rows[0]["Paid_Narration3"].ToString();
                        txtFinalAmount.Text = dt.Rows[0]["final_amount"].ToString();
                        txtPartyEmail.Text = dt.Rows[0]["partyEmail"].ToString();
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
                        hdnf.Value = txtdoc_no.Text;
                        recordExist = true;
                        lblMsg.Text = "";
                    }
                }
            }
            // this.enableDisableNavigateButtons();
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtPartyEmail")
            {
                setFocusControl(txtGETPASS_CODE);
            }

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
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";

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
            if (strTextBox == "txtDOC_DATE")
            {
                if (txtDOC_DATE.Text != string.Empty)
                {
                    try
                    {
                        string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                        if (clsCommon.isValidDate(dt) == true)
                        {
                            setFocusControl(txtPurcNo);
                        }
                        else
                        {
                            txtDOC_DATE.Text = string.Empty;
                            setFocusControl(txtDOC_DATE);
                        }
                    }
                    catch
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
                            LBLMILL_NAME.Text = millName;
                            setFocusControl(txtPurcNo);
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
                    setFocusControl(txtMILL_CODE);
                }
            }
            if (strTextBox == "txtPurcNo")
            {
                if (txtPurcNo.Text != string.Empty)
                {
                    string qry = "";
                    qry = "select * from " + qryCommon + " where doc_no=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO'";
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
                                txtMILL_CODE.Text = dt.Rows[0]["mill_code"].ToString();
                                LBLMILL_NAME.Text = dt.Rows[0]["millName"].ToString();
                                txtPartyCode.Text = dt.Rows[0]["voucher_by"].ToString();
                                LBLPARTY_NAME.Text = dt.Rows[0]["VoucherByname"].ToString();
                                txtGETPASS_CODE.Text = dt.Rows[0]["GETPASSCODE"].ToString();
                                LBLGETPASS_NAME.Text = dt.Rows[0]["GetPassName"].ToString();
                                txtGRADE.Text = dt.Rows[0]["grade"].ToString();
                                txtquantal.Text = dt.Rows[0]["quantal"].ToString();
                                txtPACKING.Text = dt.Rows[0]["packing"].ToString();
                                txtBAGS.Text = dt.Rows[0]["bags"].ToString();
                                txtTRANSPORT_CODE.Text = dt.Rows[0]["transport"].ToString();
                                LBLTRANSPORT_NAME.Text = dt.Rows[0]["TransportName"].ToString();
                                txtTruck_NO.Text = dt.Rows[0]["truck_no"].ToString();
                                txtPartyEmail.Text = dt.Rows[0]["partyEmail"].ToString();
                                setFocusControl(txtPartyCode);
                            }
                            else
                            {
                                setFocusControl(txtPurcNo);
                            }
                        }
                        else
                        {
                            setFocusControl(txtPurcNo);
                        }
                    }
                    else
                    {
                        setFocusControl(txtPurcNo);
                    }
                }
            }
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
                            setFocusControl(txtGRADE);
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
                    LBLGETPASS_NAME.Text = string.Empty;
                    setFocusControl(txtGETPASS_CODE);
                }
            }
            if (strTextBox == "txtPartyCode")
            {
                string party = "";
                if (txtPartyCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtPartyCode.Text);
                    if (a == false)
                    {
                        btntxtPartyCode_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        party = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPartyCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (party != string.Empty)
                        {

                            LBLPARTY_NAME.Text = party;

                            txtPartyEmail.Text = clsCommon.getString("select Email_Id from " + qryAccountList + " where Ac_Code=" + txtPartyCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            setFocusControl(txtPartyEmail);
                        }
                        else
                        {
                            txtPartyCode.Text = string.Empty;
                            LBLPARTY_NAME.Text = party;
                            setFocusControl(txtPartyCode);
                        }
                    }
                }
                else
                {
                    LBLPARTY_NAME.Text = string.Empty;
                    setFocusControl(txtPartyCode);
                }
            }
            if (strTextBox == "txtGRADE")
            {
                setFocusControl(txtquantal);
            }
            if (strTextBox == "txtquantal")
            {
                txtPACKING.Text = "50";
                setFocusControl(txtPACKING);
            }
            if (strTextBox == "txtPACKING")
            {
                setFocusControl(txtfreightperQtl);
            }

            if (strTextBox == "txtfreightperQtl")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (strTextBox == "txtLess")
            {
                setFocusControl(txtVasuliRate);
            }
            if (strTextBox == "txtVasuliRate")
            {
                setFocusControl(txtTruck_NO);
            }
            if (strTextBox == "txtTruck_NO")
            {
                setFocusControl(txtMobile);
            }
            if (strTextBox == "txtMobile")
            {
                setFocusControl(txtNARRATION1);
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

                            LBLTRANSPORT_NAME.Text = transportName;
                            setFocusControl(txtLess);
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
                setFocusControl(txtfreightRate);
            }
            if (strTextBox == "txtfreightRate")
            {
                setFocusControl(txtpaid1);
            }
            if (strTextBox == "txtpaid1")
            {
                setFocusControl(txtpaidNarration1);
            }
            if (strTextBox == "txtpaidNarration1")
            {
                setFocusControl(txtpaid2);
            }
            if (strTextBox == "txtpaid2")
            {
                setFocusControl(txtpaidNarration2);
            }
            if (strTextBox == "txtpaidNarration2")
            {
                setFocusControl(txtpaid3);
            }
            if (strTextBox == "txtpaid3")
            {
                setFocusControl(txtpaidNarration3);
            }
            if (strTextBox == "txtpaidNarration3")
            {
                setFocusControl(btnSave);
            }
            #region [Calculation Part]

            double qtl = Convert.ToDouble("0" + txtquantal.Text);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
            Int32 bags = 0;
            double freightperQtl = Convert.ToDouble("0" + txtfreightperQtl.Text);
            double vasulirate = Convert.ToDouble("0" + txtVasuliRate.Text);
            double freightrate = Convert.ToDouble("0" + txtfreightRate.Text);
            double freightperQtlAmount = 0.00;
            double freightAmt = 0.00;
            double vasuliAmt = 0.00;
            double lessAmt = Convert.ToDouble("0" + txtLess.Text);
            double paidrate1 = Convert.ToDouble("0" + txtpaid1.Text);
            double paidrate2 = Convert.ToDouble("0" + txtpaid2.Text);
            double paidrate3 = Convert.ToDouble("0" + txtpaid3.Text);

            double paidAmount1 = 0.00;
            double paidAmount2 = 0.00;
            double paidAmount3 = 0.00;

            double finalAmount = 0.00;

            if (qtl != 0 && packing != 0)
            {
                bags = Convert.ToInt32((qtl / packing) * 100);
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }

            freightperQtlAmount = Math.Round((freightperQtl * qtl), 2);
            txtFreightperQtlAmount.Text = freightperQtlAmount.ToString();

            finalAmount = Math.Round((freightperQtlAmount - lessAmt), 2);
            txtFinalAmount.Text = finalAmount.ToString();

            vasuliAmt = Math.Round((vasulirate * qtl), 2);
            txtVasuliAmount.Text = vasuliAmt.ToString();

            freightAmt = Math.Round((freightrate * qtl), 2);
            txtfreightAmount.Text = freightAmt.ToString();

            paidAmount1 = Math.Round((paidrate1 * qtl), 2);
            txtpaidAmount1.Text = paidAmount1.ToString();

            paidAmount2 = Math.Round((paidrate2 * qtl), 2);
            txtpaidAmount2.Text = paidAmount2.ToString();

            paidAmount3 = Math.Round((paidrate3 * qtl), 2);
            txtpaidAmount3.Text = paidAmount3.ToString();

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


    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            string partyEmail = txtPartyEmail.Text;
            string do_no = txtdoc_no.Text;
            if (do_no != string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + do_no + "','" + partyEmail + "')", true);
            }
        }
        catch
        {

        }
    }


}