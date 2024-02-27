using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeBSgroupMaster : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string AccountMasterTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = tblPrefix + "BSGroupMaster";
        AccountMasterTable = tblPrefix + "AccountMaster";
        user = Session["user"].ToString();
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                clsButtonNavigation.enableDisable("N");
                pnlPopup.Style["display"] = "none";
                this.makeEmptyForm("N");
                ViewState["mode"] = "I";
                this.showLastRecord();
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
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
    }
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = "select max(group_Code) as group_Code from " + tblHead +
                 " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

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
        }
        catch
        {
        }

    }

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
                        txtgroupCode.Text = dt.Rows[0]["group_Code"].ToString();
                        txtgroupName.Text = dt.Rows[0]["group_Name_E"].ToString();
                        drpGroupSummary.SelectedValue = dt.Rows[0]["group_Summary"].ToString();
                        drpgroupSection.SelectedValue = dt.Rows[0]["group_Type"].ToString();
                        txtGroupOrder.Text = dt.Rows[0]["group_Order"].ToString();
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
            hdnf.Value = txtgroupCode.Text;
            this.enableDisableNavigateButtons();
            return recordExist;
        }
        catch
        {
            return false;
        }
    }

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

    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead +
                " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and group_Code=" + hdnf.Value;
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }

    private void makeEmptyForm(string dAction)
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
            btntxtgroupCode.Text = "Choose No";
            btntxtgroupCode.Enabled = false;
            lblMsg.Text = string.Empty;
            drpgroupSection.Enabled = false;
            drpGroupSummary.Enabled = false;
            txtEditDoc_No.Enabled = true;
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
            btntxtgroupCode.Text = "Change No";
            btntxtgroupCode.Enabled = true;
            txtEditDoc_No.Enabled = false;
            drpgroupSection.Enabled = true;
            drpGroupSummary.Enabled = true;
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
            btntxtgroupCode.Text = "Choose No";
            btntxtgroupCode.Enabled = false;
            drpgroupSection.Enabled = false;
            drpGroupSummary.Enabled = false;
            txtEditDoc_No.Enabled = true;
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
            btntxtgroupCode.Text = "Choose No";
            btntxtgroupCode.Enabled = true;
            lblMsg.Text = string.Empty;
            drpgroupSection.Enabled = true;
            drpGroupSummary.Enabled = true;
            txtEditDoc_No.Enabled = false;
        }
    }

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
            if (a == false)
            {
                searchString = txtEditDoc_No.Text;
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtEditDoc_No";
                btnSearch_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                string qry1 = getDisplayQuery();
                fetchRecord(qry1);
                setFocusControl(txtEditDoc_No);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }


    protected void txtgroupCode_TextChanged(object sender, EventArgs e)
    {
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtgroupCode.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtgroupCode.Text != string.Empty)
                {
                    txtValue = txtgroupCode.Text;

                    string qry = "select * from " + tblHead + " where  group_Code='" + txtValue + "' " +
                        "  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                hdnf.Value = dt.Rows[0]["group_Code"].ToString();

                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        this.getMaxCode();
                                        //txtDoc_no.Enabled = false;

                                        btnSave.Enabled = true;   //IMP                                       
                                        setFocusControl(txtgroupName);
                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        qry = getDisplayQuery();
                                        bool recordExist = this.fetchRecord(qry);
                                        if (recordExist == true)
                                        {
                                            txtgroupCode.Enabled = false;
                                            setFocusControl(txtgroupName);

                                            hdnf.Value = txtgroupCode.Text;
                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";
                                    setFocusControl(txtgroupName);
                                    txtgroupCode.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("E");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtgroupCode.Text = string.Empty;
                                    setFocusControl(txtgroupCode);
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    setFocusControl(txtgroupCode);
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Doc No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtgroupCode.Text = string.Empty;
                setFocusControl(txtgroupCode);
            }
        }
        catch
        {

        }
        #endregion
    }

    protected void btntxtgroupCode_Click(object sender, EventArgs e)
    {
        try
        {
            if (btntxtgroupCode.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtgroupCode.Text = string.Empty;
                txtgroupCode.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtgroupCode);
            }

            if (btntxtgroupCode.Text == "Choose No")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtgroupCode";
                btnSearch_Click(sender, e);
            }
        }
        catch
        {
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
        {
            txtSearchText.Text = searchString;
        }
        else
        {
            txtSearchText.Text = searchString;
        }
        if (hdnfClosePopup.Value == "txtgroupCode" || hdnfClosePopup.Value == "txtEditDoc_No")
        {
            if (btntxtgroupCode.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtgroupCode.Text = string.Empty;
                txtgroupCode.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtgroupCode);
                hdnfClosePopup.Value = "Close";
            }

            if (btntxtgroupCode.Text == "Choose No")
            {
                lblPopupHead.Text = "--Select Group--";
                string qry = "SELECT  group_Code,group_Name_E from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (group_Code like '%" + txtSearchText.Text + "%' or group_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
        }
    }

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
                        //setFocusControl(txtEditDoc_No);
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

    protected void drpgroupSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(txtGroupOrder);
    }
    protected void drpGroupSummary_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(drpgroupSection);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.getMaxCode();
        setFocusControl(txtgroupName);
    }

    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                obj.code = "group_Code";

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
                                    txtgroupCode.Text = ds.Tables[0].Rows[0][0].ToString();
                                    txtgroupCode.Enabled = false;
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

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region validation
            bool isValidated = true;
            if (txtgroupCode.Text != string.Empty)
            {
                if (ViewState["mode"] != null)
                {
                    if (ViewState["mode"].ToString() == "I")
                    {
                        string str = clsCommon.getString("select group_Name_E from " + tblHead + " where group_Code=" + txtgroupCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblMsg.Text = "Code " + txtgroupCode.Text + " already exist";
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
                setFocusControl(txtgroupCode);
            }

            if (txtgroupName.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtgroupName);
            }
            #endregion

            #region declaration
            Int32 group_Code = txtgroupCode.Text != string.Empty ? Convert.ToInt32(txtgroupCode.Text) : 0;
            string group_name = txtgroupName.Text;
            string group_type = drpgroupSection.SelectedValue;
            string group_Summary = drpGroupSummary.SelectedValue;
            Int32 group_order = txtGroupOrder.Text != string.Empty ? Convert.ToInt32(txtGroupOrder.Text) : 0;
            Int32 Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string retValue = string.Empty;
            string strRev = string.Empty;

            string Created_By = Session["user"].ToString();
            string Modified_By = Session["user"].ToString();
            //Created_By = Session["user"].ToString();
            //lblCreatedBy.Text = Created_By.ToString();
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
                            obj.flag = 1;
                            obj.tableName = tblHead;
                            obj.columnNm = "group_Code,group_Name_E,group_Type,group_Summary,group_Order,Company_Code,Created_By";
                            obj.values = "'" + group_Code + "','" + group_name + "','" + group_type + "','" + group_Summary + "','" + group_order + "','" + Company_Code + "','" + Created_By + "'";
                            ds = obj.insertAccountMaster(ref strRev);

                            retValue = strRev;
                        }
                        if (ViewState["mode"].ToString() == "U")
                        {
                            obj.flag = 2;
                            obj.tableName = tblHead;
                            obj.columnNm = " group_Name_E='" + group_name + "',group_Type='" + group_type + "',group_Summary='" + group_Summary + "',group_Order=" + group_order + ",Modified_By='" + Modified_By + "' where group_Code=" + group_Code + " and Company_Code=" + Company_Code;
                            obj.values = "none";
                            ds = obj.insertAccountMaster(ref strRev);

                            retValue = strRev;
                        }
                        if (retValue == "-1")
                        {
                            hdnf.Value = txtgroupCode.Text;
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");

                            string qry = getDisplayQuery(); ;
                            this.fetchRecord(qry);

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Added!');", true);

                        }
                        if (retValue == "-2" || retValue == "-3")
                        {
                            clsButtonNavigation.enableDisable("S");
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");

                            hdnf.Value = txtgroupCode.Text;
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

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        txtgroupCode.Enabled = false;
        this.makeEmptyForm("E");

    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select group_Code from " + tblHead + " where group_Code=(select MIN(group_Code) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") " +
                "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
            if (txtgroupCode.Text != string.Empty)
            {
                string query = "SELECT top 1 [group_Code] from " + tblHead + " where group_Code<" + Convert.ToInt32(hdnf.Value) +
                    "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " ORDER BY group_Code DESC  ";
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
            if (txtgroupCode.Text != string.Empty)
            {
                string query = "SELECT top 1 [group_Code] from " + tblHead + " where group_Code>" + Convert.ToInt32(hdnf.Value) +
                    "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " ORDER BY group_Code asc  ";
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
            query = "select group_Code from " + tblHead + " where group_Code=(select MAX(group_Code) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") " +
                "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }


    #region navigateRecord
    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U";
                txtgroupCode.Text = hdnf.Value;

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

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where   Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());


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
        if (txtgroupCode.Text != string.Empty)
        {
            #region check for next or previous record exist or not

            query = "SELECT top 1 [group_Code] from " + tblHead + " where group_Code>" + Convert.ToInt32(hdnf.Value) +
                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                " ORDER BY group_Code asc  ";
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


            query = "SELECT top 1 [group_Code] from " + tblHead + " where group_Code<" + int.Parse(hdnf.Value) +
                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                " ORDER BY group_Code desc  ";
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
        #endregion
    }
    #endregion

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string str = clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Group_Code=" + txtgroupCode.Text + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
                if (str == string.Empty)   //Gledger does not contain this account then delete
                {
                    string currentDoc_No = txtgroupCode.Text;

                    DataSet ds = new DataSet();
                    string strrev = "";
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        obj.flag = 3;
                        obj.tableName = tblHead;
                        obj.columnNm = "   group_Code=" + currentDoc_No +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                        obj.values = "none";
                        ds = obj.insertAccountMaster(ref strrev);

                    }
                    string query = "";

                    if (strrev == "-3")
                    {
                        query = "SELECT top 1 [group_Code] from " + tblHead + "  where group_Code>" + Convert.ToInt32(currentDoc_No) +
                               "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                                " ORDER BY group_Code asc  ";


                        hdnf.Value = clsCommon.getString(query);

                        if (hdnf.Value == string.Empty)
                        {
                            query = "SELECT top 1 [group_Code] from " + tblHead + "  where group_Code<" + Convert.ToInt32(currentDoc_No) +
                               "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                                " ORDER BY group_Code desc  ";
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
                    lblMsg.Text = "Cannot delete this Group , it is in use";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        catch
        {
        }
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

        string str = clsCommon.getString("select count(group_Code) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

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
    protected void txtgroupName_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(drpGroupSummary);
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    protected void txtGroupOrder_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnSave);
    }
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        }
    }
}