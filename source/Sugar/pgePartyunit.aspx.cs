using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class pgePartyunit : System.Web.UI.Page
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
    string accountmasterlist = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "PartyUnit";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = tblPrefix + "qryPartyUnitlist";
            pnlPopup.Style["display"] = "none";
            accountmasterlist = tblPrefix + "qryAccountsList";
            cityMasterTable = tblPrefix + "CityMaster";
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
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "unit_code";
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
                // btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;

                #region logic
                lblParty_name.Text = string.Empty;
                btntxtac_code.Enabled = false;
                lblCityName.Text = string.Empty;
                btntxtcity_code.Enabled = false;
                lblUnitName.Text = string.Empty;
                btntxtunit_name.Enabled = false;
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
                #region set Business logic for save

                #region logic
                lblParty_name.Text = string.Empty;
                btntxtac_code.Enabled = true;
                lblCityName.Text = string.Empty;
                btntxtcity_code.Enabled = true;
                lblUnitName.Text = string.Empty;
                btntxtunit_name.Enabled = true;
                #endregion

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
                #region logic

                btntxtac_code.Enabled = false;
                btntxtcity_code.Enabled = false;
                btntxtunit_name.Enabled = false;
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

                #region logic

                btntxtac_code.Enabled = true;
                btntxtcity_code.Enabled = true;
                btntxtunit_name.Enabled = true;
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
            qry = "select max(unit_code) as doc_no from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
        query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
                query = "SELECT top 1 [unit_code] from " + tblHead + " where unit_code>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY unit_code asc  ";
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
                query = "SELECT top 1 [unit_code] from " + tblHead + " where unit_code<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY unit_code asc  ";
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
            query = "select unit_code from " + tblHead + " where unit_code=(select MIN(unit_code) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
                string query = "SELECT top 1 [unit_code] from " + tblHead + " where unit_code<" + Convert.ToInt32(hdnf.Value) +
                    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                    " ORDER BY unit_code desc  ";
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
                string query = "SELECT top 1 [unit_code] from " + tblHead + " where unit_code>" + Convert.ToInt32(hdnf.Value) +
                    "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                     " ORDER BY unit_code asc  ";
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
            query = "select unit_code from " + tblHead + " where unit_code=(select MAX(unit_code) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ")  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
        txtdoc_no.Enabled = false;
        setFocusControl(txtac_code);
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        //pnlgrdDetail.Enabled = true;
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
                string strrev = "";
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    obj.flag = 3;
                    obj.tableName = tblHead;
                    obj.columnNm = " unit_code=" + currentDoc_No +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                    obj.values = "none";
                    ds = obj.insertAccountMaster(ref strrev);

                }
                string query = "";

                if (strrev == "-3")
                {
                    query = "SELECT top 1 [unit_code] from " + tblHead + "  where unit_code>" + Convert.ToInt32(currentDoc_No) +
                           "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                            " ORDER BY unit_code asc  ";


                    hdnf.Value = clsCommon.getString(query);

                    if (hdnf.Value == string.Empty)
                    {
                        query = "SELECT top 1 [unit_code] from " + tblHead + "  where unit_code<" + Convert.ToInt32(currentDoc_No) +
                           "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                            " ORDER BY unit_code desc  ";
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
                        txtdoc_no.Text = dt.Rows[0]["unit_code"].ToString();
                        txtac_code.Text = dt.Rows[0]["AC_CODE"].ToString();
                        lblParty_name.Text = dt.Rows[0]["partyName"].ToString();
                        txtunit_name.Text = dt.Rows[0]["Unit_name"].ToString();
                        lblUnitName.Text = dt.Rows[0]["UnitName"].ToString();
                        txtunit_address.Text = dt.Rows[0]["UNIT_ADDRESS"].ToString();
                        txtcity_code.Text = dt.Rows[0]["CITY_CODE"].ToString();
                        txtvat_no.Text = dt.Rows[0]["VAT_NO"].ToString();
                        txtecc_no.Text = dt.Rows[0]["ECC_NO"].ToString();
                        txtperson1.Text = dt.Rows[0]["PERSON1"].ToString();
                        txtperson1_mobile.Text = dt.Rows[0]["PERSON1_MOBILE"].ToString();
                        txtperson2.Text = dt.Rows[0]["PERSON2"].ToString();
                        txtperson2_mobile.Text = dt.Rows[0]["Person2_mobile"].ToString();
                        txtremarks.Text = dt.Rows[0]["REMARKS"].ToString();
                        lblCityName.Text = dt.Rows[0]["unitCity"].ToString();
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
                        //pnlgrdDetail.Enabled = false;
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

                            string qry = "select * from " + tblHead + " where  unit_code='" + txtValue + "' " +
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
                                        hdnf.Value = dt.Rows[0]["unit_code"].ToString();

                                        if (ViewState["mode"] != null)
                                        {
                                            if (ViewState["mode"].ToString() == "I")
                                            {
                                                lblMsg.Text = "** unit_code (" + txtValue + ") Already Exist";
                                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                                this.getMaxCode();
                                                //txtdoc_no.Enabled = false;
                                                //  hdnf.Value = txtdoc_no.Text;
                                                btnSave.Enabled = true;   //IMP                                       
                                                setFocusControl(txtac_code);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtdoc_no.Enabled = false;
                                                    setFocusControl(txtac_code);
                                                    hdnf.Value = txtdoc_no.Text;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtac_code);
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
            if (strTextBox == "txtac_code")
            {
                string acname = "";
                if (txtac_code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtac_code.Text);
                    if (a == false)
                    {
                        btntxtac_code_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where carporate_party='Y' and Ac_Code=" + txtac_code.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty)
                        {

                            lblParty_name.Text = acname;
                            setFocusControl(txtunit_name);
                        }
                        else
                        {
                            txtac_code.Text = string.Empty;
                            lblParty_name.Text = acname;
                            setFocusControl(txtac_code);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtac_code);
                }
            }
            if (strTextBox == "txtPartycode")
            {

                string acname = "";
                if (txtPartycode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtPartycode.Text);
                    if (a == false)
                    {
                        btnGetparty_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where carporate_party='Y' and Ac_Code=" + txtac_code.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty)
                        {

                            lblpartyname.Text = acname;
                        }
                        else
                        {
                            txtPartycode.Text = string.Empty;
                            lblpartyname.Text = acname;
                        }
                    }
                }
                else
                {
                    setFocusControl(txtPartycode);
                }
            }

            if (strTextBox == "txtunit_name")
            {
                string acname = "";
                if (txtunit_name.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtunit_name.Text);
                    if (a == false)
                    {
                        btntxtunit_name_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtunit_name.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty)
                        {

                            lblUnitName.Text = acname;
                            setFocusControl(txtremarks);
                        }
                        else
                        {
                            txtunit_name.Text = string.Empty;
                            lblUnitName.Text = acname;
                            setFocusControl(txtunit_name);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtunit_name);
                }
            }
            if (strTextBox == "txtunit_address")
            {
                setFocusControl(txtcity_code);
            }
            if (strTextBox == "txtcity_code")
            {
                if (txtcity_code.Text != string.Empty)
                {
                    string cityName = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code=" + txtcity_code.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (cityName != string.Empty)
                    {
                        lblCityName.Text = cityName;
                        setFocusControl(txtvat_no);
                    }
                    else
                    {
                        txtcity_code.Text = string.Empty;
                        lblCityName.Text = string.Empty;
                        setFocusControl(txtcity_code);
                    }

                }
                else
                {

                }
            }
            if (strTextBox == "txtvat_no")
            {
                setFocusControl(txtecc_no);
            }
            if (strTextBox == "txtecc_no")
            {
                setFocusControl(txtperson1);
            }
            if (strTextBox == "txtperson1")
            {
                setFocusControl(txtperson1_mobile);
            }
            if (strTextBox == "txtperson1_mobile")
            {
                setFocusControl(txtperson2);
            }
            if (strTextBox == "txtperson2")
            {
                setFocusControl(txtperson2_mobile);
            }
            if (strTextBox == "txtperson2_mobile")
            {
                setFocusControl(txtremarks);
            }
            if (strTextBox == "txtremarks")
            {
                setFocusControl(btnSave);
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
            string qryDisplay = "select * from " + qryCommon + " where unit_code=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            if (v != "txtcity_code")
            {
                e.Row.Cells[2].Width = new Unit("80px");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
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

    #region [txtac_code_TextChanged]
    protected void txtac_code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtac_code.Text;
        strTextBox = "txtac_code";
        csCalculations();
    }
    #endregion


    #region [btntxtac_code_Click]
    protected void btntxtac_code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtac_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtcity_code_Click]
    protected void btntxtcity_code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtcity_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtunit_name_Click]
    protected void btntxtunit_name_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtunit_name";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtunit_name_TextChanged]
    protected void txtunit_name_TextChanged(object sender, EventArgs e)
    {
        searchString = txtunit_name.Text;
        strTextBox = "txtunit_name";
        csCalculations();
    }
    #endregion

    #region [txtunit_address_TextChanged]
    protected void txtunit_address_TextChanged(object sender, EventArgs e)
    {
        searchString = txtunit_address.Text;
        strTextBox = "txtunit_address";
        csCalculations();
    }
    #endregion

    #region [txtcity_code_TextChanged]
    protected void txtcity_code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcity_code.Text;
        strTextBox = "txtcity_code";
        csCalculations();
    }
    #endregion

    #region [txtvat_no_TextChanged]
    protected void txtvat_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtvat_no.Text;
        strTextBox = "txtvat_no";
        csCalculations();
    }
    #endregion

    #region [txtecc_no_TextChanged]
    protected void txtecc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtecc_no.Text;
        strTextBox = "txtecc_no";
        csCalculations();
    }
    #endregion

    #region [txtperson1_TextChanged]
    protected void txtperson1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtperson1.Text;
        strTextBox = "txtperson1";
        csCalculations();
    }
    #endregion

    #region [txtperson1_mobile_TextChanged]
    protected void txtperson1_mobile_TextChanged(object sender, EventArgs e)
    {
        searchString = txtperson1_mobile.Text;
        strTextBox = "txtperson1_mobile";
        csCalculations();
    }
    #endregion

    #region [txtperson2_TextChanged]
    protected void txtperson2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtperson2.Text;
        strTextBox = "txtperson2";
        csCalculations();
    }
    #endregion

    #region [txtperson2_mobile_TextChanged]
    protected void txtperson2_mobile_TextChanged(object sender, EventArgs e)
    {
        searchString = txtperson2_mobile.Text;
        strTextBox = "txtperson2_mobile";
        csCalculations();
    }
    #endregion

    #region [txtremarks_TextChanged]
    protected void txtremarks_TextChanged(object sender, EventArgs e)
    {
        searchString = txtremarks.Text;
        strTextBox = "txtremarks";
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

                if (btntxtdoc_no.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select Unit--";
                    string qry = "select unit_code as Doc_No,Ac_Code as Party_Code,partyName as Party,UnitName as Unit from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and (unit_code like '%" + txtSearchText.Text + "%' or Unit_name like '%" + txtSearchText.Text + "%' or Ac_Code like '%" + txtSearchText.Text + "%' )";
                    this.showPopup(qry);
                }

            }
            if (hdnfClosePopup.Value == "txtac_code")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + accountmasterlist + " where carporate_party='Y' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' ) order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPartycode")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + accountmasterlist + " where carporate_party='Y' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' ) order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtcity_code")
            {
                lblPopupHead.Text = "--Select City--";
                string qry = "select city_code,city_name_e as CityName from " + cityMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (city_code like '%" + txtSearchText.Text + "%' or city_name_e like '%" + txtSearchText.Text + "%'  )  order by city_name_e";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtunit_name")
            {
                lblPopupHead.Text = "--Select Unit--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from  " + accountmasterlist + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
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
            if (hdnfClosePopup.Value == "txtac_code")
            {
                setFocusControl(txtac_code);
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
                    string str = clsCommon.getString("select Unit_name from " + tblHead + " where unit_code=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
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
        }

        if (txtac_code.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where carporate_party='Y' and Ac_Code=" + txtac_code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtac_code);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtac_code);
            return;
        }

        if (txtunit_name.Text != string.Empty)
        {
            string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtunit_name.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtunit_name);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtunit_name);
            return;
        }
        //if (txtunit_address.Text != string.Empty)
        //{
        //    isValidated = true;
        //}
        //else
        //{
        //    isValidated = false;
        //    setFocusControl(txtunit_address);
        //    return;
        //}
        //if (txtcity_code.Text != string.Empty)
        //{
        //    isValidated = true;
        //}
        //else
        //{
        //    isValidated = false;
        //    setFocusControl(txtcity_code);
        //    return;
        //}
        #endregion

        #region -Head part declearation
        Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
        Int32 AC_CODE = txtac_code.Text != string.Empty ? Convert.ToInt32(txtac_code.Text) : 0;
        string UNIT_NAME = txtunit_name.Text;
        string UNIT_ADDRESS = "NULL"; //txtunit_address.Text;
        Int32 CITY_CODE = 0; //txtcity_code.Text != string.Empty ? Convert.ToInt32(txtcity_code.Text) : 0;
        string VAT_NO = "NULL";// txtvat_no.Text;
        string ECC_NO = "NULL";// txtecc_no.Text;
        string PERSON1 = "NULL";// txtperson1.Text;
        string PERSON1_MOBILE = "NULL"; //txtperson1_mobile.Text;
        string PERSON2 = "NULL";// txtperson2.Text;
        string PERSON2_MOBILE = "NULL"; //txtperson2_mobile.Text;
        string REMARKS = txtremarks.Text;

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
                    obj.columnNm = "unit_code,AC_CODE,UNIT_NAME,UNIT_ADDRESS,CITY_CODE,VAT_NO,ECC_NO,PERSON1,PERSON1_MOBILE,PERSON2,PERSON2_MOBILE,REMARKS,Company_Code,Year_Code,Branch_Code,Created_By";
                    obj.values = "'" + DOC_NO + "','" + AC_CODE + "','" + UNIT_NAME + "','" + UNIT_ADDRESS + "','" + CITY_CODE + "','" + VAT_NO + "','" + ECC_NO + "','" + PERSON1 + "','" + PERSON1_MOBILE + "','" + PERSON2 + "','" + PERSON2_MOBILE + "','" + REMARKS + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "'";
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }
                else
                {
                    //Update Mode
                    obj.flag = 2;
                    obj.tableName = tblHead;
                    obj.columnNm = "AC_CODE='" + AC_CODE + "',UNIT_NAME='" + UNIT_NAME + "',UNIT_ADDRESS='" + UNIT_ADDRESS + "',CITY_CODE='" + CITY_CODE + "',VAT_NO='" + VAT_NO + "',ECC_NO='" + ECC_NO + "',PERSON1='" + PERSON1 + "',PERSON1_MOBILE='" + PERSON1_MOBILE + "',PERSON2='" + PERSON2 + "',PERSON2_MOBILE='" + PERSON2_MOBILE + "',REMARKS='" + REMARKS + "',Modified_By='" + user + "' where unit_code='" + DOC_NO + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' ";
                    obj.values = "none";
                    ds = new DataSet();
                    ds = obj.insertAccountMaster(ref strRev);
                    retValue = strRev;
                }


                if (retValue == "-1")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
                    hdnf.Value = txtdoc_no.Text;
                    string qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);

                }
                if (retValue == "-2" || retValue == "-3")
                {
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    this.makeEmptyForm("S");
                    hdnf.Value = txtdoc_no.Text;
                    string qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Updated !');", true);
                }
            }
        }
        #endregion
    }
    #endregion

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        try
        {
            string partycode = txtPartycode.Text;
            if (string.IsNullOrEmpty(txtPartycode.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "au", "javascript:au()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "pd", "javascript:pd('" + partycode + "')", true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void txtPartycode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPartycode.Text;
        strTextBox = "txtPartycode";
        csCalculations();
    }
    protected void btnGetparty_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPartycode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
}

