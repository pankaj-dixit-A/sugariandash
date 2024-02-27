using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sugar_pgeCarporateReciept : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "Transact";
            tblDetails = tblPrefix + "";
            AccountMasterTable = tblPrefix + "AccountMaster";
            //qryCommon = tblPrefix + "qryTransactList";
            qryAccountList = tblPrefix + "qryAccountsList";
            cityMasterTable = tblPrefix + "CityMaster";
            systemMasterTable = tblPrefix + "SystemMaster";
            voucherTable = tblPrefix + "Voucher";
            qryVoucherList = tblPrefix + "qryVoucherList";
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
                    // this.showLastRecord();
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
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btnGetvouchers.Enabled = false;
                lblMsg.Text = string.Empty;
                txtBalance.Enabled = false;
                calenderExtenderDate.Enabled = false;
                //btnDelete.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                drpTrnType.Enabled = true;
                drpPaymentFor.Enabled = true;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                //btnDelete.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                txtBalance.Enabled = false;
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
                pnlgrdDetail.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnAdd.Enabled = false;
                btntxtCashBank.Enabled = true;
                btntxtACCode.Enabled = true;
                btnGetvouchers.Enabled = true;
                drpTrnType.Enabled = false;
                drpPaymentFor.Enabled = false;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                trntype = drpTrnType.SelectedValue;
                txtBalance.Enabled = false;
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
                pnlgrdDetail.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                drpTrnType.Enabled = true;
                drpPaymentFor.Enabled = true;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btnGetvouchers.Enabled = false;
                txtBalance.Enabled = false;
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
                pnlgrdDetail.Enabled = true;
                txtCashBank.Enabled = true;
                btntxtCashBank.Enabled = true;
                drpTrnType.Enabled = false;
                drpPaymentFor.Enabled = false;
                txtBalance.Enabled = false;
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
        Response.Redirect("~/Sugar/pgeCarporateReciept.aspx", false);
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
        int i = 0;
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        e.Row.Cells[0].ControlStyle.Width = new Unit("44px");
        e.Row.Cells[1].ControlStyle.Width = new Unit("44px");
        e.Row.Cells[2].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[3].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[4].ControlStyle.Width = new Unit("250px");
        e.Row.Cells[5].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[6].ControlStyle.Width = new Unit("250px");
        e.Row.Cells[7].ControlStyle.Width = new Unit("200px");
        e.Row.Cells[8].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[9].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[10].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[11].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[12].ControlStyle.Width = new Unit("300px");
        e.Row.Cells[13].ControlStyle.Width = new Unit("300px");
        e.Row.Cells[14].ControlStyle.Width = new Unit("300px");

        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        foreach (TableCell cell in e.Row.Cells)
        {
            string s = cell.Text.ToString();
            if (cell.Text.Length > 22)
            {
                cell.Text = cell.Text.Substring(0, 22) + "..";
                cell.ToolTip = s;
            }
        }


        //}
        try
        {
            //e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(4);
            //e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
            //e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(7);
            //e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(22);
            //e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(22);
            //e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(7);
            //e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(7);
            //e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[11].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[12].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[13].ControlStyle.Width = Unit.Percentage(25);
            //e.Row.Cells[14].ControlStyle.Width = Unit.Percentage(25);

            //e.Row.Cells[0].Style["overflow"] = "hidden";
            //e.Row.Cells[1].Style["overflow"] = "hidden";
            //e.Row.Cells[2].Style["overflow"] = "hidden";
            //e.Row.Cells[3].Style["overflow"] = "hidden";
            //e.Row.Cells[4].Style["overflow"] = "hidden";
            //e.Row.Cells[11].Style["overflow"] = "hidden";
            //e.Row.Cells[6].Style["overflow"] = "hidden";
            //e.Row.Cells[7].Style["overflow"] = "hidden";
            //e.Row.Cells[8].Style["overflow"] = "hidden";
            //e.Row.Cells[9].Style["overflow"] = "hidden";
            //e.Row.Cells[10].Style["overflow"] = "hidden";
            //e.Row.Cells[12].Style["overflow"] = "hidden";

            //int i = 0;
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Left;

            //    if (e.Row.Cells[13].Text.Length > 27)
            //    {
            //        e.Row.Cells[13].Style["overflow"] = "hidden";
            //        string s = e.Row.Cells[13].Text.ToString();
            //        //e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 27) + "..";
            //        e.Row.Cells[13].ToolTip = s;
            //    }
            //}

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
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='B'");
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

        string drpFilterValue = "A";
        if (drpPaymentFor.SelectedValue == "T")
        {
            drpFilterValue = "T";
        }
        else
        {
            drpFilterValue = "V";
        }
        #endregion-End of Head part declearation

        #region save Head Master
        using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
        {
            DataSet ds = new DataSet();
            Int32 GID = 0;
            string qry = "";
            qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + drpTrnType.SelectedValue + "' and DOC_NO=" + DOC_NO + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
            ds = clsDAL.SimpleQuery(qry);
            int detail_id = 1;
            double grdUsedAmt = 0.00;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                Int32 vocherNo = Convert.ToInt32(grdDetail.Rows[i].Cells[0].Text);
                string voucherType = grdDetail.Rows[i].Cells[1].Text;
                Int32 acCode = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                string lorry = "";
                Int32 Unit_Code = 0;
                if (drpPaymentFor.SelectedValue == "T")
                {
                    lorry = Server.HtmlEncode(grdDetail.Rows[i].Cells[5].Text);
                }
                else
                {
                    Unit_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                }

                string unit_name = Server.HtmlEncode(grdDetail.Rows[i].Cells[6].Text);
                string qntl = grdDetail.Rows[i].Cells[8].Text;
                string rate = grdDetail.Rows[i].Cells[9].Text;
                TextBox txtgrdAmount = (TextBox)grdDetail.Rows[i].Cells[12].FindControl("txtgrdAmount");
                TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[i].Cells[13].FindControl("txtgrdAdjustedAmount");
                TextBox txtNarration = (TextBox)grdDetail.Rows[i].Cells[13].FindControl("txtNarration");

                double amount = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;
                double adAmount = txtgrdAdjustedAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAdjustedAmount.Text) : 0.00;
                string narration = "";
                if (drpPaymentFor.SelectedValue == "T")
                {
                    narration = voucherType + ":" + vocherNo + " Lorry:" + lorry + " Qntl:" + qntl + "-" + rate + " " + txtNarration.Text;
                }
                else
                {
                    narration = voucherType + ":" + vocherNo + " " + unit_name + " Qntl:" + qntl + " " + txtNarration.Text;
                }


                grdUsedAmt += amount;
                if (amount > 0)
                {
                    obj.flag = 1;
                    obj.tableName = tblHead;
                    obj.columnNm = "Tran_Type,DOC_NO,DOC_DATE,debit_ac,credit_ac,Unit_Code,amount,narration,Company_Code,Year_Code,Branch_Code,Created_By,Voucher_No,Voucher_Type,Adjusted_Amount,drpFilterValue";
                    obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','" + acCode + "','" + Unit_Code + "','" + amount + "','" + narration + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + vocherNo + "','" + voucherType + "','" + adAmount + "','" + drpFilterValue + "'";
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
                detail_id++;
            }
            double EnteredAmt = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0.00;
            double finalAmt = EnteredAmt - grdUsedAmt;

            if (finalAmt > 0)
            {
                obj.flag = 1;
                obj.tableName = tblHead;
                obj.columnNm = "Tran_Type,DOC_NO,DOC_DATE,debit_ac,credit_ac,Unit_Code,amount,narration,Company_Code,Year_Code,Branch_Code,Created_By,drpFilterValue";
                obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','" + txtACCode.Text + "','" + 0 + "','" + finalAmt + "','','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + user + "','" + drpFilterValue + "'";
                ds = obj.insertAccountMaster(ref strRev);
                retValue = strRev;

                #region GLedger Effect
                if (retValue == "-1")
                {
                    GID = GID + 1;
                    obj.flag = 1;
                    obj.tableName = GLedgerTable;
                    obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + txtACCode.Text + "','On Account','" + finalAmt + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + detail_id + "','" + drcr + "','" + cashBank + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + trntype + "','" + DOC_NO + "'";
                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,Branch_Code,SORT_TYPE,SORT_NO";
                    ds = obj.insertAccountMaster(ref strRev);

                    obj.values = "'" + trntype + "','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','','" + finalAmt + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + detail_id + "','" + drcr0 + "','" + txtACCode.Text + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + trntype + "','" + DOC_NO + "'";
                    obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,Branch_Code,SORT_TYPE,SORT_NO";
                    ds = obj.insertAccountMaster(ref strRev);
                }
                #endregion
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Successfully Saved !');", true);
            Response.Redirect("~/Sugar/pgeCarporateReciept.aspx", false);
        }
        #endregion
    }
    #endregion

    protected void txtgrdAmount_TextChanged(object sender, EventArgs e)
    {

        strTextBox = "txtgrdAmount";
        int index = RowIndex(sender);
        this.calculation(index);
    }
    protected void txtgrdAdjustedAmount_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtgrdAdjustedAmount";
        int index = RowIndex(sender);
        this.calculation(index);
    }
    protected void txtNarration_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtNarration";
        int index = RowIndex(sender);
        this.calculation(index);
    }

    protected void calculation(int index)
    {
        double Balance = Convert.ToDouble(grdDetail.Rows[index].Cells[11].Text);
        TextBox txtgrdAmount = (TextBox)grdDetail.Rows[index].Cells[12].FindControl("txtgrdAmount");
        TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[index].Cells[13].FindControl("txtgrdAdjustedAmount");
        TextBox txtNarration = (TextBox)grdDetail.Rows[index].Cells[13].FindControl("txtNarration");
        double grdamount = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;

        if (strTextBox == "txtgrdAmount")
        {
            double enterAmt = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0.00;
            if (enterAmt > 0)
            {
                if (grdamount > Balance)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Amount Is Greater Than Voucher Balance !');", true);
                    setFocusControl(txtgrdAmount);
                    return;
                }
                else
                {
                    double amtAnkush = 0.00;
                    for (int k = 0; k < grdDetail.Rows.Count; k++)
                    {
                        TextBox txtgrdAmount2 = (TextBox)grdDetail.Rows[k].Cells[12].FindControl("txtgrdAmount");
                        double amt2 = txtgrdAmount2.Text != string.Empty ? Convert.ToDouble(txtgrdAmount2.Text) : 0.00;
                        amtAnkush += amt2;
                    }
                    double amtfcuk = ((Convert.ToDouble(txtAmount.Text) - amtAnkush) + grdamount);
                    if (grdamount > amtfcuk)
                    {
                        txtBalance.Text = Convert.ToString(((Convert.ToDouble(txtAmount.Text) - amtAnkush) + grdamount));
                        double bal = Convert.ToDouble(txtBalance.Text);
                        txtBalance.Text = bal.ToString();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Entered Amount Is Greater Than Total Balance!');", true);
                        txtgrdAmount.Text = "0";
                        setFocusControl(txtgrdAmount);
                        return;
                    }
                    else
                    {
                        txtBalance.Text = Convert.ToString(Convert.ToDouble(txtAmount.Text) - amtAnkush);
                        double bal = Convert.ToDouble(txtBalance.Text);
                        txtBalance.Text = bal.ToString();
                    }
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Enter Amount!');", true);
                setFocusControl(txtAmount);
                txtgrdAmount.Text = "";
                return;
            }
        }
        if (strTextBox == "txtgrdAmount")
        {
            setFocusControl(txtgrdAdjustedAmount);
        }

        if (strTextBox == "txtgrdAdjustedAmount")
        {
            setFocusControl(txtNarration);
        }

        if (strTextBox == "txtNarration")
        {
            if (index < grdDetail.Rows.Count - 1)
            {
                TextBox txtgrdAmount1 = (TextBox)grdDetail.Rows[index + 1].Cells[12].FindControl("txtgrdAmount");
                setFocusControl(txtgrdAmount1);
            }
            else
            {
                setFocusControl(btnSave);
            }
        }
    }


    private static int RowIndex(object sender)
    {
        GridViewRow grow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = grow.RowIndex;
        return index;
    }



    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtdoc_no")
            {
                //#region code
                //try
                //{
                //    int n;
                //    bool isNumeric = int.TryParse(txtdoc_no.Text, out n);

                //    if (isNumeric == true)
                //    {
                //        DataSet ds = new DataSet();
                //        DataTable dt = new DataTable();
                //        string txtValue = "";
                //        if (txtdoc_no.Text != string.Empty)
                //        {
                //            txtValue = txtdoc_no.Text;

                //            string qry = "select * from " + tblHead + " where   Doc_No='" + txtValue + "' " +
                //                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'";

                //            ds = clsDAL.SimpleQuery(qry);
                //            if (ds != null)
                //            {
                //                if (ds.Tables.Count > 0)
                //                {
                //                    dt = ds.Tables[0];
                //                    if (dt.Rows.Count > 0)
                //                    {
                //                        //Record Found
                //                        hdnf.Value = dt.Rows[0]["doc_no"].ToString();

                //                        if (ViewState["mode"] != null)
                //                        {
                //                            if (ViewState["mode"].ToString() == "I")
                //                            {
                //                                lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                //                                lblMsg.ForeColor = System.Drawing.Color.Red;
                //                                this.getMaxCode();
                //                                txtdoc_no.Enabled = false;

                //                                btnSave.Enabled = true;   //IMP
                //                                setFocusControl(txtdoc_date);
                //                            }

                //                            if (ViewState["mode"].ToString() == "U")
                //                            {
                //                                //fetch record
                //                                qry = getDisplayQuery();

                //                                bool recordExist = this.fetchRecord(qry);
                //                                if (recordExist == true)
                //                                {

                //                                    txtdoc_no.Enabled = false;

                //                                }
                //                            }
                //                        }
                //                    }
                //                    else   //Record Not Found
                //                    {
                //                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                //                        {
                //                            lblMsg.Text = "";
                //                            setFocusControl(txtdoc_date);
                //                            txtdoc_no.Enabled = false;
                //                            btnSave.Enabled = true;   //IMP
                //                        }
                //                        if (ViewState["mode"].ToString() == "U")
                //                        {
                //                            this.makeEmptyForm("E");
                //                            lblMsg.Text = "** Record Not Found";
                //                            lblMsg.ForeColor = System.Drawing.Color.Red;
                //                            txtdoc_no.Text = string.Empty;
                //                            setFocusControl(txtdoc_no);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            lblMsg.Text = string.Empty;
                //            setFocusControl(txtdoc_no);
                //        }
                //    }
                //    else
                //    {
                //        this.makeEmptyForm("A");
                //        lblMsg.Text = "Doc No is numeric";
                //        lblMsg.ForeColor = System.Drawing.Color.Red;
                //        clsButtonNavigation.enableDisable("E");
                //        txtdoc_no.Text = string.Empty;
                //        setFocusControl(txtdoc_no);
                //    }
                //}
                //catch
                //{

                //}
                //#endregion
            }
            if (strTextBox == "txtdoc_date")
            {
                if (txtdoc_date.Text != string.Empty)
                {
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtACCode);
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
                        string str = "";
                        if (drpPaymentFor.SelectedValue == "T")
                        {
                            str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_type='T' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_type!='B' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }

                        if (str != string.Empty)
                        {
                            lblACName.Text = str;
                            setFocusControl(txtAmount);
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
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_type='B' and Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblCashBank.Text = str;
                            setFocusControl(txtACCode);
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
                string qry = "";
                if (drpPaymentFor.SelectedValue == "T")
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                        " (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                }
                else
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Ac_type!='B' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                        " (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                }
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
        //this.showLastRecord();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
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
    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        trntype = drpTrnType.SelectedValue;
        this.getMaxCode();
    }
    #endregion

    protected void txtUnit_Code_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtUnit_Code.Text;
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
    protected void btnGetvouchers_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtACCode.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtACCode);
                return;
            }
            if (drpPaymentFor.SelectedValue == "T")
            {
                qry = "select do.doc_no as doc_no,do.tran_type as Tran_Type,' ' as Suffix,Convert(varchar(10),do.doc_date,103) as Doc_Date,do.truck_no as Unit_Code,GetPassName as Unit_Name,do.millName as Mill_Short,do.transport as Party_Code,do.TransportName as PartyName,do.quantal as NETQNTL,do.Memo_Advance as Bill_Amount," +
                         " (Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code ) as Paid_Amount,((do.Memo_Advance)-" +
                         "(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance" +
                         " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and do.transport=" + txtACCode.Text + " and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            }
            else
            {
                qry = "select v.Doc_No as doc_no,v.Tran_Type,Convert(varchar(10),v.doc_date,103) as Doc_Date,v.Ac_Code as Party_Code,v.PartyName,v.Unit_Code,v.Unit_Name,v.NETQNTL," +
                                " a.Short_Name as Mill_Short,v.Sale_Rate,v.Bill_Amount,(Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where Voucher_No=v.Doc_No and Voucher_Type=v.Tran_Type " +
                                " and Company_Code=v.Company_Code and Year_Code=v.Year_Code ) as Paid_Amount,(v.Bill_Amount - (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where  Voucher_No=v.Doc_No and " +
                                " Voucher_Type=v.Tran_Type and Year_Code=v.Year_Code and Company_Code=v.Company_Code )) as Balance  from " + tblPrefix + "qryVoucherSaleUnion v left outer join " + tblPrefix + "AccountMaster a on v.mill_code=a.Ac_Code and " +
                                " v.Company_Code=a.Company_Code where v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and v.Ac_Code=" + txtACCode.Text + "";
            }
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("doc_no", typeof(string)));
                dt.Columns.Add(new DataColumn("Tran_Type", typeof(string)));
                dt.Columns.Add(new DataColumn("Doc_Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Party_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("PartyName", typeof(string)));
                dt.Columns.Add(new DataColumn("Unit_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Unit_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("NETQNTL", typeof(string)));
                dt.Columns.Add(new DataColumn("Mill_Short", typeof(string)));
                dt.Columns.Add(new DataColumn("Bill_Amount", typeof(double)));
                dt.Columns.Add(new DataColumn("Paid_Amount", typeof(double)));
                dt.Columns.Add(new DataColumn("Balance", typeof(double)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["doc_no"] = ds.Tables[0].Rows[i]["doc_no"].ToString();
                    dr["Tran_Type"] = ds.Tables[0].Rows[i]["Tran_Type"].ToString();
                    dr["Doc_Date"] = ds.Tables[0].Rows[i]["Doc_Date"].ToString();
                    dr["Party_Code"] = ds.Tables[0].Rows[i]["Party_Code"].ToString();
                    dr["PartyName"] = ds.Tables[0].Rows[i]["PartyName"].ToString();
                    dr["Unit_Code"] = ds.Tables[0].Rows[i]["Unit_Code"].ToString();
                    dr["Unit_Name"] = ds.Tables[0].Rows[i]["Unit_Name"].ToString();
                    dr["NETQNTL"] = ds.Tables[0].Rows[i]["NETQNTL"].ToString();
                    dr["Mill_Short"] = ds.Tables[0].Rows[i]["Mill_Short"].ToString();
                    dr["Bill_Amount"] = ds.Tables[0].Rows[i]["Bill_Amount"].ToString();
                    dr["Paid_Amount"] = ds.Tables[0].Rows[i]["Paid_Amount"].ToString();
                    double Balance = Convert.ToDouble(ds.Tables[0].Rows[i]["Balance"].ToString());
                    dr["Balance"] = Balance;
                    if (Balance != 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    grdDetail.DataSource = dt;
                    grdDetail.DataBind();
                    if (drpPaymentFor.SelectedValue == "T")
                    {
                        grdDetail.HeaderRow.Cells[5].Text = "Lorry";
                        grdDetail.HeaderRow.Cells[6].Text = "Dispatch_To";
                    }
                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                }
            }
            else
            {
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        txtBalance.Text = txtAmount.Text;
        setFocusControl(btnGetvouchers);
    }
    protected void drpPaymentFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string paymentFor = drpPaymentFor.SelectedValue;
        if (paymentFor == "T")
        {
            drpTrnType.Items.Clear();
            drpTrnType.Items.Add(new ListItem("Cash Payment", "CP"));
            drpTrnType.Items.Add(new ListItem("Bank Payment", "BP"));
            drpTrnType.SelectedIndex = 0;
        }
        if (paymentFor == "C")
        {
            drpTrnType.Items.Clear();
            drpTrnType.Items.Add(new ListItem("Bank Reciept", "BR"));
            drpTrnType.SelectedIndex = 0;
        }
    }
}