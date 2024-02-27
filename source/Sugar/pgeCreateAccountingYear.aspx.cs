using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeCreateAccountingYear : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string qryCommon = string.Empty;
    string qryDisplay = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = "AccountingYear";
        user = Session["user"].ToString();
        if (!IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                clsButtonNavigation.enableDisable("N");
                ViewState["mode"] = "I";
                this.showLastRecord();
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }

        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        //this.makeEmptyForm("A");
        this.getMaxCode();
        txtFromDt.Text = "";
        txtToDt.Text = "";
        txtYear.Text = "";
        txtFromDt.Enabled = true;
        txtToDt.Enabled = true;
    }
    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                obj.code = "yearCode";

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
                                    if (ds.Tables[0].Rows[0][0].ToString() != "1")
                                    {
                                        txtYearCode.Text = ds.Tables[0].Rows[0][0].ToString();
                                        txtYearCode.Enabled = false;
                                    }
                                    else
                                    {
                                        txtYearCode.Text = "";
                                    }
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string strRev = "";
            string yearcode = txtYearCode.Text;
            string Start_Date = DateTime.Parse(txtFromDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string End_Date = DateTime.Parse(txtToDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string year = txtYear.Text;
            int company_code = Convert.ToInt32(Session["Company_Code"].ToString());
            DataSet ds = new DataSet();
            if (ViewState["mode"].ToString() == "I")
            {
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 1;
                    obj.tableName = tblHead;
                    obj.columnNm = "yearCode,Start_Date,End_Date,year,Company_Code";
                    obj.values = "'" + yearcode + "','" + Start_Date + "','" + End_Date + "','" + year + "','" + company_code + "'";
                    ds = obj.insertAccountMaster(ref strRev);
                }
            }
            else
            {
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 2;
                    obj.tableName = tblHead;
                    obj.columnNm = "yearCode='" + yearcode + "',Start_Date='" + Start_Date + "',End_Date='" + End_Date + "',year='" + year + "',Company_Code='" + company_code + "' where Company_Code=" + company_code + "";
                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strRev);
                }
            }
            if (strRev == "-1")
            {
                clsButtonNavigation.enableDisable("S");
                this.enableDisableNavigateButtons();
                hdnf.Value = txtYearCode.Text;
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
            }
            if (strRev == "-2" || strRev == "-3")
            {
                clsButtonNavigation.enableDisable("S");
                this.enableDisableNavigateButtons();
                hdnf.Value = txtYearCode.Text;
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Updated !')", true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        // this.makeEmptyForm("E");
        txtFromDt.Enabled = true;
        txtToDt.Enabled = true;
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
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
        string str = clsCommon.getString("select count(yearCode) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

        if (str != "0")
        {
            clsButtonNavigation.enableDisable("S");
            this.enableDisableNavigateButtons();
            // this.makeEmptyForm("S");
            txtFromDt.Enabled = false;
            txtToDt.Enabled = false;
        }
        else
        {
            clsButtonNavigation.enableDisable("N");
            this.enableDisableNavigateButtons();
            //this.makeEmptyForm("N");

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select yearCode from " + tblHead + " where yearCode=(select MIN(yearCode) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtYearCode.Text != string.Empty)
            {
                string query = "SELECT top 1 [yearCode] from " + tblHead + " where yearCode<" + Convert.ToInt32(hdnf.Value) +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by yearCode desc";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtYearCode.Text != string.Empty)
            {
                string query = "SELECT top 1 [yearCode] from " + tblHead + " where yearCode>" + Convert.ToInt32(hdnf.Value) +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by yearCode asc";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select yearCode from " + tblHead + " where yearCode=(select MAX(yearCode) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
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
                        txtYearCode.Text = dt.Rows[0]["yearCode"].ToString();
                        txtFromDt.Text = DateTime.Parse(dt.Rows[0]["Start_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                        txtToDt.Text = DateTime.Parse(dt.Rows[0]["End_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                        txtYear.Text = dt.Rows[0]["year"].ToString();
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
    protected string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead + " where yearCode=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
                txtYearCode.Text = hdnf.Value;
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
                // this.makeEmptyForm("S");
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

    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = "select max(yearCode) as yearCode from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
                        hdnf.Value = dt.Rows[0]["yearCode"].ToString();
                        qry = getDisplayQuery();
                        bool recordExist = this.fetchRecord(qry);
                        if (recordExist == true)
                        {
                            btnAdd.Focus();
                        }
                        else                     //new code
                        {
                            btnEdit.Enabled = true;
                            btnDelete.Enabled = true;
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

        if (txtYearCode.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [yearCode] from " + tblHead + " where yearCode>" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY yearCode asc  ";
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
                query = "SELECT top 1 [yearCode] from " + tblHead + " where yearCode<" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY yearCode asc  ";
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
    protected void txtFromDt_TextChanged(object sender, EventArgs e)
    {
        string substr = "";
        string date = txtFromDt.Text;
        int index = date.LastIndexOf('/');
        string year = date.Substring(index + 1, 4);
        string predate = txtYear.Text;
        if (!string.IsNullOrEmpty(predate))
        {
            int ix = predate.IndexOf('-');
            if (predate.Length > 5)
            {
                substr = predate.Substring(ix + 1, 4);
            }
        }
        txtYear.Text = year + "-" + substr;
    }
    protected void txtToDt_TextChanged(object sender, EventArgs e)
    {
        string predate = txtYear.Text.ToString();
        string substr = predate.Substring(0, 5);
        string date = txtToDt.Text;
        int index = date.LastIndexOf('/');
        string year = date.Substring(index + 1, 4);
        txtYear.Text = substr + year;
    }
}