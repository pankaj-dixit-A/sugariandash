using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Report_pgeGroupLedger : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string AccountMasterTable = string.Empty;
    string searchStr = "";
    string strTextbox = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        user = Session["user"].ToString();
        tblPrefix = Session["tblPrefix"].ToString();
        AccountMasterTable = tblPrefix + "AccountMaster";
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {
                txtFromDt.Text = clsGV.Start_Date;
                txtToDt.Text = clsGV.To_date;
                setFocusControl(txtAcCode);
            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }
    protected void btnAcCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAcCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchStr != string.Empty && strTextbox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchStr;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtAcCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtUnitCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Account Unit--";
                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBSGroupCode")
            {
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Group--";
                string qry = "select [group_Code], [group_Name_E] as [Group Name] from " + tblPrefix + "BSGroupMaster  where (group_Code like '%" + txtSearchText.Text + "%' or group_Name_E like '%" + txtSearchText.Text + "%') and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

                this.showPopup(qry);
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
            //    hdnfClosePopup.Value = "Close";
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

                searchStr = txtSearchText.Text;
                strTextbox = hdnfClosePopup.Value;

                setFocusControl(btnSearch);
            }
        }
        catch
        {
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
                e.Row.Attributes["onselectstart"] = "javascript:return true;";

                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    private void showPopup(string qry)
    {
        try
        {

            this.setFocusControl(txtSearchText);

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

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion


    #region [btnGetData_Click]
    protected void btnGetData_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtAcCode.Text != string.Empty)
            {

            }
            else
            {
                setFocusControl(txtAcCode);
                return;
            }
            if (txtUnitCode.Text != string.Empty)
            {

            }
            else
            {
                setFocusControl(txtUnitCode);
                return;
            }

            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (txtAcCode.Text == string.Empty)
            {
                setFocusControl(txtAcCode);
                return;
            }
            string accode = txtAcCode.Text;
            string accode2 = txtUnitCode.Text;
            string accodeAll = accode + "," + accode2;
            pnlPopup.Style["display"] = "none";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:sp('" + accodeAll + "','" + fromdt + "','" + todt + "')", true);
        }
        catch (Exception eex)
        {

        }
    }
    #endregion

    protected void txtAcCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtAcCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtAcCode.Text);
                if (a == false)
                {
                    searchStr = txtAcCode.Text;
                    strTextbox = "txtAcCode";
                    btnAcCode_Click(this, new EventArgs());
                }
                else
                {
                    searchStr = txtAcCode.Text;
                    strTextbox = "txtAcCode";
                    string str = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAcCode.Text);
                    if (str != string.Empty)
                    {
                        lblAcCodeName.Text = str;
                        setFocusControl(txtUnitCode);
                    }
                    else
                    {
                        txtAcCode.Text = string.Empty;
                        lblAcCodeName.Text = string.Empty;
                    }
                }
            }
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
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].Width = new Unit("250px");
            e.Row.Cells[2].Width = new Unit("100px");
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        }
    }
    protected void txtUnitCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtUnitCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtUnitCode.Text);
                if (a == false)
                {
                    searchStr = txtUnitCode.Text;
                    strTextbox = "txtUnitCode";
                    btnUnitCode_Click(this, new EventArgs());
                }
                else
                {
                    searchStr = txtUnitCode.Text;
                    strTextbox = "txtUnitCode";
                    string str = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtUnitCode.Text);
                    if (str != string.Empty)
                    {
                        lblUnitName.Text = str;
                        setFocusControl(txtFromDt);
                    }
                    else
                    {
                        txtUnitCode.Text = string.Empty;
                        lblUnitName.Text = string.Empty;
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void btnUnitCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUnitCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
}