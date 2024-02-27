using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
public partial class pgeAccountsmaster : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string GroupMasterTable = string.Empty;
    string AcGroupsTable = string.Empty;
    string SystemMastertable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextbox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string GLedgerTable = string.Empty;
    string TranTyp = "OP";
    string qry = string.Empty;
    string Debit = string.Empty;
    string Credit = string.Empty;
    string DRCRDiff = string.Empty;
    static WebControl objAsp = null;
    string user = string.Empty;
    string f_pan = "";
    string f_Main = "~/PAN/" + clsGV.user;
    string isAuthenticate = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "AccountMaster";
            tblDetails = tblPrefix + "AcContacts";
            GroupMasterTable = tblPrefix + "BSGroupMaster";
            AcGroupsTable = tblPrefix + "AcGroups";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMastertable = tblPrefix + "SystemMaster";
            qryCommon = tblPrefix + "qryAccountsList";
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            user = Session["user"].ToString();
            //btnAddCity.OnClientClick = String.Format("fnClickOK('{0}','{1}')", btnAddCity.UniqueID, "");
            //btn12.Attributes.Add("onclick", "fnSetFocus('" + txtCityName.ClientID + "');");
            // this.DebitCreditDiff();
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
                    this.DebitCreditDiff();
                    this.showLastRecord();
                    this.fillGroupsGrid();
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
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DebitCreditDiff()
    {
        try
        {
            Debit = Convert.ToString(clsCommon.getString("select SUM(AMOUNT) from " + tblPrefix + "GLEDGER where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Drcr='D' and Tran_Type='OP'"));
            if (Debit == string.Empty)
            {
                Debit = "0";
            }
            Credit = Convert.ToString(clsCommon.getString("select SUM(AMOUNT) from " + tblPrefix + "GLEDGER where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Drcr='C' and Tran_Type='OP'"));
            if (Credit == string.Empty)
            {
                Credit = "0";
            }
            DRCRDiff = Convert.ToString(Convert.ToDouble(Debit) - Convert.ToDouble(Credit));
            double value = 0;
            double diffn = double.Parse(DRCRDiff);
            if (diffn < 0)
            {
                value = Math.Abs(Math.Round(diffn, 2));
                lblDRCRDiff.Text = Convert.ToString(value);
                lblDRCRDiff.ForeColor = Color.Red;
            }
            else
            {
                value = Math.Abs(Math.Round(diffn, 2));
                lblDRCRDiff.Text = Convert.ToString(value);
                lblDRCRDiff.ForeColor = Color.Yellow;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void fillGroupsGrid()
    {
        try
        {
            string qry = "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            grdGroup.DataSource = ds;
            grdGroup.DataBind();
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
                obj.tableName = tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code>100";
                obj.code = "Ac_Code";
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
                                        txtAC_CODE.Text = ds.Tables[0].Rows[0][0].ToString();
                                        txtAC_CODE.Enabled = false;
                                    }
                                    else
                                    {
                                        txtAC_CODE.Text = "101";
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
                btntxtGstStateCode.Enabled = false;
                txtEditDoc_No.Enabled = true;
                pnlPopup.Style["display"] = "none";
                btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtAC_CODE.Text = "Choose No";
                btntxtAC_CODE.Enabled = true;
                btnAddCity.Enabled = false;
                btnAddGroup.Enabled = false;
                txtAC_CODE.Enabled = false;
                lblMsg.Text = string.Empty;
                drpBranch1Drcr.Enabled = false;
                drpBranch2Drcr.Enabled = false;
                txtSendingAcCode.Enabled = true;
                #region logic
                btntxtCITY_CODE.Enabled = false;
                btntxtGROUP_CODE.Enabled = false;
                drpType.Enabled = false;
                drpDrCr.Enabled = false;
                lblCITYNAME.Text = string.Empty;
                lblGROUPNAME.Text = string.Empty;
                pnlGroup.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                chkCarporate.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                chkIsTDS.Enabled = false;
                #endregion
            }
            if (dAction == "A")
            {
                drpType.SelectedIndex = 0;
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btntxtGstStateCode.Enabled = true;
                txtEditDoc_No.Enabled = false;
                btnSave.Text = "Save";
                btntxtAC_CODE.Text = "Change No";
                txtAC_CODE.Enabled = false;
                btnOpenDetailsPopup.Enabled = true;
                btntxtAC_CODE.Enabled = true;
                txtSendingAcCode.Enabled = true;
                chkIsTDS.Enabled = true;
                #region set Business logic for add
                setFocusControl(drpType);
                btntxtCITY_CODE.Enabled = true;
                btntxtGROUP_CODE.Enabled = true;
                lblCITYNAME.Text = string.Empty;
                lblGROUPNAME.Text = string.Empty;
                drpType.Enabled = true;
                drpDrCr.Enabled = true;
                drpBranch1Drcr.Enabled = true;
                drpBranch2Drcr.Enabled = true;
                pnlGroup.Enabled = true;
                chkCarporate.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btnOpenDetailsPopup.Enabled = true;
                chkCarporate.Checked = false;
                btnAddCity.Enabled = true;
                btnAddGroup.Enabled = true;
                for (int i = 0; i < grdGroup.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                    chk.Checked = false;
                }
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
                btntxtGstStateCode.Enabled = false;
                txtEditDoc_No.Enabled = true;
                btntxtAC_CODE.Text = "Choose No";
                txtAC_CODE.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btnAddCity.Enabled = false;
                btnAddGroup.Enabled = false;
                drpBranch1Drcr.Enabled = false;
                drpBranch2Drcr.Enabled = false;
                txtSendingAcCode.Enabled = true;
                #region logic
                btntxtCITY_CODE.Enabled = false;
                btntxtGROUP_CODE.Enabled = false;
                drpType.Enabled = false;
                drpDrCr.Enabled = false;
                //lblCITYNAME.Text = string.Empty;
                //lblGROUPNAME.Text = string.Empty;
                pnlGroup.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                chkCarporate.Enabled = false;
                chkIsTDS.Enabled = false;
                #endregion
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        if (drpType.SelectedValue == "F")
                        {
                            FixedAssetsControls();
                        }
                        else
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                        }
                    }
                }
                btntxtGstStateCode.Enabled = true;
                txtEditDoc_No.Enabled = false;
                btntxtAC_CODE.Text = "Choose No";
                txtAC_CODE.Enabled = true;
                lblMsg.Text = string.Empty;
                txtAC_NAME_R.Enabled = true;
                drpBranch1Drcr.Enabled = true;
                drpBranch2Drcr.Enabled = true;
                txtSHORT_NAME.Enabled = true;
                btnAddCity.Enabled = true;
                btnAddGroup.Enabled = true;
                txtSendingAcCode.Enabled = true;
                #region logic
                drpDrCr.Enabled = true;
                btntxtGROUP_CODE.Enabled = true;
                drpType.Enabled = true;
                chkIsTDS.Enabled = true;
                //Ckecking if the type is fixed assets
                if (drpType.SelectedValue != "F")
                {
                    btntxtAC_CODE.Enabled = true;
                    pnlGroup.Enabled = true;
                    btntxtCITY_CODE.Enabled = true;
                    btnOpenDetailsPopup.Enabled = true;
                    chkCarporate.Enabled = true;
                }
                #endregion
            }
            #region always check
            if (dAction == "A" || dAction == "E")
            {
                string s_item = drpType.SelectedValue;
                if (s_item == "I")
                {
                    setFocusControl(txtAC_RATE);
                    //    txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = false;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = false;
                    //     txtADDRESS_R.Text = "";
                    txtADDRESS_R.Enabled = false;
                    //     txtCITY_CODE.Text = "";
                    //      lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = false;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = false;
                    //     txtOPENING_BALANCE.Text = "";
                    txtOPENING_BALANCE.Enabled = false;
                    drpDrCr.Enabled = false;
                    //    txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = false;
                    //   txtTIN_NO.Text = "";
                    txtTIN_NO.Enabled = false;
                    //   txtCST_NO.Text = "";
                    txtCST_NO.Enabled = false;
                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = false;
                    //    txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = false;
                    drpBankDrCr.Enabled = false;
                }
                else if (drpType.SelectedValue != "F")
                {
                    //  txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = true;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = true;
                    //    txtADDRESS_R.Text = "";
                    txtADDRESS_R.Enabled = true;
                    //    txtCITY_CODE.Text = "";
                    //    lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = true;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = true;
                    //     txtOPENING_BALANCE.Text = "";
                    txtOPENING_BALANCE.Enabled = true;
                    drpDrCr.Enabled = true;
                    //   txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = true;
                    //   txtTIN_NO.Text = "";
                    txtTIN_NO.Enabled = true;
                    //   txtCST_NO.Text = "";
                    txtCST_NO.Enabled = true;
                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = true;
                    //     txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = true;
                    drpBankDrCr.Enabled = true;
                    grdGroup.Enabled = true;
                    btntxtAC_CODE.Enabled = true;
                }
                if (s_item == "B")
                {
                    //   txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = true;
                    drpBankDrCr.Enabled = true;
                }
                else
                {
                    //   txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = false;
                    drpBankDrCr.Enabled = false;
                }

                if (s_item == "F" || s_item == "I")
                {
                    btntxtAC_CODE.Enabled = true;

                    setFocusControl(txtAC_RATE);
                    //  txtAC_RATE.Text = "";
                    txtAC_RATE.Enabled = true;
                }
                else
                {
                    // txtAC_RATE.Text = "";
                    txtAC_RATE.Enabled = false;
                }
                if (s_item == "O" || s_item == "E")
                {
                    btntxtAC_CODE.Enabled = true;

                    TradingAndExpensesControls();
                }
            }

            #endregion
        }
        catch
        {
        }
    }
    private void TradingAndExpensesControls()
    {
        txtCOMMISSION.Enabled = false;
        txtADDRESS_E.Enabled = false;
        txtADDRESS_R.Enabled = false;
        txtCITY_CODE.Enabled = false;
        txtPINCODE.Enabled = false;
        txtOPENING_BALANCE.Enabled = false;
        drpDrCr.Enabled = false;
        txtLOCAL_LIC_NO.Enabled = false;
        txtTIN_NO.Enabled = false;
        txtCST_NO.Enabled = false;
        txtGST_NO.Enabled = false;
        txtBANK_OPENING.Enabled = false;
        drpBankDrCr.Enabled = false;
        chkCarporate.Enabled = false;
        txtBANK_AC_NO.Enabled = false;
        txtBANK_NAME.Enabled = false;
        txtEMAIL_ID.Enabled = false;
        txtEMAIL_ID_CC.Enabled = false;
        txtECC_NO.Enabled = false;
        txtRefBy.Enabled = false;
        txtOffPhone.Enabled = false;
        txtcompanyPan.Enabled = false;
        txtfax.Enabled = false;
        btnOpenDetailsPopup.Enabled = false;
        grdGroup.Enabled = false;
        txtAC_RATE.Enabled = false;
        txtOTHER_NARRATION.Enabled = false;
    }
    #endregion

    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;
            qry = "select max(AC_CODE) as AC_CODE from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
                        hdnf.Value = dt.Rows[0]["AC_CODE"].ToString();
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
        if (txtAC_CODE.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [AC_CODE] from " + tblHead + " where AC_CODE>" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY AC_CODE asc  ";
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
                query = "SELECT top 1 [AC_CODE] from " + tblHead + " where AC_CODE<" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ORDER BY AC_CODE asc  ";
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
            query = "select AC_CODE from " + tblHead + " where AC_CODE=(select MIN(AC_CODE) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
            if (txtAC_CODE.Text != string.Empty)
            {
                string query = "SELECT top 1 [AC_CODE] from " + tblHead + " where AC_CODE<" + Convert.ToInt32(hdnf.Value) +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by Ac_Code desc";
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
            if (txtAC_CODE.Text != string.Empty)
            {
                string query = "SELECT top 1 [AC_CODE] from " + tblHead + " where AC_CODE>" + Convert.ToInt32(hdnf.Value) +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by Ac_Code asc";
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
            query = "select AC_CODE from " + tblHead + " where AC_CODE=(select MAX(AC_CODE) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
        txtGROUP_CODE.Text = "140100";
        lblGROUPNAME.Text = clsCommon.getString("Select group_Name_E from " + tblPrefix + "BSGroupMaster where group_Code=140100 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        pnlPopupDetails.Style["display"] = "none";

        disableOpening();
    }

    private void disableOpening()
    {
        int yearCode = Convert.ToInt32(Session["year"].ToString());
        if (yearCode > 1)
        {
            txtOPENING_BALANCE.Enabled = false;
            drpDrCr.Enabled = false;
            txtBranch1OB.Enabled = false;
            drpBranch1Drcr.Enabled = false;
            txtBranch2OB.Enabled = false;
            drpBranch2Drcr.Enabled = false;
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
        //txtAC_CODE.Enabled = false;
        disableOpening();
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string str = clsCommon.getString("select DOC_NO from " + GLedgerTable + " where DOC_NO=" + txtAC_CODE.Text + " and TRAN_TYPE!='" + TranTyp + "' and COMPANY_CODE='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
                if (str == string.Empty)   //Gledger does not contain this account then delete
                {
                    string currentDoc_No = txtAC_CODE.Text;
                    DataSet ds = new DataSet();

                    string qry = "";
                    qry = "delete from " + tblHead + " where  Ac_Code=" + hdnf.Value + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    ds = clsDAL.SimpleQuery(qry);

                    qry = "";
                    qry = "delete from " + tblDetails + " where  Ac_Code=" + hdnf.Value + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    ds = clsDAL.SimpleQuery(qry);

                    qry = "";
                    qry = "delete from " + AcGroupsTable + " where  Ac_Code=" + hdnf.Value + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    ds = clsDAL.SimpleQuery(qry);

                    qry = "";
                    qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + TranTyp + "' and DOC_NO=" + hdnf.Value + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                    ds = clsDAL.SimpleQuery(qry);

                    string query = "SELECT top 1 [Ac_Code] from " + tblHead + "  where Ac_Code>" + Convert.ToInt32(currentDoc_No) +
                  "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                   " ORDER BY Ac_Code asc  ";

                    hdnf.Value = clsCommon.getString(query);

                    if (hdnf.Value == string.Empty)
                    {
                        query = "SELECT top 1 [Ac_Code] from " + tblHead + "  where Ac_Code<" + Convert.ToInt32(currentDoc_No) +
                             " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                            "  ORDER BY Ac_Code desc  ";
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
                        this.showLastRecord();
                        this.DebitCreditDiff();
                        clsButtonNavigation.enableDisable("N");         //No record exist  Last record deleted.

                    }
                    this.enableDisableNavigateButtons();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Cannot delete this Account , it is in use!')", true);
                    // lblMsg.Text = "Cannot delete this Account , it is in use";
                    //lblMsg.ForeColor = System.Drawing.Color.Red;
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
            string query = getDisplayQuery();
            bool recordExist = this.fetchRecord(query);
        }
        else
        {
            this.showLastRecord();
        }
        string str = clsCommon.getString("select count(Ac_Code) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

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
        ClearSendingSmsTextboxes();
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
                        txtAC_CODE.Text = dt.Rows[0]["AC_CODE"].ToString();
                        txtAC_NAME_E.Text = dt.Rows[0]["AC_NAME_E"].ToString();
                        txtAC_RATE.Text = dt.Rows[0]["AC_RATE"].ToString();
                        txtAC_NAME_R.Text = dt.Rows[0]["AC_NAME_R"].ToString();
                        txtCOMMISSION.Text = dt.Rows[0]["COMMISSION"].ToString();
                        txtSHORT_NAME.Text = dt.Rows[0]["SHORT_NAME"].ToString();
                        txtADDRESS_E.Text = dt.Rows[0]["ADDRESS_E"].ToString();
                        txtADDRESS_R.Text = dt.Rows[0]["ADDRESS_R"].ToString();
                        txtCITY_CODE.Text = dt.Rows[0]["CITY_CODE"].ToString();
                        lblCITYNAME.Text = dt.Rows[0]["CityName"].ToString();
                        txtGstStateCode.Text = dt.Rows[0]["GSTStateCode"].ToString();
                        lbltxtGstStateName.Text = dt.Rows[0]["GSTStateName"].ToString();
                        txtPINCODE.Text = dt.Rows[0]["PINCODE"].ToString();
                        txtOPENING_BALANCE.Text = dt.Rows[0]["OPENING_BALANCE"].ToString();
                        txtGROUP_CODE.Text = dt.Rows[0]["GROUP_CODE"].ToString();
                        drpDrCr.SelectedValue = dt.Rows[0]["DRCR"].ToString();
                        txtBranch1OB.Text = dt.Rows[0]["Branch1OB"].ToString();
                        txtBranch2OB.Text = dt.Rows[0]["Branch2OB"].ToString();
                        string Branch1DrCr = dt.Rows[0]["Branch1Drcr"].ToString();
                        drpBranch1Drcr.SelectedValue = Branch1DrCr;
                        drpBranch2Drcr.SelectedValue = dt.Rows[0]["Branch2Drcr"].ToString();
                        lblGROUPNAME.Text = dt.Rows[0]["BSGroupName"].ToString();
                        txtLOCAL_LIC_NO.Text = dt.Rows[0]["LOCAL_LIC_NO"].ToString();
                        txtdistance.Text = dt.Rows[0]["Distance"].ToString();
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
                        //lblCreated_Date.Text = dt.Rows[0]["Created_Date"].ToString();
                        //lblModified.Text = dt.Rows[0]["Modified_By"].ToString();
                        //lblModified_Date.Text = dt.Rows[0]["Modified_Date"].ToString();
                        txtBANK_NAME.Text = dt.Rows[0]["BANK_NAME"].ToString();
                        txtIfsc.Text = dt.Rows[0]["IFSC"].ToString();
                        txtTIN_NO.Text = dt.Rows[0]["TIN_NO"].ToString();
                        txtBANK_AC_NO.Text = dt.Rows[0]["BANK_AC_NO"].ToString();
                        txtCST_NO.Text = dt.Rows[0]["CST_NO"].ToString();
                        txtEMAIL_ID.Text = dt.Rows[0]["EMAIL_ID"].ToString();
                        txtGST_NO.Text = dt.Rows[0]["GST_NO"].ToString();
                        txtEMAIL_ID_CC.Text = dt.Rows[0]["EMAIL_ID_CC"].ToString();
                        txtOTHER_NARRATION.Text = dt.Rows[0]["OTHER_NARRATION"].ToString();
                        txtECC_NO.Text = dt.Rows[0]["ECC_NO"].ToString();
                        txtFssaiNo.Text = dt.Rows[0]["FSSAI"].ToString();
                        txtcompanyPan.Text = dt.Rows[0]["CompanyPan"].ToString();
                        txtBANK_OPENING.Text = dt.Rows[0]["BANK_OPENING"].ToString();
                        drpBankDrCr.SelectedValue = dt.Rows[0]["BANK_OP_DRCR"].ToString();
                        txtMOBILE.Text = dt.Rows[0]["Mobile_No"].ToString();
                        txtOffPhone.Text = dt.Rows[0]["OffPhone"].ToString();
                        txtRefBy.Text = dt.Rows[0]["referBy"].ToString();
                        txtfax.Text = dt.Rows[0]["Fax"].ToString();
                        drpType.SelectedValue = dt.Rows[0]["AC_TYPE"].ToString();
                        if (dt.Rows[0]["Carporate_Party"].ToString() == "Y")
                        {
                            chkCarporate.Checked = true;
                        }
                        else
                        {
                            chkCarporate.Checked = false;
                        }

                        string abcd = dt.Rows[0]["UnregisterGST"].ToString();
                        chkUnregisterGST.Checked = Convert.ToBoolean(abcd);

                        if (dt.Rows[0]["ISTDS"].ToString() == "Y")
                        {
                            chkIsTDS.Checked = true;
                        }
                        else
                        {
                            chkIsTDS.Checked = false;
                        }

                        recordExist = true;
                        hdnf.Value = txtAC_CODE.Text;
                        lblMsg.Text = "";

                        string branch1code = clsCommon.getString("select Branch1 from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblBranch1.Text = clsCommon.getString("select Branch from BranchMaster where Branch_Id=" + branch1code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string branch2code = clsCommon.getString("select Branch2 from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        lblBranch2.Text = clsCommon.getString("select Branch from BranchMaster where Branch_Id=" + branch2code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


                        #region Account Details
                        qry = "SELECT  PersonId as ID, Person_Name, Person_Mobile as Mobile, Person_Email as Email, Person_Pan as Pan,Other " +
                            " FROM   " + tblPrefix + "AcContacts where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + hdnf.Value;
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    string v = dt.Rows[0]["ID"].ToString();
                                    if (v != "")
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
                        }
                        else
                        {
                            grdDetail.DataSource = null;
                            grdDetail.DataBind();
                            ViewState["currentTable"] = null;
                        }
                        #endregion

                        pnlgrdDetail.Enabled = false;

                        #region Show Groups
                        DataTable dtTemp = new DataTable();
                        string strChecked = "";
                        qry = "select Group_Code from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + hdnf.Value;
                        ds = new DataSet();
                        dt = new DataTable();
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        strChecked = strChecked + dt.Rows[i]["Group_Code"].ToString() + ",";
                                    }
                                    strChecked = strChecked.Substring(0, strChecked.Length - 1);
                                }
                            }
                        }

                        if (strChecked != string.Empty)
                        {
                            qry = "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" +
                                Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Code in (" + strChecked + ")" +
                                "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" +
                                Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Code not in (" + strChecked + ")";

                            ds = ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        dtTemp = dt;
                                        if (ds.Tables[1].Rows.Count > 0)
                                        {
                                            dtTemp.Merge(ds.Tables[1]);
                                        }
                                    }
                                    else if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        dtTemp = ds.Tables[1];
                                    }

                                }
                            }
                            grdGroup.DataSource = dtTemp;
                            grdGroup.DataBind();
                            for (int i = 0; i < grdGroup.Rows.Count; i++)
                            {
                                CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                                if (strChecked.Contains(grdGroup.Rows[i].Cells[0].Text) == true)
                                {
                                    chk.Checked = true;
                                }
                                else
                                {
                                    chk.Checked = false;
                                }
                            }
                        }
                        else
                        {
                            qry = "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" +
                                Convert.ToInt32(Session["Company_Code"].ToString());
                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        grdGroup.DataSource = dt;
                                        grdGroup.DataBind();
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            //this.enableDisableNavigateButtons();
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
            string qryDisplay = "select * from " + qryCommon + " where Ac_Code=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
                txtAC_CODE.Text = hdnf.Value;
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
        //pnlMain.Enabled = false;
        setFocusControl(txtPERSON_NAME);
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

                        //     rowIndex = dt.Rows.Count + 1;
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

                        string id = clsCommon.getString("select PersonId from " + tblDetails +
                        " where Ac_Code='" + txtAC_CODE.Text + "' and PersonId=" + lblID.Text +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

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
                    dt.Columns.Add((new DataColumn("Person_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Mobile", typeof(string))));
                    dt.Columns.Add((new DataColumn("Email", typeof(string))));
                    dt.Columns.Add((new DataColumn("Pan", typeof(double))));
                    dt.Columns.Add((new DataColumn("Other", typeof(double))));
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
                dt.Columns.Add((new DataColumn("Person_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("Mobile", typeof(string))));
                dt.Columns.Add((new DataColumn("Email", typeof(string))));
                dt.Columns.Add((new DataColumn("Pan", typeof(string))));
                dt.Columns.Add((new DataColumn("Other", typeof(string))));
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;

            }
            dr["Person_Name"] = txtPERSON_NAME.Text;
            if (txtPERSON_MOBILE.Text != string.Empty)
            {
                dr["Mobile"] = txtPERSON_MOBILE.Text;
            }
            else
            {
                setFocusControl(txtPERSON_MOBILE);
                return;
            }
            dr["Email"] = txtPERSON_EMAIL.Text;
            dr["Pan"] = txtPerson_PAN.Text;
            dr["Other"] = txtPERSON_OTHER.Text;

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
                setFocusControl(txtPERSON_NAME);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            txtPERSON_NAME.Text = string.Empty;
            txtPERSON_MOBILE.Text = string.Empty;
            txtPERSON_EMAIL.Text = string.Empty;
            txtPerson_PAN.Text = string.Empty;
            txtPERSON_OTHER.Text = string.Empty;
            btnAdddetails.Text = "ADD";
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
        setFocusControl(btnSave);
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
            e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(8);
            e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(15);

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[5].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Left;

            }
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
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
            e.Row.Cells[0].Width = new Unit("100px");
            e.Row.Cells[1].Width = new Unit("400px");
        }
        #region scrap
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[0].Width = new Unit("100px");
        //    e.Row.Cells[1].Width = new Unit("200px");
        //}
        //if (e.Row.RowType == DataControlRowType.Header)
        //{

        //    foreach (TableCell cell in e.Row.Cells)
        //    {
        //        cell.Style["border-bottom"] = "2px solid #666666";
        //        cell.BackColor = System.Drawing.Color.Blue;
        //    }
        //}
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    for (int i = 0; i < grdPopup.Columns.Count; i++)
        //    {
        //        grdPopup.Columns[i].ItemStyle.Width = new Unit(20);
        //    }
        //    //TableCell cell = e.Row.Cells[0];
        //    //cell.Width = new Unit(10);

        ////    //Formatting Cells

        ////    for (int i = 1; i < e.Row.Cells.Count; i++)
        ////    {
        ////        cell = e.Row.Cells[1];
        ////        cell.HorizontalAlign = HorizontalAlign.Left;
        ////        cell.Width = new Unit(50);
        ////    }
        //}
        #endregion
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
                //foreach (TableCell cell in e.Row.Cells)
                //{
                //    cell.Style.Add("width", "100px");
                //}

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
                            //Making Changes by ankush
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
        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text.Trim());
        lblNo.Text = Server.HtmlDecode(gridViewRow.Cells[9].Text.Trim());
        txtPERSON_NAME.Text = Server.HtmlDecode(gridViewRow.Cells[3].Text.Trim());
        txtPERSON_MOBILE.Text = Server.HtmlDecode(gridViewRow.Cells[4].Text.Trim());
        txtPERSON_EMAIL.Text = Server.HtmlDecode(gridViewRow.Cells[5].Text.Trim());
        txtPerson_PAN.Text = Server.HtmlDecode(gridViewRow.Cells[6].Text.Trim());
        txtPERSON_OTHER.Text = Server.HtmlDecode(gridViewRow.Cells[7].Text.Trim());
        btnAdddetails.Text = "Update";
        setFocusControl(txtPERSON_NAME);
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

                string IDExisting = clsCommon.getString("select PersonId from " + tblDetails +
                    " where  Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and " +
                    " Ac_Code=" + hdnf.Value + " and PersonId=" + ID);

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
                }
                ViewState["currentTable"] = dt;

            }

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
        strTextbox = "txtAC_CODE";
        csCalculations();
        setFocusControl(txtAC_NAME_E);
    }
    #endregion

    #region [btntxtAC_CODE_Click]
    protected void btntxtAC_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtEditDoc_No";
            btnSearch_Click(sender, e);
            setFocusControl(txtSearchText);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtAC_RATE_TextChanged]
    protected void txtAC_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_RATE.Text;
        strTextbox = "txtAC_RATE";
        csCalculations();
    }
    #endregion

    #region [txtAC_NAME_E_TextChanged]
    protected void txtAC_NAME_E_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_NAME_E.Text;
        strTextbox = "txtAC_NAME_E";
        csCalculations();
    }
    #endregion

    #region [txtAC_NAME_R_TextChanged]
    protected void txtAC_NAME_R_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_NAME_R.Text;
        strTextbox = "txtAC_NAME_R";
        csCalculations();
    }
    #endregion

    #region [txtCOMMISSION_TextChanged]
    protected void txtCOMMISSION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCOMMISSION.Text;
        strTextbox = "txtCOMMISSION";
        csCalculations();
    }
    #endregion

    #region [txtSHORT_NAME_TextChanged]
    protected void txtSHORT_NAME_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSHORT_NAME.Text;
        strTextbox = "txtSHORT_NAME";
        csCalculations();
    }
    #endregion

    #region [txtADDRESS_E_TextChanged]
    protected void txtADDRESS_E_TextChanged(object sender, EventArgs e)
    {
        searchString = txtADDRESS_E.Text;
        strTextbox = "txtADDRESS_E";
        csCalculations();
    }
    #endregion

    #region [txtADDRESS_R_TextChanged]
    protected void txtADDRESS_R_TextChanged(object sender, EventArgs e)
    {
        searchString = txtADDRESS_R.Text;
        strTextbox = "txtADDRESS_R";
        csCalculations();
    }
    #endregion

    #region [txtCITY_CODE_TextChanged]
    protected void txtCITY_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCITY_CODE.Text;
        strTextbox = "txtCITY_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtCITY_CODE_Click]
    protected void btntxtCITY_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCITY_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtPINCODE_TextChanged]
    protected void txtPINCODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPINCODE.Text;
        strTextbox = "txtPINCODE";
        csCalculations();
    }
    #endregion

    #region [txtOPENING_BALANCE_TextChanged]
    protected void txtOPENING_BALANCE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOPENING_BALANCE.Text;
        strTextbox = "txtOPENING_BALANCE";
        csCalculations();
    }
    #endregion

    #region [drpDrCr_SelectedIndexChanged]
    protected void drpDrCr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = drpDrCr.SelectedValue;
            strTextbox = "drpDrCr";
            csCalculations();
        }
        catch
        {

        }
    }
    #endregion

    #region [txtGROUP_CODE_TextChanged]
    protected void txtGROUP_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGROUP_CODE.Text;
        strTextbox = "txtGROUP_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtGROUP_CODE_Click]
    protected void btntxtGROUP_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGROUP_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtLOCAL_LIC_NO_TextChanged]
    protected void txtLOCAL_LIC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLOCAL_LIC_NO.Text;
        strTextbox = "txtLOCAL_LIC_NO";
        csCalculations();
    }
    #endregion

    #region [txtBANK_NAME_TextChanged]
    protected void txtBANK_NAME_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_NAME.Text;
        strTextbox = "txtBANK_NAME";
        csCalculations();
    }
    #endregion

    #region [txtTIN_NO_TextChanged]
    protected void txtTIN_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTIN_NO.Text;
        strTextbox = "txtTIN_NO";
        csCalculations();
    }
    #endregion

    #region [txtBANK_AC_NO_TextChanged]
    protected void txtBANK_AC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_AC_NO.Text;
        strTextbox = "txtBANK_AC_NO";
        csCalculations();
    }
    #endregion

    #region [txtCST_NO_TextChanged]
    protected void txtCST_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCST_NO.Text;
        strTextbox = "txtCST_NO";
        csCalculations();
    }
    #endregion

    #region [txtEMAIL_ID_TextChanged]
    protected void txtEMAIL_ID_TextChanged(object sender, EventArgs e)
    {
        searchString = txtEMAIL_ID.Text;
        strTextbox = "txtEMAIL_ID";
        csCalculations();
    }
    #endregion

    #region [txtGST_NO_TextChanged]
    protected void txtGST_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGST_NO.Text;
        strTextbox = "txtGST_NO";
        csCalculations();
    }
    #endregion

    #region [txtEMAIL_ID_CC_TextChanged]
    protected void txtEMAIL_ID_CC_TextChanged(object sender, EventArgs e)
    {
        searchString = txtEMAIL_ID_CC.Text;
        strTextbox = "txtEMAIL_ID_CC";
        csCalculations();
    }
    #endregion

    #region [txtOTHER_NARRATION_TextChanged]
    protected void txtOTHER_NARRATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOTHER_NARRATION.Text;
        strTextbox = "txtOTHER_NARRATION";
        csCalculations();
    }
    #endregion

    #region [txtECC_NO_TextChanged]
    protected void txtECC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtECC_NO.Text;
        strTextbox = "txtECC_NO";
        csCalculations();
    }
    #endregion

    #region [txtBANK_OPENING_TextChanged]
    protected void txtBANK_OPENING_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_OPENING.Text;
        strTextbox = "txtBANK_OPENING";
        csCalculations();
    }
    #endregion

    #region [drpBankDrCr_SelectedIndexChanged]
    protected void drpBankDrCr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = drpBankDrCr.SelectedValue;
            strTextbox = "drpBankDrCr";
            csCalculations();
        }
        catch
        {

        }
    }
    #endregion

    #region [txtPERSON_NAME_TextChanged]
    protected void txtPERSON_NAME_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_NAME.Text;
        strTextbox = "txtPERSON_NAME";
        csCalculations();
    }
    #endregion

    #region [txtPERSON_MOBILE_TextChanged]
    protected void txtPERSON_MOBILE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_MOBILE.Text;
        strTextbox = "txtPERSON_MOBILE";
        csCalculations();
    }
    #endregion

    #region [txtPERSON_EMAIL_TextChanged]
    protected void txtPERSON_EMAIL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_EMAIL.Text;
        strTextbox = "txtPERSON_EMAIL";
        csCalculations();
    }
    #endregion

    #region [txtPerson_PAN_TextChanged]
    protected void txtPerson_PAN_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPerson_PAN.Text;
        strTextbox = "txtPerson_PAN";
        csCalculations();
    }
    #endregion

    #region [txtPERSON_OTHER_TextChanged]
    protected void txtPERSON_OTHER_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_OTHER.Text;
        strTextbox = "txtPERSON_OTHER";
        csCalculations();
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtAC_CODE" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtAC_CODE.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtAC_CODE.Text = string.Empty;
                    txtAC_CODE.Enabled = true;

                    btnSave.Enabled = false;
                    setFocusControl(txtAC_CODE);
                    hdnfClosePopup.Value = "Close";
                }

                if (btntxtAC_CODE.Text == "Choose No")
                {
                    if (searchString != string.Empty && strTextbox == "txtAC_CODE")
                    {
                        txtSearchText.Text = txtSearchText.Text;
                    }
                    else
                    {
                        txtSearchText.Text = txtSearchText.Text;
                    }

                    lblPopupHead.Text = "--Select Account--";
                    string qry = "select Ac_Code,Ac_Name_E from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                    this.showPopup(qry);
                }
            }

            if (hdnfClosePopup.Value == "txtSendingAcCode")
            {
                if (searchString != string.Empty && strTextbox == "txtSendingAcCode")
                {
                    txtSearchText.Text = searchString;
                }
                else
                {
                    txtSearchText.Text = string.Empty;
                }

                lblPopupHead.Text = "--Select Account--";
                string qry = "select Ac_Code,Ac_Name_E from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtCITY_CODE")
            {
                if (searchString != string.Empty && strTextbox == "txtCITY_CODE")
                {
                    txtSearchText.Text = searchString;
                }
                else
                {
                    txtSearchText.Text = string.Empty;
                }
                lblPopupHead.Text = "--Select City--";
                string qry = "select city_code,city_name_e,city_name_r,state from " + cityMasterTable + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (city_code like '%" + txtSearchText.Text + "%' or city_name_e like '%" + txtSearchText.Text + "%') order by city_name_e";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtGstStateCode")
            {
                if (searchString != string.Empty && strTextbox == "txtGstStateCode")
                {
                    txtSearchText.Text = searchString;
                }
                else
                {
                    txtSearchText.Text = string.Empty;
                }
                lblPopupHead.Text = "--Select GST State Code--";
                string qry = "Select State_Code,State_Name from GSTStateMaster where State_Code like'%" + txtSearchText.Text + "%' or State_Name like'%" + txtSearchText.Text + "%'";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtGROUP_CODE")
            {
                if (searchString != string.Empty && strTextbox == "txtGROUP_CODE")
                {
                    txtSearchText.Text = searchString;
                }
                else
                {
                    txtSearchText.Text = string.Empty;
                }

                lblPopupHead.Text = "--Select BS group--";
                string qry = "select group_Code,group_Name_E,group_Name_R from " + GroupMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                " and (group_Code like '%" + txtSearchText.Text + "%' or group_Name_E like '%" + txtSearchText.Text + "%') order by group_Name_E";
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
            using (clsDataProvider objDataProvider = new clsDataProvider())
            {
                setFocusControl(txtSearchText);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = objDataProvider.GetDataSet(qry);
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
            if (hdnfClosePopup.Value == "txtCITY_CODE")
            {
                setFocusControl(txtCITY_CODE);
            }
            if (hdnfClosePopup.Value == "txtGROUP_CODE")
            {
                setFocusControl(txtGROUP_CODE);
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
                strTextbox = hdnfClosePopup.Value;

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
        #region Validation
        bool isValidated = true;
        if (txtAC_CODE.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtAC_CODE);
            return;
        }
        if (txtAC_NAME_E.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtAC_NAME_E);
            return;
        }
        if (txtGROUP_CODE.Text != string.Empty)
        {
            string str = clsCommon.getString("select group_Code from " + GroupMasterTable + " where group_Code=" + txtGROUP_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtGROUP_CODE);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtGROUP_CODE);
            return;
        }
        #endregion

        #region -Head part declearation
        Int32 AC_CODE = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
        string AC_TYPE = drpType.SelectedValue;
        double AC_RATE = txtAC_RATE.Text != string.Empty ? Convert.ToDouble(txtAC_RATE.Text) : 0.00;
        string AC_NAME_E = txtAC_NAME_E.Text;
        string AC_NAME_R = txtAC_NAME_R.Text;
        double COMMISSION = txtCOMMISSION.Text != string.Empty ? Convert.ToDouble(txtCOMMISSION.Text) : 0.00;
        string SHORT_NAME = txtSHORT_NAME.Text;
        string ADDRESS_E = txtADDRESS_E.Text;
        //string ADDRESS_E=txtADDRESS_E.Text != string.Empty ? Convert.
        string ADDRESS_R = txtADDRESS_R.Text;
        Int32 CITY_CODE = txtCITY_CODE.Text != string.Empty ? Convert.ToInt32(txtCITY_CODE.Text) : 0;
        Int32 PINCODE = txtPINCODE.Text != string.Empty ? Convert.ToInt32(txtPINCODE.Text) : 0;
        double OPENING_BALANCE = txtOPENING_BALANCE.Text != string.Empty ? Convert.ToDouble(txtOPENING_BALANCE.Text) : 0.00;
        string DRCR = drpDrCr.SelectedValue;
        double Distance = txtdistance.Text != string.Empty ? Convert.ToDouble(txtdistance.Text) : 0.00;

        int GSTStateCode = txtGstStateCode.Text.Trim() != string.Empty ? Convert.ToInt32(txtGstStateCode.Text) : 0;


        double Branch1OB = txtBranch1OB.Text != string.Empty ? Convert.ToDouble(txtBranch1OB.Text) : 0.00;
        string Branch1Drcr = drpBranch1Drcr.SelectedValue;
        double Branch2OB = txtBranch2OB.Text != string.Empty ? Convert.ToDouble(txtBranch2OB.Text) : 0.00;
        string Branch2Drcr = drpBranch2Drcr.SelectedValue;

        Int32 GROUP_CODE = txtGROUP_CODE.Text != string.Empty ? Convert.ToInt32(txtGROUP_CODE.Text) : 0;
        string LOCAL_LIC_NO = txtLOCAL_LIC_NO.Text;
        string BANK_NAME = txtBANK_NAME.Text;
        string TIN_NO = txtTIN_NO.Text;
        string BANK_AC_NO = txtBANK_AC_NO.Text;
        string CST_NO = txtCST_NO.Text;
        string EMAIL_ID = txtEMAIL_ID.Text;
        string GST_NO = txtGST_NO.Text;
        string EMAIL_ID_CC = txtEMAIL_ID_CC.Text;
        string OTHER_NARRATIOM = txtOTHER_NARRATION.Text;
        string ECC_NO = txtECC_NO.Text;
        string MOBILE = txtMOBILE.Text;
        string IFSC = txtIfsc.Text.ToString();
        string FSSAI = txtFssaiNo.Text.ToString();
        double BANK_OPENING = txtBANK_OPENING.Text != string.Empty ? Convert.ToDouble(txtBANK_OPENING.Text) : 0.00;
        string BANK_OP_DRCR = drpBankDrCr.SelectedValue;
        string carporate_party = string.Empty;

        if (chkCarporate.Checked == true)
        {
            carporate_party = "Y";
        }
        else
        {
            carporate_party = "N";
        }

        int UnregisterGST = 0;
        if (chkUnregisterGST.Checked)
        {
            UnregisterGST = 1;
        }

        string istds = string.Empty;
        if (chkIsTDS.Checked == true)
        {
            istds = "Y";
        }
        else
        {
            istds = "N";
        }

        string referBy = txtRefBy.Text;
        string OffPhone = txtOffPhone.Text;
        string Fax = txtfax.Text;
        string CompanyPan = txtcompanyPan.Text;
        # region file upload
        //Maintaining state during postbacks
        //if (Session["FileUpload_PAN"] == null && FileUpload_PAN.HasFile)
        //{
        //    Session["FileUpload_PAN"] = FileUpload_PAN;
        //    f_pan = FileUpload_PAN.FileName;
        //}
        //else if (Session["FileUpload_PAN"] != null && (!FileUpload_PAN.HasFile))
        //{
        //    FileUpload_PAN = (FileUpload)Session["FileUpload_PAN"];
        //}
        //else if (FileUpload_PAN.HasFile)
        //{
        //    Session["FileUpload_PAN"] = FileUpload_PAN;
        //    f_pan = FileUpload_PAN.FileName;
        //}
        //Separate code
        //if (FileUpload_PAN.HasFile)
        //{
        //    string ImgPath = Server.MapPath("~/PAN/" + Guid.NewGuid() + FileUpload_PAN.FileName);
        //    FileUpload_PAN.SaveAs(ImgPath);
        //    string ShowImgPath = ImgPath.Substring(ImgPath.LastIndexOf("\\"));
        //    f_pan = FileUpload_PAN.FileName;
        //    //imgShow.ImageUrl = "~/Images" + ShowImgPath;
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Please select the image to upload');", true);
        //}
        # endregion
        string AC_Pan = f_pan;
        string retValue = string.Empty;
        string strRev = string.Empty;
        int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int year_Code = Convert.ToInt32(Session["year"].ToString());
        int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        string Branch1DrCr = "";
        string Branch2DrCr = "";
        if (drpBranch1Drcr.SelectedValue == "C")
        {
            Branch1DrCr = "Credit";
        }
        else
        {
            Branch1DrCr = "Debit";
        }
        if (drpBranch2Drcr.SelectedValue == "C")
        {
            Branch2DrCr = "Credit";
        }
        else
        {
            Branch2DrCr = "Debit";
        }

        string GlegerNarration = "Branch 1:" + txtBranch1OB.Text + " " + Branch1DrCr + " Branch 2:" + txtBranch2OB.Text + " " + Branch2DrCr;
        string Created_By = clsGV.user;
        Created_By = Session["user"].ToString();
        string Modified_By = clsGV.user;
        Modified_By = Session["user"].ToString();
        #endregion-End of Head part declearation

        #region save Account Master
        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
        {
            if (ViewState["mode"] != null)
            {
                DataSet ds = new DataSet();
                try
                {
                    if (ViewState["mode"].ToString() == "I")
                    {
                        #region ---- Accountmaster ----
                        obj.flag = 1;
                        obj.tableName = tblHead;
                        obj.columnNm = "Ac_Code,Ac_Name_E,Ac_Name_R,Ac_type,Ac_rate,Address_E,Address_R,City_Code,Pincode,Local_Lic_No,Tin_No,Cst_no,Gst_No,Email_Id,EMAIL_ID_CC,Other_Narration, " +
                        " ECC_No,Bank_Name,Bank_Ac_No,Bank_Opening,bank_Op_Drcr,Opening_Balance,Drcr,Group_Code,Company_Code,Created_By,Short_Name,Commission,carporate_party," +
                        "referBy,OffPhone,Fax,CompanyPan,AC_Pan,Mobile_No,IFSC,FSSAI,Branch1OB,Branch2OB,Branch1Drcr,Branch2Drcr,GSTStateCode,UnregisterGST,Distance,ISTDS";
                        obj.values = "'" + AC_CODE + "','" + AC_NAME_E + "','" + AC_NAME_R + "','" + AC_TYPE + "','" + AC_RATE + "',N'" + ADDRESS_E + "','" + ADDRESS_R + "','" + CITY_CODE + "','" +
                            PINCODE + "','" + LOCAL_LIC_NO + "','" + TIN_NO + "','" + CST_NO + "','" + GST_NO + "','" + EMAIL_ID + "', " +
                            " '" + EMAIL_ID_CC + "','" + OTHER_NARRATIOM + "','" + ECC_NO + "','" + BANK_NAME + "','" + BANK_AC_NO + "','" + BANK_OPENING + "','" + BANK_OP_DRCR + "','" + OPENING_BALANCE + "','" +
                            DRCR + "','" + GROUP_CODE + "','" + Company_Code + "','" + Session["user"].ToString() + "','" + SHORT_NAME + "','" + COMMISSION + "','" + carporate_party + "','" +
                            referBy + "','" + OffPhone + "','" + Fax + "','" + CompanyPan + "','" + AC_Pan + "','" + MOBILE + "','" + IFSC + "','" + FSSAI + "','" + Branch1OB + "','" + Branch2OB + "','" + Branch1Drcr + "','" + Branch2Drcr + "','" + GSTStateCode + "','" + UnregisterGST + "','" + Distance + "','" + istds + "'";
                        ds = obj.insertAccountMaster(ref strRev);
                        retValue = strRev;
                    }
                    else
                    {
                        //Update Mode
                        obj.flag = 2;
                        obj.tableName = tblHead;

                        obj.columnNm = "Ac_Name_E='" + AC_NAME_E + "',Ac_Name_R='" + AC_NAME_R + "',Ac_type='" + AC_TYPE + "',Ac_rate='" + AC_RATE
                            + "',Address_E='" + ADDRESS_E + "',Address_R='" + ADDRESS_R + "'," +
                            " City_Code='" + CITY_CODE + "',Pincode='" + PINCODE + "',Local_Lic_No='" + LOCAL_LIC_NO + "',Tin_No='" + TIN_NO + "',Cst_no='" + CST_NO + "',GST_NO='" + GST_NO + "',Email_Id='" + EMAIL_ID + "',EMAIL_ID_CC='" + EMAIL_ID_CC + "',Other_Narration='" + OTHER_NARRATIOM + "', " +
                            " ECC_No='" + ECC_NO + "', Bank_Name='" + BANK_NAME + "',Bank_Ac_No='" + BANK_AC_NO + "',Bank_Opening='" + BANK_OPENING + "',bank_Op_Drcr='" + BANK_OP_DRCR + "',Opening_Balance='" + OPENING_BALANCE + "',Drcr='" + DRCR + "',Group_Code='" + GROUP_CODE + "',Modified_By='" + Session["user"].ToString() + "',Short_Name='" + SHORT_NAME + "',Commission='" + COMMISSION +
                            "',carporate_party='" + carporate_party + "',referBy='" + referBy + "',OffPhone='" + OffPhone + "',Fax='" + Fax + "',IFSC='" + IFSC + "',FSSAI='" + FSSAI + "',CompanyPan='" + CompanyPan + "',AC_Pan='" + AC_Pan + "',Mobile_No='" + MOBILE + "',Branch1OB='" + Branch1OB + "',Branch2OB='" + Branch2OB + "',Branch1Drcr='" + Branch1Drcr + "',Branch2Drcr='" + Branch2Drcr + "',GSTStateCode='" + GSTStateCode + "',UnregisterGST='" + UnregisterGST + "' ,Distance='" + Distance + "',ISTDS='" + istds + "'  where AC_CODE=" + AC_CODE + " and Company_Code=" + Company_Code;

                        obj.values = "none";
                        ds = new DataSet();
                        ds = obj.insertAccountMaster(ref strRev);
                        retValue = strRev;
                    }
                }
                catch { }
                        #endregion

                #region -------------------- Account Details --------------------

                Int32 personId = 0;
                string personName = "";
                string mobile = "";
                string email = "";
                string pan = "";
                string other = "";
                string i_d = "";
                string DOC_DATE = null;

                if (strRev == "-1" || strRev == "-2")
                {

                    if (grdDetail.Rows.Count > 0)
                    {
                        // strRev = "";
                        for (int i = 0; i < grdDetail.Rows.Count; i++)
                        {

                            personId = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                            if (grdDetail.Rows[i].Cells[3].Text.Trim() != "&nbsp;" || grdDetail.Rows[i].Cells[3].Text.Trim() != "")
                            {
                                personName = grdDetail.Rows[i].Cells[3].Text.Trim();
                            }
                            else
                            {
                                personName = grdDetail.Rows[i].Cells[3].Text = "";
                            }
                            if (grdDetail.Rows[i].Cells[4].Text.Trim() != "&nbsp;" || grdDetail.Rows[i].Cells[4].Text.Trim() != "")
                            {
                                mobile = grdDetail.Rows[i].Cells[4].Text.Trim();
                            }
                            else
                            {
                                mobile = grdDetail.Rows[i].Cells[4].Text = "";
                            }
                            if (grdDetail.Rows[i].Cells[5].Text.Trim() != "&nbsp;" || grdDetail.Rows[i].Cells[5].Text.Trim() != "")
                            {
                                email = grdDetail.Rows[i].Cells[5].Text.Trim();
                            }
                            else
                            {
                                email = grdDetail.Rows[i].Cells[5].Text = "";
                            }
                            //mobile = grdDetail.Rows[i].Cells[4].Text;
                            //email = grdDetail.Rows[i].Cells[5].Text;
                            if (grdDetail.Rows[i].Cells[6].Text.Trim() != "&nbsp;" || grdDetail.Rows[i].Cells[6].Text.Trim() != "")
                            {
                                pan = grdDetail.Rows[i].Cells[6].Text.Trim();
                            }
                            else
                            {
                                pan = grdDetail.Rows[i].Cells[6].Text = "";
                            }
                            //pan = grdDetail.Rows[i].Cells[6].Text;
                            if (grdDetail.Rows[i].Cells[7].Text.Trim() != "&nbsp;" || grdDetail.Rows[i].Cells[7].Text.Trim() != "")
                            {
                                other = grdDetail.Rows[i].Cells[7].Text.Trim();
                            }
                            else
                            {
                                other = grdDetail.Rows[i].Cells[7].Text = "";
                            }
                            //other = grdDetail.Rows[i].Cells[7].Text;
                            i_d = grdDetail.Rows[i].Cells[2].Text;

                            if (grdDetail.Rows[i].Cells[8].Text != "N" && grdDetail.Rows[i].Cells[8].Text != "R")
                            {
                                if (grdDetail.Rows[i].Cells[8].Text == "A")
                                {
                                    obj.flag = 1;
                                    obj.tableName = tblDetails;
                                    obj.columnNm = "Company_Code,Ac_Code,Person_Name,Person_Mobile,Person_Email,Person_Pan,Other";
                                    obj.values = "'" + Company_Code + "','" + AC_CODE + "','" + Server.HtmlDecode(personName) + "','" + Server.HtmlDecode(mobile) + "','" + Server.HtmlDecode(email) + "','" + Server.HtmlDecode(pan) + "','" + Server.HtmlDecode(other) + "'";
                                    ds = new DataSet();
                                    ds = obj.insertAccountMaster(ref strRev);
                                }
                                if (grdDetail.Rows[i].Cells[8].Text == "U")
                                {

                                    obj.flag = 2;
                                    obj.tableName = tblDetails;
                                    obj.columnNm = " Person_Name='" + Server.HtmlDecode(personName) + "',Person_Mobile='" + Server.HtmlDecode(mobile) + "',Person_Email='" + Server.HtmlDecode(email) + "',Person_Pan='" + Server.HtmlDecode(pan) + "',Other='" + Server.HtmlDecode(other) + "'" +
                                        " where PersonId='" + personId + "' and Company_Code='" + Company_Code + "' and Ac_Code='" + AC_CODE + "'";
                                    obj.values = "none";
                                    ds = new DataSet();
                                    ds = obj.insertAccountMaster(ref strRev);
                                }
                                if (grdDetail.Rows[i].Cells[8].Text == "D")
                                {
                                    obj.flag = 3;
                                    obj.tableName = tblDetails;
                                    obj.columnNm = "PersonId='" + personId + "' and Company_Code='" + Company_Code + "' and Ac_Code='" + AC_CODE + "'";
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
                }

                #endregion

                #region ----------------  Account Groups ----------
                if (grdGroup.Rows.Count > 0)
                {
                    ds = new DataSet();
                    qry = "delete from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + AC_CODE;
                    ds = clsDAL.SimpleQuery(qry);


                    for (int i = 0; i < grdGroup.Rows.Count; i++)
                    {
                        CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                        if (chk.Checked)
                        {
                            string Group_Code = grdGroup.Rows[i].Cells[0].Text;

                            obj.flag = 1;
                            obj.tableName = AcGroupsTable;
                            obj.columnNm = "Company_Code,Ac_Code,Group_Code";
                            obj.values = "'" + Company_Code + "','" + AC_CODE + "','" + Group_Code + "'";

                            ds = obj.insertAccountMaster(ref strRev);
                        }
                    }
                }
                #endregion

                #region GLedger Effect
                qry = "";
                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='OP' and DOC_NO=" + AC_CODE + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                ds = clsDAL.SimpleQuery(qry);
                if (OPENING_BALANCE != 0)
                {
                    Int32 GID = 0;
                    obj.flag = 1;
                    obj.tableName = GLedgerTable;
                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE," +
                        "DRCR,DRCR_HEAD,ADJUSTED_AMOUNT";
                    obj.values = " '" + TranTyp + "','" + AC_CODE + "','2015/03/31','" + AC_CODE + "','" + "Opening Balance " + GlegerNarration + "' , "
                        + OPENING_BALANCE + ",null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "',1,0,'" + DRCR + "',0,0";
                    ds = obj.insertAccountMaster(ref strRev);
                    //retValue = strRev;
                }
                #endregion

                if (retValue == "-1")
                {
                    // this.saveDocuments();
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    hdnf.Value = txtAC_CODE.Text;
                    this.makeEmptyForm("S");

                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    this.DebitCreditDiff();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
                }
                if (retValue == "-2" || retValue == "-3")
                {
                    //this.saveDocuments();
                    clsButtonNavigation.enableDisable("S");
                    this.enableDisableNavigateButtons();
                    hdnf.Value = txtAC_CODE.Text;
                    this.makeEmptyForm("S");
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    this.DebitCreditDiff();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Updated Successfully!')", true);
                }
            }
        }
        #endregion

        ClearSendingSmsTextboxes();
    }
    #endregion

    #region [saveDocuments]
    private void saveDocuments()
    {
        //try
        //{
        //    if (FileUpload_PAN.HasFile)
        //    {
        //        try
        //        {
        //            string filename = Path.GetFileName(FileUpload_PAN.FileName);
        //            FileUpload_PAN.SaveAs(Server.MapPath("~/PAN/PAN_" + clsGV.user + "") + filename);
        //            //
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //            // StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
        //        }
        //    }
        //    //using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
        //    //{
        //    //    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
        //    //    {
        //    //        w.WriteLine(s);
        //    //    }
        //    //}
        //}
        //catch
        //{
        //}
    }
    #endregion


    #region [drpType_SelectedIndexChanged]
    protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region always check
        setFocusControl(txtAC_NAME_E);
        string val = drpType.SelectedValue.ToString();
        //drpType.Attributes.Add("onChange", "javascript:EnableDisable();");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "javascript:EnableDisable('" + val + "');", true);
        //if (btnSave.Enabled == true)
        //{

        //    string s_item = drpType.SelectedValue;
        //    if (s_item == "T")
        //    {
        //        txtCOMMISSION.Enabled = false;
        //        txtADDRESS_E.Enabled = false;
        //        txtADDRESS_R.Enabled = false;
        //        txtCITY_CODE.Enabled = true;
        //        txtPINCODE.Enabled = false;
        //        txtOPENING_BALANCE.Enabled = false;
        //        drpDrCr.Enabled = false;
        //        txtLOCAL_LIC_NO.Enabled = false;
        //        txtTIN_NO.Enabled = false;
        //        txtCST_NO.Enabled = false;
        //        txtGST_NO.Enabled = false;
        //        txtBANK_OPENING.Enabled = true;
        //        drpBankDrCr.Enabled = false;
        //    }
        //    else if (drpType.SelectedValue == "E" || drpType.SelectedValue == "O")
        //    {
        //        txtAC_NAME_E.Enabled = true;
        //        txtOPENING_BALANCE.Enabled = false;
        //        drpDrCr.Enabled = false;
        //        txtGROUP_CODE.Enabled = true;
        //        txtCOMMISSION.Enabled = false;
        //        txtADDRESS_E.Enabled = false;
        //        txtADDRESS_R.Enabled = false;
        //        txtCITY_CODE.Enabled = false;
        //        txtPINCODE.Enabled = false;
        //        txtLOCAL_LIC_NO.Enabled = false;
        //        txtTIN_NO.Enabled = false;
        //        txtCST_NO.Enabled = false;
        //        txtGST_NO.Enabled = false;
        //        txtBANK_AC_NO.Enabled = false;
        //        txtBANK_NAME.Enabled = false;
        //        txtEMAIL_ID.Enabled = false;
        //        txtEMAIL_ID_CC.Enabled = false;
        //        txtECC_NO.Enabled = false;
        //        txtRefBy.Enabled = false;
        //        txtOffPhone.Enabled = false;
        //        txtcompanyPan.Enabled = false;
        //        txtfax.Enabled = false;
        //        btnOpenDetailsPopup.Enabled = false;
        //        grdGroup.Enabled = false;
        //        txtAC_RATE.Enabled = false;
        //        txtOTHER_NARRATION.Enabled = false;
        //        chkCarporate.Enabled = false;
        //    }
        //    else if (s_item != "B")
        //    {
        //        txtCOMMISSION.Enabled = true;
        //        txtADDRESS_E.Enabled = true;
        //        txtADDRESS_R.Enabled = true;
        //        txtCITY_CODE.Enabled = true;
        //        txtPINCODE.Enabled = true;
        //        txtOPENING_BALANCE.Enabled = true;
        //        drpDrCr.Enabled = true;
        //        txtLOCAL_LIC_NO.Enabled = true;
        //        txtTIN_NO.Enabled = true;
        //        txtCST_NO.Enabled = true;
        //        txtGST_NO.Enabled = true;
        //        txtBANK_OPENING.Enabled = true;
        //        drpBankDrCr.Enabled = true;
        //        txtAC_NAME_R.Enabled = false;
        //        txtECC_NO.Enabled = true;
        //    }

        //    if (s_item == "B")
        //    {
        //        txtAC_NAME_R.Enabled = false;
        //        txtCOMMISSION.Enabled = false;
        //        chkCarporate.Enabled = false;
        //        txtLOCAL_LIC_NO.Enabled = false;
        //        txtTIN_NO.Enabled = false;
        //        txtGST_NO.Enabled = false;
        //        txtECC_NO.Enabled = false;
        //        txtCST_NO.Enabled = false;
        //        txtBANK_OPENING.Enabled = true;
        //        drpBankDrCr.Enabled = true;
        //    }
        //    else
        //    {
        //        txtBANK_OPENING.Enabled = false;
        //        drpBankDrCr.Enabled = false;
        //    }

        //    //Enabling Required controls to true
        //    if (s_item == "F")
        //    {
        //        FixedAssetsControls();
        //    }
        //    if (s_item == "F" || s_item == "I")
        //    {

        //        setFocusControl(txtAC_RATE);
        //        txtAC_RATE.Enabled = true;
        //    }
        //    else
        //    {
        //        txtAC_RATE.Enabled = false;
        //    }
        //}
        #endregion
    }

    private void FixedAssetsControls()
    {
        txtAC_NAME_E.Enabled = true;
        txtOPENING_BALANCE.Enabled = true;
        drpDrCr.SelectedValue = "D";
        txtGROUP_CODE.Enabled = true;
        drpDrCr.Enabled = false;
        txtCOMMISSION.Enabled = false;
        txtADDRESS_E.Enabled = false;
        txtADDRESS_R.Enabled = false;
        txtCITY_CODE.Enabled = false;
        txtPINCODE.Enabled = false;
        txtLOCAL_LIC_NO.Enabled = false;
        txtTIN_NO.Enabled = false;
        txtCST_NO.Enabled = false;
        txtGST_NO.Enabled = false;
        txtBANK_AC_NO.Enabled = false;
        txtBANK_NAME.Enabled = false;
        txtEMAIL_ID.Enabled = false;
        txtEMAIL_ID_CC.Enabled = false;
        txtECC_NO.Enabled = false;
        txtRefBy.Enabled = false;
        txtOffPhone.Enabled = false;
        txtcompanyPan.Enabled = false;
        txtfax.Enabled = false;
        btnOpenDetailsPopup.Enabled = false;
        grdGroup.Enabled = false;
        txtAC_RATE.Enabled = false;
        txtOTHER_NARRATION.Enabled = false;
        chkCarporate.Enabled = false;
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            #region  txtAC_CODE
            if (strTextbox == "txtAC_CODE")
            {
                #region code
                try
                {
                    int n;
                    bool isNumeric = int.TryParse(txtAC_CODE.Text, out n);

                    if (isNumeric == true)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string txtValue = "";
                        if (txtAC_CODE.Text != string.Empty)
                        {
                            txtValue = txtAC_CODE.Text;

                            string qry = "select * from " + tblHead + " where  Ac_Code='" + txtValue + "' " +
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        //Record Found
                                        hdnf.Value = dt.Rows[0]["Ac_Code"].ToString();
                                        if (ViewState["mode"] != null)
                                        {
                                            if (ViewState["mode"].ToString() == "I")
                                            {
                                                lblMsg.Text = "** Ac_Code (" + txtValue + ") Already Exist";
                                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                                btnSave.Enabled = true;   //IMP
                                                this.getMaxCode();
                                                setFocusControl(drpType);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = this.getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtAC_CODE.Enabled = false;
                                                    setFocusControl(drpType);
                                                    hdnf.Value = txtAC_CODE.Text;
                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtAC_CODE);
                                            txtAC_CODE.Enabled = false;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtAC_CODE.Text = string.Empty;
                                            setFocusControl(txtAC_CODE);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                            setFocusControl(txtAC_CODE);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Ac code is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtAC_CODE.Text = string.Empty;
                        setFocusControl(txtAC_CODE);
                    }
                }
                catch
                {

                }
                #endregion
            }
            #endregion

            if (strTextbox == "txtSendingAcCode")
            {
                bool a = clsCommon.isStringIsNumeric(txtSendingAcCode.Text);
                if (a == false)
                {
                    btntxtSendingAcCode_Click(this, new EventArgs());
                }
                else
                {
                    string SendingAcCode = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + txtSendingAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (SendingAcCode != string.Empty)
                    {
                        txtSendingEmail.Text = clsCommon.getString("select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=" + txtSendingAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txtSendingMobile.Text = clsCommon.getString("select Mobile_No from " + tblPrefix + "AccountMaster where Ac_Code=" + txtSendingAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (SendingAcCode.Length >= 10)
                        {
                            lblSendingAcCode.ToolTip = SendingAcCode;
                            SendingAcCode = SendingAcCode.Substring(0, 10);
                        }
                        lblSendingAcCode.Text = SendingAcCode;
                        setFocusControl(btnSendSMS);
                    }
                    else
                    {
                        txtSendingAcCode.Text = string.Empty;
                        lblSendingAcCode.Text = SendingAcCode;
                        setFocusControl(txtSendingAcCode);
                    }
                }
            }
            else
            {
                lblSendingAcCode.Text = "";
                setFocusControl(txtSendingAcCode);
            }

            try
            {
                if (drpType.SelectedValue != "F" || drpType.SelectedValue != "E" || drpType.SelectedValue != "O")
                {
                    if (strTextbox == "txtAC_NAME_E")
                    {
                        setFocusControl(txtAC_NAME_R);
                    }
                    if (strTextbox == "txtAC_NAME_R")
                    {
                        if (drpType.SelectedValue == "F" || drpType.SelectedValue == "O" || drpType.SelectedValue == "E")
                        {
                            setFocusControl(txtSHORT_NAME);
                        }
                        else
                        {
                            setFocusControl(txtCOMMISSION);
                        }
                    }
                    if (strTextbox == "txtAC_RATE")
                    {
                        setFocusControl(txtAC_NAME_E);
                    }
                    if (strTextbox == "txtCOMMISSION")
                    {
                        setFocusControl(txtSHORT_NAME);
                    }
                    if (strTextbox == "txtSHORT_NAME")
                    {
                        if (drpType.SelectedValue == "E" || drpType.SelectedValue == "O")
                        {
                            setFocusControl(txtGROUP_CODE);
                        }
                        else if (drpType.SelectedValue == "F")
                        {
                            setFocusControl(txtOPENING_BALANCE);
                        }
                        else
                        {
                            setFocusControl(txtADDRESS_E);
                        }
                    }
                    if (strTextbox == "txtADDRESS_E")
                    {
                        setFocusControl(txtADDRESS_R);
                    }
                    if (strTextbox == "txtADDRESS_R")
                    {
                        setFocusControl(txtCITY_CODE);
                    }
                    if (strTextbox == "txtCITY_CODE")
                    {
                        if (txtCITY_CODE.Text != string.Empty)
                        {
                            bool a = clsCommon.isStringIsNumeric(txtCITY_CODE.Text);
                            if (a == false)
                            {
                                btntxtCITY_CODE_Click(this, new EventArgs());
                            }
                            else
                            {
                                string str = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code=" + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                if (str != string.Empty)
                                {
                                    if (str.Length > 14)
                                    {
                                        str = str.Substring(1, 14);
                                    }
                                    else if (str.Length > 12)
                                    {
                                        str = str.Substring(1, 12);
                                    }
                                    else if (str.Length > 8)
                                    {
                                        str = str.Substring(1, 8);
                                    }
                                    lblCITYNAME.Text = str;
                                    setFocusControl(txtPINCODE);
                                }
                                else
                                {
                                    lblCITYNAME.Text = str;
                                    txtCITY_CODE.Text = string.Empty;
                                    setFocusControl(txtCITY_CODE);
                                }
                            }
                        }
                        else
                        {
                            setFocusControl(txtCITY_CODE);
                        }
                    }
                    if (strTextbox == "txtGstStateCode")
                    {
                        if (txtGstStateCode.Text != string.Empty)
                        {
                            bool a = clsCommon.isStringIsNumeric(txtGstStateCode.Text);
                            if (a == false)
                            {
                                btntxtGstStateCode_Click(this, new EventArgs());
                            }
                            else
                            {
                                string str = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");
                                if (str != string.Empty)
                                {
                                    lbltxtGstStateName.Text = str;
                                    setFocusControl(txtOPENING_BALANCE);
                                }
                                else
                                {
                                    lbltxtGstStateName.Text = str;
                                    txtGstStateCode.Text = string.Empty;
                                    setFocusControl(txtGstStateCode);
                                }
                            }
                        }
                        else
                        {
                            setFocusControl(txtGstStateCode);
                        }
                    }

                    if (strTextbox == "txtPINCODE")
                    {
                        setFocusControl(txtOPENING_BALANCE);
                    }
                    if (strTextbox == "txtOPENING_BALANCE")
                    {
                        if (drpType.SelectedValue == "F")
                        {
                            setFocusControl(txtGROUP_CODE);
                        }
                        else
                        {
                            setFocusControl(drpDrCr);
                        }
                    }
                    if (strTextbox == "drpDrCr")
                    {
                        setFocusControl(txtGROUP_CODE);
                    }

                    if (strTextbox == "txtLOCAL_LIC_NO")
                    {
                        setFocusControl(txtTIN_NO);
                    }
                    if (strTextbox == "txtTIN_NO")
                    {
                        setFocusControl(txtCST_NO);
                    }
                    if (strTextbox == "txtCST_NO")
                    {
                        setFocusControl(txtGST_NO);
                    }
                    if (strTextbox == "txtGST_NO")
                    {
                        setFocusControl(txtOTHER_NARRATION);
                    }
                    if (strTextbox == "txtOTHER_NARRATION")
                    {
                        setFocusControl(txtRefBy);
                    }
                    if (strTextbox == "txtBANK_NAME")
                    {
                        setFocusControl(txtIfsc);
                    }
                    if (strTextbox == "txtIfsc")
                    {
                        setFocusControl(txtBANK_AC_NO);
                    }

                    if (strTextbox == "txtBANK_AC_NO")
                    {
                        setFocusControl(txtEMAIL_ID);
                    }
                    if (strTextbox == "txtEMAIL_ID")
                    {
                        setFocusControl(txtEMAIL_ID_CC);
                    }
                    if (strTextbox == "txtEMAIL_ID_CC")
                    {
                        setFocusControl(txtECC_NO);
                    }
                    if (strTextbox == "txtECC_NO")
                    {
                        setFocusControl(txtFssaiNo);
                    }
                    if (strTextbox == "txtFssaiNo")
                    {
                        setFocusControl(txtBANK_NAME);
                    }
                    if (strTextbox == "txtRefBy")
                    {
                        setFocusControl(txtOffPhone);
                    }
                    if (strTextbox == "txtOffPhone")
                    {
                        setFocusControl(txtcompanyPan);
                    }
                    if (strTextbox == "txtcompanyPan")
                    {
                        setFocusControl(txtfax);
                    }
                    if (strTextbox == "txtPERSON_NAME")
                    {
                        setFocusControl(txtPERSON_MOBILE);
                    }
                    if (strTextbox == "txtPERSON_MOBILE")
                    {
                        setFocusControl(txtPERSON_EMAIL);
                    }
                    if (strTextbox == "txtPERSON_EMAIL")
                    {
                        setFocusControl(txtPerson_PAN);
                    }
                    if (strTextbox == "txtPerson_PAN")
                    {
                        setFocusControl(txtPERSON_OTHER);
                    }
                    if (strTextbox == "txtPERSON_OTHER")
                    {
                        setFocusControl(btnAdddetails);
                    }
                    if (strTextbox == "txtRefBy")
                    {
                        setFocusControl(txtOffPhone);
                    }
                    if (strTextbox == "txtOffPhone")
                    {
                        setFocusControl(txtcompanyPan);
                    }
                    if (strTextbox == "txtcompanyPan")
                    {
                        setFocusControl(txtfax);
                    }
                    if (strTextbox == "txtfax")
                    {
                        setFocusControl(txtMOBILE);
                    }
                    if (strTextbox == "txtMOBILE")
                    {
                        setFocusControl(btnOpenDetailsPopup);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (strTextbox == "txtGROUP_CODE")
            {
                if (txtGROUP_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGROUP_CODE.Text);
                    if (a == false)
                    {
                        btntxtGROUP_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        string s = clsCommon.getString("select group_Name_E from " + GroupMasterTable + " where group_Code=" + txtGROUP_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (s != string.Empty)
                        {
                            if (s.Length > 14)
                            {
                                s = s.Substring(0, 14);
                            }
                            else if (s.Length > 13)
                            {
                                s = s.Substring(0, 13);
                            }
                            else if (s.Length > 12)
                            {
                                s = s.Substring(0, 12);
                            }
                            else if (s.Length > 11)
                            {
                                s = s.Substring(0, 11);
                            }
                            else if (s.Length > 10)
                            {
                                s = s.Substring(0, 10);
                            }
                            else if (s.Length > 9)
                            {
                                s = s.Substring(0, 9);
                            }
                            else if (s.Length > 5)
                            {
                                s = s.Substring(0, 5);
                            }
                            lblGROUPNAME.Text = s;
                            setFocusControl(txtLOCAL_LIC_NO);
                        }
                        else
                        {
                            lblGROUPNAME.Text = string.Empty;
                            txtGROUP_CODE.Text = string.Empty;
                            setFocusControl(txtGROUP_CODE);
                        }
                    }
                }
                else
                {
                    lblGROUPNAME.Text = string.Empty;
                    txtGROUP_CODE.Text = string.Empty;
                    setFocusControl(txtGROUP_CODE);
                }
            }
            #region always check
            if (btnSave.Enabled == true)
            {
                string s_item = drpType.SelectedValue;
                if (s_item == "T")
                {
                    //    txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = false;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = false;
                    //     txtADDRESS_R.Text = "";
                    txtADDRESS_R.Enabled = false;
                    //     txtCITY_CODE.Text = "";
                    //      lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = false;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = false;
                    //     txtOPENING_BALANCE.Text = "";
                    txtOPENING_BALANCE.Enabled = false;
                    drpDrCr.Enabled = false;
                    //    txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = false;
                    //   txtTIN_NO.Text = "";
                    txtTIN_NO.Enabled = false;
                    //   txtCST_NO.Text = "";
                    txtCST_NO.Enabled = false;
                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = false;
                    //    txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = false;
                    drpBankDrCr.Enabled = false;
                }
                else if (drpType.SelectedValue == "F" || drpType.SelectedValue == "O" || drpType.SelectedValue == "E")
                {
                    //  txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = false;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = false;
                    //    txtADDRESS_R.Text = "";
                    txtADDRESS_R.Enabled = false;
                    //    txtCITY_CODE.Text = "";
                    //    lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = false;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = false;
                    //     txtOPENING_BALANCE.Text = "";
                    //txtOPENING_BALANCE.Enabled = false;
                    drpDrCr.Enabled = false;
                    //   txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = false;
                    //   txtTIN_NO.Text = "";
                    txtTIN_NO.Enabled = false;
                    //   txtCST_NO.Text = "";
                    txtCST_NO.Enabled = false;
                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = false;
                    //     txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = false;
                    drpBankDrCr.Enabled = true;
                }
                else
                {
                    //  txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = true;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = true;
                    //    txtADDRESS_R.Text = "";
                    txtADDRESS_R.Enabled = true;
                    //    txtCITY_CODE.Text = "";
                    //    lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = true;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = true;
                    //     txtOPENING_BALANCE.Text = "";
                    txtOPENING_BALANCE.Enabled = true;
                    drpDrCr.Enabled = true;
                    //   txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = true;
                    //   txtTIN_NO.Text = "";
                    txtTIN_NO.Enabled = true;
                    //   txtCST_NO.Text = "";
                    txtCST_NO.Enabled = true;
                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = true;
                    //     txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = true;
                    drpBankDrCr.Enabled = true;
                }
                if (s_item == "B")
                {
                    //   txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = true;
                    drpBankDrCr.Enabled = true;
                }
                else
                {
                    //   txtBANK_OPENING.Text = "";
                    txtBANK_OPENING.Enabled = false;
                    drpBankDrCr.Enabled = false;
                }

                if (s_item == "F" || s_item == "I")
                {
                    //  txtAC_RATE.Text = "";
                    txtAC_RATE.Enabled = true;
                }
                else
                {
                    // txtAC_RATE.Text = "";
                    txtAC_RATE.Enabled = false;
                }
            }


            #endregion
        }
        catch
        {
        }
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    #endregion

    protected void FileUpload_PAN_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        //string filePath = "~/PAN/" + clsGV.user + "_" + e.FileName;
        //FileUpload_PAN.SaveAs(Server.MapPath(filePath));
        //f_pan = filePath;
    }
    protected void grdDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
    protected void txtcompanyPan_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcompanyPan.Text;
        strTextbox = "txtcompanyPan";
        csCalculations();
    }
    protected void txtMOBILE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMOBILE.Text;
        strTextbox = "txtMOBILE";
        csCalculations();

    }
    protected void txtfax_TextChanged(object sender, EventArgs e)
    {
        searchString = txtfax.Text;
        strTextbox = "txtfax";
        csCalculations();
    }
    protected void txtOffPhone_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOffPhone.Text;
        strTextbox = "txtOffPhone";
        csCalculations();

    }
    protected void txtRefBy_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRefBy.Text;
        strTextbox = "txtRefBy";
        csCalculations();
    }
    protected void txtFssaiNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFssaiNo.Text;
        strTextbox = "txtFssaiNo";
        csCalculations();
    }
    protected void txtIfsc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIfsc.Text;
        strTextbox = "txtIfsc";
        csCalculations();
    }
    protected void btnAddCity_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ViewState["mode"].ToString() == "I")
            //{
            string maxcity = clsCommon.getString("Select ISNULL(MAX(city_code+1),1) from  " + tblPrefix + "CityMaster where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtCityCode.Text = maxcity;
            txtCityName.Text = string.Empty;
            txtRegionalName.Text = string.Empty;
            setFocusControl(txtCityName);
            modalCity.Show();
            //}
        }
        catch (Exception)
        {

            throw;
        }

    }
    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnSaveCity_Click(object sender, EventArgs e)
    {
        try
        {
            string citycode = txtCityCode.Text;
            string cityname = txtCityName.Text;
            string citynamer = txtRegionalName.Text;
            string state = txtState.Text;
            bool isValidated = true;
            if (!string.IsNullOrEmpty(txtCityName.Text))
            {
                string str = clsCommon.getString("select city_code from " + tblHead + " where  city_code='" + txtCityCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    lblErr.Text = "Doc No " + txtCityCode.Text + " already exist";
                    string maxcity = clsCommon.getString("Select ISNULL(MAX(city_code+1),1) from  " + tblPrefix + "CityMaster where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    txtCityCode.Text = maxcity;
                    modalCity.Show();
                    isValidated = true;
                    return;
                }
                else
                {
                    isValidated = true;
                }
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    string strRev = "";
                    obj.flag = 1;
                    obj.tableName = tblPrefix + "CityMaster";
                    obj.columnNm = "city_code,city_name_e,city_name_r,company_code,state,Created_By";
                    obj.values = "'" + citycode + "','" + cityname + "','" + citynamer + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + state + "','" + user + "'";
                    DataSet ds = new DataSet();
                    ds = obj.insertAccountMaster(ref strRev);
                    txtCITY_CODE.Text = citycode;
                    lblCITYNAME.Text = cityname;
                }
            }
            else
            {
                lblErr.Text = "City Name Is Reuired!";
                setFocusControl(txtCityName);
                modalCity.Show();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void drpgroupSection_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void drpGroupSummary_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnAddGroup_Click1(object sender, EventArgs e)
    {
        try
        {
            string GroupCode = clsCommon.getString("Select ISNULL(MAX(group_Code+1),1) from  " + tblPrefix + "BSGroupMaster where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtGroupCode.Text = GroupCode;
            ViewState["group"] = "S";
            txtGroupName.Text = string.Empty;
            setFocusControl(txtGroupName);
            ModalGroupMaster.Show();
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btnSaveGroup_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtGroupCode.Text != string.Empty)
            {
                if (ViewState["group"].ToString() == "S")
                {
                    string str = clsCommon.getString("select group_Name_E from " + tblPrefix + "BSGroupMaster where group_Code=" + txtGroupCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblGropCodeexist.Text = "Code " + txtGroupCode.Text + " already exist";
                        isValidated = false;
                        return;
                    }
                    else
                    {
                        isValidated = true;
                    }
                }


            }
            string GroupCod = txtGroupCode.Text;
            string GroupName = txtGroupName.Text;
            string group_type = drpgroupSection.SelectedValue;
            string group_Summary = drpGroupSummary.SelectedValue;
            Int32 group_order = txtGroupOrder.Text != string.Empty ? Convert.ToInt32(txtGroupOrder.Text) : 0;
            string user = Session["user"].ToString();

            if (isValidated == true)
            {
                if (!string.IsNullOrEmpty(txtGroupName.Text))
                {
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        string strRev = "";
                        DataSet ds = new DataSet();
                        obj.flag = 1;
                        obj.tableName = tblPrefix + "BSGroupMaster";
                        obj.columnNm = "group_Code,group_Name_E,group_Type,group_Summary,group_Order,Company_Code,Created_By";
                        obj.values = "'" + GroupCod + "','" + GroupName + "','" + group_type + "','" + group_Summary + "','" + group_order + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + user + "'";
                        ds = obj.insertAccountMaster(ref strRev);
                        txtGROUP_CODE.Text = GroupCod;
                        lblGROUPNAME.Text = GroupName;
                    }
                }
                else
                {
                    lblGrr.Text = "Required!";
                    ModalGroupMaster.Show();
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void txtBranch1OB_TextChanged(object sender, EventArgs e)
    {
        OBCalculation();
    }

    protected void txtBranch2OB_TextChanged(object sender, EventArgs e)
    {
        OBCalculation();
    }
    protected void drpBranch1Drcr_SelectedIndexChanged(object sender, EventArgs e)
    {
        OBCalculation();
    }
    protected void drpBranch2Drcr_SelectedIndexChanged(object sender, EventArgs e)
    {
        OBCalculation();
    }
    public void OBCalculation()
    {
        try
        {
            double MyOpBal = 0.0;
            double Branch1OpBal = txtBranch1OB.Text != string.Empty ? Convert.ToDouble(txtBranch1OB.Text) : 0.00;
            double Branch2OpBal = txtBranch2OB.Text != string.Empty ? Convert.ToDouble(txtBranch2OB.Text) : 0.00;
            string Branch1DrCr = drpBranch1Drcr.SelectedValue.ToString();
            string Branch2DrCr = drpBranch2Drcr.SelectedValue.ToString();
            if (Branch1DrCr == "D")
            {
                MyOpBal = Branch1OpBal;
            }
            else
            {
                MyOpBal = -Branch1OpBal;
            }

            if (Branch2DrCr == "D")
            {
                MyOpBal += Branch2OpBal;
            }
            else
            {
                MyOpBal -= Branch2OpBal;
            }

            if (MyOpBal > 0)
            {
                drpDrCr.SelectedValue = "D";
            }
            else
            {
                drpDrCr.SelectedValue = "C";
            }
            txtOPENING_BALANCE.Text = Convert.ToString(Math.Abs(MyOpBal));

        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void btnSendSMS_Click(object sender, CommandEventArgs e)
    {
        try
        {
            string PartyName = txtAC_NAME_E.Text;
            string PartyAddress = txtADDRESS_E.Text;
            string CityName = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            CityName = CityName == string.Empty ? Convert.ToString(CityName) : " City: " + CityName;
            string State = clsCommon.getString("Select state from " + tblPrefix + "CityMaster where city_code=" + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            State = State == string.Empty ? Convert.ToString(State) : " State: " + State;
            string PINCODE = txtPINCODE.Text == string.Empty ? Convert.ToString(txtPINCODE.Text) : " PINCODE: " + txtPINCODE.Text.Trim();
            string LOCAL_LIC_NO = txtLOCAL_LIC_NO.Text == string.Empty ? Convert.ToString(txtLOCAL_LIC_NO.Text) : "LIC: " + txtLOCAL_LIC_NO.Text + Environment.NewLine + ",";
            string TIN_NO = txtTIN_NO.Text.Trim() == string.Empty ? Convert.ToString(txtTIN_NO.Text) : "TIN: " + txtTIN_NO.Text + Environment.NewLine + ",";
            string CST_NO = txtCST_NO.Text.Trim() == string.Empty ? Convert.ToString(txtCST_NO.Text) : "CST: " + txtCST_NO.Text + Environment.NewLine + ",";
            string GST_NO = txtGST_NO.Text.Trim() == string.Empty ? Convert.ToString(txtGST_NO.Text) : "GST: " + txtGST_NO.Text + Environment.NewLine + ",";
            string ECC_NO = txtECC_NO.Text.Trim() == string.Empty ? Convert.ToString(txtECC_NO.Text) : "ECC: " + txtECC_NO.Text + Environment.NewLine + ",";
            string FSSAI = txtFssaiNo.Text.Trim() == string.Empty ? Convert.ToString(txtFssaiNo.Text) : "FSSAI: " + txtFssaiNo.Text + Environment.NewLine + ",";
            string PAN = txtcompanyPan.Text.Trim() == string.Empty ? Convert.ToString(txtcompanyPan.Text) : "PAN: " + txtcompanyPan.Text + Environment.NewLine + ",";
            string MOBILE = txtSendingMobile.Text;
            string EMAIL_ID = txtSendingEmail.Text;

            string BankName = txtBANK_NAME.Text == string.Empty ? Convert.ToString(txtBANK_NAME.Text) : "Bank Name: " + txtBANK_NAME.Text + ",";
            string BankAc_number = txtBANK_AC_NO.Text == string.Empty ? Convert.ToString(txtBANK_AC_NO.Text) : "Bank A/c Number: " + txtBANK_AC_NO.Text + ",";
            string BankIFSCode = txtIfsc.Text == string.Empty ? Convert.ToString(txtIfsc.Text) : "IFSC: " + txtIfsc.Text + ",";
            string msg = string.Empty;
            string MsgforMail = string.Empty;
            if (chkBankDetails.Checked == true)
            {
                msg = "Bank Details Of Party <br/>" + PartyName + " " + CityName + " " + PINCODE + " " + State + BankName + " " + BankAc_number + " " + BankIFSCode;
                MsgforMail = "Bank Details Of Party <br/>" + PartyName + " <br/>" + BankName + " <br/>" + BankAc_number + " <br/>" + BankIFSCode;
            }
            if (chkAddressDetails.Checked == true)
            {
                msg = PartyName + " Address:" + PartyAddress + " " + CityName + " " + State + Environment.NewLine + LOCAL_LIC_NO + TIN_NO +
                    CST_NO + GST_NO + ECC_NO + FSSAI + PAN;
                MsgforMail = PartyName + "<br/> Address:" + PartyAddress + " <br/>" + CityName + " <br/>" + State + " <br/>" + PINCODE + " <br/>" + LOCAL_LIC_NO + " <br/>" + TIN_NO + " <br/>" + CST_NO + " <br/>" + GST_NO + " <br/>" + ECC_NO + " <br/>" + FSSAI + " <br/>" + PAN + " <br/>";
            }
            if (e.CommandName == "sms")
            {
                if (!string.IsNullOrWhiteSpace(MOBILE))
                {
                    string msgAPI = clsGV.msgAPI;
                    string URL = msgAPI + "mobile=" + MOBILE + "&message=" + msg;
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string read = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sms", "javascript:alert('SMS  Sent Successfully!')", true);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(EMAIL_ID))
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            string mailFrom = Session["EmailId"].ToString();
                            string smtpPort = "587";
                            string emailPassword = Session["EmailPassword"].ToString();
                            MailMessage msgs = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                            SmtpServer.Host = clsGV.Email_Address;
                            msgs.From = new MailAddress(mailFrom);
                            msgs.To.Add(EMAIL_ID);
                            msgs.Body = MsgforMail;
                            //msgs.Attachments.Add(attachment);
                            msgs.IsBodyHtml = true;
                            msgs.Subject = "Account details...";
                            msgs.IsBodyHtml = true;
                            if (smtpPort != string.Empty)
                            {
                                SmtpServer.Port = Convert.ToInt32(smtpPort);
                            }
                            SmtpServer.EnableSsl = true;
                            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            };
                            SmtpServer.Send(msgs);
                        }
                    }
                    catch (Exception e1)
                    {
                        Response.Write("mail err:" + e1);
                        return;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sms", "javascript:alert('Email Sent Successfully!')", true);
                }
            }

            //ClearSendingSmsTextboxes();

        }
        catch (Exception)
        {
            throw;
        }
    }

    private void ClearSendingSmsTextboxes()
    {
        txtSendingAcCode.Text = string.Empty;
        txtSendingMobile.Text = string.Empty;
        txtSendingEmail.Text = string.Empty;
        lblSendingAcCode.Text = string.Empty;
        chkAddressDetails.Checked = false;
        chkBankDetails.Checked = false;
    }

    protected void txtSendingAcCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSendingAcCode.Text;
        strTextbox = "txtSendingAcCode";
        csCalculations();

    }
    protected void btntxtSendingAcCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSendingAcCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void chkAddressDetails_CheckedChanged(object sender, EventArgs e)
    {
        chkBankDetails.Checked = false;
    }
    protected void chkBankDetails_CheckedChanged(object sender, EventArgs e)
    {
        chkAddressDetails.Checked = false;
    }

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
            if (a == false)
            {
                txtSearchText.Text = txtEditDoc_No.Text.ToString();
                strTextbox = "txtAC_CODE";
                btntxtAC_CODE_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                string qry = "select * from " + qryCommon + " where Ac_Code='" + txtEditDoc_No.Text + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region [txtGstStateCode_TextChanged]
    protected void txtGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGstStateCode.Text;
        strTextbox = "txtGstStateCode";
        csCalculations();
    }
    #endregion

    #region [btntxtGstStateCode_Click]
    protected void btntxtGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
}

